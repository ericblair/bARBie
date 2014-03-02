var cheerio = require('cheerio');
var request = require('request');
var sql = require('msnodesql');
var squel = require("squel");
var configSettings = require("./ocConfigSettings.js");

var connectionString = configSettings.GetConnectionString();
var oddsCheckerBaseUrl = configSettings.GetOddsCheckerBaseUrl();

var fixtureId = process.argv[2];
var countryId = process.argv[3];
var competitionId = process.argv[4];
var homeTeam = process.argv[5];
var awayTeam = process.argv[6];
var matchWinnerOddsUrl = process.argv[7];

main();

function main() {

    CallMatchWinnerOddsPage(matchWinnerOddsUrl);
}

    function CallMatchWinnerOddsPage(matchWinnerOddsUrl) {

        var matchWinnerOddsUrlOptions = {
            url: matchWinnerOddsUrl,
            headers: { 'User-Agent': 'Mozilla/5.0' }
        };

        request(matchWinnerOddsUrlOptions, ExtractMatchWinnerOdds);
    }

function ExtractMatchWinnerOdds(error, response, body) {

    if (!error && response.statusCode == 200) {

        $ = cheerio.load(body);

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

        writeOddsToDb(fixtureId, countryId, competitionId, homeTeam, homeOdds);
        writeOddsToDb(fixtureId, countryId, competitionId, awayTeam, awayOdds);
        writeOddsToDb(fixtureId, countryId, competitionId, 'draw', drawOdds);
    }
}

function writeOddsToDb(fixtureId, countryId, competitionId, prediction, odds) {

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
                .set("FixtureID", fixtureId)
                .set("CountryID", countryId)
                .set("CompetitionID", competitionId)
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