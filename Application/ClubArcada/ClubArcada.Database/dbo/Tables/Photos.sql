CREATE TABLE [dbo].[Photos] (
    [PhotoId]     UNIQUEIDENTIFIER NOT NULL,
    [GalleryId]   UNIQUEIDENTIFIER NOT NULL,
    [DateCreated] DATETIME         NOT NULL,
    [DateDeleted] DATETIME         NULL,
    CONSTRAINT [PK_Photos] PRIMARY KEY CLUSTERED ([PhotoId] ASC)
);

