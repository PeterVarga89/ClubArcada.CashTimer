CREATE TABLE [dbo].[News] (
    [NewsId]          UNIQUEIDENTIFIER NOT NULL,
    [CreatedByUserId] UNIQUEIDENTIFIER NOT NULL,
    [DeletedByUserId] UNIQUEIDENTIFIER NULL,
    [Title]           VARCHAR (100)    NOT NULL,
    [Text]            VARCHAR (8000)   NOT NULL,
    [Link]            VARCHAR (1000)   NOT NULL,
    [DateCreated]     DATETIME         NOT NULL,
    [DateDeleted]     DATETIME         NULL,
    CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED ([NewsId] ASC)
);

