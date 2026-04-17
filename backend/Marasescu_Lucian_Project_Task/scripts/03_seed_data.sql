USE DeviceManagementDb;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Name = 'Stancu Marius')
BEGIN
    INSERT INTO dbo.Users (Name, Role, Location)
    VALUES ('Stancu Marius', 'QA Engineer', 'Bucharest');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Name = 'Popescu Mihai')
BEGIN
    INSERT INTO dbo.Users (Name, Role, Location)
    VALUES ('Popescu Mihai', 'Sales Representative', 'Cluj-Napoca');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Name = 'Ionescu Andreea')
BEGIN
    INSERT INTO dbo.Users (Name, Role, Location)
    VALUES ('Ionescu Andreea', 'Support Specialist', 'Iasi');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Name = 'Grigore Radu')
BEGIN
    INSERT INTO dbo.Users (Name, Role, Location)
    VALUES ('Grigore Radu', 'Manager', 'Timisoara');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE Name = 'iPhone 15')
BEGIN
    INSERT INTO dbo.Devices
    (
        Name, Manufacturer, Type, OperatingSystem,
        OsVersion, Processor, RamAmount, Description
    )
    VALUES
    (
        'iPhone 13', 'Apple', 'phone', 'iOS',
        '17.2', 'A15 Bionic', 4, 'Sales team device'
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE Name = 'Samsung Galaxy Tab S8')
BEGIN
    INSERT INTO dbo.Devices
    (
        Name, Manufacturer, Type, OperatingSystem,
        OsVersion, Processor, RamAmount, Description
    )
    VALUES
    (
        'Samsung Galaxy Tab S8', 'Samsung', 'tablet', 'Android',
        '14', 'Snapdragon 8 Gen 1', 8, 'Tablet for field presentations'
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE Name = 'Google Pixel 8')
BEGIN
    INSERT INTO dbo.Devices
    (
        Name, Manufacturer, Type, OperatingSystem,
        OsVersion, Processor, RamAmount, Description
    )
    VALUES
    (
        'Google Pixel 8', 'Google', 'phone', 'Android',
        '14', 'Google Tensor G3', 8, 'Testing device'
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Devices WHERE Name = 'iPad Air 5')
BEGIN
    INSERT INTO dbo.Devices
    (
        Name, Manufacturer, Type, OperatingSystem,
        OsVersion, Processor, RamAmount, Description
    )
    VALUES
    (
        'iPad Air 5', 'Apple', 'tablet', 'iPadOS',
        '17.4', 'Apple M1', 8, 'Marketing team tablet'
    );
END
GO