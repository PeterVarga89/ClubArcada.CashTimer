CREATE TABLE [dbo].[UserRoles] (
    [UserRoleId]            UNIQUEIDENTIFIER NOT NULL,
    [UserId]                UNIQUEIDENTIFIER NOT NULL,
    [IsMasterAdmin]         BIT              NOT NULL,
    [CanOpenBarCalendar]    BIT              NOT NULL,
    [CanOpenPokerCalendar]  BIT              NOT NULL,
    [CanOpenWebSettings]    BIT              NOT NULL,
    [CanAddTournament]      BIT              NOT NULL,
    [CanDeleteTournament]   BIT              NOT NULL,
    [CanEditTournament]     BIT              NOT NULL,
    [CanEditPastTournament] BIT              NOT NULL,
    [CanOpenUsers]          BIT              CONSTRAINT [DF_UserRoles_CanOpenUsers] DEFAULT ((0)) NOT NULL,
    [CanEditUsers]          BIT              CONSTRAINT [DF_UserRoles_CanEditUsers] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([UserRoleId] ASC),
    CONSTRAINT [FK__UserRoles__UserI__09FE775D] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([UserId]) ON DELETE CASCADE ON UPDATE CASCADE
);

