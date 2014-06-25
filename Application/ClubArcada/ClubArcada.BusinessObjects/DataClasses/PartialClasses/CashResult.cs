using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubArcada.BusinessObjects.DataClasses
{
    public partial class CashResult
    {
        public User User { get; set; }
        public List<CashIn> CashIns { get; set; }

        public string StartTimeString
        {
            get
            {
                return string.Format("Start: {0}", this.StartTime.Value.ToString("dd.MM.yyyy hh:mm"));
            }
        }

        public double CashInTotal
        {
            get
            {
                return CashIns != null && CashIns.Count > 0 ? CashIns.Sum(ci => ci.Amount) : 0;
            }
        }

        public int CalculatedPoints
        {
            get
            {
                if(this.EndTime.HasValue)
                {
                    return (EndTime.Value - StartTime.Value).Minutes;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
