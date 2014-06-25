using System.ComponentModel;

namespace ClubArcada.BusinessObjects
{
    public class Enumerators
    {
        public enum eConnectionString
        {
            NotSet = 0,

            [Description(@"Data Source=(LocalDB)\v11.0;Integrated Security=True;AttachDbFilename=|DataDirectory|\CashDB.mdf")]
            Local = 1,

            [Description("Data Source=82.119.117.77;Initial Catalog=PokerSystem;Integrated Security=False;User Id=PokerTimer;Password=Poker1969;MultipleActiveResultSets=True")]
            Online = 2
        }

        public enum eGameType
        {
            NotSet = 0,

            [Description("1 € – 1 € a nizšie")]
            type_single = 1,

            [Description("1 € – 2 € a vyššie")]
            type_double = 2,
        }
    }
}
