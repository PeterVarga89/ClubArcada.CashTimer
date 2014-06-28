
CREATE PROCEDURE [dbo].[uspGetPrizePoolByLeagueId]
@LeagueId uniqueidentifier 

AS
BEGIN


SELECT

SUM(tc.PrizePool) as PrizePool

FROM
Tournaments t
LEFT JOIN TournamentDetails td on td.TournamentId = t.TournamentId
LEFT JOIN TournamentCashouts tc on tc.TournamentId = t.TournamentId
WHERE
t.LeagueId = (@leagueId) AND td.IsLeague = 1
END