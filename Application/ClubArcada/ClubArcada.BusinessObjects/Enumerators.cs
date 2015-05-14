using System.ComponentModel;

namespace ClubArcada.BusinessObjects
{
    public enum eAutoReturnState
    {
        NotSet = 0,
        Neto = 1,
        Full = 2
    }

    public enum eConnectionString
    {
        NotSet = 0,

        [Description(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|Cash.Database.mdf;Integrated Security=True")]
        Local = 1,

        [Description("Data Source=82.119.117.77;Initial Catalog=PokerSystem;Integrated Security=False;User Id=PokerTimer;Password=Poker1969;MultipleActiveResultSets=True")]
        Online = 2
    }

    public enum eGameType
    {
        NotSet = 0,

        [Description("1€ – 1€ a nizšie")]
        type_single = 1,

        [Description("1€ – 2€ a vyššie")]
        type_double = 2,
    }

    public enum eTransactionType
    {
        [Description("Pôžička (-)")]
        NotSet = 0,

        [Description("Pôžička (+)")]
        Returned = 1,

        [Description("Bar (-)")]
        Bar = 2,

        [Description("Bar (+)")]
        BarReturned = 3,

        [Description("Turnaj (-)")]
        Tournament = 4,

        [Description("Turnaj (+)")]
        TournamentReturned = 5,

        [Description("Cash Game (-)")]
        CashGame = 6,

        [Description("Cash Game (+)")]
        CashGameReturned = 7,

        [Description("Bonus (-)")]
        Bonus = 8,

        [Description("Bonus (-)")]
        BonusReturned = 9
    }
}