CREATE TABLE [dbo].[Structures] (
    [StructureId]      UNIQUEIDENTIFIER NOT NULL,
    [Name]             VARCHAR (50)     NOT NULL,
    [DateCreated]      DATETIME         NOT NULL,
    [DateDeleted]      DATETIME         NULL,
    [DataFreezedLevel] INT              NOT NULL,
    CONSTRAINT [PK_Structures] PRIMARY KEY CLUSTERED ([StructureId] ASC)
);

