CREATE TABLE [dbo].[Screenshots] (
    [ScreenshotId] UNIQUEIDENTIFIER NOT NULL,
    [DateCreated]  DATETIME         NOT NULL,
    [DateSolved]   DATETIME         NULL,
    CONSTRAINT [PK_Screenshots] PRIMARY KEY CLUSTERED ([ScreenshotId] ASC)
);

