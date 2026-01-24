IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Roles] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(50) NOT NULL,
    [Description] nvarchar(255) NULL,
    CONSTRAINT [PK__Roles__3214EC072A8C9B4D] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] uniqueidentifier NOT NULL,
    [Username] nvarchar(100) NOT NULL,
    [Email] nvarchar(255) NOT NULL,
    [PasswordHash] nvarchar(255) NOT NULL,
    [PasswordSalt] nvarchar(255) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT ((sysdatetime())),
    [UpdatedAt] datetime2 NULL,
    CONSTRAINT [PK__Users__3214EC07A9F30ECD] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [asRole] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_asRole] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_asRole_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]),
    CONSTRAINT [FK_asRole_Users] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);
GO

CREATE TABLE [RefreshTokens] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [Token] nvarchar(500) NOT NULL,
    [ExpiresAt] datetime2 NOT NULL,
    [RevokedAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT ((sysdatetime())),
    [Device] nvarchar(255) NULL,
    CONSTRAINT [PK__RefreshT__3214EC07745F8BD0] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);
GO

CREATE INDEX [IX_asRole_RoleId] ON [asRole] ([RoleId]);
GO

CREATE INDEX [IX_RefreshTokens_UserId] ON [RefreshTokens] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260124082422_v1', N'8.0.22');
GO

COMMIT;
GO

