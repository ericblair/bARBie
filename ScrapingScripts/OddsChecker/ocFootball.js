var cheerio = require('cheerio');
var request = require('request');
var sql = require('msnodesql');
var squel = require("squel");
var configSettings = require("./ocConfigSettings.js");

var connectionString = configSettings.GetConnectionString();
var oddsCheckerBaseUrl = configSettings.GetOddsCheckerBaseUrl();
var competitionUrls = configSettings.GetCompetitionUrls();

main();

function main() {

    competitionUrls.forEach(function (competitionUrl) {
        CallCompetitionPage(competitionUrl);
    });
}

function CallCompetitionPage(competitionUrl) {

    var competitionUrlOptions = {
        url: competitionUrl,
        headers: { 'User-Agent': 'Mozilla/5.0' }
    };

    request(competitionUrlOptions, ExtractFixtureMarketUrls);
}

function ExtractFixtureMarketUrls(error, response, body) {

    if (!error && response.statusCode == 200) {

        $ = cheerio.load(body);

        var fixtureMarketUrls = new Array();

        $('.content-4 tbody tr .button.in-play-solo').each(function () {
            fixtureMarketUrls.push($(this).attr('href'));
        });

        $('.content-4 tbody tr .button.button-f60-10').each(function () {
            fixtureMarketUrls.push($(this).attr('href'));
        });

        fixtureMarketUrls.forEach(function (fixtureMarketUrl) {
            CallMatchWinnerMarketPage(oddsCheckerBaseUrl + fixtureMarketUrl);
        });
    }
}

function CallMatchWinnerMarketPage(matchWinnerMarketUrl) {

    var matchOddsWinnersPageUrlOptions = {
        url: matchWinnerMarketUrl,
        headers: { 'User-Agent': 'Mozilla/5.0' }
    };

    request(matchOddsWinnersPageUrlOptions, ExtractOddsFromWinnersMarketPage);
}

function ExtractOddsFromWinnersMarketPage(error, response, body) {

    if (!error && response.statusCode == 200) {

        $ = cheerio.load(body);

        // matchDateTime format: 15:00 Saturday 18th January
        var matchDateTime = convertDateTimeMinusYearToSqlFormat($('.date').text());

        var homeTeam = $('head title').text().split(' v ')[0]
        var awayTeam = $('head title').text().split(' v ')[1].split(' Winner')[0];
        var fixture = homeTeam + ' v ' + awayTeam;

        var oddsTable = $('.eventTable');

        var homeOdds;
        var awayOdds;
        var drawOdds;

        for (var i = 0; i < 3; i++) {

            var teamId = getTeamId(i);
            var teamName = getTeamName(oddsTable, '.selTxt', i);

            if (teamName == homeTeam) {
                homeOdds = getResultOdds(teamId);
            } else if (teamName == awayTeam) {
                awayOdds = getResultOdds(teamId);
            } else {
                drawOdds = getResultOdds(teamId);
            }
        }

        writeOddsToDb(matchDateTime, fixture, homeTeam, awayTeam, homeTeam, homeOdds);
        writeOddsToDb(matchDateTime, fixture, homeTeam, awayTeam, awayTeam, awayOdds);
        writeOddsToDb(matchDateTime, fixture, homeTeam, awayTeam, 'draw', drawOdds);
    }
}

function writeOddsToDb(matchDateTime, fixture, homeTeam, awayTeam, prediction, odds) {

    if (typeof odds == 'undefined') {
        // No odds to write to database
        return;
    }

    sql.open(connectionString, function (err, conn) {

        if (err) {
            console.log("Database connection failed!");
            console.log(err);
            return;
        }
        else {

            var oddsInsertSql =
                squel.insert()
                .into("OddsCheckerFootballOdds")
                .set("MatchDateTime", matchDateTime)
                .set("Fixture", fixture)
                .set("HomeTeam", homeTeam)
                .set("AwayTeam", awayTeam)
                .set("Prediction", prediction)
                .set("Bet365", odds["B3"])
                .set("SkyBet", odds["SK"])
                .set("ToteSport", odds["BX"])
                .set("BoyleSports", odds["BY"])
                .set("BetFred", odds["FR"])
                .set("SportingBet", odds["SO"])
                .set("BetVictor", odds["VC"])
                .set("PaddyPower", odds["PP"])
                .set("StanJames", odds["SJ"])
                .set("[888Sport]", odds["EE"])
                .set("Ladbrokes", odds["LD"])
                .set("Coral", odds["CE"])
                .set("WilliamHill", odds["WH"])
                .set("Winner", odds["WN"])
                .set("SpreadEx", odds["SX"])
                .set("BetWay", odds["WA"])
                .set("Bwin", odds["BW"])
                .set("UniBet", odds["UN"])
                .set("YouWin", odds["YW"])
                .set("[32RedBet]", odds["RD"])
                .set("BetFair", odds["BF"])
                .set("BetDaq", odds["BD"])
                .set("Updated", configSettings.GetCurrentDateTimeInSqlFormat())
                .toString();

            conn.queryRaw(oddsInsertSql, function (err, results) {

                if (err) {
                    console.log("writeOddsToDb: Error");
                    console.log(err);
                    return;
                } 
            });
        }

    });
}

