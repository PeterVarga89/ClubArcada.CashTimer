CREATE TABLE [dbo].[Wages] (
    [WageId]      UNIQUEIDENTIFIER NOT NULL,
    [PersonId]    UNIQUEIDENTIFIER NOT NULL,
    [Wage]        FLOAT (53)       NOT NULL,
    [DateCreated] DATETIME         NOT NULL,
    [DateFrom]    DATETIME         NOT NULL,
    CONSTRAINT [PK_Wages] PRIMARY KEY CLUSTERED ([WageId] ASC)
);

