CREATE TABLE [dbo].[Tournaments] (
    [TournamentId]    UNIQUEIDENTIFIER NOT NULL,
    [LeagueId]        UNIQUEIDENTIFIER NOT NULL,
    [CreatedByUserId] UNIQUEIDENTIFIER NOT NULL,
    [DeletedByUserId] UNIQUEIDENTIFIER NULL,
    [DateCreated]     DATETIME         NOT NULL,
    [DateDeleted]     DATETIME         NULL,
    [Name]            VARCHAR (100)    NOT NULL,
    [Date]            DATETIME         NOT NULL,
    [GameType]        CHAR (1)         NOT NULL,
    [Description]     VARCHAR (500)    NOT NULL,
    [OldId]           BIGINT           NOT NULL,
    [DateEnded]       DATETIME         NULL,
    [IsHidden]        BIT              NULL,
    CONSTRAINT [PK_Tournaments] PRIMARY KEY CLUSTERED ([TournamentId] ASC)
);

