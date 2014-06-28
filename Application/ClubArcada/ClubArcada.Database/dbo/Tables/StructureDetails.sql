CREATE TABLE [dbo].[StructureDetails] (
    [StructureDetailId] UNIQUEIDENTIFIER NOT NULL,
    [StructureId]       UNIQUEIDENTIFIER NOT NULL,
    [Type]              INT              NOT NULL,
    [Number]            INT              NOT NULL,
    [Level]             INT              NOT NULL,
    [SmallBlind]        INT              NOT NULL,
    [BigBlind]          INT              NOT NULL,
    [Ante]              INT              NOT NULL,
    [Time]              INT              NOT NULL,
    CONSTRAINT [PK_StructureDetails] PRIMARY KEY CLUSTERED ([StructureDetailId] ASC)
);

