-- This query matches the fixture and match datetime between BetFair and OddsChecker tables. 
-- I've played with the difference parameter a bit and 5 returns the same results as 100 & 1000.

-- NOTE: I've altered the BetFair scraper so that the matchDateTime value might be slightly 
-- before the actual datetime of the match

IF OBJECT_ID('tempdb..#matches') IS NOT NULL
    DROP TABLE #matches

create table #matches 
(
	Fixture nvarchar(200),
	MatchDateTime dateTime
)

insert into #matches
select 
	distinct oc.Fixture, oc.MatchDateTime
from OddsCheckerFootballOdds as oc
join BetFairFootballOdds as bf 
	on bf.MatchDateTime BETWEEN DATEADD(hour, -1, oc.MatchDateTime) and DATEADD(hour, +3, oc.MatchDateTime)
where DateDiff(hh,oc.MatchDateTime,GetDate()) < 6
and DIFFERENCE(oc.Fixture, bf.Fixture) < 5
order by oc.MatchDateTime, oc.Fixture

insert into FootballMatches
select m.Fixture, m.MatchDateTime
from #matches m
left outer join FootballMatches fm
on m.Fixture = fm.Fixture 
and m.MatchDateTime = fm.MatchDateTime
where fm.Fixture is null
order by m.MatchDateTime, m.Fixture

--select distinct Fixture, MatchDateTime
--from OddsCheckerFootballOdds
--where DateDiff(hh,MatchDateTime,GetDate()) < 6
--order by MatchDateTime, Fixture

--SELECT distinct REPLACE(REPLACE(Fixture, CHAR(13), ''), CHAR(10), ''), MatchDateTime
--from BetFairFootballOdds
--where DateDiff(hh,MatchDateTime,GetDate()) < 6

--DELETE OddsCheckerFootballOdds
--DELETE BetFairFootballOdds
-- DELETE FootballMatches

--select * from FootballMatches