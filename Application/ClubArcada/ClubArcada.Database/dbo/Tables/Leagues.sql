CREATE TABLE [dbo].[Leagues] (
    [LeagueId]        UNIQUEIDENTIFIER NOT NULL,
    [CreatedByUserId] UNIQUEIDENTIFIER NOT NULL,
    [Name]            VARCHAR (50)     NOT NULL,
    [IsActive]        BIT              NOT NULL,
    [DateCreated]     DATETIME         NOT NULL,
    [DateDeleted]     DATETIME         NULL,
    [OldId]           INT              NOT NULL,
    CONSTRAINT [PK_Leagues] PRIMARY KEY CLUSTERED ([LeagueId] ASC)
);

