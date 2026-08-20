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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE TABLE [ASSET] (
        [ID] bigint NOT NULL,
        [ASSET_TYPE_ID] bigint NOT NULL,
        [NAME] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_ASSET] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE TABLE [HD_PRIORITY] (
        [ID] bigint NOT NULL,
        [NAME] nvarchar(max) NOT NULL,
        [ORDINAL] bigint NOT NULL,
        CONSTRAINT [PK_HD_PRIORITY] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE TABLE [HD_STATUS] (
        [ID] bigint NOT NULL,
        [NAME] nvarchar(max) NOT NULL,
        [ORDINAL] bigint NOT NULL,
        CONSTRAINT [PK_HD_STATUS] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE TABLE [USER] (
        [ID] bigint NOT NULL,
        [USER_NAME] nvarchar(max) NULL,
        [FULL_NAME] nvarchar(max) NULL,
        [ROLE_ID] bigint NULL,
        CONSTRAINT [PK_USER] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE TABLE [HD_TICKET] (
        [ID] bigint NOT NULL,
        [TITLE] nvarchar(max) NULL,
        [SUMMARY] nvarchar(max) NULL,
        [HD_QUEUE_ID] bigint NOT NULL,
        [CREATED] datetime2 NOT NULL,
        [HD_PRIORITY_ID] bigint NULL,
        [HD_STATUS_ID] bigint NULL,
        [OWNER_ID] bigint NULL,
        [SUBMITTER_ID] bigint NULL,
        [ASSET_ID] bigint NULL,
        [CUSTOM_FIELD_VALUE1] nvarchar(max) NULL,
        [CUSTOM_FIELD_VALUE2] nvarchar(max) NULL,
        [CUSTOM_FIELD_VALUE5] nvarchar(max) NULL,
        CONSTRAINT [PK_HD_TICKET] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_HD_TICKET_ASSET_ASSET_ID] FOREIGN KEY ([ASSET_ID]) REFERENCES [ASSET] ([ID]),
        CONSTRAINT [FK_HD_TICKET_HD_PRIORITY_HD_PRIORITY_ID] FOREIGN KEY ([HD_PRIORITY_ID]) REFERENCES [HD_PRIORITY] ([ID]),
        CONSTRAINT [FK_HD_TICKET_HD_STATUS_HD_STATUS_ID] FOREIGN KEY ([HD_STATUS_ID]) REFERENCES [HD_STATUS] ([ID]),
        CONSTRAINT [FK_HD_TICKET_USER_OWNER_ID] FOREIGN KEY ([OWNER_ID]) REFERENCES [USER] ([ID]),
        CONSTRAINT [FK_HD_TICKET_USER_SUBMITTER_ID] FOREIGN KEY ([SUBMITTER_ID]) REFERENCES [USER] ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE TABLE [HD_TICKET_CHANGE] (
        [ID] bigint NOT NULL,
        [HD_TICKET_ID] bigint NOT NULL,
        [TIMESTAMP] datetime2 NOT NULL,
        [USER_ID] bigint NULL,
        [COMMENT] nvarchar(max) NULL,
        [OWNERS_ONLY] bit NOT NULL,
        CONSTRAINT [PK_HD_TICKET_CHANGE] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_HD_TICKET_CHANGE_HD_TICKET_HD_TICKET_ID] FOREIGN KEY ([HD_TICKET_ID]) REFERENCES [HD_TICKET] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_HD_TICKET_CHANGE_USER_USER_ID] FOREIGN KEY ([USER_ID]) REFERENCES [USER] ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ASSET_NAME] ON [ASSET] ([NAME]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_HD_TICKET_ASSET_ID] ON [HD_TICKET] ([ASSET_ID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_HD_TICKET_HD_PRIORITY_ID] ON [HD_TICKET] ([HD_PRIORITY_ID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_HD_TICKET_HD_QUEUE_ID] ON [HD_TICKET] ([HD_QUEUE_ID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_HD_TICKET_HD_STATUS_ID] ON [HD_TICKET] ([HD_STATUS_ID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_HD_TICKET_OWNER_ID_HD_STATUS_ID] ON [HD_TICKET] ([OWNER_ID], [HD_STATUS_ID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_HD_TICKET_SUBMITTER_ID] ON [HD_TICKET] ([SUBMITTER_ID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_HD_TICKET_CHANGE_HD_TICKET_ID] ON [HD_TICKET_CHANGE] ([HD_TICKET_ID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_HD_TICKET_CHANGE_USER_ID] ON [HD_TICKET_CHANGE] ([USER_ID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260820222519_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260820222519_InitialCreate', N'9.0.11');
END;

COMMIT;
GO

