
USE DeviceManagementDb;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'Email')
BEGIN
    ALTER TABLE dbo.Users
    ADD Email NVARCHAR(256) NOT NULL DEFAULT '';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'PasswordHash')
BEGIN
    ALTER TABLE dbo.Users
    ADD PasswordHash NVARCHAR(512) NOT NULL DEFAULT '';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.Users') AND name = 'IX_Users_Email')
BEGIN
    CREATE UNIQUE INDEX IX_Users_Email ON dbo.Users(Email)
    WHERE Email != '';
END
GO

