using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubArcada.BusinessObjects.DataClasses
{
    [Serializable]
    public partial class User
    {
        public string FullDislpayName
        {
            get
            {
                return string.Format("{0} - {1} {2}", this.NickName, this.FirstName, this.LastName);
            }
        }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}",this.FirstName, this.LastName);
            }
            set { }
        }
    }
}
