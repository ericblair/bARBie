var cheerio = require('cheerio');
var request = require('request');
var sql = require('msnodesql');
var squel = require("squel");
var winston = require('winston');
var configSettings = require("./configSettings.js");

var connectionString = configSettings.GetConnectionString();
var betFairBaseUrl = configSettings.GetBetFairBaseUrl();

var fixtureId = process.argv[2];
var countryId = process.argv[3];
var competitionId = process.argv[4];
var homeTeam = process.argv[5];
var awayTeam = process.argv[6];
var matchWinnerOddsUrl = process.argv[7];

var logger = new (winston.Logger)({
    transports: [
      new (winston.transports.File)({ filename: 'bfScrapeFootballMatchWinnerOdds.log' })
    ]
});

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

        var homeOdds = ExtractOddsAndCash('home');
        var awayOdds = ExtractOddsAndCash('away');
        var drawOdds = ExtractOddsAndCash('draw');

        writeOddsToDb(fixtureId, countryId, competitionId, homeTeam, homeOdds);
        writeOddsToDb(fixtureId, countryId, competitionId, awayTeam, awayOdds);
        writeOddsToDb(fixtureId, countryId, competitionId, 'draw', drawOdds);
    }
}

function ExtractOddsAndCash(prediction) {

    var predictionIndex;
    if (prediction == 'home') {
        predictionIndex = 0;
    } else if (prediction == 'away') {
        predictionIndex = 1;
    } else if (prediction == 'draw') {
        predictionIndex = 2;
    }

    var oddsAndCashValues = new Array();

    oddsAndCashValues["BackLow"] = parseFloat($('.odds.back.selection-' + predictionIndex + '.back-cell .price').eq(0).text().replace(/ /g, '')).toFixed(2);
    oddsAndCashValues["BackLowCash"] = parseFloat($('.odds.back.selection-' + predictionIndex + '.back-cell .size').eq(0).text().replace(/ /g, '').replace(/\u00A3/g, '')).toFixed(2);
    oddsAndCashValues["BackMid"] = parseFloat($('.odds.back.selection-' + predictionIndex + '.back-cell.depth.depth-1 .price').text().replace(/ /g, '')).toFixed(2);
    oddsAndCashValues["BackMidCash"] = parseFloat($('.odds.back.selection-' + predictionIndex + '.back-cell.depth.depth-1 .size').text().replace(/ /g, '').replace(/\u00A3/g, '')).toFixed(2);
    oddsAndCashValues["BackHigh"] = parseFloat($('.odds.back.selection-' + predictionIndex + '.back-cell.depth.depth-2 .price').text().replace(/ /g, '')).toFixed(2);
    oddsAndCashValues["BackHighCash"] = parseFloat($('.odds.back.selection-' + predictionIndex + '.back-cell.depth.depth-2 .size').text().replace(/ /g, '').replace(/\u00A3/g, '')).toFixed(2);

    oddsAndCashValues["LayLow"] = parseFloat($('.odds.lay.selection-' + predictionIndex + '.lay-cell .price').eq(0).text().replace(/ /g, '')).toFixed(2);
    oddsAndCashValues["LayLowCash"] = parseFloat($('.odds.lay.selection-' + predictionIndex + '.lay-cell .size').eq(0).text().replace(/ /g, '').replace(/\u00A3/g, '')).toFixed(2);
    oddsAndCashValues["LayMid"] = parseFloat($('.odds.lay.selection-' + predictionIndex + '.lay-cell.depth.depth-1 .price').text().replace(/ /g, '')).toFixed(2);
    oddsAndCashValues["LayMidCash"] = parseFloat($('.odds.lay.selection-' + predictionIndex + '.lay-cell.depth.depth-1 .size').text().replace(/ /g, '').replace(/\u00A3/g, '')).toFixed(2);
    oddsAndCashValues["LayHigh"] = parseFloat($('.odds.lay.selection-' + predictionIndex + '.lay-cell.depth.depth-2 .price').text().replace(/ /g, '')).toFixed(2);
    oddsAndCashValues["LayHighCash"] = parseFloat($('.odds.lay.selection-' + predictionIndex + '.lay-cell.depth.depth-2 .size').text().replace(/ /g, '').replace(/\u00A3/g, '')).toFixed(2);

    for (var indexString in oddsAndCashValues) {

        if (isNaN(oddsAndCashValues[indexString])) {

            oddsAndCashValues[indexString] = null;
        }
    }

    return oddsAndCashValues;
}

function writeOddsToDb(fixtureId, countryId, competitionId, prediction, odds) {

    sql.open(connectionString, function (err, conn) {
        if (err) {
            logger.error('Database connection failed:', err);
            return;
        }
        else {

            var oddsInsertSql =
                squel.insert()
                .into("BetFairFootballOdds")
                .set("FixtureID", fixtureId)
                .set("CountryID", countryId)
                .set("CompetitionID", competitionId)
                .set("Prediction", prediction)
                .set("BackLow", odds["BackLow"])
                .set("BackLowCash", odds["BackLowCash"])
                .set("BackMid", odds["BackMid"])
                .set("BackMidCash", odds["BackMidCash"])
                .set("BackHigh", odds["BackHigh"])
                .set("BackHighCash", odds["BackHighCash"])
                .set("LayLow", odds["LayLow"])
                .set("LayLowCash", odds["LayLowCash"])
                .set("LayMid", odds["LayMid"])
                .set("LayMidCash", odds["LayMidCash"])
                .set("LayHigh", odds["LayHigh"])
                .set("LayHighCash", odds["LayHighCash"])
                .set("Updated", getCurrentDateTimeInSqlFormat())
                .toString();

            conn.queryRaw(oddsInsertSql, function (err, results) {

                if (err) {
                    logger.error('oddsInsertSql', oddsInsertSql);
                    return;
                }
            });
        }
    });
}

function getCurrentDateTimeInSqlFormat() {

    var currentDateTime = new Date();
    var currentDateTimeString = currentDateTime.getFullYear()
        + '-' + pad2(currentDateTime.getMonth() + 1)
        + '-' + pad2(currentDateTime.getDate())
        + ' ' + pad2(currentDateTime.getHours())
        + ':' + pad2(currentDateTime.getMinutes())
        + ':' + pad2(currentDateTime.getSeconds());

    return currentDateTimeString;
}

function pad2(number) {
    return (number < 10 ? '0' : '') + number
}