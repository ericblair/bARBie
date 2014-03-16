var connectionString = "Driver={SQL Server Native Client 11.0};Server=STARSHARE-AG;Database=bARBie;Trusted_Connection={Yes}";

var oddsCheckerBaseUrl = "http://www.oddschecker.com";

var userAgentString = "'User-Agent': 'Mozilla/5.0'";

module.exports = {

    GetConnectionString: function () {
        return connectionString;
    },

    GetOddsCheckerBaseUrl: function () {
        return oddsCheckerBaseUrl;
    },

    GetCurrentDateTimeInSqlFormat: function () {
        var currentDateTime = new Date();
        var currentDateTimeString = currentDateTime.getFullYear()
            + '-' + pad2(currentDateTime.getMonth() + 1)
            + '-' + pad2(currentDateTime.getDate())
            + ' ' + pad2(currentDateTime.getHours())
            + ':' + pad2(currentDateTime.getMinutes())
            + ':' + pad2(currentDateTime.getSeconds());

        return currentDateTimeString;
    }
};

function pad2(number) {
    return (number < 10 ? '0' : '') + number
}
