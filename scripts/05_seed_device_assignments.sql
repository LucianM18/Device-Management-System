USE DeviceManagementDb;
GO

IF NOT EXISTS (
    SELECT 1
    FROM dbo.DeviceAssignments da
    INNER JOIN dbo.Devices d ON da.DeviceId = d.Id
    INNER JOIN dbo.Users u ON da.UserId = u.Id
    WHERE d.Name = 'iPhone 13'
      AND u.Name = 'Popescu Mihai'
      AND da.IsActive = 1
)
BEGIN
    INSERT INTO dbo.DeviceAssignments (DeviceId, UserId, AssignedAt, ReleasedAt, IsActive)
    SELECT d.Id, u.Id, GETDATE(), NULL, 1
    FROM dbo.Devices d
    CROSS JOIN dbo.Users u
    WHERE d.Name = 'iPhone 13'
      AND u.Name = 'Popescu Mihai';
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM dbo.DeviceAssignments da
    INNER JOIN dbo.Devices d ON da.DeviceId = d.Id
    INNER JOIN dbo.Users u ON da.UserId = u.Id
    WHERE d.Name = 'Samsung Galaxy Tab S8'
      AND u.Name = 'Ionescu Andreea'
      AND da.IsActive = 1
)
BEGIN
    INSERT INTO dbo.DeviceAssignments (DeviceId, UserId, AssignedAt, ReleasedAt, IsActive)
    SELECT d.Id, u.Id, GETDATE(), NULL, 1
    FROM dbo.Devices d
    CROSS JOIN dbo.Users u
    WHERE d.Name = 'Samsung Galaxy Tab S8'
      AND u.Name = 'Ionescu Andreea';
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM dbo.DeviceAssignments da
    INNER JOIN dbo.Devices d ON da.DeviceId = d.Id
    INNER JOIN dbo.Users u ON da.UserId = u.Id
    WHERE d.Name = 'Google Pixel 8'
      AND u.Name = 'Grigore Radu'
      AND da.IsActive = 1
)
BEGIN
    INSERT INTO dbo.DeviceAssignments (DeviceId, UserId, AssignedAt, ReleasedAt, IsActive)
    SELECT d.Id, u.Id, GETDATE(), NULL, 1
    FROM dbo.Devices d
    CROSS JOIN dbo.Users u
    WHERE d.Name = 'Google Pixel 8'
      AND u.Name = 'Grigore Radu';
END
GO