CREATE TABLE [dbo].[Galleries] (
    [GalleryId]   UNIQUEIDENTIFIER NOT NULL,
    [Name]        VARCHAR (50)     NOT NULL,
    [Date]        DATETIME         NULL,
    [DateCreated] DATETIME         NOT NULL,
    [DateDeleted] DATETIME         NULL,
    CONSTRAINT [PK_Galleries] PRIMARY KEY CLUSTERED ([GalleryId] ASC)
);

