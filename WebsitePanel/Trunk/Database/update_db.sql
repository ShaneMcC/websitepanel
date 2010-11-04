USE [${install.database}]
GO

-- update database version
DECLARE @build_version nvarchar(10), @build_date datetime
SET @build_version = '1.1.0.0'
SET @build_date = '09/22/2010'

IF NOT EXISTS (SELECT * FROM [dbo].[Versions] WHERE [DatabaseVersion] = @build_version)
BEGIN
	INSERT INTO [Versions] ([DatabaseVersion], [BuildDate]) VALUES (@build_version, @build_date)
END
GO

-- Update SmarterStats service provider display name
IF EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = 62)
BEGIN
	UPDATE [dbo].[Providers] SET [DisplayName] = N'SmarterStats 5.x-6.x' WHERE [ProviderID] = 62
END
GO

IF EXISTS (SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupID] = 33)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupName] = N'ExchangeHostingMode' WHERE [GroupID] = 33
END
GO

IF EXISTS (SELECT * FROM [dbo].[Providers] WHERE [ProviderID] = 207)
BEGIN
	UPDATE [dbo].[Providers] SET [DisplayName] = N'Exchange Server 2010 SP1 Hosting Mode' WHERE [ProviderID] = 207
END
GO