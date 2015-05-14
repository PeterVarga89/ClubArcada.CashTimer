using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubArcada.BusinessObjects.DataClasses
{
    public partial class Transaction
    {
        public eTransactionType Type
        {
            get
            {
                return (eTransactionType)this.TransactionType;
            }
            set
            {
                this.TransactionType = (int)value;
            }
        }
    }
}
