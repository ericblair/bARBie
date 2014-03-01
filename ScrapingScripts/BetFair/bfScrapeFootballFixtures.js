var cheerio = require('cheerio');
var request = require('request');
var sql = require('msnodesql');
var squel = require("squel");
var configSettings = require("./configSettings.js");

var connectionString = configSettings.GetConnectionString();
var betFairBaseUrl = configSettings.GetBetFairBaseUrl();

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

        var fixtureMarketUrls = new Array();

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

    request(fixtureMarketUrlOptions, ScrapeFixureDetails);
}

function ScrapeFixureDetails(error, response, body) {

    if (!error && response.statusCode == 200) {

        $ = cheerio.load(body);

        var matchWinnerMarketHref = $('.i13n-X-FullMarket').eq(0).attr('href');

        if (typeof matchWinnerMarketHref != 'undefined') {

            var matchWinnerMarketUrl = betFairBaseUrl + matchWinnerMarketHref;

            var homeTeam = $('.home-team').eq(0).text();
            var awayTeam = $('.away-team').eq(0).text();

            // matchDateTime format: Tue 28 Jan 7:45PM
            var dateTimeString = $('.match-status > .status').eq(0).text();
            console.log("dateTimeString: " + dateTimeString);
            var matchDateTime = convertDateTimeMinusYearToSqlFormat(dateTimeString);

            console.log("countryId: " + countryId);
            console.log("competitionId: " + competitionId);
            console.log("matchDateTime: " + matchDateTime);
            console.log("homeTeam: " + homeTeam);
            console.log("awayTeam: " + awayTeam);
            console.log("matchWinnerMarketUrl: " + matchWinnerMarketUrl);

            WriteFixtureToDatabase(countryId, competitionId, matchDateTime, homeTeam, awayTeam, matchWinnerMarketUrl);
        }
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
                .from("BetFairFootballFixtures")
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
                            .into("BetFairFootballFixtures")
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
