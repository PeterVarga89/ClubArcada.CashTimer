using ClubArcada.Common;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClubArcada.BusinessObjects.DataClasses
{
    public partial class CashTable 
    {
        # region PropertyChanged

        public void RefreshVisibility()
        {
            PropertyChanged.Raise(() => ActivePlayerCount);
            PropertyChanged.Raise(() => PlayerCountStatString);
            PropertyChanged.Raise(() => GameTypeString);
        }

        # endregion

        public string GameTypeString
        {
            get
            {
                return GameTypeEnum.GetEnumDescription();
            }
        }

        public eGameType GameTypeEnum
        {
            get
            {
                if (this.GameType == 1)
                    return eGameType.type_single;
                else if (this.GameType == 2)
                    return eGameType.type_double;
                else
                    throw new NullReferenceException();

            }
        }

        public ObservableCollection<CashResult> CashResults
        {
            get;
            set;
        }

        public int ActivePlayerCount
        {
            get
            {
                if (this.CashResults.IsNotNull())
                    return this.CashResults.Where(p => !p.EndTime.HasValue).Count();
                else
                    return 0;
            }
        }

        public string PlayerCountStatString
        {
            get
            {
                return string.Format("9/{0}", this.ActivePlayerCount);
            }
        }

        public bool IsFull { get { return ActivePlayerCount == 9; } }
    }
}
