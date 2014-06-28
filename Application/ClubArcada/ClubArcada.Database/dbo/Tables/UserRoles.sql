CREATE TABLE [dbo].[UserRoles] (
    [UserRoleId]              UNIQUEIDENTIFIER NOT NULL,
    [UserId]                  UNIQUEIDENTIFIER NOT NULL,
    [CanAddTournament]        BIT              NOT NULL,
    [CanDeleteTournament]     BIT              NOT NULL,
    [CanDeletePastTournament] BIT              NOT NULL,
    [CanEditTournament]       BIT              NOT NULL,
    [CanEditPastTournament]   BIT              NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([UserRoleId] ASC)
);

