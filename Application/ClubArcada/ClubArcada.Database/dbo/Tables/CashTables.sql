CREATE TABLE [dbo].[CashTables] (
    [CashTableId] UNIQUEIDENTIFIER NOT NULL,
    [GameType]    INT              NOT NULL,
    [Name]        VARCHAR (50)     NOT NULL,
    [DateCreated] DATETIME         NOT NULL,
    [DateDeleted] DATETIME         NULL,
    CONSTRAINT [PK_CashTables] PRIMARY KEY CLUSTERED ([CashTableId] ASC)
);

