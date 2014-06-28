
CREATE PROCEDURE [dbo].[uspGetTournamentsStats]
	@LeagueId uniqueidentifier
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT 
		datename(dw,t.Date),
		t.Name,
		
		a.PlayerCount * td.BuyInPrize + a.ReBuys * td.ReBuyPrize + a.AddOns * td.AddOnPrize AS Bank,
		td.GTD,
		a.PlayerCount,
		a.ReBuys,
		a.AddOns
	 FROM
	(
	SELECT DISTINCT
		t.TournamentId,	
		COUNT(tr.TournamentId) AS PlayerCount,
		SUM(tr.ReBuyCount) AS ReBuys,
		SUM(tr.AddOnCount) AS AddOns
		
	FROM
		Tournaments t
	LEFT JOIN TournamentResults tr on tr.TournamentId = t.TournamentId
	WHERE t.LeagueId = (@LeagueId) AND t.DateDeleted IS NULL AND t.GameType != 'C' AND datename(dw,t.Date) = 'friday'
	GROUP BY t.TournamentId
	)
	a
	INNER JOIN Tournaments t ON t.TournamentId = a.TournamentId
	INNER JOIN TournamentDetails td ON td.TournamentId = a.TournamentId

	ORDER BY t.Date


END