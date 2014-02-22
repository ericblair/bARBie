var connectionString = "Driver={SQL Server Native Client 11.0};Server=SAM\\SQLEXPRESS;Database=bARBie;Trusted_Connection={Yes}";

var oddsCheckerBaseUrl = "http://www.oddschecker.com";

var userAgentString = "'User-Agent': 'Mozilla/5.0'";

var competitionUrls = new Array();
competitionUrls.push("http://www.oddschecker.com/football/english/premier-league");  // ENGLISH PREMIER LEAGUE
competitionUrls.push("http://www.oddschecker.com/football/english/championship");  // SKY BET CHAMPIONSHIP
competitionUrls.push("http://www.oddschecker.com/football/english/league-1");  // SKY BET LEAGUE 1
competitionUrls.push("http://www.oddschecker.com/football/english/league-2");  // SKY BET LEAGUE 2 
//competitionUrls.push("http://www.oddschecker.com/football/english/fa-cup");  // ENGLISH FA CUP
//competitionUrls.push("http://www.oddschecker.com/football/english/league-cup");  // CAPITAL ONE CUP
//competitionUrls.push("http://www.oddschecker.com/football/english/football-league-trophy");  // ENGLISH FOOTBALL LEAGUE TROPHY
//competitionUrls.push("http://www.oddschecker.com/football/english/non-league/conference-premier");  // THE SKRILL PREMIER
//competitionUrls.push("http://www.oddschecker.com/football/english/non-league/conference-north");  // THE SKRILL NORTH
//competitionUrls.push("http://www.oddschecker.com/football/english/non-league/conference-south");  // THE SKRILL SOUTH
//competitionUrls.push("http://www.oddschecker.com/football/english/non-league/southern-premier");  // SOUTHERN LEAGUE
//competitionUrls.push("http://www.oddschecker.com/football/english/non-league/northern-premier");  // NORTHERN LEAGUE
//competitionUrls.push("http://www.oddschecker.com/football/english/non-league/fa-trophy");  // FA TROPHY
//competitionUrls.push("http://www.oddschecker.com/football/english/non-league/reserves");  // ENGLISH RESERVES
competitionUrls.push("http://www.oddschecker.com/football/scottish/premiership");  // SCOTTISH PREMIERSHIP
//competitionUrls.push("http://www.oddschecker.com/football/scottish/championship");  // SCOTTISH CHAMPIONSHIP
//competitionUrls.push("http://www.oddschecker.com/football/scottish/league-1");  // SCOTTISH LEAGUE 1 
//competitionUrls.push("http://www.oddschecker.com/football/scottish/league-2");  // SCOTTISH LEAGUE 2
//competitionUrls.push("http://www.oddschecker.com/football/scottish/fa-cup");  // SCOTTISH FA CUP
//competitionUrls.push("http://www.oddschecker.com/football/scottish/league-cup");  // SCOTTISH LEAGUE CUP
competitionUrls.push("http://www.oddschecker.com/football/france/ligue-1");  // FRANCE LIGUE 1
//competitionUrls.push("http://www.oddschecker.com/football/france/ligue-2");  // FRANCE LIGUE 2
//competitionUrls.push("http://www.oddschecker.com/football/france/national");  // FRANCE NATIONAL
//competitionUrls.push("http://www.oddschecker.com/football/france/coupe-de-france");  // FRANCE COUPE DE FRANCE
//competitionUrls.push("http://www.oddschecker.com/football/france/coupe-de-la-ligue");  // FRANCE COUPE DE LA LIGUE
//competitionUrls.push("http://www.oddschecker.com/football/france/super-cup");  // FRANCE SUPER CUP
competitionUrls.push("http://www.oddschecker.com/football/germany/bundesliga");  // GERMANY BUNDESLIGA
//competitionUrls.push("http://www.oddschecker.com/football/germany/bundesliga-2");  // GERMANY BUNDESLIGA 2
//competitionUrls.push("http://www.oddschecker.com/football/germany/3rd-liga");  // GERMANY 3RD LIGA
//competitionUrls.push("http://www.oddschecker.com/football/germany/dfb-pokal");  // GERMANY DFB POKAL
//competitionUrls.push("http://www.oddschecker.com/football/germany/liga-pokal");  // GERMANY LIGA POKAL
//competitionUrls.push("http://www.oddschecker.com/football/germany/super-cup");  // GERMANY SUPER CUP
//competitionUrls.push("http://www.oddschecker.com/football/germany/regionalliga");  // GERMANY REGIONALLIGA
competitionUrls.push("http://www.oddschecker.com/football/italy/serie-a");  // ITALY SERIE A 
//competitionUrls.push("http://www.oddschecker.com/football/italy/serie-b");  // ITALY SERIE B
//competitionUrls.push("http://www.oddschecker.com/football/italy/coppa-italia");  // ITALY COPPA ITALIA
//competitionUrls.push("http://www.oddschecker.com/football/italy/super-cup");  // ITALY SUPER CUP
//competitionUrls.push("http://www.oddschecker.com/football/italy/lega-pro");  // ITALY LEGA PRO
//competitionUrls.push("http://www.oddschecker.com/football/italy/lega-pro-coppa");  // ITALY LEGA PRO COPPA
competitionUrls.push("http://www.oddschecker.com/football/spain/la-liga-primera");  // SPAIN LA LIGA PRIMERA
//competitionUrls.push("http://www.oddschecker.com/football/spain/la-liga-segunda");  // SPAIN LA LIGA SEGUNDA
//competitionUrls.push("http://www.oddschecker.com/football/spain/copa-del-rey");  // SPAIN COPA DEL REY
//competitionUrls.push("http://www.oddschecker.com/football/spain/la-liga-segunda-b");  // SPAIN LA LIGA SEGUNDA B
//competitionUrls.push("http://www.oddschecker.com/football/spain/la-liga-tercera");  // SPAIN LA LIGA TERCERA
//competitionUrls.push("http://www.oddschecker.com/football/spain/super-cup");  // SPAIN SUPER CUP
//competitionUrls.push("http://www.oddschecker.com/football/spain/copa-federacion");  // SPAIN COPA FEDERACION
//competitionUrls.push("http://www.oddschecker.com/football/other/albania/super-league");  // ALBANIA SUPER LEAGUE
//competitionUrls.push("http://www.oddschecker.com/football/other/andorra/primero-divisio");  // ANDORRA PRIMERO DIVISIO
//competitionUrls.push("http://www.oddschecker.com/football/other/armenia/premier-league");  // ARMENIA PREMIER LEAGUE
//competitionUrls.push("http://www.oddschecker.com/football/other/austria/bundesliga");  // AUSTRIA BUNDESLIGA
//competitionUrls.push("http://www.oddschecker.com/football/other/austria/1-liga");  // AUSTRIA 1 LIGA
//competitionUrls.push("http://www.oddschecker.com/football/other/austria/cup");  // AUSTRIA CUP
//competitionUrls.push("http://www.oddschecker.com/football/other/azerbaijan/premier-league");  // AZERBAIJAN PREMIER LEAGUE
//competitionUrls.push("http://www.oddschecker.com/football/other/belarus/premier-league");  // BELARUS PREMIER LEAGUE
//competitionUrls.push("http://www.oddschecker.com/football/other/belarus/cup");  // BELARUS CUP
//competitionUrls.push("http://www.oddschecker.com/football/other/belgium/jupiler-pro-league");  // BELGIUM JUPILER PRO LEAGUE
//competitionUrls.push("http://www.oddschecker.com/football/other/belgium/division-2");  // BELGIUM DIVISION 2
//competitionUrls.push("http://www.oddschecker.com/football/other/belgium/cup");  // BELGIUM CUP
//competitionUrls.push("");  // 

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
    },

    GetCompetitionUrls: function () {
        return competitionUrls;
    }
};

function pad2(number) {
    return (number < 10 ? '0' : '') + number
}
