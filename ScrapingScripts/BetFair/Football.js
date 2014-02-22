var cheerio = require('cheerio');
var request = require('request');
var sql = require('msnodesql');
var squel = require("squel");
var configSettings = require("./configSettings.js");

var connectionString = configSettings.GetConnectionString();
var betFairBaseUrl = configSettings.GetBetFairBaseUrl();
var competitionUrls = configSettings.GetCompetitionUrls();

main();

function main() {

    competitionUrls.forEach(function (competitionUrl)
    {
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

        var fixtureMarketUrls= new Array();

        $('.event-link').each(function () {
            fixtureMarketUrls.push($(this).attr('href'));
        });

        fixtureMarketUrls.forEach(function (fixtureMarketUrl) {
            
            CallFixtureMarketUrl(betFairBaseUrl + fixtureMarketUrl);
        });
    }
}

function CallFixtureMarketUrl(fixtureMarketUrl) {

    var fixtureMarketUrlOptions = {
        url: fixtureMarketUrl,
        headers: { 'User-Agent': 'Mozilla/5.0' }
    };

    request(fixtureMarketUrlOptions, ExtractWinnerMarketUrl);
}

function ExtractWinnerMarketUrl(error, response, body) {

    if (!error && response.statusCode == 200) {

        $ = cheerio.load(body);

        var matchWinnerMarketHref = $('.i13n-X-FullMarket').eq(0).attr('href');

        if (typeof matchWinnerMarketHref != 'undefined') {

            var matchWinnerMarketUrl = betFairBaseUrl + matchWinnerMarketHref;

            CallMatchWinnerMarketPage(matchWinnerMarketUrl);
        }
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

        var homeTeam = $('.sel-name').eq(0).text().replace(/\n/g, "").replace(/\s+/g, "");
        var awayTeam = $('.sel-name').eq(1).text().replace(/\n/g, "").replace(/\s+/g, "");
        var fixture = homeTeam + ' v ' + awayTeam;

        var homeOdds = ExtractOddsAndCash('home');
        var awayOdds = ExtractOddsAndCash('away');
        var drawOdds = ExtractOddsAndCash('draw');

        // matchDateTime format: Tue 28 Jan 7:45PM
        var matchDateTime;
        try {
            matchDateTime = convertDateTimeMinusYearToSqlFormat($('.status').text());

            writeOddsToDb(matchDateTime, fixture, homeTeam, awayTeam, homeTeam, homeOdds);
            writeOddsToDb(matchDateTime, fixture, homeTeam, awayTeam, awayTeam, awayOdds);
            writeOddsToDb(matchDateTime, fixture, homeTeam, awayTeam, 'draw', drawOdds);
        }
        catch (error) {
            console.log(error);
        }
    }
}

function writeOddsToDb(matchDateTime, fixture, homeTeam, awayTeam, prediction, odds) {

    sql.open(connectionString, function (err, conn) {
        if (err) {
            console.log("Database connection failed!");
            console.log(err);
            return;
        }
        else {
            
            var oddsInsertSql =
                squel.insert()
                .into("BetFairFootballOdds")
                .set("MatchDateTime", matchDateTime)
                .set("Fixture", fixture)
                .set("HomeTeam", homeTeam)
                .set("AwayTeam", awayTeam)
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
                    console.log("oddsInsertSql: " + oddsInsertSql);
                    console.log(err);
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

function getCurrentDateTimeInSqlFormatWithMins(minutesToAdd) {

    var currentDateTime = new Date();
    var currentDateTimeString = currentDateTime.getFullYear()
        + '-' + pad2(currentDateTime.getMonth() + 1)
        + '-' + pad2(currentDateTime.getDate())
        + ' ' + pad2(currentDateTime.getHours())
        + ':' + pad2(currentDateTime.setMinutes(currentDateTime.getMinutes() + minutesToAdd))
        + ':' + pad2(currentDateTime.getSeconds());

    return currentDateTimeString;
}

function pad2(number) {
    return (number < 10 ? '0' : '') + number
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

function convertDateTimeMinusYearToSqlFormat(matchDateTimeUnformatted) {
    // Example string parsed from web site: Tue 28 Jan 7:45PM
    // alt example: Starting in 2' 

    // if the string contains 'Starting' then just insert the current datetime
    if (matchDateTimeUnformatted.indexOf("Starting") != -1) {

        var matchDateTime = getCurrentDateTimeInSqlFormat();
        return matchDateTime;
    }
    // if the string contains 'Elapsed' then just insert the current datetime
    else if (matchDateTimeUnformatted.indexOf("Elapsed") != -1) {
        var matchDateTime = getCurrentDateTimeInSqlFormat();
        return matchDateTime;
    }
    // if the string contains 'HT' then just insert the current datetime
    else if (matchDateTimeUnformatted.indexOf("HT") != -1) {
        var matchDateTime = getCurrentDateTimeInSqlFormat();
        return matchDateTime;
    }

    var dayRaw = matchDateTimeUnformatted.split(' ')[1];
    var dayInt = parseInt(dayRaw);
    var day = ((dayInt < 10) ? '0' : '') + dayInt;

    var monthRaw = matchDateTimeUnformatted.split(' ')[2];
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

    var hourRaw = matchDateTimeUnformatted.split(' ')[3].split(':')[0];
    var hourInt = parseInt(hourRaw);
    
    var minuteRaw = matchDateTimeUnformatted.split(' ')[3].split(':')[1];
    if (typeof minuteRaw != 'undefined') {
        var minutesInt = parseInt(minuteRaw);
        var minutes = ((minutesInt < 10) ? '0' : '') + minutesInt;
        var phaseOfDay = matchDateTimeUnformatted.split(' ')[3].split(':')[1].substring(2, 4);

    } else {
        var minutes = '00';
        var phaseOfDay = matchDateTimeUnformatted.split(' ')[3].split(':')[0].substring(1, 3);
    }

    if (phaseOfDay == 'PM' && hourInt != 12) {
        hourInt = hourInt + 12;
    }
    var hours = ((hourInt < 10) ? '0' : '') + hourInt;

    var seconds = '00';

    // '2014-12-20T14:25:10'
    var matchDateTime = year + "-" + month + "-" + day + "T" + hours + ":" + minutes + ":" + seconds;
    return matchDateTime;
}

function convertMonthNameToNumber(monthAbrv) {

    var monthName;
    switch (monthAbrv) {
        case "Jan":
            monthName = "January";
            break;
        case "Feb":
            monthName = "February";
            break;
        case "Mar":
            monthName = "March";
            break;
        case "Apr":
            monthName = "April";
            break;
        case "May":
            monthName = "May";
            break;
        case "Jun":
            monthName = "June";
            break;
        case "Jul":
            monthName = "July";
            break;
        case "Aug":
            monthName = "August";
            break;
        case "Sep":
            monthName = "September";
            break;
        case "Oct":
            monthName = "October";
            break;
        case "Nov":
            monthName = "November";
            break;
        case "Dec":
            monthName = "December";
            break;
    }

    var myDate = new Date(monthName + " 1, 2000");
    var monthDigit = myDate.getMonth();
    return isNaN(monthDigit) ? 0 : (monthDigit + 1);
}