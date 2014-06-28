CREATE TABLE [dbo].[CashIns] (
    [CashInId]        UNIQUEIDENTIFIER NOT NULL,
    [CashResultId]    UNIQUEIDENTIFIER NOT NULL,
    [CreatedByUserId] UNIQUEIDENTIFIER NOT NULL,
    [DateCreated]     DATETIME         NOT NULL,
    [DateDeleted]     DATETIME         NULL,
    [Amount]          FLOAT (53)       NOT NULL,
    CONSTRAINT [PK_CashIns] PRIMARY KEY CLUSTERED ([CashInId] ASC)
);

