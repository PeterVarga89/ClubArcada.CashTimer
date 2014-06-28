

CREATE PROCEDURE [dbo].[uspGetCGBankByLeagueId]
@LeagueId uniqueidentifier 

AS
BEGIN


SELECT

SUM(tc.CGBank) as PrizePool

FROM
Tournaments t
LEFT JOIN TournamentDetails td on td.TournamentId = t.TournamentId
LEFT JOIN TournamentCashouts tc on tc.TournamentId = t.TournamentId
WHERE
t.LeagueId = (@leagueId)
END