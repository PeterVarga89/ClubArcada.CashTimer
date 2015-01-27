CREATE TABLE [dbo].[TournamentResults] (
    [TournamentResultId] UNIQUEIDENTIFIER NOT NULL,
    [TournamentId]       UNIQUEIDENTIFIER NOT NULL,
    [UserId]             UNIQUEIDENTIFIER NOT NULL,
    [IsTimeBonus]        BIT              NOT NULL,
    [Points]             FLOAT (53)       NOT NULL,
    [Rank]               INT              NOT NULL,
    [ReBuyCount]         INT              NOT NULL,
    [AddOnCount]         INT              NOT NULL,
    [PokerCount]         INT              NOT NULL,
    [StraightFlushCount] INT              NOT NULL,
    [RoyalFlushCount]    INT              NOT NULL,
    [DateAdded]          DATETIME         NOT NULL,
    [DateDeleted]        DATETIME         NULL,
    [DateReEntry]        DATETIME         NULL,
    CONSTRAINT [PK_TournamentResults] PRIMARY KEY CLUSTERED ([TournamentResultId] ASC)
);

