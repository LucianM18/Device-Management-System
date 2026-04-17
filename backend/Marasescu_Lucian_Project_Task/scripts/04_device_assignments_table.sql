USE DeviceManagementDb;
GO

IF OBJECT_ID('dbo.DeviceAssignments', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.DeviceAssignments
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        DeviceId INT NOT NULL,
        UserId INT NOT NULL,
        AssignedAt DATETIME2 NOT NULL,
        ReleasedAt DATETIME2 NULL,
        IsActive BIT NOT NULL,
        CONSTRAINT FK_DeviceAssignments_Devices FOREIGN KEY (DeviceId) REFERENCES dbo.Devices(Id),
        CONSTRAINT FK_DeviceAssignments_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id)
    );
END
GO