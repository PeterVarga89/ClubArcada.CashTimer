CREATE TABLE [dbo].[TournamentTemplates] (
    [TournamentTemplateId] UNIQUEIDENTIFIER NOT NULL,
    [StrucktureId]         UNIQUEIDENTIFIER NOT NULL,
    [CreatedBy]            UNIQUEIDENTIFIER NOT NULL,
    [Name]                 VARCHAR (50)     NOT NULL,
    [BuyInPrize]           INT              NOT NULL,
    [ReBuyPrize]           INT              NOT NULL,
    [AddOnPrize]           INT              NOT NULL,
    [BuyInStack]           INT              NOT NULL,
    [ReBuyStack]           INT              NOT NULL,
    [AddOnStack]           INT              NOT NULL,
    [BonusStack]           INT              NOT NULL,
    [GameType]             CHAR (1)         NOT NULL,
    [IsLeague]             BIT              NOT NULL,
    [IsFullPointed]        BIT              NOT NULL,
    [Description]          VARCHAR (150)    NOT NULL,
    [StartHour]            INT              NOT NULL,
    [DateCreated]          DATETIME         NOT NULL,
    [DateDeleted]          DATETIME         NULL,
    CONSTRAINT [PK_TournamentTemplates] PRIMARY KEY CLUSTERED ([TournamentTemplateId] ASC)
);

