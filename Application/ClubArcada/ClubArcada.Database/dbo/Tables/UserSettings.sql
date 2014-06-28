CREATE TABLE [dbo].[UserSettings] (
    [SettingId]                 UNIQUEIDENTIFIER NOT NULL,
    [LeagueTableUpdateDateTime] DATETIME         NULL,
    [CashTableUpdateDateTime]   DATETIME         NULL,
    [ApcLeagueUpdateDateTime]   DATETIME         NULL,
    [ApcCashUpdateDateTime]     DATETIME         NULL,
    CONSTRAINT [PK_UserSettings] PRIMARY KEY CLUSTERED ([SettingId] ASC)
);

