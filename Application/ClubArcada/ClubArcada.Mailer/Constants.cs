namespace ClubArcada.Mailer
{
    public class Constants
    {
        public const string MailFrom = "cashtimer@arcade-group.sk";
        public const string MailSender = "cashtimer@arcade-group.sk";

        public const string MailTo = "office@arcade-group.sk";
        public const string MailToCC = "petervarga@arcade-group.sk";

        public static int MailPort = 25;
        public static string MailUserName = "cashtimer@arcade-group.sk";
        public static string MailPassword = "vape6931";
        public static string MailSmtpClient = "imap.arcade-group.sk";

        public static string MailNewUserRegistrationSubject = "Registrácia nového hráča";
        public static string MailNewUserRegistrationBody = "Nick: {0}\nMeno: {1}\nPriezvisko: {2}\nTelefon: {3}\nEmail: {4}";

        public static string MailNewBonusSubject = "Bonus bol priradený";
        public static string MailNewBonusBody = "Nick: {0}\nMeno: {1}\nPriezvisko: {2}\nHra: {3}\nSuma: {4}€\nBalance: {5}€";

        public static string MailNewReplySubject = "Peniaze boli vrátené";
        public static string MailNewBorrowSubject = "Peniaze boli požičané";
        public static string MailNewBorrowBody = "Nick: {0}\nMeno: {1}\nPriezvisko: {2}\nHra: {3}\nSuma: {4}€\nBalance: {5}€\nZodpovedný: {6}";

        public static string MailUserRegisteredWithNegativeBalanceSubject = "Hráč s negatívnym balancom sa registroval do hry";
        public static string MailUserRegisteredWithNegativeBalanceBody = "Nick: {0}\nMeno: {1}\nPriezvisko: {2}\nBalance: {3}€\nZodpovedný: {4}";

        public static string MailSignature = "\n----\nTento mail je generovaný automaticky.\nNeodpovedajte naň.";
    }
}