
CREATE PROCEDURE [dbo].[uspGetLeagueLadder] @Count INT
AS
    SET nocount ON;

    SELECT TOP (@Count) a.userid,
                        a.points,
                        a.playcount,
                        u.nickname
    FROM   (SELECT DISTINCT tr.userid,
                            Sum(tr.points)   AS Points,
                            Count(tr.userid) AS PlayCount
            FROM   tournamentresults tr
                   INNER JOIN tournaments t
                           ON t.tournamentid = tr.tournamentid
                   INNER JOIN tournamentdetails td
                           ON td.tournamentid = tr.tournamentid
                   INNER JOIN leagues l
                           ON l.leagueid = t.leagueid
            WHERE  td.isleague = 1
                   AND l.isactive = 1
            GROUP  BY tr.userid) a
           INNER JOIN users u
                   ON u.userid = a.userid
				   WHERE u.IsPersonal = 0
    ORDER  BY a.points DESC,
              a.playcount DESC,
              u.nickname