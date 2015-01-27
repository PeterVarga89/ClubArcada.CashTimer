CREATE TABLE [dbo].[Transactions] (
    [TransactionId]  UNIQUEIDENTIFIER NOT NULL,
    [UserId]         UNIQUEIDENTIFIER NOT NULL,
    [CratedByUserId] UNIQUEIDENTIFIER NOT NULL,
    [DateCreated]    DATETIME         NOT NULL,
    [IsBorrowed]     BIT              NOT NULL,
    [DateDeleted]    DATETIME         NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED ([TransactionId] ASC),
    CONSTRAINT [FK_Transactions_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId])
);

