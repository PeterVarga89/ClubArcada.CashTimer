
Create PROCEDURE [dbo].[uspAPCBankByLeagueId]

AS
BEGIN


SELECT

SUM(tc.APCBank) as PrizePool

FROM
Tournaments t
LEFT JOIN TournamentDetails td on td.TournamentId = t.TournamentId
LEFT JOIN TournamentCashouts tc on tc.TournamentId = t.TournamentId

END