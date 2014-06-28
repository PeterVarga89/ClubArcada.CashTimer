CREATE TABLE [dbo].[CashResults] (
    [CashResultId] UNIQUEIDENTIFIER NOT NULL,
    [TournamentId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [CashTableId]  UNIQUEIDENTIFIER NULL,
    [PlayerId]     UNIQUEIDENTIFIER NOT NULL,
    [Duration]     INT              NOT NULL,
    [StartTime]    DATETIME         CONSTRAINT [DF_CashResults_StartTime] DEFAULT (NULL) NULL,
    [EndTime]      DATETIME         CONSTRAINT [DF_CashResults_EndTime] DEFAULT (NULL) NULL,
    [Quociente]    FLOAT (53)       NULL,
    [CashOut]      INT              NULL,
    CONSTRAINT [PK_CashResults] PRIMARY KEY CLUSTERED ([CashResultId] ASC)
);

