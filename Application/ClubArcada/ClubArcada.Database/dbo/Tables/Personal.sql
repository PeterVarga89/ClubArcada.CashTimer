CREATE TABLE [dbo].[Personal] (
    [PersonId]    UNIQUEIDENTIFIER NOT NULL,
    [FirstName]   VARBINARY (50)   NOT NULL,
    [LastName]    VARCHAR (50)     NOT NULL,
    [Login]       VARCHAR (50)     NOT NULL,
    [Password]    VARCHAR (50)     NOT NULL,
    [DateCreated] DATETIME         NOT NULL,
    [DateDeleted] DATETIME         NULL,
    CONSTRAINT [PK_Personal] PRIMARY KEY CLUSTERED ([PersonId] ASC)
);

