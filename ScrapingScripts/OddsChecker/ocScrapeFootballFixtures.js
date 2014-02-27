var cheerio = require('cheerio');
var request = require('request');
var sql = require('msnodesql');
var squel = require("squel");
var configSettings = require("./ocConfigSettings.js");

var connectionString = configSettings.GetConnectionString();
var oddsCheckerBaseUrl = configSettings.GetOddsCheckerBaseUrl();

var countryId = process.argv[2];
var competitionId = process.argv[3];
var competitionUrl = process.argv[4];

main();

function main() {
    
    CallCompetitionPage(competitionUrl);
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

        var fixtureDate;

        try {
            $('.content-4 tbody tr').each(function (index, data) { ScrapeFixureDetails(index, data); });
        }
        catch (exception) {
            console.log(exception);
        }
    }
}

function ScrapeFixureDetails(index, data) {

    if ($(data).hasClass("date") && $(data).hasClass("first")) {
        fixtureDate = $(data).find('.date.first > .day').text().trim();
    }
    else if ($(data).hasClass("match-on")) {

        var fixtureTime = $(data).find("td.time").eq(0).text();
        var fixtureDateTime = ConvertDateTimeToSqlFormat(fixtureTime, fixtureDate);

        var homeTeam = $(data).find('.fixtures-bet-name').eq(0).text();
        var awayTeam = $(data).find('.fixtures-bet-name').eq(2).text();

        var fixtureOddsUrl = oddsCheckerBaseUrl + $(data).find('.button.button-f60-10').eq(0).attr('href');

        WriteFixtureToDatabase(countryId, competitionId, fixtureDateTime, homeTeam, awayTeam, fixtureOddsUrl);
    }
}

function WriteFixtureToDatabase(countryId, competitionId, fixtureDateTime, homeTeam, awayTeam, fixtureOddsUrl) {

    sql.open(connectionString, function (err, conn) {

        if (err) {
            console.log("Database connection failed!");
            console.log(err);
            return;
        }
        else {

            var matchingFixtures =
                squel.select()
                .field("ID")
                .from("OddsCheckerFootballFixtures")
                .where("CountryID = '" + countryId + "'")
                .where("CompetitionID = '" + competitionId + "'")
                .where("MatchDateTime = '" + fixtureDateTime + "'")
                .where("HomeTeam = '" + homeTeam + "'")
                .where("AwayTeam = '" + awayTeam + "'")
                .where("MatchWinnerOddsUrl = '" + fixtureOddsUrl + "'")
                .toString();

            conn.queryRaw(matchingFixtures, function (err, results) {

                if (err) {
                    console.log("WriteFixtureToDatabase: matchingFixtures: Error");
                    console.log(err);
                    return;
                }
                else {

                    if (results.rows.length != 0) {
                        return;
                    }
                    else {
                        var oddsInsertSql =
                            squel.insert()
                            .into("OddsCheckerFootballFixtures")
                            .set("CountryID", countryId)
                            .set("CompetitionID", competitionId)
                            .set("MatchDateTime", fixtureDateTime)
                            .set("HomeTeam", homeTeam)
                            .set("AwayTeam", awayTeam)
                            .set("MatchWinnerOddsUrl", fixtureOddsUrl)
                            .toString();

                        conn.queryRaw(oddsInsertSql, function (err, results) {

                            if (err) {
                                console.log("WriteFixtureToDatabase: oddsInsertSql: Error");
                                console.log(err);
                                return;
                            }
                        });
                    }
                }
            });
        }
    });
}

function ConvertDateTimeToSqlFormat(timeString, dateString) {
    // 15:00 Saturday 18th January 2014

    var hoursRaw = timeString.split(':')[0];
    var hoursInt = parseInt(hoursRaw);
    var hours = ((hoursInt < 10) ? '0' : '') + hoursInt;
    console.log('hours : ' + hours);

    var minutes = timeString.split(':')[1].substring(0, 2);
    console.log('minutes : ' + minutes);

    var seconds = '00';

    console.log('dateString : ' + dateString);
    var dayRaw = dateString.split(' ')[1];
    var dayInt = parseInt(dayRaw);
    var day = ((dayInt < 10) ? '0' : '') + dayInt;
    console.log('day : ' + day);

    var monthRaw = dateString.split(' ')[2];
    var monthInt = convertMonthNameToNumber(monthRaw);
    var month = ((monthInt < 10) ? '0' : '') + monthInt;
    console.log('month : ' + month);

    var year = dateString.split(' ')[3];
    console.log('year : ' + year);

    // '2014-12-20T14:25:10'
    var matchDateTime = year + "-" + month + "-" + day + "T" + hours + ":" + minutes + ":" + seconds;
    console.log('matchDateTime : ' + matchDateTime);
    return matchDateTime;
}

function convertMonthNameToNumber(monthName) {
    var myDate = new Date(monthName + " 1, 2000");
    var monthDigit = myDate.getMonth();
    return isNaN(monthDigit) ? 0 : (monthDigit + 1);
}