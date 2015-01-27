CREATE TABLE [dbo].[Banners] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [CreatedByUserId] UNIQUEIDENTIFIER NOT NULL,
    [Description]     VARCHAR (100)    NOT NULL,
    [Url]             VARCHAR (500)    NULL,
    [DateCreated]     DATETIME         NOT NULL,
    [DateDeleted]     DATETIME         NULL,
    [SortNumber]      INT              NOT NULL,
    [Data]            VARBINARY (MAX)  NULL,
    [TargetType]      VARCHAR (50)     NULL,
    CONSTRAINT [PK_Banners] PRIMARY KEY CLUSTERED ([Id] ASC)
);

