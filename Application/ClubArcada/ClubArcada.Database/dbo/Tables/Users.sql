CREATE TABLE [dbo].[Users] (
    [UserId]        UNIQUEIDENTIFIER NOT NULL,
    [NickName]      VARCHAR (50)     NOT NULL,
    [FirstName]     VARCHAR (50)     NOT NULL,
    [LastName]      VARCHAR (50)     NOT NULL,
    [PeronalId]     NCHAR (10)       NOT NULL,
    [Email]         VARCHAR (50)     NOT NULL,
    [PhoneNumber]   VARCHAR (20)     NOT NULL,
    [Comment]       VARCHAR (200)    NOT NULL,
    [Password]      VARCHAR (50)     NOT NULL,
    [IsSms]         BIT              NOT NULL,
    [IsMail]        BIT              NOT NULL,
    [IsAdmin]       BIT              NOT NULL,
    [IsBlocked]     BIT              NOT NULL,
    [DateActivated] DATETIME         NULL,
    [DateCreated]   DATETIME         NOT NULL,
    [DateDeleted]   DATETIME         NULL,
    [OldId]         INT              NOT NULL,
    [IsPersonal]    BIT              CONSTRAINT [DF_Users_IsPersonal] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

