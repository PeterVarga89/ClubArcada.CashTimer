CREATE TABLE [dbo].[Logins] (
    [LoginId]     UNIQUEIDENTIFIER NOT NULL,
    [UserId]      UNIQUEIDENTIFIER NOT NULL,
    [DateCreated] DATETIME         NOT NULL,
    CONSTRAINT [PK_Logins] PRIMARY KEY CLUSTERED ([LoginId] ASC)
);

