
CREATE PROCEDURE [dbo].[uspGetCashLadder] @Count INT
AS
    SET nocount ON;

    SELECT TOP (@Count) a.userid,
                        a.points,
                        a.playcount,
                        u.nickname
    FROM   (SELECT DISTINCT cr.UserId,
                            Sum(cr.Duration) AS Points,
                            Count(cr.UserId) AS PlayCount
            FROM   CashResults cr
                   INNER JOIN tournaments t ON t.tournamentid = cr.tournamentid
                   INNER JOIN leagues l ON l.leagueid = t.leagueid
            WHERE l.isactive = 1
            GROUP  BY cr.UserId) a
           INNER JOIN users u ON u.UserId = a.UserId
		   WHERE u.IsPersonal = 0
    ORDER  BY a.points DESC,
              a.playcount DESC,
              u.nickname