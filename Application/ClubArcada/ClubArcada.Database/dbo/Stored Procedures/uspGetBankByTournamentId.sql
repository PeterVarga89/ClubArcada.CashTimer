
CREATE PROCEDURE [dbo].[uspGetBankByTournamentId]
	@tournamentId uniqueidentifier 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
	t.Dotation +
	t.Rake +
	t.PrizePool +
	t.Place_01 +
	t.Place_02 +
	t.Place_03 +
	t.Place_04 +
	t.Place_05 +

	t.Place_06 +
	t.Place_07 +
	t.Place_08 +
	t.Place_09 +
	t.Place_10 as Bank
	

	FROM
	TournamentCashouts t
	WHERE t.TournamentId = (@tournamentId)
END