function getTeamId(index) {

    var teamId = $('.eventTableRow.bgc').eq(index).attr('data-participant-id');
    return teamId;
}

var bookieKeys = new Array();
bookieKeys[0] = "B3";
bookieKeys[1] = "SK";
bookieKeys[2] = "BX";
bookieKeys[3] = "BY";
bookieKeys[4] = "FR";
bookieKeys[5] = "SO";
bookieKeys[6] = "VC";
bookieKeys[7] = "PP";
bookieKeys[8] = "SJ";
bookieKeys[9] = "EE";
bookieKeys[10] = "LD";
bookieKeys[11] = "CE";
bookieKeys[12] = "WH";
bookieKeys[13] = "WN";
bookieKeys[14] = "SX";
bookieKeys[15] = "WA";
bookieKeys[16] = "BW";
bookieKeys[17] = "UN";
bookieKeys[18] = "YW";
bookieKeys[19] = "RD";
bookieKeys[20] = "BF";
bookieKeys[21] = "BD";

function getResultOdds(resultId) {

    var resultOdds = {};

    for (var i = 0; i < bookieKeys.length; i++) {

        var oddsId = resultId + '_' + bookieKeys[i];
        var odds = convertOddsFractionalToDecimal($('#' + oddsId).text());

        if (!(isNaN(odds))) {
            resultOdds[bookieKeys[i]] = parseFloat(odds).toFixed(2);
        } else {
            resultOdds[bookieKeys[i]] = null;
        }
    }

    return resultOdds;
}

function convertDateTimeMinusYearToSqlFormat(matchDateTimeUnformatted) {
    // Example string parsed from web site: 15:00 Saturday 18th January

    var hoursRaw = matchDateTimeUnformatted.split(':')[0];
    var hoursInt = parseInt(hoursRaw);
    var hours = ((hoursInt < 10) ? '0' : '') + hoursInt;

    var minutes = matchDateTimeUnformatted.split(':')[1].substring(0, 2);

    var seconds = '00';

    var dayRaw = matchDateTimeUnformatted.split(' ')[2];
    var dayInt = parseInt(dayRaw);
    var day = ((dayInt < 10) ? '0' : '') + dayInt;

    var monthRaw = matchDateTimeUnformatted.split(' ')[3];
    var monthInt = convertMonthNameToNumber(monthRaw);
    var month = ((monthInt < 10) ? '0' : '') + monthInt;

    // If current month is December and the dateTime string contains
    // the word 'January' then increment the current year,
    // otherwise just return the current year
    var year;
    var currentDate = new Date();
    if (currentDate.getMonth() == 11) {
        var x = matchDateTimeUnformatted.indexOf('January');
        if (x > 0) {
            year = currentDate.getFullYear() + 1;
        } else {
            year = currentDate.getFullYear();
        }
    } else {
        year = currentDate.getFullYear();
    }

    // '2014-12-20T14:25:10'
    var matchDateTime = year + "-" + month + "-" + day + "T" + hours + ":" + minutes + ":" + seconds;
    return matchDateTime;
}

function convertMonthNameToNumber(monthName) {
    var myDate = new Date(monthName + " 1, 2000");
    var monthDigit = myDate.getMonth();
    return isNaN(monthDigit) ? 0 : (monthDigit + 1);
}

function convertOddsFractionalToDecimal(fractionalOdds) {
    // fractionalOdds string format: (5) or (23/10)

    var numerator = parseInt(fractionalOdds.replace('(', ''));
    var denominator = parseInt(fractionalOdds.replace(')', '').split('/')[1]);

    if (isNaN(denominator)) {
        numerator++;
        var decimalOdds = numerator;
    } else {
        var decimalOdds = ((numerator / denominator) + 1).toFixed(2);
    }

    return decimalOdds;
}

function getTeamName(element, cssClass, index) {
    var team = $(element).find(cssClass).eq(index).text();

    return team
}