using ClubArcada.BusinessObjects.DataClasses;
using GemBox.Spreadsheet;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;

namespace ClubArcada.Documents
{
    public class Documents
    {
        private static byte[] CreateExcel(DateTime startDate, Tournament tournament, TournamentCashout cashout, List<CashResult> playerList, List<CashTable> tableList)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.Visible = false;

            var templateExcelFile = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Documents\\ClubArcadaCashReport.xlsx";

            Workbook wb = xlApp.Workbooks.Open(templateExcelFile, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

            Worksheet ws = (Worksheet)wb.Worksheets[1];

            ws.get_Range("A4", "A4").Cells.Value2 = tournament.Date.ToString("dd.MM.yyyy");
            ws.get_Range("H4", "H4").Cells.Value2 = "generované: " + DateTime.Now.ToString("dd.MM.yyyy - hh:mm");

            ws.get_Range("C7", "C7").Cells.Value2 = tournament.Date.ToString("dd.MM.yyyy - hh:mm");
            ws.get_Range("C8", "C8").Cells.Value2 = tournament.DateEnded.Value.ToString("dd.MM.yyyy - hh:mm");
            ws.get_Range("C9", "C9").Cells.Value2 = (tournament.DateEnded.Value - tournament.Date).TotalMinutes.ToString() + " min";
            ws.get_Range("C11", "C11").Cells.Value2 = playerList.Count.ToString();
            ws.get_Range("C12", "C12").Cells.Value2 = tableList.Count.ToString();

            ws.get_Range("G7", "G7").Cells.Value2 = cashout.APCBank;
            ws.get_Range("G8", "G8").Cells.Value2 = cashout.CGBank;
            ws.get_Range("G9", "G9").Cells.Value2 = cashout.Rake;
            ws.get_Range("G10", "G10").Cells.Value2 = cashout.Food;
            ws.get_Range("G11", "G11").Cells.Value2 = cashout.RunnerHelp;
            ws.get_Range("G12", "G12").Cells.Value2 = cashout.BonusUsed;
            ws.get_Range("G13", "G13").Cells.Value2 = cashout.Floor;
            ws.get_Range("G14", "G14").Cells.Value2 = cashout.Dealer;

            var row = 18;
            foreach (var p in playerList)
            {
                ws.get_Range("A" + row.ToString(), "A" + row.ToString()).Cells.Value2 = p.User.FullDislpayName;
                ws.get_Range("E" + row.ToString(), "E" + row.ToString()).Cells.Value2 = p.CashInTotal;
                ws.get_Range("F" + row.ToString(), "F" + row.ToString()).Cells.Value2 = p.CashOut.HasValue ? p.CashOut.Value : 0;
                ws.get_Range("G" + row.ToString(), "G" + row.ToString()).Cells.Value2 = p.CashOut.Value - p.CashInTotal;
                ws.get_Range("H" + row.ToString(), "H" + row.ToString()).Cells.Value2 = p.Duration;

                row++;
            }

            var exceldoc = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\FileSn.xlsx";
            var pdfdoc = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\FileSn.pdf";

            xlApp.ActiveWorkbook.SaveCopyAs(exceldoc);

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            SpreadsheetInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial;
            ExcelFile.Load(exceldoc).Save(pdfdoc);

            var bytes = File.ReadAllBytes(pdfdoc);

            File.Delete(exceldoc);
            File.Delete(pdfdoc);

            return bytes;
        }

        public static MemoryStream GetStream(DateTime startDate, Tournament tournament, TournamentCashout cashout, List<CashResult> playerList, List<CashTable> tableList)
        {
            var fileBytes = CreateExcel(startDate, tournament, cashout, playerList, tableList);
            var stream = fileBytes.ToMemoryStream();

            return stream;
        }
    }
}