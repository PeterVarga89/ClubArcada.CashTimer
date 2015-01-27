using System.Collections.Generic;
using System.Linq;

namespace ClubArcada.BusinessObjects.DataClasses
{
    public partial class CashResult
    {
        public User User { get; set; }

        public List<CashIn> CashIns { get; set; }

        public eGameType GameType { get; set; }

        public string StartTimeString
        {
            get
            {
                return string.Format("Reg.: {0}", this.StartTime.Value.ToString("dd.MM.yyyy HH:mm"));
            }
        }

        public double CashInTotal
        {
            get
            {
                return CashIns != null && CashIns.Count > 0 ? CashIns.Sum(ci => ci.Amount) : 0;
            }
        }

        public int MinutesPlayed { get; set; }

        public int PlayedMinutes { get { return MinutesPlayed / 60; } }
    }
}