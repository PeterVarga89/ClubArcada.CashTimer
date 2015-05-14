using System;

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
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
            set { }
        }

        public eAutoReturnState AutoReturnState
        {
            get
            {
                if (!this.IsAutoReturn.HasValue)
                    return eAutoReturnState.NotSet;
                else if (this.IsAutoReturn.HasValue && !this.IsAutoReturn.Value)
                    return eAutoReturnState.Neto;
                else
                    return eAutoReturnState.Full;
            }
            set
            {
                if (value == eAutoReturnState.NotSet)
                {
                    IsAutoReturn = null;
                }
                else if (value == eAutoReturnState.Neto)
                {
                    IsAutoReturn = false;
                }
                else
                {
                    IsAutoReturn = true;
                }
            }
        }
    }
}