CREATE TABLE [dbo].[TournamentDetails] (
    [TournamentDetailId] UNIQUEIDENTIFIER NOT NULL,
    [TournamentId]       UNIQUEIDENTIFIER NOT NULL,
    [StructureId]        UNIQUEIDENTIFIER NOT NULL,
    [BuyInPrize]         INT              NOT NULL,
    [ReBuyPrize]         INT              NOT NULL,
    [AddOnPrize]         INT              NOT NULL,
    [BuyInStack]         INT              NOT NULL,
    [ReBuyStack]         INT              NOT NULL,
    [AddOnStack]         INT              NOT NULL,
    [BonusStack]         INT              NOT NULL,
    [IsFullPointed]      BIT              NOT NULL,
    [IsLeague]           BIT              NOT NULL,
    [ReEntryCount]       INT              NULL,
    [GTD]                INT              NULL,
    [ReBuyCount]         INT              NULL,
    [IsFood]             BIT              CONSTRAINT [DF_TournamentDetails_IsFood] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TournamentDetails] PRIMARY KEY CLUSTERED ([TournamentDetailId] ASC)
);

