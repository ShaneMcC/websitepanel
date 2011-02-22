USE [${install.database}]
GO

-- update database version
DECLARE @build_version nvarchar(10), @build_date datetime
SET @build_version = N'${release.version}'
SET @build_date = '${release.date}T00:00:00' -- ISO 8601 Format (YYYY-MM-DDTHH:MM:SS)

IF NOT EXISTS (SELECT * FROM [dbo].[Versions] WHERE [DatabaseVersion] = @build_version)
BEGIN
	INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (@build_version, @build_date)
END
GO

-- Helicon APE's quota
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'Web.Htaccess')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (344, 2, 9, N'Web.Htaccess', N'htaccess', 1, 0, NULL)
END
GO

-- Change quota order for MySQL 4 Quotas
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [GroupID] = 6 And [QuotaOrder] = 4)
BEGIN
	UPDATE [dbo].[Quotas] SET [QuotaOrder] = 4 WHERE [GroupID] = 6 And [QuotaOrder] = 3
END
GO
-- Check for new MySQL 4 quota (MaxDatabaseSize)
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'MySQL4.MaxDatabaseSize')
BEGIN
	-- Add new MySQL 4 quotas if not exists
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (103, 6, 3, N'MySQL4.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL)
END
GO
-- Check for new MySQL 4 quota (Restore)
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'MySQL4.Restore')
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (104, 6, 5, N'MySQL4.Restore', N'Database Restores', 1, 0, NULL)
END
GO
-- Check for new MySQL 4 quota (Truncate)
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'MySQL4.Truncate')
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (105, 6, 6, N'MySQL4.Truncate', N'Database Truncate', 1, 0, NULL)
END
GO

-- Change quota order for MySQL 5 Quotas
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [GroupID] = 11 And [QuotaOrder] = 4)
BEGIN
	UPDATE [dbo].[Quotas] SET [QuotaOrder] = 4 WHERE [GroupID] = 11 And [QuotaOrder] = 3
END
GO
-- Add new MySQL 5 quota if not exists (MaxDatabaseSize)
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'MySQL5.MaxDatabaseSize')
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (106, 11, 3, N'MySQL5.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL)
END
GO
-- Add new MySQL 5 quota if not exists (Restore)
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'MySQL5.Restore')
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (107, 11, 5, N'MySQL5.Restore', N'Database Restores', 1, 0, NULL)
END
GO
-- Add new MySQL 5 quota if not exists (Truncate)
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'MySQL5.Truncate')
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (108, 11, 6, N'MySQL5.Truncate', N'Database Truncate', 1, 0, NULL)
END
GO

-- Notify Overused Databases Scheduled Task
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES')
BEGIN
	INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WebsitePanel.EnterpriseServer.NotifyOverusedDatabasesTask, WebsitePanel.EnterpriseServer', 2)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskViewConfiguration] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ConfigurationID] = N'ASP_NET')
BEGIN
	INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/NotifyOverusedDatabases.ascx')
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'MSSQL_OVERUSED')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'MSSQL_OVERUSED', N'Boolean', N'true', 1)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'MYSQL_OVERUSED')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'MYSQL_OVERUSED', N'Boolean', N'true', 1)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'SEND_WARNING_EMAIL')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'SEND_WARNING_EMAIL', N'Boolean', N'true', 1)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'SEND_OVERUSED_EMAIL')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'SEND_OVERUSED_EMAIL', N'Boolean', N'true', 1)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'OVERUSED_MAIL_BCC')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_MAIL_BCC', N'String', N'', 1)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'OVERUSED_MAIL_BODY')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_MAIL_BODY', N'String', N'', 1)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'OVERUSED_MAIL_FROM')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_MAIL_FROM', N'String', N'', 1)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'OVERUSED_MAIL_SUBJECT')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_MAIL_SUBJECT', N'String', N'', 1)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'OVERUSED_USAGE_THRESHOLD')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_USAGE_THRESHOLD', N'String', N'100', 1)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'WARNING_MAIL_BCC')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_MAIL_BCC', N'String', N'', 1)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'WARNING_MAIL_BODY')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_MAIL_BODY', N'String', N'', 1)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'WARNING_MAIL_FROM')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_MAIL_FROM', N'String', N'', 1)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'WARNING_MAIL_SUBJECT')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_MAIL_SUBJECT', N'String', N'', 1)
END
GO
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'WARNING_USAGE_THRESHOLD')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_USAGE_THRESHOLD', N'String', N'80', 1)
END
GO