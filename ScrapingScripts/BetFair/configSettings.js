var connectionString = "Driver={SQL Server Native Client 11.0};Server=STARSHARE-AG;Database=bARBie;Trusted_Connection={Yes}";

var betFairBaseUrl = "http://www.betfair.com";

var userAgentString = "'User-Agent': 'Mozilla/5.0'";


var competitionUrls = new Array();
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=67646");  // World Cup 2014
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=39218");  // AFC Champions League
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=59953");  // Argentina Torneo de Verano
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=7");      // Austrian Bundesliga
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=895129"); //Azerbaijan Premier
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=845129"); // Bahrain Premier
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=89979");  // Belgian Jupiler League
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=744098"); // Chilean Primera
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=835595"); // Brazilian Baiano
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=61025");  // Brazilian Carioca
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=439090");  // Brazilian Catarinense
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=829189");  // Brazilian Cearense
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=439085");  // Brazilian Gaucho
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=483094");  // Brazilian Goiano
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=454158");  // Brazilian Mineiro
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=439106");  // Brazilian Paranaense
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=2490975");  // Brazilian Paulista A1
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=2519611");  // Brazilian Paulista A2
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=2517464");  // Brazilian Copa do Nordeste
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=961354");  // Chilean Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=744098");  // Chilean Primera
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=857992");  // Chilean Primera B 
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=3085749");  // Chilean Segunda Division
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=856134");  // Colombian Primera B 
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=2519695");  // Colombian Super Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=62815");  // Copa Libertadores
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=2079376");  // Costa Rican Primera Division
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=138629");  // Croatian Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=21");  // Czech Gambrinus Liga
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=30921");  // Danish Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=23");  // Danish Superliga
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=11");  // Dutch Jupiler League
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=71");  // Dutch Eredivisie
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=803690");  // Ecuadorian Primera A
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=801976");  // Egyptian Premier
competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=31");  // Barclays Premier League
competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=2134");  // Capital One Cup
competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=33");  // English Championship
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=41");  // English Conference North
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=39");  // Conference Premier
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=43");  // Conference South
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=30558");  // English FA Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=42886");  // English FA Trophy
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=24789");  // English Johnstone's Paint Trophy
competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=35");  // English League One
competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=37");  // English League Two
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=4381282");  // English Liverpool Senior Cup Liverpool Senior Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=2129602");  // English Professional Development League
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=820588");  // English Southern Premier
//competitionUrls.push("http://www.betfair.com/exchange/football/event?id=26952112");  // Euro 2016
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=67721");  // Finnish League Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=22358");  // French Coupe de la Ligue
competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=55");  // French Ligue 1 Orange
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=57");  // French Ligue 2 Orange
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=1081960");  // French Ligue National
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=219");  // Friendlies 
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=158146");  // German 3. Liga
competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=59");  // Bundesliga 1
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=61");  // Bundesliga 2 
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=30961");  // German Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=63");  // German Regionalliga Nord
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=840808");  // German Regionalliga West
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=26207");  // Greek Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=67");  // Greek Super League
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=3085802");  // Hong Kong Reserve Division League
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=1842928");  // Hungarian OTP Bank Liga
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=1277723");  // Northern Iceland Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=829185");  // Reykjavik Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=3745029");  // Iran Persian Gulf Cup 
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=1327435");  // Israeli Liga Alef North
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=827033");  // Israeli Liga Alef South 
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=804044");  // Israeli Liga Leumit
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=822165");  // Israeli Premier
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=841133");  // Italian Campeonato Primavera
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=1874");  // Coppa Italia
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=85");  // Italian Lega Pro 1/A
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=87");  // Lega Pro 1/B 
competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=81");  // Serie A 
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=83");  // Serie B 
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=89");  // J League
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=1979279");  // Copa Mexico
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=59870");  // Mexican Clausura
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=827754");  // Mexican Liga de Ascenso
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=2609677");  // Moroccan Division 1 
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=20351");  // NIFL Premiership
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=93");  // Norway Tippeligaen
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=97");  // Poland Ekstraklasa
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=99");  // Portugal Primeira Liga
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=9513");  // Portugal Segunda Liga
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=61351");  // Taca de Portugal
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=842258");  // Qatari Stars League
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=4905");  // Romanian Liga 1
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=101");  // Russian Premier Division
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=2315454");  // Saudi Division 1
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=853446");  // Saudi Premier
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=107");  // Scottish Championship
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=409743");  // Scottish Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=1038449");  // Scottish League Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=109");  // Scottish League One
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=111");  // Scottish League Two 
competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=105");  // Scottish Premiership
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=103");  // Serbian Super Liga
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=113");  // Slovakian Corgon Liga
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=115");  // Slovenian Prva Liga
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=867459");  // South Korean K League
competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=117");  // Primera Division
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=121");  // Segunda B/1 
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=123");  // Segunda B/2 online betting
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=125");  // Segunda B/3
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=127");  // Segunda B/4
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=119");  // Segunda Division
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=12801");  // Spanish Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=129");  // Sweden Allsvenskan
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=135");  // Swiss Challenge League
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=29051");  // Swisscom Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=133");  // Swiss Super League
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=175680");  // Turkish Division 1
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=194215");  // Turkish Super League
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=228");  // UEFA Champions League
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=2005");  // UEFA Europa League
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=139");  // Ukraine Vischya Liga
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=299924");  // Welsh Cup
//competitionUrls.push("http://www.betfair.com/exchange/football/competition?id=252549");  // Welsh Premier League

module.exports = {

    GetConnectionString : function() {
        return connectionString;
    },

    GetBetFairBaseUrl : function() {
        return betFairBaseUrl;
    },

    GetCurrentDateTimeInSqlFormat : function() {
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



