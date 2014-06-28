CREATE PROCEDURE [dbo].[uspGetTournaments]
	@LeagueId uniqueidentifier,
	@onlyFuture bit
AS
BEGIN
	SET NOCOUNT OFF;

	SELECT 
		a.TournamentId,
		t.Name,
		a.PlayerCount,
		a.PlayerCount * td.BuyInPrize + a.ReBuys * td.ReBuyPrize + a.AddOns * td.AddOnPrize AS Bank,
		t.GameType,
		t.Date,
		t.Description,
		td.IsFullPointed,
		td.IsLeague,

		td.BuyInPrize,
		td.ReBuyPrize,
		td.AddOnPrize,

		td.BuyInStack,
		td.ReBuyStack,
		td.AddOnStack,
		td.BonusStack,

		td.GTD,
		td.ReEntryCount,
		td.ReBuyCount,
		td.IsFood,

		t.IsHidden,
		t.DateDeleted,
		t.DeletedByUserId,
		t.DateCreated,
		t.CreatedByUserId,
		a.AddOns,
		a.PlayerCount,
		a.ReBuys
	
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
	WHERE t.LeagueId = (@LeagueId) AND ((t.Date >= getdate() AND (@onlyFuture) = 1) OR ((@onlyFuture) = 0)) AND t.DateDeleted IS NULL
	GROUP BY t.TournamentId
	)
	a
	INNER JOIN Tournaments t ON t.TournamentId = a.TournamentId
	INNER JOIN TournamentDetails td ON td.TournamentId = a.TournamentId

	ORDER BY t.Date


END