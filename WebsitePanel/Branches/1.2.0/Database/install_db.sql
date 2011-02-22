USE [${install.database}]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleParameters](
	[ScheduleID] [int] NOT NULL,
	[ParameterID] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[ParameterValue] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_ScheduleParameters] PRIMARY KEY CLUSTERED 
(
	[ScheduleID] ASC,
	[ParameterID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[ScheduleParameters] ([ScheduleID], [ParameterID], [ParameterValue]) VALUES (1, N'SUSPEND_OVERUSED', N'false')
GO
INSERT [dbo].[ScheduleParameters] ([ScheduleID], [ParameterID], [ParameterValue]) VALUES (2, N'SUSPEND_OVERUSED', N'false')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExchangeAccounts](
	[AccountID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[AccountType] [int] NOT NULL,
	[AccountName] [nvarchar](20) COLLATE Latin1_General_CI_AS NOT NULL,
	[DisplayName] [nvarchar](300) COLLATE Latin1_General_CI_AS NOT NULL,
	[PrimaryEmailAddress] [nvarchar](300) COLLATE Latin1_General_CI_AS NULL,
	[MailEnabledPublicFolder] [bit] NULL,
	[MailboxManagerActions] [varchar](200) COLLATE Latin1_General_CI_AS NULL,
	[SamAccountName] [nvarchar](100) COLLATE Latin1_General_CI_AS NULL,
	[AccountPassword] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ExchangeAccounts] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [IX_ExchangeAccounts_UniqueAccountName] UNIQUE NONCLUSTERED 
(
	[AccountName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

















CREATE PROCEDURE [dbo].[GetExchangeMailboxes]
	@ItemID int
AS
BEGIN
SELECT
	AccountID,
	ItemID,
	AccountType,
	AccountName,
	DisplayName,
	PrimaryEmailAddress,
	MailEnabledPublicFolder
FROM
	ExchangeAccounts
WHERE
	ItemID = @ItemID AND
	(AccountType =1  OR AccountType=5 OR AccountType=6) 
ORDER BY 1

END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



























CREATE PROCEDURE [dbo].[GetExchangeAccounts]
(
	@ItemID int,
	@AccountType int
)
AS
SELECT
	AccountID,
	ItemID,
	AccountType,
	AccountName,
	DisplayName,
	PrimaryEmailAddress,
	MailEnabledPublicFolder
FROM
	ExchangeAccounts
WHERE
	ItemID = @ItemID AND
	(AccountType = @AccountType OR @AccountType IS NULL) 
ORDER BY DisplayName
RETURN



























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


























CREATE PROCEDURE [dbo].[GetExchangeAccount] 
(
	@ItemID int,
	@AccountID int
)
AS
SELECT
	AccountID,
	ItemID,
	AccountType,
	AccountName,
	DisplayName,
	PrimaryEmailAddress,
	MailEnabledPublicFolder,
	MailboxManagerActions,
	SamAccountName,
	AccountPassword 
FROM
	ExchangeAccounts
WHERE
	ItemID = @ItemID AND
	AccountID = @AccountID
RETURN





























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE ExchangeAccountExists 
(
	@AccountName nvarchar(20),
	@Exists bit OUTPUT
)
AS
SET @Exists = 0
IF EXISTS(SELECT * FROM ExchangeAccounts WHERE AccountName = @AccountName)
BEGIN
	SET @Exists = 1
END

RETURN































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HostingPlans](
	[PlanID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[PackageID] [int] NULL,
	[ServerID] [int] NULL,
	[PlanName] [nvarchar](200) COLLATE Latin1_General_CI_AS NOT NULL,
	[PlanDescription] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[Available] [bit] NOT NULL,
	[SetupPrice] [money] NULL,
	[RecurringPrice] [money] NULL,
	[RecurrenceUnit] [int] NULL,
	[RecurrenceLength] [int] NULL,
	[IsAddon] [bit] NULL,
 CONSTRAINT [PK_HostingPlans] PRIMARY KEY CLUSTERED 
(
	[PlanID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetHostingPlan
(
	@ActorID int,
	@PlanID int
)
AS

SELECT
	PlanID,
	UserID,
	PackageID,
	ServerID,
	PlanName,
	PlanDescription,
	Available,
	SetupPrice,
	RecurringPrice,
	RecurrenceLength,
	RecurrenceUnit,
	IsAddon
FROM HostingPlans AS HP
WHERE HP.PlanID = @PlanID

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[OwnerID] [int] NULL,
	[RoleID] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[IsDemo] [bit] NOT NULL,
	[IsPeer] [bit] NOT NULL,
	[Username] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[Password] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[FirstName] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[LastName] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[Email] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[Created] [datetime] NULL,
	[Changed] [datetime] NULL,
	[Comments] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[SecondaryEmail] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[Address] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[City] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[State] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[Country] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[Zip] [varchar](20) COLLATE Latin1_General_CI_AS NULL,
	[PrimaryPhone] [varchar](30) COLLATE Latin1_General_CI_AS NULL,
	[SecondaryPhone] [varchar](30) COLLATE Latin1_General_CI_AS NULL,
	[Fax] [varchar](30) COLLATE Latin1_General_CI_AS NULL,
	[InstantMessenger] [varchar](100) COLLATE Latin1_General_CI_AS NULL,
	[HtmlMail] [bit] NULL,
	[CompanyName] [nvarchar](100) COLLATE Latin1_General_CI_AS NULL,
	[EcommerceEnabled] [bit] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [IX_Users_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET IDENTITY_INSERT [dbo].[Users] ON 

GO
INSERT [dbo].[Users] ([UserID], [OwnerID], [RoleID], [StatusID], [IsDemo], [IsPeer], [Username], [Password], [FirstName], [LastName], [Email], [Created], [Changed], [Comments], [SecondaryEmail], [Address], [City], [State], [Country], [Zip], [PrimaryPhone], [SecondaryPhone], [Fax], [InstantMessenger], [HtmlMail], [CompanyName], [EcommerceEnabled]) VALUES (1, NULL, 1, 1, 0, 0, N'serveradmin', N'', N'Enterprise', N'Administrator', N'serveradmin@myhosting.com', CAST(0x00009DB500D45270 AS DateTime), CAST(0x00009DB500D45270 AS DateTime), N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', 1, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE FUNCTION [dbo].[CheckIsUserAdmin]
(
	@UserID int
)
RETURNS bit
AS
BEGIN

IF @UserID = -1
RETURN 1

IF EXISTS (SELECT UserID FROM Users
WHERE UserID = @UserID AND RoleID = 1) -- administrator
RETURN 1

RETURN 0
END



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE FUNCTION [dbo].[CheckActorUserRights]
(
	@ActorID int,
	@UserID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1 OR @UserID IS NULL
RETURN 1


-- check if the user requests himself
IF @ActorID = @UserID
BEGIN
	RETURN 1
END

DECLARE @IsPeer bit
DECLARE @OwnerID int

SELECT @IsPeer = IsPeer, @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @IsPeer = 1
SET @ActorID = @OwnerID

-- check if the user requests his owner
/*
IF @ActorID = @UserID
BEGIN
	RETURN 0
END
*/
IF @ActorID = @UserID
BEGIN
	RETURN 1
END

DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
		BREAK
	
	IF @ParentUserID = @ActorID
	RETURN 1
	
	SET @TmpUserID = @ParentUserID
END


RETURN 0
END


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Packages](
	[PackageID] [int] IDENTITY(1,1) NOT NULL,
	[ParentPackageID] [int] NULL,
	[UserID] [int] NOT NULL,
	[PackageName] [nvarchar](300) COLLATE Latin1_General_CI_AS NULL,
	[PackageComments] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[ServerID] [int] NULL,
	[StatusID] [int] NOT NULL,
	[PlanID] [int] NULL,
	[PurchaseDate] [datetime] NULL,
	[OverrideQuotas] [bit] NOT NULL,
	[BandwidthUpdated] [datetime] NULL,
 CONSTRAINT [PK_Packages] PRIMARY KEY CLUSTERED 
(
	[PackageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET IDENTITY_INSERT [dbo].[Packages] ON 

GO
INSERT [dbo].[Packages] ([PackageID], [ParentPackageID], [UserID], [PackageName], [PackageComments], [ServerID], [StatusID], [PlanID], [PurchaseDate], [OverrideQuotas], [BandwidthUpdated]) VALUES (1, NULL, 1, N'System', N'', NULL, 1, NULL, CAST(0x00009DB500D45272 AS DateTime), 0, NULL)
GO
SET IDENTITY_INSERT [dbo].[Packages] OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE FUNCTION [dbo].[CheckActorPackageRights]
(
	@ActorID int,
	@PackageID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1 OR @PackageID IS NULL
RETURN 1

-- check if this is a 'system' package
IF @PackageID < 2 AND @PackageID > -1 AND dbo.CheckIsUserAdmin(@ActorID) = 0
RETURN 0

-- get package owner
DECLARE @UserID int
SELECT @UserID = UserID FROM Packages
WHERE PackageID = @PackageID

IF @UserID IS NULL
RETURN 1 -- unexisting package

-- check user
RETURN dbo.CheckActorUserRights(@ActorID, @UserID)

RETURN 0
END


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceItems](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[PackageID] [int] NULL,
	[ItemTypeID] [int] NULL,
	[ServiceID] [int] NULL,
	[ItemName] [nvarchar](500) COLLATE Latin1_General_CI_AS NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_ServiceItems] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[GetExchangeAccountsPaged]
(
	@ActorID int,
	@ItemID int,
	@AccountTypes nvarchar(30),
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS

DECLARE @PackageID int
SELECT @PackageID = PackageID FROM ServiceItems
WHERE ItemID = @ItemID

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
EA.AccountType IN (' + @AccountTypes + ')
AND EA.ItemID = @ItemID
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
BEGIN
	IF @FilterColumn = 'PrimaryEmailAddress' AND @AccountTypes <> '2'
	BEGIN		
		SET @condition = @condition + ' AND EA.AccountID IN (SELECT EAEA.AccountID FROM ExchangeAccountEmailAddresses EAEA WHERE EAEA.EmailAddress LIKE ''' + @FilterValue + ''')'
	END
	ELSE
	BEGIN		
		SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''
	END
END

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'EA.DisplayName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(EA.AccountID) FROM ExchangeAccounts AS EA
WHERE ' + @condition + ';

WITH Accounts AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		EA.AccountID,
		EA.ItemID,
		EA.AccountType,
		EA.AccountName,
		EA.DisplayName,
		EA.PrimaryEmailAddress,
		EA.MailEnabledPublicFolder
	FROM ExchangeAccounts AS EA
	WHERE ' + @condition + '
)

SELECT * FROM Accounts
WHERE Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows
'

print @sql

exec sp_executesql @sql, N'@ItemID int, @StartRow int, @MaximumRows int',
@ItemID, @StartRow, @MaximumRows

RETURN 







GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO











CREATE PROCEDURE [dbo].[GetDomainsPaged]
(
	@ActorID int,
	@PackageID int,
	@ServerID int,
	@Recursive bit,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS
SET NOCOUNT ON

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

IF @SortColumn = '' OR @SortColumn IS NULL
SET @SortColumn = 'DomainName'

SET @sql = '
DECLARE @Domains TABLE
(
	ItemPosition int IDENTITY(1,1),
	DomainID int
)
INSERT INTO @Domains (DomainID)
SELECT
	D.DomainID
FROM Domains AS D
INNER JOIN Packages AS P ON D.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
LEFT OUTER JOIN Services AS S ON Z.ServiceID = S.ServiceID
LEFT OUTER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
WHERE D.IsInstantAlias = 0 AND
		((@Recursive = 0 AND D.PackageID = @PackageID)
		OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, D.PackageID) = 1))
AND (@ServerID = 0 OR (@ServerID > 0 AND S.ServerID = @ServerID))
'

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE @FilterValue '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(DomainID) FROM @Domains;SELECT
	D.DomainID,
	D.PackageID,
	D.ZoneItemID,
	D.DomainName,
	D.HostingAllowed,
	ISNULL(WS.ItemID, 0) AS WebSiteID,
	WS.ItemName AS WebSiteName,
	ISNULL(MD.ItemID, 0) AS MailDomainID,
	MD.ItemName AS MailDomainName,
	D.IsSubDomain,
	D.IsInstantAlias,
	D.IsDomainPointer,
	
	-- packages
	P.PackageName,
	
	-- server
	ISNULL(SRV.ServerID, 0) AS ServerID,
	ISNULL(SRV.ServerName, '''') AS ServerName,
	ISNULL(SRV.Comments, '''') AS ServerComments,
	ISNULL(SRV.VirtualServer, 0) AS VirtualServer,
	
	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM @Domains AS SD
INNER JOIN Domains AS D ON SD.DomainID = D.DomainID
INNER JOIN Packages AS P ON D.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
LEFT OUTER JOIN Services AS S ON Z.ServiceID = S.ServiceID
LEFT OUTER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
WHERE SD.ItemPosition BETWEEN @StartRow + 1 AND @StartRow + @MaximumRows'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @PackageID int, @FilterValue nvarchar(50), @ServerID int, @Recursive bit',
@StartRow, @MaximumRows, @PackageID, @FilterValue, @ServerID, @Recursive


RETURN












GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Domains](
	[DomainID] [int] IDENTITY(1,1) NOT NULL,
	[PackageID] [int] NOT NULL,
	[ZoneItemID] [int] NULL,
	[DomainName] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[HostingAllowed] [bit] NOT NULL,
	[WebSiteID] [int] NULL,
	[MailDomainID] [int] NULL,
	[IsSubDomain] [bit] NOT NULL,
	[IsInstantAlias] [bit] NOT NULL,
	[IsDomainPointer] [bit] NOT NULL,
 CONSTRAINT [PK_Domains] PRIMARY KEY CLUSTERED 
(
	[DomainID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

















CREATE PROCEDURE [dbo].[DeleteOrganizationUsers]
	@ItemID int 
AS
BEGIN
	SET NOCOUNT ON;

    DELETE FROM ExchangeAccounts WHERE ItemID = @ItemID
END





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


























CREATE PROCEDURE [dbo].[AddExchangeAccount] 
(
	@AccountID int OUTPUT,
	@ItemID int,
	@AccountType int,
	@AccountName nvarchar(20),
	@DisplayName nvarchar(300),
	@PrimaryEmailAddress nvarchar(300),
	@MailEnabledPublicFolder bit,
	@MailboxManagerActions varchar(200),
	@SamAccountName nvarchar(100),
	@AccountPassword nvarchar(200) 
)
AS

INSERT INTO ExchangeAccounts
(
	ItemID,
	AccountType,
	AccountName,
	DisplayName,
	PrimaryEmailAddress,
	MailEnabledPublicFolder,
	MailboxManagerActions,
	SamAccountName,
	AccountPassword
)
VALUES
(
	@ItemID,
	@AccountType,
	@AccountName,
	@DisplayName,
	@PrimaryEmailAddress,
	@MailEnabledPublicFolder,
	@MailboxManagerActions,
	@SamAccountName,
	@AccountPassword
)

SET @AccountID = SCOPE_IDENTITY()

RETURN






























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO











CREATE PROCEDURE AddDomain 
(
	@DomainID int OUTPUT,
	@ActorID int,
	@PackageID int,
	@ZoneItemID int,
	@DomainName nvarchar(200),
	@HostingAllowed bit,
	@WebSiteID int,
	@MailDomainID int,
	@IsSubDomain bit,
	@IsInstantAlias bit,
	@IsDomainPointer bit
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

IF @ZoneItemID = 0 SET @ZoneItemID = NULL
IF @WebSiteID = 0 SET @WebSiteID = NULL
IF @MailDomainID = 0 SET @MailDomainID = NULL

-- insert record
INSERT INTO Domains
(
	PackageID,
	ZoneItemID,
	DomainName,
	HostingAllowed,
	WebSiteID,
	MailDomainID,
	IsSubDomain,
	IsInstantAlias,
	IsDomainPointer
)
VALUES
(
	@PackageID,
	@ZoneItemID,
	@DomainName,
	@HostingAllowed,
	@WebSiteID,
	@MailDomainID,
	@IsSubDomain,
	@IsInstantAlias,
	@IsDomainPointer
)

SET @DomainID = SCOPE_IDENTITY()
RETURN












GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedule](
	[ScheduleID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[PackageID] [int] NULL,
	[ScheduleName] [nvarchar](100) COLLATE Latin1_General_CI_AS NULL,
	[ScheduleTypeID] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[Interval] [int] NULL,
	[FromTime] [datetime] NULL,
	[ToTime] [datetime] NULL,
	[StartTime] [datetime] NULL,
	[LastRun] [datetime] NULL,
	[NextRun] [datetime] NULL,
	[Enabled] [bit] NOT NULL,
	[PriorityID] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[HistoriesNumber] [int] NULL,
	[MaxExecutionTime] [int] NULL,
	[WeekMonthDay] [int] NULL,
 CONSTRAINT [PK_Schedule] PRIMARY KEY CLUSTERED 
(
	[ScheduleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET IDENTITY_INSERT [dbo].[Schedule] ON 

GO
INSERT [dbo].[Schedule] ([ScheduleID], [TaskID], [PackageID], [ScheduleName], [ScheduleTypeID], [Interval], [FromTime], [ToTime], [StartTime], [LastRun], [NextRun], [Enabled], [PriorityID], [HistoriesNumber], [MaxExecutionTime], [WeekMonthDay]) VALUES (1, N'SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE', 1, N'Calculate Disk Space', N'Daily', 0, CAST(0x00008EAC00C5C100 AS DateTime), CAST(0x00008EAC00C5C100 AS DateTime), CAST(0x00008EAC00CDFE60 AS DateTime), NULL, CAST(0x00009DB500F547F5 AS DateTime), 1, N'Normal', 7, 3600, 1)
GO
INSERT [dbo].[Schedule] ([ScheduleID], [TaskID], [PackageID], [ScheduleName], [ScheduleTypeID], [Interval], [FromTime], [ToTime], [StartTime], [LastRun], [NextRun], [Enabled], [PriorityID], [HistoriesNumber], [MaxExecutionTime], [WeekMonthDay]) VALUES (2, N'SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH', 1, N'Calculate Bandwidth', N'Daily', 0, CAST(0x00008EAC00C5C100 AS DateTime), CAST(0x00008EAC00C5C100 AS DateTime), CAST(0x00008EAC00C5C100 AS DateTime), NULL, CAST(0x00009DB500F547F7 AS DateTime), 1, N'Normal', 7, 3600, 1)
GO
SET IDENTITY_INSERT [dbo].[Schedule] OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE AddSchedule
(
	@ActorID int,
	@ScheduleID int OUTPUT,
	@TaskID nvarchar(100),
	@PackageID int,
	@ScheduleName nvarchar(100),
	@ScheduleTypeID nvarchar(50),
	@Interval int,
	@FromTime datetime,
	@ToTime datetime,
	@StartTime datetime,
	@NextRun datetime,
	@Enabled bit,
	@PriorityID nvarchar(50),
	@HistoriesNumber int,
	@MaxExecutionTime int,
	@WeekMonthDay int,
	@XmlParameters ntext
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- insert record
BEGIN TRAN
INSERT INTO Schedule
(
	TaskID,
	PackageID,
	ScheduleName,
	ScheduleTypeID,
	Interval,
	FromTime,
	ToTime,
	StartTime,
	NextRun,
	Enabled,
	PriorityID,
	HistoriesNumber,
	MaxExecutionTime,
	WeekMonthDay
)
VALUES
(
	@TaskID,
	@PackageID,
	@ScheduleName,
	@ScheduleTypeID,
	@Interval,
	@FromTime,
	@ToTime,
	@StartTime,
	@NextRun,
	@Enabled,
	@PriorityID,
	@HistoriesNumber,
	@MaxExecutionTime,
	@WeekMonthDay
)

SET @ScheduleID = SCOPE_IDENTITY()

DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @XmlParameters

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM ScheduleParameters
WHERE ScheduleID = @ScheduleID

INSERT INTO ScheduleParameters
(
	ScheduleID,
	ParameterID,
	ParameterValue
)
SELECT
	@ScheduleID,
	ParameterID,
	ParameterValue
FROM OPENXML(@idoc, '/parameters/parameter',1) WITH 
(
	ParameterID nvarchar(50) '@id',
	ParameterValue nvarchar(3000) '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN

RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE CheckDomain
(
	@PackageID int,
	@DomainName nvarchar(100),
	@Result int OUTPUT
)
AS

/*
@Result values:
	0 - OK
	-1 - already exists
	-2 - sub-domain of prohibited domain
*/

SET @Result = 0 -- OK

-- check if the domain already exists
IF EXISTS(
SELECT DomainID FROM Domains
WHERE DomainName = @DomainName
)
BEGIN
	SET @Result = -1
	RETURN
END

-- check if this is a sub-domain of other domain
-- that is not allowed for 3rd level hosting

DECLARE @UserID int
SELECT @UserID = UserID FROM Packages
WHERE PackageID = @PackageID

-- find sub-domains
DECLARE @DomainUserID int, @HostingAllowed bit
SELECT
	@DomainUserID = P.UserID,
	@HostingAllowed = D.HostingAllowed
FROM Domains AS D
INNER JOIN Packages AS P ON D.PackageID = P.PackageID
WHERE CHARINDEX('.' + DomainName, @DomainName) > 0
AND (CHARINDEX('.' + DomainName, @DomainName) + LEN('.' + DomainName)) = LEN(@DomainName) + 1

-- this is a domain of other user
IF @UserID <> @DomainUserID AND @HostingAllowed = 0
BEGIN
	SET @Result = -2
	RETURN
END

RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetSchedulesPaged
(
	@ActorID int,
	@PackageID int,
	@Recursive bit,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS
BEGIN

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @condition nvarchar(400)
SET @condition = ' 1 = 1 '

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'S.ScheduleName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(S.ScheduleID) FROM Schedule AS S
INNER JOIN PackagesTree(@PackageID, @Recursive) AS PT ON S.PackageID = PT.PackageID
INNER JOIN Packages AS P ON S.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
WHERE ' + @condition + '

DECLARE @Schedules AS TABLE
(
	ScheduleID int
);

WITH TempSchedules AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		S.ScheduleID
	FROM Schedule AS S
	INNER JOIN Packages AS P ON S.PackageID = P.PackageID
	INNER JOIN PackagesTree(@PackageID, @Recursive) AS PT ON S.PackageID = PT.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	WHERE ' + @condition + '
)

INSERT INTO @Schedules
SELECT ScheduleID FROM TempSchedules
WHERE TempSchedules.Row BETWEEN @StartRow and @StartRow + @MaximumRows - 1

SELECT
	S.ScheduleID,
	S.TaskID,
	ST.TaskType,
	ST.RoleID,
	S.ScheduleName,
	S.ScheduleTypeID,
	S.Interval,
	S.FromTime,
	S.ToTime,
	S.StartTime,
	S.LastRun,
	S.NextRun,
	S.Enabled,
	1 AS StatusID,
	S.PriorityID,
	S.MaxExecutionTime,
	S.WeekMonthDay,
	ISNULL(0, (SELECT TOP 1 SeverityID FROM AuditLog WHERE ItemID = S.ScheduleID AND SourceName = ''SCHEDULER'' ORDER BY StartDate DESC)) AS LastResult,

	-- packages
	P.PackageID,
	P.PackageName,
	
	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM @Schedules AS STEMP
INNER JOIN Schedule AS S ON STEMP.ScheduleID = S.ScheduleID
INNER JOIN ScheduleTasks AS ST ON S.TaskID = ST.TaskID
INNER JOIN Packages AS P ON S.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID'

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int, @Recursive bit',
@PackageID, @StartRow, @MaximumRows, @Recursive

END
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE UpdatePackageBandwidthUpdate
(
	@PackageID int,
	@UpdateDate datetime
)
AS

UPDATE Packages SET BandwidthUpdated = @UpdateDate
WHERE PackageID = @PackageID

RETURN 































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrivateIPAddresses](
	[PrivateAddressID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[IPAddress] [varchar](15) COLLATE Latin1_General_CI_AS NOT NULL,
	[IsPrimary] [bit] NOT NULL,
 CONSTRAINT [PK_PrivateIPAddresses] PRIMARY KEY CLUSTERED 
(
	[PrivateAddressID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[GetPackagePrivateIPAddresses]
	@PackageID int
AS
BEGIN

	SELECT
		PA.PrivateAddressID,
		PA.IPAddress,
		PA.ItemID,
		SI.ItemName,
		PA.IsPrimary
	FROM PrivateIPAddresses AS PA
	INNER JOIN ServiceItems AS SI ON PA.ItemID = SI.ItemID
	WHERE SI.PackageID = @PackageID

END


















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[GetItemPrivateIPAddresses]
(
	@ActorID int,
	@ItemID int
)
AS

SELECT
	PIP.PrivateAddressID AS AddressID,
	PIP.IPAddress,
	PIP.IsPrimary
FROM PrivateIPAddresses AS PIP
INNER JOIN ServiceItems AS SI ON PIP.ItemID = SI.ItemID
WHERE PIP.ItemID = @ItemID
AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
ORDER BY PIP.IsPrimary DESC

RETURN





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackagesTreeCache](
	[ParentPackageID] [int] NOT NULL,
	[PackageID] [int] NOT NULL
)

GO
INSERT [dbo].[PackagesTreeCache] ([ParentPackageID], [PackageID]) VALUES (1, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE FUNCTION dbo.PackagesTree
(
	@PackageID int,
	@Recursive bit = 0
)
RETURNS @T TABLE (PackageID int)
AS
BEGIN

INSERT INTO @T VALUES (@PackageID)

IF @Recursive = 1
BEGIN
	WITH RecursivePackages(ParentPackageID, PackageID, PackageLevel) AS 
	(
		SELECT ParentPackageID, PackageID, 0 AS PackageLevel
		FROM Packages
		WHERE ParentPackageID = @PackageID
		UNION ALL
		SELECT p.ParentPackageID, p.PackageID, PackageLevel + 1
		FROM Packages p
			INNER JOIN RecursivePackages d
			ON p.ParentPackageID = d.PackageID
		WHERE @Recursive = 1
	)
	INSERT INTO @T
	SELECT PackageID
	FROM RecursivePackages
END
	
RETURN
END



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExchangeAccountEmailAddresses](
	[AddressID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[EmailAddress] [nvarchar](300) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_ExchangeAccountEmailAddresses] PRIMARY KEY CLUSTERED 
(
	[AddressID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [IX_ExchangeAccountEmailAddresses_UniqueEmail] UNIQUE NONCLUSTERED 
(
	[EmailAddress] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetExchangeAccountEmailAddresses
(
	@AccountID int
)
AS
SELECT
	AddressID,
	AccountID,
	EmailAddress
FROM
	ExchangeAccountEmailAddresses
WHERE
	AccountID = @AccountID
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ExchangeAccountEmailAddressExists]
(
	@EmailAddress nvarchar(300),
	@Exists bit OUTPUT
)
AS

	SET @Exists = 0
	IF EXISTS(SELECT * FROM [dbo].[ExchangeAccountEmailAddresses] WHERE [EmailAddress] = @EmailAddress)
		BEGIN
			SET @Exists = 1
		END
	ELSE IF EXISTS(SELECT * FROM [dbo].[ExchangeAccounts] WHERE [PrimaryEmailAddress] = @EmailAddress AND [AccountType] <> 2)
		BEGIN
			SET @Exists = 1
		END

	RETURN

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






















-- =============================================
-- Description:	Delete user email addresses except primary email
-- =============================================
CREATE PROCEDURE [dbo].[DeleteUserEmailAddresses] 
	@AccountId int,
	@PrimaryEmailAddress nvarchar(300)
AS
BEGIN
	
DELETE FROM 
	ExchangeAccountEmailAddresses 
WHERE
	AccountID = @AccountID AND LOWER(EmailAddress) <> LOWER(@PrimaryEmailAddress)
END


























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE AddExchangeAccountEmailAddress
(
	@AccountID int,
	@EmailAddress nvarchar(300)
)
AS
INSERT INTO ExchangeAccountEmailAddresses
(
	AccountID,
	EmailAddress
)
VALUES
(
	@AccountID,
	@EmailAddress
)
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetPackagesDiskspacePaged
(
	@ActorID int,
	@UserID int,
	@PackageID int,
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @sql nvarchar(4000)

SET @sql = '
DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows

DECLARE @Report TABLE
(
	ItemPosition int IDENTITY(0,1),
	PackageID int,
	QuotaValue int,
	Diskspace int,
	UsagePercentage int,
	PackagesNumber int
)

INSERT INTO @Report (PackageID, QuotaValue, Diskspace, UsagePercentage, PackagesNumber)
SELECT
	P.PackageID,
	PD.QuotaValue,
	PD.Diskspace,
	UsagePercentage = 	CASE
							WHEN PD.QuotaValue = -1 THEN 0
							WHEN PD.QuotaValue <> 0 THEN PD.Diskspace * 100 / PD.QuotaValue
							ELSE 0
						END,
	(SELECT COUNT(NP.PackageID) FROM Packages AS NP WHERE NP.ParentPackageID = P.PackageID) AS PackagesNumber
FROM Packages AS P
LEFT OUTER JOIN
(
	SELECT
		P.PackageID,
		dbo.GetPackageAllocatedQuota(P.PackageID, 52) AS QuotaValue, -- diskspace
		ROUND(CONVERT(float, SUM(ISNULL(PD.DiskSpace, 0))) / 1024 / 1024, 0) AS Diskspace -- in megabytes
	FROM Packages AS P
	INNER JOIN PackagesTreeCache AS PT ON P.PackageID = PT.ParentPackageID
	INNER JOIN Packages AS PC ON PT.PackageID = PC.PackageID
	INNER JOIN PackagesDiskspace AS PD ON PT.PackageID = PD.PackageID
	INNER JOIN HostingPlanResources AS HPR ON PD.GroupID = HPR.GroupID
		AND HPR.PlanID = PC.PlanID
	WHERE HPR.CalculateDiskspace = 1
	GROUP BY P.PackageID
) AS PD ON P.PackageID = PD.PackageID
WHERE (@PackageID = -1 AND P.UserID = @UserID) OR
	(@PackageID <> -1 AND P.ParentPackageID = @PackageID)
'

IF @SortColumn = '' OR @SortColumn IS NULL
SET @SortColumn = 'UsagePercentage DESC'

SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + '
SELECT COUNT(PackageID) FROM @Report

SELECT
	R.PackageID,
	ISNULL(R.QuotaValue, 0) AS QuotaValue,
	ISNULL(R.Diskspace, 0) AS Diskspace,
	ISNULL(R.UsagePercentage, 0) AS UsagePercentage,
	
	-- package
	P.PackageName,
	ISNULL(R.PackagesNumber, 0) AS PackagesNumber,
	P.StatusID,
	
	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email,
	dbo.GetItemComments(U.UserID, ''USER'', @ActorID) AS UserComments
FROM @Report AS R
INNER JOIN Packages AS P ON R.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
WHERE R.ItemPosition BETWEEN @StartRow AND @EndRow
'

exec sp_executesql @sql, N'@ActorID int, @UserID int, @PackageID int, @StartRow int, @MaximumRows int',
@ActorID, @UserID, @PackageID, @StartRow, @MaximumRows

RETURN 

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetPackagesBandwidthPaged
(
	@ActorID int,
	@UserID int,
	@PackageID int,
	@StartDate datetime,
	@EndDate datetime,
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @sql nvarchar(4000)

SET @sql = '
DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows

DECLARE @Report TABLE
(
	ItemPosition int IDENTITY(0,1),
	PackageID int,
	QuotaValue int,
	Bandwidth int,
	UsagePercentage int,
	PackagesNumber int
)

INSERT INTO @Report (PackageID, QuotaValue, Bandwidth, UsagePercentage, PackagesNumber)
SELECT
	P.PackageID,
	PB.QuotaValue,
	PB.Bandwidth,
	UsagePercentage = 	CASE
							WHEN PB.QuotaValue = -1 THEN 0
							WHEN PB.QuotaValue <> 0 THEN PB.Bandwidth * 100 / PB.QuotaValue
							ELSE 0
						END,
	(SELECT COUNT(NP.PackageID) FROM Packages AS NP WHERE NP.ParentPackageID = P.PackageID) AS PackagesNumber
FROM Packages AS P
LEFT OUTER JOIN
(
	SELECT
		P.PackageID,
		dbo.GetPackageAllocatedQuota(P.PackageID, 51) AS QuotaValue, -- bandwidth
		ROUND(CONVERT(float, SUM(ISNULL(PB.BytesSent + PB.BytesReceived, 0))) / 1024 / 1024, 0) AS Bandwidth -- in megabytes
	FROM Packages AS P
	INNER JOIN PackagesTreeCache AS PT ON P.PackageID = PT.ParentPackageID
	INNER JOIN Packages AS PC ON PT.PackageID = PC.PackageID
	INNER JOIN PackagesBandwidth AS PB ON PT.PackageID = PB.PackageID
	INNER JOIN HostingPlanResources AS HPR ON PB.GroupID = HPR.GroupID
		AND HPR.PlanID = PC.PlanID
	WHERE PB.LogDate BETWEEN @StartDate AND @EndDate
		AND HPR.CalculateBandwidth = 1
	GROUP BY P.PackageID
) AS PB ON P.PackageID = PB.PackageID
WHERE (@PackageID = -1 AND P.UserID = @UserID) OR
	(@PackageID <> -1 AND P.ParentPackageID = @PackageID) '

IF @SortColumn = '' OR @SortColumn IS NULL
SET @SortColumn = 'UsagePercentage DESC'

SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + '
SELECT COUNT(PackageID) FROM @Report

SELECT
	R.PackageID,
	ISNULL(R.QuotaValue, 0) AS QuotaValue,
	ISNULL(R.Bandwidth, 0) AS Bandwidth,
	ISNULL(R.UsagePercentage, 0) AS UsagePercentage,
	
	-- package
	P.PackageName,
	ISNULL(R.PackagesNumber, 0) AS PackagesNumber,
	P.StatusID,
	
	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email,
	dbo.GetItemComments(U.UserID, ''USER'', @ActorID) AS UserComments
FROM @Report AS R
INNER JOIN Packages AS P ON R.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
WHERE R.ItemPosition BETWEEN @StartRow AND @EndRow
'

exec sp_executesql @sql, N'@ActorID int, @UserID int, @PackageID int, @StartDate datetime, @EndDate datetime, @StartRow int, @MaximumRows int',
@ActorID, @UserID, @PackageID, @StartDate, @EndDate, @StartRow, @MaximumRows

RETURN 































GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE FUNCTION dbo.PackageParents
(
	@PackageID int
)
RETURNS @T TABLE (PackageOrder int IDENTITY(1,1), PackageID int)
AS
BEGIN
	-- insert current user
	INSERT @T VALUES (@PackageID)

	-- owner
	DECLARE @ParentPackageID int, @TmpPackageID int
	SET @TmpPackageID = @PackageID
	
	WHILE 10 = 10
	BEGIN
	
		SET @ParentPackageID = NULL --reset var
		SELECT @ParentPackageID = ParentPackageID FROM Packages
		WHERE PackageID = @TmpPackageID
		
		IF @ParentPackageID IS NULL -- parent not found
		BREAK
	
		INSERT @T VALUES (@ParentPackageID)
		
		SET @TmpPackageID = @ParentPackageID
	END

RETURN
END




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageIPAddresses](
	[PackageAddressID] [int] IDENTITY(1,1) NOT NULL,
	[PackageID] [int] NOT NULL,
	[AddressID] [int] NOT NULL,
	[ItemID] [int] NULL,
	[IsPrimary] [bit] NULL,
 CONSTRAINT [PK_PackageIPAddresses] PRIMARY KEY CLUSTERED 
(
	[PackageAddressID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[AddItemIPAddress]
(
	@ActorID int,
	@ItemID int,
	@PackageAddressID int
)
AS
BEGIN
	UPDATE PackageIPAddresses
	SET
		ItemID = @ItemID,
		IsPrimary = 0
	FROM PackageIPAddresses AS PIP
	WHERE
		PIP.PackageAddressID = @PackageAddressID
		AND dbo.CheckActorPackageRights(@ActorID, PIP.PackageID) = 1
END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageAddons](
	[PackageAddonID] [int] IDENTITY(1,1) NOT NULL,
	[PackageID] [int] NULL,
	[PlanID] [int] NULL,
	[Quantity] [int] NULL,
	[PurchaseDate] [datetime] NULL,
	[Comments] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[StatusID] [int] NULL,
 CONSTRAINT [PK_PackageAddons] PRIMARY KEY CLUSTERED 
(
	[PackageAddonID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE GetPackageAddons
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	PA.PackageAddonID,
	PA.PackageID,
	PA.PlanID,
	PA.Quantity,
	PA.PurchaseDate,
	PA.StatusID,
	PA.Comments,
	HP.PlanName,
	HP.PlanDescription
FROM PackageAddons AS PA
INNER JOIN HostingPlans AS HP ON PA.PlanID = HP.PlanID
WHERE PA.PackageID = @PackageID
RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE [dbo].[GetPackageAddon]
(
	@ActorID int,
	@PackageAddonID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = @PackageID FROM PackageAddons
WHERE PackageAddonID = @PackageAddonID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	PackageAddonID,
	PackageID,
	PlanID,
	PurchaseDate,
	Quantity,
	StatusID,
	Comments
FROM PackageAddons AS PA
WHERE PA.PackageAddonID = @PackageAddonID
RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HostingPlanQuotas](
	[PlanID] [int] NOT NULL,
	[QuotaID] [int] NOT NULL,
	[QuotaValue] [int] NOT NULL,
 CONSTRAINT [PK_HostingPlanQuotas_1] PRIMARY KEY CLUSTERED 
(
	[PlanID] ASC,
	[QuotaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageQuotas](
	[PackageID] [int] NOT NULL,
	[QuotaID] [int] NOT NULL,
	[QuotaValue] [int] NOT NULL,
 CONSTRAINT [PK_PackageQuotas] PRIMARY KEY CLUSTERED 
(
	[PackageID] ASC,
	[QuotaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Quotas](
	[QuotaID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[QuotaOrder] [int] NOT NULL,
	[QuotaName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[QuotaDescription] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[QuotaTypeID] [int] NOT NULL,
	[ServiceQuota] [bit] NULL,
	[ItemTypeID] [int] NULL,
 CONSTRAINT [PK_Quotas] PRIMARY KEY CLUSTERED 
(
	[QuotaID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (2, 6, 1, N'MySQL4.Databases', N'Databases', 2, 1, 7)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (3, 5, 1, N'MsSQL2000.Databases', N'Databases', 2, 1, 5)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (4, 3, 1, N'FTP.Accounts', N'FTP Accounts', 2, 1, 9)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (11, 9, 2, N'SharePoint.Users', N'SharePoint Users', 2, 0, 1)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (12, 8, 1, N'Stats.Sites', N'Statistics Sites', 2, 1, 14)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (13, 2, 1, N'Web.Sites', N'Web Sites', 2, 1, 10)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (14, 4, 1, N'Mail.Accounts', N'Mail Accounts', 2, 1, 15)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (15, 5, 2, N'MsSQL2000.Users', N'Users', 2, 0, 6)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (18, 4, 3, N'Mail.Forwardings', N'Mail Forwardings', 2, 0, 16)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (19, 6, 2, N'MySQL4.Users', N'Users', 2, 0, 8)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (20, 4, 6, N'Mail.Lists', N'Mail Lists', 2, 0, 17)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (22, 9, 3, N'SharePoint.Groups', N'SharePoint Groups', 2, 0, 3)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (24, 4, 4, N'Mail.Groups', N'Mail Groups', 2, 0, 18)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (25, 2, 3, N'Web.AspNet11', N'ASP.NET 1.1', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (26, 2, 4, N'Web.AspNet20', N'ASP.NET 2.0', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (27, 2, 2, N'Web.Asp', N'ASP', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (28, 2, 5, N'Web.Php4', N'PHP 4.x', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (29, 2, 6, N'Web.Php5', N'PHP 5.x', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (30, 2, 7, N'Web.Perl', N'Perl', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (31, 2, 8, N'Web.Python', N'Python', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (32, 2, 9, N'Web.VirtualDirs', N'Virtual Directories', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (33, 2, 10, N'Web.FrontPage', N'FrontPage', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (34, 2, 11, N'Web.Security', N'Custom Security Settings', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (35, 2, 12, N'Web.DefaultDocs', N'Custom Default Documents', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (36, 2, 13, N'Web.AppPools', N'Dedicated Application Pools', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (37, 2, 14, N'Web.Headers', N'Custom Headers', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (38, 2, 15, N'Web.Errors', N'Custom Errors', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (39, 2, 16, N'Web.Mime', N'Custom MIME Types', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (40, 4, 2, N'Mail.MaxBoxSize', N'Max Mailbox Size', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (41, 5, 3, N'MsSQL2000.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (42, 5, 5, N'MsSQL2000.Backup', N'Database Backups', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (43, 5, 6, N'MsSQL2000.Restore', N'Database Restores', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (44, 5, 7, N'MsSQL2000.Truncate', N'Database Truncate', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (45, 6, 4, N'MySQL4.Backup', N'Database Backups', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (47, 1, 6, N'OS.ODBC', N'ODBC DSNs', 2, 0, 20)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (48, 7, 1, N'DNS.Editor', N'DNS Editor', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (49, 4, 5, N'Mail.MaxGroupMembers', N'Max Group Recipients', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (50, 4, 7, N'Mail.MaxListMembers', N'Max List Recipients', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (51, 1, 2, N'OS.Bandwidth', N'Bandwidth, MB', 2, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (52, 1, 1, N'OS.Diskspace', N'Disk space, MB', 2, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (53, 1, 3, N'OS.Domains', N'Domains', 2, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (54, 1, 4, N'OS.SubDomains', N'Sub-Domains', 2, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (55, 1, 6, N'OS.FileManager', N'File Manager', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (56, 9, 1, N'SharePoint.Sites', N'SharePoint Sites', 2, 0, 19)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (57, 2, 8, N'Web.CgiBin', N'CGI-BIN Folder', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (58, 2, 8, N'Web.SecuredFolders', N'Secured Folders', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (59, 2, 8, N'Web.SharedSSL', N'Shared SSL Folders', 2, 0, 25)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (60, 2, 8, N'Web.Redirections', N'Web Sites Redirection', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (61, 2, 8, N'Web.HomeFolders', N'Changing Sites Root Folders', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (62, 10, 1, N'MsSQL2005.Databases', N'Databases', 2, 0, 21)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (63, 10, 2, N'MsSQL2005.Users', N'Users', 2, 0, 22)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (64, 10, 3, N'MsSQL2005.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (65, 10, 5, N'MsSQL2005.Backup', N'Database Backups', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (66, 10, 6, N'MsSQL2005.Restore', N'Database Restores', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (67, 10, 7, N'MsSQL2005.Truncate', N'Database Truncate', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (68, 11, 1, N'MySQL5.Databases', N'Databases', 2, 0, 23)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (69, 11, 2, N'MySQL5.Users', N'Users', 2, 0, 24)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (70, 11, 3, N'MySQL5.Backup', N'Database Backups', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (71, 1, 9, N'OS.ScheduledTasks', N'Scheduled Tasks', 2, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (72, 1, 10, N'OS.ScheduledIntervalTasks', N'Interval Tasks Allowed', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (73, 1, 11, N'OS.MinimumTaskInterval', N'Minimum Tasks Interval, minutes', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (74, 1, 7, N'OS.AppInstaller', N'Applications Installer', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (75, 1, 8, N'OS.ExtraApplications', N'Extra Application Packs', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (77, 12, 2, N'Exchange2007.DiskSpace', N'Organization Disk Space, MB', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (78, 12, 3, N'Exchange2007.Mailboxes', N'Mailboxes per Organization', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (79, 12, 4, N'Exchange2007.Contacts', N'Contacts per Organization', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (80, 12, 5, N'Exchange2007.DistributionLists', N'Distribution Lists per Organization', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (81, 12, 6, N'Exchange2007.PublicFolders', N'Public Folders per Organization', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (83, 12, 9, N'Exchange2007.POP3Allowed', N'POP3 Access', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (84, 12, 11, N'Exchange2007.IMAPAllowed', N'IMAP Access', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (85, 12, 13, N'Exchange2007.OWAAllowed', N'OWA/HTTP Access', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (86, 12, 15, N'Exchange2007.MAPIAllowed', N'MAPI Access', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (87, 12, 17, N'Exchange2007.ActiveSyncAllowed', N'ActiveSync Access', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (88, 12, 8, N'Exchange2007.MailEnabledPublicFolders', N'Mail Enabled Public Folders Allowed', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (89, 12, 10, N'Exchange2007.POP3Enabled', N'POP3 Enabled by default', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (90, 12, 12, N'Exchange2007.IMAPEnabled', N'IMAP Enabled by default', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (91, 12, 14, N'Exchange2007.OWAEnabled', N'OWA  Enabled by default', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (92, 12, 16, N'Exchange2007.MAPIEnabled', N'MAPI  Enabled by default', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (93, 12, 18, N'Exchange2007.ActiveSyncEnabled', N'ActiveSync Enabled by default', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (94, 2, 17, N'Web.ColdFusion', N'ColdFusion', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (95, 2, 1, N'Web.WebAppGallery', N'Web Application Gallery', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (96, 2, 18, N'Web.CFVirtualDirectories', N'ColdFusion Virtual Directories', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (97, 2, 20, N'Web.RemoteManagement', N'Remote web management allowed', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (100, 2, 19, N'Web.IPAddresses', N'Dedicated IP Addresses', 2, 1, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (102, 4, 8, N'Mail.DisableSizeEdit', N'Disable Mailbox Size Edit', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (200, 20, 1, N'HostedSharePoint.Sites', N'SharePoint Site Collections', 3, 0, 200)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (203, 10, 4, N'MsSQL2005.MaxLogSize', N'Max Log Size', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (204, 5, 4, N'MsSQL2000.MaxLogSize', N'Max Log Size', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (205, 13, 1, N'HostedSolution.Organizations', N'Organizations', 2, 0, 29)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (206, 13, 2, N'HostedSolution.Users', N'Users', 3, 0, 30)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (207, 13, 3, N'HostedSolution.Domains', N'Domains per Organizations', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (208, 20, 2, N'HostedSharePoint.MaxStorage', N'Max site storage, MB', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (209, 21, 2, N'HostedCRM.Users', N'Users', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (210, 21, 1, N'HostedCRM.Organization', N'CRM Organization', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (211, 22, 1, N'MsSQL2008.Databases', N'Databases', 2, 0, 31)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (212, 22, 2, N'MsSQL2008.Users', N'Users', 2, 0, 32)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (213, 22, 3, N'MsSQL2008.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (214, 22, 5, N'MsSQL2008.Backup', N'Database Backups', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (215, 22, 6, N'MsSQL2008.Restore', N'Database Restores', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (216, 22, 7, N'MsSQL2008.Truncate', N'Database Truncate', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (217, 22, 4, N'MsSQL2008.MaxLogSize', N'Max Log Size', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (220, 1, 5, N'OS.DomainPointers', N'Domain Pointers', 2, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (300, 30, 1, N'VPS.ServersNumber', N'Number of VPS', 2, 0, 33)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (301, 30, 2, N'VPS.ManagingAllowed', N'Allow user to create VPS', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (302, 30, 3, N'VPS.CpuNumber', N'Number of CPU cores', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (303, 30, 7, N'VPS.BootCdAllowed', N'Boot from CD allowed', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (304, 30, 8, N'VPS.BootCdEnabled', N'Boot from CD', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (305, 30, 4, N'VPS.Ram', N'RAM size, MB', 2, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (306, 30, 5, N'VPS.Hdd', N'Hard Drive size, GB', 2, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (307, 30, 6, N'VPS.DvdEnabled', N'DVD drive', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (308, 30, 10, N'VPS.ExternalNetworkEnabled', N'External Network', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (309, 30, 11, N'VPS.ExternalIPAddressesNumber', N'Number of External IP addresses', 2, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (310, 30, 13, N'VPS.PrivateNetworkEnabled', N'Private Network', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (311, 30, 14, N'VPS.PrivateIPAddressesNumber', N'Number of Private IP addresses per VPS', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (312, 30, 9, N'VPS.SnapshotsNumber', N'Number of Snaphots', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (313, 30, 15, N'VPS.StartShutdownAllowed', N'Allow user to Start, Turn off and Shutdown VPS', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (314, 30, 16, N'VPS.PauseResumeAllowed', N'Allow user to Pause, Resume VPS', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (315, 30, 17, N'VPS.RebootAllowed', N'Allow user to Reboot VPS', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (316, 30, 18, N'VPS.ResetAlowed', N'Allow user to Reset VPS', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (317, 30, 19, N'VPS.ReinstallAllowed', N'Allow user to Re-install VPS', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (318, 30, 12, N'VPS.Bandwidth', N'Monthly bandwidth, GB', 2, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (319, 31, 1, N'BlackBerry.Users', NULL, 2, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (320, 32, 1, N'OCS.Users', NULL, 2, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (321, 32, 2, N'OCS.Federation', NULL, 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (322, 32, 3, N'OCS.FederationByDefault', NULL, 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (323, 32, 4, N'OCS.PublicIMConnectivity', NULL, 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (324, 32, 5, N'OCS.PublicIMConnectivityByDefault', NULL, 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (325, 32, 6, N'OCS.ArchiveIMConversation', NULL, 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (326, 32, 7, N'OCS.ArchiveIMConvervationByDefault', NULL, 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (327, 32, 8, N'OCS.ArchiveFederatedIMConversation', NULL, 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (328, 32, 9, N'OCS.ArchiveFederatedIMConversationByDefault', NULL, 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (329, 32, 10, N'OCS.PresenceAllowed', NULL, 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (330, 32, 10, N'OCS.PresenceAllowedByDefault', NULL, 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (331, 2, 4, N'Web.AspNet40', N'ASP.NET 4.0', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (332, 2, 21, N'Web.SSL', N'SSL', 1, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (340, 33, 1, N'ExchangeHostedEdition.Domains', N'Domains', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (341, 33, 2, N'ExchangeHostedEdition.Mailboxes', N'Mailboxes', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (342, 33, 3, N'ExchangeHostedEdition.Contacts', N'Contacts', 3, 0, NULL)
GO
INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (343, 33, 4, N'ExchangeHostedEdition.DistributionLists', N'Distribution Lists', 3, 0, NULL)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE FUNCTION dbo.GetPackageAllocatedQuota 
(
	@PackageID int,
	@QuotaID int
)
RETURNS int
AS
BEGIN

DECLARE @Result int

DECLARE @QuotaTypeID int
SELECT @QuotaTypeID = QuotaTypeID FROM Quotas
WHERE QuotaID = @QuotaID

IF @QuotaTypeID = 1
	SET @Result = 1 -- enabled
ELSE
	SET @Result = -1 -- unlimited

DECLARE @PID int, @ParentPackageID int
SET @PID = @PackageID

DECLARE @OverrideQuotas bit

WHILE 1 = 1
BEGIN

	DECLARE @QuotaValue int
	
	-- get package info
	SELECT
		@ParentPackageID = ParentPackageID,
		@OverrideQuotas = OverrideQuotas
	FROM Packages WHERE PackageID = @PID

	SET @QuotaValue = NULL

	-- check if this is a root 'System' package
	IF @ParentPackageID IS NULL
	BEGIN
		IF @QuotaTypeID = 1 -- boolean
			SET @QuotaValue = 1 -- enabled
		ELSE IF @QuotaTypeID > 1 -- numeric
			SET @QuotaValue = -1 -- unlimited
	END
	ELSE
	BEGIN
		-- check the current package
		IF @OverrideQuotas = 1
			SELECT @QuotaValue = QuotaValue FROM PackageQuotas WHERE QuotaID = @QuotaID AND PackageID = @PID
		ELSE
			SELECT @QuotaValue = HPQ.QuotaValue FROM Packages AS P
			INNER JOIN HostingPlanQuotas AS HPQ ON P.PlanID = HPQ.PlanID
			WHERE HPQ.QuotaID = @QuotaID AND P.PackageID = @PID
			
		IF @QuotaValue IS NULL
		SET @QuotaValue = 0
			
		-- check package addons
		DECLARE @QuotaAddonValue int
		SELECT
			@QuotaAddonValue = SUM(HPQ.QuotaValue * PA.Quantity)
		FROM PackageAddons AS PA
		INNER JOIN HostingPlanQuotas AS HPQ ON PA.PlanID = HPQ.PlanID
		WHERE PA.PackageID = @PID AND HPQ.QuotaID = @QuotaID AND PA.StatusID = 1 -- active
		
		-- process bool quota
		IF @QuotaAddonValue IS NOT NULL
		BEGIN
			IF @QuotaTypeID = 1
			BEGIN
				IF @QuotaAddonValue > 0 AND @QuotaValue = 0 -- enabled
				SET @QuotaValue = 1
			END
			ELSE
			BEGIN -- numeric quota
				IF @QuotaAddonValue < 0 -- unlimited
					SET @QuotaValue = -1
				ELSE
					SET @QuotaValue = @QuotaValue + @QuotaAddonValue
			END
		END
	END
	
	-- process bool quota
	IF @QuotaTypeID = 1
	BEGIN
		IF @QuotaValue = 0 OR @QuotaValue IS NULL -- disabled
		RETURN 0
	END
	ELSE
	BEGIN -- numeric quota
		IF @QuotaValue = 0 OR @QuotaValue IS NULL -- zero quantity
		RETURN 0
		
		IF (@QuotaValue <> -1 AND @Result = -1) OR (@QuotaValue < @Result AND @QuotaValue <> -1)
			SET @Result = @QuotaValue
	END
	
	IF @ParentPackageID IS NULL
	RETURN @Result -- exit from the loop
	
	SET @PID = @ParentPackageID

END -- end while

RETURN @Result
END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE FUNCTION dbo.CheckExceedingQuota
(
	@PackageID int,
	@QuotaID int,
	@QuotaTypeID int
)
RETURNS int
AS
BEGIN

DECLARE @ExceedValue int
SET @ExceedValue = 0

DECLARE @PackageQuotaValue int
SET @PackageQuotaValue = dbo.GetPackageAllocatedQuota(@PackageID, @QuotaID)

-- check boolean quota
IF @QuotaTypeID = 1-- AND @PackageQuotaValue > 0 -- enabled
RETURN 0 -- can exceed

-- check numeric quota
IF @QuotaTypeID = 2 AND @PackageQuotaValue = -1 -- unlimited
RETURN 0 -- can exceed

-- get summary usage for the numeric quota
DECLARE @UsedQuantity int
DECLARE @UsedPlans int
DECLARE @UsedOverrides int
DECLARE @UsedAddons int

	-- limited by hosting plans
	SELECT @UsedPlans = SUM(HPQ.QuotaValue) FROM Packages AS P
	INNER JOIN HostingPlanQuotas AS HPQ ON P.PlanID = HPQ.PlanID
	WHERE HPQ.QuotaID = @QuotaID
		AND P.ParentPackageID = @PackageID
		AND P.OverrideQuotas = 0

	-- overrides
	SELECT @UsedOverrides = SUM(PQ.QuotaValue) FROM Packages AS P
	INNER JOIN PackageQuotas AS PQ ON P.PackageID = PQ.PackageID AND PQ.QuotaID = @QuotaID
	WHERE P.ParentPackageID = @PackageID
		AND P.OverrideQuotas = 1
	
	-- addons
	SELECT @UsedAddons = SUM(HPQ.QuotaValue * PA.Quantity)
	FROM Packages AS P
	INNER JOIN PackageAddons AS PA ON P.PackageID = PA.PackageID
	INNER JOIN HostingPlanQuotas AS HPQ ON PA.PlanID = HPQ.PlanID
	WHERE P.ParentPackageID = @PackageID AND HPQ.QuotaID = @QuotaID AND PA.StatusID = 1 -- active

--SET @UsedQuantity = (SELECT SUM(dbo.GetPackageAllocatedQuota(PackageID, @QuotaID)) FROM Packages WHERE ParentPackageID = @PackageID)
	
SET @UsedQuantity = @UsedPlans + @UsedOverrides + @UsedAddons
	
IF @UsedQuantity IS NULL
RETURN 0 -- can exceed

SET @ExceedValue = @UsedQuantity - @PackageQuotaValue

RETURN @ExceedValue
END





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[DeallocatePackageIPAddress]
	@PackageAddressID int
AS
BEGIN

	SET NOCOUNT ON;

	-- check parent package
	DECLARE @ParentPackageID int

	SELECT @ParentPackageID = P.ParentPackageID
	FROM PackageIPAddresses AS PIP
	INNER JOIN Packages AS P ON PIP.PackageID = P.PackageId
	WHERE PIP.PackageAddressID = @PackageAddressID

	IF (@ParentPackageID = 1) -- "System" space
	BEGIN
		DELETE FROM dbo.PackageIPAddresses
		WHERE PackageAddressID = @PackageAddressID
	END
	ELSE -- 2rd level space and below
	BEGIN
		UPDATE PackageIPAddresses
		SET PackageID = @ParentPackageID
		WHERE PackageAddressID = @PackageAddressID
	END

END


















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CRMUsers](
	[CRMUserID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ChangedDate] [datetime] NOT NULL,
	[CRMUserGuid] [uniqueidentifier] NULL,
	[BusinessUnitID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CRMUsers] PRIMARY KEY CLUSTERED 
(
	[CRMUserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



















CREATE PROCEDURE [dbo].[InsertCRMUser]
	@ItemID int,
	@CrmUserID uniqueidentifier,
	@BusinessUnitID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

INSERT INTO
	CRMUsers
(
	AccountID,
	CRMUserGuid,
	BusinessUnitID
)
VALUES 
(
	@ItemID, 
	@CrmUserID,
	@BusinessUnitID
)

	
    
END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[GetCRMUsersCount] 
(
	@ItemID int,
	@Name nvarchar(400),
	@Email nvarchar(400)
	
)
AS

IF (@Name IS NULL)
BEGIN
	SET @Name = '%'
END

IF (@Email IS NULL)
BEGIN
	SET @Email = '%'
END

SELECT 
	COUNT(ea.AccountID)		
FROM 
	ExchangeAccounts ea 
INNER JOIN 
	CRMUsers cu 
ON 
	ea.AccountID = cu.AccountID
WHERE 
	ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email	























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



















CREATE PROCEDURE [dbo].[GetCRMUsers]
(
	@ItemID int,
	@SortColumn nvarchar(40),
	@SortDirection nvarchar(20),
	@Name nvarchar(400),
	@Email nvarchar(400),
	@StartRow int,
	@Count int	
)
AS

IF (@Name IS NULL)
BEGIN
	SET @Name = '%'
END

IF (@Email IS NULL)
BEGIN
	SET @Email = '%'
END

CREATE TABLE #TempCRMUsers 
(	
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int],	
	[ItemID] [int] NOT NULL,
	[AccountName] [nvarchar](20) NOT NULL,
	[DisplayName] [nvarchar](300) NOT NULL,
	[PrimaryEmailAddress] [nvarchar](300) NULL,
	[SamAccountName] [nvarchar](100) NULL	
)


IF (@SortColumn = 'DisplayName')
BEGIN
	INSERT INTO 
		#TempCRMUsers
	SELECT 
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.PrimaryEmailAddress,
		ea.SamAccountName 
	FROM 
		ExchangeAccounts ea 
	INNER JOIN 
		CRMUsers cu 
	ON 
		ea.AccountID = cu.AccountID
	WHERE 
		ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email	
	ORDER BY 
		ea.DisplayName
END
ELSE
BEGIN
	INSERT INTO 
		#TempCRMUsers
	SELECT 
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.PrimaryEmailAddress,
		ea.SamAccountName 
	FROM 
		ExchangeAccounts ea 
	INNER JOIN 
		CRMUsers cu 
	ON 
		ea.AccountID = cu.AccountID
	WHERE 
		ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email	
	ORDER BY 
		ea.PrimaryEmailAddress 
END

DECLARE @RetCount int
SELECT @RetCount = COUNT(ID) FROM #TempCRMUsers

IF (@SortDirection = 'ASC')
BEGIN
	SELECT * FROM #TempCRMUsers
	WHERE ID > @StartRow AND ID <= (@StartRow + @Count) 
END
ELSE
BEGIN
	IF (@SortColumn = 'DisplayName')
	BEGIN
		SELECT * FROM #TempCRMUsers
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY DisplayName DESC
	END
	ELSE
	BEGIN
		SELECT * FROM #TempCRMUsers
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY PrimaryEmailAddress DESC
	END
	
END



DROP TABLE #TempCRMUsers























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[GetCRMUser]
	@AccountID int
AS
BEGIN
	SET NOCOUNT ON;
SELECT
	CRMUserGUID as CRMUserID,
	BusinessUnitID
FROM
	CRMUsers
WHERE
	AccountID = @AccountID		
END






















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

















CREATE PROCEDURE [dbo].[GetCRMOrganizationUsers]
	@ItemID int
AS
BEGIN
	SELECT 
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.PrimaryEmailAddress,
		ea.SamAccountName 
	FROM 
		ExchangeAccounts ea 
	INNER JOIN 
		CRMUsers cu 
	ON 
		ea.AccountID = cu.AccountID
	WHERE 
		ea.ItemID = @ItemID
END




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HostingPlanResources](
	[PlanID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[CalculateDiskSpace] [bit] NULL,
	[CalculateBandwidth] [bit] NULL,
 CONSTRAINT [PK_HostingPlanResources] PRIMARY KEY CLUSTERED 
(
	[PlanID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackagesDiskspace](
	[PackageID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[DiskSpace] [bigint] NOT NULL,
 CONSTRAINT [PK_PackagesDiskspace] PRIMARY KEY CLUSTERED 
(
	[PackageID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE FUNCTION CalculatePackageDiskspace
(
	@PackageID int
)
RETURNS int
AS
BEGIN

DECLARE @Diskspace int

SELECT
	@Diskspace = ROUND(CONVERT(float, SUM(ISNULL(PD.DiskSpace, 0))) / 1024 / 1024, 0) -- in megabytes
FROM PackagesTreeCache AS PT
INNER JOIN Packages AS P ON PT.PackageID = P.PackageID
INNER JOIN PackagesDiskspace AS PD ON P.PackageID = PD.PackageID
INNER JOIN HostingPlanResources AS HPR ON PD.GroupID = HPR.GroupID
	AND HPR.PlanID = P.PlanID AND HPR.CalculateDiskspace = 1
WHERE PT.ParentPackageID = @PackageID

RETURN @Diskspace
END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackagesBandwidth](
	[PackageID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[LogDate] [datetime] NOT NULL,
	[BytesSent] [bigint] NOT NULL,
	[BytesReceived] [bigint] NOT NULL,
 CONSTRAINT [PK_PackagesBandwidth] PRIMARY KEY CLUSTERED 
(
	[PackageID] ASC,
	[GroupID] ASC,
	[LogDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE FUNCTION CalculatePackageBandwidth
(
	@PackageID int
)
RETURNS int
AS
BEGIN

DECLARE @d datetime, @StartDate datetime, @EndDate datetime
SET @d = GETDATE()
SET @StartDate = DATEADD(Day, -DAY(@d) + 1, @d)
SET @EndDate = DATEADD(Day, -1, DATEADD(Month, 1, @StartDate))
--SET @EndDate =  GETDATE()
--SET @StartDate = DATEADD(month, -1, @EndDate)

-- remove hours and minutes
SET @StartDate = CONVERT(datetime, CONVERT(nvarchar, @StartDate, 112))
SET @EndDate = CONVERT(datetime, CONVERT(nvarchar, @EndDate, 112))

DECLARE @Bandwidth int
SELECT
	@Bandwidth = ROUND(CONVERT(float, SUM(ISNULL(PB.BytesSent + PB.BytesReceived, 0))) / 1024 / 1024, 0) -- in megabytes
FROM PackagesTreeCache AS PT
INNER JOIN Packages AS P ON PT.PackageID = P.PackageID
INNER JOIN PackagesBandwidth AS PB ON PT.PackageID = PB.PackageID
INNER JOIN HostingPlanResources AS HPR ON PB.GroupID = HPR.GroupID
	AND HPR.PlanID = P.PlanID AND HPR.CalculateBandwidth = 1
WHERE
	PT.ParentPackageID = @PackageID
	AND PB.LogDate BETWEEN @StartDate AND @EndDate

IF @Bandwidth IS NULL
SET @Bandwidth = 0

RETURN @Bandwidth
END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlackBerryUsers](
	[BlackBerryUserId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_BlackBerryUsers] PRIMARY KEY CLUSTERED 
(
	[BlackBerryUserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[GetBlackBerryUsersCount] 
(
	@ItemID int,
	@Name nvarchar(400),
	@Email nvarchar(400)
	
)
AS

IF (@Name IS NULL)
BEGIN
	SET @Name = '%'
END

IF (@Email IS NULL)
BEGIN
	SET @Email = '%'
END

SELECT 
	COUNT(ea.AccountID)		
FROM 
	ExchangeAccounts ea 
INNER JOIN 
	BlackBerryUsers bu 
ON 
	ea.AccountID = bu.AccountID
WHERE 
	ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email	

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[GetBlackBerryUsers]
(
	@ItemID int,
	@SortColumn nvarchar(40),
	@SortDirection nvarchar(20),
	@Name nvarchar(400),
	@Email nvarchar(400),
	@StartRow int,
	@Count int	
)
AS

IF (@Name IS NULL)
BEGIN
	SET @Name = '%'
END

IF (@Email IS NULL)
BEGIN
	SET @Email = '%'
END

CREATE TABLE #TempBlackBerryUsers 
(	
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int],	
	[ItemID] [int] NOT NULL,
	[AccountName] [nvarchar](20) NOT NULL,
	[DisplayName] [nvarchar](300) NOT NULL,
	[PrimaryEmailAddress] [nvarchar](300) NULL,
	[SamAccountName] [nvarchar](100) NULL	
)


IF (@SortColumn = 'DisplayName')
BEGIN
	INSERT INTO 
		#TempBlackBerryUsers 
	SELECT 
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.PrimaryEmailAddress,
		ea.SamAccountName 
	FROM 
		ExchangeAccounts ea 
	INNER JOIN 
		BlackBerryUsers bu 
	ON 
		ea.AccountID = bu.AccountID
	WHERE 
		ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email	
	ORDER BY 
		ea.DisplayName
END
ELSE
BEGIN
	INSERT INTO 
		#TempBlackBerryUsers
	SELECT 
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ea.PrimaryEmailAddress,
		ea.SamAccountName 
	FROM 
		ExchangeAccounts ea 
	INNER JOIN 
		BlackBerryUsers bu 
	ON 
		ea.AccountID = bu.AccountID
	WHERE 
		ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email	
	ORDER BY 
		ea.PrimaryEmailAddress 
END

DECLARE @RetCount int
SELECT @RetCount = COUNT(ID) FROM #TempBlackBerryUsers 

IF (@SortDirection = 'ASC')
BEGIN
	SELECT * FROM #TempBlackBerryUsers 
	WHERE ID > @StartRow AND ID <= (@StartRow + @Count) 
END
ELSE
BEGIN
	IF (@SortColumn = 'DisplayName')
	BEGIN
		SELECT * FROM #TempBlackBerryUsers 
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY DisplayName DESC
	END
	ELSE
	BEGIN
		SELECT * FROM #TempBlackBerryUsers 
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY PrimaryEmailAddress DESC
	END
	
END


DROP TABLE #TempBlackBerryUsers

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[CheckBlackBerryUserExists] 
	@AccountID int
AS
BEGIN	
	SELECT 
		COUNT(AccountID)
	FROM 
		dbo.BlackBerryUsers
	WHERE AccountID = @AccountID
END
















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO













CREATE PROCEDURE [dbo].[AddBlackBerryUser]	
	@AccountID int
AS
BEGIN	
	SET NOCOUNT ON;

INSERT INTO
	dbo.BlackBerryUsers
	(	 
	 
	 AccountID,
	 CreatedDate,
	 ModifiedDate)
VALUES
(		
	@AccountID,
	getdate(),
	getdate()
)		
END
















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION dbo.GetPackageExceedingQuotas
(
	@PackageID int
)
RETURNS @quotas TABLE (QuotaID int, QuotaName nvarchar(50), QuotaValue int)
AS
BEGIN

DECLARE @ParentPackageID int
DECLARE @PlanID int
DECLARE @OverrideQuotas bit

SELECT
	@ParentPackageID = ParentPackageID,
	@PlanID = PlanID,
	@OverrideQuotas = OverrideQuotas
FROM Packages WHERE PackageID = @PackageID


IF @ParentPackageID IS NOT NULL -- not root package
BEGIN

	IF @OverrideQuotas = 0 -- hosting plan quotas
		BEGIN
			INSERT INTO @quotas (QuotaID, QuotaName, QuotaValue)
			SELECT
				Q.QuotaID,
				Q.QuotaName,
				dbo.CheckExceedingQuota(@PackageID, Q.QuotaID, Q.QuotaTypeID) AS QuotaValue
			FROM HostingPlanQuotas AS HPQ
			INNER JOIN Quotas AS Q ON HPQ.QuotaID = Q.QuotaID
			WHERE HPQ.PlanID = @PlanID AND Q.QuotaTypeID <> 3
		END
	ELSE -- overriden quotas
		BEGIN
			INSERT INTO @quotas (QuotaID, QuotaName, QuotaValue)
			SELECT
				Q.QuotaID,
				Q.QuotaName,
				dbo.CheckExceedingQuota(@PackageID, Q.QuotaID, Q.QuotaTypeID) AS QuotaValue
			FROM PackageQuotas AS PQ
			INNER JOIN Quotas AS Q ON PQ.QuotaID = Q.QuotaID
			WHERE PQ.PackageID = @PackageID AND Q.QuotaTypeID <> 3
		END
END -- if 'root' package

RETURN
END





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE AddPackageAddon
(
	@ActorID int,
	@PackageAddonID int OUTPUT,
	@PackageID int,
	@PlanID int,
	@Quantity int,
	@StatusID int,
	@PurchaseDate datetime,
	@Comments ntext
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

DECLARE @ParentPackageID int
SELECT @ParentPackageID = ParentPackageID FROM Packages
WHERE PackageID = @PackageID

-- insert record
INSERT INTO PackageAddons
(
	PackageID,
	PlanID,
	PurchaseDate,
	Quantity,
	StatusID,
	Comments
)
VALUES
(
	@PackageID,
	@PlanID,
	@PurchaseDate,
	@Quantity,
	@StatusID,
	@Comments
)

SET @PackageAddonID = SCOPE_IDENTITY()

DECLARE @ExceedingQuotas AS TABLE (QuotaID int, QuotaName nvarchar(50), QuotaValue int)
INSERT INTO @ExceedingQuotas
SELECT * FROM dbo.GetPackageExceedingQuotas(@ParentPackageID) WHERE QuotaValue > 0

SELECT * FROM @ExceedingQuotas

IF EXISTS(SELECT * FROM @ExceedingQuotas)
BEGIN
	ROLLBACK TRAN
	RETURN
END

COMMIT TRAN
RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE [dbo].[AddPackage]
(
	@ActorID int,
	@PackageID int OUTPUT,
	@UserID int,
	@PackageName nvarchar(300),
	@PackageComments ntext,
	@StatusID int,
	@PlanID int,
	@PurchaseDate datetime
)
AS


DECLARE @ParentPackageID int, @PlanServerID int
SELECT @ParentPackageID = PackageID, @PlanServerID = ServerID FROM HostingPlans
WHERE PlanID = @PlanID

IF @ParentPackageID = 0 OR @ParentPackageID IS NULL
SELECT @ParentPackageID = PackageID FROM Packages
WHERE ParentPackageID IS NULL -- root space

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @ParentPackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1);
	RETURN;
END

BEGIN TRAN
-- insert package
INSERT INTO Packages
(
	ParentPackageID,
	UserID,
	PackageName,
	PackageComments,
	ServerID,
	StatusID,
	PlanID,
	PurchaseDate
)
VALUES
(
	@ParentPackageID,
	@UserID,
	@PackageName,
	@PackageComments,
	@PlanServerID,
	@StatusID,
	@PlanID,
	@PurchaseDate
)

SET @PackageID = SCOPE_IDENTITY()

-- add package to packages cache
INSERT INTO PackagesTreeCache (ParentPackageID, PackageID)
SELECT PackageID, @PackageID FROM dbo.PackageParents(@PackageID)

DECLARE @ExceedingQuotas AS TABLE (QuotaID int, QuotaName nvarchar(50), QuotaValue int)
INSERT INTO @ExceedingQuotas
SELECT * FROM dbo.GetPackageExceedingQuotas(@ParentPackageID) WHERE QuotaValue > 0

SELECT * FROM @ExceedingQuotas

IF EXISTS(SELECT * FROM @ExceedingQuotas)
BEGIN
	ROLLBACK TRAN
	RETURN
END

COMMIT TRAN

RETURN






































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


























CREATE PROCEDURE [dbo].[ConvertToExchangeOrganization]
(
	@ItemID int	
)
AS

UPDATE 
	[dbo].[ServiceItems]
SET 
	[ItemTypeID] = 26
WHERE 
	[ItemID] = @ItemID

RETURN 






























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[AddItemPrivateIPAddress]
(
	@ActorID int,
	@ItemID int,
	@IPAddress varchar(15)
)
AS


IF EXISTS (SELECT ItemID FROM ServiceItems AS SI WHERE dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1)
BEGIN

	INSERT INTO PrivateIPAddresses
	(
		ItemID,
		IPAddress,
		IsPrimary
	)
	VALUES
	(
		@ItemID,
		@IPAddress,
		0 -- not primary
	)

END

RETURN





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceItemProperties](
	[ItemID] [int] NOT NULL,
	[PropertyName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyValue] [nvarchar](3000) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_ServiceItemProperties] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO













CREATE PROCEDURE [dbo].[DeleteBlackBerryUser]
(	
	@AccountID int
)
AS

DELETE FROM 
	BlackBerryUsers
WHERE 
	AccountID = @AccountID

RETURN 

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[GetVirtualMachinesPaged]
(
	@ActorID int,
	@PackageID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int,
	@Recursive bit
)
AS


-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
SI.ItemTypeID = 33 -- VPS
AND ((@Recursive = 0 AND P.PackageID = @PackageID)
OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, P.PackageID) = 1))
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'SI.ItemName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(SI.ItemID) FROM Packages AS P
INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
INNER JOIN Users AS U ON P.UserID = U.UserID
LEFT OUTER JOIN (
	SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
) AS EIP ON SI.ItemID = EIP.ItemID
LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
WHERE ' + @condition + '

DECLARE @Items AS TABLE
(
	ItemID int
);

WITH TempItems AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		SI.ItemID
	FROM Packages AS P
	INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
	INNER JOIN Users AS U ON P.UserID = U.UserID
	LEFT OUTER JOIN (
		SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
		INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
		WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
	) AS EIP ON SI.ItemID = EIP.ItemID
	LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
	WHERE ' + @condition + '
)

INSERT INTO @Items
SELECT ItemID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	SI.ItemID,
	SI.ItemName,
	SI.PackageID,
	P.PackageName,
	P.UserID,
	U.Username,

	EIP.ExternalIP,
	PIP.IPAddress
FROM @Items AS TSI
INNER JOIN ServiceItems AS SI ON TSI.ItemID = SI.ItemID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Users AS U ON P.UserID = U.UserID
LEFT OUTER JOIN (
	SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
) AS EIP ON SI.ItemID = EIP.ItemID
LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
'

--print @sql

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int, @Recursive bit',
@PackageID, @StartRow, @MaximumRows, @Recursive

RETURN 

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteExchangeAccountEmailAddress
(
	@AccountID int,
	@EmailAddress nvarchar(300)
)
AS
DELETE FROM ExchangeAccountEmailAddresses
WHERE AccountID = @AccountID AND EmailAddress = @EmailAddress
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteExchangeAccount
(
	@ItemID int,
	@AccountID int
)
AS

-- delete e-mail addresses
DELETE FROM ExchangeAccountEmailAddresses
WHERE AccountID = @AccountID

-- delete account
DELETE FROM ExchangeAccounts
WHERE ItemID = @ItemID AND AccountID = @AccountID

RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[DeleteCRMOrganization]
	@ItemID int
AS
BEGIN
	SET NOCOUNT ON
DELETE FROM dbo.CRMUsers WHERE AccountID IN (SELECT AccountID FROM dbo.ExchangeAccounts WHERE ItemID = @ItemID)
END






















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[AllocatePackageIPAddresses]
(
	@PackageID int,
	@xml ntext
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @idoc int
	--Create an internal representation of the XML document.
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

	-- delete
	DELETE FROM PackageIPAddresses
	FROM PackageIPAddresses AS PIP
	INNER JOIN OPENXML(@idoc, '/items/item', 1) WITH 
	(
		AddressID int '@id'
	) as PV ON PIP.AddressID = PV.AddressID


	-- insert
	INSERT INTO dbo.PackageIPAddresses
	(		
		PackageID,
		AddressID	
	)
	SELECT		
		@PackageID,
		AddressID

	FROM OPENXML(@idoc, '/items/item', 1) WITH 
	(
		AddressID int '@id'
	) as PV

	-- remove document
	exec sp_xml_removedocument @idoc

END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE UpdateServiceItem
(
	@ActorID int,
	@ItemID int,
	@ItemName nvarchar(500),
	@XmlProperties ntext
)
AS
BEGIN TRAN

-- check rights
DECLARE @PackageID int
SELECT PackageID = @PackageID FROM ServiceItems
WHERE ItemID = @ItemID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- update item
UPDATE ServiceItems SET ItemName = @ItemName
WHERE ItemID=@ItemID

DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @XmlProperties

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM ServiceItemProperties
WHERE ItemID = @ItemID

INSERT INTO ServiceItemProperties
(
	ItemID,
	PropertyName,
	PropertyValue
)
SELECT
	@ItemID,
	PropertyName,
	PropertyValue
FROM OPENXML(@idoc, '/properties/property',1) WITH 
(
	PropertyName nvarchar(50) '@name',
	PropertyValue nvarchar(3000) '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE UpdateSchedule
(
	@ActorID int,
	@ScheduleID int,
	@TaskID nvarchar(100),
	@ScheduleName nvarchar(100),
	@ScheduleTypeID nvarchar(50),
	@Interval int,
	@FromTime datetime,
	@ToTime datetime,
	@StartTime datetime,
	@LastRun datetime,
	@NextRun datetime,
	@Enabled bit,
	@PriorityID nvarchar(50),
	@HistoriesNumber int,
	@MaxExecutionTime int,
	@WeekMonthDay int,
	@XmlParameters ntext
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM Schedule
WHERE ScheduleID = @ScheduleID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

UPDATE Schedule
SET
	TaskID = @TaskID,
	ScheduleName = @ScheduleName,
	ScheduleTypeID = @ScheduleTypeID,
	Interval = @Interval,
	FromTime = @FromTime,
	ToTime = @ToTime,
	StartTime = @StartTime,
	LastRun = @LastRun,
	NextRun = @NextRun,
	Enabled = @Enabled,
	PriorityID = @PriorityID,
	HistoriesNumber = @HistoriesNumber,
	MaxExecutionTime = @MaxExecutionTime,
	WeekMonthDay = @WeekMonthDay
WHERE
	ScheduleID = @ScheduleID
	
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @XmlParameters

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM ScheduleParameters
WHERE ScheduleID = @ScheduleID

INSERT INTO ScheduleParameters
(
	ScheduleID,
	ParameterID,
	ParameterValue
)
SELECT
	@ScheduleID,
	ParameterID,
	ParameterValue
FROM OPENXML(@idoc, '/parameters/parameter',1) WITH 
(
	ParameterID nvarchar(50) '@id',
	ParameterValue nvarchar(3000) '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageResources](
	[PackageID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[CalculateDiskspace] [bit] NOT NULL,
	[CalculateBandwidth] [bit] NOT NULL,
 CONSTRAINT [PK_PackageResources_1] PRIMARY KEY CLUSTERED 
(
	[PackageID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE UpdatePackageQuotas
(
	@ActorID int,
	@PackageID int,
	@Xml ntext
)
AS

/*
XML Format:

<plan>
	<groups>
		<group id="16" enabled="1" calculateDiskSpace="1" calculateBandwidth="1"/>
	</groups>
	<quotas>
		<quota id="2" value="2"/>
	</quotas>
</plan>

*/

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @OverrideQuotas bit
SELECT @OverrideQuotas = OverrideQuotas FROM Packages
WHERE PackageID = @PackageID

IF @OverrideQuotas = 0
BEGIN
	-- delete old Package resources
	DELETE FROM PackageResources
	WHERE PackageID = @PackageID

	-- delete old Package quotas
	DELETE FROM PackageQuotas
	WHERE PackageID = @PackageID
END

IF @OverrideQuotas = 1 AND @Xml IS NOT NULL
BEGIN
	-- delete old Package resources
	DELETE FROM PackageResources
	WHERE PackageID = @PackageID

	-- delete old Package quotas
	DELETE FROM PackageQuotas
	WHERE PackageID = @PackageID
	
	DECLARE @idoc int
	--Create an internal representation of the XML document.
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

	-- update Package resources
	INSERT INTO PackageResources
	(
		PackageID,
		GroupID,
		CalculateDiskSpace,
		CalculateBandwidth
	)
	SELECT
		@PackageID,
		GroupID,
		CalculateDiskSpace,
		CalculateBandwidth
	FROM OPENXML(@idoc, '/plan/groups/group',1) WITH 
	(
		GroupID int '@id',
		CalculateDiskSpace bit '@calculateDiskSpace',
		CalculateBandwidth bit '@calculateBandwidth'
	) as XRG

	-- update Package quotas
	INSERT INTO PackageQuotas
	(
		PackageID,
		QuotaID,
		QuotaValue
	)
	SELECT
		@PackageID,
		QuotaID,
		QuotaValue
	FROM OPENXML(@idoc, '/plan/quotas/quota',1) WITH 
	(
		QuotaID int '@id',
		QuotaValue int '@value'
	) as PV
	
	-- remove document
	exec sp_xml_removedocument @idoc
END
RETURN 

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE UpdatePackageAddon
(
	@ActorID int,
	@PackageAddonID int,
	@PlanID int,
	@Quantity int,
	@PurchaseDate datetime,
	@StatusID int,
	@Comments ntext
)
AS

DECLARE @PackageID int
SELECT @PackageID = PackageID FROM PackageAddons
WHERE PackageAddonID = @PackageAddonID

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

DECLARE @ParentPackageID int
SELECT @ParentPackageID = ParentPackageID FROM Packages
WHERE PackageID = @PackageID

-- update record
UPDATE PackageAddons SET
	PlanID = @PlanID,
	Quantity = @Quantity,
	PurchaseDate = @PurchaseDate,
	StatusID = @StatusID,
	Comments = @Comments
WHERE PackageAddonID = @PackageAddonID

DECLARE @ExceedingQuotas AS TABLE (QuotaID int, QuotaName nvarchar(50), QuotaValue int)
INSERT INTO @ExceedingQuotas
SELECT * FROM dbo.GetPackageExceedingQuotas(@ParentPackageID) WHERE QuotaValue > 0

SELECT * FROM @ExceedingQuotas

IF EXISTS(SELECT * FROM @ExceedingQuotas)
BEGIN
	ROLLBACK TRAN
	RETURN
END

COMMIT TRAN

RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[UpdatePackage]
(
	@ActorID int,
	@PackageID int,
	@PackageName nvarchar(300),
	@PackageComments ntext,
	@StatusID int,
	@PlanID int,
	@PurchaseDate datetime,
	@OverrideQuotas bit,
	@QuotasXml ntext
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

DECLARE @ParentPackageID int
DECLARE @OldPlanID int

SELECT @ParentPackageID = ParentPackageID, @OldPlanID = PlanID FROM Packages
WHERE PackageID = @PackageID

-- update package
UPDATE Packages SET
	PackageName = @PackageName,
	PackageComments = @PackageComments,
	StatusID = @StatusID,
	PlanID = @PlanID,
	PurchaseDate = @PurchaseDate,
	OverrideQuotas = @OverrideQuotas
WHERE
	PackageID = @PackageID

-- update quotas (if required)
EXEC UpdatePackageQuotas @ActorID, @PackageID, @QuotasXml

-- check resulting quotas
DECLARE @ExceedingQuotas AS TABLE (QuotaID int, QuotaName nvarchar(50), QuotaValue int)

-- check exceeding quotas if plan has been changed
IF (@OldPlanID <> @PlanID) OR (@OverrideQuotas = 1)
BEGIN
	INSERT INTO @ExceedingQuotas
	SELECT * FROM dbo.GetPackageExceedingQuotas(@ParentPackageID) WHERE QuotaValue > 0
END

SELECT * FROM @ExceedingQuotas

IF EXISTS(SELECT * FROM @ExceedingQuotas)
BEGIN
	ROLLBACK TRAN
	RETURN
END


COMMIT TRAN
RETURN





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE UpdateDomain
(
	@DomainID int,
	@ActorID int,
	@ZoneItemID int,
	@HostingAllowed bit,
	@WebSiteID int,
	@MailDomainID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM Domains
WHERE DomainID = @DomainID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

IF @ZoneItemID = 0 SET @ZoneItemID = NULL
IF @WebSiteID = 0 SET @WebSiteID = NULL
IF @MailDomainID = 0 SET @MailDomainID = NULL

-- update record
UPDATE Domains
SET
	ZoneItemID = @ZoneItemID,
	HostingAllowed = @HostingAllowed,
	WebSiteID = @WebSiteID,
	MailDomainID = @MailDomainID
WHERE
	DomainID = @DomainID
	RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[SetItemPrivatePrimaryIPAddress]
(
	@ActorID int,
	@ItemID int,
	@PrivateAddressID int
)
AS
BEGIN
	UPDATE PrivateIPAddresses
	SET IsPrimary = CASE PIP.PrivateAddressID WHEN @PrivateAddressID THEN 1 ELSE 0 END
	FROM PrivateIPAddresses AS PIP
	INNER JOIN ServiceItems AS SI ON PIP.ItemID = SI.ItemID
	WHERE PIP.ItemID = @ItemID
	AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
END


















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO







CREATE PROCEDURE [dbo].[MoveServiceItem]
(
	@ActorID int,
	@ItemID int,
	@DestinationServiceID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT PackageID = @PackageID FROM ServiceItems
WHERE ItemID = @ItemID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

UPDATE ServiceItems
SET ServiceID = @DestinationServiceID
WHERE ItemID = @ItemID

COMMIT TRAN

RETURN 










GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetResellerDomains
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- load parent package
DECLARE @ParentPackageID int
SELECT @ParentPackageID = ParentPackageID FROM Packages
WHERE PackageID = @PackageID

SELECT
	D.DomainID,
	D.PackageID,
	D.ZoneItemID,
	D.DomainName,
	D.HostingAllowed,
	D.WebSiteID,
	WS.ItemName,
	D.MailDomainID,
	MD.ItemName
FROM Domains AS D
INNER JOIN PackagesTree(@ParentPackageID, 0) AS PT ON D.PackageID = PT.PackageID
LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
WHERE HostingAllowed = 1
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetPackageBandwidthUpdate
(
	@PackageID int,
	@UpdateDate datetime OUTPUT
)
AS
	SELECT @UpdateDate = BandwidthUpdated FROM Packages
	WHERE PackageID = @PackageID
RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE [dbo].[GetPackage]
(
	@PackageID int,
	@ActorID int
)
AS

-- Note: ActorID is not verified
-- check both requested and parent package

SELECT
	P.PackageID,
	P.ParentPackageID,
	P.UserID,
	P.PackageName,
	P.PackageComments,
	P.ServerID,
	P.StatusID,
	P.PlanID,
	P.PurchaseDate,
	P.OverrideQuotas
FROM Packages AS P
WHERE P.PackageID = @PackageID
RETURN





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetNestedPackagesSummary
(
	@ActorID int,
	@PackageID int
)
AS
-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- ALL spaces
SELECT COUNT(PackageID) AS PackagesNumber FROM Packages
WHERE ParentPackageID = @PackageID

-- BY STATUS spaces
SELECT StatusID, COUNT(PackageID) AS PackagesNumber FROM Packages
WHERE ParentPackageID = @PackageID AND StatusID > 0
GROUP BY StatusID
ORDER BY StatusID

RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExchangeOrganizations](
	[ItemID] [int] NOT NULL,
	[OrganizationID] [nvarchar](10) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_ExchangeOrganizations] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [IX_ExchangeOrganizations_UniqueOrg] UNIQUE NONCLUSTERED 
(
	[OrganizationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





















CREATE PROCEDURE [dbo].[GetItemIdByOrganizationId] 	
	@OrganizationId nvarchar(10)
AS
BEGIN
	SET NOCOUNT ON;
    
	SELECT 
		ItemID
	FROM 
		dbo.ExchangeOrganizations
	WHERE 
		OrganizationId = @OrganizationId
END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteExchangeOrganization 
(
	@ItemID int
)
AS
DELETE FROM ExchangeOrganizations
WHERE ItemID = @ItemID
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE AddExchangeOrganization
(
	@ItemID int,
	@OrganizationID nvarchar(10)
)
AS

IF NOT EXISTS(SELECT * FROM ExchangeOrganizations WHERE OrganizationID = @OrganizationID)
BEGIN
	INSERT INTO ExchangeOrganizations
	(ItemID, OrganizationID)
	VALUES
	(@ItemID, @OrganizationID)
END

RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE ExchangeOrganizationExists
(
	@OrganizationID nvarchar(10),
	@Exists bit OUTPUT
)
AS
SET @Exists = 0
IF EXISTS(SELECT * FROM ExchangeOrganizations WHERE OrganizationID = @OrganizationID)
BEGIN
	SET @Exists = 1
END

RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExchangeOrganizationDomains](
	[OrganizationDomainID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[DomainID] [int] NULL,
	[IsHost] [bit] NULL,
 CONSTRAINT [PK_ExchangeOrganizationDomains] PRIMARY KEY CLUSTERED 
(
	[OrganizationDomainID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [IX_ExchangeOrganizationDomains_UniqueDomain] UNIQUE NONCLUSTERED 
(
	[DomainID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


























CREATE PROCEDURE [dbo].[GetExchangeOrganizationStatistics] 
(
	@ItemID int
)
AS
SELECT
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 1 OR AccountType = 5 OR AccountType = 6) AND ItemID = @ItemID) AS CreatedMailboxes,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 2 AND ItemID = @ItemID) AS CreatedContacts,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 3 AND ItemID = @ItemID) AS CreatedDistributionLists,
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE AccountType = 4 AND ItemID = @ItemID) AS CreatedPublicFolders,
	(SELECT COUNT(*) FROM ExchangeOrganizationDomains WHERE ItemID = @ItemID) AS CreatedDomains

RETURN





























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetExchangeOrganizationDomains 
(
	@ItemID int
)
AS
SELECT
	ED.DomainID,
	D.DomainName,
	ED.IsHost
FROM
	ExchangeOrganizationDomains AS ED
INNER JOIN Domains AS D ON ED.DomainID = D.DomainID
WHERE ED.ItemID = @ItemID
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteExchangeOrganizationDomain 
(
	@ItemID int,
	@DomainID int
)
AS
DELETE FROM ExchangeOrganizationDomains
WHERE DomainID = @DomainID AND ItemID = @ItemID
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE AddExchangeOrganizationDomain 
(
	@ItemID int,
	@DomainID int,
	@IsHost bit
)
AS
INSERT INTO ExchangeOrganizationDomains
(ItemID, DomainID, IsHost)
VALUES
(@ItemID, @DomainID, @IsHost)
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE ExchangeOrganizationDomainExists 
(
	@DomainID int,
	@Exists bit OUTPUT
)
AS
SET @Exists = 0
IF EXISTS(SELECT * FROM ExchangeOrganizationDomains WHERE DomainID = @DomainID)
BEGIN
	SET @Exists = 1
END
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO













CREATE PROCEDURE [dbo].[UpdateExchangeAccount] 
(
	@AccountID int,
	@AccountName nvarchar(20),
	@DisplayName nvarchar(300),
	@PrimaryEmailAddress nvarchar(300),
	@AccountType int,
	@SamAccountName nvarchar(100),
	@MailEnabledPublicFolder bit,
	@MailboxManagerActions varchar(200),
	@Password varchar(200)
)
AS

BEGIN TRAN	
UPDATE ExchangeAccounts SET
	AccountName = @AccountName,
	DisplayName = @DisplayName,
	PrimaryEmailAddress = @PrimaryEmailAddress,
	MailEnabledPublicFolder = @MailEnabledPublicFolder,
	MailboxManagerActions = @MailboxManagerActions,	
	AccountType =@AccountType,
	SamAccountName = @SamAccountName

WHERE
	AccountID = @AccountID

IF (@@ERROR <> 0 )
	BEGIN
		ROLLBACK TRANSACTION
		RETURN -1
	END

UPDATE ExchangeAccounts SET 
	AccountPassword = @Password WHERE AccountID = @AccountID AND @Password IS NOT NULL

IF (@@ERROR <> 0 )
	BEGIN
		ROLLBACK TRANSACTION
		RETURN -1
	END
COMMIT TRAN
RETURN
















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



























CREATE PROCEDURE [dbo].[OrganizationUserExists] 
(
	@LoginName nvarchar(20),
	@Exists bit OUTPUT
)
AS
SET @Exists = 0
IF EXISTS(SELECT * FROM ExchangeAccounts WHERE AccountName = @LoginName)
BEGIN
	SET @Exists = 1
END

RETURN






























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


























CREATE PROCEDURE [dbo].[GetOrganizationStatistics] 
(
	@ItemID int
)
AS
SELECT
	(SELECT COUNT(*) FROM ExchangeAccounts WHERE (AccountType = 7 OR AccountType = 1 OR AccountType = 6 OR AccountType = 5)  AND ItemID = @ItemID) AS CreatedUsers,
	(SELECT COUNT(*) FROM ExchangeOrganizationDomains WHERE ItemID = @ItemID) AS CreatedDomains
RETURN





























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[GetOrganizationCRMUserCount]
	@ItemID int
AS
BEGIN
SELECT 
 COUNT(CRMUserID) 
FROM 
	CrmUsers CU
INNER JOIN 
	ExchangeAccounts EA
ON 
	CU.AccountID = EA.AccountID
WHERE EA.ItemID = @ItemID
END






















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO











CREATE PROCEDURE GetDomains
(
	@ActorID int,
	@PackageID int,
	@Recursive bit = 1
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	D.DomainID,
	D.PackageID,
	D.ZoneItemID,
	D.DomainName,
	D.HostingAllowed,
	ISNULL(WS.ItemID, 0) AS WebSiteID,
	WS.ItemName AS WebSiteName,
	ISNULL(MD.ItemID, 0) AS MailDomainID,
	MD.ItemName AS MailDomainName,
	Z.ItemName AS ZoneName,
	D.IsSubDomain,
	D.IsInstantAlias,
	D.IsDomainPointer
FROM Domains AS D
INNER JOIN PackagesTree(@PackageID, @Recursive) AS PT ON D.PackageID = PT.PackageID
LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
RETURN












GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO











CREATE PROCEDURE GetDomainByName
(
	@ActorID int,
	@DomainName nvarchar(100)
)
AS

SELECT
	D.DomainID,
	D.PackageID,
	D.ZoneItemID,
	D.DomainName,
	D.HostingAllowed,
	ISNULL(D.WebSiteID, 0) AS WebSiteID,
	WS.ItemName AS WebSiteName,
	ISNULL(D.MailDomainID, 0) AS MailDomainID,
	MD.ItemName AS MailDomainName,
	Z.ItemName AS ZoneName,
	D.IsSubDomain,
	D.IsInstantAlias,
	D.IsDomainPointer
FROM Domains AS D
INNER JOIN Packages AS P ON D.PackageID = P.PackageID
LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
WHERE
	D.DomainName = @DomainName
	AND dbo.CheckActorPackageRights(@ActorID, P.PackageID) = 1
RETURN












GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO











CREATE PROCEDURE GetDomain
(
	@ActorID int,
	@DomainID int
)
AS

SELECT
	D.DomainID,
	D.PackageID,
	D.ZoneItemID,
	D.DomainName,
	D.HostingAllowed,
	ISNULL(WS.ItemID, 0) AS WebSiteID,
	WS.ItemName AS WebSiteName,
	ISNULL(MD.ItemID, 0) AS MailDomainID,
	MD.ItemName AS MailDomainName,
	Z.ItemName AS ZoneName,
	D.IsSubDomain,
	D.IsInstantAlias,
	D.IsDomainPointer
FROM Domains AS D
INNER JOIN Packages AS P ON D.PackageID = P.PackageID
LEFT OUTER JOIN ServiceItems AS WS ON D.WebSiteID = WS.ItemID
LEFT OUTER JOIN ServiceItems AS MD ON D.MailDomainID = MD.ItemID
LEFT OUTER JOIN ServiceItems AS Z ON D.ZoneItemID = Z.ItemID
WHERE
	D.DomainID = @DomainID
	AND dbo.CheckActorPackageRights(@ActorID, P.PackageID) = 1
RETURN












GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteSchedule
(
	@ActorID int,
	@ScheduleID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM Schedule
WHERE ScheduleID = @ScheduleID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN
-- delete schedule parameters
DELETE FROM ScheduleParameters
WHERE ScheduleID = @ScheduleID

-- delete schedule
DELETE FROM Schedule
WHERE ScheduleID = @ScheduleID

COMMIT TRAN

RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE DeletePackageAddon
(
	@ActorID int,
	@PackageAddonID int
)
AS

DECLARE @PackageID int
SELECT @PackageID = PackageID FROM PackageAddons
WHERE PackageAddonID = @PackageAddonID

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- delete record
DELETE FROM PackageAddons
WHERE PackageAddonID = @PackageAddonID

RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE DeleteItemPrivateIPAddresses
(
	@ActorID int,
	@ItemID int
)
AS
BEGIN
	DELETE FROM PrivateIPAddresses
	FROM PrivateIPAddresses AS PIP
	INNER JOIN ServiceItems AS SI ON PIP.ItemID = SI.ItemID
	WHERE PIP.ItemID = @ItemID
	AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
END





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE DeleteItemPrivateIPAddress
(
	@ActorID int,
	@ItemID int,
	@PrivateAddressID int
)
AS
BEGIN
	DELETE FROM PrivateIPAddresses
	FROM PrivateIPAddresses AS PIP
	INNER JOIN ServiceItems AS SI ON PIP.ItemID = SI.ItemID
	WHERE PIP.PrivateAddressID = @PrivateAddressID
	AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
END


















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[DeleteItemIPAddresses]
(
	@ActorID int,
	@ItemID int
)
AS
BEGIN
	UPDATE PackageIPAddresses
	SET
		ItemID = NULL,
		IsPrimary = 0
	FROM PackageIPAddresses AS PIP
	WHERE
		PIP.ItemID = @ItemID
		AND dbo.CheckActorPackageRights(@ActorID, PIP.PackageID) = 1
END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[DeleteItemIPAddress]
(
	@ActorID int,
	@ItemID int,
	@PackageAddressID int
)
AS
BEGIN
	UPDATE PackageIPAddresses
	SET
		ItemID = NULL,
		IsPrimary = 0
	FROM PackageIPAddresses AS PIP
	WHERE
		PIP.PackageAddressID = @PackageAddressID
		AND dbo.CheckActorPackageRights(@ActorID, PIP.PackageID) = 1
END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE DeleteHostingPlan
(
	@ActorID int,
	@PlanID int,
	@Result int OUTPUT
)
AS
SET @Result = 0

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM HostingPlans
WHERE PlanID = @PlanID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- check if some packages uses this plan
IF EXISTS (SELECT PackageID FROM Packages WHERE PlanID = @PlanID)
BEGIN
	SET @Result = -1
	RETURN
END

-- check if some package addons uses this plan
IF EXISTS (SELECT PackageID FROM PackageAddons WHERE PlanID = @PlanID)
BEGIN
	SET @Result = -2
	RETURN
END

-- delete hosting plan
DELETE FROM HostingPlans
WHERE PlanID = @PlanID

RETURN





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE DeleteDomain
(
	@DomainID int,
	@ActorID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM Domains
WHERE DomainID = @DomainID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DELETE FROM Domains
WHERE DomainID = @DomainID

RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE FUNCTION [dbo].[CheckPackageParent]
(
	@ParentPackageID int,
	@PackageID int
)
RETURNS bit
AS
BEGIN

-- check if the user requests hiself
IF @ParentPackageID = @PackageID
BEGIN
	RETURN 1
END

DECLARE @TmpParentPackageID int, @TmpPackageID int
SET @TmpPackageID = @PackageID

WHILE 10 = 10
BEGIN

	SET @TmpParentPackageID = NULL --reset var

	-- get owner
	SELECT
		@TmpParentPackageID = ParentPackageID
	FROM Packages
	WHERE PackageID = @TmpPackageID

	IF @TmpParentPackageID IS NULL -- the last parent package
		BREAK
	
	IF @TmpParentPackageID = @ParentPackageID
	RETURN 1
	
	SET @TmpPackageID = @TmpParentPackageID
END


RETURN 0
END




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

























CREATE PROCEDURE [dbo].[SearchOrganizationAccounts]
(
	@ActorID int,
	@ItemID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@IncludeMailboxes bit
)
AS
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM ServiceItems
WHERE ItemID = @ItemID

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
(EA.AccountType = 7 OR (EA.AccountType = 1 AND @IncludeMailboxes = 1)  )
AND EA.ItemID = @ItemID
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'EA.DisplayName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT
	EA.AccountID,
	EA.ItemID,
	EA.AccountType,
	EA.AccountName,
	EA.DisplayName,
	EA.PrimaryEmailAddress
FROM ExchangeAccounts AS EA
WHERE ' + @condition

print @sql

exec sp_executesql @sql, N'@ItemID int, @IncludeMailboxes bit', 
@ItemID, @IncludeMailboxes

RETURN 




























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


























CREATE PROCEDURE SearchExchangeAccounts
(
	@ActorID int,
	@ItemID int,
	@IncludeMailboxes bit,
	@IncludeContacts bit,
	@IncludeDistributionLists bit,
	@IncludeRooms bit,
	@IncludeEquipment bit,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50)
)
AS
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM ServiceItems
WHERE ItemID = @ItemID

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
((@IncludeMailboxes = 1 AND EA.AccountType = 1)
OR (@IncludeContacts = 1 AND EA.AccountType = 2)
OR (@IncludeDistributionLists = 1 AND EA.AccountType = 3)
OR (@IncludeRooms = 1 AND EA.AccountType = 5)
OR (@IncludeEquipment = 1 AND EA.AccountType = 6))
AND EA.ItemID = @ItemID
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'EA.DisplayName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT
	EA.AccountID,
	EA.ItemID,
	EA.AccountType,
	EA.AccountName,
	EA.DisplayName,
	EA.PrimaryEmailAddress,
	EA.MailEnabledPublicFolder
FROM ExchangeAccounts AS EA
WHERE ' + @condition

print @sql

exec sp_executesql @sql, N'@ItemID int, @IncludeMailboxes int, @IncludeContacts int,
    @IncludeDistributionLists int, @IncludeRooms bit, @IncludeEquipment bit',
@ItemID, @IncludeMailboxes, @IncludeContacts, @IncludeDistributionLists, @IncludeRooms, @IncludeEquipment

RETURN 





























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


























CREATE PROCEDURE [dbo].[SearchExchangeAccount]
(
      @ActorID int,
      @AccountType int,
      @PrimaryEmailAddress nvarchar(300)
)
AS

DECLARE @PackageID int
DECLARE @ItemID int
DECLARE @AccountID int

SELECT
      @AccountID = AccountID,
      @ItemID = ItemID
FROM ExchangeAccounts
WHERE PrimaryEmailAddress = @PrimaryEmailAddress
AND AccountType = @AccountType


-- check space rights
SELECT @PackageID = PackageID FROM ServiceItems
WHERE ItemID = @ItemID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	AccountID,
	ItemID,
	@PackageID AS PackageID,
	AccountType,
	AccountName,
	DisplayName,
	PrimaryEmailAddress,
	MailEnabledPublicFolder,
	MailboxManagerActions,
	SamAccountName,
	AccountPassword 
FROM ExchangeAccounts
WHERE AccountID = @AccountID

RETURN 





























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



























CREATE PROCEDURE [dbo].[OrganizationExists]
(
	@OrganizationID nvarchar(10),
	@Exists bit OUTPUT
)
AS
SET @Exists = 0
IF EXISTS(SELECT * FROM Organizations WHERE OrganizationID = @OrganizationID)
BEGIN
	SET @Exists = 1
END

RETURN































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OCSUsers](
	[OCSUserID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[InstanceID] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_OCSUsers] PRIMARY KEY CLUSTERED 
(
	[OCSUserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE PROCEDURE [dbo].[GetOCSUsersCount] 
(
	@ItemID int,
	@Name nvarchar(400),
	@Email nvarchar(400)
	
)
AS

IF (@Name IS NULL)
BEGIN
	SET @Name = '%'
END

IF (@Email IS NULL)
BEGIN
	SET @Email = '%'
END

SELECT 
	COUNT(ea.AccountID)		
FROM 
	ExchangeAccounts ea 
INNER JOIN 
	OCSUsers ou 
ON 
	ea.AccountID = ou.AccountID
WHERE 
	ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email	












GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










CREATE PROCEDURE [dbo].[GetOCSUsers]
(
	@ItemID int,
	@SortColumn nvarchar(40),
	@SortDirection nvarchar(20),
	@Name nvarchar(400),
	@Email nvarchar(400),
	@StartRow int,
	@Count int	
)
AS

IF (@Name IS NULL)
BEGIN
	SET @Name = '%'
END

IF (@Email IS NULL)
BEGIN
	SET @Email = '%'
END

CREATE TABLE #TempOCSUsers 
(	
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int],	
	[ItemID] [int] NOT NULL,
	[AccountName] [nvarchar](20)  NOT NULL,
	[DisplayName] [nvarchar](300)  NOT NULL,
	[InstanceID] [nvarchar](50)  NOT NULL,
	[PrimaryEmailAddress] [nvarchar](300) NULL,
	[SamAccountName] [nvarchar](100) NULL	
)


IF (@SortColumn = 'DisplayName')
BEGIN
	INSERT INTO 
		#TempOCSUsers 
	SELECT 
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ou.InstanceID,
		ea.PrimaryEmailAddress,
		ea.SamAccountName		
	FROM 
		ExchangeAccounts ea 
	INNER JOIN 
		OCSUsers ou 
	ON 
		ea.AccountID = ou.AccountID
	WHERE 
		ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email	
	ORDER BY 
		ea.DisplayName
END
ELSE
BEGIN
	INSERT INTO 
		#TempOCSUsers
	SELECT 
		ea.AccountID,
		ea.ItemID,
		ea.AccountName,
		ea.DisplayName,
		ou.InstanceID,
		ea.PrimaryEmailAddress,
		ea.SamAccountName		
	FROM 
		ExchangeAccounts ea 
	INNER JOIN 
		OCSUsers ou 
	ON 
		ea.AccountID = ou.AccountID
	WHERE 
		ea.ItemID = @ItemID AND ea.DisplayName LIKE @Name AND ea.PrimaryEmailAddress LIKE @Email	
	ORDER BY 
		ea.PrimaryEmailAddress 
END

DECLARE @RetCount int
SELECT @RetCount = COUNT(ID) FROM #TempOCSUsers 

IF (@SortDirection = 'ASC')
BEGIN
	SELECT * FROM #TempOCSUsers 
	WHERE ID > @StartRow AND ID <= (@StartRow + @Count) 
END
ELSE
BEGIN
	IF (@SortColumn = 'DisplayName')
	BEGIN
		SELECT * FROM #TempOCSUsers 
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY DisplayName DESC
	END
	ELSE
	BEGIN
		SELECT * FROM #TempOCSUsers 
			WHERE ID >@RetCount - @Count - @StartRow AND ID <= @RetCount- @StartRow  ORDER BY PrimaryEmailAddress DESC
	END
	
END


DROP TABLE #TempOCSUsers












GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



























CREATE VIEW [dbo].[UsersDetailed]
AS
SELECT     U.UserID, U.RoleID, U.StatusID, U.OwnerID, U.Created, U.Changed, U.IsDemo, U.Comments, U.IsPeer, U.Username, U.FirstName, U.LastName, U.Email, 
                      U.CompanyName, U.FirstName + ' ' + U.LastName AS FullName, UP.Username AS OwnerUsername, UP.FirstName AS OwnerFirstName, 
                      UP.LastName AS OwnerLastName, UP.RoleID AS OwnerRoleID, UP.FirstName + ' ' + UP.LastName AS OwnerFullName, UP.Email AS OwnerEmail, UP.RoleID AS Expr1,
                          (SELECT     COUNT(PackageID) AS Expr1
                            FROM          dbo.Packages AS P
                            WHERE      (UserID = U.UserID)) AS PackagesNumber, U.EcommerceEnabled
FROM         dbo.Users AS U LEFT OUTER JOIN
                      dbo.Users AS UP ON U.OwnerID = UP.UserID































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE UpdatePackageName
(
	@ActorID int,
	@PackageID int,
	@PackageName nvarchar(300),
	@PackageComments ntext
)
AS

-- check rights
DECLARE @UserID int
SELECT @UserID = UserID FROM Packages
WHERE PackageID = @PackageID

IF NOT(dbo.CheckActorPackageRights(@ActorID, @PackageID) = 1
	OR @UserID = @ActorID
	OR EXISTS(SELECT UserID FROM Users WHERE UserID = @ActorID AND OwnerID = @UserID AND IsPeer = 1))
RAISERROR('You are not allowed to access this package', 16, 1)

-- update package
UPDATE Packages SET
	PackageName = @PackageName,
	PackageComments = @PackageComments
WHERE
	PackageID = @PackageID

RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecBillingCycles](
	[CycleID] [int] IDENTITY(1,1) NOT NULL,
	[ResellerID] [int] NOT NULL,
	[CycleName] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[BillingPeriod] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PeriodLength] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
 CONSTRAINT [PK_ecBillingCycles] PRIMARY KEY CLUSTERED 
(
	[CycleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteAllLogRecords
AS

DELETE FROM Log

RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogTasks](
	[SourceName] [varchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[TaskName] [varchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[TaskDescription] [nvarchar](100) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_LogActions] PRIMARY KEY CLUSTERED 
(
	[SourceName] ASC,
	[TaskName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'APP_INSTALLER', N'INSTALL_APPLICATION', N'Install application')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'BACKUP', N'BACKUP', N'Backup')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'BACKUP', N'RESTORE', N'Restore')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'DNS_ZONE', N'ADD_RECORD', N'Add record')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'DNS_ZONE', N'DELETE_RECORD', N'Delete record')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'DNS_ZONE', N'UPDATE_RECORD', N'Update record')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'DOMAIN', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'DOMAIN', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'DOMAIN', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'ADD_DISTR_LIST_ADDRESS', N'Add distribution list e-mail address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'ADD_DOMAIN', N'Add organization domain')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'ADD_MAILBOX_ADDRESS', N'Add mailbox e-mail address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'ADD_PUBLIC_FOLDER_ADDRESS', N'Add public folder e-mail address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'CALCULATE_DISKSPACE', N'Calculate organization disk space')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'CREATE_CONTACT', N'Create contact')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'CREATE_DISTR_LIST', N'Create distribution list')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'CREATE_MAILBOX', N'Create mailbox')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'CREATE_ORG', N'Create organization')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'CREATE_PUBLIC_FOLDER', N'Create public folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_CONTACT', N'Delete contact')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_DISTR_LIST', N'Delete distribution list')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_DISTR_LIST_ADDRESSES', N'Delete distribution list e-mail addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_DOMAIN', N'Delete organization domain')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_MAILBOX', N'Delete mailbox')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_MAILBOX_ADDRESSES', N'Delete mailbox e-mail addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_ORG', N'Delete organization')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_PUBLIC_FOLDER', N'Delete public folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DELETE_PUBLIC_FOLDER_ADDRESSES', N'Delete public folder e-mail addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'DISABLE_MAIL_PUBLIC_FOLDER', N'Disable mail public folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'ENABLE_MAIL_PUBLIC_FOLDER', N'Enable mail public folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_CONTACT_GENERAL', N'Get contact general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_CONTACT_MAILFLOW', N'Get contact mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_DISTR_LIST_ADDRESSES', N'Get distribution list e-mail addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_DISTR_LIST_GENERAL', N'Get distribution list general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_DISTR_LIST_MAILFLOW', N'Get distribution list mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_FOLDERS_STATS', N'Get organization public folder statistics')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MAILBOX_ADDRESSES', N'Get mailbox e-mail addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MAILBOX_ADVANCED', N'Get mailbox advanced settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MAILBOX_GENERAL', N'Get mailbox general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MAILBOX_MAILFLOW', N'Get mailbox mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_MAILBOXES_STATS', N'Get organization mailboxes statistics')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_ORG_LIMITS', N'Get organization storage limits')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_ORG_STATS', N'Get organization statistics')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_PUBLIC_FOLDER_ADDRESSES', N'Get public folder e-mail addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_PUBLIC_FOLDER_GENERAL', N'Get public folder general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'GET_PUBLIC_FOLDER_MAILFLOW', N'Get public folder mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'SET_ORG_LIMITS', N'Update organization storage limits')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'SET_PRIMARY_DISTR_LIST_ADDRESS', N'Set distribution list primary e-mail address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'SET_PRIMARY_MAILBOX_ADDRESS', N'Set mailbox primary e-mail address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'SET_PRIMARY_PUBLIC_FOLDER_ADDRESS', N'Set public folder primary e-mail address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_CONTACT_GENERAL', N'Update contact general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_CONTACT_MAILFLOW', N'Update contact mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_DISTR_LIST_GENERAL', N'Update distribution list general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_DISTR_LIST_MAILFLOW', N'Update distribution list mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_MAILBOX_ADVANCED', N'Update mailbox advanced settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_MAILBOX_GENERAL', N'Update mailbox general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_MAILBOX_MAILFLOW', N'Update mailbox mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_PUBLIC_FOLDER_GENERAL', N'Update public folder general settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'EXCHANGE', N'UPDATE_PUBLIC_FOLDER_MAILFLOW', N'Update public folder mail flow settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'COPY_FILES', N'Copy files')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'CREATE_ACCESS_DATABASE', N'Create MS Access database')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'CREATE_FILE', N'Create file')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'CREATE_FOLDER', N'Create folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'DELETE_FILES', N'Delete files')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'MOVE_FILES', N'Move files')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'RENAME_FILE', N'Rename file')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'SET_PERMISSIONS', NULL)
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'UNZIP_FILES', N'Unzip files')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'UPDATE_BINARY_CONTENT', N'Update file binary content')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FILES', N'ZIP_FILES', N'Zip files')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FTP_ACCOUNT', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FTP_ACCOUNT', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'FTP_ACCOUNT', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'GLOBAL_DNS', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'GLOBAL_DNS', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'GLOBAL_DNS', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IMPORT', N'IMPORT', N'Import')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'ADD_RANGE', N'Add range')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'ALLOCATE_PACKAGE_IP', N'Allocate package IP addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'DEALLOCATE_PACKAGE_IP', N'Deallocate package IP addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'DELETE_RANGE', N'Delete IP Addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'IP_ADDRESS', N'UPDATE_RANGE', N'Update IP Addresses')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_ACCOUNT', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_ACCOUNT', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_ACCOUNT', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_DOMAIN', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_DOMAIN', N'ADD_POINTER', N'Add pointer')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_DOMAIN', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_DOMAIN', N'DELETE_POINTER', N'Update pointer')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_DOMAIN', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_FORWARDING', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_FORWARDING', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_FORWARDING', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_GROUP', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_GROUP', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_GROUP', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_LIST', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_LIST', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'MAIL_LIST', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ODBC_DSN', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ODBC_DSN', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'ODBC_DSN', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SCHEDULER', N'RUN_SCHEDULE', NULL)
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'ADD_SERVICE', N'Add service')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'CHANGE_WINDOWS_SERVICE_STATUS', N'Change Windows service status')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'CHECK_AVAILABILITY', N'Check availability')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'CLEAR_EVENT_LOG', N'Clear Windows event log')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'DELETE_SERVICE', N'Delete service')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'REBOOT', N'Reboot')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'RESET_TERMINAL_SESSION', N'Reset terminal session')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'TERMINATE_SYSTEM_PROCESS', N'Terminate system process')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'UPDATE_AD_PASSWORD', N'Update active directory password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'UPDATE_PASSWORD', N'Update access password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SERVER', N'UPDATE_SERVICE', N'Update service')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'ADD_GROUP', N'Add group')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'ADD_SITE', N'Add site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'ADD_USER', N'Add user')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'BACKUP_SITE', N'Backup site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'DELETE_GROUP', N'Delete group')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'DELETE_SITE', N'Delete site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'DELETE_USER', N'Delete user')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'INSTALL_WEBPARTS', N'Install Web Parts package')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'RESTORE_SITE', N'Restore site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'UNINSTALL_WEBPARTS', N'Uninstall Web Parts package')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'UPDATE_GROUP', N'Update group')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SHAREPOINT', N'UPDATE_USER', N'Update user')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SPACE', N'CALCULATE_DISKSPACE', N'Calculate disk space')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SPACE', N'CHANGE_ITEMS_STATUS', N'Change hosting items status')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SPACE', N'CHANGE_STATUS', N'Change hostng space status')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SPACE', N'DELETE', N'Delete hosting space')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SPACE', N'DELETE_ITEMS', N'Delete hosting items')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_DATABASE', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_DATABASE', N'BACKUP', N'Backup')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_DATABASE', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_DATABASE', N'RESTORE', N'Restore')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_DATABASE', N'TRUNCATE', N'Truncate')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_DATABASE', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_USER', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_USER', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'SQL_USER', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'STATS_SITE', N'ADD', N'Add statistics site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'STATS_SITE', N'DELETE', N'Delete statistics site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'STATS_SITE', N'UPDATE', N'Update statistics site')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'AUTHENTICATE', N'Authenticate')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'CHANGE_PASSWORD', N'Change password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'CHANGE_PASSWORD_BY_USERNAME_PASSWORD', N'Change password by username/password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'CHANGE_STATUS', N'Change status')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'GET_BY_USERNAME_PASSWORD', N'Get by username/password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'SEND_REMINDER', N'Send password reminder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'USER', N'UPDATE_SETTINGS', N'Update settings')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VIRTUAL_SERVER', N'ADD_SERVICES', N'Add services')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VIRTUAL_SERVER', N'DELETE_SERVICES', N'Delete services')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'ADD_EXTERNAL_IP', N'Add external IP')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'ADD_PRIVATE_IP', N'Add private IP')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'APPLY_SNAPSHOT', N'Apply VPS snapshot')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'CANCEL_JOB', N'Cancel Job')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'CHANGE_ADMIN_PASSWORD', N'Change administrator password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'CHANGE_STATE', N'Change VPS state')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'CREATE', N'Create VPS')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'DELETE', N'Delete VPS')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'DELETE_EXTERNAL_IP', N'Delete external IP')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'DELETE_PRIVATE_IP', N'Delete private IP')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'DELETE_SNAPSHOT', N'Delete VPS snapshot')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'DELETE_SNAPSHOT_SUBTREE', N'Delete VPS snapshot subtree')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'EJECT_DVD_DISK', N'Eject DVD disk')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'INSERT_DVD_DISK', N'Insert DVD disk')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'REINSTALL', N'Re-install VPS')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'RENAME_SNAPSHOT', N'Rename VPS snapshot')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'SEND_SUMMARY_LETTER', N'Send VPS summary letter')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'SET_PRIMARY_EXTERNAL_IP', N'Set primary external IP')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'SET_PRIMARY_PRIVATE_IP', N'Set primary private IP')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'TAKE_SNAPSHOT', N'Take VPS snapshot')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'UPDATE_CONFIGURATION', N'Update VPS configuration')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'UPDATE_HOSTNAME', N'Update host name')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'UPDATE_IP', N'Update IP Address')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'UPDATE_PERMISSIONS', N'Update VPS permissions')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'VPS', N'UPDATE_VDC_PERMISSIONS', N'Update space permissions')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'ADD', N'Add')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'ADD_POINTER', N'Add domain pointer')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'ADD_SSL_FOLDER', N'Add shared SSL folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'ADD_VDIR', N'Add virtual directory')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'CHANGE_FP_PASSWORD', N'Change FrontPage account password')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'CHANGE_STATE', N'Change state')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE', N'Delete')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE_POINTER', N'Delete domain pointer')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE_SECURED_FOLDER', N'Delete secured folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE_SECURED_GROUP', N'Delete secured group')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE_SECURED_USER', N'Delete secured user')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE_SSL_FOLDER', N'Delete shared SSL folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'DELETE_VDIR', N'Delete virtual directory')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'INSTALL_FP', N'Install FrontPage Extensions')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'INSTALL_SECURED_FOLDERS', N'Install secured folders')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UNINSTALL_FP', N'Uninstall FrontPage Extensions')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UNINSTALL_SECURED_FOLDERS', N'Uninstall secured folders')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UPDATE', N'Update')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UPDATE_SECURED_FOLDER', N'Add/update secured folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UPDATE_SECURED_GROUP', N'Add/update secured group')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UPDATE_SECURED_USER', N'Add/update secured user')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UPDATE_SSL_FOLDER', N'Update shared SSL folder')
GO
INSERT [dbo].[AuditLogTasks] ([SourceName], [TaskName], [TaskDescription]) VALUES (N'WEB_SITE', N'UPDATE_VDIR', N'Update virtual directory')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogSources](
	[SourceName] [varchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_AuditLogSources] PRIMARY KEY CLUSTERED 
(
	[SourceName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'APP_INSTALLER')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'BACKUP')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'DNS_ZONE')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'DOMAIN')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'EXCHANGE')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'FILES')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'FTP_ACCOUNT')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'GLOBAL_DNS')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'IMPORT')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'IP_ADDRESS')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'MAIL_ACCOUNT')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'MAIL_DOMAIN')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'MAIL_FORWARDING')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'MAIL_GROUP')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'MAIL_LIST')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'ODBC_DSN')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'SCHEDULER')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'SERVER')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'SHAREPOINT')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'SPACE')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'SQL_DATABASE')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'SQL_USER')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'STATS_SITE')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'USER')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'VIRTUAL_SERVER')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'VPS')
GO
INSERT [dbo].[AuditLogSources] ([SourceName]) VALUES (N'WEB_SITE')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLog](
	[RecordID] [varchar](32) COLLATE Latin1_General_CI_AS NOT NULL,
	[UserID] [int] NULL,
	[Username] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[ItemID] [int] NULL,
	[SeverityID] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[FinishDate] [datetime] NOT NULL,
	[SourceName] [varchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[TaskName] [varchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ItemName] [nvarchar](100) COLLATE Latin1_General_CI_AS NULL,
	[ExecutionLog] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[PackageID] [int] NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetAuditLogRecord]
(
	@RecordID varchar(32)
)
AS

SELECT
	L.RecordID,
    L.SeverityID,
    L.StartDate,
    L.FinishDate,
    L.ItemID,
    L.SourceName,
    L.TaskName,
    L.ItemName,
    L.ExecutionLog,
    
    ISNULL(L.UserID, 0) AS UserID,
	L.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	ISNULL(U.RoleID, 0) AS RoleID,
	U.Email
FROM AuditLog AS L
LEFT OUTER JOIN UsersDetailed AS U ON L.UserID = U.UserID
WHERE RecordID = @RecordID
RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE FUNCTION [dbo].[CanUpdatePackageDetails]
(
	@ActorID int,
	@PackageID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1
RETURN 1

DECLARE @UserID int
SELECT @UserID = UserID FROM Packages
WHERE PackageID = @PackageID

-- check if the user requests himself
IF @ActorID = @UserID
RETURN 1


DECLARE @IsPeer bit
DECLARE @OwnerID int

SELECT @IsPeer = IsPeer, @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @IsPeer = 1
SET @ActorID = @OwnerID

IF @ActorID = @UserID
RETURN 1

DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
		BREAK
	
	IF @ParentUserID = @ActorID
	RETURN 1
	
	SET @TmpUserID = @ParentUserID
END

RETURN 0
END


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceGroups](
	[GroupID] [int] NOT NULL,
	[GroupName] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[GroupOrder] [int] NOT NULL,
	[GroupController] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_ResourceGroups] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (1, N'OS', 1, N'WebsitePanel.EnterpriseServer.OperatingSystemController')
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (2, N'Web', 2, N'WebsitePanel.EnterpriseServer.WebServerController')
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (3, N'FTP', 3, N'WebsitePanel.EnterpriseServer.FtpServerController')
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (4, N'Mail', 4, N'WebsitePanel.EnterpriseServer.MailServerController')
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (5, N'MsSQL2000', 5, N'WebsitePanel.EnterpriseServer.DatabaseServerController')
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (6, N'MySQL4', 7, N'WebsitePanel.EnterpriseServer.DatabaseServerController')
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (7, N'DNS', 10, N'WebsitePanel.EnterpriseServer.DnsServerController')
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (8, N'Statistics', 11, N'WebsitePanel.EnterpriseServer.StatisticsServerController')
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (9, N'SharePoint', 9, N'WebsitePanel.EnterpriseServer.SharePointServerController')
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (10, N'MsSQL2005', 6, N'WebsitePanel.EnterpriseServer.DatabaseServerController')
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (11, N'MySQL5', 8, N'WebsitePanel.EnterpriseServer.DatabaseServerController')
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (12, N'Exchange', 4, NULL)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (13, N'Hosted Organizations', 4, NULL)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (20, N'Hosted SharePoint', 9, N'WebsitePanel.EnterpriseServer.HostedSharePointServerController')
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (21, N'Hosted CRM', 10, NULL)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (22, N'MsSQL2008', 7, N'WebsitePanel.EnterpriseServer.DatabaseServerController')
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (30, N'VPS', 12, NULL)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (31, N'BlackBerry', 13, NULL)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (32, N'OCS', 14, NULL)
GO
INSERT [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (33, N'ExchangeHostedEdition', 4, N'WebsitePanel.EnterpriseServer.ExchangeHostedEditionController')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetPackageDiskspace
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	RG.GroupID,
	RG.GroupName,
	ROUND(CONVERT(float, ISNULL(GD.Diskspace, 0)) / 1024 / 1024, 0) AS Diskspace,
	ISNULL(GD.Diskspace, 0) AS DiskspaceBytes
FROM ResourceGroups AS RG
LEFT OUTER JOIN
(
	SELECT
		PD.GroupID,
		SUM(ISNULL(PD.DiskSpace, 0)) AS Diskspace -- in megabytes
	FROM PackagesTreeCache AS PT
	INNER JOIN PackagesDiskspace AS PD ON PT.PackageID = PD.PackageID
	INNER JOIN Packages AS P ON PT.PackageID = P.PackageID
	INNER JOIN HostingPlanResources AS HPR ON PD.GroupID = HPR.GroupID
		AND HPR.PlanID = P.PlanID AND HPR.CalculateDiskspace = 1
	WHERE PT.ParentPackageID = @PackageID
	GROUP BY PD.GroupID
) AS GD ON RG.GroupID = GD.GroupID
WHERE GD.Diskspace <> 0
ORDER BY RG.GroupOrder

RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetPackageBandwidth
(
	@ActorID int,
	@PackageID int,
	@StartDate datetime,
	@EndDate datetime
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	RG.GroupID,
	RG.GroupName,
	ROUND(CONVERT(float, ISNULL(GB.BytesSent, 0)) / 1024 / 1024, 0) AS MegaBytesSent,
	ROUND(CONVERT(float, ISNULL(GB.BytesReceived, 0)) / 1024 / 1024, 0) AS MegaBytesReceived,
	ROUND(CONVERT(float, ISNULL(GB.BytesTotal, 0)) / 1024 / 1024, 0) AS MegaBytesTotal,
	ISNULL(GB.BytesSent, 0) AS BytesSent,
	ISNULL(GB.BytesReceived, 0) AS BytesReceived,
	ISNULL(GB.BytesTotal, 0) AS BytesTotal
FROM ResourceGroups AS RG
LEFT OUTER JOIN
(
	SELECT
		PB.GroupID,
		SUM(ISNULL(PB.BytesSent, 0)) AS BytesSent,
		SUM(ISNULL(PB.BytesReceived, 0)) AS BytesReceived,
		SUM(ISNULL(PB.BytesSent, 0)) + SUM(ISNULL(PB.BytesReceived, 0)) AS BytesTotal
	FROM PackagesTreeCache AS PT
	INNER JOIN PackagesBandwidth AS PB ON PT.PackageID = PB.PackageID
	INNER JOIN Packages AS P ON PB.PackageID = P.PackageID
	INNER JOIN HostingPlanResources AS HPR ON PB.GroupID = HPR.GroupID AND HPR.PlanID = P.PlanID
		AND HPR.CalculateBandwidth = 1
	WHERE
		PT.ParentPackageID = @PackageID
		AND PB.LogDate BETWEEN @StartDate AND @EndDate
	GROUP BY PB.GroupID
) AS GB ON RG.GroupID = GB.GroupID
WHERE GB.BytesTotal > 0
ORDER BY RG.GroupOrder

RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemSettings](
	[SettingsName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyValue] [ntext] COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_SystemSettings] PRIMARY KEY CLUSTERED 
(
	[SettingsName] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[SystemSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'BackupSettings', N'BackupsPath', N'c:\HostingBackups')
GO
INSERT [dbo].[SystemSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SmtpSettings', N'SmtpEnableSsl', N'False')
GO
INSERT [dbo].[SystemSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SmtpSettings', N'SmtpPort', N'25')
GO
INSERT [dbo].[SystemSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SmtpSettings', N'SmtpServer', N'127.0.0.1')
GO
INSERT [dbo].[SystemSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SmtpSettings', N'SmtpUsername', N'postmaster')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SSLCertificates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[SiteID] [int] NOT NULL,
	[FriendlyName] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[Hostname] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[DistinguishedName] [nvarchar](500) COLLATE Latin1_General_CI_AS NULL,
	[CSR] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[CSRLength] [int] NULL,
	[Certificate] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[Hash] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[Installed] [bit] NULL,
	[IsRenewal] [bit] NULL,
	[ValidFrom] [datetime] NULL,
	[ExpiryDate] [datetime] NULL,
	[SerialNumber] [nvarchar](250) COLLATE Latin1_General_CI_AS NULL,
	[Pfx] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[PreviousId] [int] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[GetSSLCertificateByID]
(
	@ActorID int,
	@ID int
)
AS

SELECT
	[ID], [UserID], [SiteID], [Hostname], [FriendlyName], [CSR], [Certificate], [Hash], [Installed], [IsRenewal], [PreviousId]
FROM
	[dbo].[SSLCertificates]
INNER JOIN
	[dbo].[ServiceItems] AS [SI] ON [SSLCertificates].[SiteID] = [SI].[ItemID]
WHERE
	[ID] = @ID AND [dbo].CheckActorPackageRights(@ActorID, [SI].[PackageID]) = 1

RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[GetSiteCert]
(
	@ActorID int,
	@ID int
)
AS

SELECT
	[UserID], [SiteID], [Hostname], [CSR], [Certificate], [Hash], [Installed], [IsRenewal]
FROM
	[dbo].[SSLCertificates]
INNER JOIN
	[dbo].[ServiceItems] AS [SI] ON [SSLCertificates].[SiteID] = [SI].[ItemID]
WHERE
	[SiteID] = @ID AND [Installed] = 1 AND [dbo].CheckActorPackageRights(@ActorID, [SI].[PackageID]) = 1
RETURN



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[GetPendingSSLForWebsite]
(
	@ActorID int,
	@PackageID int,
	@websiteid int,
	@Recursive bit = 1
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1)
	RETURN
END

SELECT
	[ID], [UserID], [SiteID], [Hostname], [CSR], [Certificate], [Hash], [Installed]
FROM
	[dbo].[SSLCertificates]
WHERE
	@websiteid = 2 AND [Installed] = 0 AND [IsRenewal] = 0

RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetCertificatesForSite]
(
	@ActorID int,
	@PackageID int,
	@websiteid int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1)
	RETURN
END

SELECT
	[ID], [UserID], [SiteID], [FriendlyName], [Hostname], [DistinguishedName], 
	[CSR], [CSRLength], [ValidFrom], [ExpiryDate], [Installed], [IsRenewal], 
	[PreviousId], [SerialNumber]
FROM
	[dbo].[SSLCertificates]
WHERE
	[SiteID] = @websiteid
RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[DeleteCertificate] 
(
	@ActorID int,
	@PackageID int,
	@id int
	
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1)
	RETURN
END

-- insert record
DELETE FROM
	[dbo].[SSLCertificates]
WHERE
	[ID] = @id
           
RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[CompleteSSLRequest] 
(
	@ActorID int,
	@PackageID int,
	@ID int,	
	@Certificate ntext,
	@SerialNumber nvarchar(250),
	@Hash ntext,
	@DistinguishedName nvarchar(500),
	@ValidFrom datetime,
	@ExpiryDate datetime
	
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1)
	RETURN
END

-- insert record
UPDATE
	[dbo].[SSLCertificates]
SET	
	[Certificate] = @Certificate,
	[Installed] = 1,
	[SerialNumber] = @SerialNumber,
	[DistinguishedName] = @DistinguishedName,
	[Hash] = @Hash,
	[ValidFrom] = @ValidFrom,
	[ExpiryDate] = @ExpiryDate 
WHERE
	[ID] = @ID;           


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO


CREATE PROCEDURE [dbo].[AddSSLRequest] 
(
	@SSLID int OUTPUT,
	@ActorID int,
	@PackageID int,
	@UserID int,
	@WebSiteID int,
	@FriendlyName nvarchar(255),
	@HostName nvarchar(255),
	@CSR ntext,
	@CSRLength int,
	@DistinguishedName nvarchar(500),
	@IsRenewal bit = 0,
	@PreviousId int = NULL
	
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1)
	RETURN
END

-- insert record
INSERT INTO [dbo].[SSLCertificates]
	([UserID], [SiteID], [FriendlyName], [Hostname], [DistinguishedName], [CSR], [CSRLength], [IsRenewal], [PreviousId])
VALUES
	(@UserID, @WebSiteID, @FriendlyName, @HostName, @DistinguishedName, @CSR, @CSRLength, @IsRenewal, @PreviousId)

SET @SSLID = SCOPE_IDENTITY()
RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO



CREATE PROCEDURE [dbo].[AddPFX] 
(
	@ActorID int,
	@PackageID int,
	@UserID int,
	@WebSiteID int,
	@FriendlyName nvarchar(255),
	@HostName nvarchar(255),	
	@CSRLength int,
	@DistinguishedName nvarchar(500),
	@SerialNumber nvarchar(250),
	@ValidFrom datetime,
	@ExpiryDate datetime
	
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1)
	RETURN
END

-- insert record
INSERT INTO [dbo].[SSLCertificates]
	([UserID], [SiteID], [FriendlyName], [Hostname], [DistinguishedName], [CSRLength], [SerialNumber], [ValidFrom], [ExpiryDate], [Installed])
VALUES
	(@UserID, @WebSiteID, @FriendlyName, @HostName, @DistinguishedName, @CSRLength, @SerialNumber, @ValidFrom, @ExpiryDate, 1)

RETURN


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetPackagesPaged]
(
	@ActorID int,
	@UserID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS

-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

SET @sql = '
DECLARE @HasUserRights bit
SET @HasUserRights = dbo.CheckActorUserRights(@ActorID, @UserID)

DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows
DECLARE @Packages TABLE
(
	ItemPosition int IDENTITY(1,1),
	PackageID int
)
INSERT INTO @Packages (PackageID)
SELECT
	P.PackageID
FROM Packages AS P
--INNER JOIN UsersTree(@UserID, 1) AS UT ON P.UserID = UT.UserID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE
	P.UserID <> @UserID AND dbo.CheckUserParent(@UserID, P.UserID) = 1
	AND @HasUserRights = 1 '

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE @FilterValue '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(PackageID) FROM @Packages;
SELECT
	P.PackageID,
	P.PackageName,
	P.StatusID,
	P.PurchaseDate,
	
	dbo.GetItemComments(P.PackageID, ''PACKAGE'', @ActorID) AS Comments,
	
	-- server
	P.ServerID,
	ISNULL(S.ServerName, ''None'') AS ServerName,
	ISNULL(S.Comments, '''') AS ServerComments,
	ISNULL(S.VirtualServer, 1) AS VirtualServer,
	
	-- hosting plan
	P.PlanID,
	HP.PlanName,
	
	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM @Packages AS TP
INNER JOIN Packages AS P ON TP.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE TP.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @UserID int, @FilterValue nvarchar(50), @ActorID int',
@StartRow, @MaximumRows, @UserID, @FilterValue, @ActorID


RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[GetPackagePrivateIPAddressesPaged]
	@PackageID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
AS
BEGIN


-- start
DECLARE @condition nvarchar(700)
SET @condition = '
SI.PackageID = @PackageID
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'PA.IPAddress ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(PA.PrivateAddressID)
FROM dbo.PrivateIPAddresses AS PA
INNER JOIN dbo.ServiceItems AS SI ON PA.ItemID = SI.ItemID
WHERE ' + @condition + '

DECLARE @Addresses AS TABLE
(
	PrivateAddressID int
);

WITH TempItems AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		PA.PrivateAddressID
	FROM dbo.PrivateIPAddresses AS PA
	INNER JOIN dbo.ServiceItems AS SI ON PA.ItemID = SI.ItemID
	WHERE ' + @condition + '
)

INSERT INTO @Addresses
SELECT PrivateAddressID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	PA.PrivateAddressID,
	PA.IPAddress,
	PA.ItemID,
	SI.ItemName,
	PA.IsPrimary
FROM @Addresses AS TA
INNER JOIN dbo.PrivateIPAddresses AS PA ON TA.PrivateAddressID = PA.PrivateAddressID
INNER JOIN dbo.ServiceItems AS SI ON PA.ItemID = SI.ItemID
'

print @sql

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int',
@PackageID, @StartRow, @MaximumRows

END


















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageSettings](
	[PackageID] [int] NOT NULL,
	[SettingsName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyValue] [ntext] COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_PackageSettings] PRIMARY KEY CLUSTERED 
(
	[PackageID] ASC,
	[SettingsName] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE UpdatePackageSettings
(
	@ActorID int,
	@PackageID int,
	@SettingsName nvarchar(50),
	@Xml ntext
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- delete old properties
BEGIN TRAN
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM PackageSettings
WHERE PackageID = @PackageID AND SettingsName = @SettingsName

INSERT INTO PackageSettings
(
	PackageID,
	SettingsName,
	PropertyName,
	PropertyValue
)
SELECT
	@PackageID,
	@SettingsName,
	PropertyName,
	PropertyValue
FROM OPENXML(@idoc, '/properties/property',1) WITH 
(
	PropertyName nvarchar(50) '@name',
	PropertyValue ntext '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE [dbo].[GetUsersPaged]
(
	@ActorID int,
	@UserID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@StatusID int,
	@RoleID int,
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int,
	@Recursive bit
)
AS
-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

SET @sql = '

DECLARE @HasUserRights bit
SET @HasUserRights = dbo.CheckActorUserRights(@ActorID, @UserID)

DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows
DECLARE @Users TABLE
(
	ItemPosition int IDENTITY(0,1),
	UserID int
)
INSERT INTO @Users (UserID)
SELECT
	U.UserID
FROM UsersDetailed AS U
WHERE 
	U.UserID <> @UserID AND U.IsPeer = 0 AND
	(
		(@Recursive = 0 AND OwnerID = @UserID) OR
		(@Recursive = 1 AND dbo.CheckUserParent(@UserID, U.UserID) = 1)
	)
	AND ((@StatusID = 0) OR (@StatusID > 0 AND U.StatusID = @StatusID))
	AND ((@RoleID = 0) OR (@RoleID > 0 AND U.RoleID = @RoleID))
	AND @HasUserRights = 1 '

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE @FilterValue '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(UserID) FROM @Users;
SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	dbo.GetItemComments(U.UserID, ''USER'', @ActorID) AS Comments,
	U.IsPeer,
	U.Username,
	U.FirstName,
	U.LastName,
	U.Email,
	U.FullName,
	U.OwnerUsername,
	U.OwnerFirstName,
	U.OwnerLastName,
	U.OwnerRoleID,
	U.OwnerFullName,
	U.OwnerEmail,
	U.PackagesNumber,
	U.CompanyName,
	U.EcommerceEnabled
FROM @Users AS TU
INNER JOIN UsersDetailed AS U ON TU.UserID = U.UserID
WHERE TU.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @UserID int, @FilterValue nvarchar(50), @ActorID int, @Recursive bit, @StatusID int, @RoleID int',
@StartRow, @MaximumRows, @UserID, @FilterValue, @ActorID, @Recursive, @StatusID, @RoleID


RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetUserDomainsPaged
(
	@ActorID int,
	@UserID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS
-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

SET @sql = '
DECLARE @HasUserRights bit
SET @HasUserRights = dbo.CheckActorUserRights(@ActorID, @UserID)

DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows
DECLARE @Users TABLE
(
	ItemPosition int IDENTITY(1,1),
	UserID int,
	DomainID int
)
INSERT INTO @Users (UserID, DomainID)
SELECT
	U.UserID,
	D.DomainID
FROM Users AS U
INNER JOIN UsersTree(@UserID, 1) AS UT ON U.UserID = UT.UserID
LEFT OUTER JOIN Packages AS P ON U.UserID = P.UserID
LEFT OUTER JOIN Domains AS D ON P.PackageID = D.PackageID
WHERE
	U.UserID <> @UserID AND U.IsPeer = 0
	AND @HasUserRights = 1 '

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE @FilterValue '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(UserID) FROM @Users;
SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	U.Comments,
	U.IsPeer,
	U.Username,
	U.FirstName,
	U.LastName,
	U.Email,
	D.DomainName
FROM @Users AS TU
INNER JOIN Users AS U ON TU.UserID = U.UserID
LEFT OUTER JOIN Domains AS D ON TU.DomainID = D.DomainID
WHERE TU.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @UserID int, @FilterValue nvarchar(50), @ActorID int',
@StartRow, @MaximumRows, @UserID, @FilterValue, @ActorID


RETURN

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetNestedPackagesPaged]
(
	@ActorID int,
	@PackageID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@StatusID int,
	@PlanID int,
	@ServerID int,
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS

-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

SET @sql = '
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR(''You are not allowed to access this package'', 16, 1)

DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows
DECLARE @Packages TABLE
(
	ItemPosition int IDENTITY(1,1),
	PackageID int
)
INSERT INTO @Packages (PackageID)
SELECT
	P.PackageID
FROM Packages AS P
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE
	P.ParentPackageID = @PackageID
	AND ((@StatusID = 0) OR (@StatusID > 0 AND P.StatusID = @StatusID))
	AND ((@PlanID = 0) OR (@PlanID > 0 AND P.PlanID = @PlanID))
	AND ((@ServerID = 0) OR (@ServerID > 0 AND P.ServerID = @ServerID)) '

IF @FilterColumn <> '' AND @FilterValue <> ''
SET @sql = @sql + ' AND ' + @FilterColumn + ' LIKE @FilterValue '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(PackageID) FROM @Packages;
SELECT
	P.PackageID,
	P.PackageName,
	P.StatusID,
	P.PurchaseDate,
	
	dbo.GetItemComments(P.PackageID, ''PACKAGE'', @ActorID) AS Comments,
	
	-- server
	P.ServerID,
	ISNULL(S.ServerName, ''None'') AS ServerName,
	ISNULL(S.Comments, '''') AS ServerComments,
	ISNULL(S.VirtualServer, 1) AS VirtualServer,
	
	-- hosting plan
	P.PlanID,
	HP.PlanName,
	
	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM @Packages AS TP
INNER JOIN Packages AS P ON TP.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE TP.ItemPosition BETWEEN @StartRow AND @EndRow'

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @PackageID int, @FilterValue nvarchar(50), @ActorID int, @StatusID int, @PlanID int, @ServerID int',
@StartRow, @MaximumRows, @PackageID, @FilterValue, @ActorID, @StatusID, @PlanID, @ServerID


RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecSupportedPlugins](
	[PluginID] [int] IDENTITY(1,1) NOT NULL,
	[PluginName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[DisplayName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PluginGroup] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[TypeName] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Interactive] [bit] NOT NULL,
	[SupportedItems] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[UniqueID] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_ecSupportedPlugins] PRIMARY KEY CLUSTERED 
(
	[PluginID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET IDENTITY_INSERT [dbo].[ecSupportedPlugins] ON 

GO
INSERT [dbo].[ecSupportedPlugins] ([PluginID], [PluginName], [DisplayName], [PluginGroup], [TypeName], [Interactive], [SupportedItems], [UniqueID]) VALUES (1, N'AuthorizeNet', N'Authorize.Net', N'CC_GATEWAY', N'WebsitePanel.Ecommerce.EnterpriseServer.AuthorizeNetProvider, WebsitePanel.Plugins.AuthorizeNet', 0, N'American Express=Amex,Discover,Master Card=MasterCard,Visa', NULL)
GO
INSERT [dbo].[ecSupportedPlugins] ([PluginID], [PluginName], [DisplayName], [PluginGroup], [TypeName], [Interactive], [SupportedItems], [UniqueID]) VALUES (2, N'PayPalPro', N'PayPal Pro', N'CC_GATEWAY', N'WebsitePanel.Ecommerce.EnterpriseServer.PayPalProProvider, WebsitePanel.Plugins.PayPalPro', 0, N'Visa,Master Card=MasterCard,Discover,American Express=Amex,Switch,Solo', NULL)
GO
INSERT [dbo].[ecSupportedPlugins] ([PluginID], [PluginName], [DisplayName], [PluginGroup], [TypeName], [Interactive], [SupportedItems], [UniqueID]) VALUES (3, N'2Checkout', N'2Checkout', N'2CO', N'WebsitePanel.Ecommerce.EnterpriseServer.TCOProvider, WebsitePanel.Plugins.2Checkout', 1, NULL, N'6A847B61-6178-445d-93FC-1929E86984DF')
GO
INSERT [dbo].[ecSupportedPlugins] ([PluginID], [PluginName], [DisplayName], [PluginGroup], [TypeName], [Interactive], [SupportedItems], [UniqueID]) VALUES (4, N'PayPalStandard', N'PayPal Standard', N'PP_ACCOUNT', N'WebsitePanel.Ecommerce.EnterpriseServer.PayPalStandardProvider, WebsitePanel.Plugins.PayPalStandard', 1, NULL, N'C7EA147E-880D-46f4-88C0-90A9D58BB8C0')
GO
INSERT [dbo].[ecSupportedPlugins] ([PluginID], [PluginName], [DisplayName], [PluginGroup], [TypeName], [Interactive], [SupportedItems], [UniqueID]) VALUES (5, N'OfflinePayment', N'Offline Payment', N'OFFLINE', N'WebsitePanel.Ecommerce.EnterpriseServer.OfflinePayment, WebsitePanel.Plugins.OfflinePayment', 0, NULL, NULL)
GO
INSERT [dbo].[ecSupportedPlugins] ([PluginID], [PluginName], [DisplayName], [PluginGroup], [TypeName], [Interactive], [SupportedItems], [UniqueID]) VALUES (6, N'Enom', N'Enom', N'DOMAIN_REGISTRAR', N'WebsitePanel.Ecommerce.EnterpriseServer.EnomRegistrar, WebsitePanel.Plugins.Enom', 0, NULL, NULL)
GO
INSERT [dbo].[ecSupportedPlugins] ([PluginID], [PluginName], [DisplayName], [PluginGroup], [TypeName], [Interactive], [SupportedItems], [UniqueID]) VALUES (7, N'Directi', N'Directi', N'DOMAIN_REGISTRAR', N'WebsitePanel.Ecommerce.EnterpriseServer.DirectiRegistrar, WebsitePanel.Plugins.Directi', 0, NULL, NULL)
GO
INSERT [dbo].[ecSupportedPlugins] ([PluginID], [PluginName], [DisplayName], [PluginGroup], [TypeName], [Interactive], [SupportedItems], [UniqueID]) VALUES (8, N'OfflineRegistrar', N'Offline Registrar', N'DOMAIN_REGISTRAR', N'WebsitePanel.Ecommerce.EnterpriseServer.OfflineRegistrar, WebsitePanel.Plugins.OfflineRegistrar', 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[ecSupportedPlugins] OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecSupportedPluginLog](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[PluginID] [int] NOT NULL,
	[RecordType] [int] NOT NULL,
	[RawData] [ntext] COLLATE Latin1_General_CI_AS NOT NULL,
	[Created] [datetime] NOT NULL,
	[ContractID] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_ecSpacePluginLog] PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecStoreSettings](
	[SettingsName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyValue] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[ResellerID] [int] NOT NULL,
 CONSTRAINT [PK_ecSystemSettings] PRIMARY KEY CLUSTERED 
(
	[ResellerID] ASC,
	[SettingsName] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecStoreDefaultSettings](
	[SettingsName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyValue] [ntext] COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_ecSpaceDefaultSettings] PRIMARY KEY CLUSTERED 
(
	[SettingsName] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'ActivateServicesTemplate', N'CC', N'info@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'ActivateServicesTemplate', N'From', N'support@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'ActivateServicesTemplate', N'HtmlBody', N'
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Activated Service Summary Information</title>
    <style type="text/css">
		.Summary { background-color: ##ffffff; padding: 5px; }
		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }
        .Summary A { color: ##0153A4; }
        .Summary { font-family: Tahoma; font-size: 9pt; }
        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }
        .Summary H2 { font-size: 1.2em; } 
        .Summary TABLE { border: solid 1px ##e5e5e5; }
        .Summary TH,
        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }
        .Summary TD { padding: 8px; font-size: 9pt; }
        .Summary UL LI { font-size: 1.1em; font-weight: bold; }
        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }
    </style>
</head>
<body>
<div class="Summary">

<a name="top"></a>

<div class="Header">
	Activated Service Information
</div>

<p>Hello #customer["FirstName"]#,</p>

<p>"#service.ServiceName#" service has been activated under your user account
and below is the summary information.</p>
<a name="overview"></a>
<h1>Service Overview</h1>

<table>
    <tr>
        <td class="Label">Service Name:</td>
        <td>#service.ServiceName#</td>
    </tr>
	<tr>
        <td class="Label">Created:</td>
        <td>#service.Created#</td>
    </tr>
	<tr>
        <td class="Label">Setup fee:</td>
        <td>#service.Currency#&nbsp;#format(service.SetupFee, "0.00")#</td>
    </tr>
	<tr>
        <td class="Label">Recurring fee:</td>
        <td>#service.Currency#&nbsp;#format(service.RecurringFee, "0.00")#</td>
    </tr>
</table>

<p>
If you have any questions regarding your hosting account, feel free to contact our sales department at any time.
</p>

<p>
Best regards,<br />
ACME Hosting Inc.<br />
Web Site: <a href="http://www.acmehosting.com">www.acmehosting.com</a><br />
E-Mail: <a href="mailto:support@acmehosting.com">support@acmehosting.com</a></p>

</div>
</body>
</html>')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'ActivateServicesTemplate', N'Subject', N'Activate Service Notification')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'ActivateServicesTemplate', N'TextBody', N'
================================
   Activated Service Information
================================

Hello #Customer["FirstName"]#,

"#service.ServiceName#" service has been activated under your user account
and below is the summary information.

Service Overview
=============
Service Name: #service.ServiceName#
Created: #service.Created#
Setup fee: #service.Currency# #format(service.SetupFee, "0.00")#
Recurring fee: #service.Currency# #format(service.RecurringFee, "0.00")#

If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards,
ACME Hosting Inc.
Web Site: http://www.acmehosting.com
E-Mail: support@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'CancelServicesTemplate', N'CC', N'sales@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'CancelServicesTemplate', N'From', N'support@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'CancelServicesTemplate', N'HtmlBody', N'
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Cancel Service Summary Information</title>
    <style type="text/css">
		.Summary { background-color: ##ffffff; padding: 5px; }
		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }
        .Summary A { color: ##0153A4; }
        .Summary { font-family: Tahoma; font-size: 9pt; }
        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }
        .Summary H2 { font-size: 1.2em; } 
        .Summary TABLE { border: solid 1px ##e5e5e5; }
        .Summary TH,
        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }
        .Summary TD { padding: 8px; font-size: 9pt; }
        .Summary UL LI { font-size: 1.1em; font-weight: bold; }
        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }
    </style>
</head>
<body>
<div class="Summary">

<a name="top"></a>

<div class="Header">
	Cancelled Service Information
</div>

<p>Hello #customer["FirstName"]#,</p>

<p>"#service.ServiceName#" service has been cancelled under your user account
and below is the summary information.</p>
<h1>Service Overview</h1>

<table>
    <tr>
        <td class="Label">Service Name:</td>
        <td>#service.ServiceName#</td>
    </tr>
	<tr>
        <td class="Label">Created:</td>
        <td>#service.Created#</td>
    </tr>
</table>

<p>
If you have any questions regarding your hosting account, feel free to contact our sales department at any time.
</p>

<p>
Best regards,<br />
ACME Hosting Inc.<br />
Web Site: <a href="http://www.acmehosting.com">www.acmehosting.com</a><br />
E-Mail: <a href="mailto:support@acmehosting.com">support@acmehosting.com</a></p>

</div>
</body>
</html>')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'CancelServicesTemplate', N'Subject', N'Cancel Service Notificaton')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'CancelServicesTemplate', N'TextBody', N'
================================
   Cancelled Service Information
================================

Hello #Customer["FirstName"]#,

"#service.ServiceName#" service has been cancelled under your user account
and below is the summary information.

Service Overview
=============
Service Name: #service.ServiceName#
Created: #service.Created#

If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards,
ACME Hosting Inc.
Web Site: http://www.acmehosting.com
E-Mail: support@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'EmitInvoiceTemplate', N'CC', N'info@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'EmitInvoiceTemplate', N'From', N'sales@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'EmitInvoiceTemplate', N'HtmlBody', N'<style type="text/css">
	.Summary { background-color: ##ffffff; padding: 5px; }
	.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }
	.Summary A { color: ##0153A4; }
	.Summary { font-family: Tahoma; font-size: 9pt; }
	.Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }
	.Summary H2 { font-size: 1.2em; } 
	.Summary TABLE { border: solid 1px ##e5e5e5; }
	.Summary TH,
	.Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }
	.Summary TD { padding: 8px; font-size: 9pt; }
	.Summary UL LI { font-size: 1.1em; font-weight: bold; }
	.Summary UL UL LI { font-size: 0.9em; font-weight: normal; }
	.Centered { text-align: center; }
	.AlignRight { text-align: right; }
	.Width12Percent { width: 12%; }
</style>
<div class="Summary">

<a name="top"></a>

<div class="Header">
	Invoice Summary Information
</div>

<p>Hello #Customer["FirstName"]#,</p>

<p>We''ve created this invoice for services you ordered. You can find this invoice details under your user account 
and below is the summary information.</p>

<ad:if test="#isdefined("IsEmail") and isdefined("ExtraArgs")#">
	<ad:if test="#not(isnull(ExtraArgs["InvoiceDirectURL"]))#">
	<p>You may use the following link to pay for invoice later <a href="#ExtraArgs["InvoiceDirectURL"]#">#ExtraArgs["InvoiceDirectURL"]#</a>.</p>
	</ad:if>
</ad:if>

<h1>Invoice Overview</h1>
<table>
    <tr>
        <td class="Label">Invoice ##:</td>
<ad:if test="#isnull(Invoice.InvoiceNumber)#">
        <td>#Invoice.InvoiceId#</td>
<ad:else>
        <td>#Invoice.InvoiceNumber#</td>
</ad:if>
    </tr>
	<tr>
        <td class="Label">Terms:</td>
        <td>Due Upon Receipt</td>
    </tr>
    <tr>
        <td class="Label">Invoice Date:</td>
        <td>#Invoice.Created.ToShortDateString()#</td>
    </tr>
</table>
<h1>Invoice Items</h1>
<table width="100%">
    <tr>
        <td class="Label">Description</td>
	<td class="Label Centered">Type</td>
        <td class="Label Centered">Qty</td>
		<td class="Label Centered">Unit Price</td>
		<td class="Label Centered">Total</td>
    </tr>
    <ad:foreach collection="#InvoiceLines#" var="Line" index="i">
	<tr>
		<td>#Line.ItemName#<ad:ShowBillingPeriod Item="#Line#" /></td>
		<td class="Centered Width12Percent">#Line.TypeName#</td>
		<td class="Centered Width12Percent">#Line.Quantity#</td>
		<td class="Centered Width12Percent">#Invoice.Currency#&nbsp;#Line.UnitPrice.ToString("C")#</td>
		<td class="Centered Width12Percent">#Invoice.Currency#&nbsp;#Line.Total.ToString("C")#</td>
	</tr>
	</ad:foreach>
	<tr>
		<td colspan="4" class="AlignRight">Sub Total&nbsp;</td>
		<td class="Centered">#Invoice.Currency#&nbsp;#Invoice.SubTotal.ToString("C")#</td>
	</tr>
	<ad:if test="#isnull(Tax)#">
	<tr>
		<td colspan="4" class="AlignRight">Taxes&nbsp;#format(0.00, "#.##")#%</td>
		<td class="Centered" nowrap="nowrap">#Invoice.Currency#&nbsp;#format(0.00, "C")#</td>
	</tr>
	<ad:elseif test="#equals(Tax.TypeId, 1)#">
	<tr>
		<td colspan="4" class="AlignRight">#Tax.Description#</td>
		<td class="Centered" nowrap="nowrap">#Invoice.Currency#&nbsp;#Tax.Amount.ToString("C")#</td>
	</tr>
	<ad:elseif test="#equals(Tax.TypeId, 2)#">
	<tr>
		<td colspan="4" class="AlignRight">#Tax.Description#&nbsp;#Tax.Amount.ToString("#.##")#%</td>
		<td class="Centered" nowrap="nowrap">#Invoice.Currency#&nbsp;#Invoice.TaxAmount.ToString("C")#</td>
	</tr>
	<ad:elseif test="#equals(Tax.TypeId, 3)#">
	<tr>
		<td colspan="4" class="AlignRight">#Tax.Description#&nbsp;#Tax.Amount.ToString("#.##")#%</td>
		<td class="Centered" nowrap="nowrap">#Invoice.Currency#&nbsp;#Invoice.TaxAmount.ToString("C")#</td>
	</tr>
	</ad:if>
	<tr>
		<td colspan="4" class="AlignRight">Total&nbsp;</td>
		<td class="Centered">#Invoice.Currency#&nbsp;#Invoice.Total.ToString("C")#</td>
	</tr>
</table>
<p>
If you have any questions regarding your hosting account or this invoice, feel free to contact our sales department at any time.
</p>

<p>Some notes regarding invoice or how to apply your enquiries...</p>

<p>
Best regards,<br />
ACME Hosting Inc.<br />
Web Site: <a href="http://www.acmehosting.com">www.acmehosting.com</a><br />
E-Mail: <a href="mailto:support@acmehosting.com">support@acmehosting.com</a></p>

</div>

<ad:template name="ShowBillingPeriod">
	<ad:if test="#InvoiceServices.ContainsKey(Item.ServiceId)#">
		<ad:set name="Service" value="#InvoiceServices[Item.ServiceId]#" />
		<ad:if test="#isnotempty(Service.BillingPeriod)#">&nbsp;(<b>#Service.PeriodLength#&nbsp;#Service.BillingPeriod#</b>)</ad:if>
	</ad:if>
</ad:template>')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'EmitInvoiceTemplate', N'Subject', N'Invoice Summary Information')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'EmitInvoiceTemplate', N'TextBody', N'================================
   Invoice Summary Information
================================

Hello #Customer["FirstName"]#,

We''ve created this invoice for services you ordered. You can find this invoice details under your user account 
and below is the summary information.

<ad:if test="#isdefined("IsEmail") and isdefined("ExtraArgs")#">
	<ad:if test="#not(isnull(ExtraArgs["InvoiceDirectURL"]))#">
You may copy and paste the following link to the browser in order to pay for invoice later #ExtraArgs["InvoiceDirectURL"]#.
	</ad:if>
</ad:if>

Invoice Overview
=============
Invoice ##: <ad:if test="#isnull(Invoice.InvoiceNumber)#">#Invoice.InvoiceId#<ad:else>#Invoice.InvoiceNumber#</ad:if>
Terms: Due Upon Receipt
Invoice Date: #Invoice.Created.ToShortDateString()#
Invoice SubTotal: #Invoice.Currency# #Invoice.SubTotal.ToString("C")#
<ad:if test="#isnull(Tax)#">
Taxes&nbsp;#format(0.00, "#.##")#%: #Invoice.Currency#&nbsp;#format(0.00, "C")#
<ad:elseif test="#equals(Tax.TypeId, 1)#">
#Tax.Description#: #Invoice.Currency#&nbsp;#Tax.Amount.ToString("C")#
<ad:elseif test="#equals(Tax.TypeId, 2)#">
#Tax.Description#&nbsp;#Tax.Amount.ToString("#.##")#%: #Invoice.Currency#&nbsp;#Invoice.TaxAmount.ToString("C")#
<ad:elseif test="#equals(Tax.TypeId, 3)#">
#Tax.Description#&nbsp;#Tax.Amount.ToString("#.##")#%: #Invoice.Currency#&nbsp;#Invoice.TaxAmount.ToString("C")#
</ad:if>
Invoice Total: #Invoice.Currency# #Invoice.Total.ToString("C")#


Invoice Items
=============
<ad:foreach collection="#InvoiceLines#" var="Line" index="i">
#Line.ItemName#<ad:ShowBillingPeriod Item="#Line#" />
	Type: #Line.TypeName#
	Quantity: #Line.Quantity#
	Unit Price: #Invoice.Currency#&nbsp;#Line.UnitPrice.ToString("C")#
	Total: #Invoice.Currency#&nbsp;#Line.Total.ToString("C")#
</ad:foreach>


If you have any questions regarding your hosting account, feel free to contact our sales department at any time.

Some notes regarding invoice or how to apply your enquiries...


Best regards,
ACME Hosting Inc.
Web Site: http://www.acmehosting.com
E-Mail: support@acmehosting.com

<ad:template name="ShowBillingPeriod">
	<ad:if test="#InvoiceServices.ContainsKey(Item.ServiceId)#">
		<ad:set name="Service" value="#InvoiceServices[Item.ServiceId]#" />
		<ad:if test="#isnotempty(Service.BillingPeriod)#"> (#Service.PeriodLength# #Service.BillingPeriod#)</ad:if>
	</ad:if>
</ad:template>')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'PaymentReceivedTemplate', N'CC', N'info@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'PaymentReceivedTemplate', N'From', N'sales@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'PaymentReceivedTemplate', N'HtmlBody', N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>Received Payment Summary Information</title>
	<style type="text/css">
		.Summary { background-color: ##ffffff; padding: 5px; }
		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }
        .Summary A { color: ##0153A4; }
        .Summary { font-family: Tahoma; font-size: 9pt; }
        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }
        .Summary H2 { font-size: 1.2em; } 
        .Summary TABLE { border: solid 1px ##e5e5e5; }
        .Summary TH,
        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }
        .Summary TD { padding: 8px; font-size: 9pt; }
        .Summary UL LI { font-size: 1.1em; font-weight: bold; }
        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }
    </style>
</head>
<body>
	<div class="Summary">
		<a name="top"></a>
		<div class="Header">
			Received Payment Information
		</div>
		<p>
			Hello #Customer["FirstName"]#,</p>
		<p>
			We''ve received your payment successfully. You can find this payment details under
			your user account and below is the summary information.</p>
		<h1>
			Payment Overview</h1>
		<table>
			<tr>
				<td class="Label">
					Payment ##:
				</td>
				<td>
					#payment.paymentid#
				</td>
			</tr>
			<tr>
				<td class="Label">
					Invoice ##:
				</td>
				<td>
					#payment.invoiceid#
				</td>
			</tr>
			<tr>
				<td class="Label">
					Transaction ID ##:
				</td>
				<td>
					#payment.transactionid#
				</td>
			</tr>
			<tr>
				<td class="Label">
					Created:
				</td>
				<td>
					#payment.created#
				</td>
			</tr>
			<tr>
				<td class="Label">
					Payment Method:
				</td>
				<td>
					#payment.methodname#
				</td>
			</tr>
			<tr>
				<td class="Label">
					Payment Status:
				</td>
				<td>
					#payment.status.ToString()#
				</td>
			</tr>
			<tr>
				<td class="Label">
					Payment Total:
				</td>
				<td>
					#payment.currency# #format(payment.total, "C")#
				</td>
			</tr>
		</table>
		<p>
			If you have any questions regarding your hosting account, feel free to contact our
			sales department at any time.
		</p>
		<p>
			Best regards,<br />
			ACME Hosting Inc.<br />
			Web Site: <a href="http://www.acmehosting.com">www.acmehosting.com</a><br />
			E-Mail: <a href="mailto:support@acmehosting.com">support@acmehosting.com</a></p>
	</div>
</body>
</html>
')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'PaymentReceivedTemplate', N'Subject', N'Payment Received Notification')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'PaymentReceivedTemplate', N'TextBody', N'================================ Received Payment Information ================================
Hello #Customer["FirstName"]#,

We''ve received your payment successfully. You can find this payment details under your user account and below is the summary information.

Payment Overview
=============
Payment ##: #payment.paymentid#
Invoice ##: #payment.invoiceid#
Transaction ID ##: #payment.transactionid#
Created: #payment.created#
Payment Method: #payment.methodname#
Payment Status: #payment.status.ToString()#
Payment Total: #payment.currency# #format(payment.total, "C")#

If you have any questions regarding your hosting account, feel free to contact our sales department at any time.

Best regards,
ACME Hosting Inc.
Web Site: http://www.acmehosting.com
E-Mail: support@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SignupSettings', N'NewbieStatus', N'4')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SignupSettings', N'RegDomainMode', N'OPT_DOM_AVAIL')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SignupSettings', N'SendRegistrationEmail', N'true')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SuspendServicesTemplate', N'CC', N'sales@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SuspendServicesTemplate', N'From', N'support@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SuspendServicesTemplate', N'HtmlBody', N'
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Suspend Service Summary Information</title>
    <style type="text/css">
		.Summary { background-color: ##ffffff; padding: 5px; }
		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }
        .Summary A { color: ##0153A4; }
        .Summary { font-family: Tahoma; font-size: 9pt; }
        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }
        .Summary H2 { font-size: 1.2em; } 
        .Summary TABLE { border: solid 1px ##e5e5e5; }
        .Summary TH,
        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }
        .Summary TD { padding: 8px; font-size: 9pt; }
        .Summary UL LI { font-size: 1.1em; font-weight: bold; }
        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }
    </style>
</head>
<body>
<div class="Summary">

<a name="top"></a>

<div class="Header">
	Suspended Service Information
</div>

<p>Hello #Customer["FirstName"]#,</p>

<p>"#service.ServiceName#" service has been suspended under your user account
and below is the summary information.</p>
<h1>Service Overview</h1>

<table>
    <tr>
        <td class="Label">Service Name:</td>
        <td>#service.ServiceName#</td>
    </tr>
	<tr>
        <td class="Label">Created:</td>
        <td>#service.Created#</td>
    </tr>
</table>

<p>
If you have any questions regarding your hosting account, feel free to contact our sales department at any time.
</p>

<p>
Best regards,<br />
ACME Hosting Inc.<br />
Web Site: <a href="http://www.acmehosting.com">www.acmehosting.com</a><br />
E-Mail: <a href="mailto:support@acmehosting.com">support@acmehosting.com</a></p>

</div>
</body>
</html>')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SuspendServicesTemplate', N'Subject', N'Suspend Service Notification')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SuspendServicesTemplate', N'TextBody', N'
================================
   Suspended Service Information
================================

Hello #Customer["FirstName"]#,

"#service.ServiceName#" service has been suspended under your user account
and below is the summary information.

Service Overview
=============
Service Name: #service.ServiceName#
Created: #service.Created#

If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards,
ACME Hosting Inc.
Web Site: http://www.acmehosting.com
E-Mail: support@acmehosting.com')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SystemSettings', N'BaseCurrency', N'USD')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SystemSettings', N'InvoiceGracePeriod', N'2')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SystemSettings', N'ServiceSuspendThreshold', N'3')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SystemSettings', N'SvcCancelThreshold', N'3')
GO
INSERT [dbo].[ecStoreDefaultSettings] ([SettingsName], [PropertyName], [PropertyValue]) VALUES (N'SystemSettings', N'SvcInvoiceThreshold', N'7')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[GetPackageIPAddresses]
(
	@PackageID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int,
	@PoolID int = 0,
	@Recursive bit = 0
)
AS
BEGIN


-- start
DECLARE @condition nvarchar(700)
SET @condition = '
((@Recursive = 0 AND PA.PackageID = @PackageID)
OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, PA.PackageID) = 1))
AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'IP.ExternalIP ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(PA.PackageAddressID)
FROM dbo.PackageIPAddresses PA
INNER JOIN dbo.IPAddresses AS IP ON PA.AddressID = IP.AddressID
INNER JOIN dbo.Packages P ON PA.PackageID = P.PackageID
INNER JOIN dbo.Users U ON U.UserID = P.UserID
LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
WHERE ' + @condition + '

DECLARE @Addresses AS TABLE
(
	PackageAddressID int
);

WITH TempItems AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		PA.PackageAddressID
	FROM dbo.PackageIPAddresses PA
	INNER JOIN dbo.IPAddresses AS IP ON PA.AddressID = IP.AddressID
	INNER JOIN dbo.Packages P ON PA.PackageID = P.PackageID
	INNER JOIN dbo.Users U ON U.UserID = P.UserID
	LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
	WHERE ' + @condition + '
)

INSERT INTO @Addresses
SELECT PackageAddressID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	PA.PackageAddressID,
	PA.AddressID,
	IP.ExternalIP,
	IP.InternalIP,
	IP.SubnetMask,
	IP.DefaultGateway,
	PA.ItemID,
	SI.ItemName,
	PA.PackageID,
	P.PackageName,
	P.UserID,
	U.UserName,
	PA.IsPrimary
FROM @Addresses AS TA
INNER JOIN dbo.PackageIPAddresses AS PA ON TA.PackageAddressID = PA.PackageAddressID
INNER JOIN dbo.IPAddresses AS IP ON PA.AddressID = IP.AddressID
INNER JOIN dbo.Packages P ON PA.PackageID = P.PackageID
INNER JOIN dbo.Users U ON U.UserID = P.UserID
LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
'

print @sql

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int, @Recursive bit, @PoolID int',
@PackageID, @StartRow, @MaximumRows, @Recursive, @PoolID

END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Versions](
	[DatabaseVersion] [varchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[BuildDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Versions] PRIMARY KEY CLUSTERED 
(
	[DatabaseVersion] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (N'1.0', CAST(0x00009D5400000000 AS DateTime))
GO
INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (N'1.0.1.0', CAST(0x00009DB500D453BD AS DateTime))
GO
INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (N'1.0.2.0', CAST(0x00009DE600000000 AS DateTime))
GO
INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (N'1.1.0.9', CAST(0x00009E3000000000 AS DateTime))
GO
INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (N'1.2.0.0', CAST(0x00009E4700000000 AS DateTime))
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clusters](
	[ClusterID] [int] IDENTITY(1,1) NOT NULL,
	[ClusterName] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_Clusters] PRIMARY KEY CLUSTERED 
(
	[ClusterID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecProductTypeControls](
	[TypeID] [int] NOT NULL,
	[ControlKey] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ControlSrc] [nvarchar](512) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_ecProductTypeControls] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC,
	[ControlKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecProductType](
	[TypeID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[ProvisioningController] [nvarchar](512) COLLATE Latin1_General_CI_AS NULL,
	[Created] [datetime] NOT NULL,
	[NativeItemType] [nvarchar](512) COLLATE Latin1_General_CI_AS NOT NULL,
	[ServiceItemType] [nvarchar](512) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_EC_ProductTypes] PRIMARY KEY CLUSTERED 
(
	[TypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET IDENTITY_INSERT [dbo].[ecProductType] ON 

GO
INSERT [dbo].[ecProductType] ([TypeID], [TypeName], [ProvisioningController], [Created], [NativeItemType], [ServiceItemType]) VALUES (1, N'Hosting Plan', N'WebsitePanel.Ecommerce.EnterpriseServer.HostingPackageController,WebsitePanel.EnterpriseServer', CAST(0x0000993E010F2F0C AS DateTime), N'WebsitePanel.Ecommerce.EnterpriseServer.HostingPlan, WebsitePanel.EnterpriseServer', N'WebsitePanel.Ecommerce.EnterpriseServer.HostingPackageSvc, WebsitePanel.EnterpriseServer')
GO
INSERT [dbo].[ecProductType] ([TypeID], [TypeName], [ProvisioningController], [Created], [NativeItemType], [ServiceItemType]) VALUES (2, N'Hosting Add-On', N'WebsitePanel.Ecommerce.EnterpriseServer.HostingAddonController,WebsitePanel.EnterpriseServer', CAST(0x0000993E010F2F0C AS DateTime), N'WebsitePanel.Ecommerce.EnterpriseServer.HostingAddon, WebsitePanel.EnterpriseServer', N'WebsitePanel.Ecommerce.EnterpriseServer.HostingAddonSvc, WebsitePanel.EnterpriseServer')
GO
INSERT [dbo].[ecProductType] ([TypeID], [TypeName], [ProvisioningController], [Created], [NativeItemType], [ServiceItemType]) VALUES (3, N'Domain Name', N'WebsitePanel.Ecommerce.EnterpriseServer.DomainNameController,WebsitePanel.EnterpriseServer', CAST(0x0000993E010F2F0C AS DateTime), N'WebsitePanel.Ecommerce.EnterpriseServer.DomainName, WebsitePanel.EnterpriseServer', N'WebsitePanel.Ecommerce.EnterpriseServer.DomainNameSvc, WebsitePanel.EnterpriseServer')
GO
SET IDENTITY_INSERT [dbo].[ecProductType] OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecCategory](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[CategorySku] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ParentID] [int] NULL,
	[Level] [int] NOT NULL,
	[ShortDescription] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[FullDescription] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NULL,
	[CreatorID] [int] NOT NULL,
	[ModifierID] [int] NULL,
	[ItemOrder] [int] NULL,
	[ResellerID] [int] NOT NULL,
 CONSTRAINT [PK_EC_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecContracts](
	[ContractID] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[CustomerID] [int] NULL,
	[ResellerID] [int] NOT NULL,
	[AccountName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[OpenedDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[ClosedDate] [datetime] NULL,
	[Balance] [money] NOT NULL,
	[FirstName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[LastName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Email] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[CompanyName] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[PropertyNames] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[PropertyValues] [ntext] COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_ecContracts] PRIMARY KEY CLUSTERED 
(
	[ContractID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [UQ_ecContracts_ContractID] UNIQUE NONCLUSTERED 
(
	[ContractID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecServiceHandlersResponses](
	[ResponseID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceID] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ContractID] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[InvoiceID] [int] NULL,
	[TextResponse] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[Received] [datetime] NOT NULL,
	[ErrorMessage] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_ecServiceHandlersResponses] PRIMARY KEY CLUSTERED 
(
	[ResponseID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecCustomersPayments](
	[PaymentID] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceID] [int] NOT NULL,
	[TransactionID] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Total] [money] NOT NULL,
	[Currency] [nvarchar](3) COLLATE Latin1_General_CI_AS NOT NULL,
	[Created] [datetime] NOT NULL,
	[MethodName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PluginID] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[ContractID] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_EC_Payments] PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
 CONSTRAINT [IX_ecCustomersPayments] UNIQUE NONCLUSTERED 
(
	[TransactionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecInvoice](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[Created] [datetime] NOT NULL,
	[DueDate] [datetime] NOT NULL,
	[Total] [money] NOT NULL,
	[SubTotal] [money] NOT NULL,
	[TaxAmount] [money] NULL,
	[Currency] [nvarchar](3) COLLATE Latin1_General_CI_AS NOT NULL,
	[InvoiceNumber] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[TaxationID] [int] NULL,
	[ContractID] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_EC_Invoices] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecTaxations](
	[TaxationID] [int] IDENTITY(1,1) NOT NULL,
	[ResellerID] [int] NOT NULL,
	[Country] [nvarchar](3) COLLATE Latin1_General_CI_AS NOT NULL,
	[State] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Description] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[TypeID] [int] NOT NULL,
	[Amount] [decimal](5, 2) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_ecTaxations_1] PRIMARY KEY CLUSTERED 
(
	[TaxationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_ecTaxations] ON [dbo].[ecTaxations] 
(
	[ResellerID] ASC,
	[Country] ASC,
	[State] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecSystemTriggers](
	[TriggerID] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[OwnerID] [int] NOT NULL,
	[TriggerHandler] [nvarchar](512) COLLATE Latin1_General_CI_AS NOT NULL,
	[ReferenceID] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Namespace] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Status] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_ecSystemTriggers] PRIMARY KEY CLUSTERED 
(
	[TriggerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecPluginsProperties](
	[PluginID] [int] NOT NULL,
	[PropertyName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyValue] [nvarchar](512) COLLATE Latin1_General_CI_AS NULL,
	[ResellerID] [int] NOT NULL,
 CONSTRAINT [PK_ecPluginsSettings] PRIMARY KEY CLUSTERED 
(
	[PluginID] ASC,
	[ResellerID] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecPaymentProfiles](
	[PropertyNames] [ntext] COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyValues] [ntext] COLLATE Latin1_General_CI_AS NOT NULL,
	[Created] [datetime] NOT NULL,
	[ContractID] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_ecPaymentProfiles] PRIMARY KEY CLUSTERED 
(
	[ContractID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE FUNCTION GetFullIPAddress
(
	@ExternalIP varchar(24),
	@InternalIP varchar(24)
)
RETURNS varchar(60)
AS
BEGIN
DECLARE @IP varchar(60)
SET @IP = ''

IF @ExternalIP IS NOT NULL AND @ExternalIP <> ''
SET @IP = @ExternalIP

IF @InternalIP IS NOT NULL AND @InternalIP <> ''
SET @IP = @IP + ' (' + @InternalIP + ')'

RETURN @IP
END

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetPackageSettings
(
	@ActorID int,
	@PackageID int,
	@SettingsName nvarchar(50)
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @ParentPackageID int, @TmpPackageID int
SET @TmpPackageID = @PackageID

WHILE 10 = 10
BEGIN
	IF @TmpPackageID < 2 -- system package
	BEGIN
		SELECT
			@TmpPackageID AS PackageID,
			'Dump' AS PropertyName,
			'' AS PropertyValue
	END
	ELSE
	BEGIN
		-- user package
		IF EXISTS
		(
			SELECT PropertyName FROM PackageSettings
			WHERE SettingsName = @SettingsName AND PackageID = @TmpPackageID
		)
		BEGIN
			SELECT
				PackageID,
				PropertyName,
				PropertyValue
			FROM
				PackageSettings
			WHERE
				PackageID = @TmpPackageID AND
				SettingsName = @SettingsName
				
			BREAK
		END
	END


	SET @ParentPackageID = NULL --reset var
	
	-- get owner
	SELECT
		@ParentPackageID = ParentPackageID
	FROM Packages
	WHERE PackageID = @TmpPackageID
	
	IF @ParentPackageID IS NULL -- the last parent
	BREAK
	
	SET @TmpPackageID = @ParentPackageID
END

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleTasks](
	[TaskID] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[TaskType] [nvarchar](500) COLLATE Latin1_General_CI_AS NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_ScheduleTasks] PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_ACTIVATE_PAID_INVOICES', N'WebsitePanel.Ecommerce.EnterpriseServer.ActivatePaidInvoicesTask, WebsitePanel.EnterpriseServer', 2)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_BACKUP', N'WebsitePanel.EnterpriseServer.BackupTask, WebsitePanel.EnterpriseServer', 1)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'WebsitePanel.EnterpriseServer.BackupDatabaseTask, WebsitePanel.EnterpriseServer', 3)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_CALCULATE_EXCHANGE_DISKSPACE', N'WebsitePanel.EnterpriseServer.CalculateExchangeDiskspaceTask, WebsitePanel.EnterpriseServer', 2)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH', N'WebsitePanel.EnterpriseServer.CalculatePackagesBandwidthTask, WebsitePanel.EnterpriseServer', 1)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE', N'WebsitePanel.EnterpriseServer.CalculatePackagesDiskspaceTask, WebsitePanel.EnterpriseServer', 1)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES', N'WebsitePanel.Ecommerce.EnterpriseServer.CancelOverdueInvoicesTask, WebsitePanel.EnterpriseServer', 2)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'WebsitePanel.EnterpriseServer.CheckWebSiteTask, WebsitePanel.EnterpriseServer', 3)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'WebsitePanel.EnterpriseServer.FTPFilesTask, WebsitePanel.EnterpriseServer', 3)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_GENERATE_INVOICES', N'WebsitePanel.Ecommerce.EnterpriseServer.GenerateInvoicesTask, WebsitePanel.EnterpriseServer', 2)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'WebsitePanel.EnterpriseServer.HostedSolutionReportTask, WebsitePanel.EnterpriseServer', 2)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_RUN_PAYMENT_QUEUE', N'WebsitePanel.Ecommerce.EnterpriseServer.RunPaymentQueueTask, WebsitePanel.EnterpriseServer', 2)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_RUN_SYSTEM_COMMAND', N'WebsitePanel.EnterpriseServer.RunSystemCommandTask, WebsitePanel.EnterpriseServer', 1)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_SEND_MAIL', N'WebsitePanel.EnterpriseServer.SendMailNotificationTask, WebsitePanel.EnterpriseServer', 3)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES', N'WebsitePanel.Ecommerce.EnterpriseServer.SuspendOverdueInvoicesTask, WebsitePanel.EnterpriseServer', 2)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'WebsitePanel.EnterpriseServer.SuspendOverusedPackagesTask, WebsitePanel.EnterpriseServer', 2)
GO
INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_ZIP_FILES', N'WebsitePanel.EnterpriseServer.ZipFilesTask, WebsitePanel.EnterpriseServer', 3)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetScheduleInternal
(
	@ScheduleID int
)
AS

-- select schedule
SELECT
	S.ScheduleID,
	S.TaskID,
	ST.TaskType,
	ST.RoleID,
	S.PackageID,
	S.ScheduleName,
	S.ScheduleTypeID,
	S.Interval,
	S.FromTime,
	S.ToTime,
	S.StartTime,
	S.LastRun,
	S.NextRun,
	S.Enabled,
	1 AS StatusID,
	S.PriorityID,
	S.HistoriesNumber,
	S.MaxExecutionTime,
	S.WeekMonthDay
FROM Schedule AS S
INNER JOIN ScheduleTasks AS ST ON S.TaskID = ST.TaskID
WHERE ScheduleID = @ScheduleID
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceItemTypes](
	[ItemTypeID] [int] NOT NULL,
	[GroupID] [int] NULL,
	[DisplayName] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[TypeName] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[TypeOrder] [int] NOT NULL,
	[CalculateDiskspace] [bit] NULL,
	[CalculateBandwidth] [bit] NULL,
	[Suspendable] [bit] NULL,
	[Disposable] [bit] NULL,
	[Searchable] [bit] NULL,
	[Importable] [bit] NOT NULL,
	[Backupable] [bit] NOT NULL,
 CONSTRAINT [PK_ServiceItemTypes] PRIMARY KEY CLUSTERED 
(
	[ItemTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (1, 9, N'SharePointUser', N'WebsitePanel.Providers.OS.SystemUser, WebsitePanel.Providers.Base', 19, 0, 0, 1, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (2, 1, N'HomeFolder', N'WebsitePanel.Providers.OS.HomeFolder, WebsitePanel.Providers.Base', 15, 1, 0, 0, 1, 0, 0, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (3, 9, N'SharePointGroup', N'WebsitePanel.Providers.OS.SystemGroup, WebsitePanel.Providers.Base', 20, 0, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (5, 5, N'MsSQL2000Database', N'WebsitePanel.Providers.Database.SqlDatabase, WebsitePanel.Providers.Base', 9, 1, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (6, 5, N'MsSQL2000User', N'WebsitePanel.Providers.Database.SqlUser, WebsitePanel.Providers.Base', 10, 0, 0, 1, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (7, 6, N'MySQL4Database', N'WebsitePanel.Providers.Database.SqlDatabase, WebsitePanel.Providers.Base', 13, 1, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (8, 6, N'MySQL4User', N'WebsitePanel.Providers.Database.SqlUser, WebsitePanel.Providers.Base', 14, 0, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (9, 3, N'FTPAccount', N'WebsitePanel.Providers.FTP.FtpAccount, WebsitePanel.Providers.Base', 3, 0, 1, 1, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (10, 2, N'WebSite', N'WebsitePanel.Providers.Web.WebSite, WebsitePanel.Providers.Base', 2, 1, 1, 1, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (11, 4, N'MailDomain', N'WebsitePanel.Providers.Mail.MailDomain, WebsitePanel.Providers.Base', 8, 0, 1, 1, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (12, 7, N'DNSZone', N'WebsitePanel.Providers.DNS.DnsZone, WebsitePanel.Providers.Base', 0, 0, 0, 1, 1, 0, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (13, 1, N'Domain', N'WebsitePanel.Providers.OS.Domain, WebsitePanel.Providers.Base', 1, 0, 0, 0, 0, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (14, 8, N'StatisticsSite', N'WebsitePanel.Providers.Statistics.StatsSite, WebsitePanel.Providers.Base', 17, 0, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (15, 4, N'MailAccount', N'WebsitePanel.Providers.Mail.MailAccount, WebsitePanel.Providers.Base', 4, 1, 0, 0, 0, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (16, 4, N'MailAlias', N'WebsitePanel.Providers.Mail.MailAlias, WebsitePanel.Providers.Base', 5, 0, 0, 0, 0, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (17, 4, N'MailList', N'WebsitePanel.Providers.Mail.MailList, WebsitePanel.Providers.Base', 7, 0, 0, 0, 0, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (18, 4, N'MailGroup', N'WebsitePanel.Providers.Mail.MailGroup, WebsitePanel.Providers.Base', 6, 0, 0, 0, 0, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (19, 9, N'SharePointSite', N'WebsitePanel.Providers.SharePoint.SharePointSite, WebsitePanel.Providers.Base', 18, 0, 0, 0, 1, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (20, 1, N'ODBCDSN', N'WebsitePanel.Providers.OS.SystemDSN, WebsitePanel.Providers.Base', 22, 0, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (21, 10, N'MsSQL2005Database', N'WebsitePanel.Providers.Database.SqlDatabase, WebsitePanel.Providers.Base', 11, 1, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (22, 10, N'MsSQL2005User', N'WebsitePanel.Providers.Database.SqlUser, WebsitePanel.Providers.Base', 12, 0, 0, 1, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (23, 11, N'MySQL5Database', N'WebsitePanel.Providers.Database.SqlDatabase, WebsitePanel.Providers.Base', 15, 1, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (24, 11, N'MySQL5User', N'WebsitePanel.Providers.Database.SqlUser, WebsitePanel.Providers.Base', 16, 0, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (25, 2, N'SharedSSLFolder', N'WebsitePanel.Providers.Web.SharedSSLFolder, WebsitePanel.Providers.Base', 21, 0, 0, 0, 1, 1, 0, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (28, 7, N'SecondaryDNSZone', N'WebsitePanel.Providers.DNS.SecondaryDnsZone, WebsitePanel.Providers.Base', 0, 0, 0, 1, 1, 0, 0, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (29, 13, N'Organization', N'WebsitePanel.Providers.HostedSolution.Organization, WebsitePanel.Providers.Base', 1, 1, 0, 1, 1, 1, 0, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (30, 13, N'OrganizationDomain', N'WebsitePanel.Providers.HostedSolution.OrganizationDomain, WebsitePanel.Providers.Base', 1, NULL, NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (31, 22, N'MsSQL2008Database', N'WebsitePanel.Providers.Database.SqlDatabase, WebsitePanel.Providers.Base', 1, 1, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (32, 22, N'MsSQL2008User', N'WebsitePanel.Providers.Database.SqlUser, WebsitePanel.Providers.Base', 1, 0, 0, 0, 1, 1, 1, 1)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (33, 30, N'VirtualMachine', N'WebsitePanel.Providers.Virtualization.VirtualMachine, WebsitePanel.Providers.Base', 1, 0, 0, 1, 1, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (34, 30, N'VirtualSwitch', N'WebsitePanel.Providers.Virtualization.VirtualSwitch, WebsitePanel.Providers.Base', 2, 0, 0, 1, 1, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (40, 33, N'ExchangeOrganization', N'WebsitePanel.Providers.ExchangeHostedEdition.ExchangeOrganization, WebsitePanel.Providers.Base', 1, 0, 0, 1, 1, 1, 0, 0)
GO
INSERT [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (200, 20, N'SharePointSiteCollection', N'WebsitePanel.Providers.SharePoint.SharePointSiteCollection, WebsitePanel.Providers.Base', 25, 1, 0, 0, 1, 1, 1, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE UpdatePackageDiskSpace
(
	@PackageID int,
	@xml ntext
)
AS
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
-- Execute a SELECT statement that uses the OPENXML rowset provider.

DECLARE @Items TABLE
(
	ItemID int,
	Bytes bigint
)

INSERT INTO @Items (ItemID, Bytes)
SELECT ItemID, DiskSpace FROM OPENXML (@idoc, '/items/item',1) 
WITH
(
	ItemID int '@id',
	DiskSpace bigint '@bytes'
) as XSI

-- remove current diskspace
DELETE FROM PackagesDiskspace
WHERE PackageID = @PackageID

-- update package diskspace
INSERT INTO PackagesDiskspace (PackageID, GroupID, Diskspace)
SELECT
	@PackageID,
	SIT.GroupID,
	SUM(I.Bytes)
FROM @Items AS I
INNER JOIN ServiceItems AS SI ON I.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
GROUP BY SIT.GroupID

-- remove document
exec sp_xml_removedocument @idoc

RETURN































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE UpdatePackageBandwidth
(
	@PackageID int,
	@xml ntext
)
AS
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml


DECLARE @Items TABLE
(
	ItemID int,
	LogDate datetime,
	BytesSent bigint,
	BytesReceived bigint
)

INSERT INTO @Items
(
	ItemID,
	LogDate,
	BytesSent,
	BytesReceived	
)
SELECT
	ItemID,
	CONVERT(datetime, LogDate, 101),
	BytesSent,
	BytesReceived
FROM OPENXML(@idoc, '/items/item',1) WITH 
(
	ItemID int '@id',
	LogDate nvarchar(10) '@date',
    BytesSent bigint '@sent',
    BytesReceived bigint '@received'
)

-- delete current statistics
DELETE FROM PackagesBandwidth
FROM PackagesBandwidth AS PB
INNER JOIN (
	SELECT
		SIT.GroupID,
		I.LogDate
	FROM @Items AS I
	INNER JOIN ServiceItems AS SI ON I.ItemID = SI.ItemID
	INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
	GROUP BY I.LogDate, SIT.GroupID
) AS STAT ON PB.LogDate = STAT.LogDate AND PB.GroupID = STAT.GroupID
WHERE PB.PackageID = @PackageID

-- insert new statistics
INSERT INTO PackagesBandwidth (PackageID, GroupID, LogDate, BytesSent, BytesReceived)
SELECT
	@PackageID,
	SIT.GroupID,
	I.LogDate,
	SUM(I.BytesSent),
	SUM(I.BytesReceived)
FROM @Items AS I
INNER JOIN ServiceItems AS SI ON I.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
GROUP BY I.LogDate, SIT.GroupID

-- remove document
exec sp_xml_removedocument @idoc

RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[GetServiceItemsPaged]
(
	@ActorID int,
	@PackageID int,
	@ItemTypeName nvarchar(200),
	@GroupName nvarchar(100) = NULL,
	@ServerID int,
	@Recursive bit,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS


-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- start
DECLARE @GroupID int
SELECT @GroupID = GroupID FROM ResourceGroups
WHERE GroupName = @GroupName

DECLARE @ItemTypeID int
SELECT @ItemTypeID = ItemTypeID FROM ServiceItemTypes
WHERE TypeName = @ItemTypeName
AND ((@GroupID IS NULL) OR (@GroupID IS NOT NULL AND GroupID = @GroupID))

DECLARE @condition nvarchar(700)
SET @condition = 'SI.ItemTypeID = @ItemTypeID
AND ((@Recursive = 0 AND P.PackageID = @PackageID)
		OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, P.PackageID) = 1))
AND ((@GroupID IS NULL) OR (@GroupID IS NOT NULL AND IT.GroupID = @GroupID))
AND (@ServerID = 0 OR (@ServerID > 0 AND S.ServerID = @ServerID))
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'SI.ItemName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(SI.ItemID) FROM Packages AS P
INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
INNER JOIN ServiceItemTypes AS IT ON SI.ItemTypeID = IT.ItemTypeID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
WHERE ' + @condition + '

DECLARE @Items AS TABLE
(
	ItemID int
);

WITH TempItems AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		SI.ItemID
	FROM Packages AS P
	INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	INNER JOIN ServiceItemTypes AS IT ON SI.ItemTypeID = IT.ItemTypeID
	INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
	INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
	WHERE ' + @condition + '
)

INSERT INTO @Items
SELECT ItemID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	IT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	SI.CreatedDate,
	RG.GroupName,

	-- packages
	P.PackageName,

	-- server
	ISNULL(SRV.ServerID, 0) AS ServerID,
	ISNULL(SRV.ServerName, '''') AS ServerName,
	ISNULL(SRV.Comments, '''') AS ServerComments,
	ISNULL(SRV.VirtualServer, 0) AS VirtualServer,

	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM @Items AS TSI
INNER JOIN ServiceItems AS SI ON TSI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS IT ON SI.ItemTypeID = IT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN ResourceGroups AS RG ON IT.GroupID = RG.GroupID


SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS TSI ON IP.ItemID = TSI.ItemID'

--print @sql

exec sp_executesql @sql, N'@ItemTypeID int, @PackageID int, @GroupID int, @StartRow int, @MaximumRows int, @Recursive bit, @ServerID int',
@ItemTypeID, @PackageID, @GroupID, @StartRow, @MaximumRows, @Recursive, @ServerID

RETURN 


















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[GetServiceItemsForStatistics]
(
	@ActorID int,
	@ServiceID int,
	@PackageID int,
	@CalculateDiskspace bit,
	@CalculateBandwidth bit,
	@Suspendable bit,
	@Disposable bit
)
AS
DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
WHERE
	((@ServiceID = 0) OR (@ServiceID > 0 AND SI.ServiceID = @ServiceID))
	AND ((@PackageID = 0) OR (@PackageID > 0 AND SI.PackageID = @PackageID))
	AND ((@CalculateDiskspace = 0) OR (@CalculateDiskspace = 1 AND SIT.CalculateDiskspace = @CalculateDiskspace))
	AND ((@CalculateBandwidth = 0) OR (@CalculateBandwidth = 1 AND SIT.CalculateBandwidth = @CalculateBandwidth))
	AND ((@Suspendable = 0) OR (@Suspendable = 1 AND SIT.Suspendable = @Suspendable))
	AND ((@Disposable = 0) OR (@Disposable = 1 AND SIT.Disposable = @Disposable))

-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	RG.GroupName,
	SIT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN ResourceGroups AS RG ON SIT.GroupID = RG.GroupID
ORDER BY RG.GroupOrder DESC, SI.ItemName

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID

RETURN





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[GetServiceItemsCount]
(
	@ItemTypeName nvarchar(200),
	@GroupName nvarchar(100) = NULL,
	@ServiceID int = 0,
	@TotalNumber int OUTPUT
)
AS

SET @TotalNumber = 0

-- find service items
SELECT
	@TotalNumber = COUNT(SI.ItemID)
FROM ServiceItems AS SI
INNER JOIN ServiceItemTypes AS IT ON SI.ItemTypeID = IT.ItemTypeID
INNER JOIN ResourceGroups AS RG ON IT.GroupID = RG.GroupID
WHERE IT.TypeName = @ItemTypeName
AND ((@GroupName IS NULL) OR (@GroupName IS NOT NULL AND RG.GroupName = @GroupName))
AND ((@ServiceID = 0) OR (@ServiceID > 0 AND SI.ServiceID = @ServiceID))

RETURN


















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE CheckServiceItemExistsInService
(
	@Exists bit OUTPUT,
	@ServiceID int,
	@ItemName nvarchar(500),
	@ItemTypeName nvarchar(200)
)
AS

SET @Exists = 0

DECLARE @ItemTypeID int
SELECT @ItemTypeID = ItemTypeID FROM ServiceItemTypes
WHERE TypeName = @ItemTypeName

IF EXISTS (SELECT ItemID FROM ServiceItems
WHERE ItemName = @ItemName AND ItemTypeID = @ItemTypeID AND ServiceID = @ServiceID)
SET @Exists = 1

RETURN

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE UpdateHostingPlanQuotas
(
	@ActorID int,
	@PlanID int,
	@Xml ntext
)
AS

/*
XML Format:

<plan>
	<groups>
		<group id="16" enabled="1" calculateDiskSpace="1" calculateBandwidth="1"/>
	</groups>
	<quotas>
		<quota id="2" value="2"/>
	</quotas>
</plan>

*/

-- check rights
DECLARE @UserID int
SELECT @UserID = UserID FROM HostingPlans
WHERE PlanID = @PlanID

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- delete old HP resources
DELETE FROM HostingPlanResources
WHERE PlanID = @PlanID

-- delete old HP quotas
DELETE FROM HostingPlanQuotas
WHERE PlanID = @PlanID

-- update HP resources
INSERT INTO HostingPlanResources
(
	PlanID,
	GroupID,
	CalculateDiskSpace,
	CalculateBandwidth
)
SELECT
	@PlanID,
	GroupID,
	CalculateDiskSpace,
	CalculateBandwidth
FROM OPENXML(@idoc, '/plan/groups/group',1) WITH 
(
	GroupID int '@id',
	CalculateDiskSpace bit '@calculateDiskSpace',
	CalculateBandwidth bit '@calculateBandwidth'
) as XRG

-- update HP quotas
INSERT INTO HostingPlanQuotas
(
	PlanID,
	QuotaID,
	QuotaValue
)
SELECT
	@PlanID,
	QuotaID,
	QuotaValue
FROM OPENXML(@idoc, '/plan/quotas/quota',1) WITH 
(
	QuotaID int '@id',
	QuotaValue int '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

RETURN 

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE AddHostingPlan
(
	@ActorID int,
	@PlanID int OUTPUT,
	@UserID int,
	@PackageID int,
	@PlanName nvarchar(200),
	@PlanDescription ntext,
	@Available bit,
	@ServerID int,
	@SetupPrice money,
	@RecurringPrice money,
	@RecurrenceLength int,
	@RecurrenceUnit int,
	@IsAddon bit,
	@QuotasXml ntext
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

BEGIN TRAN

IF @ServerID = 0
SELECT @ServerID = ServerID FROM Packages
WHERE PackageID = @PackageID

IF @IsAddon = 1
SET @ServerID = NULL

IF @PackageID = 0 SET @PackageID = NULL

INSERT INTO HostingPlans
(
	UserID,
	PackageID,
	PlanName,
	PlanDescription,
	Available,
	ServerID,
	SetupPrice,
	RecurringPrice,
	RecurrenceLength,
	RecurrenceUnit,
	IsAddon
)
VALUES
(
	@UserID,
	@PackageID,
	@PlanName,
	@PlanDescription,
	@Available,
	@ServerID,
	@SetupPrice,
	@RecurringPrice,
	@RecurrenceLength,
	@RecurrenceUnit,
	@IsAddon
)

SET @PlanID = SCOPE_IDENTITY()

-- save quotas
EXEC UpdateHostingPlanQuotas @ActorID, @PlanID, @QuotasXml

COMMIT TRAN
RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleTaskParameters](
	[TaskID] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[ParameterID] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[DataTypeID] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[DefaultValue] [nvarchar](140) COLLATE Latin1_General_CI_AS NULL,
	[ParameterOrder] [int] NOT NULL,
 CONSTRAINT [PK_ScheduleTaskParameters] PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC,
	[ParameterID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP', N'BACKUP_FILE_NAME', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP', N'DELETE_TEMP_BACKUP', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP', N'STORE_PACKAGE_FOLDER', N'String', N'\', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP', N'STORE_PACKAGE_ID', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP', N'STORE_SERVER_FOLDER', N'String', N'', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'BACKUP_FOLDER', N'String', N'\backups', 3)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'BACKUP_NAME', N'String', N'database_backup.bak', 4)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'DATABASE_GROUP', N'List', N'MsSQL2000=SQL Server 2000;MsSQL2005=SQL Server 2005;MsSQL2008=SQL Server 2008;MySQL4=MySQL 4.0;MySQL5=MySQL 5.0', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'DATABASE_NAME', N'String', N'', 2)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'ZIP_BACKUP', N'List', N'true=Yes;false=No', 5)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'MAIL_BODY', N'MultiString', N'', 10)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'MAIL_FROM', N'String', N'admin@mysite.com', 7)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'MAIL_SUBJECT', N'String', N'Web Site is unavailable', 9)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'MAIL_TO', N'String', N'admin@mysite.com', 8)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'PASSWORD', N'String', NULL, 3)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'RESPONSE_CONTAIN', N'String', NULL, 5)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'RESPONSE_DOESNT_CONTAIN', N'String', NULL, 6)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'RESPONSE_STATUS', N'String', N'500', 4)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'URL', N'String', N'http://', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'USE_RESPONSE_CONTAIN', N'Boolean', N'false', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'USE_RESPONSE_DOESNT_CONTAIN', N'Boolean', N'false', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'USE_RESPONSE_STATUS', N'Boolean', N'false', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'USERNAME', N'String', NULL, 2)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'FILE_PATH', N'String', N'\', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'FTP_FOLDER', N'String', NULL, 5)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'FTP_PASSWORD', N'String', NULL, 4)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'FTP_SERVER', N'String', N'ftp.myserver.com', 2)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'FTP_USERNAME', N'String', NULL, 3)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'CRM_REPORT', N'Boolean', N'true', 3)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'EMAIL', N'String', NULL, 5)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'EXCHANGE_REPORT', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'ORGANIZATION_REPORT', N'Boolean', N'true', 4)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'SHAREPOINT_REPORT', N'Boolean', N'true', 2)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_RUN_SYSTEM_COMMAND', N'EXECUTABLE_PARAMS', N'String', N'', 3)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_RUN_SYSTEM_COMMAND', N'EXECUTABLE_PATH', N'String', N'Executable.exe', 2)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_RUN_SYSTEM_COMMAND', N'SERVER_NAME', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SEND_MAIL', N'MAIL_BODY', N'MultiString', NULL, 4)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SEND_MAIL', N'MAIL_FROM', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SEND_MAIL', N'MAIL_SUBJECT', N'String', NULL, 3)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SEND_MAIL', N'MAIL_TO', N'String', NULL, 2)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'BANDWIDTH_OVERUSED', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'DISKSPACE_OVERUSED', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SEND_SUSPENSION_EMAIL', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SEND_WARNING_EMAIL', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SUSPEND_OVERUSED', N'Boolean', N'true', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SUSPENSION_MAIL_BCC', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SUSPENSION_MAIL_BODY', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SUSPENSION_MAIL_FROM', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SUSPENSION_MAIL_SUBJECT', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'SUSPENSION_USAGE_THRESHOLD', N'String', N'100', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'WARNING_MAIL_BCC', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'WARNING_MAIL_BODY', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'WARNING_MAIL_FROM', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'WARNING_MAIL_SUBJECT', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'WARNING_USAGE_THRESHOLD', N'String', N'80', 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_ZIP_FILES', N'FOLDER', N'String', NULL, 1)
GO
INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_ZIP_FILES', N'ZIP_FILE', N'String', N'\archive.zip', 2)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetSchedules
(
	@ActorID int,
	@PackageID int,
	@Recursive bit
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @Schedules TABLE
(
	ScheduleID int
)

INSERT INTO @Schedules (ScheduleID)
SELECT
	S.ScheduleID
FROM Schedule AS S
INNER JOIN PackagesTree(@PackageID, @Recursive) AS PT ON S.PackageID = PT.PackageID
ORDER BY S.Enabled DESC, S.NextRun
	

-- select schedules
SELECT
	S.ScheduleID,
	S.TaskID,
	ST.TaskType,
	ST.RoleID,
	S.PackageID,
	S.ScheduleName,
	S.ScheduleTypeID,
	S.Interval,
	S.FromTime,
	S.ToTime,
	S.StartTime,
	S.LastRun,
	S.NextRun,
	S.Enabled,
	1 AS StatusID,
	S.PriorityID,
	S.MaxExecutionTime,
	S.WeekMonthDay,
	ISNULL(0, (SELECT TOP 1 SeverityID FROM AuditLog WHERE ItemID = S.ScheduleID AND SourceName = 'SCHEDULER' ORDER BY StartDate DESC)) AS LastResult,

	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM @Schedules AS STEMP
INNER JOIN Schedule AS S ON STEMP.ScheduleID = S.ScheduleID
INNER JOIN Packages AS P ON S.PackageID = P.PackageID
INNER JOIN ScheduleTasks AS ST ON S.TaskID = ST.TaskID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID

-- select schedule parameters
SELECT
	S.ScheduleID,
	STP.ParameterID,
	STP.DataTypeID,
	ISNULL(SP.ParameterValue, STP.DefaultValue) AS ParameterValue
FROM @Schedules AS STEMP
INNER JOIN Schedule AS S ON STEMP.ScheduleID = S.ScheduleID
INNER JOIN ScheduleTaskParameters AS STP ON S.TaskID = STP.TaskID
LEFT OUTER JOIN ScheduleParameters AS SP ON STP.ParameterID = SP.ParameterID AND SP.ScheduleID = S.ScheduleID
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[GetScheduleParameters]
(
	@ActorID int,
	@TaskID nvarchar(100),
	@ScheduleID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT @PackageID = PackageID FROM Schedule
WHERE ScheduleID = @ScheduleID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	@ScheduleID AS ScheduleID,
	STP.ParameterID,
	STP.DataTypeID,
	SP.ParameterValue,
	STP.DefaultValue
FROM ScheduleTaskParameters AS STP
LEFT OUTER JOIN ScheduleParameters AS SP ON STP.ParameterID = SP.ParameterID AND SP.ScheduleID = @ScheduleID
WHERE STP.TaskID = @TaskID
ORDER BY STP.ParameterOrder

RETURN

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetSchedule
(
	@ActorID int,
	@ScheduleID int
)
AS

-- select schedule
SELECT TOP 1
	S.ScheduleID,
	S.TaskID,
	S.PackageID,
	S.ScheduleName,
	S.ScheduleTypeID,
	S.Interval,
	S.FromTime,
	S.ToTime,
	S.StartTime,
	S.LastRun,
	S.NextRun,
	S.Enabled,
	S.HistoriesNumber,
	S.PriorityID,
	S.MaxExecutionTime,
	S.WeekMonthDay,
	1 AS StatusID
FROM Schedule AS S
WHERE
	S.ScheduleID = @ScheduleID
	AND dbo.CheckActorPackageRights(@ActorID, S.PackageID) = 1

-- select task
SELECT
	ST.TaskID,
	ST.TaskType,
	ST.RoleID
FROM Schedule AS S
INNER JOIN ScheduleTasks AS ST ON S.TaskID = ST.TaskID
WHERE
	S.ScheduleID = @ScheduleID
	AND dbo.CheckActorPackageRights(@ActorID, S.PackageID) = 1

-- select schedule parameters
SELECT
	S.ScheduleID,
	STP.ParameterID,
	STP.DataTypeID,
	ISNULL(SP.ParameterValue, STP.DefaultValue) AS ParameterValue
FROM Schedule AS S
INNER JOIN ScheduleTaskParameters AS STP ON S.TaskID = STP.TaskID
LEFT OUTER JOIN ScheduleParameters AS SP ON STP.ParameterID = SP.ParameterID AND SP.ScheduleID = S.ScheduleID
WHERE
	S.ScheduleID = @ScheduleID
	AND dbo.CheckActorPackageRights(@ActorID, S.PackageID) = 1

RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetNextSchedule
AS

-- find next schedule
DECLARE @ScheduleID int
DECLARE @TaskID nvarchar(100)
SELECT TOP 1
	@ScheduleID = ScheduleID,
	@TaskID = TaskID
FROM Schedule AS S
WHERE Enabled = 1
ORDER BY NextRun ASC

-- select schedule
SELECT TOP 1
	S.ScheduleID,
	S.TaskID,
	S.PackageID,
	S.ScheduleName,
	S.ScheduleTypeID,
	S.Interval,
	S.FromTime,
	S.ToTime,
	S.StartTime,
	S.LastRun,
	S.NextRun,
	S.Enabled,
	S.HistoriesNumber,
	S.PriorityID,
	S.MaxExecutionTime,
	S.WeekMonthDay,
	1 AS StatusID
FROM Schedule AS S
WHERE S.ScheduleID = @ScheduleID
ORDER BY NextRun ASC

-- select task
SELECT
	TaskID,
	TaskType,
	RoleID
FROM ScheduleTasks
WHERE TaskID = @TaskID

-- select schedule parameters
SELECT
	S.ScheduleID,
	STP.ParameterID,
	STP.DataTypeID,
	ISNULL(SP.ParameterValue, STP.DefaultValue) AS ParameterValue
FROM Schedule AS S
INNER JOIN ScheduleTaskParameters AS STP ON S.TaskID = STP.TaskID
LEFT OUTER JOIN ScheduleParameters AS SP ON STP.ParameterID = SP.ParameterID AND SP.ScheduleID = S.ScheduleID
WHERE S.ScheduleID = @ScheduleID
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceGroupDnsRecords](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[RecordOrder] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[RecordType] [varchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[RecordName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[RecordData] [nvarchar](200) COLLATE Latin1_General_CI_AS NOT NULL,
	[MXPriority] [int] NULL,
 CONSTRAINT [PK_ResourceGroupDnsRecords] PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET IDENTITY_INSERT [dbo].[ResourceGroupDnsRecords] ON 

GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (1, 1, 2, N'A', N'', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (2, 2, 2, N'A', N'*', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (3, 3, 2, N'A', N'www', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (4, 1, 3, N'A', N'ftp', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (5, 1, 4, N'A', N'mail', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (6, 2, 4, N'A', N'mail2', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (7, 3, 4, N'MX', N'', N'mail.[DOMAIN_NAME]', 10)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (9, 4, 4, N'MX', N'', N'mail2.[DOMAIN_NAME]', 21)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (10, 1, 5, N'A', N'mssql', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (11, 1, 6, N'A', N'mysql', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (12, 1, 8, N'A', N'stats', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (13, 5, 4, N'TXT', N'', N'v=spf1 a mx -all', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (14, 1, 12, N'A', N'smtp', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (15, 2, 12, N'MX', N'', N'smtp.[DOMAIN_NAME]', 10)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (16, 3, 12, N'CNAME', N'autodiscover', N'', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (17, 4, 12, N'CNAME', N'owa', N'', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (18, 1, 33, N'A', N'smtp', N'[IP]', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (19, 2, 33, N'MX', N'', N'smtp.[DOMAIN_NAME]', 10)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (20, 3, 33, N'CNAME', N'autodiscover', N'', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (21, 4, 33, N'CNAME', N'owa', N'', 0)
GO
INSERT [dbo].[ResourceGroupDnsRecords] ([RecordID], [RecordOrder], [GroupID], [RecordType], [RecordName], [RecordData], [MXPriority]) VALUES (22, 5, 33, N'CNAME', N'ecp', N'', 0)
GO
SET IDENTITY_INSERT [dbo].[ResourceGroupDnsRecords] OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecUpdateContract]
	@ContractID nvarchar(50),
	@CustomerID int,
	@AccountName nvarchar(50),
	@Status int,
	@Balance money,
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(255),
	@CompanyName nvarchar(50),
	@PropertyNames ntext,
	@PropertyValues ntext,
	@Result int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SET @Result = 0;

	IF @CustomerID < 1
		SET @CustomerID = NULL;

	UPDATE [dbo].[ecContracts] SET
		[CustomerID] = @CustomerID,
		[AccountName] = @AccountName,
		[Status] = @Status,
		[Balance] = @Balance,
		[FirstName] = @FirstName,
		[LastName] = @LastName,
		[Email] = @Email,
		[CompanyName] = @CompanyName,
		[PropertyNames] = @PropertyNames,
		[PropertyValues] = @PropertyValues
	WHERE
		[ContractID] = @ContractID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecInvoiceItems](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceID] [int] NOT NULL,
	[ServiceID] [int] NULL,
	[ItemName] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[TypeName] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[Quantity] [int] NOT NULL,
	[Total] [money] NOT NULL,
	[SubTotal] [money] NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[Processed] [bit] NOT NULL,
 CONSTRAINT [PK_EC_InvoiceItems] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecPaymentMethods](
	[ResellerID] [int] NOT NULL,
	[MethodName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PluginID] [int] NOT NULL,
	[DisplayName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[SupportedItems] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_ecPaymentMethods] PRIMARY KEY CLUSTERED 
(
	[ResellerID] ASC,
	[MethodName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecLookupForTransaction]
	@TransactionID nvarchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM [dbo].[ecCustomersPayments] WHERE [TransactionID] = @TransactionID;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecService](
	[ServiceID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[TypeID] [int] NULL,
	[Status] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NULL,
	[ParentID] [int] NULL,
	[ContractID] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_SpaceInstances] PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



















CREATE PROCEDURE [dbo].[ecCheckCustomerContractExists]
	@CustomerID int,
	@Result bit OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF EXISTS (SELECT * FROM [ecContracts] WHERE [CustomerID] = @CustomerID)
	BEGIN
		SET @Result = 1;
		RETURN;
	END

	SET @Result = 0;

END























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[ItemTypeID] [varchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[ItemID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CommentText] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
	[SeverityID] [int] NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE FUNCTION [dbo].[CheckUserParent]
(
	@OwnerID int,
	@UserID int
)
RETURNS bit
AS
BEGIN

-- check if the user requests himself
IF @OwnerID = @UserID
BEGIN
	RETURN 1
END

-- check if the owner is peer
DECLARE @IsPeer int, @TmpOwnerID int
SELECT @IsPeer = IsPeer, @TmpOwnerID = OwnerID FROM Users
WHERE UserID = @OwnerID

IF @IsPeer = 1
SET @OwnerID = @TmpOwnerID

-- check if the user requests himself
IF @OwnerID = @UserID
BEGIN
	RETURN 1
END

DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
		BREAK
	
	IF @ParentUserID = @OwnerID
	RETURN 1
	
	SET @TmpUserID = @ParentUserID
END


RETURN 0
END



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE FUNCTION GetItemComments
(
	@ItemID int,
	@ItemTypeID varchar(50),
	@ActorID int
)
RETURNS nvarchar(3000)
AS
BEGIN
DECLARE @text nvarchar(3000)
SET @text = ''

SELECT @text = @text + U.Username + ' - ' + CONVERT(nvarchar(50), C.CreatedDate) + '
' + CommentText + '
--------------------------------------
' FROM Comments AS C
INNER JOIN UsersDetailed AS U ON C.UserID = U.UserID
WHERE
	ItemID = @ItemID
	AND ItemTypeID = @ItemTypeID
	AND dbo.CheckUserParent(@ActorID, C.UserID) = 1
ORDER BY C.CreatedDate DESC

RETURN @text
END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[DeleteServiceItem]
(
	@ActorID int,
	@ItemID int
)
AS

-- check rights
DECLARE @PackageID int
SELECT PackageID = @PackageID FROM ServiceItems
WHERE ItemID = @ItemID

IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

BEGIN TRAN

UPDATE Domains
SET ZoneItemID = NULL
WHERE ZoneItemID = @ItemID

UPDATE Domains
SET WebSiteID = NULL
WHERE WebSiteID = @ItemID

UPDATE Domains
SET MailDomainID = NULL
WHERE MailDomainID = @ItemID

-- delete item comments
DELETE FROM Comments
WHERE ItemID = @ItemID AND ItemTypeID = 'SERVICE_ITEM'

-- delete item properties
DELETE FROM ServiceItemProperties
WHERE ItemID = @ItemID

-- delete external IP addresses
EXEC dbo.DeleteItemIPAddresses @ActorID, @ItemID

-- delete item
DELETE FROM ServiceItems
WHERE ItemID = @ItemID

COMMIT TRAN

RETURN 

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GlobalDnsRecords](
	[RecordID] [int] IDENTITY(1,1) NOT NULL,
	[RecordType] [varchar](10) COLLATE Latin1_General_CI_AS NOT NULL,
	[RecordName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[RecordData] [nvarchar](500) COLLATE Latin1_General_CI_AS NOT NULL,
	[MXPriority] [int] NOT NULL,
	[ServiceID] [int] NULL,
	[ServerID] [int] NULL,
	[PackageID] [int] NULL,
	[IPAddressID] [int] NULL,
 CONSTRAINT [PK_GlobalDnsRecords] PRIMARY KEY CLUSTERED 
(
	[RecordID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageServices](
	[PackageID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
 CONSTRAINT [PK_PackageServices] PRIMARY KEY CLUSTERED 
(
	[PackageID] ASC,
	[ServiceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[DeletePackage]
(
	@ActorID int,
	@PackageID int
)
AS
BEGIN
	-- check rights
	IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
	RAISERROR('You are not allowed to access this package', 16, 1)

	BEGIN TRAN

	-- remove package from cache
	DELETE FROM PackagesTreeCache
	WHERE
		ParentPackageID = @PackageID OR
		PackageID = @PackageID

	-- delete package comments
	DELETE FROM Comments
	WHERE ItemID = @PackageID AND ItemTypeID = 'PACKAGE'

	-- delete diskspace
	DELETE FROM PackagesDiskspace
	WHERE PackageID = @PackageID

	-- delete bandwidth
	DELETE FROM PackagesBandwidth
	WHERE PackageID = @PackageID

	-- delete settings
	DELETE FROM PackageSettings
	WHERE PackageID = @PackageID

	-- delete domains
	DELETE FROM Domains
	WHERE PackageID = @PackageID

	-- delete package IP addresses
	DELETE FROM PackageIPAddresses
	WHERE PackageID = @PackageID

	-- delete service items
	DELETE FROM ServiceItems
	WHERE PackageID = @PackageID

	-- delete global DNS records
	DELETE FROM GlobalDnsRecords
	WHERE PackageID = @PackageID

	-- delete package services
	DELETE FROM PackageServices
	WHERE PackageID = @PackageID

	-- delete package quotas
	DELETE FROM PackageQuotas
	WHERE PackageID = @PackageID

	-- delete package resources
	DELETE FROM PackageResources
	WHERE PackageID = @PackageID

	-- delete package
	DELETE FROM Packages
	WHERE PackageID = @PackageID

	COMMIT TRAN
END 

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE VIEW [dbo].[ContractsInvoicesDetailed]
AS
SELECT dbo.ecInvoice.InvoiceID, dbo.ecInvoice.ContractID, dbo.ecContracts.ResellerID, dbo.ecContracts.CustomerID, ISNULL(dbo.Users.Username, 
dbo.ecContracts.AccountName) AS Username, dbo.ecContracts.Status, dbo.ecInvoice.Created, dbo.ecInvoice.DueDate, dbo.ecInvoice.Total, dbo.ecInvoice.SubTotal, 
dbo.ecInvoice.TaxAmount, dbo.ecInvoice.Currency, dbo.ecInvoice.InvoiceNumber, dbo.ecInvoice.TaxationID, dbo.ecCustomersPayments.PaymentID, 
dbo.ecCustomersPayments.StatusID, dbo.ecCustomersPayments.TransactionID, (CASE WHEN dbo.ecCustomersPayments.PaymentID IS NOT NULL AND 
dbo.ecCustomersPayments.StatusID = 1 THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END) AS Paid
FROM dbo.ecInvoice INNER JOIN 
dbo.ecContracts ON dbo.ecInvoice.ContractID = dbo.ecContracts.ContractID LEFT OUTER JOIN 
dbo.ecCustomersPayments ON dbo.ecInvoice.InvoiceID = dbo.ecCustomersPayments.InvoiceID LEFT OUTER JOIN 
dbo.Users ON dbo.ecContracts.CustomerID = dbo.Users.UserID
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



















CREATE PROCEDURE [dbo].[ecGetCustomerTaxation]
	@ContractID nvarchar(50),
	@Country nvarchar(50),
	@State nvarchar(50)
AS
BEGIN
	DECLARE @ResellerID int;
	SELECT
		@ResellerID = [ResellerID] FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--
    SELECT TOP 1
		ROW_NUMBER() OVER (ORDER BY [TypeId] DESC, [State] ASC),
		*
	FROM
		[dbo].[ecTaxations]
	WHERE 
		[ResellerID] = @ResellerID
	AND
		([Country] = @Country OR [Country] = '*')
	AND
		([State] = @State OR [State] = '*')
	AND
		[Active] = 1;

END






















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetCustomerPayment]
	@ActorID int,
	@PaymentID int
AS
BEGIN
	-- read an issuer information
	DECLARE @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecCustomersPayments]
	WHERE
		[PaymentID] = @PaymentID;
	
	SET NOCOUNT ON;

    SELECT
		[P].*, [INV].[InvoiceNumber] FROM [dbo].[ecCustomersPayments] AS [P]
	LEFT OUTER JOIN
		[dbo].[ecInvoice] AS [INV] ON [INV].[InvoiceID] = [P].[InvoiceID]
	WHERE
		[P].[PaymentID] = @PaymentID AND [P].[ContractID] = @ContractID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetCustomerContract]
	@CustomerID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM [dbo].[ecContracts] WHERE [CustomerID] = @CustomerID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetContract]
	@ContractID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM [dbo].[ecContracts] WHERE [ContractID] = @ContractID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetSupportedPluginsByGroup]
	@GroupName nvarchar(50)
AS
BEGIN

	SELECT * FROM [dbo].[ecSupportedPlugins] 
	WHERE [PluginGroup] = @GroupName
	ORDER BY [DisplayName];

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetSupportedPluginByID]
	@PluginID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM [dbo].[ecSupportedPlugins] WHERE [PluginID] = @PluginID;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetSupportedPlugin] 
	@PluginName nvarchar(50),
	@GroupName nvarchar(50)
AS
BEGIN

	SELECT * FROM [dbo].[ecSupportedPlugins]
	WHERE [PluginName] = @PluginName AND [PluginGroup] = @GroupName;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE [dbo].[ecGetStoreSettings]
	@ResellerID int,
	@SettingsName nvarchar(50)
AS
BEGIN

	SET NOCOUNT ON;

    SELECT * FROM [dbo].[ecStoreSettings]
		WHERE [SettingsName] = @SettingsName AND [ResellerID] = @ResellerID;

END



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetStorefrontPath]
	@ResellerID int,
	@CategoryID int
AS
BEGIN

	SET NOCOUNT ON;

	WITH [PathCTE] ([CategoryID]) AS (
		SELECT [ParentID] FROM [dbo].[ecCategory]
		WHERE [CategoryID] = @CategoryID AND [ResellerID] = @ResellerID
		UNION ALL
		SELECT [ParentID] FROM [dbo].[ecCategory] AS [C]
		INNER JOIN [PathCTE] AS [P] ON [C].[CategoryID] = [P].[CategoryID]
		WHERE [ParentID] IS NOT NULL AND [ResellerID] = @ResellerID
	)

	SELECT
		[CategoryID], [CategoryName]
	FROM
		[dbo].[ecCategory]
	WHERE
	(
		[CategoryID] IN (
			SELECT [CategoryID] FROM [PathCTE]
		)
		OR
		[CategoryID] = @CategoryID
	)
	AND
		[ResellerID] = @ResellerID
	ORDER BY
		[Level];
END

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetStorefrontCategory]
	@ResellerID int,
	@CategoryID int
AS
BEGIN

	SET NOCOUNT ON;

    SELECT
		*
	FROM
		[dbo].[ecCategory]
	WHERE
		[ResellerID] = @ResellerID
		AND
		[CategoryID] = @CategoryID;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetStorefrontCategories]
	@ResellerID int,
	@ParentID int
AS
BEGIN

	SET NOCOUNT ON;

	IF @ParentID < 1 OR @ParentID IS NULL
		SET @ParentID = 0;

    SELECT
		*
	FROM
		[dbo].[ecCategory]
	WHERE
		[ResellerID] = @ResellerID
	AND
		ISNULL([ParentID], 0) = @ParentID
	ORDER BY
		[ItemOrder] ASC;

END

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetStoreDefaultSettings]
	@SettingsName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM [dbo].[ecStoreDefaultSettings]
		WHERE [SettingsName] = @SettingsName;
END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE [dbo].[ecGetCategory] 
	@ActorID int,
	@UserID int,
	@CategoryID int
AS
BEGIN

	SET NOCOUNT ON;

    SELECT 
		*
	FROM 
		[dbo].[ecCategory] 
	WHERE 
		[CategoryID] = @CategoryID 
		AND 
		[ResellerID] = @UserID;

END





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE [dbo].[ecGetProductTypeControl]
	@TypeID int,
	@ControlKey nvarchar(50),
	@ControlSrc nvarchar(512) OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

    SELECT
		@ControlSrc = [ControlSrc]
	FROM
		[dbo].[ecProductTypeControls]
	WHERE
		[TypeID] = @TypeID
		AND
		[ControlKey] = @ControlKey;

END

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetProductType]
	@TypeID int
AS
BEGIN

	SELECT * FROM [dbo].[ecProductType]	WHERE [TypeID] = @TypeID;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



























CREATE PROCEDURE [dbo].[ecGetCategoriesCount]
	@ActorID int,
	@UserID int,
	@ParentID int,
	@Count int OUTPUT
AS
	IF @ParentID > 0
		BEGIN
			SELECT
				@Count = COUNT([CategoryID])
			FROM
				[ecCategory]
			WHERE
				[ParentID] = @ParentID
				AND
				[ResellerID] = @UserID;
		END
	ELSE
		BEGIN
			SELECT
				@Count = COUNT([CategoryID])
			FROM
				[ecCategory]
			WHERE
				[ParentID] IS NULL
				AND
				[ResellerID] = @UserID;
		END

RETURN































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE UpdateHostingPlan
(
	@ActorID int,
	@PlanID int,
	@PackageID int,
	@ServerID int,
	@PlanName nvarchar(200),
	@PlanDescription ntext,
	@Available bit,
	@SetupPrice money,
	@RecurringPrice money,
	@RecurrenceLength int,
	@RecurrenceUnit int,
	@QuotasXml ntext
)
AS

-- check rights
DECLARE @UserID int
SELECT @UserID = UserID FROM HostingPlans
WHERE PlanID = @PlanID

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

IF @ServerID = 0
SELECT @ServerID = ServerID FROM Packages
WHERE PackageID = @PackageID

IF @PackageID = 0 SET @PackageID = NULL
IF @ServerID = 0 SET @ServerID = NULL

-- update record
UPDATE HostingPlans SET
	PackageID = @PackageID,
	ServerID = @ServerID,
	PlanName = @PlanName,
	PlanDescription = @PlanDescription,
	Available = @Available,
	SetupPrice = @SetupPrice,
	RecurringPrice = @RecurringPrice,
	RecurrenceLength = @RecurrenceLength,
	RecurrenceUnit = @RecurrenceUnit
WHERE PlanID = @PlanID

BEGIN TRAN

-- update quotas
EXEC UpdateHostingPlanQuotas @ActorID, @PlanID, @QuotasXml

DECLARE @ExceedingQuotas AS TABLE (QuotaID int, QuotaName nvarchar(50), QuotaValue int)
INSERT INTO @ExceedingQuotas
SELECT * FROM dbo.GetPackageExceedingQuotas(@PackageID) WHERE QuotaValue > 0

SELECT * FROM @ExceedingQuotas

IF EXISTS(SELECT * FROM @ExceedingQuotas)
BEGIN
	ROLLBACK TRAN
	RETURN
END

COMMIT TRAN

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetUserAvailableHostingPlans
(
	@ActorID int,
	@UserID int
)
AS

-- user should see the plans only of his reseller
-- also user can create packages based on his own plans (admins and resellers)

DECLARE @Plans TABLE
(
	PlanID int
)

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

DECLARE @OwnerID int
SELECT @OwnerID = OwnerID FROM Users
WHERE UserID = @UserID

SELECT
	HP.PlanID,
	HP.PackageID,
	HP.PlanName,
	HP.PlanDescription,
	HP.Available,
	HP.ServerID,
	HP.SetupPrice,
	HP.RecurringPrice,
	HP.RecurrenceLength,
	HP.RecurrenceUnit,
	HP.IsAddon
FROM
	HostingPlans AS HP
WHERE HP.UserID = @OwnerID
AND HP.IsAddon = 0
ORDER BY PlanName
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetUserAvailableHostingAddons
(
	@ActorID int,
	@UserID int
)
AS

-- user should see the plans only of his reseller
-- also user can create packages based on his own plans (admins and resellers)

DECLARE @Plans TABLE
(
	PlanID int
)

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

DECLARE @OwnerID int
SELECT @OwnerID = OwnerID FROM Users
WHERE UserID = @UserID

SELECT
	HP.PlanID,
	HP.PackageID,
	HP.PlanName,
	HP.PlanDescription,
	HP.Available,
	HP.ServerID,
	HP.SetupPrice,
	HP.RecurringPrice,
	HP.RecurrenceLength,
	HP.RecurrenceUnit,
	HP.IsAddon
FROM
	HostingPlans AS HP
WHERE HP.UserID = @OwnerID
AND HP.IsAddon = 1
ORDER BY PlanName
RETURN

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servers](
	[ServerID] [int] IDENTITY(1,1) NOT NULL,
	[ServerName] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[ServerUrl] [nvarchar](100) COLLATE Latin1_General_CI_AS NULL,
	[Password] [nvarchar](100) COLLATE Latin1_General_CI_AS NULL,
	[Comments] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[VirtualServer] [bit] NOT NULL,
	[InstantDomainAlias] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[PrimaryGroupID] [int] NULL,
	[ADRootDomain] [nvarchar](200) COLLATE Latin1_General_CI_AS NULL,
	[ADUsername] [nvarchar](100) COLLATE Latin1_General_CI_AS NULL,
	[ADPassword] [nvarchar](100) COLLATE Latin1_General_CI_AS NULL,
	[ADAuthenticationType] [varchar](50) COLLATE Latin1_General_CI_AS NULL,
	[ADEnabled] [bit] NULL,
 CONSTRAINT [PK_Servers] PRIMARY KEY CLUSTERED 
(
	[ServerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetHostingPlans]
(
	@ActorID int,
	@UserID int
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

SELECT
	HP.PlanID,
	HP.UserID,
	HP.PackageID,
	HP.PlanName,
	HP.PlanDescription,
	HP.Available,
	HP.SetupPrice,
	HP.RecurringPrice,
	HP.RecurrenceLength,
	HP.RecurrenceUnit,
	HP.IsAddon,
	
	(SELECT COUNT(P.PackageID) FROM Packages AS P WHERE P.PlanID = HP.PlanID) AS PackagesNumber,
	
	-- server
	ISNULL(HP.ServerID, 0) AS ServerID,
	ISNULL(S.ServerName, 'None') AS ServerName,
	ISNULL(S.Comments, '') AS ServerComments,
	ISNULL(S.VirtualServer, 1) AS VirtualServer,
	
	-- package
	ISNULL(HP.PackageID, 0) AS PackageID,
	ISNULL(P.PackageName, 'None') AS PackageName
	
FROM HostingPlans AS HP
LEFT OUTER JOIN Servers AS S ON HP.ServerID = S.ServerID
LEFT OUTER JOIN Packages AS P ON HP.PackageID = P.PackageID
WHERE
	HP.UserID = @UserID
	AND HP.IsAddon = 0
ORDER BY HP.PlanName
RETURN








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE [dbo].[GetHostingAddons]
(
	@ActorID int,
	@UserID int
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

SELECT
	PlanID,
	UserID,
	PackageID,
	PlanName,
	PlanDescription,
	Available,
	SetupPrice,
	RecurringPrice,
	RecurrenceLength,
	RecurrenceUnit,
	IsAddon,
	(SELECT COUNT(P.PackageID) FROM PackageAddons AS P WHERE P.PlanID = HP.PlanID) AS PackagesNumber
FROM
	HostingPlans AS HP
WHERE
	UserID = @UserID
	AND IsAddon = 1
ORDER BY PlanName
RETURN







































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetComments
(
	@ActorID int,
	@UserID int,
	@ItemTypeID varchar(50),
	@ItemID int
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

SELECT
	C.CommentID,
	C.ItemTypeID,
	C.ItemID,
	C.UserID,
	C.CreatedDate,
	C.CommentText,
	C.SeverityID,
	
	-- user
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM Comments AS C
INNER JOIN UsersDetailed AS U ON C.UserID = U.UserID
WHERE
	ItemTypeID = @ItemTypeID
	AND ItemID = @ItemID
	AND dbo.CheckUserParent(@UserID, C.UserID) = 1
ORDER BY C.CreatedDate ASC
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[AddOCSUser]	
	@AccountID int,
	@InstanceID nvarchar(50)
AS
BEGIN	
	SET NOCOUNT ON;

INSERT INTO
	dbo.OCSUsers
	(	 
	 
	 AccountID,
     InstanceID,
	 CreatedDate,
	 ModifiedDate)
VALUES
(		
	@AccountID,
	@InstanceID,
	getdate(),
	getdate()
)		
END














GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[AddAuditLogRecord]
(
	@RecordID varchar(32),
	@SeverityID int,
	@UserID int,
	@PackageID int,
	@Username nvarchar(50),
	@ItemID int,
	@StartDate datetime,
	@FinishDate datetime,
	@SourceName varchar(50),
	@TaskName varchar(50),
	@ItemName nvarchar(50),
	@ExecutionLog ntext
)
AS

IF @ItemID = 0 SET @ItemID = NULL
IF @UserID = 0 OR @UserID = -1 SET @UserID = NULL


INSERT INTO AuditLog
(
	RecordID,
	SeverityID,
	UserID,
	PackageID,
	Username,
	ItemID,
	SourceName,
	StartDate,
	FinishDate,
	TaskName,
	ItemName,
	ExecutionLog
)
VALUES
(
	@RecordID,
	@SeverityID,
	@UserID,
	@PackageID,
	@Username,
	@ItemID,
	@SourceName,
	@StartDate,
	@FinishDate,
	@TaskName,
	@ItemName,
	@ExecutionLog
)
RETURN


















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[DeleteOCSUser]
(	
	@InstanceId nvarchar(50)
)
AS

DELETE FROM 
	OCSUsers
WHERE 
	InstanceId = @InstanceId

RETURN 





GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecDeleteContract]
	@ContractID nvarchar(50),
	@Result int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS (SELECT * FROM [dbo].[ecContracts] WHERE [ContractID] = @ContractID)
	BEGIN
		SET @Result = -1;
		RETURN;
	END
	
	SET @Result = 0;
	DELETE FROM [dbo].[ecContracts]  WHERE [ContractID] = @ContractID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE AddCluster
(
	@ClusterID int OUTPUT,
	@ClusterName nvarchar(100)
)
AS
INSERT INTO Clusters
(
	ClusterName
)
VALUES
(
	@ClusterName
)

SET @ClusterID = SCOPE_IDENTITY()
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecAddContract]
	@ContractID nvarchar(50) OUTPUT,
	@CustomerID int,
	@ResellerID int,
	@AccountName nvarchar(50),
	@Status int,
	@Balance money,
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(255),
	@CompanyName nvarchar(50),
	@PropertyNames ntext,
	@PropertyValues ntext
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @CustomerID < 1
		SET @CustomerID = NULL;

	SET @ContractID = CAST(NEWID() as nvarchar(50));

	INSERT INTO [ecContracts]
		([ContractID],[CustomerID],[ResellerID],[AccountName],[Status],[Balance],
		[FirstName],[LastName],[Email],[CompanyName],[PropertyNames],[PropertyValues])
	VALUES
		(@ContractID, @CustomerID, @ResellerID, @AccountName, @Status, @Balance, @FirstName,
		@LastName, @Email, @CompanyName, @PropertyNames, @PropertyValues);

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetResourceGroups
AS
SELECT
	GroupID,
	GroupName,
	GroupController
FROM ResourceGroups
ORDER BY GroupOrder
RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetResourceGroup
(
	@GroupID int
)
AS
SELECT
	RG.GroupID,
	RG.GroupOrder,
	RG.GroupName,
	RG.GroupController
FROM ResourceGroups AS RG
WHERE RG.GroupID = @GroupID

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetSystemTrigger]
	@ActorID int,
	@ReferenceID nvarchar(50),
	@Namespace nvarchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		* FROM [dbo].[ecSystemTriggers]
	WHERE
		[OwnerID] = @ActorID AND [ReferenceID] = @ReferenceID AND [Namespace] = @Namespace;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetAuditLogTasks
(
	@SourceName varchar(100)
)
AS

IF @SourceName = '' SET @SourceName = NULL

SELECT SourceName, TaskName FROM AuditLogTasks
WHERE (@SourceName = NULL OR @SourceName IS NOT NULL AND SourceName = @SourceName)

RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetAuditLogSources
AS

SELECT SourceName FROM AuditLogSources

RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetScheduleTasks 
(
	@ActorID int
)
AS

-- get user role
DECLARE @RoleID int
SELECT @RoleID = RoleID FROM Users
WHERE UserID = @ActorID

SELECT
	TaskID,
	TaskType,
	RoleID
FROM ScheduleTasks
WHERE @RoleID <= RoleID
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetScheduleTask
(
	@ActorID int,
	@TaskID nvarchar(100)
)
AS

-- get user role
DECLARE @RoleID int
SELECT @RoleID = RoleID FROM Users
WHERE UserID = @ActorID

SELECT
	TaskID,
	TaskType,
	RoleID
FROM ScheduleTasks
WHERE
	TaskID = @TaskID
	AND @RoleID >= RoleID
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE GetSystemSettings
	@SettingsName nvarchar(50)
AS
BEGIN

	SET NOCOUNT ON;

    SELECT
		[PropertyName],
		[PropertyValue]
	FROM
		[dbo].[SystemSettings]
	WHERE
		[SettingsName] = @SettingsName;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE [dbo].[GetUserByIdInternally]
(
	@UserID int
)
AS
SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	U.Comments,
	U.IsPeer,
	U.Username,
	U.Password,
	U.FirstName,
	U.LastName,
	U.Email,
	U.SecondaryEmail,
	U.Address,
	U.City,
	U.State,
	U.Country,
	U.Zip,
	U.PrimaryPhone,
	U.SecondaryPhone,
	U.Fax,
	U.InstantMessenger,
	U.HtmlMail,
	U.CompanyName,
	U.EcommerceEnabled
FROM Users AS U
WHERE U.UserID = @UserID

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE [dbo].[GetUserByUsernameInternally]
(
	@Username nvarchar(50)
)
AS
SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	U.Comments,
	U.IsPeer,
	U.Username,
	U.Password,
	U.FirstName,
	U.LastName,
	U.Email,
	U.SecondaryEmail,
	U.Address,
	U.City,
	U.State,
	U.Country,
	U.Zip,
	U.PrimaryPhone,
	U.SecondaryPhone,
	U.Fax,
	U.InstantMessenger,
	U.HtmlMail,
	U.CompanyName,
	U.EcommerceEnabled
FROM Users AS U
WHERE U.Username = @Username

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Providers](
	[ProviderID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[ProviderName] [nvarchar](100) COLLATE Latin1_General_CI_AS NULL,
	[DisplayName] [nvarchar](200) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProviderType] [nvarchar](400) COLLATE Latin1_General_CI_AS NULL,
	[EditorControl] [nvarchar](100) COLLATE Latin1_General_CI_AS NULL,
	[DisableAutoDiscovery] [bit] NULL,
 CONSTRAINT [PK_ServiceTypes] PRIMARY KEY CLUSTERED 
(
	[ProviderID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (1, 1, N'Windows2003', N'Windows Server 2003', N'WebsitePanel.Providers.OS.Windows2003, WebsitePanel.Providers.OS.Windows2003', N'Windows2003', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (2, 2, N'IIS60', N'Internet Information Services 6.0', N'WebsitePanel.Providers.Web.IIs60, WebsitePanel.Providers.Web.IIs60', N'IIS60', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (3, 3, N'MSFTP60', N'Microsoft FTP Server 6.0', N'WebsitePanel.Providers.FTP.MsFTP, WebsitePanel.Providers.FTP.IIs60', N'MSFTP60', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (4, 4, N'MailEnable', N'MailEnable Server 1.x - 4.x', N'WebsitePanel.Providers.Mail.MailEnable, WebsitePanel.Providers.Mail.MailEnable', N'MailEnable', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (5, 5, N'MSSQL', N'Microsoft SQL Server 2000', N'WebsitePanel.Providers.Database.MsSqlServer, WebsitePanel.Providers.Database.SqlServer', N'MSSQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (6, 6, N'MySQL', N'MySQL Server 4.x', N'WebsitePanel.Providers.Database.MySqlServer, WebsitePanel.Providers.Database.MySQL', N'MySQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (7, 7, N'MSDNS', N'Microsoft DNS Server', N'WebsitePanel.Providers.DNS.MsDNS, WebsitePanel.Providers.DNS.MsDNS', N'MSDNS', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (8, 8, N'AWStats', N'AWStats Statistics Service', N'WebsitePanel.Providers.Statistics.AWStats, WebsitePanel.Providers.Statistics.AWStats', N'AWStats', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (9, 7, N'SimpleDNS', N'SimpleDNS Plus 4.x', N'WebsitePanel.Providers.DNS.SimpleDNS, WebsitePanel.Providers.DNS.SimpleDNS', N'SimpleDNS', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (10, 8, N'SmarterStats', N'SmarterStats 3.x', N'WebsitePanel.Providers.Statistics.SmarterStats, WebsitePanel.Providers.Statistics.SmarterStats', N'SmarterStats', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (11, 4, N'SmarterMail', N'SmarterMail 2.x', N'WebsitePanel.Providers.Mail.SmarterMail2, WebsitePanel.Providers.Mail.SmarterMail2', N'SmarterMail', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (12, 3, N'Gene6FTP', N'Gene6 FTP Server 3.x', N'WebsitePanel.Providers.FTP.Gene6, WebsitePanel.Providers.FTP.Gene6', N'Gene6FTP', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (13, 4, N'Merak', N'Merak Mail Server 8.0.3 - 9.2.x', N'WebsitePanel.Providers.Mail.Merak, WebsitePanel.Providers.Mail.Merak', N'Merak', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (14, 4, N'SmarterMail', N'SmarterMail 3.x - 4.x', N'WebsitePanel.Providers.Mail.SmarterMail3, WebsitePanel.Providers.Mail.SmarterMail3', N'SmarterMail', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (15, 9, N'Sps20', N'Windows SharePoint Services 2.0', N'WebsitePanel.Providers.SharePoint.Sps20, WebsitePanel.Providers.SharePoint.Sps20', N'Sps20', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (16, 10, N'MSSQL', N'Microsoft SQL Server 2005', N'WebsitePanel.Providers.Database.MsSqlServer2005, WebsitePanel.Providers.Database.SqlServer', N'MSSQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (17, 11, N'MySQL', N'MySQL Server 5.0', N'WebsitePanel.Providers.Database.MySqlServer50, WebsitePanel.Providers.Database.MySQL', N'MySQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (18, 4, N'MDaemon', N'MDaemon 9.x - 11.x', N'WebsitePanel.Providers.Mail.MDaemon, WebsitePanel.Providers.Mail.MDaemon', N'MDaemon', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (19, 4, N'ArgoMail', N'ArGoSoft Mail Server 1.x', N'WebsitePanel.Providers.Mail.ArgoMail, WebsitePanel.Providers.Mail.ArgoMail', N'ArgoMail', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (20, 4, N'hMailServer', N'hMailServer 4.2', N'WebsitePanel.Providers.Mail.hMailServer, WebsitePanel.Providers.Mail.hMailServer', N'hMailServer', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (21, 4, N'AbilityMailServer', N'Ability Mail Server 2.x', N'WebsitePanel.Providers.Mail.AbilityMailServer, WebsitePanel.Providers.Mail.AbilityMailServer', N'AbilityMailServer', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (22, 4, N'hMailServer43', N'hMailServer 4.3', N'WebsitePanel.Providers.Mail.hMailServer43, WebsitePanel.Providers.Mail.hMailServer43', N'hMailServer43', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (23, 9, N'Sps20', N'Windows SharePoint Services 3.0', N'WebsitePanel.Providers.SharePoint.Sps30, WebsitePanel.Providers.SharePoint.Sps30', N'Sps20', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (24, 7, N'Bind', N'ISC BIND 8.x - 9.x', N'WebsitePanel.Providers.DNS.IscBind, WebsitePanel.Providers.DNS.Bind', N'Bind', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (25, 3, N'ServU', N'Serv-U FTP 6.x', N'WebsitePanel.Providers.FTP.ServU, WebsitePanel.Providers.FTP.ServU', N'ServU', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (26, 3, N'FileZilla', N'FileZilla FTP Server 0.9', N'WebsitePanel.Providers.FTP.FileZilla, WebsitePanel.Providers.FTP.FileZilla', N'FileZilla', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (27, 12, N'Exchange2007', N'Hosted Microsoft Exchange Server 2007', N'WebsitePanel.Providers.HostedSolution.Exchange2007, WebsitePanel.Providers.HostedSolution', N'Exchange', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (28, 7, N'SimpleDNS', N'SimpleDNS Plus 5.x', N'WebsitePanel.Providers.DNS.SimpleDNS5, WebsitePanel.Providers.DNS.SimpleDNS50', N'SimpleDNS', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (29, 4, N'SmarterMail', N'SmarterMail 5.x', N'WebsitePanel.Providers.Mail.SmarterMail5, WebsitePanel.Providers.Mail.SmarterMail5', N'SmarterMail50', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (30, 11, N'MySQL', N'MySQL Server 5.1', N'WebsitePanel.Providers.Database.MySqlServer51, WebsitePanel.Providers.Database.MySQL', N'MySQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (31, 8, N'SmarterStats', N'SmarterStats 4.x', N'WebsitePanel.Providers.Statistics.SmarterStats4, WebsitePanel.Providers.Statistics.SmarterStats', N'SmarterStats', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (32, 12, N'Exchange2010', N'Hosted Microsoft Exchange Server 2010', N'WebsitePanel.Providers.HostedSolution.Exchange2010, WebsitePanel.Providers.HostedSolution', N'Exchange', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (55, 7, N'NetticaDNS', N'Nettica DNS', N'WebsitePanel.Providers.DNS.Nettica, WebsitePanel.Providers.DNS.Nettica', N'NetticaDNS', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (56, 7, N'PowerDNS', N'PowerDNS', N'WebsitePanel.Providers.DNS.PowerDNS, WebsitePanel.Providers.DNS.PowerDNS', N'PowerDNS', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (60, 4, N'SmarterMail', N'SmarterMail 6.x', N'WebsitePanel.Providers.Mail.SmarterMail6, WebsitePanel.Providers.Mail.SmarterMail6', N'SmarterMail60', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (61, 4, N'Merak', N'Merak Mail Server 10.x', N'WebsitePanel.Providers.Mail.Merak10, WebsitePanel.Providers.Mail.Merak10', N'Merak', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (62, 8, N'SmarterStats', N'SmarterStats 5.x-6.x', N'WebsitePanel.Providers.Statistics.SmarterStats5, WebsitePanel.Providers.Statistics.SmarterStats', N'SmarterStats', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (63, 4, N'hMailServer5', N'hMailServer 5.x', N'WebsitePanel.Providers.Mail.hMailServer5, WebsitePanel.Providers.Mail.hMailServer5', N'hMailServer43', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (64, 4, N'SmarterMail', N'SmarterMail 7.x', N'WebsitePanel.Providers.Mail.SmarterMail7, WebsitePanel.Providers.Mail.SmarterMail7', N'SmarterMail60', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (100, 1, N'Windows2008', N'Windows Server 2008', N'WebsitePanel.Providers.OS.Windows2008, WebsitePanel.Providers.OS.Windows2008', N'Windows2008', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (101, 2, N'IIS70', N'Internet Information Services 7.0', N'WebsitePanel.Providers.Web.IIs70, WebsitePanel.Providers.Web.IIs70', N'IIS70', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (102, 3, N'MSFTP70', N'Microsoft FTP Server 7.0', N'WebsitePanel.Providers.FTP.MsFTP, WebsitePanel.Providers.FTP.IIs70', N'MSFTP70', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (103, 13, N'Organizations', N'Hosted Organizations', N'WebsitePanel.Providers.HostedSolution.OrganizationProvider, WebsitePanel.Providers.HostedSolution', N'Organizations', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (200, 20, N'HostedSharePoint30', N'Hosted Windows SharePoint Services 3.0', N'WebsitePanel.Providers.HostedSolution.HostedSharePointServer, WebsitePanel.Providers.HostedSolution', N'HostedSharePoint30', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (201, 21, N'CRM', N'Hosted MS CRM 4.0', N'WebsitePanel.Providers.HostedSolution.CRMProvider, WebsitePanel.Providers.HostedSolution', N'CRM', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (202, 22, N'MsSQL', N'Microsoft SQL Server 2008', N'WebsitePanel.Providers.Database.MsSqlServer2008, WebsitePanel.Providers.Database.SqlServer', N'MSSQL', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (203, 31, N'BlackBerry 4.1', N'BlackBerry 4.1', N'WebsitePanel.Providers.HostedSolution.BlackBerryProvider, WebsitePanel.Providers.HostedSolution', N'BlackBerry', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (204, 31, N'BlackBerry 5.0', N'BlackBerry 5.0', N'WebsitePanel.Providers.HostedSolution.BlackBerry5Provider, WebsitePanel.Providers.HostedSolution', N'BlackBerry5', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (205, 32, N'OCS', N'Office Communications Server 2007 R2', N'WebsitePanel.Providers.HostedSolution.OCS2007R2, WebsitePanel.Providers.HostedSolution', N'OCS', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (206, 32, N'OCSEdge', N'OCS Edge server', N'WebsitePanel.Providers.HostedSolution.OCSEdge2007R2, WebsitePanel.Providers.HostedSolution', N'OCS_Edge', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (207, 33, N'Exchange2010SP1', N'Exchange Server 2010 SP1 Hosting Mode', N'WebsitePanel.Providers.ExchangeHostedEdition.Exchange2010SP1, WebsitePanel.Providers.ExchangeHostedEdition', N'Exchange2010SP1', 1)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (208, 20, N'HostedSharePoint2010', N'Hosted SharePoint Foundation 2010', N'WebsitePanel.Providers.HostedSolution.HostedSharePointServer2010, WebsitePanel.Providers.HostedSolution', N'HostedSharePoint30', NULL)
GO
INSERT [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (300, 30, N'HyperV', N'Microsoft Hyper-V', N'WebsitePanel.Providers.Virtualization.HyperV, WebsitePanel.Providers.Virtualization.HyperV', N'HyperV', 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE FUNCTION [dbo].[CanGetUserPassword]
(
	@ActorID int,
	@UserID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1
RETURN 1 -- unauthenticated mode

-- check if the user requests himself
IF @ActorID = @UserID
BEGIN
	RETURN 1
END

DECLARE @IsPeer bit
DECLARE @OwnerID int

SELECT @IsPeer = IsPeer, @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @IsPeer = 1
BEGIN
	-- peer can't get the password of his peers
	-- and his owner
	IF @UserID = @OwnerID
	RETURN 0
	
	IF EXISTS (
		SELECT UserID FROM Users
		WHERE IsPeer = 1 AND OwnerID = @OwnerID AND UserID = @UserID
	) RETURN 0
	
	-- set actor to his owner
	SET @ActorID = @OwnerID
END

-- get user's owner
SELECT @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @UserID = @OwnerID
RETURN 0 -- user can't get the password of his owner

DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
		BREAK
	
	IF @ParentUserID = @ActorID
	RETURN 1
	
	SET @TmpUserID = @ParentUserID
END

RETURN 0
END


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE FUNCTION [dbo].[CanGetUserDetails]
(
	@ActorID int,
	@UserID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1
RETURN 1

-- check if the user requests himself
IF @ActorID = @UserID
BEGIN
	RETURN 1
END

DECLARE @IsPeer bit
DECLARE @OwnerID int

SELECT @IsPeer = IsPeer, @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @IsPeer = 1
SET @ActorID = @OwnerID

-- get user's owner
SELECT @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @UserID = @OwnerID
RETURN 1 -- user can get the details of his owner

-- check if the user requests himself
IF @ActorID = @UserID
BEGIN
	RETURN 1
END

DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
		BREAK
	
	IF @ParentUserID = @ActorID
	RETURN 1
	
	SET @TmpUserID = @ParentUserID
END

RETURN 0
END


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[GetUsers]
(
	@ActorID int,
	@OwnerID int,
	@Recursive bit = 0
)
AS

DECLARE @CanGetDetails bit
SET @CanGetDetails = dbo.CanGetUserDetails(@ActorID, @OwnerID)

SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	U.Comments,
	U.IsPeer,
	U.Username,
	U.FirstName,
	U.LastName,
	U.Email,
	U.FullName,
	U.OwnerUsername,
	U.OwnerFirstName,
	U.OwnerLastName,
	U.OwnerRoleID,
	U.OwnerFullName,
	U.PackagesNumber,
	U.CompanyName,
	U.EcommerceEnabled
FROM UsersDetailed AS U
WHERE U.UserID <> @OwnerID AND
((@Recursive = 1 AND dbo.CheckUserParent(@OwnerID, U.UserID) = 1) OR 
(@Recursive = 0 AND U.OwnerID = @OwnerID))
AND U.IsPeer = 0
AND @CanGetDetails = 1 -- actor user rights

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE [dbo].[GetUserPeers]
(
	@ActorID int,
	@UserID int
)
AS

DECLARE @CanGetDetails bit
SET @CanGetDetails = dbo.CanGetUserDetails(@ActorID, @UserID)

SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	U.Comments,
	U.IsPeer,
	U.Username,
	U.FirstName,
	U.LastName,
	U.Email,
	U.FullName,
	(U.FirstName + ' ' + U.LastName) AS FullName,
	U.CompanyName,
	U.EcommerceEnabled
FROM UsersDetailed AS U
WHERE U.OwnerID = @UserID AND IsPeer = 1
AND @CanGetDetails = 1 -- actor rights

RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE FUNCTION [dbo].[CheckActorParentPackageRights]
(
	@ActorID int,
	@PackageID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1 OR @PackageID IS NULL
RETURN 1

-- get package owner
DECLARE @UserID int
SELECT @UserID = UserID FROM Packages
WHERE PackageID = @PackageID

IF @UserID IS NULL
RETURN 1 -- unexisting package

-- check user
RETURN dbo.CanGetUserDetails(@ActorID, @UserID)

RETURN 0
END


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE FUNCTION [dbo].[CanUpdateUserDetails]
(
	@ActorID int,
	@UserID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1
RETURN 1

-- check if the user requests himself
IF @ActorID = @UserID
BEGIN
	RETURN 1
END

DECLARE @IsPeer bit
DECLARE @OwnerID int

SELECT @IsPeer = IsPeer, @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @IsPeer = 1
BEGIN
	-- check if the peer is trying to update his owner
	IF @UserID = @OwnerID
	RETURN 0
	
	-- check if the peer is trying to update his peers
	IF EXISTS (SELECT UserID FROM Users
	WHERE IsPeer = 1 AND OwnerID = @OwnerID AND UserID = @UserID)
	RETURN 0
	
	SET @ActorID = @OwnerID
END

DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
		BREAK
	
	IF @ParentUserID = @ActorID
	RETURN 1
	
	SET @TmpUserID = @ParentUserID
END

RETURN 0
END


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[DeleteUser]
(
	@ActorID int,
	@UserID int
)
AS

-- check actor rights
IF dbo.CanUpdateUserDetails(@ActorID, @UserID) = 0
RETURN

BEGIN TRAN
-- delete user comments
DELETE FROM Comments
WHERE ItemID = @UserID AND ItemTypeID = 'USER'

IF (@@ERROR <> 0 )
      BEGIN
            ROLLBACK TRANSACTION
            RETURN -1
      END

--delete reseller addon  
DELETE FROM HostingPlans WHERE UserID = @UserID AND IsAddon = 'True'

IF (@@ERROR <> 0 )
      BEGIN
            ROLLBACK TRANSACTION
            RETURN -1
      END

-- delete user peers
DELETE FROM Users
WHERE IsPeer = 1 AND OwnerID = @UserID

IF (@@ERROR <> 0 )
      BEGIN
            ROLLBACK TRANSACTION
            RETURN -1
      END

-- delete user
DELETE FROM Users
WHERE UserID = @UserID

IF (@@ERROR <> 0 )
      BEGIN
            ROLLBACK TRANSACTION
            RETURN -1
      END

COMMIT TRAN

RETURN

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecWriteSupportedPluginLog]
	@ContractID nvarchar(50),
	@PluginID int,
	@RecordType int,
	@RawData ntext,
	@Result int OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

    INSERT INTO [dbo].[ecSupportedPluginLog]
	(
		[PluginID],
		[ContractID],
		[RecordType],
		[RawData]
	)
	VALUES
	(
		@PluginID,
		@ContractID,
		@RecordType,
		@RawData
	);

	SET @Result = SCOPE_IDENTITY();
END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE [dbo].[SetSystemSettings]
	@SettingsName nvarchar(50),
	@Xml ntext
AS
BEGIN
/*
XML Format:
<properties>
	<property name="" value=""/>
</properties>
*/
	SET NOCOUNT ON;

	BEGIN TRAN
		DECLARE @idoc int;
		--Create an internal representation of the XML document.
		EXEC sp_xml_preparedocument @idoc OUTPUT, @xml;
	
		DELETE FROM [dbo].[SystemSettings] WHERE [SettingsName] = @SettingsName;

		INSERT INTO [dbo].[SystemSettings]
		(
			[SettingsName],
			[PropertyName],
			[PropertyValue]
		)
		SELECT
			@SettingsName,
			[XML].[PropertyName],
			[XML].[PropertyValue]
		FROM OPENXML(@idoc, '/properties/property',1) WITH 
		(
			[PropertyName] nvarchar(50) '@name',
			[PropertyValue] ntext '@value'
		) AS XML;

		-- remove document
		EXEC sp_xml_removedocument @idoc;

	COMMIT TRAN;

END

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE FUNCTION [dbo].[CanCreateUser]
(
	@ActorID int,
	@UserID int
)
RETURNS bit
AS
BEGIN

IF @ActorID = -1
RETURN 1

-- check if the user requests himself
IF @ActorID = @UserID
RETURN 1

DECLARE @IsPeer bit
DECLARE @OwnerID int

SELECT @IsPeer = IsPeer, @OwnerID = OwnerID FROM Users
WHERE UserID = @ActorID

IF @IsPeer = 1
BEGIN
	SET @ActorID = @OwnerID
END

IF @ActorID = @UserID
RETURN 1

DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	SET @ParentUserID = NULL --reset var

	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID

	IF @ParentUserID IS NULL -- the last parent
		BREAK
	
	IF @ParentUserID = @ActorID
	RETURN 1
	
	SET @TmpUserID = @ParentUserID
END

RETURN 0
END


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE FUNCTION dbo.UserParents
(
	@ActorID int,
	@UserID int
)
RETURNS @T TABLE (UserOrder int IDENTITY(1,1), UserID int)
AS
BEGIN
	-- insert current user
	INSERT @T VALUES (@UserID)
	
	DECLARE @TopUserID int
	IF @ActorID = -1
	BEGIN
		SELECT @TopUserID = UserID FROM Users WHERE OwnerID IS NULL
	END
	ELSE
	BEGIN
		SET @TopUserID = @ActorID
		
		IF EXISTS (SELECT UserID FROM Users WHERE UserID = @ActorID AND IsPeer = 1)
		SELECT @TopUserID = OwnerID FROM Users WHERE UserID = @ActorID AND IsPeer = 1
	END

	-- owner
	DECLARE @OwnerID int, @TmpUserID int
	
	SET @TmpUserID = @UserID

	WHILE (@TmpUserID <> @TopUserID)
	BEGIN

		SET @OwnerID = NULL
		SELECT @OwnerID = OwnerID FROM Users WHERE UserID = @TmpUserID
		
		IF @OwnerID IS NOT NULL
		BEGIN
			INSERT @T VALUES (@OwnerID)
			SET @TmpUserID = @OwnerID
		END
	END

RETURN
END




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecUpdateServiceHandlersResponses] 
	@ResellerID int,
	@XmlData xml
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

/*
<Succeed>
	<Response ID="" />
</Succeed>
<Failed>
	<Response ID="" Error="" />
</Failed>
*/
	DELETE
		FROM [ecServiceHandlersResponses]
	WHERE
		[ResponseID] IN (SELECT [SXML].[Data].value('@ID','int') FROM @XmlData.nodes('/Succeed/Response') [SXML]([Data]))

	UPDATE 
		[ecServiceHandlersResponses]
	SET
		[ErrorMessage] = [SXML].[Data].value('@Error','nvarchar(255)')
	FROM @XmlData.nodes('/Failed/Response') [SXML]([Data])
	WHERE
		[ResponseID] = [SXML].[Data].value('@ID', 'int')
END























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteAuditLogRecordsComplete
AS

TRUNCATE TABLE AuditLog

RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecAddServiceHandlerTextResponse]
	@ServiceID nvarchar(50),
	@ContractID nvarchar(50),
	@InvoiceID int,
	@DataReceived ntext
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @InvoiceID = 0
		SET @InvoiceID = NULL;

    INSERT INTO [ecServiceHandlersResponses] ([ServiceID],[ContractID],[InvoiceID],[TextResponse])
	VALUES (@ServiceID,@ContractID,@InvoiceID,@DataReceived);

END























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecProduct](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProductSKU] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[TypeID] [int] NULL,
	[Description] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[Created] [datetime] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[ResellerID] [int] NOT NULL,
	[TaxInclusive] [bit] NULL,
 CONSTRAINT [PK_EC_Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE GetPackages
(
	@ActorID int,
	@UserID int
)
AS

IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

SELECT
	P.PackageID,
	P.ParentPackageID,
	P.PackageName,
	P.StatusID,
	P.PurchaseDate,
	
	-- server
	ISNULL(P.ServerID, 0) AS ServerID,
	ISNULL(S.ServerName, 'None') AS ServerName,
	ISNULL(S.Comments, '') AS ServerComments,
	ISNULL(S.VirtualServer, 1) AS VirtualServer,
	
	-- hosting plan
	P.PlanID,
	HP.PlanName,
	
	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.RoleID,
	U.Email
FROM Packages AS P
INNER JOIN Users AS U ON P.UserID = U.UserID
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE
	P.UserID <> @UserID
	AND dbo.CheckUserParent(@UserID, P.UserID) = 1
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE GetPackagePackages
(
	@ActorID int,
	@PackageID int,
	@Recursive bit
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	P.PackageID,
	P.ParentPackageID,
	P.PackageName,
	P.StatusID,
	P.PurchaseDate,
	
	-- server
	P.ServerID,
	ISNULL(S.ServerName, 'None') AS ServerName,
	ISNULL(S.Comments, '') AS ServerComments,
	ISNULL(S.VirtualServer, 1) AS VirtualServer,
	
	-- hosting plan
	P.PlanID,
	HP.PlanName,
	
	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.RoleID,
	U.Email
FROM Packages AS P
INNER JOIN Users AS U ON P.UserID = U.UserID
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
INNER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE
	((@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, P.PackageID) = 1)
		OR (@Recursive = 0 AND P.ParentPackageID = @PackageID))
	AND P.PackageID <> @PackageID
RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE GetMyPackages
(
	@ActorID int,
	@UserID int
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

SELECT
	P.PackageID,
	P.ParentPackageID,
	P.PackageName,
	P.StatusID,
	P.PlanID,
	P.PurchaseDate,
	
	dbo.GetItemComments(P.PackageID, 'PACKAGE', @ActorID) AS Comments,
	
	-- server
	ISNULL(P.ServerID, 0) AS ServerID,
	ISNULL(S.ServerName, 'None') AS ServerName,
	ISNULL(S.Comments, '') AS ServerComments,
	ISNULL(S.VirtualServer, 1) AS VirtualServer,
	
	-- hosting plan
	HP.PlanName,
	
	-- user
	P.UserID,
	U.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	U.RoleID,
	U.Email
FROM Packages AS P
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
LEFT OUTER JOIN Servers AS S ON P.ServerID = S.ServerID
LEFT OUTER JOIN HostingPlans AS HP ON P.PlanID = HP.PlanID
WHERE P.UserID = @UserID
RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[GetInstanceID]
	 @AccountID int
AS
BEGIN	
	SET NOCOUNT ON;
    
	SELECT InstanceID FROM OCSUsers WHERE AccountID = @AccountID
END











GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE FUNCTION dbo.UsersTree
(
	@OwnerID int,
	@Recursive bit = 0
)
RETURNS @T TABLE (UserID int)
AS
BEGIN

	IF @Recursive = 1
	BEGIN
		-- insert "root" user
		INSERT @T VALUES(@OwnerID)
	
		-- get all children recursively
		WHILE @@ROWCOUNT > 0
		BEGIN
			INSERT @T SELECT UserID
			FROM Users
			WHERE OwnerID IN(SELECT UserID from @T) AND UserID NOT IN(SELECT UserID FROM @T)
		END
	END
	ELSE
	BEGIN
		INSERT @T VALUES(@OwnerID)
	END
	
RETURN
END



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSettings](
	[UserID] [int] NOT NULL,
	[SettingsName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyValue] [ntext] COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_UserSettings] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[SettingsName] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'CC', N'support@HostingCompany.com')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'EnableLetter', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'From', N'support@HostingCompany.com')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'HtmlBody', N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Account Summary Information</title>
    <style type="text/css">
		.Summary { background-color: ##ffffff; padding: 5px; }
		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }
        .Summary A { color: ##0153A4; }
        .Summary { font-family: Tahoma; font-size: 9pt; }
        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }
        .Summary H2 { font-size: 1.3em; color: ##1F4978; } 
        .Summary TABLE { border: solid 1px ##e5e5e5; }
        .Summary TH,
        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }
        .Summary TD { padding: 8px; font-size: 9pt; }
        .Summary UL LI { font-size: 1.1em; font-weight: bold; }
        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }
    </style>
</head>
<body>
<div class="Summary">

<a name="top"></a>
<div class="Header">
	Hosting Account Information
</div>

<ad:if test="#Signup#">
<p>
Hello #user.FirstName#,
</p>

<p>
New user account has been created and below you can find its summary information.
</p>

<h1>Control Panel URL</h1>
<table>
    <thead>
        <tr>
            <th>Control Panel URL</th>
            <th>Username</th>
            <th>Password</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><a href="http://panel.HostingCompany.com">http://panel.HostingCompany.com</a></td>
            <td>#user.Username#</td>
            <td>#user.Password#</td>
        </tr>
    </tbody>
</table>
</ad:if>

<h1>Hosting Spaces</h1>
<p>
    The following hosting spaces have been created under your account:
</p>
<ad:foreach collection="#Spaces#" var="Space" index="i">
<h2>#Space.PackageName#</h2>
<table>
	<tbody>
		<tr>
			<td class="Label">Hosting Plan:</td>
			<td>
				<ad:if test="#not(isnull(Plans[Space.PlanId]))#">#Plans[Space.PlanId].PlanName#<ad:else>System</ad:if>
			</td>
		</tr>
		<ad:if test="#not(isnull(Plans[Space.PlanId]))#">
		<tr>
			<td class="Label">Purchase Date:</td>
			<td>
				#Space.PurchaseDate#
			</td>
		</tr>
		<tr>
			<td class="Label">Disk Space, MB:</td>
			<td><ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Diskspace" /></td>
		</tr>
		<tr>
			<td class="Label">Bandwidth, MB/Month:</td>
			<td><ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Bandwidth" /></td>
		</tr>
		<tr>
			<td class="Label">Maximum Number of Domains:</td>
			<td><ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Domains" /></td>
		</tr>
		<tr>
			<td class="Label">Maximum Number of Sub-Domains:</td>
			<td><ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.SubDomains" /></td>
		</tr>
		</ad:if>
	</tbody>
</table>
</ad:foreach>

<ad:if test="#Signup#">
<p>
If you have any questions regarding your hosting account, feel free to contact our support department at any time.
</p>

<p>
Best regards,<br />
ACME Hosting Inc.<br />
Web Site: <a href="http://www.AcmeHosting.com">www.AcmeHosting.com</a><br />
E-Mail: <a href="mailto:support@AcmeHosting.com">support@AcmeHosting.com</a>
</p>
</ad:if>

<ad:template name="NumericQuota">
	<ad:if test="#space.Quotas.ContainsKey(quota)#">
		<ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot -1#">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if>
	<ad:else>
		0
	</ad:if>
</ad:template>

</div>
</body>
</html>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'Priority', N'Normal')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'Subject', N'<ad:if test="#Signup#">WebsitePanel  account has been created for<ad:else>WebsitePanel  account summary for</ad:if> #user.FirstName# #user.LastName#')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'AccountSummaryLetter', N'TextBody', N'=================================
   Hosting Account Information
=================================
<ad:if test="#Signup#">Hello #user.FirstName#,

New user account has been created and below you can find its summary information.

Control Panel URL: http://panel.AcmeHosting.com
Username: #user.Username#
Password: #user.Password#
</ad:if>

Hosting Spaces
==============
The following hosting spaces have been created under your account:

<ad:foreach collection="#Spaces#" var="Space" index="i">
=== #Space.PackageName# ===
Hosting Plan: <ad:if test="#not(isnull(Plans[Space.PlanId]))#">#Plans[Space.PlanId].PlanName#<ad:else>System</ad:if>
<ad:if test="#not(isnull(Plans[Space.PlanId]))#">Purchase Date: #Space.PurchaseDate#
Disk Space, MB: <ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Diskspace" />
Bandwidth, MB/Month: <ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Bandwidth" />
Maximum Number of Domains: <ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.Domains" />
Maximum Number of Sub-Domains: <ad:NumericQuota space="#SpaceContexts[Space.PackageId]#" quota="OS.SubDomains" />
</ad:if>
</ad:foreach>

<ad:if test="#Signup#">If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards,
ACME Hosting Inc.
Web Site: http://www.AcmeHosting.com">
E-Mail: support@AcmeHosting.com
</ad:if><ad:template name="NumericQuota"><ad:if test="#space.Quotas.ContainsKey(quota)#"><ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot -1#">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if><ad:else>0</ad:if></ad:template>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'DisplayPreferences', N'GridItems', N'11')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'ExchangePolicy', N'MailboxPasswordPolicy', N'True;8;20;0;2;0;True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'FtpPolicy', N'UserNamePolicy', N'True;-;1;20;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'FtpPolicy', N'UserPasswordPolicy', N'True;5;20;0;1;0;True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MailPolicy', N'AccountNamePolicy', N'True;;1;50;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MailPolicy', N'AccountPasswordPolicy', N'True;5;20;0;1;0;False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MailPolicy', N'CatchAllName', N'mail')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MsSqlPolicy', N'DatabaseNamePolicy', N'True;-;1;120;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MsSqlPolicy', N'UserNamePolicy', N'True;-;1;120;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MsSqlPolicy', N'UserPasswordPolicy', N'True;5;20;0;1;0;True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MySqlPolicy', N'DatabaseNamePolicy', N'True;;1;40;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MySqlPolicy', N'UserNamePolicy', N'True;;1;16;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'MySqlPolicy', N'UserPasswordPolicy', N'True;5;20;0;1;0;False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'OsPolicy', N'DsnNamePolicy', N'True;-;2;40;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'CC', N'support@HostingCompany.com')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'EnableLetter', N'True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'From', N'support@HostingCompany.com')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'HtmlBody', N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Hosting Space Summary Information</title>
    <style type="text/css">
		.Summary { background-color: ##ffffff; padding: 5px; }
		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }
        .Summary A { color: ##0153A4; }
        .Summary { font-family: Tahoma; font-size: 9pt; }
        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }
        .Summary H2 { font-size: 1.2em; } 
        .Summary TABLE { border: solid 1px ##e5e5e5; }
        .Summary TH,
        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }
        .Summary TD { padding: 8px; font-size: 9pt; }
        .Summary UL LI { font-size: 1.1em; font-weight: bold; }
        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }
    </style>
</head>
<body>
<div class="Summary">

<a name="top"></a>

<div class="Header">
	Hosting Space Information
</div>

<ad:if test="#Signup#">
<p>
Hello #user.FirstName#,
</p>

<p>
&quot;#space.Package.PackageName#&quot; hosting space has been created under your user account
and below is the summary information for its resources.
</p>
</ad:if>

<ul>
    <ad:if test="#Signup#">
		<li><a href="##cp">Control Panel URL</a></li>
	</ad:if>
    <li><a href="##overview">Hosting Space Overview</a></li>
    <ad:if test="#space.Groups.ContainsKey("Web")#">
    <li><a href="##web">Web</a></li>
    <ul>
        <li><a href="##weblimits">Limits</a></li>
        <li><a href="##dns">Name Servers</a></li>
        <li><a href="##sites">Web Sites</a></li>
        <li><a href="##tempurl">Temporary URL</a></li>
        <li><a href="##files">Files Location</a></li>
    </ul>
    </ad:if>
    <ad:if test="#space.Groups.ContainsKey("FTP")#">
		<li><a href="##ftp">FTP</a></li>
		<ul>
			<li><a href="##ftplimits">Limits</a></li>
			<li><a href="##ftpserver">FTP Server</a></li>
			<li><a href="##ftpaccounts">FTP Accounts</a></li>
		</ul>
	</ad:if>
    <ad:if test="#space.Groups.ContainsKey("Mail")#">
		<li><a href="##mail">Mail</a></li>
		<ul>
			<li><a href="##maillimits">Limits</a></li>
			<li><a href="##smtp">SMTP/POP3 Server</a></li>
			<li><a href="##mailaccounts">Mail Accounts</a></li>
		</ul>
    </ad:if>
    <li><a href="##db">Databases</a></li>
    <ul>
        <ad:if test="#space.Groups.ContainsKey("MsSQL2000")#"><li><a href="##mssql2000">SQL Server 2000</a></li></ad:if>
        <ad:if test="#space.Groups.ContainsKey("MsSQL2005")#"><li><a href="##mssql2005">SQL Server 2005</a></li></ad:if>
        <ad:if test="#space.Groups.ContainsKey("MsSQL2008")#"><li><a href="##mssql2008">SQL Server 2008</a></li></ad:if>
        <ad:if test="#space.Groups.ContainsKey("MySQL4")#"><li><a href="##mysql4">My SQL 4.x</a></li></ad:if>
        <ad:if test="#space.Groups.ContainsKey("MySQL5")#"><li><a href="##mysql5">My SQL 5.x</a></li></ad:if>
        <li><a href="##msaccess">Microsoft Access</a></li>
    </ul>
    <ad:if test="#space.Groups.ContainsKey("Statistics")#"><li><a href="##stats">Statistics</a></li></ad:if>
</ul>

<ad:if test="#Signup#">
<a name="cp"></a>
<h1>Control Panel URL</h1>
<table>
    <thead>
        <tr>
            <th>Control Panel URL</th>
            <th>Username</th>
            <th>Password</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><a href="http://panel.HostingCompany.com">http://panel.HostingCompany.com</a></td>
            <td>#user.Username#</td>
            <td>#user.Password#</td>
        </tr>
    </tbody>
</table>
</ad:if>

<a name="overview"></a>
<h1>Hosting Space Overview</h1>

<p>
    General hosting space limits:
</p>
<table>
    <tr>
        <td class="Label">Disk Space, MB:</td>
        <td><ad:NumericQuota quota="OS.Diskspace" /></td>
    </tr>
    <tr>
        <td class="Label">Bandwidth, MB/Month:</td>
        <td><ad:NumericQuota quota="OS.Bandwidth" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Domains:</td>
        <td><ad:NumericQuota quota="OS.Domains" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Sub-Domains:</td>
        <td><ad:NumericQuota quota="OS.SubDomains" /></td>
    </tr>
</table>

<ad:if test="#space.Groups.ContainsKey("Web")#">
<a name="web"></a>
<h1>Web</h1>
<a name="weblimits"></a>
<h2>
    Limits
</h2>
<table>
    <tr>
        <td class="Label">Maximum Number of Web Sites:</td>
        <td><ad:NumericQuota quota="Web.Sites" /></td>
    </tr>
	<tr>
        <td class="Label">Web Application Gallery:</td>
        <td><ad:BooleanQuota quota="Web.WebAppGallery" /></td>
    </tr>
    <tr>
        <td class="Label">Classic ASP:</td>
        <td><ad:BooleanQuota quota="Web.Asp" /></td>
    </tr>
    <tr>
        <td class="Label">ASP.NET 1.1:</td>
        <td><ad:BooleanQuota quota="Web.AspNet11" /></td>
    </tr>
    <tr>
        <td class="Label">ASP.NET 2.0:</td>
        <td><ad:BooleanQuota quota="Web.AspNet20" /></td>
    </tr>
    <tr>
        <td class="Label">ASP.NET 4.0:</td>
        <td><ad:BooleanQuota quota="Web.AspNet40" /></td>
    </tr>
    <tr>
        <td class="Label">PHP 4:</td>
        <td><ad:BooleanQuota quota="Web.Php4" /></td>
    </tr>
    <tr>
        <td class="Label">PHP 5:</td>
        <td><ad:BooleanQuota quota="Web.Php5" /></td>
    </tr>
    <tr>
        <td class="Label">Perl:</td>
        <td><ad:BooleanQuota quota="Web.Perl" /></td>
    </tr>
    <tr>
        <td class="Label">CGI-BIN:</td>
        <td><ad:BooleanQuota quota="Web.CgiBin" /></td>
    </tr>
</table>


<a name="dns"></a>
<h2>Name Servers</h2>
<p>
    In order to point your domain to the web site in this hosting space you should use the following Name Servers:
</p>
<table>
    <ad:foreach collection="#NameServers#" var="NameServer" index="i">
        <tr>
            <td class="Label">#NameServer#</td>
        </tr>
    </ad:foreach>
</table>
<p>
    You should change the name servers in domain registrar (Register.com, GoDaddy.com, etc.) control panel.
    Please, study domain registrar''s user manual for directions how to change name servers or contact your domain
    registrar directly by e-mail or phone.
</p>
<p>
    Please note, the changes in domain registrar database do not reflect immediately and sometimes it requires from
    12 to 48 hours till the end of DNS propagation.
</p>

<a name="sites"></a>
<h2>Web Sites</h2>
<p>
    The following web sites have been created under hosting space:
</p>
<table>
    <ad:foreach collection="#WebSites#" var="WebSite">
        <tr>
            <td><a href="http://#WebSite.Name#" target="_blank">http://#WebSite.Name#</a></td>
        </tr>
    </ad:foreach>
</table>
<p>
    * Please note, your web sites may not be accessible from 12 to 48 hours after you''ve changed name servers for their respective domains.
</p>

<ad:if test="#isnotempty(InstantAlias)#">
<a name="tempurl"></a>
<h2>Temporary URL</h2>
<p>
    You can access your web sites right now using their respective temporary URLs (instant aliases).
    Temporary URL is a sub-domain of the form http://yourdomain.com.providerdomain.com where &quot;yourdomain.com&quot; is your
    domain and &quot;providerdomain.com&quot; is the domain of your hosting provider.
</p>
<p>
    You can use the following Temporary URL for all your web sites:
</p>
<table>
    <tr>
        <td>
            http://YourDomain.com.<b>#InstantAlias#</b>
        </td>
    </tr>
</table>
</ad:if>

<a name="files"></a>
<h2>Files Location</h2>
<p>
    Sometimes it is required to know the physical location of the hosting space folder (absolute path).
    Hosting space folder is the folder where all hosting space files such as web sites content, web logs, data files, etc. are located.
</p>
<p>
    The root of your hosting space on our HDD is here:
</p>
<table>
    <tr>
        <td>
             #PackageRootFolder#
        </td>
    </tr>
</table>
<p>
    By default the root folder of any web site within your hosting space is built as following (you can change it anytime from the control panel):
</p>
<table>
    <tr>
        <td>
             #PackageRootFolder#\YourDomain.com\wwwroot
        </td>
    </tr>
</table>
</ad:if>


<ad:if test="#space.Groups.ContainsKey("FTP")#">
<a name="ftp"></a>
<h1>FTP</h1>

<a name="ftplimits"></a>
<h2>Limits</h2>
<table>
    <tr>
        <td class="Label">Maximum Number of FTP Accounts:</td>
        <td><ad:NumericQuota quota="FTP.Accounts" /></td>
    </tr>
</table>


<a name="ftpserver"></a>
<h2>FTP Server</h2>
<p>
Your hosting space allows working with your files by FTP.
You can use the following FTP server to access your space files remotely:
</p>
<table>
    <tr>
        <td><a href="ftp://#FtpIP#">ftp://#FtpIP#</a></td>
    </tr>
</table>
<p>
    Also, you can use the following domain names to access your FTP server:
</p>
<table>
    <tr>
        <td>ftp://ftp.YourDomain.com</td>
    </tr>
</table>
<ad:if test="#isnotempty(InstantAlias)#">
<p>
    During DNS propagation period (when domain name servers have been changed), similar to web sites, FTP server can be access with Temporary URL too:
</p>
<table>
    <tr>
        <td>ftp://ftp.YourDomain.com.<b>#InstantAlias#</b></td>
    </tr>
</table>
</ad:if>
<a name="ftpaccounts"></a>
<h2>FTP Accounts</h2>
<p>
    The following FTP accounts have been created under your hosting space and can be used to access FTP server:
</p>
<table>
    <thead>
        <tr>
            <th>Username</th>
            <ad:if test="#Signup#">
                <th>Password</th>
            </ad:if>
            <th>Folder</th>
        </tr>
    </thead>
    <tbody>
        <ad:foreach collection="#FtpAccounts#" var="FtpAcocunt" index="i">
            <tr>
                <td>#FtpAcocunt.Name#</td>
                <ad:if test="#Signup#">
                    <td>
                        #FtpAcocunt.Password#
                    </td>
                </ad:if>
                <td>#FtpAcocunt.Folder#</td>
            </tr>
        </ad:foreach>
    </tbody>
</table>
</ad:if>


<ad:if test="#space.Groups.ContainsKey("Mail")#">
<a name="mail"></a>
<h1>Mail</h1>

<a name="maillimits"></a>
<h2>Limits</h2>
<table>
    <tr>
        <td class="Label">Maximum Number of Mail Accounts:</td>
        <td><ad:NumericQuota quota="Mail.Accounts" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Mail Forwardings:</td>
        <td><ad:NumericQuota quota="Mail.Forwardings" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Mail Groups (Aliases):</td>
        <td><ad:NumericQuota quota="Mail.Groups" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Mailing Lists:</td>
        <td><ad:NumericQuota quota="Mail.Lists" /></td>
    </tr>
</table>

<a name="smtp"></a>
<h2>SMTP/POP3 Server</h2>
<p>
Below is the IP address of your POP3/SMTP/IMAP server. You can always access your mailbox(es)
using this IP address instead of actual POP3/SMTP/IMAP servers name:
</p>
<table>
	<tr>
		<td>
			#MailRecords[0].ExternalIP#
		</td>
	</tr>
</table>

<p>
    Also, you can use the following domain names to access SMTP/POP3 server from your favourite e-mail client software:
</p>
<table>
    <tr>
        <td>mail.YourDomain.com</td>
    </tr>
</table>

<ad:if test="#isnotempty(InstantAlias)#">
<p>
    During DNS propagation period (when domain name servers have been changed), similar to web sites, SMTP/POP3 server can be access with temporary domain too:
</p>
<table>
    <tr>
        <td>mail.YourDomain.com.<b>#InstantAlias#</b></td>
    </tr>
</table>
</ad:if>

<a name="mailaccounts"></a>
<h2>Mail Accounts</h2>
<p>
	The following mail accounts have been created under your hosting space:
</p>
<table>
	<thead>
		<tr>
			<th>E-mail</th>
			<th>Username (for POP3/SMTP/IMAP/WebMail)</th>
			<ad:if test="#Signup#">
				<th>Password</th>
			</ad:if>
		</tr>
	</thead>
	<tbody>
		<ad:foreach collection="#MailAccounts#" var="MailAccount">
			<tr>
				<td>#MailAccount.Name#</td>
				<td>#MailAccount.Name#</td>
				<ad:if test="#Signup#">
					<td>
						 #MailAccount.Password#
					</td>
				</ad:if>
			</tr>
		</ad:foreach>
	</tbody>
</table>
</ad:if>

<a name="db"></a>
<h1>Databases</h1>

<p>
	You can create databases and database users on "Space Home -&gt; Databases" screen in the control panel.
</p>

<ad:if test="#space.Groups.ContainsKey("MsSQL2000")#">
<a name="mssql2000"></a>

<h2>SQL Server 2000</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MsSQL2000.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MsSQL2000.Users" /></td>
    </tr>
</table>

<p>
	In order to connect to SQL Server 2000 from Management Studio, Enterprise Manager, Query Analyzer
	or other client software you can use the following SQL Server address:
</p>
<table>
	<tr>
		<td>#MsSQL2000Address#</td>
	</tr>
</table>
<ad:MsSqlConnectionStrings server="#MsSQL2000Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MsSQL2005")#">
<a name="mssql2005"></a>

<h2>SQL Server 2005</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MsSQL2005.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MsSQL2005.Users" /></td>
    </tr>
</table>

<p>
	In order to connect to SQL Server 2005 from Management Studio, Enterprise Manager, Query Analyzer
	or other client software you can use the following SQL Server address:
</p>
<table>
	<tr>
		<td>#MsSQL2005Address#</td>
	</tr>
</table>
<ad:MsSqlConnectionStrings server="#MsSQL2005Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MsSQL2008")#">
<a name="mssql2008"></a>

<h2>SQL Server 2008</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MsSQL2008.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MsSQL2008.Users" /></td>
    </tr>
</table>

<p>
	In order to connect to SQL Server 2008 from Management Studio, Enterprise Manager, Query Analyzer
	or other client software you can use the following SQL Server address:
</p>
<table>
	<tr>
		<td>#MsSQL2008Address#</td>
	</tr>
</table>
<ad:MsSqlConnectionStrings server="#MsSQL2008Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MySQL4")#">
<a name="mysql4"></a>
<h2>MySQL 4.x</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MySQL4.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MySQL4.Users" /></td>
    </tr>
</table>

<p>
	In order to connect to MySQL 4.x server you can use the following address:
</p>
<table>
	<tr>
		<td>#MySQL4Address#</td>
	</tr>
</table>
</ad:if>


<ad:if test="#space.Groups.ContainsKey("MySQL5")#">
<a name="mysql5"></a>
<h2>MySQL 5.x</h2>

<table>
    <tr>
        <td class="Label">Maximum Number of Databases:</td>
        <td><ad:NumericQuota quota="MySQL5.Databases" /></td>
    </tr>
    <tr>
        <td class="Label">Maximum Number of Users:</td>
        <td><ad:NumericQuota quota="MySQL5.Users" /></td>
    </tr>
</table>

<p>
	In order to connect to MySQL 5.x server you can use the following address:
</p>
<table>
	<tr>
		<td>#MySQL5Address#</td>
	</tr>
</table>
</ad:if>


<a name="msaccess"></a>
<h2>Microsoft Access</h2>
<p>
	Microsoft Access database are automatically allowed in any hosting plan. You can create/upload any number of Access
	database from File Manager in control panel.
</p>


<ad:if test="#space.Groups.ContainsKey("Statistics")#">
<a name="stats"></a>
<h1>Web Statistics</h1>

<table>
    <tr>
        <td class="Label">Maximum Number of Statistics Sites:</td>
        <td><ad:NumericQuota quota="Stats.Sites" /></td>
    </tr>
</table>

<p>
	You can view advanced statistics from your domain using URL of the following form:
</p>
<table>
	<tr>
		<td>http://stats.YourDomain.com</td>
	</tr>
</table>
<ad:if test="#isnotempty(InstantAlias)#">
<p>
    During DNS propagation period (when domain name servers have been changed), you can access web site statistics with Temporary URL:
</p>
<table>
    <tr>
        <td>http://stats.YourDomain.com.<b>#InstantAlias#</b></td>
    </tr>
</table>
</ad:if>
</ad:if>

<ad:if test="#Signup#">
<p>
If you have any questions regarding your hosting account, feel free to contact our support department at any time.
</p>

<p>
Best regards,<br />
ACME Hosting Inc.<br />
Web Site: <a href="http://www.AcmeHosting.com">www.AcmeHosting.com</a><br />
E-Mail: <a href="mailto:support@AcmeHosting.com">support@AcmeHosting.com</a>
</p>
</ad:if>

<!-- Templates -->
<ad:template name="MsSqlConnectionStrings">
<p>
	You may also use SQL Server address above in your application connection strings, for example:
</p>
<table>
	<tr>
		<td class="Label">Classic ASP (ADO Library)</td>
		<td>Provider=SQLOLEDB;Data source=<b>#server#</b>;Initial catalog=databaseName;User Id=userName;Password=password;</td>
	</tr>
	<tr>
		<td class="Label">ASP.NET (ADO.NET Library)</td>
		<td>Server=<b>#server#</b>;Database=databaseName;Uid=userName;Password=password;</td>
	</tr>
</table>
</ad:template>

<ad:template name="NumericQuota">
	<ad:if test="#space.Quotas.ContainsKey(quota)#">
		<ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot -1#">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if>
	<ad:else>
		0
	</ad:if>
</ad:template>

<ad:template name="BooleanQuota">
	<ad:if test="#space.Quotas.ContainsKey(quota)#">
		<ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot 0#">Enabled<ad:else>Disabled</ad:if>
	<ad:else>
		Disabled
	</ad:if>
</ad:template>

</div>
</body>
</html>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'Priority', N'Normal')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'Subject', N'"#space.Package.PackageName#" <ad:if test="#Signup#">hosting space has been created for<ad:else>hosting space summary for</ad:if> #user.FirstName# #user.LastName#')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PackageSummaryLetter', N'TextBody', N'================================
   Hosting Space Information
================================

<ad:if test="#Signup#">
Hello #user.FirstName#,

"#space.Package.PackageName#" hosting space has been created under your user account
and below is the summary information for its resources.
</ad:if>

Control Panel
=============
Control Panel URL: http://panel.AcmeHosting.com
Username: #user.Username#
Password: #user.Password#


Hosting Space Overview
======================
General hosting space limits:
Disk Space, MB: <ad:NumericQuota quota="OS.Diskspace" />
Bandwidth, MB/Month: <ad:NumericQuota quota="OS.Bandwidth" />
Maximum Number of Domains: <ad:NumericQuota quota="OS.Domains" />
Maximum Number of Sub-Domains: <ad:NumericQuota quota="OS.SubDomains" />

<ad:if test="#space.Groups.ContainsKey("Web")#">Web
======

Limits
------
Maximum Number of Web Sites: <ad:NumericQuota quota="Web.Sites" />
Web Application Gallery: <ad:BooleanQuota quota="Web.WebAppGallery" />
Classic ASP: <ad:BooleanQuota quota="Web.Asp" />
ASP.NET 1.1: <ad:BooleanQuota quota="Web.AspNet11" />
ASP.NET 2.0: <ad:BooleanQuota quota="Web.AspNet20" />
ASP.NET 4.0: <ad:BooleanQuota quota="Web.AspNet40" />
PHP 4: <ad:BooleanQuota quota="Web.Php4" />
PHP 5: <ad:BooleanQuota quota="Web.Php5" />
Perl: <ad:BooleanQuota quota="Web.Perl" />
CGI-BIN: <ad:BooleanQuota quota="Web.CgiBin" />

Name Servers
------------
In order to point your domain to the web site in this hosting space you should use the following Name Servers:
<ad:foreach collection="#NameServers#" var="NameServer" index="i">
   #NameServer#</ad:foreach>

You should change the name servers in domain registrar (Register.com, GoDaddy.com, etc.) control panel. Please, study domain registrar''s user manual for directions how to change name servers or contact your domain registrar directly by e-mail or phone.

Please note, the changes in domain registrar database do not reflect immediately and sometimes it requires from 12 to 48 hours till the end of DNS propagation.


Web Sites
---------
The following web sites have been created under hosting space:
<ad:foreach collection="#WebSites#" var="WebSite">
   http://#WebSite.Name#</ad:foreach>

* Please note, your web sites may not be accessible from 12 to 48 hours after you''ve changed name servers for their respective domains.

<ad:if test="#isnotempty(InstantAlias)#">Temporary URL
-------------
You can access your web sites right now using their respective temporary URLs (instant aliases). Temporary URL is a sub-domain of the form http://yourdomain.com.providerdomain.com where &quot;yourdomain.com&quot; is your domain and &quot;providerdomain.com&quot; is the domain of your hosting provider.

You can use the following Temporary URL for all your web sites:
    
    http://YourDomain.com.#InstantAlias#
</ad:if>


Files Location
--------------
Sometimes it is required to know the physical location of the hosting space folder (absolute path).
Hosting space folder is the folder where all hosting space files such as web sites content, web logs, data files, etc. are located.

The root of your hosting space on our HDD is here:
   
   #PackageRootFolder#
   
By default the root folder of any web site within your hosting space is built as following (you can change it anytime from the control panel):
   
   #PackageRootFolder#\YourDomain.com\wwwroot

</ad:if>


<ad:if test="#space.Groups.ContainsKey("FTP")#">FTP
=====

Limits
------
Maximum Number of FTP Accounts: <ad:NumericQuota quota="FTP.Accounts" />

FTP Server
----------
Your hosting space allows working with your files by FTP.
You can use the following FTP server to access your space files remotely:

   ftp://#FtpIP#
   
Also, you can use the following domain names to access your FTP server:

   ftp://ftp.YourDomain.com
   
<ad:if test="#isnotempty(InstantAlias)#">During DNS propagation period (when domain name servers have been changed), similar to web sites, FTP server can be access with Temporary URL too:
    
    ftp://ftp.YourDomain.com.#InstantAlias#
</ad:if>

FTP Accounts
------------
The following FTP accounts have been created under your hosting space and can be used to access FTP server:

<ad:foreach collection="#FtpAccounts#" var="FtpAcocunt" index="i">Username: #FtpAcocunt.Name#
<ad:if test="#Signup#">Password: #FtpAcocunt.Password#
</ad:if>Folder: #FtpAcocunt.Folder#

</ad:foreach>
</ad:if>

<ad:if test="#space.Groups.ContainsKey("Mail")#">Mail
========

Limits
------
Maximum Number of Mail Accounts: <ad:NumericQuota quota="Mail.Accounts" />
Maximum Number of Mail Forwardings: <ad:NumericQuota quota="Mail.Forwardings" />
Maximum Number of Mail Groups (Aliases): <ad:NumericQuota quota="Mail.Groups" />
Maximum Number of Mailing Lists: <ad:NumericQuota quota="Mail.Lists" />

SMTP/POP3 Server
----------------
Below is the IP address of your POP3/SMTP/IMAP server. You can always access your mailbox(es) using this IP address instead of actual POP3/SMTP/IMAP servers name:
   
   #MailRecords[0].ExternalIP#

Also, you can use the following domain names to access SMTP/POP3 server from your favourite e-mail client software:
   
   mail.YourDomain.com</td>

<ad:if test="#isnotempty(InstantAlias)#">During DNS propagation period (when domain name servers have been changed), similar to web sites, SMTP/POP3 server can be access with temporary domain too:

   mail.YourDomain.com.#InstantAlias#
</ad:if>

Mail Accounts
-------------
The following mail accounts have been created under your hosting space:
<ad:foreach collection="#MailAccounts#" var="MailAccount">E-mail: #MailAccount.Name#
<ad:if test="#Signup#">Password: #MailAccount.Password#</ad:if>
</ad:foreach>
</ad:if>


Databases
=========
You can create databases and database users on "Space Home -&gt; Databases" screen in the control panel.

<ad:if test="#space.Groups.ContainsKey("MsSQL2000")#">SQL Server 2000
---------------
Maximum Number of Databases: <ad:NumericQuota quota="MsSQL2000.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MsSQL2000.Users" />

In order to connect to SQL Server 2000 from Management Studio, Enterprise Manager, Query Analyzer or other client software you can use the following SQL Server address:
    
    #MsSQL2000Address#</td>

<ad:MsSqlConnectionStrings server="#MsSQL2000Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MsSQL2005")#">SQL Server 2005
---------------
Maximum Number of Databases: <ad:NumericQuota quota="MsSQL2005.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MsSQL2005.Users" />

In order to connect to SQL Server 2005 from Management Studio, Enterprise Manager, Query Analyzer or other client software you can use the following SQL Server address:
    
    #MsSQL2005Address#</td>

<ad:MsSqlConnectionStrings server="#MsSQL2005Address#" />
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MsSQL2008")#">SQL Server 2008
---------------
Maximum Number of Databases: <ad:NumericQuota quota="MsSQL2008.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MsSQL2008.Users" />

In order to connect to SQL Server 2008 from Management Studio, Enterprise Manager, Query Analyzer or other client software you can use the following SQL Server address:
    
    #MsSQL2008Address#</td>

<ad:MsSqlConnectionStrings server="#MsSQL2008Address#" />
</ad:if>


<ad:if test="#space.Groups.ContainsKey("MySQL4")#">MySQL 4.x
---------
Maximum Number of Databases: <ad:NumericQuota quota="MySQL4.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MySQL4.Users" />

In order to connect to MySQL 4.x server you can use the following address:
   
   #MySQL4Address#</td>
</ad:if>

<ad:if test="#space.Groups.ContainsKey("MySQL5")#">MySQL 5.x
---------
Maximum Number of Databases: <ad:NumericQuota quota="MySQL5.Databases" />
Maximum Number of Users: <ad:NumericQuota quota="MySQL5.Users" />

In order to connect to MySQL 5.x server you can use the following address:
   
   #MySQL5Address#</td>
</ad:if>

Microsoft Access
----------------
Microsoft Access database are automatically allowed in any hosting plan. You can create/upload any number of Access database from File Manager in control panel.

<ad:if test="#space.Groups.ContainsKey("Statistics")#">Web Statistics
==============
Maximum Number of Statistics Sites: <ad:NumericQuota quota="Stats.Sites" />

You can view advanced statistics from your domain using URL of the following form:

   http://stats.YourDomain.com
<ad:if test="#isnotempty(InstantAlias)#">
During DNS propagation period (when domain name servers have been changed), you can access web site statistics with Temporary URL:

   http://stats.YourDomain.com.#InstantAlias#
</ad:if>
</ad:if>

<ad:if test="#Signup#">If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards,
ACME Hosting Inc.
Web Site: http://www.AcmeHosting.com"
E-Mail: support@AcmeHosting.com
</ad:if>
<ad:template name="MsSqlConnectionStrings">You may also use SQL Server address above in your application connection strings, for example:

Classic ASP (ADO Library): Provider=SQLOLEDB;Data source=#server#;Initial catalog=databaseName;User Id=userName;Password=password;
ASP.NET (ADO.NET Library): Server=#server#;Database=databaseName;Uid=userName;Password=password;
</ad:template>
<ad:template name="NumericQuota"><ad:if test="#space.Quotas.ContainsKey(quota)#"><ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot -1#">#space.Quotas[quota].QuotaAllocatedValue#<ad:else>Unlimited</ad:if><ad:else>0</ad:if></ad:template>
<ad:template name="BooleanQuota"><ad:if test="#space.Quotas.ContainsKey(quota)#"><ad:if test="#space.Quotas[quota].QuotaAllocatedValue isnot 0#">Enabled<ad:else>Disabled</ad:if><ad:else>Disabled</ad:if></ad:template>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PasswordReminderLetter', N'CC', N'')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PasswordReminderLetter', N'From', N'support@HostingCompany.com')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PasswordReminderLetter', N'HtmlBody', N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Account Summary Information</title>
    <style type="text/css">
		.Summary { background-color: ##ffffff; padding: 5px; }
		.Summary .Header { padding: 10px 0px 10px 10px; font-size: 16pt; background-color: ##E5F2FF; color: ##1F4978; border-bottom: solid 2px ##86B9F7; }
        .Summary A { color: ##0153A4; }
        .Summary { font-family: Tahoma; font-size: 9pt; }
        .Summary H1 { font-size: 1.7em; color: ##1F4978; border-bottom: dotted 3px ##efefef; }
        .Summary H2 { font-size: 1.3em; color: ##1F4978; } 
        .Summary TABLE { border: solid 1px ##e5e5e5; }
        .Summary TH,
        .Summary TD.Label { padding: 5px; font-size: 8pt; font-weight: bold; background-color: ##f5f5f5; }
        .Summary TD { padding: 8px; font-size: 9pt; }
        .Summary UL LI { font-size: 1.1em; font-weight: bold; }
        .Summary UL UL LI { font-size: 0.9em; font-weight: normal; }
    </style>
</head>
<body>
<div class="Summary">

<a name="top"></a>
<div class="Header">
	Hosting Account Information
</div>

<p>
Hello #user.FirstName#,
</p>

<p>
Please, find below details of your control panel account.
</p>

<h1>Control Panel URL</h1>
<table>
    <thead>
        <tr>
            <th>Control Panel URL</th>
            <th>Username</th>
            <th>Password</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td><a href="http://panel.HostingCompany.com">http://panel.HostingCompany.com</a></td>
            <td>#user.Username#</td>
            <td>#user.Password#</td>
        </tr>
    </tbody>
</table>


<p>
If you have any questions regarding your hosting account, feel free to contact our support department at any time.
</p>

<p>
Best regards,<br />
ACME Hosting Inc.<br />
Web Site: <a href="http://www.AcmeHosting.com">www.AcmeHosting.com</a><br />
E-Mail: <a href="mailto:support@AcmeHosting.com">support@AcmeHosting.com</a>
</p>

</div>
</body>
</html>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PasswordReminderLetter', N'Priority', N'Normal')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PasswordReminderLetter', N'Subject', N'Password reminder for #user.FirstName# #user.LastName#')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'PasswordReminderLetter', N'TextBody', N'=================================
   Hosting Account Information
=================================

Hello #user.FirstName#,

Please, find below details of your control panel account.

Control Panel URL: http://panel.AcmeHosting.com
Username: #user.Username#
Password: #user.Password#

If you have any questions regarding your hosting account, feel free to contact our support department at any time.

Best regards,
ACME Hosting Inc.
Web Site: http://www.AcmeHosting.com"
E-Mail: support@AcmeHosting.com')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'SharePointPolicy', N'GroupNamePolicy', N'True;-;1;20;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'SharePointPolicy', N'UserNamePolicy', N'True;-;1;20;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'SharePointPolicy', N'UserPasswordPolicy', N'True;5;20;0;1;0;True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'AddParkingPage', N'True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'AddRandomDomainString', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'AnonymousAccountPolicy', N'True;;5;20;;_web;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'AspInstalled', N'True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'AspNetInstalled', N'2')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'CgiBinInstalled', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'DefaultDocuments', N'Default.htm,Default.asp,index.htm,Default.aspx')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableAnonymousAccess', N'True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableBasicAuthentication', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableDedicatedPool', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableDirectoryBrowsing', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableParentPaths', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableWindowsAuthentication', N'True')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'EnableWritePermissions', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'FrontPageAccountPolicy', N'True;;1;20;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'FrontPagePasswordPolicy', N'True;5;20;0;1;0;False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'ParkingPageContent', N'<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>The web site is under construction</title>
<style type="text/css">
	BODY { color: #444444; background-color: #E5F2FF; font-family: verdana; margin: 0px; }
	#PageOutline { text-align: center; margin-top: 300px; }
	A { color: #0153A4; }
	H1 { font-size: 16pt; margin-bottom: 4px; }
	H2 { font-size: 14pt; margin-bottom: 4px; font-weight: normal; }
</style>
</head>
<body>
<div id="PageOutline">
	<h1>This web site has just been created from <a href="http://www.websitepanel.net">WebsitePanel </a> and it is still under construction.</h1>
	<h2>The web site is hosted by <a href="http://www.AcmeHosting.com">AcmeHosting</a>.</h2>
</div>
</body>
</html>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'ParkingPageName', N'default.aspx')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'PerlInstalled', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'PhpInstalled', N'')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'PythonInstalled', N'False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'SecuredGroupNamePolicy', N'True;;1;20;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'SecuredUserNamePolicy', N'True;;1;20;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'SecuredUserPasswordPolicy', N'True;5;20;0;1;0;False')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'VirtDirNamePolicy', N'True;-;3;50;;;')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'WebDataFolder', N'\[DOMAIN_NAME]\data')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'WebLogsFolder', N'\[DOMAIN_NAME]\logs')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebPolicy', N'WebRootFolder', N'\[DOMAIN_NAME]\wwwroot')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebsitePanelPolicy', N'DemoMessage', N'When user account is in demo mode the majority of operations are
disabled, especially those ones that modify or delete records.
You are welcome to ask your questions or place comments about
this demo on  <a href="http://forum.websitepanel.net"
target="_blank">WebsitePanel  Support Forum</a>')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebsitePanelPolicy', N'ForbiddenIP', N'')
GO
INSERT [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES (1, N'WebsitePanelPolicy', N'PasswordPolicy', N'True;6;20;0;1;0;True')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE CheckUserExists
(
	@Exists bit OUTPUT,
	@Username nvarchar(100)
)
AS

SET @Exists = 0

IF EXISTS (SELECT UserID FROM Users
WHERE Username = @Username)
SET @Exists = 1

RETURN

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CheckSSLExistsForWebsite]
(
	@siteID int,	
	@SerialNumber nvarchar(250),
	@Result bit OUTPUT
)
AS

/*
@Result values:
	0 - OK
	-1 - already exists
*/

SET @Result = 0 -- OK

-- check if a SSL Certificate is installed for domain
IF EXISTS(SELECT [ID] FROM [dbo].[SSLCertificates] WHERE [SiteID] = @siteID 
--AND SerialNumber=@SerialNumber
)
BEGIN
	SET @Result = 1
	RETURN
END

RETURN

SET ANSI_NULLS ON

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CheckSSL]
(
	@siteID int,
	@Renewal bit = 0,	
	@Result int OUTPUT
)
AS

/*
@Result values:
	0 - OK
	-1 - already exists
*/

SET @Result = 0 -- OK

-- check if a SSL Certificate is installed for domain
IF EXISTS(SELECT [ID] FROM [dbo].[SSLCertificates] WHERE [SiteID] = @siteID)
BEGIN
	SET @Result = -1
	RETURN
END

--To Do add renewal stuff

RETURN

SET ANSI_NULLS ON

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE PROCEDURE [dbo].[CheckOCSUserExists] 
	@AccountID int
AS
BEGIN	
	SELECT 
		COUNT(AccountID)
	FROM 
		dbo.OCSUsers
	WHERE AccountID = @AccountID
END












GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleTaskViewConfiguration](
	[TaskID] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[ConfigurationID] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[Environment] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
	[Description] [nvarchar](100) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_ScheduleTaskViewConfiguration] PRIMARY KEY CLUSTERED 
(
	[ConfigurationID] ASC,
	[TaskID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_ACTIVATE_PAID_INVOICES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_BACKUP', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/Backup.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_BACKUP_DATABASE', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/BackupDatabase.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_CALCULATE_EXCHANGE_DISKSPACE', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_CALCULATE_PACKAGES_BANDWIDTH', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_CALCULATE_PACKAGES_DISKSPACE', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_CANCEL_OVERDUE_INVOICES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_CHECK_WEBSITE', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/CheckWebsite.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_FTP_FILES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/SendFilesViaFtp.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_GENERATE_INVOICES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_HOSTED_SOLUTION_REPORT', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/HostedSolutionReport.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_RUN_PAYMENT_QUEUE', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_RUN_SYSTEM_COMMAND', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/ExecuteSystemCommand.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_SEND_MAIL', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/SendEmailNotification.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_SUSPEND_OVERDUE_INVOICES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/EmptyView.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_SUSPEND_PACKAGES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/SuspendOverusedSpaces.ascx')
GO
INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_ZIP_FILES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/ZipFiles.ascx')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE UpdateDnsRecord
(
	@ActorID int,
	@RecordID int,
	@RecordType nvarchar(10),
	@RecordName nvarchar(50),
	@RecordData nvarchar(500),
	@MXPriority int,
	@IPAddressID int
)
AS

IF @IPAddressID = 0 SET @IPAddressID = NULL

-- check rights
DECLARE @ServiceID int, @ServerID int, @PackageID int
SELECT
	@ServiceID = ServiceID,
	@ServerID = ServerID,
	@PackageID = PackageID
FROM GlobalDnsRecords
WHERE
	RecordID = @RecordID

IF (@ServiceID > 0 OR @ServerID > 0) AND dbo.CheckIsUserAdmin(@ActorID) = 0
RAISERROR('You are not allowed to perform this operation', 16, 1)

IF (@PackageID > 0) AND dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)


-- update record
UPDATE GlobalDnsRecords
SET
	RecordType = @RecordType,
	RecordName = @RecordName,
	RecordData = @RecordData,
	MXPriority = @MXPriority,
	IPAddressID = @IPAddressID
WHERE
	RecordID = @RecordID
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetDnsRecord
(
	@ActorID int,
	@RecordID int
)
AS

-- check rights
DECLARE @ServiceID int, @ServerID int, @PackageID int
SELECT
	@ServiceID = ServiceID,
	@ServerID = ServerID,
	@PackageID = PackageID
FROM GlobalDnsRecords
WHERE
	RecordID = @RecordID

IF (@ServiceID > 0 OR @ServerID > 0) AND dbo.CheckIsUserAdmin(@ActorID) = 0
RAISERROR('You are not allowed to perform this operation', 16, 1)

IF (@PackageID > 0) AND dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	NR.RecordID,
	NR.ServiceID,
	NR.ServerID,
	NR.PackageID,
	NR.RecordType,
	NR.RecordName,
	NR.RecordData,
	NR.MXPriority,
	NR.IPAddressID
FROM
	GlobalDnsRecords AS NR
WHERE NR.RecordID = @RecordID
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE DeleteDnsRecord
(
	@ActorID int,
	@RecordID int
)
AS

-- check rights
DECLARE @ServiceID int, @ServerID int, @PackageID int
SELECT
	@ServiceID = ServiceID,
	@ServerID = ServerID,
	@PackageID = PackageID
FROM GlobalDnsRecords
WHERE
	RecordID = @RecordID

IF (@ServiceID > 0 OR @ServerID > 0) AND dbo.CheckIsUserAdmin(@ActorID) = 0
RETURN

IF (@PackageID > 0) AND dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RETURN

-- delete record
DELETE FROM GlobalDnsRecords
WHERE RecordID = @RecordID

RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE AddDnsRecord
(
	@ActorID int,
	@ServiceID int,
	@ServerID int,
	@PackageID int,
	@RecordType nvarchar(10),
	@RecordName nvarchar(50),
	@RecordData nvarchar(500),
	@MXPriority int,
	@IPAddressID int
)
AS

IF (@ServiceID > 0 OR @ServerID > 0) AND dbo.CheckIsUserAdmin(@ActorID) = 0
RAISERROR('You should have administrator role to perform such operation', 16, 1)

IF (@PackageID > 0) AND dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

IF @ServiceID = 0 SET @ServiceID = NULL
IF @ServerID = 0 SET @ServerID = NULL
IF @PackageID = 0 SET @PackageID = NULL
IF @IPAddressID = 0 SET @IPAddressID = NULL

IF EXISTS
(
	SELECT RecordID FROM GlobalDnsRecords WHERE
	ServiceID = @ServiceID AND ServerID = @ServerID AND PackageID = @PackageID
	AND RecordName = @RecordName AND RecordType = @RecordType
)
	
	UPDATE GlobalDnsRecords
	SET
		RecordData = RecordData,
		MXPriority = MXPriority,
		IPAddressID = @IPAddressID
	WHERE
		ServiceID = @ServiceID AND ServerID = @ServerID AND PackageID = @PackageID
ELSE
	INSERT INTO GlobalDnsRecords
	(
		ServiceID,
		ServerID,
		PackageID,
		RecordType,
		RecordName,
		RecordData,
		MXPriority,
		IPAddressID
	)
	VALUES
	(
		@ServiceID,
		@ServerID,
		@PackageID,
		@RecordType,
		@RecordName,
		@RecordData,
		@MXPriority,
		@IPAddressID
	)

RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[ServiceID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NOT NULL,
	[ProviderID] [int] NOT NULL,
	[ServiceName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[Comments] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[ServiceQuotaValue] [int] NULL,
	[ClusterID] [int] NULL,
 CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[GetServiceItemsByService]
(
	@ActorID int,
	@ServiceID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
WHERE SI.ServiceID = @ServiceID


-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	SIT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	P.PackageName,
	S.ServiceID,
	S.ServiceName,
	SRV.ServerID,
	SRV.ServerName,
	RG.GroupName,
	U.UserID,
	U.Username,
	(U.FirstName + U.LastName) AS UserFullName,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN ResourceGroups AS RG ON SIT.GroupID = RG.GroupID
INNER JOIN Users AS U ON P.UserID = U.UserID
WHERE @IsAdmin = 1

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID
WHERE @IsAdmin = 1

RETURN






















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[GetServiceItemsByPackage]
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
WHERE SI.PackageID = @PackageID


-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	SIT.TypeName,
	SIT.DisplayName,
	SI.ServiceID,
	SI.PackageID,
	P.PackageName,
	S.ServiceID,
	S.ServiceName,
	SRV.ServerID,
	SRV.ServerName,
	RG.GroupName,
	U.UserID,
	U.Username,
	(U.FirstName + U.LastName) AS UserFullName,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN ResourceGroups AS RG ON SIT.GroupID = RG.GroupID
INNER JOIN Users AS U ON P.UserID = U.UserID

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID

RETURN





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[GetServiceItemsByName]
(
	@ActorID int,
	@PackageID int,
	@ItemName nvarchar(500)
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
WHERE SI.PackageID = @PackageID
AND SI.ItemName LIKE @ItemName


-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	SIT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	P.PackageName,
	S.ServiceID,
	S.ServiceName,
	SRV.ServerID,
	SRV.ServerName,
	RG.GroupName,
	U.UserID,
	U.Username,
	U.FullName AS UserFullName,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN ResourceGroups AS RG ON SIT.GroupID = RG.GroupID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID


RETURN 























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[GetServiceItems]
(
	@ActorID int,
	@PackageID int,
	@ItemTypeName nvarchar(200),
	@GroupName nvarchar(100) = NULL,
	@Recursive bit
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
INNER JOIN PackagesTree(@PackageID, @Recursive) AS PT ON SI.PackageID = PT.PackageID
INNER JOIN ServiceItemTypes AS IT ON SI.ItemTypeID = IT.ItemTypeID
INNER JOIN ResourceGroups AS RG ON IT.GroupID = RG.GroupID
WHERE IT.TypeName = @ItemTypeName
AND ((@GroupName IS NULL) OR (@GroupName IS NOT NULL AND RG.GroupName = @GroupName))


-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	SIT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	P.PackageName,
	S.ServiceID,
	S.ServiceName,
	SRV.ServerID,
	SRV.ServerName,
	RG.GroupName,
	U.UserID,
	U.Username,
	(U.FirstName + U.LastName) AS UserFullName,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN ResourceGroups AS RG ON SIT.GroupID = RG.GroupID
INNER JOIN Users AS U ON P.UserID = U.UserID

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID

RETURN 





















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[GetServiceItemByName]
(
	@ActorID int,
	@PackageID int,
	@ItemName nvarchar(500),
	@GroupName nvarchar(100) = NULL,
	@ItemTypeName nvarchar(200)
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN ResourceGroups AS RG ON SIT.GroupID = RG.GroupID
WHERE SI.PackageID = @PackageID AND SIT.TypeName = @ItemTypeName
AND SI.ItemName = @ItemName
AND ((@GroupName IS NULL) OR (@GroupName IS NOT NULL AND RG.GroupName = @GroupName))


-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	SIT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	P.PackageName,
	S.ServiceID,
	S.ServiceName,
	SRV.ServerID,
	SRV.ServerName,
	RG.GroupName,
	U.UserID,
	U.Username,
	U.FullName AS UserFullName,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID


RETURN 






















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


















CREATE PROCEDURE [dbo].[GetServiceItem]
(
	@ActorID int,
	@ItemID int
)
AS

DECLARE @Items TABLE
(
	ItemID int
)

-- find service items
INSERT INTO @Items
SELECT
	SI.ItemID
FROM ServiceItems AS SI
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
WHERE
	SI.ItemID = @ItemID
	AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1


-- select service items
SELECT
	SI.ItemID,
	SI.ItemName,
	SI.ItemTypeID,
	SIT.TypeName,
	SI.ServiceID,
	SI.PackageID,
	P.PackageName,
	S.ServiceID,
	S.ServiceName,
	SRV.ServerID,
	SRV.ServerName,
	RG.GroupName,
	U.UserID,
	U.Username,
	U.FullName AS UserFullName,
	SI.CreatedDate
FROM @Items AS FI
INNER JOIN ServiceItems AS SI ON FI.ItemID = SI.ItemID
INNER JOIN ServiceItemTypes AS SIT ON SI.ItemTypeID = SIT.ItemTypeID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID

-- select item properties
-- get corresponding item properties
SELECT
	IP.ItemID,
	IP.PropertyName,
	IP.PropertyValue
FROM ServiceItemProperties AS IP
INNER JOIN @Items AS FI ON IP.ItemID = FI.ItemID


RETURN 






















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VirtualServices](
	[VirtualServiceID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
 CONSTRAINT [PK_VirtualServices] PRIMARY KEY CLUSTERED 
(
	[VirtualServiceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE FUNCTION dbo.GetPackageAllocatedResource
(
	@PackageID int,
	@GroupID int,
	@ServerID int
)
RETURNS bit
AS
BEGIN

IF @PackageID IS NULL
RETURN 1

DECLARE @Result bit
SET @Result = 1 -- enabled

DECLARE @PID int, @ParentPackageID int
SET @PID = @PackageID

DECLARE @OverrideQuotas bit

IF @ServerID IS NULL OR @ServerID = 0
SELECT @ServerID = ServerID FROM Packages
WHERE PackageID = @PackageID

WHILE 1 = 1
BEGIN

	DECLARE @GroupEnabled int
	
	-- get package info
	SELECT
		@ParentPackageID = ParentPackageID,
		@OverrideQuotas = OverrideQuotas
	FROM Packages WHERE PackageID = @PID

	-- check if this is a root 'System' package
	SET @GroupEnabled = 1 -- enabled
	IF @ParentPackageID IS NULL
	BEGIN
	
		IF @ServerID = -1 OR @ServerID IS NULL
		RETURN 1
	
		IF EXISTS (SELECT VirtualServer FROM Servers WHERE ServerID = @ServerID AND VirtualServer = 1)
		BEGIN
			IF NOT EXISTS(
				SELECT 
					DISTINCT(PROV.GroupID)
				FROM VirtualServices AS VS
				INNER JOIN Services AS S ON VS.ServiceID = S.ServiceID
				INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
				WHERE PROV.GroupID = @GroupID AND VS.ServerID = @ServerID
			)
			SET @GroupEnabled = 0
		END
		ELSE
		BEGIN
			IF NOT EXISTS(
				SELECT 
					DISTINCT(PROV.GroupID)
				FROM Services AS S
				INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
				WHERE PROV.GroupID = @GroupID AND  S.ServerID = @ServerID
			)
			SET @GroupEnabled = 0
		END
		
		RETURN @GroupEnabled -- exit from the loop
	END
	ELSE -- parentpackage is not null
	BEGIN
		-- check the current package
		IF @OverrideQuotas = 1
		BEGIN
			IF NOT EXISTS(
				SELECT GroupID FROM PackageResources WHERE GroupID = @GroupID AND PackageID = @PID
			)
			SET @GroupEnabled = 0
		END
		ELSE
		BEGIN
			IF NOT EXISTS(
				SELECT HPR.GroupID FROM Packages AS P
				INNER JOIN HostingPlanResources AS HPR ON P.PlanID = HPR.PlanID
				WHERE HPR.GroupID = @GroupID AND P.PackageID = @PID
			)
			SET @GroupEnabled = 0
		END
		
		-- check addons
		IF EXISTS(
			SELECT HPR.GroupID FROM PackageAddons AS PA
			INNER JOIN HostingPlanResources AS HPR ON PA.PlanID = HPR.PlanID
			WHERE HPR.GroupID = @GroupID AND PA.PackageID = @PID
			AND PA.StatusID = 1 -- active add-on
		)
		SET @GroupEnabled = 1
	END
	
	IF @GroupEnabled = 0
		RETURN 0
	
	SET @PID = @ParentPackageID

END -- end while

RETURN @Result
END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VirtualGroups](
	[VirtualGroupID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[DistributionType] [int] NULL,
	[BindDistributionToPrimary] [bit] NULL,
 CONSTRAINT [PK_VirtualGroups] PRIMARY KEY CLUSTERED 
(
	[VirtualGroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE [dbo].[DistributePackageServices]
(
	@ActorID int,
	@PackageID int
)
AS

-- get primary distribution group
DECLARE @PrimaryGroupID int
DECLARE @VirtualServer bit
DECLARE @PlanID int
DECLARE @ServerID int
SELECT
	@PrimaryGroupID = ISNULL(S.PrimaryGroupID, 0),
	@VirtualServer = S.VirtualServer,
	@PlanID = P.PlanID,
	@ServerID = P.ServerID
FROM Packages AS P
INNER JOIN Servers AS S ON P.ServerID = S.ServerID
WHERE P.PackageID = @PackageID


-- get the list of available groups from hosting plan
DECLARE @Groups TABLE
(
	GroupID int,
	PrimaryGroup bit
)

INSERT INTO @Groups (GroupID, PrimaryGroup)
SELECT
	RG.GroupID,
	CASE WHEN RG.GroupID = @PrimaryGroupID THEN 1 -- mark primary group
	ELSE 0
	END
FROM ResourceGroups AS RG
WHERE dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, NULL) = 1
AND RG.GroupID NOT IN 
(
	SELECT P.GroupID
	FROM PackageServices AS PS
	INNER JOIN Services AS S ON PS.ServiceID = S.ServiceID
	INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
	WHERE PS.PackageID = @PackageID
)

IF @VirtualServer <> 1
BEGIN
	-- PHYSICAL SERVER
	-- just return the list of services based on the plan
	INSERT INTO PackageServices (PackageID, ServiceID)
	SELECT
		@PackageID,
		S.ServiceID
	FROM Services AS S
	INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
	INNER JOIN @Groups AS G ON P.GroupID = G.GroupID
	WHERE S.ServerID = @ServerID
		AND S.ServiceID NOT IN (SELECT ServiceID FROM PackageServices WHERE PackageID = @PackageID)
END
ELSE
BEGIN
	-- VIRTUAL SERVER
	
	DECLARE @GroupID int, @PrimaryGroup int
	DECLARE GroupsCursor CURSOR FOR
	SELECT GroupID, PrimaryGroup FROM @Groups
	ORDER BY PrimaryGroup DESC

	OPEN GroupsCursor

	WHILE (10 = 10)
	BEGIN    --LOOP 10: thru groups
		FETCH NEXT FROM GroupsCursor
		INTO @GroupID, @PrimaryGroup

		IF (@@fetch_status <> 0)
		BEGIN
			DEALLOCATE GroupsCursor
			BREAK
		END
		
		-- read group information
		DECLARE @DistributionType int, @BindDistributionToPrimary int
		SELECT
			@DistributionType = DistributionType,
			@BindDistributionToPrimary = BindDistributionToPrimary
		FROM VirtualGroups AS VG
		WHERE ServerID = @ServerID AND GroupID = @GroupID
		
		-- bind distribution to primary
		IF @BindDistributionToPrimary = 1 AND @PrimaryGroup = 0 AND @PrimaryGroupID <> 0
		BEGIN
			-- if only one service found just use it and do not distribute			
			IF (SELECT COUNT(*) FROM VirtualServices AS VS
				INNER JOIN Services AS S ON VS.ServiceID = S.ServiceID
				INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
				WHERE VS.ServerID = @ServerID AND P.GroupID = @GroupID) = 1
				BEGIN
					INSERT INTO PackageServices (PackageID, ServiceID)
					SELECT
						@PackageID,
						VS.ServiceID
					FROM VirtualServices AS VS
					INNER JOIN Services AS S ON VS.ServiceID = S.ServiceID
					INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
					WHERE VS.ServerID = @ServerID AND P.GroupID = @GroupID
				END
			ELSE
				BEGIN
					DECLARE @PrimaryServerID int
					-- try to get primary distribution server
					SELECT
						@PrimaryServerID = S.ServerID
					FROM PackageServices AS PS
					INNER JOIN Services AS S ON PS.ServiceID = S.ServiceID
					INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
					WHERE PS.PackageID = @PackageID AND P.GroupID = @PrimaryGroupID
					
					INSERT INTO PackageServices (PackageID, ServiceID)
					SELECT
						@PackageID,
						VS.ServiceID
					FROM VirtualServices AS VS
					INNER JOIN Services AS S ON VS.ServiceID = S.ServiceID
					INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
					WHERE VS.ServerID = @ServerID AND P.GroupID = @GroupID AND S.ServerID = @PrimaryServerID
				END
		END
		ELSE
		BEGIN
			
			-- DISTRIBUTION
			DECLARE @Services TABLE
			(
				ServiceID int,
				ItemsNumber int,
				RandomNumber int
			)
			
			DELETE FROM @Services
			
			INSERT INTO @Services (ServiceID, ItemsNumber, RandomNumber)
			SELECT
				VS.ServiceID,
				(SELECT COUNT(ItemID) FROM ServiceItems WHERE ServiceID = VS.ServiceID),
				RAND()
			FROM VirtualServices AS VS
			INNER JOIN Services AS S ON VS.ServiceID = S.ServiceID
			INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
			WHERE VS.ServerID = @ServerID AND P.GroupID = @GroupID
			
			-- BALANCED DISTRIBUTION
			IF @DistributionType = 1
			BEGIN
				-- get the less allocated service
				INSERT INTO PackageServices (PackageID, ServiceID)
				SELECT TOP 1
					@PackageID,
					ServiceID
				FROM @Services
				ORDER BY ItemsNumber
			END
			ELSE
			-- RANDOMIZED DISTRIBUTION
			BEGIN
				-- get the less allocated service
				INSERT INTO PackageServices (PackageID, ServiceID)
				SELECT TOP 1
					@PackageID,
					ServiceID
				FROM @Services
				ORDER BY RandomNumber
			END
		END
		
		IF @PrimaryGroup = 1
		SET @PrimaryGroupID = @GroupID

	END -- while groups
	
END -- end virtual server

RETURN































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetPackageServiceID
(
	@ActorID int,
	@PackageID int,
	@GroupName nvarchar(100),
	@ServiceID int OUTPUT
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SET @ServiceID = 0

-- load group info
DECLARE @GroupID int
SELECT @GroupID = GroupID FROM ResourceGroups
WHERE GroupName = @GroupName

-- check if user has this resource enabled
IF dbo.GetPackageAllocatedResource(@PackageID, @GroupID, NULL) = 0
BEGIN
	-- remove all resource services from the space
	DELETE FROM PackageServices FROM PackageServices AS PS
	INNER JOIN Services AS S ON PS.ServiceID = S.ServiceID
	INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
	WHERE P.GroupID = @GroupID AND PS.PackageID = @PackageID
	RETURN
END

-- check if the service is already distributed
SELECT
	@ServiceID = PS.ServiceID
FROM PackageServices AS PS
INNER JOIN Services AS S ON PS.ServiceID = S.ServiceID
INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
WHERE PS.PackageID = @PackageID AND P.GroupID = @GroupID

IF @ServiceID <> 0
RETURN

-- distribute services
EXEC DistributePackageServices @ActorID, @PackageID

-- get distributed service again
SELECT
	@ServiceID = PS.ServiceID
FROM PackageServices AS PS
INNER JOIN Services AS S ON PS.ServiceID = S.ServiceID
INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
WHERE PS.PackageID = @PackageID AND P.GroupID = @GroupID

RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE CheckServiceItemExists
(
	@Exists bit OUTPUT,
	@ItemName nvarchar(500),
	@ItemTypeName nvarchar(200),
	@GroupName nvarchar(100) = NULL
)
AS

SET @Exists = 0

DECLARE @ItemTypeID int
SELECT @ItemTypeID = ItemTypeID FROM ServiceItemTypes
WHERE TypeName = @ItemTypeName

IF EXISTS (
SELECT ItemID FROM ServiceItems AS SI
INNER JOIN Services AS S ON SI.ServiceID = S.ServiceID
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
WHERE SI.ItemName = @ItemName AND SI.ItemTypeID = @ItemTypeID
AND ((@GroupName IS NULL) OR (@GroupName IS NOT NULL AND RG.GroupName = @GroupName))
)
SET @Exists = 1

RETURN

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

















CREATE PROCEDURE [dbo].[AddServiceItem]
(
	@ActorID int,
	@PackageID int,
	@ServiceID int,
	@ItemName nvarchar(500),
	@ItemTypeName nvarchar(200),
	@ItemID int OUTPUT,
	@XmlProperties ntext,
	@CreatedDate datetime
)
AS
BEGIN TRAN

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- get GroupID
DECLARE @GroupID int
SELECT
	@GroupID = PROV.GroupID
FROM Services AS S
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
WHERE S.ServiceID = @ServiceID

DECLARE @ItemTypeID int
SELECT @ItemTypeID = ItemTypeID FROM ServiceItemTypes
WHERE TypeName = @ItemTypeName
AND ((@GroupID IS NULL) OR (@GroupID IS NOT NULL AND GroupID = @GroupID))

-- add item
INSERT INTO ServiceItems
(
	PackageID,
	ServiceID,
	ItemName,
	ItemTypeID,
	CreatedDate
)
VALUES
(
	@PackageID,
	@ServiceID,
	@ItemName,
	@ItemTypeID,
	@CreatedDate
)

SET @ItemID = SCOPE_IDENTITY()

DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @XmlProperties

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM ServiceItemProperties
WHERE ItemID = @ItemID

INSERT INTO ServiceItemProperties
(
	ItemID,
	PropertyName,
	PropertyValue
)
SELECT
	@ItemID,
	PropertyName,
	PropertyValue
FROM OPENXML(@idoc, '/properties/property',1) WITH 
(
	PropertyName nvarchar(50) '@name',
	PropertyValue nvarchar(3000) '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN
RETURN 




















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceDefaultProperties](
	[ProviderID] [int] NOT NULL,
	[PropertyName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyValue] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_ServiceDefaultProperties_1] PRIMARY KEY CLUSTERED 
(
	[ProviderID] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (1, N'UsersHome', N'%SYSTEMDRIVE%\HostingSpaces')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspNet11Path', N'%SYSTEMROOT%\Microsoft.NET\Framework\v1.1.4322\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspNet11Pool', N'ASP.NET V1.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspNet20Path', N'%SYSTEMROOT%\Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspNet20Pool', N'ASP.NET V2.0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspNet40Path', N'%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspNet40Pool', N'ASP.NET V4.0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'AspPath', N'%SYSTEMROOT%\System32\InetSrv\asp.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'CFFlashRemotingDirectory', N'C:\ColdFusion9\runtime\lib\wsconfig\1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'CFScriptsDirectory', N'C:\Inetpub\wwwroot\CFIDE')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'ColdFusionPath', N'C:\ColdFusion9\runtime\lib\wsconfig\jrun_iis6.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'GalleryXmlFeedUrl', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'PerlPath', N'%SYSTEMDRIVE%\Perl\bin\Perl.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'Php4Path', N'%PROGRAMFILES%\PHP\php.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'Php5Path', N'%PROGRAMFILES%\PHP\php-cgi.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'ProtectedAccessFile', N'.htaccess')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'ProtectedFoldersFile', N'.htfolders')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'ProtectedGroupsFile', N'.htgroup')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'ProtectedUsersFile', N'.htpasswd')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'PythonPath', N'%SYSTEMDRIVE%\Python\python.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'SecuredFoldersFilterPath', N'%SYSTEMROOT%\System32\InetSrv\IISPasswordFilter.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (2, N'WebGroupName', N'WSPWebUsers')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (3, N'FtpGroupName', N'WSPFtpUsers')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (3, N'SiteId', N'MSFTPSVC/1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'DatabaseLocation', N'%SYSTEMDRIVE%\SQL2000Databases\[USER_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'ExternalAddress', N'(local)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'InternalAddress', N'(local)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'SaLogin', N'sa')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'SaPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'UseDefaultDatabaseLocation', N'True')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (5, N'UseTrustedConnection', N'True')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (6, N'ExternalAddress', N'localhost')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (6, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 4.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (6, N'InternalAddress', N'localhost,3306')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (6, N'RootLogin', N'root')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (6, N'RootPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'ExpireLimit', N'1209600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'MinimumTTL', N'86400')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'NameServers', N'ns1.yourdomain.com;ns2.yourdomain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'RefreshInterval', N'3600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'ResponsiblePerson', N'hostmaster.[DOMAIN_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (7, N'RetryDelay', N'600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (8, N'AwStatsFolder', N'%SYSTEMDRIVE%\AWStats\wwwroot\cgi-bin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (8, N'BatchFileName', N'UpdateStats.bat')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (8, N'BatchLineTemplate', N'%SYSTEMDRIVE%\perl\bin\perl.exe awstats.pl config=[DOMAIN_NAME] -update')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (8, N'ConfigFileName', N'awstats.[DOMAIN_NAME].conf')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (8, N'ConfigFileTemplate', N'LogFormat = "%time2 %other %other %other %method %url %other %other %logname %host %other %ua %other %referer %other %code %other %other %bytesd %other %other"
LogSeparator = " "
DNSLookup = 2
DirCgi = "/cgi-bin"
DirIcons = "/icon"
AllowFullYearView=3
AllowToUpdateStatsFromBrowser = 0
UseFramesWhenCGI = 1
ShowFlagLinks = "en fr de it nl es"
LogFile = "[LOGS_FOLDER]\ex%YY-3%MM-3%DD-3.log"
DirData = "%SYSTEMDRIVE%\AWStats\data"
SiteDomain = "[DOMAIN_NAME]"
HostAliases = [DOMAIN_ALIASES]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (8, N'StatisticsURL', N'http://127.0.0.1/AWStats/cgi-bin/awstats.pl?config=[domain_name]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'AdminLogin', N'Admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'ExpireLimit', N'1209600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'MinimumTTL', N'86400')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'NameServers', N'ns1.yourdomain.com;ns2.yourdomain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'RefreshInterval', N'3600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'ResponsiblePerson', N'hostmaster.[DOMAIN_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'RetryDelay', N'600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (9, N'SimpleDnsUrl', N'http://127.0.0.1:8053')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'LogDeleteDays', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'LogFormat', N'W3Cex')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'LogWildcard', N'*.log')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'Password', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'ServerID', N'1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'SmarterLogDeleteMonths', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'SmarterLogsPath', N'%SYSTEMDRIVE%\SmarterLogs')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'SmarterUrl', N'http://127.0.0.1:9999/services')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'StatisticsURL', N'http://127.0.0.1:9999/Login.aspx?txtSiteID=[site_id]&txtUser=[username]&txtPass=[password]&shortcutLink=autologin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'TimeZoneId', N'27')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (10, N'Username', N'Admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (11, N'AdminPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (11, N'AdminUsername', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (11, N'DomainsPath', N'%SYSTEMDRIVE%\SmarterMail')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (11, N'ServerIPAddress', N'127.0.0.1;127.0.0.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (11, N'ServiceUrl', N'http://127.0.0.1:9998/services')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (12, N'InstallFolder', N'%PROGRAMFILES%\Gene6 FTP Server')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (14, N'AdminPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (14, N'AdminUsername', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (14, N'DomainsPath', N'%SYSTEMDRIVE%\SmarterMail')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (14, N'ServerIPAddress', N'127.0.0.1;127.0.0.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (14, N'ServiceUrl', N'http://127.0.0.1:9998/services')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'BrowseMethod', N'POST')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'BrowseParameters', N'ServerName=[SERVER]
Login=[USER]
Password=[PASSWORD]
Protocol=dbmssocn')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'BrowseURL', N'http://localhost/MLA/silentlogon.aspx')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'DatabaseLocation', N'%SYSTEMDRIVE%\SQL2005Databases\[USER_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'ExternalAddress', N'(local)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'InternalAddress', N'(local)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'SaLogin', N'sa')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'SaPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'UseDefaultDatabaseLocation', N'True')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (16, N'UseTrustedConnection', N'True')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (17, N'ExternalAddress', N'localhost')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (17, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 5.0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (17, N'InternalAddress', N'localhost,3306')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (17, N'RootLogin', N'root')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (17, N'RootPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (22, N'AdminPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (22, N'AdminUsername', N'Administrator')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'BindConfigPath', N'c:\BIND\dns\etc\named.conf')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'BindReloadBatch', N'c:\BIND\dns\reload.bat')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'ExpireLimit', N'1209600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'MinimumTTL', N'86400')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'NameServers', N'ns1.yourdomain.com;ns2.yourdomain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'RefreshInterval', N'3600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'ResponsiblePerson', N'hostmaster.[DOMAIN_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'RetryDelay', N'600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'ZoneFileNameTemplate', N'db.[domain_name].txt')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (24, N'ZonesFolderPath', N'c:\BIND\dns\zones')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (25, N'DomainId', N'1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (27, N'KeepDeletedItemsDays', N'14')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (27, N'KeepDeletedMailboxesDays', N'30')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (27, N'MailboxDatabase', N'Hosted Exchange Database')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (27, N'RootOU', N'WSP Hosting')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (27, N'StorageGroup', N'Hosted Exchange Storage Group')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (27, N'TempDomain', N'my-temp-domain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'AdminLogin', N'Admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'ExpireLimit', N'1209600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'MinimumTTL', N'86400')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'NameServers', N'ns1.yourdomain.com;ns2.yourdomain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'RefreshInterval', N'3600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'ResponsiblePerson', N'hostmaster.[DOMAIN_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'RetryDelay', N'600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (28, N'SimpleDnsUrl', N'http://127.0.0.1:8053')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (29, N'AdminPassword', N' ')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (29, N'AdminUsername', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (29, N'DomainsPath', N'%SYSTEMDRIVE%\SmarterMail')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (29, N'ServerIPAddress', N'127.0.0.1;127.0.0.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (29, N'ServiceUrl', N'http://localhost:9998/services/')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (30, N'ExternalAddress', N'localhost')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (30, N'InstallFolder', N'%PROGRAMFILES%\MySQL\MySQL Server 5.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (30, N'InternalAddress', N'localhost,3306')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (30, N'RootLogin', N'root')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (30, N'RootPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'LogDeleteDays', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'LogFormat', N'W3Cex')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'LogWildcard', N'*.log')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'Password', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'ServerID', N'1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'SmarterLogDeleteMonths', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'SmarterLogsPath', N'%SYSTEMDRIVE%\SmarterLogs')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'SmarterUrl', N'http://127.0.0.1:9999/services')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'StatisticsURL', N'http://127.0.0.1:9999/Login.aspx?txtSiteID=[site_id]&txtUser=[username]&txtPass=[password]&shortcutLink=autologin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'TimeZoneId', N'27')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (31, N'Username', N'Admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (32, N'KeepDeletedItemsDays', N'14')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (32, N'KeepDeletedMailboxesDays', N'30')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (32, N'MailboxDatabase', N'Hosted Exchange Database')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (32, N'RootOU', N'WSP Hosting')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (32, N'TempDomain', N'my-temp-domain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'ExpireLimit', N'1209600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'MinimumTTL', N'86400')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'NameServers', N'ns1.yourdomain.com;ns2.yourdomain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'PDNSDbName', N'pdnsdb')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'PDNSDbPort', N'3306')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'PDNSDbServer', N'localhost')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'PDNSDbUser', N'root')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'RefreshInterval', N'3600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'ResponsiblePerson', N'hostmaster.[DOMAIN_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (56, N'RetryDelay', N'600')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (60, N'AdminPassword', N' ')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (60, N'AdminUsername', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (60, N'DomainsPath', N'%SYSTEMDRIVE%\SmarterMail')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (60, N'ServerIPAddress', N'127.0.0.1;127.0.0.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (60, N'ServiceUrl', N'http://localhost:9998/services/')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'LogDeleteDays', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'LogFormat', N'W3Cex')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'LogWildcard', N'*.log')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'Password', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'ServerID', N'1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'SmarterLogDeleteMonths', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'SmarterLogsPath', N'%SYSTEMDRIVE%\SmarterLogs')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'SmarterUrl', N'http://127.0.0.1:9999/services')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'StatisticsURL', N'http://127.0.0.1:9999/Login.aspx?txtSiteID=[site_id]&txtUser=[username]&txtPass=[password]&shortcutLink=autologin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'TimeZoneId', N'27')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (62, N'Username', N'Admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (64, N'AdminPassword', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (64, N'AdminUsername', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (64, N'DomainsPath', N'%SYSTEMDRIVE%\SmarterMail')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (64, N'ServerIPAddress', N'127.0.0.1;127.0.0.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (64, N'ServiceUrl', N'http://localhost:9998/services/')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (100, N'UsersHome', N'%SYSTEMDRIVE%\HostingSpaces')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'AspNet11Pool', N'ASP.NET 1.1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'AspNet40Path', N'%WINDIR%\Microsoft.NET\Framework\v4.0.30319\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'AspNet40x64Path', N'%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\aspnet_isapi.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'AspNetBitnessMode', N'32')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'CFFlashRemotingDirectory', N'C:\ColdFusion9\runtime\lib\wsconfig\1')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'CFScriptsDirectory', N'C:\Inetpub\wwwroot\CFIDE')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'ClassicAspNet20Pool', N'ASP.NET 2.0 (Classic)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'ClassicAspNet40Pool', N'ASP.NET 4.0 (Classic)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'ColdFusionPath', N'C:\ColdFusion9\runtime\lib\wsconfig\jrun_iis6.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'GalleryXmlFeedUrl', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'IntegratedAspNet20Pool', N'ASP.NET 2.0 (Integrated)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'IntegratedAspNet40Pool', N'ASP.NET 4.0 (Integrated)')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'PerlPath', N'%SYSTEMDRIVE%\Perl\bin\PerlEx30.dll')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'Php4Path', N'%PROGRAMFILES%\PHP\php.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'PhpMode', N'FastCGI')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'PhpPath', N'%PROGRAMFILES%\PHP\php-cgi.exe')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'ProtectedGroupsFile', N'.htgroup')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'ProtectedUsersFile', N'.htpasswd')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'SecureFoldersModuleAssembly', N'WebsitePanel.IIsModules.SecureFolders, WebsitePanel.IIsModules, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=37f9c58a0aa32ff0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'WebGroupName', N'WSP_IUSRS')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'WmSvc.CredentialsMode', N'WINDOWS')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (101, N'WmSvc.Port', N'8172')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (102, N'FtpGroupName', N'WSPFtpUsers')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (102, N'SiteId', N'Default FTP Site')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (200, N'RootWebApplicationIpAddress', N'')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (204, N'UserName', N'admin')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (204, N'UtilityPath', N'C:\Program Files\Research In Motion\BlackBerry Enterprise Server Resource Kit\BlackBerry Enterprise Server User Administration Tool')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (207, N'ecpURL', N'http://ecp.[DOMAIN_NAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (207, N'location', N'en-us')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'CpuLimit', N'100')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'CpuReserve', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'CpuWeight', N'100')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'DvdLibraryPath', N'C:\Hyper-V\Library')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'ExportedVpsPath', N'C:\Hyper-V\Exported')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'HostnamePattern', N'vps[user_id].hosterdomain.com')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'OsTemplatesPath', N'C:\Hyper-V\Templates')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'PrivateNetworkFormat', N'192.168.0.1/16')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'RootFolder', N'C:\Hyper-V\VirtualMachines\[VPS_HOSTNAME]')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'StartAction', N'start')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'StartupDelay', N'0')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'StopAction', N'shutDown')
GO
INSERT [dbo].[ServiceDefaultProperties] ([ProviderID], [PropertyName], [PropertyValue]) VALUES (300, N'VirtualDiskType', N'dynamic')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecDeleteBillingCycle]
	@ActorID int,
	@UserID int,
	@CycleID int,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

    DELETE FROM [dbo].[ecBillingCycles] WHERE [ResellerID] = @UserID AND [CycleID] = @CycleID;

	SET @Result = 0;
	RETURN;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecUpdateCustomerPayment]
	@ActorID int,
	@PaymentID int,
	@InvoiceID int,
	@TransactionID nvarchar(255),
	@Total money,
	@Currency nvarchar(3),
	@MethodName nvarchar(50),
	@PluginID int,
	@StatusID int,
	@Result int OUTPUT
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecCustomersPayments]
	WHERE
		[PaymentID] = @PaymentID;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET @Result = 0;

    UPDATE [dbo].[ecCustomersPayments]
	SET
		[InvoiceID] = @InvoiceID,
		[TransactionID] = @TransactionID,
		[Total] = @Total,
		[Currency] = @Currency,
		[MethodName] = @MethodName,
		[PluginID] = @PluginID,
		[StatusID] = @StatusID
	WHERE
		[PaymentID] = @PaymentID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecProductsHighlights](
	[ProductID] [int] NOT NULL,
	[HighlightText] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[SortOrder] [int] NOT NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecProductCategories](
	[ProductID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[ResellerID] [int] NOT NULL,
 CONSTRAINT [PK_ecProductCategories] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC,
	[CategoryID] ASC,
	[ResellerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IPAddresses](
	[AddressID] [int] IDENTITY(1,1) NOT NULL,
	[ExternalIP] [varchar](24) COLLATE Latin1_General_CI_AS NOT NULL,
	[InternalIP] [varchar](24) COLLATE Latin1_General_CI_AS NULL,
	[ServerID] [int] NULL,
	[Comments] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[SubnetMask] [varchar](15) COLLATE Latin1_General_CI_AS NULL,
	[DefaultGateway] [varchar](15) COLLATE Latin1_General_CI_AS NULL,
	[PoolID] [int] NULL,
 CONSTRAINT [PK_IPAddresses] PRIMARY KEY CLUSTERED 
(
	[AddressID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[SetItemPrimaryIPAddress]
(
	@ActorID int,
	@ItemID int,
	@PackageAddressID int
)
AS
BEGIN

	-- read item pool
	DECLARE @PoolID int
	SELECT @PoolID = IP.PoolID FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.PackageAddressID = @PackageAddressID

	-- update all IP addresses of the specified pool
	UPDATE PackageIPAddresses
	SET IsPrimary = CASE PIP.PackageAddressID WHEN @PackageAddressID THEN 1 ELSE 0 END
	FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.ItemID = @ItemID
	AND IP.PoolID = @PoolID
	AND dbo.CheckActorPackageRights(@ActorID, PIP.PackageID) = 1
END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[GetPackageUnassignedIPAddresses]
(
	@ActorID int,
	@PackageID int,
	@PoolID int = 0
)
AS
BEGIN
	SELECT
		PIP.PackageAddressID,
		IP.AddressID,
		IP.ExternalIP,
		IP.InternalIP,
		IP.ServerID,
		IP.PoolID,
		PIP.IsPrimary,
		IP.SubnetMask,
		IP.DefaultGateway
	FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE
		PIP.ItemID IS NULL
		AND PIP.PackageID = @PackageID
		AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
		AND dbo.CheckActorPackageRights(@ActorID, PIP.PackageID) = 1
	ORDER BY IP.DefaultGateway, IP.ExternalIP
END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[GetPackageIPAddress]
	@PackageAddressID int
AS
BEGIN


SELECT
	PA.PackageAddressID,
	PA.AddressID,
	IP.ExternalIP,
	IP.InternalIP,
	IP.SubnetMask,
	IP.DefaultGateway,
	PA.ItemID,
	SI.ItemName,
	PA.PackageID,
	P.PackageName,
	P.UserID,
	U.UserName,
	PA.IsPrimary
FROM dbo.PackageIPAddresses AS PA
INNER JOIN dbo.IPAddresses AS IP ON PA.AddressID = IP.AddressID
INNER JOIN dbo.Packages P ON PA.PackageID = P.PackageID
INNER JOIN dbo.Users U ON U.UserID = P.UserID
LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
WHERE PA.PackageAddressID = @PackageAddressID

END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO











CREATE PROCEDURE [dbo].[GetItemIPAddresses]
(
	@ActorID int,
	@ItemID int,
	@PoolID int
)
AS

SELECT
	PIP.PackageAddressID AS AddressID,
	IP.ExternalIP AS IPAddress,
	IP.InternalIP AS NATAddress,
	IP.SubnetMask,
	IP.DefaultGateway,
	PIP.IsPrimary
FROM PackageIPAddresses AS PIP
INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
INNER JOIN ServiceItems AS SI ON PIP.ItemID = SI.ItemID
WHERE PIP.ItemID = @ItemID
AND dbo.CheckActorPackageRights(@ActorID, SI.PackageID) = 1
AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
ORDER BY PIP.IsPrimary DESC

RETURN














GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[GetIPAddresses]	
(
	@ActorID int,
	@PoolID int,
	@ServerID int
)
AS
BEGIN

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	IP.AddressID,
	IP.PoolID,
	IP.ExternalIP,
	IP.InternalIP,
	IP.SubnetMask,
	IP.DefaultGateway,
	IP.Comments,

	IP.ServerID,
	S.ServerName,

	PA.ItemID,
	SI.ItemName,

	PA.PackageID,
	P.PackageName,

	P.UserID,
	U.UserName
FROM dbo.IPAddresses AS IP
LEFT JOIN Servers AS S ON IP.ServerID = S.ServerID
LEFT JOIN PackageIPAddresses AS PA ON IP.AddressID = PA.AddressID
LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
LEFT JOIN dbo.Packages P ON PA.PackageID = P.PackageID
LEFT JOIN dbo.Users U ON U.UserID = P.UserID
WHERE @IsAdmin = 1
AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)

AND (@ServerID = 0 OR @ServerID <> 0 AND IP.ServerID = @ServerID)

END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetDnsRecordsTotal
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- create temp table for DNS records
DECLARE @Records TABLE
(
	RecordID int,
	RecordType nvarchar(10) COLLATE DATABASE_DEFAULT,
	RecordName nvarchar(50) COLLATE DATABASE_DEFAULT
)

-- select PACKAGES DNS records
DECLARE @ParentPackageID int, @TmpPackageID int
SET @TmpPackageID = @PackageID

WHILE 10 = 10
BEGIN

	-- get DNS records for the current package
	INSERT INTO @Records (RecordID, RecordType, RecordName)
	SELECT
		GR.RecordID,
		GR.RecordType,
		GR.RecordName
	FROM GlobalDNSRecords AS GR
	WHERE GR.PackageID = @TmpPackageID
	AND GR.RecordType + GR.RecordName NOT IN (SELECT RecordType + RecordName FROM @Records)

	SET @ParentPackageID = NULL

	-- get parent package
	SELECT
		@ParentPackageID = ParentPackageID
	FROM Packages
	WHERE PackageID = @TmpPackageID
	
	IF @ParentPackageID IS NULL -- the last parent
	BREAK
	
	SET @TmpPackageID = @ParentPackageID
END

-- select SERVER DNS records
DECLARE @ServerID int
SELECT @ServerID = ServerID FROM Packages
WHERE PackageID = @PackageID

INSERT INTO @Records (RecordID, RecordType, RecordName)
SELECT
	GR.RecordID,
	GR.RecordType,
	GR.RecordName
FROM GlobalDNSRecords AS GR
WHERE GR.ServerID = @ServerID
AND GR.RecordType + GR.RecordName NOT IN (SELECT RecordType + RecordName FROM @Records)


-- select SERVICES DNS records
-- re-distribute package services
EXEC DistributePackageServices @ActorID, @PackageID

INSERT INTO @Records (RecordID, RecordType, RecordName)
SELECT
	GR.RecordID,
	GR.RecordType,
	GR.RecordName
FROM GlobalDNSRecords AS GR
WHERE GR.ServiceID IN (SELECT ServiceID FROM PackageServices WHERE PackageID = @PackageID)
AND GR.RecordType + GR.RecordName NOT IN (SELECT RecordType + RecordName FROM @Records)


SELECT
	NR.RecordID,
	NR.ServiceID,
	NR.ServerID,
	NR.PackageID,
	NR.RecordType,
	NR.RecordName,
	NR.RecordData,
	NR.MXPriority,
	NR.IPAddressID,
	ISNULL(IP.ExternalIP, '') AS ExternalIP,
	ISNULL(IP.InternalIP, '') AS InternalIP,
	CASE
		WHEN NR.RecordType = 'A' AND NR.RecordData = '' THEN dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP)
		WHEN NR.RecordType = 'MX' THEN CONVERT(varchar(3), NR.MXPriority) + ', ' + NR.RecordData
		ELSE NR.RecordData
	END AS FullRecordData,
	dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP) AS IPAddress
FROM @Records AS TR
INNER JOIN GlobalDnsRecords AS NR ON TR.RecordID = NR.RecordID
LEFT OUTER JOIN IPAddresses AS IP ON NR.IPAddressID = IP.AddressID

RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetDnsRecordsByService
(
	@ActorID int,
	@ServiceID int
)
AS

SELECT
	NR.RecordID,
	NR.ServiceID,
	NR.ServerID,
	NR.PackageID,
	NR.RecordType,
	NR.RecordName,
	CASE
		WHEN NR.RecordType = 'A' AND NR.RecordData = '' THEN dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP)
		WHEN NR.RecordType = 'MX' THEN CONVERT(varchar(3), NR.MXPriority) + ', ' + NR.RecordData
		ELSE NR.RecordData
	END AS FullRecordData,
	NR.RecordData,
	NR.MXPriority,
	NR.IPAddressID,
	dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP) AS IPAddress,
	IP.ExternalIP,
	IP.InternalIP
FROM
	GlobalDnsRecords AS NR
LEFT OUTER JOIN IPAddresses AS IP ON NR.IPAddressID = IP.AddressID
WHERE
	NR.ServiceID = @ServiceID
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetDnsRecordsByServer
(
	@ActorID int,
	@ServerID int
)
AS

SELECT
	NR.RecordID,
	NR.ServiceID,
	NR.ServerID,
	NR.PackageID,
	NR.RecordType,
	NR.RecordName,
	NR.RecordData,
	CASE
		WHEN NR.RecordType = 'A' AND NR.RecordData = '' THEN dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP)
		WHEN NR.RecordType = 'MX' THEN CONVERT(varchar(3), NR.MXPriority) + ', ' + NR.RecordData
		ELSE NR.RecordData
	END AS FullRecordData,
	NR.MXPriority,
	NR.IPAddressID,
	dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP) AS IPAddress,
	IP.ExternalIP,
	IP.InternalIP
FROM
	GlobalDnsRecords AS NR
LEFT OUTER JOIN IPAddresses AS IP ON NR.IPAddressID = IP.AddressID
WHERE
	NR.ServerID = @ServerID
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetDnsRecordsByPackage
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

SELECT
	NR.RecordID,
	NR.ServiceID,
	NR.ServerID,
	NR.PackageID,
	NR.RecordType,
	NR.RecordName,
	NR.RecordData,
	NR.MXPriority,
	NR.IPAddressID,
	CASE
		WHEN NR.RecordType = 'A' AND NR.RecordData = '' THEN dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP)
		WHEN NR.RecordType = 'MX' THEN CONVERT(varchar(3), NR.MXPriority) + ', ' + NR.RecordData
		ELSE NR.RecordData
	END AS FullRecordData,
	dbo.GetFullIPAddress(IP.ExternalIP, IP.InternalIP) AS IPAddress,
	IP.ExternalIP,
	IP.InternalIP
FROM
	GlobalDnsRecords AS NR
LEFT OUTER JOIN IPAddresses AS IP ON NR.IPAddressID = IP.AddressID
WHERE NR.PackageID = @PackageID
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































/*
Algorythm:
	0. Get the primary distribution resource from hosting plan
	1. Check whether user has Resource of requested type in his user plans/add-ons
		EXCEPTION "The requested service is not available for the user. The resource of the requested type {type} should be assigned to him through hosting plan or add-on"
		1.1 If the number of returned reources is greater than 1
			EXCEPTION "User has several resources assigned of the requested type"

	2. If the requested resource has 0 services
		EXCEPTION "The resource {name} of type {type} should contain atleast one service
	3. If the requested resource has one service
		remember the ID of this single service
	4. If the requested resource has several services DO distribution:
		
		4.1. If the resource is NOT BOUNDED or is PRIMARY DISTRIBUTION RESOURCE
			if PRIMARY DISTRIBUTION RESOURCE and exists in UserServices
				return serviceId from UserServices table
				
			remember any service from that resource according to distribution type ("BALANCED" or "RANDOM") - get the number of ServiceItems for each service
			
		4.2. If the resource is BOUNDED to primary distribution resource
			- If the primary distribution resource is NULL
			EXCEPTION "Requested resource marked as bound to primary distribution resource, but there is no any resources in hosting plan marked as primary"
			
			- Get the service id of the primary distribution resource
			GetServiceId(userId, primaryResourceId)
			
		
		Get from user assigned hosting plan 
		
	5. If it is PRIMARY DISTRIBUTION RESOURCE
		Save it's ID to UserServices table
	
	6. return serviceId
	
ERROR CODES:
	-1 - there are several hosting plans with PDR assigned to that user
	-2 - The requested service is not available for the user. The resource of the
		requested type {type} should be assigned to him through hosting plan or add-on
	-3 - several resources of the same type was assigned through hosting plan or add-on
	-4 - The resource {name} of type {type} should contain atleast one service
	-5 - Requested resource marked as bound to primary distribution resource,
		but there is no any resources in hosting plan marked as primary
	-6 - the server where PDR is located doesn't contain the service of requested resource type
*/
CREATE PROCEDURE GetUserServiceID
(
	@UserID int,
	@TypeName nvarchar(1000),
	@ServiceID int OUTPUT
)
AS
	DECLARE @PrimaryResourceID int -- primary distribution resource assigned through hosting plan
	
	----------------------------------------
	-- Get the primary distribution resource
	----------------------------------------
	IF (SELECT COUNT (HP.PrimaryResourceID) FROM PurchasedHostingPlans AS PHP
	INNER JOIN HostingPlans AS HP ON PHP.PlanID = HP.PlanID
	WHERE PHP.UserID = @UserID AND HP.PrimaryResourceID IS NOT NULL AND HP.PrimaryResourceID <> 0) > 1
	BEGIN
		SET @ServiceID = -1
		RETURN
	END
		
	SELECT @PrimaryResourceID = HP.PrimaryResourceID FROM PurchasedHostingPlans AS PHP
	INNER JOIN HostingPlans AS HP ON PHP.PlanID = HP.PlanID
	WHERE PHP.UserID = @UserID AND HP.PrimaryResourceID IS NOT NULL AND HP.PrimaryResourceID <> 0

	
	----------------------------------------------
	-- Check whether user has a resource 
	-- of this type in his hosting plans or addons
	----------------------------------------------
	DECLARE @UserResourcesTable TABLE
	(
		ResourceID int
	)
	INSERT INTO @UserResourcesTable
	SELECT DISTINCT HPR.ResourceID FROM PurchasedHostingPlans AS PHP
		INNER JOIN HostingPlans AS HP ON PHP.PlanID = HP.PlanID
		INNER JOIN HostingPlanResources AS HPR ON HP.PlanID = HPR.PlanID
		INNER JOIN Resources AS R ON HPR.ResourceID = R.ResourceID
		INNER JOIN ServiceTypes AS ST ON R.ServiceTypeID = ST.ServiceTypeID
		WHERE PHP.UserID = @UserID AND (ST.ImplementedTypeNames LIKE @TypeName OR ST.TypeName LIKE @TypeName)

	----------------------------------------
	-- Check resources number
	----------------------------------------
	DECLARE @ResourcesCount int
	SET @ResourcesCount = @@ROWCOUNT
	IF @ResourcesCount = 0
	BEGIN
		SET @ServiceID = -2 -- user doesn't have requested service assigned
		RETURN
	END
	IF @ResourcesCount > 1
	BEGIN
		SET @ServiceID = -3 -- several resources of the same type was assigned
		RETURN
	END
	
	----------------------------------------
	-- Check services number
	----------------------------------------
	DECLARE @ResourceID int
	SET @ResourceID = (SELECT TOP 1 ResourceID FROM @UserResourcesTable)
	
	DECLARE @UserServicesTable TABLE
	(
		ServiceID int,
		ServerID int,
		ItemsNumber int,
		Randomizer float
	)
	INSERT INTO @UserServicesTable
	SELECT
		RS.ServiceID,
		S.ServerID,
		(SELECT COUNT(ItemID) FROM ServiceItems AS SI WHERE SI.ServiceID = RS.ServiceID),
		RAND()
	FROM ResourceServices AS RS
	INNER JOIN Services AS S ON RS.ServiceID = S.ServiceID
	WHERE RS.ResourceID = @ResourceID
	
	DECLARE @ServicesCount int
	SET @ServicesCount = @@ROWCOUNT
	IF @ServicesCount = 0
	BEGIN
		SET @ServiceID = -4 -- The resource {name} of type {type} should contain atleast one service
		RETURN
	END
	
	-- try to return from UserServices
	-- if it is a PDR
	IF @ResourceID = @PrimaryResourceID
	BEGIN
		-- check in UserServices table
		SELECT @ServiceID = US.ServiceID FROM ResourceServices AS RS
		INNER JOIN UserServices AS US ON RS.ServiceID = US.ServiceID
		WHERE RS.ResourceID = @ResourceID AND US.UserID = @UserID
		
		-- check validness of the current primary service id
		IF @ServiceID IS NOT NULL
		BEGIN
			IF EXISTS(SELECT ResourceServiceID FROM ResourceServices
			WHERE ResourceID = @ResourceID AND ServiceID = @ServiceID)
				RETURN
			ELSE -- invalidate service
				DELETE FROM UserServices WHERE UserID = @UserID
		END
	END
	
	IF @ServicesCount = 1
	BEGIN
		-- nothing to distribute
		-- just remember this single service id
		SET @ServiceID = (SELECT TOP 1 ServiceID FROM @UserServicesTable)
	END
	ELSE
	BEGIN
		-- the service should be distributed
		DECLARE @DistributionTypeID int
		DECLARE @BoundToPrimaryResource bit
		SELECT @DistributionTypeID = R.DistributionTypeID, @BoundToPrimaryResource = R.BoundToPrimaryResource
		FROM Resources AS R WHERE R.ResourceID = @ResourceID
	
		IF @BoundToPrimaryResource = 0 OR @ResourceID = @PrimaryResourceID
		BEGIN
			IF @ResourceID = @PrimaryResourceID -- it's PDR itself
			BEGIN
				-- check in UserServices table
				SELECT @ServiceID = US.ServiceID FROM ResourceServices AS RS
				INNER JOIN UserServices AS US ON RS.ServiceID = US.ServiceID
				WHERE RS.ResourceID = @ResourceID AND US.UserID = @UserID
				
				-- check validness of the current primary service id
				IF @ServiceID IS NOT NULL
				BEGIN
					IF EXISTS(SELECT ResourceServiceID FROM ResourceServices
					WHERE ResourceID = @ResourceID AND ServiceID = @ServiceID)
						RETURN
					ELSE -- invalidate service
						DELETE FROM UserServices WHERE UserID = @UserID
				END
			END
			
			-- distribute
			IF @DistributionTypeID = 1 -- BALANCED distribution
				SELECT @ServiceID = ServiceID FROM @UserServicesTable
				ORDER BY ItemsNumber ASC
			ELSE -- RANDOM distribution
				SELECT @ServiceID = ServiceID FROM @UserServicesTable
				ORDER BY Randomizer
		END
		ELSE -- BOUND to PDR resource
		BEGIN
			IF @PrimaryResourceID IS NULL
			BEGIN
				SET @ServiceID = -5 -- Requested resource marked as bound to primary distribution resource,
									-- but there is no any resources in hosting plan marked as primary
				RETURN
			END
			
			-- get the type of primary resource
			DECLARE @PrimaryTypeName nvarchar(200)
			SELECT @PrimaryTypeName = ST.TypeName FROM  Resources AS R
			INNER JOIN ServiceTypes AS ST ON R.ServiceTypeID = ST.ServiceTypeID
			WHERE R.ResourceID = @PrimaryResourceID
			
			
			DECLARE @PrimaryServiceID int
			EXEC GetUserServiceID @UserID, @PrimaryTypeName, @PrimaryServiceID OUTPUT

			IF @PrimaryServiceID < 0
			BEGIN
				SET @ServiceID = @PrimaryServiceID
				RETURN
			END
			
			DECLARE @ServerID int
			SET @ServerID = (SELECT ServerID FROM Services WHERE ServiceID = @PrimaryServiceID)
			
			-- try to get the service of the requested type on PDR server
			SET @ServiceID = (SELECT ServiceID FROM @UserServicesTable WHERE ServerID = @ServerID)
			
			IF @ServiceID IS NULL
			BEGIN
				SET @ServiceID = -6 -- the server where PDR is located doesn't contain the service of requested resource type
			END
		END
	END
	
	IF @ResourceID = @PrimaryResourceID -- it's PDR
	BEGIN
		DELETE FROM UserServices WHERE UserID = @UserID
		
		INSERT INTO UserServices (UserID, ServiceID)
		VALUES (@UserID, @ServiceID)
	END
	
RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


































CREATE PROCEDURE [dbo].[AddUser]
(
	@ActorID int,
	@UserID int OUTPUT,
	@OwnerID int,
	@RoleID int,
	@StatusID int,
	@IsDemo bit,
	@IsPeer bit,
	@Comments ntext,
	@Username nvarchar(50),
	@Password nvarchar(200),
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(255),
	@SecondaryEmail nvarchar(255),
	@Address nvarchar(200),
	@City nvarchar(50),
	@State nvarchar(50),
	@Country nvarchar(50),
	@Zip varchar(20),
	@PrimaryPhone varchar(30),
	@SecondaryPhone varchar(30),
	@Fax varchar(30),
	@InstantMessenger nvarchar(200),
	@HtmlMail bit,
	@CompanyName nvarchar(100),
	@EcommerceEnabled bit
)
AS

-- check if the user already exists
IF EXISTS(SELECT UserID FROM Users WHERE Username = @Username)
BEGIN
	SET @UserID = -1
	RETURN
END

-- check actor rights
IF dbo.CanCreateUser(@ActorID, @OwnerID) = 0
BEGIN
	SET @UserID = -2
	RETURN
END

INSERT INTO Users
(
	OwnerID,
	RoleID,
	StatusID,
	Created,
	Changed,
	IsDemo,
	IsPeer,
	Comments,
	Username,
	Password,
	FirstName,
	LastName,
	Email,
	SecondaryEmail,
	Address,
	City,
	State,
	Country,
	Zip,
	PrimaryPhone,
	SecondaryPhone,
	Fax,
	InstantMessenger,
	HtmlMail,
	CompanyName,
	EcommerceEnabled
)
VALUES
(
	@OwnerID,
	@RoleID,
	@StatusID,
	GetDate(),
	GetDate(),
	@IsDemo,
	@IsPeer,
	@Comments,
	@Username,
	@Password,
	@FirstName,
	@LastName,
	@Email,
	@SecondaryEmail,
	@Address,
	@City,
	@State,
	@Country,
	@Zip,
	@PrimaryPhone,
	@SecondaryPhone,
	@Fax,
	@InstantMessenger,
	@HtmlMail,
	@CompanyName,
	@EcommerceEnabled
)

SET @UserID = SCOPE_IDENTITY()

RETURN 








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE UpdateUserSettings
(
	@ActorID int,
	@UserID int,
	@SettingsName nvarchar(50),
	@Xml ntext
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

-- delete old properties
BEGIN TRAN
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM UserSettings
WHERE UserID = @UserID AND SettingsName = @SettingsName

INSERT INTO UserSettings
(
	UserID,
	SettingsName,
	PropertyName,
	PropertyValue
)
SELECT
	@UserID,
	@SettingsName,
	PropertyName,
	PropertyValue
FROM OPENXML(@idoc, '/properties/property',1) WITH 
(
	PropertyName nvarchar(50) '@name',
	PropertyValue ntext '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


































CREATE PROCEDURE [dbo].[UpdateUser]
(
	@ActorID int,
	@UserID int,
	@RoleID int,
	@StatusID int,
	@IsDemo bit,
	@IsPeer bit,
	@Comments ntext,
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(255),
	@SecondaryEmail nvarchar(255),
	@Address nvarchar(200),
	@City nvarchar(50),
	@State nvarchar(50),
	@Country nvarchar(50),
	@Zip varchar(20),
	@PrimaryPhone varchar(30),
	@SecondaryPhone varchar(30),
	@Fax varchar(30),
	@InstantMessenger nvarchar(200),
	@HtmlMail bit,
	@CompanyName nvarchar(100),
	@EcommerceEnabled bit
)
AS

-- check actor rights
IF dbo.CanUpdateUserDetails(@ActorID, @UserID) = 0
RETURN

UPDATE Users SET 
	RoleID = @RoleID,
	StatusID = @StatusID,
	Changed = GetDate(),
	IsDemo = @IsDemo,
	IsPeer = @IsPeer,
	Comments = @Comments,
	FirstName = @FirstName,
	LastName = @LastName,
	Email = @Email,
	SecondaryEmail = @SecondaryEmail,
	Address = @Address,
	City = @City,
	State = @State,
	Country = @Country,
	Zip = @Zip,
	PrimaryPhone = @PrimaryPhone,
	SecondaryPhone = @SecondaryPhone,
	Fax = @Fax,
	InstantMessenger = @InstantMessenger,
	HtmlMail = @HtmlMail,
	CompanyName = @CompanyName,
	EcommerceEnabled = @EcommerceEnabled
WHERE UserID = @UserID

RETURN 






































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteComment
(
	@ActorID int,
	@CommentID int
)
AS

-- check rights
DECLARE @UserID int
SELECT @UserID = UserID FROM Comments
WHERE CommentID = @CommentID

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to perform this operation', 16, 1)


-- delete comment
DELETE FROM Comments
WHERE CommentID = @CommentID

RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



















CREATE VIEW [dbo].[ServiceHandlersResponsesDetailed]
AS
SELECT     dbo.ecServiceHandlersResponses.ResponseID, dbo.ecServiceHandlersResponses.ServiceID, dbo.ecContracts.ResellerID, 
                      dbo.ecServiceHandlersResponses.ContractID, dbo.ecServiceHandlersResponses.TextResponse, dbo.ecServiceHandlersResponses.Received, 
                      dbo.ecServiceHandlersResponses.ErrorMessage, dbo.ecPaymentMethods.MethodName, dbo.ecServiceHandlersResponses.InvoiceID
FROM         dbo.ecContracts RIGHT OUTER JOIN
                      dbo.ecPaymentMethods INNER JOIN
                      dbo.ecSupportedPlugins ON dbo.ecPaymentMethods.PluginID = dbo.ecSupportedPlugins.PluginID RIGHT OUTER JOIN
                      dbo.ecServiceHandlersResponses ON dbo.ecSupportedPlugins.UniqueID = dbo.ecServiceHandlersResponses.ServiceID ON 
                      dbo.ecContracts.ContractID = dbo.ecServiceHandlersResponses.ContractID
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecAddonProducts](
	[AddonID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[ResellerID] [int] NOT NULL,
 CONSTRAINT [PK_ecAddonProducts] PRIMARY KEY CLUSTERED 
(
	[AddonID] ASC,
	[ProductID] ASC,
	[ResellerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



















CREATE PROCEDURE [dbo].[ecAddInvoice] 
	@ContractID nvarchar(50),
	@Created datetime,
	@DueDate datetime,
	@TaxationID int,
	@TotalAmount money,
	@SubTotalAmount money,
	@TaxAmount money,
	@Xml ntext,
	@Currency nvarchar(3),
	@Result int OUTPUT
AS
BEGIN
/*
	XML Format:
	<items>
		<item serviceid="" itemname="" typename="" quantity="" total="" subtotal="" unitprice="" />
	</items>
*/

BEGIN TRAN ADD_INVOICE
		DECLARE @XmlDocID int;
		SET @XmlDocID = NULL;
		--
		IF @TaxationID < 1
			SET @TaxationID = NULL;
		-- emit invoice
		INSERT INTO [dbo].[ecInvoice]
			([ContractID], [Created], [DueDate], [TaxationID], [Total], [SubTotal], [TaxAmount], [Currency])
		VALUES
			(@ContractID, @Created, @DueDate, @TaxationID, @TotalAmount, @SubTotalAmount, @TaxAmount, @Currency);
		-- obtain result
		SET @Result = SCOPE_IDENTITY();

		--Create an internal representation of the XML document.
		EXEC sp_xml_preparedocument @XmlDocID OUTPUT, @Xml;;
		-- 
		INSERT INTO [dbo].[ecInvoiceItems]
		(
			[InvoiceID],
			[ServiceID],
			[ItemName],
			[TypeName],
			[Quantity],
			[Total],
			[SubTotal],
			[UnitPrice]
		)
		SELECT
			@Result,
			CASE [XML].[ServiceID] 
				WHEN 0 THEN NULL
				ELSE [XML].[ServiceID]
			END,
			[XML].[ItemName],
			[XML].[TypeName],
			[XML].[Quantity],
			[XML].[Total],
			[XML].[SubTotal],
			[XML].[UnitPrice]
		FROM OPENXML(@XmlDocID, '/items/item',1) WITH 
		(
			[ServiceID] int '@serviceid',
			[ItemName] nvarchar(255) '@itemname',
			[TypeName] nvarchar(255) '@typename',
			[Quantity] int '@quantity',
			[Total] money '@total',
			[SubTotal] money '@subtotal',
			[UnitPrice] money '@unitprice'
		) AS [XML];
		-- check errors
		IF @@ERROR <> 0
			GOTO ERROR_HANDLE;
		-- remove document
		EXEC sp_xml_removedocument @XmlDocID;

	-- commit
	COMMIT TRAN ADD_INVOICE;
	-- exit
	RETURN;
-- error handle
ERROR_HANDLE:
BEGIN
	IF NOT @XmlDocID IS NULL
		EXEC sp_xml_removedocument @XmlDocID;

	SET @Result = -1;
	ROLLBACK TRAN ADD_INVOICE;
	RETURN;
END

END






















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteAuditLogRecords
(
	@ActorID int,
	@UserID int,
	@ItemID int,
	@ItemName nvarchar(100),
	@StartDate datetime,
	@EndDate datetime,
	@SeverityID int,
	@SourceName varchar(100),
	@TaskName varchar(100)
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

DECLARE @IsAdmin bit
SET @IsAdmin = 0
IF EXISTS(SELECT UserID FROM Users WHERE UserID = @ActorID AND RoleID = 1)
SET @IsAdmin = 1

DELETE FROM AuditLog
WHERE (dbo.CheckUserParent(@UserID, UserID) = 1 OR (UserID IS NULL AND @IsAdmin = 1))
AND StartDate BETWEEN @StartDate AND @EndDate
AND ((@SourceName = '') OR (@SourceName <> '' AND SourceName = @SourceName))
AND ((@TaskName = '') OR (@TaskName <> '' AND TaskName = @TaskName))
AND ((@ItemID = 0) OR (@ItemID > 0 AND ItemID = @ItemID))
AND ((@ItemName = '') OR (@ItemName <> '' AND ItemName LIKE @ItemName))

RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecUpdateInvoice] 
	@ActorID int,
	@InvoiceID int,
	@InvoiceNumber nvarchar(50),
	@DueDate datetime,
	@TaxationID int,
	@Total money,
	@SubTotal money,
	@TaxAmount money,
	@Currency nvarchar(3),
	@Result int OUTPUT
AS
BEGIN
	-- ensure an update request has been issued by the right person
	DECLARE @ContractID nvarchar(50), @IssuerID int;
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecInvoice]
	WHERE
		[InvoiceID] = @InvoiceID;
	SELECT
		@IssuerID = [ResellerID] FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

	SET NOCOUNT ON;

	SET @Result = 0;

    UPDATE
		[dbo].[ecInvoice]
	SET
		[InvoiceNumber] = @InvoiceNumber,
		[DueDate] = @DueDate,
		[Total] = @Total,
		[SubTotal] = @SubTotal,
		[TaxationID] = @TaxationID,
		[TaxAmount] = @TaxAmount,
		[Currency] = @Currency
	WHERE
		[InvoiceID] = @InvoiceID
	AND
		[ContractID] = @ContractID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecHostingPlansBillingCycles](
	[ProductID] [int] NOT NULL,
	[CycleID] [int] NOT NULL,
	[SetupFee] [money] NOT NULL,
	[RecurringFee] [money] NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_ecHostingPlansBillingCycles] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC,
	[CycleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecHostingPlans](
	[ProductID] [int] NOT NULL,
	[ResellerID] [int] NOT NULL,
	[PlanID] [int] NOT NULL,
	[UserRole] [int] NOT NULL,
	[InitialStatus] [int] NOT NULL,
	[DomainOption] [int] NOT NULL,
 CONSTRAINT [PK_ecHostingPlans] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC,
	[ResellerID] ASC,
	[PlanID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecVoidCustomerInvoice]
	@ActorID int,
	@InvoiceID int
AS
BEGIN
	-- load customer and reseller identities
	DECLARE @ContractID nvarchar(50), @IssuerID int;
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecInvoice]
	WHERE
		[InvoiceID] = @InvoiceID;
	SELECT
		@IssuerID = [ResellerID] FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access the contract', 16, 1);
		RETURN;
	END
	-- 
	SET NOCOUNT ON;
	-- void invoice
	DELETE FROM [dbo].[ecInvoice] WHERE [InvoiceID] = @InvoiceID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE PROCEDURE [dbo].[ecUpdateTaxation]
	@ActorID int,
	@UserID int,
	@TaxationID int,
	@Country nvarchar(3),
	@State nvarchar(50),
	@Description nvarchar(50),
	@TypeID int,
	@Amount decimal(5,2),
	@Active bit,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--
	DECLARE @T_TaxationID int;
	--
	SELECT @T_TaxationID = [TaxationID] FROM [dbo].[ecTaxations] WHERE [ResellerID] = @UserID AND [Country] = @Country AND [State] = @State;
	--
	SET @T_TaxationID = ISNULL(@T_TaxationID, @TaxationID);
	--
	IF @T_TaxationID = @TaxationID
	BEGIN
		-- insert
		UPDATE
			[dbo].[ecTaxations]
		SET
			[Country] = @Country,
			[State] = @State,
			[Description] = @Description,
			[TypeID] = @TypeID,
			[Amount] = @Amount,
			[Active] = @Active
		WHERE
			[ResellerID] = @UserID
		AND
			[TaxationID] = @TaxationID;
		--
		SET @Result = 0;
		--
		RETURN;
	END

	-- taxation update error
	SET @Result = -202;
	
END


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecUpdateSystemTrigger]
	@ActorID int,
	@TriggerID nvarchar(50),
	@TriggerHandler nvarchar(512),
	@ReferenceID nvarchar(50),
	@Namespace nvarchar(255),
	@Status nvarchar(50)
AS
BEGIN
	DECLARE @OwnerID int;
	SELECT
		@OwnerID = [OwnerID] FROM [dbo].[ecSystemTriggers]
	WHERE
		[TriggerID] = @TriggerID;
	--
	IF [dbo].[CheckUserParent](@ActorID, @OwnerID) = 0
	BEGIN
		RAISERROR('You are not allowed to perform this action', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE [dbo].[ecSystemTriggers] SET
		[TriggerHandler] = @TriggerHandler,
		[ReferenceID] = @ReferenceID,
		[Namespace] = @Namespace,
		[Status] = @Status
	WHERE
		[TriggerID] = @TriggerID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE [dbo].[ecAddTaxation]
	@ActorID int,
	@UserID int,
	@Country nvarchar(3),
	@State nvarchar(50),
	@Description nvarchar(50),
	@TypeID int,
	@Amount decimal(5,2),
	@Active bit,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- check before insert
	IF EXISTS (SELECT [TaxationID] FROM [dbo].[ecTaxations] 
		WHERE [ResellerID] = @UserID AND [Country] = @Country AND [State] = @State)
	BEGIN
		SET @Result = -202;
		RETURN;
	END

	-- insert
    INSERT INTO [dbo].[ecTaxations]
	(
		[ResellerID],
		[Country],
		[State],
		[Description],
		[TypeID],
		[Amount],
		[Active]
	)
	VALUES
	(
		@UserID,
		@Country,
		@State,
		@Description,
		@TypeID,
		@Amount,
		@Active
	);
	--
	SET @Result = SCOPE_IDENTITY();
	
END

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO





CREATE PROCEDURE [dbo].[ecAddSystemTrigger]
	@ActorID int,
	@OwnerID int,
	@TriggerHandler nvarchar(512),
	@ReferenceID nvarchar(50),
	@Namespace nvarchar(255),
	@Status nvarchar(50)
AS
BEGIN
	IF [dbo].[CheckUserParent](@ActorID, @OwnerID) = 0
	BEGIN
		RAISERROR('You are not allowed to perform this action', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT * FROM [dbo].[ecSystemTriggers] WHERE [OwnerID] = @OwnerID AND 
		[TriggerHandler] = @TriggerHandler AND [ReferenceID] = @ReferenceID AND
		[Namespace] = @Namespace AND [Status] = @Status)
	BEGIN
		INSERT INTO [dbo].[ecSystemTriggers]
			([OwnerID], [TriggerHandler], [ReferenceID], [Namespace], [Status])
		VALUES
			(@OwnerID, @TriggerHandler, @ReferenceID, @Namespace, @Status);
	END

END








GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE UpdateServer
(
	@ServerID int,
	@ServerName nvarchar(100),
	@ServerUrl nvarchar(100),
	@Password nvarchar(100),
	@Comments ntext,
	@InstantDomainAlias nvarchar(200),
	@PrimaryGroupID int,
	@ADEnabled bit,
	@ADRootDomain nvarchar(200),
	@ADUsername nvarchar(100),
	@ADPassword nvarchar(100),
	@ADAuthenticationType varchar(50)
)
AS

IF @PrimaryGroupID = 0
SET @PrimaryGroupID = NULL

UPDATE Servers SET
	ServerName = @ServerName,
	ServerUrl = @ServerUrl,
	Password = @Password,
	Comments = @Comments,
	InstantDomainAlias = @InstantDomainAlias,
	PrimaryGroupID = @PrimaryGroupID,
	ADEnabled = @ADEnabled,
	ADRootDomain = @ADRootDomain,
	ADUsername = @ADUsername,
	ADPassword = @ADPassword,
	ADAuthenticationType = @ADAuthenticationType
WHERE ServerID = @ServerID
RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE GetProvider
(
	@ProviderID int
)
AS
SELECT
	ProviderID,
	GroupID,
	ProviderName,
	EditorControl,
	DisplayName,
	ProviderType
FROM Providers
WHERE
	ProviderID = @ProviderID

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE GetUserSettings
(
	@ActorID int,
	@UserID int,
	@SettingsName nvarchar(50)
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

-- find which parent package has overriden NS
DECLARE @ParentUserID int, @TmpUserID int
SET @TmpUserID = @UserID

WHILE 10 = 10
BEGIN

	IF EXISTS
	(
		SELECT PropertyName FROM UserSettings
		WHERE SettingsName = @SettingsName AND UserID = @TmpUserID
	)
	BEGIN
		SELECT
			UserID,
			PropertyName,
			PropertyValue
		FROM
			UserSettings
		WHERE
			UserID = @TmpUserID AND
			SettingsName = @SettingsName
			
		BREAK
	END

	SET @ParentUserID = NULL --reset var
	
	-- get owner
	SELECT
		@ParentUserID = OwnerID
	FROM Users
	WHERE UserID = @TmpUserID
	
	IF @ParentUserID IS NULL -- the last parent
	BREAK
	
	SET @TmpUserID = @ParentUserID
END

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


































CREATE PROCEDURE [dbo].[GetUserParents]
(
	@ActorID int,
	@UserID int
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	U.Comments,
	U.IsPeer,
	U.Username,
	U.FirstName,
	U.LastName,
	U.Email,
	U.CompanyName,
	U.EcommerceEnabled
FROM UserParents(@ActorID, @UserID) AS UP
INNER JOIN Users AS U ON UP.UserID = U.UserID
ORDER BY UP.UserOrder DESC
RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetUsersSummary
(
	@ActorID int,
	@UserID int
)
AS
-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

-- ALL users
SELECT COUNT(UserID) AS UsersNumber FROM Users
WHERE OwnerID = @UserID AND IsPeer = 0

-- BY STATUS users
SELECT StatusID, COUNT(UserID) AS UsersNumber FROM Users
WHERE OwnerID = @UserID AND IsPeer = 0
GROUP BY StatusID
ORDER BY StatusID

-- BY ROLE users
SELECT RoleID, COUNT(UserID) AS UsersNumber FROM Users
WHERE OwnerID = @UserID AND IsPeer = 0
GROUP BY RoleID
ORDER BY RoleID DESC

RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


































CREATE PROCEDURE [dbo].[GetUserByUsername]
(
	@ActorID int,
	@Username nvarchar(50)
)
AS

SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	U.Comments,
	U.IsPeer,
	U.Username,
	CASE WHEN dbo.CanGetUserPassword(@ActorID, UserID) = 1 THEN U.Password
	ELSE '' END AS Password,
	U.FirstName,
	U.LastName,
	U.Email,
	U.SecondaryEmail,
	U.Address,
	U.City,
	U.State,
	U.Country,
	U.Zip,
	U.PrimaryPhone,
	U.SecondaryPhone,
	U.Fax,
	U.InstantMessenger,
	U.HtmlMail,
	U.CompanyName,
	U.EcommerceEnabled
FROM Users AS U
WHERE U.Username = @Username
AND dbo.CanGetUserDetails(@ActorID, UserID) = 1 -- actor user rights

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


































CREATE PROCEDURE [dbo].[GetUserById]
(
	@ActorID int,
	@UserID int
)
AS

-- user can retrieve his own account, his users accounts
-- and his reseller account (without pasword)
SELECT
	U.UserID,
	U.RoleID,
	U.StatusID,
	U.OwnerID,
	U.Created,
	U.Changed,
	U.IsDemo,
	U.Comments,
	U.IsPeer,
	U.Username,
	CASE WHEN dbo.CanGetUserPassword(@ActorID, @UserID) = 1 THEN U.Password
	ELSE '' END AS Password,
	U.FirstName,
	U.LastName,
	U.Email,
	U.SecondaryEmail,
	U.Address,
	U.City,
	U.State,
	U.Country,
	U.Zip,
	U.PrimaryPhone,
	U.SecondaryPhone,
	U.Fax,
	U.InstantMessenger,
	U.HtmlMail,
	U.CompanyName,
	U.EcommerceEnabled
FROM Users AS U
WHERE U.UserID = @UserID
AND dbo.CanGetUserDetails(@ActorID, @UserID) = 1 -- actor user rights

RETURN



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetServiceItemTypes]
AS
SELECT
	[ItemTypeID],
	[GroupID],
	[DisplayName],
	[TypeName],
	[TypeOrder],
	[CalculateDiskspace],
	[CalculateBandwidth],
	[Suspendable],
	[Disposable],
	[Searchable],
	[Importable],
	[Backupable]
FROM
	[ServiceItemTypes]
ORDER BY TypeOrder































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetServiceItemType]
(
	@ItemTypeID int
)
AS
SELECT
	[ItemTypeID],
	[GroupID],
	[DisplayName],
	[TypeName],
	[TypeOrder],
	[CalculateDiskspace],
	[CalculateBandwidth],
	[Suspendable],
	[Disposable],
	[Searchable],
	[Importable],
	[Backupable]
FROM
	[ServiceItemTypes]
WHERE
	[ItemTypeID] = @ItemTypeID































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetServerShortDetails
(
	@ServerID int
)
AS

SELECT
	ServerID,
	ServerName,
	Comments,
	VirtualServer,
	InstantDomainAlias
FROM Servers
WHERE
	ServerID = @ServerID

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetServerInternal
(
	@ServerID int
)
AS
SELECT
	ServerID,
	ServerName,
	ServerUrl,
	Password,
	Comments,
	VirtualServer,
	InstantDomainAlias,
	PrimaryGroupID,
	ADEnabled,
	ADRootDomain,
	ADUsername,
	ADPassword,
	ADAuthenticationType
FROM Servers
WHERE
	ServerID = @ServerID

RETURN 
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE GetServerByName
(
	@ActorID int,
	@ServerName nvarchar(100)
)
AS
-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	ServerID,
	ServerName,
	ServerUrl,
	Password,
	Comments,
	VirtualServer,
	InstantDomainAlias,
	PrimaryGroupID,
	ADRootDomain,
	ADUsername,
	ADPassword,
	ADAuthenticationType
FROM Servers
WHERE
	ServerName = @ServerName
	AND @IsAdmin = 1

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetServer]
(
	@ActorID int,
	@ServerID int
)
AS
-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	ServerID,
	ServerName,
	ServerUrl,
	Password,
	Comments,
	VirtualServer,
	InstantDomainAlias,
	PrimaryGroupID,
	ADEnabled,
	ADRootDomain,
	ADUsername,
	ADPassword,
	ADAuthenticationType
FROM Servers
WHERE
	ServerID = @ServerID
	AND @IsAdmin = 1

RETURN 






































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetSearchableServiceItemTypes

AS
SELECT
	ItemTypeID,
	DisplayName
FROM
	ServiceItemTypes
WHERE Searchable = 1
ORDER BY TypeOrder
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

























/****** Object:  StoredProcedure [dbo].[GetScheduleTaskViewConfigurations]    Script Date: 09/10/2007 17:53:56 ******/

CREATE PROCEDURE [dbo].[GetScheduleTaskViewConfigurations]
(
	@TaskID nvarchar(100)
)
AS

SELECT
	@TaskID AS TaskID,
	STVC.ConfigurationID,
	STVC.Environment,
	STVC.Description
FROM ScheduleTaskViewConfiguration AS STVC
WHERE STVC.TaskID = @TaskID

RETURN





























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
















CREATE PROCEDURE [dbo].[GetProviders]
AS
SELECT
	PROV.ProviderID,
	PROV.GroupID,
	PROV.ProviderName,
	PROV.EditorControl,
	PROV.DisplayName,
	PROV.ProviderType,
	RG.GroupName + ' - ' + PROV.DisplayName AS ProviderName,
	PROV.DisableAutoDiscovery
FROM Providers AS PROV
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
ORDER BY RG.GroupOrder, PROV.DisplayName
RETURN



















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetClusters
(
	@ActorID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

-- get the list
SELECT
	ClusterID,
	ClusterName
FROM Clusters
WHERE @IsAdmin = 1

RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[GetAuditLogRecordsPaged]
(
	@ActorID int,
	@UserID int,
	@PackageID int,
	@ItemID int,
	@ItemName nvarchar(100),
	@StartDate datetime,
	@EndDate datetime,
	@SeverityID int,
	@SourceName varchar(100),
	@TaskName varchar(100),
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS

-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

IF @SourceName IS NULL SET @SourceName = ''
IF @TaskName IS NULL SET @TaskName = ''
IF @ItemName IS NULL SET @ItemName = ''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'L.StartDate DESC' 

-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

SET @sql = '
DECLARE @IsAdmin bit
SET @IsAdmin = 0
IF EXISTS(SELECT UserID FROM Users WHERE UserID = @ActorID AND RoleID = 1)
SET @IsAdmin = 1

DECLARE @EndRow int
SET @EndRow = @StartRow + @MaximumRows
DECLARE @Records TABLE
(
	ItemPosition int IDENTITY(1,1),
	RecordID varchar(32)
)
INSERT INTO @Records (RecordID)
SELECT
	L.RecordID
FROM AuditLog AS L
WHERE
((@PackageID = 0 AND dbo.CheckUserParent(@UserID, L.UserID) = 1 OR (L.UserID IS NULL AND @IsAdmin = 1))
	OR (@PackageID > 0 AND L.PackageID = @PackageID))
AND L.StartDate BETWEEN @StartDate AND @EndDate
AND ((@SourceName = '''') OR (@SourceName <> '''' AND L.SourceName = @SourceName))
AND ((@TaskName = '''') OR (@TaskName <> '''' AND L.TaskName = @TaskName))
AND ((@ItemID = 0) OR (@ItemID > 0 AND L.ItemID = @ItemID))
AND ((@ItemName = '''') OR (@ItemName <> '''' AND L.ItemName LIKE @ItemName))
AND ((@SeverityID = -1) OR (@SeverityID > -1 AND L.SeverityID = @SeverityID)) '

IF @SortColumn <> '' AND @SortColumn IS NOT NULL
SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

SET @sql = @sql + ' SELECT COUNT(RecordID) FROM @Records;
SELECT
	TL.RecordID,
    L.SeverityID,
    L.StartDate,
    L.FinishDate,
    L.ItemID,
    L.SourceName,
    L.TaskName,
    L.ItemName,
    L.ExecutionLog,

    ISNULL(L.UserID, 0) AS UserID,
	L.Username,
	U.FirstName,
	U.LastName,
	U.FullName,
	ISNULL(U.RoleID, 0) AS RoleID,
	U.Email,
	CASE U.IsPeer
		WHEN 1 THEN U.OwnerID
		ELSE U.UserID
	END EffectiveUserID
FROM @Records AS TL
INNER JOIN AuditLog AS L ON TL.RecordID = L.RecordID
LEFT OUTER JOIN UsersDetailed AS U ON L.UserID = U.UserID
WHERE TL.ItemPosition BETWEEN @StartRow + 1 AND @EndRow'

exec sp_executesql @sql, N'@TaskName varchar(100), @SourceName varchar(100), @PackageID int, @ItemID int, @ItemName nvarchar(100), @StartDate datetime,
@EndDate datetime, @StartRow int, @MaximumRows int, @UserID int, @ActorID int, @SeverityID int',
@TaskName, @SourceName, @PackageID, @ItemID, @ItemName, @StartDate, @EndDate, @StartRow, @MaximumRows, @UserID, @ActorID,
@SeverityID


RETURN


















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetTaxationsPaged]
	@ActorID int,
	@UserID int,
	@MaximumRows int,
	@StartRowIndex int
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- do some calculations
    DECLARE @EndIndex int;
	--
	SET @EndIndex = @MaximumRows + @StartRowIndex;
	SET @StartRowIndex = @StartRowIndex + 1;
	--
    WITH [TaxesCTE] AS (
		SELECT
			ROW_NUMBER() OVER(ORDER BY [TaxationID] ASC) AS [RowIndex],
			*
		FROM
			[dbo].[ecTaxations]
		WHERE
			[ResellerID] = @UserID
	)
	--
	SELECT * FROM [TaxesCTE] WHERE [RowIndex] BETWEEN @StartRowIndex AND @EndIndex;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetTaxationsCount]
	@ActorID int,
	@UserID int,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT @Result = COUNT([TaxationID]) FROM [dbo].[ecTaxations] WHERE [ResellerID] = @UserID;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetTaxation]
	@ActorID int,
	@UserID int,
	@TaxationID int
AS
BEGIN
	--
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    --
	SELECT * FROM [dbo].[ecTaxations] WHERE [ResellerID] = @UserID AND [TaxationID] = @TaxationID;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE [dbo].[ecGetStorefrontProductsByType]
	@UserID int,
	@TypeID int
AS
BEGIN

	SELECT 
		*
	FROM 
		[dbo].[ecProduct]
	WHERE
		[ResellerID] = @UserID
	AND
		[TypeID] = @TypeID
	AND
		[Enabled] = 1
	ORDER BY
		[ProductName] ASC;

END


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetStorefrontProduct]
	@ResellerID int,
	@ProductID int
AS
BEGIN

	SET NOCOUNT ON;

    SELECT 
		*
	FROM 
		[dbo].[ecProduct]
	WHERE 
		[ProductID] = @ProductID 
	AND 
		[ResellerID] = @ResellerID
	AND
		[Enabled] = 1;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecSvcsUsageLog](
	[ServiceID] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[SvcCycleID] [int] NOT NULL,
	[PeriodClosed] [bit] NULL,
 CONSTRAINT [PK_ecServicesLifeCyclesLog] PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC,
	[SvcCycleID] ASC,
	[StartDate] ASC,
	[EndDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



























CREATE PROCEDURE [dbo].[ecSetStoreSettings] 
	@ActorID int,
	@UserID int,
	@SettingsName nvarchar(50),
	@Xml ntext,
	@Result int OUTPUT
AS
BEGIN
/*
XML Format:

<settings>
	<setting name="" value="" />
</settings>
*/
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	--
	SET @Result = 0;

	DECLARE @docid int;
	--Create an internal representation of the XML document.
	EXEC sp_xml_preparedocument @docid OUTPUT, @Xml;

	-- cleanup
    DELETE FROM [dbo].[ecStoreSettings] 
    WHERE [SettingsName] = @SettingsName AND [ResellerID] = @UserID;

	INSERT INTO [dbo].[ecStoreSettings]
	(
		[ResellerID],
		[SettingsName],
		[PropertyName],
		[PropertyValue]
	)
	SELECT
		@UserID,
		@SettingsName,
		[XML].[PropertyName],
		[XML].[PropertyValue]
	FROM OPENXML(@docid, '/settings/setting', 1) WITH 
	(
		[PropertyName] nvarchar(50) '@name',
		[PropertyValue] ntext '@value'
	) AS [XML];

	-- remove document
	EXEC sp_xml_removedocument @docid;
	
END






























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecSetPluginProperties]
	@ActorID int,
	@UserID int,
	@PluginID int,
	@Xml ntext,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

/*
XML Format:
<properties>
	<property name="" value="" />
</properties>
*/
	-- result is ok
	SET @Result = 0;
	--
	DECLARE @XmlDocID int;
	--Create an internal representation of the XML document.
	EXEC sp_xml_preparedocument @XmlDocID OUTPUT, @xml;
    -- cleanup
	DELETE FROM [dbo].[ecPluginsProperties] 
	WHERE [ResellerID] = @UserID AND [PluginID] = @PluginID;
	-- insert
	INSERT INTO [dbo].[ecPluginsProperties]
	(
		[PluginID],
		[ResellerID],
		[PropertyName],
		[PropertyValue]
	)
	SELECT
		@PluginID,
		@UserID,
		[XML].[PropertyName],
		[XML].[PropertyValue]
	FROM OPENXML(@XmlDocID, '/properties/property',1) WITH 
	(
		[PropertyName] nvarchar(50) '@name',
		[PropertyValue] ntext '@value'
	) AS [XML];

	-- remove document
	EXEC sp_xml_removedocument @XmlDocID;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecSetPaymentProfile]
	@ActorID int,
	@ContractID nvarchar(50),
	@PropertyNames ntext,
	@PropertyValues ntext
AS
BEGIN
	DECLARE @IssuerID int;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- cleanup first
	DELETE FROM [dbo].[ecPaymentProfiles] WHERE [ContractID] = @ContractID;

	--
    INSERT INTO [dbo].[ecPaymentProfiles]
	(
		[ContractID],
		[PropertyNames],
		[PropertyValues]
	)
	VALUES
	(
		@ContractID,
		@PropertyNames,
		@PropertyValues
	);

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecHostingAddons](
	[ProductID] [int] NOT NULL,
	[PlanID] [int] NULL,
	[Recurring] [bit] NOT NULL,
	[DummyAddon] [bit] NOT NULL,
	[Countable] [bit] NOT NULL,
	[SetupFee] [money] NULL,
	[OneTimeFee] [money] NULL,
	[2COID] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[ResellerID] [int] NOT NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetWholeCategoriesSet]
	@ActorID int,
	@UserID int
AS
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

	SELECT * FROM [dbo].[ecCategory] WHERE [ResellerID] = @UserID ORDER BY [CategoryName];

RETURN

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetUnpaidInvoices]
	@ActorID int,
	@ResellerID int
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @ResellerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- lookup for paid invoices
	WITH [UNPAID_INVOICES] ([InvoiceID]) AS
	(
		SELECT
			[I].[InvoiceID]
		FROM
			[dbo].[ecInvoice] AS [I]
		INNER JOIN
			[dbo].[ecContracts] AS [C] ON [C].[ContractID] = [I].[ContractID]
		WHERE
			[C].[ResellerID] = @ResellerID
		EXCEPT
		SELECT
			[InvoiceID]
		FROM
			[dbo].[ecCustomersPayments] AS [CP]
		INNER JOIN
			[dbo].[ecContracts] AS [C] ON [C].[ContractID] = [CP].[ContractID]
		WHERE
			[C].[ResellerID] = @ResellerID
	)
	-- select unpaid invoices
    SELECT * FROM [dbo].[ecInvoice] WHERE [InvoiceID] IN (
		SELECT [InvoiceID] FROM [UNPAID_INVOICES]
	);

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[GetIPAddressesPaged]	
(
	@ActorID int,
	@PoolID int,
	@ServerID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS
BEGIN

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
@IsAdmin = 1
AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
AND (@ServerID = 0 OR @ServerID <> 0 AND IP.ServerID = @ServerID)
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'IP.ExternalIP ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(IP.AddressID)
FROM dbo.IPAddresses AS IP
LEFT JOIN Servers AS S ON IP.ServerID = S.ServerID
LEFT JOIN PackageIPAddresses AS PA ON IP.AddressID = PA.AddressID
LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
LEFT JOIN dbo.Packages P ON PA.PackageID = P.PackageID
LEFT JOIN dbo.Users U ON P.UserID = U.UserID
WHERE ' + @condition + '

DECLARE @Addresses AS TABLE
(
	AddressID int
);

WITH TempItems AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		IP.AddressID
	FROM dbo.IPAddresses AS IP
	LEFT JOIN Servers AS S ON IP.ServerID = S.ServerID
	LEFT JOIN PackageIPAddresses AS PA ON IP.AddressID = PA.AddressID
	LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
	LEFT JOIN dbo.Packages P ON PA.PackageID = P.PackageID
	LEFT JOIN dbo.Users U ON U.UserID = P.UserID
	WHERE ' + @condition + '
)

INSERT INTO @Addresses
SELECT AddressID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	IP.AddressID,
	IP.PoolID,
	IP.ExternalIP,
	IP.InternalIP,
	IP.SubnetMask,
	IP.DefaultGateway,
	IP.Comments,

	IP.ServerID,
	S.ServerName,

	PA.ItemID,
	SI.ItemName,

	PA.PackageID,
	P.PackageName,

	P.UserID,
	U.UserName
FROM @Addresses AS TA
INNER JOIN dbo.IPAddresses AS IP ON TA.AddressID = IP.AddressID
LEFT JOIN Servers AS S ON IP.ServerID = S.ServerID
LEFT JOIN PackageIPAddresses AS PA ON IP.AddressID = PA.AddressID
LEFT JOIN ServiceItems SI ON PA.ItemId = SI.ItemID
LEFT JOIN dbo.Packages P ON PA.PackageID = P.PackageID
LEFT JOIN dbo.Users U ON U.UserID = P.UserID
'

exec sp_executesql @sql, N'@IsAdmin bit, @PoolID int, @ServerID int, @StartRow int, @MaximumRows int',
@IsAdmin, @PoolID, @ServerID, @StartRow, @MaximumRows

END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetDnsRecordsByGroup
(
	@GroupID int
)
AS
SELECT
	RGR.RecordID,
	RGR.RecordOrder,
	RGR.GroupID,
	RGR.RecordType,
	RGR.RecordName,
	RGR.RecordData,
	RGR.MXPriority
FROM
	ResourceGroupDnsRecords AS RGR
WHERE RGR.GroupID = @GroupID
ORDER BY RGR.RecordOrder
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







































CREATE PROCEDURE [dbo].[ecAddCategory] 
	@ActorID int,
	@UserID int,
	@CategoryName nvarchar(255),
	@CategorySku nvarchar(50),
	@ParentID int,
	@ShortDescription ntext,
	@FullDescription ntext,
	@Result int OUTPUT
AS
BEGIN
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

	SET NOCOUNT ON;

	DECLARE @Level int;

	IF @ParentID = -1
		SET @ParentID = NULL;
	
	-- identify category level
	SELECT @Level = [Level] FROM [dbo].[ecCategory] WHERE [CategoryID] = @ParentID AND [ResellerID] = @UserID;
	IF @Level >= 0
		SET @Level = @Level + 1;
	ELSE
		SET @Level = 0;

    INSERT INTO [dbo].[ecCategory]
	(
		[CategoryName],
		[CategorySku],
		[ParentID],
		[Level],
		[ShortDescription],
		[FullDescription],
		[CreatorID],
		[ResellerID]
	)
	VALUES 
	(
		@CategoryName,
		@CategorySku,
		@ParentID,
		@Level,
		@ShortDescription,
		@FullDescription,
		@ActorID,
		@UserID
	)

	SET @Result = SCOPE_IDENTITY();

END












































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecAddBillingCycle] 
	@ActorID int,
	@UserID int,
	@CycleName nvarchar(255),
	@BillingPeriod nvarchar(50),
	@PeriodLength int,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

    INSERT INTO [dbo].[ecBillingCycles]
	(
		[ResellerID],
		[CycleName],
		[BillingPeriod],
		[PeriodLength],
		[Created]
	)
	VALUES
	(
		@UserID,
		@CycleName,
		@BillingPeriod,
		@PeriodLength,
		GETDATE()
	);
	-- return result
	SET @Result = SCOPE_IDENTITY();

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE AddServer
(
	@ServerID int OUTPUT,
	@ServerName nvarchar(100),
	@ServerUrl nvarchar(100),
	@Password nvarchar(100),
	@Comments ntext,
	@VirtualServer bit,
	@InstantDomainAlias nvarchar(200),
	@PrimaryGroupID int,
	@ADEnabled bit,
	@ADRootDomain nvarchar(200),
	@ADUsername nvarchar(100),
	@ADPassword nvarchar(100),
	@ADAuthenticationType varchar(50)
)
AS

IF @PrimaryGroupID = 0
SET @PrimaryGroupID = NULL

INSERT INTO Servers
(
	ServerName,
	ServerUrl,
	Password,
	Comments,
	VirtualServer,
	InstantDomainAlias,
	PrimaryGroupID,
	ADEnabled,
	ADRootDomain,
	ADUsername,
	ADPassword,
	ADAuthenticationType
)
VALUES
(
	@ServerName,
	@ServerUrl,
	@Password,
	@Comments,
	@VirtualServer,
	@InstantDomainAlias,
	@PrimaryGroupID,
	@ADEnabled,
	@ADRootDomain,
	@ADUsername,
	@ADPassword,
	@ADAuthenticationType
)

SET @ServerID = SCOPE_IDENTITY()

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecAddCustomerPayment]
	@ActorID int, 
	@ContractID nvarchar(50),
	@InvoiceID int,
	@TransactionID nvarchar(255),
	@Total money,
	@Currency nvarchar(3),
	@MethodName nvarchar(50),
	@StatusID int,
	@Result int OUTPUT
AS
BEGIN
	DECLARE @ResellerID int, @IssuerID int;
	SELECT
		@ResellerID = [ResellerID],
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	--
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to perform this action', 16, 1);
		RETURN;
	END
	
	SET NOCOUNT ON;

	INSERT INTO [dbo].[ecCustomersPayments]
	(
		[ContractID],
		[InvoiceID],
		[TransactionID],
		[Total],
		[Currency],
		[MethodName],
		[PluginID],
		[StatusID]
	)
	SELECT
		@ContractID,
		@InvoiceID,
		@TransactionID,
		@Total,
		@Currency,
		@MethodName,
		[PluginID],
		@StatusID
	FROM
		[dbo].[ecPaymentMethods]
	WHERE
		[MethodName] = @MethodName
	AND
		[ResellerID] = @ResellerID;

	SET @Result = SCOPE_IDENTITY();

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE AddComment
(
	@ActorID int,
	@ItemTypeID varchar(50),
	@ItemID int,
	@CommentText nvarchar(1000),
	@SeverityID int
)
AS
INSERT INTO Comments
(
	ItemTypeID,
	ItemID,
	UserID,
	CreatedDate,
	CommentText,
	SeverityID
)
VALUES
(
	@ItemTypeID,
	@ItemID,
	@ActorID,
	GETDATE(),
	@CommentText,
	@SeverityID
)
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecDomainSvcsCycles](
	[SvcCycleID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceID] [int] NOT NULL,
	[CycleName] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[BillingPeriod] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PeriodLength] [int] NOT NULL,
	[SetupFee] [money] NOT NULL,
	[RecurringFee] [money] NOT NULL,
	[Currency] [nvarchar](3) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_ecDomainsSvcsCycles] PRIMARY KEY CLUSTERED 
(
	[SvcCycleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecDomainSvcs](
	[ServiceID] [int] NOT NULL,
	[ProductID] [int] NULL,
	[DomainID] [int] NULL,
	[PluginID] [int] NULL,
	[FQDN] [nvarchar](64) COLLATE Latin1_General_CI_AS NOT NULL,
	[SvcCycleID] [int] NULL,
	[PropertyNames] [ntext] COLLATE Latin1_General_CI_AS NULL,
	[PropertyValues] [ntext] COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_ecDomainsSvcs] PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecDeleteTaxation]
	@ActorID int,
	@UserID int,
	@TaxationID int,
	@Result int OUTPUT
AS
BEGIN
	--
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--
    DELETE FROM [dbo].[ecTaxations] WHERE [ResellerID] = @UserID AND [TaxationID] = @TaxationID;
	--
	SET @Result = 0;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecDeleteSystemTrigger]
	@ActorID int,
	@TriggerID nvarchar(50)
AS
BEGIN
	DECLARE @OwnerID int;
	SELECT
		@OwnerID = [OwnerID] FROM [dbo].[ecSystemTriggers]
	WHERE
		[TriggerID] = @TriggerID;
	--
	IF [dbo].[CheckUserParent](@ActorID, @OwnerID) = 0
	BEGIN
		RAISERROR('You are not allowed to perform this action', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE FROM [dbo].[ecSystemTriggers] WHERE [TriggerID] = @TriggerID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecDeletePaymentProfile]
	@ActorID int,
	@ContractID nvarchar(50),
	@Result int OUTPUT
AS
BEGIN
	DECLARE @CustomerID int;
	SELECT
		@CustomerID = [CustomerID] FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @CustomerID) = 0
	BEGIN
		RAISERROR('You are not allowed to perform this action', 16, 1);
		RETURN;
	END

	SET @Result = 0;

    DELETE FROM [dbo].[ecPaymentProfiles] WHERE [ContractID] = @ContractID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecDeletePaymentMethod]
	@ActorID int,
	@UserID int,
	@MethodName nvarchar(50),
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- 
	SET @Result = 0;
    -- remove
	DELETE FROM [dbo].[ecPaymentMethods] 
	WHERE [ResellerID] = @UserID AND [MethodName] = @MethodName;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecDeleteCustomerService]
	@ActorID int,
	@ServiceID int,
	@Result int OUTPUT
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecService]
	WHERE
		[ServiceID] = @ServiceID;
	SELECT
		@IssuerID = [ResellerID] FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this action', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET @Result = 0;

    DELETE FROM [dbo].[ecService] WHERE	[ServiceID] = @ServiceID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecDeleteCustomerPayment]
	@ActorID int,
	@PaymentID int,
	@Result int OUTPUT
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecCustomersPayments]
	WHERE
		[PaymentID] = @PaymentID
	SELECT
		@IssuerID = [ResellerID] FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this action', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET @Result = 0;

    DELETE
		FROM [dbo].[ecCustomersPayments] 
	WHERE
		[PaymentID] = @PaymentID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetBillingCyclesPaged]
	@ActorID int,
	@UserID int,
	@MaximumRows int,
	@StartRowIndex int
AS
BEGIN
	
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

	DECLARE @EndIndex int;

	SET @EndIndex = @MaximumRows + @StartRowIndex;
	SET @StartRowIndex = @StartRowIndex + 1;

    WITH [BillingCyclesCTE] AS (
		SELECT
			ROW_NUMBER() OVER(ORDER BY [Created] DESC) AS [RowIndex],
			*
		FROM
			[dbo].[ecBillingCycles]
		WHERE
			[ResellerID] = @UserID
	)

	SELECT
		*
	FROM
		[BillingCyclesCTE]
	WHERE
		[RowIndex] BETWEEN @StartRowIndex AND @EndIndex;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetBillingCyclesFree]
	@ActorID int,
	@UserID int,
	@CyclesTakenXml ntext
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

	DECLARE @DocID int;

	EXEC sp_xml_preparedocument @DocID OUTPUT, @CyclesTakenXml;

    SELECT * FROM [dbo].[ecBillingCycles] WHERE [ResellerID] = @UserID AND [CycleID] NOT IN (
		SELECT [CycleTakenID] FROM OPENXML(@DocID, '/CyclesTaken/Cycle',1) WITH
		(
			[CycleTakenID] int '@id'
		)
	);

	EXEC sp_xml_removedocument @DocID;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetBillingCyclesCount]
	@ActorID int,
	@UserID int,
	@Count int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

    SELECT @Count = COUNT(*) FROM [dbo].[ecBillingCycles] WHERE [ResellerID] = @UserID;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetBillingCycle]
	@ActorID int,
	@UserID int,
	@CycleID int
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

    SELECT * FROM [dbo].[ecBillingCycles] WHERE [ResellerID] = @UserID AND [CycleID] = @CycleID;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





































CREATE PROCEDURE [dbo].[ecGetPluginProperties]
	@ActorID int,
	@UserID int,
	@PluginID int
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

	SET NOCOUNT ON;

	SELECT [PropertyName], [PropertyValue] FROM [dbo].[ecPluginsProperties]
	WHERE [PluginID] = @PluginID AND [ResellerID] = @UserID;

END








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetPaymentProfile]
	@ActorID int,
	@ContractID nvarchar(50)
AS
BEGIN
	DECLARE @IssuerID int;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    --
	SELECT * FROM [dbo].[ecPaymentProfiles] WHERE [ContractID] = @ContractID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetPaymentMethod]
	@ActorID int,
	@UserID int,
	@MethodName nvarchar(50)
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM [dbo].[ecPaymentMethods] 
	WHERE [MethodName] = @MethodName AND [ResellerID] = @UserID;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetInvoicesItemsToActivate]
	@ActorID int,
	@ResellerID int
AS
BEGIN

	IF [dbo].[CheckUserParent](@ActorID, @ResellerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END

	SET NOCOUNT ON;

	-- lookup for paid invoices
	WITH [PAID_INVOICES] ([InvoiceID]) AS
	(
		SELECT
			[InvoiceID] FROM [dbo].[ecCustomersPayments] AS [CP]
		INNER JOIN
			[dbo].[ecContracts] AS [C] ON [C].[ContractID] = [CP].[ContractID]
		WHERE
			[C].[ResellerID] = @ResellerID AND [StatusID] = 1 -- Approved Payments Only
	)
	SELECT
		* FROM [dbo].[ecInvoiceItems]
	WHERE
		[ServiceID] IS NOT NULL AND [Processed] = 0 AND [InvoiceID] IN (SELECT [InvoiceID] FROM [PAID_INVOICES]);
		

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetInvoicesItemsOverdue]
	@ActorID int,
	@ResellerID int,
	@DateOverdue datetime
AS
BEGIN
	-- check user parent
	IF [dbo].[CheckUserParent](@ActorID, @ResellerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- lookup for paid invoices
	WITH [OVERDUE_INVOICES] ([InvoiceID]) AS
	(
		SELECT
			[I].[InvoiceID]
		FROM
			[dbo].[ecInvoice] AS [I]
		INNER JOIN
			[dbo].[ecContracts] AS [C] ON [C].[ContractID] = [I].[ContractID]
		WHERE
			[C].[ResellerID] = @ResellerID
		AND
			DATEDIFF(second, [DueDate], @DateOverdue) >= 0
		EXCEPT
		SELECT
			[InvoiceID]
		FROM
			[dbo].[ecCustomersPayments] AS [CP]
		INNER JOIN
			[dbo].[ecContracts] AS [C] ON [C].[ContractID] = [CP].[ContractID]
		WHERE
			[C].[ResellerID] = @ResellerID
		AND
			[CP].[StatusID] = 1 -- Approved payments only
	)
    SELECT * FROM [dbo].[ecInvoiceItems] 
		WHERE [ServiceID] IS NOT NULL
			AND [InvoiceID] IN (
				SELECT [InvoiceID] FROM [OVERDUE_INVOICES]
			);

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetProductsPagedByType]
	@ActorID int,
	@UserID int,
	@TypeID int,
	@MaximumRows int,
	@StartRowIndex int
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

    DECLARE @EndIndex int;

	SET @EndIndex = @MaximumRows + @StartRowIndex;
	SET @StartRowIndex = @StartRowIndex + 1;

	WITH [ProductsCTE] AS (
		SELECT
			ROW_NUMBER() OVER(ORDER BY [Created] DESC) AS [RowIndex],
			*
		FROM 
			[dbo].[ecProduct]
		WHERE
			[ResellerID] = @UserID
		AND
			[TypeID] = @TypeID
	)

	SELECT * FROM [ProductsCTE] WHERE [RowIndex] BETWEEN @StartRowIndex AND @EndIndex;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetProductsCountByType]
	@ActorID int,
	@UserID int,
	@TypeID int,
	@Count int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

    SELECT @Count = COUNT(*) FROM [dbo].[ecProduct] WHERE [TypeID] = @TypeID AND [ResellerID] = @UserID;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetProductsByType] 
	@UserID int,
	@TypeID int
AS
BEGIN

    SELECT * FROM [dbo].[ecProduct] WHERE [ResellerID] = @UserID AND [TypeID] = @TypeID ORDER BY [ProductName];

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetResellerPMPlugin]
	@ResellerID int,
	@MethodName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		[SP].*
	FROM
		[dbo].[ecPaymentMethods] AS [PM]
	INNER JOIN
		[dbo].[ecSupportedPlugins] AS [SP]
	ON
		[SP].[PluginID] = [PM].[PluginID]
	WHERE
		[PM].[MethodName] = @MethodName
	AND
		[PM].[ResellerID] = @ResellerID;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetResellerPaymentMethods]
	@ResellerID int
AS
BEGIN

	SELECT * FROM [dbo].[ecPaymentMethods]
	WHERE [ResellerID] = @ResellerID;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetResellerPaymentMethod]
	@ResellerID int,
	@MethodName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		[PM].[ResellerID],
		[PM].[MethodName],
		[PM].[DisplayName],
		[PM].[SupportedItems],
		[SP].[Interactive]
	FROM
		[dbo].[ecPaymentMethods] AS [PM]
	INNER JOIN
		[dbo].[ecSupportedPlugins] AS [SP]
	ON
		[SP].[PluginID] = [PM].[PluginID]
	WHERE
		[PM].[ResellerID] = @ResellerID
	AND
		[PM].[MethodName] = @MethodName;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetCategoriesPaged]
	@ActorID int,
	@UserID int,
	@ParentID int,
	@MaximumRows int,
	@StartRowIndex int
AS
		-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

	DECLARE @EndIndex int;

	SET @EndIndex = @MaximumRows + @StartRowIndex;
	SET @StartRowIndex = @StartRowIndex + 1;

	IF @ParentID > 0
		BEGIN
			WITH [CategoryCTE] AS (
				SELECT
					ROW_NUMBER() OVER(ORDER BY [Created] DESC) AS [RowIndex],
					*
				FROM 
					[dbo].[ecCategory]
				WHERE
					[ParentID] = @ParentID
					AND
					[ResellerID] = @UserID
			)

			SELECT * FROM [CategoryCTE] WHERE [RowIndex] BETWEEN @StartRowIndex AND @EndIndex ORDER BY [CategoryName];
		END
	ELSE
		BEGIN
			WITH [CategoryCTE] AS (
				SELECT
					ROW_NUMBER() OVER(ORDER BY [Created] DESC) AS [RowIndex],
					*
				FROM 
					[dbo].[ecCategory]
				WHERE
					[ParentID] IS NULL
					AND
					[ResellerID] = @UserID
			)

			SELECT * FROM [CategoryCTE] WHERE [RowIndex] BETWEEN @StartRowIndex AND @EndIndex ORDER BY [CategoryID], [CategoryName];
		END
	
RETURN
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetServiceItemType]
	@ServiceID int
AS
BEGIN
	
    SELECT
		[PT].* FROM [dbo].[ecProductType] AS [PT]
	INNER JOIN
		[dbo].[ecService] AS [S] ON [PT].[TypeID] = [S].[TypeID]
	WHERE
		[S].[ServiceID] = @ServiceID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetCustomersPaymentsPaged]
	@ActorID int,
	@UserID int,
	@IsReseller bit,
	@MaximumRows int,
	@StartRowIndex int
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @EndIndex int;

	SET @EndIndex = @MaximumRows + @StartRowIndex;
	SET @StartRowIndex = @StartRowIndex + 1;

	IF @IsReseller = 1
	BEGIN
		WITH [PAYMENTS] AS (
			SELECT
				ROW_NUMBER() OVER(ORDER BY [CP].[Created] DESC) AS [RowIndex], [CP].* FROM [dbo].[ecCustomersPayments] AS [CP]
			INNER JOIN
				[dbo].[ecContracts] AS [C] ON [C].[ContractID] = [CP].[ContractID]
			WHERE
				[C].[ResellerID] = @UserID
		)
		
		SELECT
			[P].*, [INV].[InvoiceNumber], [SP].[DisplayName] AS [ProviderName] FROM [PAYMENTS] AS [P]
		LEFT OUTER JOIN
			[dbo].[ecSupportedPlugins] AS [SP] ON [SP].[PluginID] = [P].[PluginID]
		LEFT OUTER JOIN
			[dbo].[ecInvoice] AS [INV] ON [INV].[InvoiceID] = [P].[InvoiceID]
		WHERE
			[RowIndex] BETWEEN @StartRowIndex AND @EndIndex 
		ORDER BY
			[Created] DESC;
		
		RETURN;
	END;
	
	WITH [PAYMENTS] AS (
		SELECT
			ROW_NUMBER() OVER(ORDER BY [CP].[Created] DESC) AS [RowIndex], [CP].* FROM [dbo].[ecCustomersPayments] AS [CP]
		INNER JOIN
			[dbo].[ecContracts] AS [C] ON [C].[ContractID] = [CP].[ContractID]
		WHERE
			[C].[CustomerID] = @UserID
	)
	
	SELECT
		[P].*, [INV].[InvoiceNumber], [SP].[DisplayName] AS [ProviderName] FROM [PAYMENTS] AS [P]
	LEFT OUTER JOIN
		[dbo].[ecSupportedPlugins] AS [SP] ON [SP].[PluginID] = [P].[PluginID]
	LEFT OUTER JOIN
		[dbo].[ecInvoice] AS [INV] ON [INV].[InvoiceID] = [P].[InvoiceID] 
	WHERE
		[RowIndex] BETWEEN @StartRowIndex AND @EndIndex 
	ORDER BY
		[Created] DESC;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetCustomersPaymentsCount]
	@ActorID int,
	@UserID int,
	@IsReseller bit,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @IsReseller = 1
	BEGIN
		SELECT
			@Result = COUNT([CP].[PaymentID]) FROM [dbo].[ecCustomersPayments] AS [CP]
		INNER JOIN
			[dbo].[ecContracts] AS [C] ON [C].[ContractID] = [CP].[ContractID]
		WHERE
			[C].[ResellerID] = @UserID;
		RETURN;
	END

	SELECT
		@Result = COUNT([CP].[PaymentID]) FROM [dbo].[ecCustomersPayments] AS [CP]
	INNER JOIN
		[dbo].[ecContracts] AS [C] ON [C].[ContractID] = [CP].[ContractID]
	WHERE
		[C].[CustomerID] = @UserID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetCustomersInvoicesPaged]
	@ActorID int,
	@UserID int,
	@IsReseller bit,
	@MaximumRows int,
	@StartRowIndex int
AS
BEGIN
	-- check actor rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END

	SET NOCOUNT ON;

    DECLARE @EndIndex int;

	SET @EndIndex = @MaximumRows + @StartRowIndex;
	SET @StartRowIndex = @StartRowIndex + 1;

	IF @IsReseller = 1
	BEGIN
		-- get reseller invoices
		WITH [INVOICES] AS (
			SELECT
				ROW_NUMBER() OVER(ORDER BY [Created] DESC) AS [RowIndex], * FROM [dbo].[ContractsInvoicesDetailed]
			WHERE
				[ResellerID] = @UserID
		)

		SELECT * FROM [INVOICES]
			WHERE [RowIndex] BETWEEN @StartRowIndex AND @EndIndex
				ORDER BY [Created] DESC;

		RETURN;
	END;
	
	-- get customer invoices
	WITH [INVOICES] AS (
		SELECT
			ROW_NUMBER() OVER(ORDER BY [Created] DESC) AS [RowIndex], * FROM [dbo].[ContractsInvoicesDetailed]
		WHERE
			[CustomerID] = @UserID
	)

	SELECT * FROM [INVOICES] 
		WHERE [RowIndex] BETWEEN @StartRowIndex AND @EndIndex
			ORDER BY [Created] DESC;

	RETURN;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetCustomersInvoicesCount]
	@ActorID int,
	@UserID int,
	@IsReseller bit,
	@Result int OUTPUT
AS
BEGIN

	-- check user parent
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END

	SET NOCOUNT ON;

	IF @IsReseller = 1
	BEGIN
		SELECT
			@Result = COUNT([InvoiceID]) FROM [dbo].[ContractsInvoicesDetailed]
		WHERE
			[ResellerID] = @UserID;
		RETURN;
	END
	
	SELECT
		@Result = COUNT([InvoiceID]) FROM [dbo].[ContractsInvoicesDetailed]
	WHERE
		[CustomerID] = @UserID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetCustomerService]
	@ActorID int,
	@ServiceID int
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecService]
	WHERE
		[ServiceID] = @ServiceID;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END

    SELECT * FROM [dbo].[ecService] WHERE [ServiceID] = @ServiceID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE PROCEDURE [dbo].[ChangeUserPassword]
(
	@ActorID int,
	@UserID int,
	@Password nvarchar(200)
)
AS

-- check actor rights
IF dbo.CanUpdateUserDetails(@ActorID, @UserID) = 0
RETURN

UPDATE Users
SET Password = @Password
WHERE UserID = @UserID

RETURN 



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO














CREATE PROCEDURE [dbo].[ecGetCustomerInvoiceItems] 
	@ActorID int,
	@InvoiceID int
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecInvoice]
	WHERE
		[InvoiceID] = @InvoiceID;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID,@IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access the contract', 16, 1);
		RETURN;
	END
	--
	SET NOCOUNT ON;
	--
    SELECT
		* FROM [dbo].[ecInvoiceItems] AS [II]
	INNER JOIN
		[ecInvoice] AS [I] ON [I].[InvoiceID] = [II].[InvoiceID]
	WHERE
		[I].[InvoiceID] = @InvoiceID;

END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetCustomerInvoice]
	@ActorID int,
	@InvoiceID int
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecInvoice]
	WHERE
		[InvoiceID] = @InvoiceID;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access the contract', 16, 1);
		RETURN;
	END
	--
	SET NOCOUNT ON;
	--
	SELECT
		* FROM [dbo].[ContractsInvoicesDetailed]
	WHERE
		[InvoiceID] = @InvoiceID AND [ContractID] = @ContractID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecBulkServiceDelete]
	@ActorID int,
	@ContractID nvarchar(50),
	@SvcsXml xml,
	@Result int OUTPUT
AS
BEGIN
	DECLARE @IssuerID int;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE
		FROM [dbo].[ecService]
	WHERE
		[ServiceID] IN(SELECT [SXML].[Data].value('@id','int') FROM @SvcsXml.nodes('/Svcs/Svc') [SXML]([Data]));
	--
	SET @Result = 0;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecIsSupportedPluginActive]
	@ActorID int,
	@ResellerID int,
	@PluginID int,
	@Active bit OUTPUT
AS
BEGIN
	-- check user parent
	IF [dbo].[CheckUserParent](@ActorID, @ResellerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF EXISTS(SELECT * FROM [dbo].[ecPluginsProperties] 
		WHERE [ResellerID] = @ResellerID AND [PluginID] = @PluginID)
	BEGIN
		SET @Active = 1;
		RETURN;
	END

	SET @Active = 0;
	RETURN;
END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE VIEW [dbo].[ContractsServicesDetailed]
AS
SELECT dbo.ecService.ServiceID, ISNULL(dbo.Users.Username, dbo.ecContracts.AccountName) AS Username, dbo.ecContracts.CustomerID, dbo.ecService.ContractID, 
dbo.ecContracts.ResellerID, dbo.ecService.ServiceName, dbo.ecService.TypeID, dbo.ecService.Status, dbo.ecService.Created, dbo.ecService.Modified, 
dbo.ecService.ParentID
FROM dbo.ecContracts INNER JOIN 
dbo.ecService ON dbo.ecContracts.ContractID = dbo.ecService.ContractID LEFT OUTER JOIN 
dbo.Users ON dbo.ecContracts.CustomerID = dbo.Users.UserID
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


































CREATE PROCEDURE [dbo].[ecUpdateCategory] 
	@ActorID int,
	@UserID int,
	@CategoryID int,
	@CategoryName nvarchar(255),
	@CategorySku nvarchar(50),
	@ParentID int,
	@ShortDescription ntext,
	@FullDescription ntext,
	@Result int OUTPUT
AS
BEGIN
	-- check actor rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

	SET NOCOUNT ON;

	SET @Result = 0;

	DECLARE @Level int;

	IF @ParentID <= 0
		SET @ParentID = NULL;

	-- check whether a category exists
	IF NOT EXISTS(
		SELECT
			[CategoryName]
		FROM
			[dbo].[ecCategory]
		WHERE
			[CategoryID] = @CategoryID
			AND
			[ResellerID] = @UserID
	)
	BEGIN
		SET @Result = -1;
		RETURN;
	END

	-- check whether the update is correct
	IF @ParentID = @CategoryID
		BEGIN
			SET @Result = -1;
			RETURN;
		END

	-- check consistency: sub-categories won't include a parent category
	IF @ParentID IN (
		SELECT
			[CategoryID]
		FROM
			[dbo].[ecCategory]
		WHERE
			[ParentID] = @CategoryID
			AND
			[ResellerID] = @UserID
	)
	BEGIN
		SET @Result = -1;
		RETURN;
	END

	-- category level updates
	IF @ParentID = 0
		BEGIN
			SET @ParentID = NULL;
			SET @Level = 0;
		END
	ELSE
		BEGIN
			-- identify parent level
			SELECT
				@Level = [Level]
			FROM
				[dbo].[ecCategory]
			WHERE
				[CategoryID] = @ParentID
				AND
				[ResellerID] = @UserID;

			-- increase if necessary
			IF @Level >= 0
				SET @Level = @Level + 1;
			ELSE
				SET @Level = 0;
		END

	-- update a category
    UPDATE 
		[dbo].[ecCategory]
	SET 
		[CategoryName] = @CategoryName,
		[CategorySku] = @CategorySku,
		[ParentID] = @ParentID,
		[Level] = @Level,
		[ShortDescription] = @ShortDescription,
		[FullDescription] = @FullDescription,
		[Modified] = GETUTCDATE(),
		[ModifierID] = @ActorID
	WHERE 
		[CategoryID] = @CategoryID 
		AND 
		[ResellerID] = @UserID;

END











































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecUpdateBillingCycle]
	@ActorID int,
	@UserID int,
	@CycleID int,
	@CycleName nvarchar(255),
	@BillingPeriod nvarchar(50),
	@PeriodLength int,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- update cycle details
    UPDATE
		[dbo].[ecBillingCycles]
	SET
		[CycleName] = @CycleName,
		[BillingPeriod] = @BillingPeriod,
		[PeriodLength] = @PeriodLength
	WHERE
		[ResellerID] = @UserID
	AND
		[CycleID] = @CycleID;

	SET @Result = 0;
	RETURN;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecTopLevelDomainsCycles](
	[ProductID] [int] NOT NULL,
	[CycleID] [int] NOT NULL,
	[SetupFee] [money] NOT NULL,
	[RecurringFee] [money] NOT NULL,
	[TransferFee] [money] NULL,
	[SortOrder] [int] NOT NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecTopLevelDomains](
	[TopLevelDomain] [nvarchar](10) COLLATE Latin1_General_CI_AS NOT NULL,
	[ProductID] [int] NOT NULL,
	[PluginID] [int] NOT NULL,
	[ResellerID] [int] NOT NULL,
	[WhoisEnabled] [bit] NULL,
 CONSTRAINT [PK_ecTopLevelDomains] PRIMARY KEY CLUSTERED 
(
	[TopLevelDomain] ASC,
	[ResellerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecSetPaymentMethod]
	@ActorID int,
	@UserID int,
	@MethodName nvarchar(50),
	@DisplayName nvarchar(50),
	@PluginID int,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- cleanup
    DELETE FROM [dbo].[ecPaymentMethods]
	WHERE [ResellerID] = @UserID AND [MethodName] = @MethodName;
	-- add
	INSERT INTO [dbo].[ecPaymentMethods]
	(
		[ResellerID],
		[MethodName],
		[PluginID],
		[DisplayName],
		[SupportedItems]
	)
	SELECT
		@UserID,
		@MethodName,
		@PluginID,
		@DisplayName,
		[SupportedItems]
	FROM
		[dbo].[ecSupportedPlugins]
	WHERE
		[PluginID] = @PluginID;
	--
	SET @Result = 0;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE [dbo].[ecSetInvoiceItemProcessed]
	@InvoiceID int,
	@ItemID int,
	@Result int OUTPUT
AS
BEGIN

	SET NOCOUNT ON;

	SET @Result = 0;

    UPDATE
		[dbo].[ecInvoiceItems]
	SET
		[Processed] = 1
	WHERE
		[InvoiceID] = @InvoiceID
	AND
		[ItemID] = @ItemID;

END

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecHostingPackageSvcsCycles](
	[SvcCycleID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceID] [int] NOT NULL,
	[CycleName] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[BillingPeriod] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PeriodLength] [int] NOT NULL,
	[SetupFee] [money] NULL,
	[RecurringFee] [money] NOT NULL,
	[Currency] [nvarchar](3) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_ecPackagesSvcsCycles] PRIMARY KEY CLUSTERED 
(
	[SvcCycleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecHostingPackageSvcs](
	[ServiceID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[PlanID] [int] NOT NULL,
	[PackageID] [int] NULL,
	[UserRole] [int] NOT NULL,
	[InitialStatus] [int] NOT NULL,
	[SvcCycleID] [int] NOT NULL,
 CONSTRAINT [PK_ecPackagesSvcs] PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecHostingAddonSvcsCycles](
	[SvcCycleID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceID] [int] NOT NULL,
	[CycleName] [nvarchar](255) COLLATE Latin1_General_CI_AS NULL,
	[BillingPeriod] [nvarchar](50) COLLATE Latin1_General_CI_AS NULL,
	[PeriodLength] [int] NULL,
	[SetupFee] [money] NULL,
	[CyclePrice] [money] NOT NULL,
	[Currency] [nvarchar](3) COLLATE Latin1_General_CI_AS NOT NULL,
 CONSTRAINT [PK_ecAddonPackagesSvcsCycles] PRIMARY KEY CLUSTERED 
(
	[SvcCycleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecHostingAddonSvcs](
	[ServiceID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[PlanID] [int] NULL,
	[PackageAddonID] [int] NULL,
	[Quantity] [int] NOT NULL,
	[Recurring] [bit] NOT NULL,
	[DummyAddon] [bit] NOT NULL,
	[SvcCycleID] [int] NOT NULL,
 CONSTRAINT [PK_ecAddonPackagesSvcs] PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ecHostingAddonsCycles](
	[ProductID] [int] NOT NULL,
	[CycleID] [int] NOT NULL,
	[SetupFee] [money] NOT NULL,
	[RecurringFee] [money] NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_ecHostingAddonsCycles] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC,
	[CycleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO















CREATE PROCEDURE [dbo].[DeleteIPAddress]
(
	@AddressID int,
	@Result int OUTPUT
)
AS

SET @Result = 0

IF EXISTS(SELECT RecordID FROM GlobalDnsRecords WHERE IPAddressID = @AddressID)
BEGIN
	SET @Result = -1
	RETURN
END

IF EXISTS(SELECT AddressID FROM PackageIPAddresses WHERE AddressID = @AddressID AND ItemID IS NOT NULL)
BEGIN
	SET @Result = -2

	RETURN
END

-- delete package-IP relation
DELETE FROM PackageIPAddresses
WHERE AddressID = @AddressID

-- delete IP address
DELETE FROM IPAddresses
WHERE AddressID = @AddressID

RETURN


















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE GetGroupProviders
(
	@GroupID int
)
AS
SELECT
	PROV.ProviderID,
	PROV.GroupID,
	PROV.ProviderName,
	PROV.DisplayName,
	PROV.ProviderType,
	RG.GroupName + ' - ' + PROV.DisplayName AS ProviderName
FROM Providers AS PROV
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
WHERE RG.GroupID = @GroupId
ORDER BY RG.GroupOrder, PROV.DisplayName
RETURN





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecPaymentProfileExists]
	@ActorID int,
	@ContractID nvarchar(50),
	@Result bit OUTPUT
AS
BEGIN
	DECLARE @IssuerID int;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF EXISTS (SELECT [ContractID] FROM [dbo].[ecPaymentProfiles] WHERE [ContractID] = @ContractID)
	BEGIN
		SET @Result = 1;
		RETURN;
	END
	--
	SET @Result = 0;
END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetPackageQuotasForEdit
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @ServerID int, @ParentPackageID int, @PlanID int
SELECT @ServerID = ServerID, @ParentPackageID = ParentPackageID, @PlanID = PlanID FROM Packages
WHERE PackageID = @PackageID

-- get resource groups
SELECT
	RG.GroupID,
	RG.GroupName,
	ISNULL(PR.CalculateDiskSpace, ISNULL(HPR.CalculateDiskSpace, 0)) AS CalculateDiskSpace,
	ISNULL(PR.CalculateBandwidth, ISNULL(HPR.CalculateBandwidth, 0)) AS CalculateBandwidth,
	dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, @ServerID) AS Enabled,
	dbo.GetPackageAllocatedResource(@ParentPackageID, RG.GroupID, @ServerID) AS ParentEnabled
FROM ResourceGroups AS RG
LEFT OUTER JOIN PackageResources AS PR ON RG.GroupID = PR.GroupID AND PR.PackageID = @PackageID
LEFT OUTER JOIN HostingPlanResources AS HPR ON RG.GroupID = HPR.GroupID AND HPR.PlanID = @PlanID
ORDER BY RG.GroupOrder


-- return quotas
SELECT
	Q.QuotaID,
	Q.GroupID,
	Q.QuotaName,
	Q.QuotaDescription,
	Q.QuotaTypeID,
	CASE
		WHEN PQ.QuotaValue IS NULL THEN dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID)
		ELSE PQ.QuotaValue
	END QuotaValue,
	dbo.GetPackageAllocatedQuota(@ParentPackageID, Q.QuotaID) AS ParentQuotaValue
FROM Quotas AS Q
LEFT OUTER JOIN PackageQuotas AS PQ ON PQ.QuotaID = Q.QuotaID AND PQ.PackageID = @PackageID
ORDER BY Q.QuotaOrder

RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO





CREATE FUNCTION [dbo].[CalculateQuotaUsage]
(
	@PackageID int,
	@QuotaID int
)
RETURNS int
AS
BEGIN

DECLARE @QuotaTypeID int
SELECT @QuotaTypeID = QuotaTypeID FROM Quotas
WHERE QuotaID = @QuotaID

IF @QuotaTypeID <> 2
RETURN 0

DECLARE @Result int

IF @QuotaID = 52 -- diskspace
	SET @Result = dbo.CalculatePackageDiskspace(@PackageID)
ELSE IF @QuotaID = 51 -- bandwidth
	SET @Result = dbo.CalculatePackageBandwidth(@PackageID)
ELSE IF @QuotaID = 53 -- domains
	SET @Result = (SELECT COUNT(D.DomainID) FROM PackagesTreeCache AS PT
		INNER JOIN Domains AS D ON D.PackageID = PT.PackageID
		WHERE IsSubDomain = 0 AND IsInstantAlias = 0 AND IsDomainPointer = 0 AND PT.ParentPackageID = @PackageID)
ELSE IF @QuotaID = 54 -- sub-domains
	SET @Result = (SELECT COUNT(D.DomainID) FROM PackagesTreeCache AS PT
		INNER JOIN Domains AS D ON D.PackageID = PT.PackageID
		WHERE IsSubDomain = 1 AND IsInstantAlias = 0 AND PT.ParentPackageID = @PackageID)
ELSE IF @QuotaID = 220 -- domain pointers
	SET @Result = (SELECT COUNT(D.DomainID) FROM PackagesTreeCache AS PT
		INNER JOIN Domains AS D ON D.PackageID = PT.PackageID
		WHERE IsDomainPointer = 1 AND PT.ParentPackageID = @PackageID)
ELSE IF @QuotaID = 71 -- scheduled tasks
	SET @Result = (SELECT COUNT(S.ScheduleID) FROM PackagesTreeCache AS PT
		INNER JOIN Schedule AS S ON S.PackageID = PT.PackageID
		WHERE PT.ParentPackageID = @PackageID)
ELSE IF @QuotaID = 305 -- RAM of VPS
	SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
					INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
					INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
					WHERE SIP.PropertyName = 'RamSize' AND PT.ParentPackageID = @PackageID)
ELSE IF @QuotaID = 306 -- HDD of VPS
	SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
					INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
					INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
					WHERE SIP.PropertyName = 'HddSize' AND PT.ParentPackageID = @PackageID)
ELSE IF @QuotaID = 309 -- External IP addresses of VPS
	SET @Result = (SELECT COUNT(PIP.PackageAddressID) FROM PackageIPAddresses AS PIP
					INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
					INNER JOIN PackagesTreeCache AS PT ON PIP.PackageID = PT.PackageID
					WHERE PT.ParentPackageID = @PackageID AND IP.PoolID = 3)
ELSE IF @QuotaID = 100 -- Dedicated Web IP addresses
	SET @Result = (SELECT COUNT(PIP.PackageAddressID) FROM PackageIPAddresses AS PIP
					INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
					INNER JOIN PackagesTreeCache AS PT ON PIP.PackageID = PT.PackageID
					WHERE PT.ParentPackageID = @PackageID AND IP.PoolID = 2)
ELSE IF @QuotaID = 319 -- BB Users
	SET @Result = (SELECT COUNT(ea.AccountID) 
						FROM 
							ExchangeAccounts ea 
						INNER JOIN 
							BlackBerryUsers bu 
						ON 
							ea.AccountID = bu.AccountID
						INNER JOIN 
							ServiceItems  si 
						ON 
							ea.ItemID = si.ItemID
						INNER JOIN 
							PackagesTreeCache pt ON si.PackageID = pt.PackageID
						WHERE 
							pt.ParentPackageID = @PackageID)
ELSE
	SET @Result = (SELECT COUNT(SI.ItemID) FROM Quotas AS Q
	INNER JOIN ServiceItems AS SI ON SI.ItemTypeID = Q.ItemTypeID
	INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID AND PT.ParentPackageID = @PackageID
	WHERE Q.QuotaID = @QuotaID)

RETURN @Result
END










GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetPackageQuotas
(
	@ActorID int,
	@PackageID int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @PlanID int, @ParentPackageID int
SELECT @PlanID = PlanID, @ParentPackageID = ParentPackageID FROM Packages
WHERE PackageID = @PackageID

-- get resource groups
SELECT
	RG.GroupID,
	RG.GroupName,
	ISNULL(HPR.CalculateDiskSpace, 0) AS CalculateDiskSpace,
	ISNULL(HPR.CalculateBandwidth, 0) AS CalculateBandwidth,
	dbo.GetPackageAllocatedResource(@ParentPackageID, RG.GroupID, 0) AS ParentEnabled
FROM ResourceGroups AS RG
LEFT OUTER JOIN HostingPlanResources AS HPR ON RG.GroupID = HPR.GroupID AND HPR.PlanID = @PlanID
WHERE dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, 0) = 1
ORDER BY RG.GroupOrder


-- return quotas
SELECT
	Q.QuotaID,
	Q.GroupID,
	Q.QuotaName,
	Q.QuotaDescription,
	Q.QuotaTypeID,
	dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID) AS QuotaValue,
	dbo.GetPackageAllocatedQuota(@ParentPackageID, Q.QuotaID) AS ParentQuotaValue,
	ISNULL(dbo.CalculateQuotaUsage(@PackageID, Q.QuotaID), 0) AS QuotaUsedValue
FROM Quotas AS Q
ORDER BY Q.QuotaOrder
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE [dbo].[GetPackageQuota]
(
	@ActorID int,
	@PackageID int,
	@QuotaName nvarchar(50)
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

-- return quota
SELECT
	Q.QuotaID,
	Q.QuotaName,
	Q.QuotaDescription,
	Q.QuotaTypeID,
	ISNULL(dbo.GetPackageAllocatedQuota(@PackageId, Q.QuotaID), 0) AS QuotaAllocatedValue,
	ISNULL(dbo.CalculateQuotaUsage(@PackageId, Q.QuotaID), 0) AS QuotaUsedValue
FROM Quotas AS Q
WHERE Q.QuotaName = @QuotaName

RETURN





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetHostingPlanQuotas
(
	@ActorID int,
	@PlanID int,
	@PackageID int,
	@ServerID int
)
AS

-- check rights
IF dbo.CheckActorParentPackageRights(@ActorID, @PackageID) = 0
RAISERROR('You are not allowed to access this package', 16, 1)

DECLARE @IsAddon bit

IF @ServerID = 0
SELECT @ServerID = ServerID FROM Packages
WHERE PackageID = @PackageID

-- get resource groups
SELECT
	RG.GroupID,
	RG.GroupName,
	CASE
		WHEN HPR.CalculateDiskSpace IS NULL THEN CAST(0 as bit)
		ELSE CAST(1 as bit)
	END AS Enabled,
	dbo.GetPackageAllocatedResource(@PackageID, RG.GroupID, @ServerID) AS ParentEnabled,
	ISNULL(HPR.CalculateDiskSpace, 1) AS CalculateDiskSpace,
	ISNULL(HPR.CalculateBandwidth, 1) AS CalculateBandwidth
FROM ResourceGroups AS RG
LEFT OUTER JOIN HostingPlanResources AS HPR ON RG.GroupID = HPR.GroupID AND HPR.PlanID = @PlanID
ORDER BY RG.GroupOrder

-- get quotas by groups
SELECT
	Q.QuotaID,
	Q.GroupID,
	Q.QuotaName,
	Q.QuotaDescription,
	Q.QuotaTypeID,
	ISNULL(HPQ.QuotaValue, 0) AS QuotaValue,
	dbo.GetPackageAllocatedQuota(@PackageID, Q.QuotaID) AS ParentQuotaValue
FROM Quotas AS Q
LEFT OUTER JOIN HostingPlanQuotas AS HPQ ON Q.QuotaID = HPQ.QuotaID AND HPQ.PlanID = @PlanID
ORDER BY Q.QuotaOrder
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[SearchServiceItemsPaged]
(
	@ActorID int,
	@UserID int,
	@ItemTypeID int,
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int
)
AS


-- check rights
IF dbo.CheckActorUserRights(@ActorID, @UserID) = 0
RAISERROR('You are not allowed to access this account', 16, 1)

-- build query and run it to the temporary table
DECLARE @sql nvarchar(2000)

IF @ItemTypeID <> 13
BEGIN
	SET @sql = '
	DECLARE @EndRow int
	SET @EndRow = @StartRow + @MaximumRows
	DECLARE @Items TABLE
	(
		ItemPosition int IDENTITY(1,1),
		ItemID int
	)
	INSERT INTO @Items (ItemID)
	SELECT
		SI.ItemID
	FROM ServiceItems AS SI
	INNER JOIN Packages AS P ON P.PackageID = SI.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	WHERE
		dbo.CheckUserParent(@UserID, P.UserID) = 1
		AND SI.ItemTypeID = @ItemTypeID
	'

	IF @FilterValue <> ''
	SET @sql = @sql + ' AND SI.ItemName LIKE @FilterValue '

	IF @SortColumn = '' OR @SortColumn IS NULL
	SET @SortColumn = 'ItemName'

	SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

	SET @sql = @sql + ' SELECT COUNT(ItemID) FROM @Items;
	SELECT
		
		SI.ItemID,
		SI.ItemName,

		P.PackageID,
		P.PackageName,
		P.StatusID,
		P.PurchaseDate,
		
		-- user
		P.UserID,
		U.Username,
		U.FirstName,
		U.LastName,
		U.FullName,
		U.RoleID,
		U.Email
	FROM @Items AS I
	INNER JOIN ServiceItems AS SI ON I.ItemID = SI.ItemID
	INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	WHERE I.ItemPosition BETWEEN @StartRow AND @EndRow'
END
ELSE
BEGIN

	SET @SortColumn = REPLACE(@SortColumn, 'ItemName', 'DomainName')
	
	SET @sql = '
	DECLARE @EndRow int
	SET @EndRow = @StartRow + @MaximumRows
	DECLARE @Items TABLE
	(
		ItemPosition int IDENTITY(1,1),
		ItemID int
	)
	INSERT INTO @Items (ItemID)
	SELECT
		D.DomainID
	FROM Domains AS D
	INNER JOIN Packages AS P ON P.PackageID = D.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	WHERE
		dbo.CheckUserParent(@UserID, P.UserID) = 1
	'

	IF @FilterValue <> ''
	SET @sql = @sql + ' AND D.DomainName LIKE @FilterValue '

	IF @SortColumn = '' OR @SortColumn IS NULL
	SET @SortColumn = 'DomainName'

	SET @sql = @sql + ' ORDER BY ' + @SortColumn + ' '

	SET @sql = @sql + ' SELECT COUNT(ItemID) FROM @Items;
	SELECT
		
		D.DomainID AS ItemID,
		D.DomainName AS ItemName,

		P.PackageID,
		P.PackageName,
		P.StatusID,
		P.PurchaseDate,
		
		-- user
		P.UserID,
		U.Username,
		U.FirstName,
		U.LastName,
		U.FullName,
		U.RoleID,
		U.Email
	FROM @Items AS I
	INNER JOIN Domains AS D ON I.ItemID = D.DomainID
	INNER JOIN Packages AS P ON D.PackageID = P.PackageID
	INNER JOIN UsersDetailed AS U ON P.UserID = U.UserID
	WHERE I.ItemPosition BETWEEN @StartRow AND @EndRow'
END

exec sp_executesql @sql, N'@StartRow int, @MaximumRows int, @UserID int, @FilterValue nvarchar(50), @ItemTypeID int, @ActorID int',
@StartRow, @MaximumRows, @UserID, @FilterValue, @ItemTypeID, @ActorID

RETURN































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceProperties](
	[ServiceID] [int] NOT NULL,
	[PropertyName] [nvarchar](50) COLLATE Latin1_General_CI_AS NOT NULL,
	[PropertyValue] [nvarchar](1000) COLLATE Latin1_General_CI_AS NULL,
 CONSTRAINT [PK_ServiceProperties_1] PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC,
	[PropertyName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE AddService
(
	@ServiceID int OUTPUT,
	@ServerID int,
	@ProviderID int,
	@ServiceQuotaValue int,
	@ServiceName nvarchar(50),
	@ClusterID int,
	@Comments ntext
)
AS
BEGIN

BEGIN TRAN
IF @ClusterID = 0 SET @ClusterID = NULL

INSERT INTO Services
(
	ServerID,
	ProviderID,
	ServiceName,
	ServiceQuotaValue,
	ClusterID,
	Comments
)
VALUES
(
	@ServerID,
	@ProviderID,
	@ServiceName,
	@ServiceQuotaValue,
	@ClusterID,
	@Comments
)

SET @ServiceID = SCOPE_IDENTITY()

-- copy default service settings
INSERT INTO ServiceProperties (ServiceID, PropertyName, PropertyValue)
SELECT @ServiceID, PropertyName, PropertyValue
FROM ServiceDefaultProperties
WHERE ProviderID = @ProviderID

-- copy all default DNS records for the given service
DECLARE @GroupID int
SELECT @GroupID = GroupID FROM Providers
WHERE ProviderID = @ProviderID

-- default IP address for added records
DECLARE @AddressID int
SELECT TOP 1 @AddressID = AddressID FROM IPAddresses
WHERE ServerID = @ServerID

INSERT INTO GlobalDnsRecords
(
	RecordType,
	RecordName,
	RecordData,
	MXPriority,
	IPAddressID,
	ServiceID,
	ServerID,
	PackageID
)
SELECT
	RecordType,
	RecordName,
	CASE WHEN RecordData = '[ip]' THEN ''
	ELSE RecordData END,
	MXPriority,
	CASE WHEN RecordData = '[ip]' THEN @AddressID
	ELSE NULL END,
	@ServiceID,
	NULL, -- server
	NULL -- package
FROM
	ResourceGroupDnsRecords
WHERE GroupID = @GroupID
ORDER BY RecordOrder
COMMIT TRAN

END
RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[GetIPAddress]	
(
	@AddressID int
)
AS
BEGIN

	-- select
	SELECT
		AddressID,
		ServerID,
		ExternalIP,
		InternalIP,
		PoolID,
		SubnetMask,
		DefaultGateway,
		Comments
	FROM IPAddresses
	WHERE
		AddressID = @AddressID

	RETURN 
END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





































CREATE PROCEDURE [dbo].[ecDeleteCategory] 
	@ActorID int,
	@UserID int,
	@CategoryID int,
	@Result int OUTPUT
AS
BEGIN
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

	SET NOCOUNT ON;

	SET @Result = 0;

	-- check whether the category doesn't empty 
    IF EXISTS(
		SELECT
			[ProductID]
		FROM
			[dbo].[ecProductCategories]
		WHERE
			[CategoryID] = @CategoryID
			AND
			[ResellerID] = @UserID
	)
	BEGIN
		SET @Result = -1;
		RETURN;
	END

	-- check if category has a sub-categories
	IF EXISTS(
		SELECT
			[CategoryID]
		FROM
			[dbo].[ecCategory]
		WHERE
			[ParentID] = @CategoryID
			AND
			[ResellerID] = @UserID
	)
	BEGIN
		SET @Result = -2;
		RETURN;
	END

	-- delete a category
	DELETE FROM
		[dbo].[ecCategory]
	WHERE
		[CategoryID] = @CategoryID 
		AND 
		[ResellerID] = @UserID;

END









































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecUpdateHostingAddon]
	@ActorID int,
	@UserID int,
	@ProductID int,
	@AddonName nvarchar(255),
	@ProductSku nvarchar(50),
	@TaxInclusive bit,
	@Enabled bit,
	@PlanID int,
	@Recurring bit,
	@DummyAddon bit,
	@Countable bit,
	@Description ntext,
	@AddonCyclesXml xml,
	@AssignedProductsXml xml,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- dummy addon clause
	IF @DummyAddon = 1
		SET @PlanID = NULL;

BEGIN TRAN UPDATE_ADDON
	-- update product first
	UPDATE [dbo].[ecProduct]
	SET
		[ProductName] = @AddonName,
		[ProductSKU] = @ProductSku,
		[Description] = @Description,
		[Enabled] = @Enabled,
		[TaxInclusive] = @TaxInclusive
	WHERE
		[ProductID] = @ProductID
	AND
		[ResellerID] = @UserID;

	-- update hosting addon details
	UPDATE [dbo].[ecHostingAddons]
	SET
		[PlanID] = @PlanID,
		[Recurring] = @Recurring,
		[DummyAddon] = @DummyAddon,
		[Countable] = @Countable
	WHERE
		[ProductID] = @ProductID
	AND
		[ResellerID] = @UserID;

	-- check errors
	IF @@ERROR <> 0
	BEGIN
		GOTO ERROR_HANDLE;
	END
/*
XML Format:

<PlanCycles>
	<Cycle ID="2" SetupFee="2.99" RecurringFee="3.99" TCOID="1005" SortOrder="1" />
</PlanCycles>
*/
	-- cleanup hosting addon cycles first
	DELETE FROM [dbo].[ecHostingAddonsCycles] WHERE [ProductID] = @ProductID;
	
	IF @Recurring = 1
		-- insert cycles
		INSERT INTO [dbo].[ecHostingAddonsCycles]
			([ProductID], [CycleID], [SetupFee], [RecurringFee], [SortOrder])
		SELECT
			@ProductID,[SXML].[Data].value('@ID','int'),[SXML].[Data].value('@SetupFee','money'),
			[SXML].[Data].value('@RecurringFee','money'), [SXML].[Data].value('@SortOrder','int')
		FROM @AddonCyclesXml.nodes('/PlanCycles/Cycle') [SXML]([Data]);
		-- check errors
		IF @@ERROR <> 0
			GOTO ERROR_HANDLE;
	ELSE
		UPDATE
			[dbo].[ecHostingAddons]
		SET
			[SetupFee] = [SXML].[Data].value('@SetupFee','money'),
			[OneTimeFee] = [SXML].[Data].value('@OneTimeFee','money')
		FROM @AddonCyclesXml.nodes('/PlanCycles/Cycle') [SXML]([Data])
		WHERE
			[ResellerID] = @UserID
		AND
			[ProductID] = @ProductID;

	-- check errors
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;

/*
XML Format:

<AssignedProducts>
	<Product ID="25" />
</AssignedProducts>
*/
	-- cleanup addon products first
	DELETE FROM [dbo].[ecAddonProducts] WHERE [AddonID] = @ProductID AND [ResellerID] = @UserID;
	-- insert cycles
	INSERT INTO [dbo].[ecAddonProducts]
		([AddonID], [ProductID], [ResellerID])
	SELECT
		@ProductID,[SXML].[Data].value('@ID','int'),@UserID
	FROM @AssignedProductsXml.nodes('/AssignedProducts/Product') [SXML]([Data]);
	-- check errors
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;	

	-- set result ok
	SET @Result = 0;
	--
	COMMIT TRAN UPDATE_ADDON;
	-- 
	RETURN;
	

ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN UPDATE_ADDON;
	RETURN;
		
END
END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecUpdateDomainNameSvc]
	@ActorID int,
	@ServiceID int,
	@ProductID int,
	@Status int,
	@DomainID int,
	@FQDN nvarchar(64),
	@PropertyNames ntext,
	@PropertyValues ntext,
	@Result int OUTPUT
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecService]
	WHERE
		[ServiceID] = @ServiceID;
	SELECT
		@IssuerID = [ResellerID] FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this action', 16, 1);
		RETURN;
	END

BEGIN TRAN UPD_TLD_SVC
	-- update tld svc
	UPDATE
		[dbo].[ecService]
	SET
		[ServiceName] = @FQDN,
		[Status] = @Status,
		[Modified] = GETDATE()
	WHERE
		[ServiceID] = @ServiceID;

	-- check error
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;
	-- update tld svc
	IF @DomainID < 1
		SET @DomainID = NULL;
	--
	UPDATE
		[dbo].[ecDomainSvcs]
	SET
		[ProductID] = @ProductID,
		[DomainID] = @DomainID,
		[FQDN] = @FQDN,
		[PropertyNames] = @PropertyNames,
		[PropertyValues] = @PropertyValues
	WHERE
		[ServiceID] = @ServiceID;
	-- check error
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;

	-- set result ok
	SET @Result = 0;
	-- commit changes
	COMMIT TRAN UPD_TLD_SVC;
	-- exit
	RETURN;

-- error handler
ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN UPD_TLD_SVC;
	RETURN;
END

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecChangeHostingPlanSvcCycle]
	@ActorID int,
	@ServiceID int,
	@ProductID int,
	@CycleID int,
	@Currency nvarchar(3),
	@Result int OUTPUT
AS
BEGIN
	DECLARE @ResellerID int, @CustomerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecService]
	WHERE
		[ServiceID] = @ServiceID;
	SELECT
		@CustomerID = [CustomerID], @ResellerID = [ResellerID] FROM [ecContracts]
	WHERE
		[ContractID] = @ContractID;

	-- check actor user rights
	IF [dbo].[CanUpdateUserDetails](@ActorID, @CustomerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this action', 16, 1);
		RETURN;
	END

BEGIN TRAN CHNG_SVC_CYCLE
	-- insert svc life-cycle
	INSERT INTO [dbo].[ecHostingPackageSvcsCycles]
	(
		[ServiceID],
		[CycleName],
		[BillingPeriod],
		[PeriodLength],
		[SetupFee],
		[RecurringFee],
		[Currency]
	)
	SELECT
		@Result,
		[BC].[CycleName],
		[BC].[BillingPeriod],
		[BC].[PeriodLength],
		[HPBC].[SetupFee],
		[HPBC].[RecurringFee],
		@Currency
	FROM
		[dbo].[ecHostingPlansBillingCycles] AS [HPBC]
	INNER JOIN
		[dbo].[ecBillingCycles] AS [BC]
	ON
		[BC].[CycleID] = [HPBC].[CycleID]
	WHERE
		[HPBC].[ProductID] = @ProductID
	AND
		[BC].[ResellerID] = @ResellerID;
	-- check error
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;
	-- obtain result
	SET @Result = SCOPE_IDENTITY();
	
	-- update service
	UPDATE
		[dbo].[ecHostingPackageSvcs]
	SET
		[SvcCycleID] = @Result
	WHERE
		[ServiceID] = @ServiceID;
	-- check error
	IF @@ERROR <> 0 OR @@ROWCOUNT = 0
		GOTO ERROR_HANDLE;

	-- commit changes
	COMMIT TRAN CHNG_SVC_CYCLE;
	-- exit
	RETURN;
		
-- error handler
ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN CHNG_SVC_CYCLE;
	RETURN;
END

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO











CREATE PROCEDURE [dbo].[GetUnallottedIPAddresses]	
	@PackageID int,
	@ServiceID int,
	@PoolID int = 0
AS
BEGIN

	DECLARE @ParentPackageID int
	DECLARE @ServerID int
	
	SELECT
		@ParentPackageID = ParentPackageID,
		@ServerID = ServerID
	FROM Packages
	WHERE PackageID = @PackageId

    IF (@ParentPackageID = 1 OR @PoolID = 4 /* management network */) -- "System" space
    BEGIN		
		-- check if server is physical
		IF EXISTS(SELECT * FROM Servers WHERE ServerID = @ServerID AND VirtualServer = 0)
		BEGIN
			-- physical server
			SELECT
				IP.AddressID,
				IP.ExternalIP,
				IP.InternalIP,
				IP.ServerID,
				IP.PoolID,
				IP.SubnetMask,
				IP.DefaultGateway
			FROM dbo.IPAddresses AS IP
			WHERE
				IP.ServerID = @ServerID
				AND IP.AddressID NOT IN (SELECT PIP.AddressID FROM dbo.PackageIPAddresses AS PIP)
				AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
			ORDER BY IP.DefaultGateway, IP.ExternalIP
		END
		ELSE
		BEGIN
			-- virtual server
			-- get resource group by service
			DECLARE @GroupID int
			SELECT @GroupID = P.GroupID FROM Services AS S
			INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
			WHERE S.ServiceID = @ServiceID
			
			SELECT
				IP.AddressID,
				IP.ExternalIP,
				IP.InternalIP,
				IP.ServerID,
				IP.PoolID,
				IP.SubnetMask,
				IP.DefaultGateway
			FROM dbo.IPAddresses AS IP
			WHERE
				IP.ServerID IN (
					SELECT SVC.ServerID FROM VirtualServices AS VS
					INNER JOIN Services AS SVC ON VS.ServiceID = SVC.ServiceID
					INNER JOIN Providers AS P ON SVC.ProviderID = P.ProviderID
					WHERE VS.ServerID = @ServerID AND P.GroupID = @GroupID
				)
				AND IP.AddressID NOT IN (SELECT PIP.AddressID FROM dbo.PackageIPAddresses AS PIP)
				AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
			ORDER BY IP.DefaultGateway, IP.ExternalIP
		END
	END
	ELSE -- 2rd level space and below
	BEGIN
		-- get service location
		SELECT @ServerID = S.ServerID FROM Services AS S
		WHERE S.ServiceID = @ServiceID
	
		SELECT
			IP.AddressID,
			IP.ExternalIP,
			IP.InternalIP,
			IP.ServerID,
			IP.PoolID,
			IP.SubnetMask,
			IP.DefaultGateway
		FROM dbo.PackageIPAddresses AS PIP
		INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
		WHERE
			PIP.PackageID = @ParentPackageID
			AND PIP.ItemID IS NULL
			AND (@PoolID = 0 OR @PoolID <> 0 AND IP.PoolID = @PoolID)
			AND IP.ServerID = @ServerID
		ORDER BY IP.DefaultGateway, IP.ExternalIP
	END
END














GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE DeleteService
(
	@ServiceID int,
	@Result int OUTPUT
)
AS

SET @Result = 0

-- check related service items
IF EXISTS (SELECT ItemID FROM ServiceItems WHERE ServiceID = @ServiceID)
BEGIN
	SET @Result = -1
	RETURN
END

IF EXISTS (SELECT ServiceID FROM VirtualServices WHERE ServiceID = @ServiceID)
BEGIN
	SET @Result = -2
	RETURN
END

BEGIN TRAN
-- delete global DNS records
DELETE FROM GlobalDnsRecords
WHERE ServiceID = @ServiceID

-- delete service
DELETE FROM Services
WHERE ServiceID = @ServiceID

COMMIT TRAN

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE DeleteServer
(
	@ServerID int,
	@Result int OUTPUT
)
AS
SET @Result = 0

-- check related services
IF EXISTS (SELECT ServiceID FROM Services WHERE ServerID = @ServerID)
BEGIN
	SET @Result = -1
	RETURN
END

-- check related packages
IF EXISTS (SELECT PackageID FROM Packages WHERE ServerID = @ServerID)
BEGIN
	SET @Result = -2
	RETURN
END

-- check related hosting plans
IF EXISTS (SELECT PlanID FROM HostingPlans WHERE ServerID = @ServerID)
BEGIN
	SET @Result = -3
	RETURN
END

BEGIN TRAN

-- delete IP addresses
DELETE FROM IPAddresses
WHERE ServerID = @ServerID

-- delete global DNS records
DELETE FROM GlobalDnsRecords
WHERE ServerID = @ServerID

-- delete server
DELETE FROM Servers
WHERE ServerID = @ServerID

-- delete virtual services if any
DELETE FROM VirtualServices
WHERE ServerID = @ServerID
COMMIT TRAN

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetHostingAddonSvc]
	@ActorID int,
	@ServiceID int
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecService]
	WHERE
		[ServiceID] = @ServiceID;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		[SVC].*,
		[HASVC].*,
		[HASC].[CycleName],
		[HASC].[BillingPeriod],
		[HASC].[PeriodLength],
		[HASC].[SetupFee],
		[HASC].[CyclePrice],
		[HASC].[Currency]
	FROM
		[dbo].[ecService] AS [SVC]
	INNER JOIN
		[dbo].[ecHostingAddonSvcs] AS [HASVC]
	ON
		[HASVC].[ServiceID] = [SVC].[ServiceID]
	INNER JOIN
		[dbo].[ecHostingAddonSvcsCycles] AS [HASC]
	ON
		[HASC].[SvcCycleID] = [HASVC].[SvcCycleID]
	WHERE
		[SVC].[ServiceID] = @ServiceID;
END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetHostingAddonsTaken]
	@ActorID int,
	@UserID int
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

    SELECT [PlanID] FROM [dbo].[ecHostingAddons] WHERE [ResellerID] = @UserID;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetHostingAddonCycles]
	@UserID int,
	@ProductID int
AS
BEGIN

	SELECT
		[BC].*, [HAC].[ProductID], [HAC].[SetupFee], [HAC].[RecurringFee] FROM [dbo].[ecHostingAddonsCycles] AS [HAC]
	INNER JOIN
		[dbo].[ecBillingCycles] AS [BC] ON [BC].[CycleID] = [HAC].[CycleID]
	INNER JOIN
		[dbo].[ecProduct] AS [PR] ON [HAC].[ProductID] = [PR].[ProductID]
	WHERE
		[PR].[ResellerID] = @UserID AND [HAC].[ProductID] = @ProductID
	ORDER BY
		[HAC].[SortOrder];

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetHostingAddon]
	@UserID int,
	@ProductID int
AS
BEGIN

	SELECT
		[P].*,
		[HA].*,
		[HA].[2COID] AS [TCOID]
	FROM
		[dbo].[ecHostingAddons] AS [HA]
	INNER JOIN
		[dbo].[ecProduct] AS [P]
	ON
		[P].[ProductID] = [HA].[ProductID]
	WHERE
		[HA].[ResellerID] = @UserID
	AND
		[HA].[ProductID] = @ProductID;

END

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetDomainNameSvcHistory] 
	@ActorID int,
	@ServiceID int
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecService]
	WHERE
		[ServiceID] = @ServiceID;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		[DNC].[CycleName],
		[DNC].[BillingPeriod],
		[DNC].[PeriodLength],
		[DNC].[SetupFee],
		[DNC].[RecurringFee],
		[DNC].[Currency],
		[SUL].[StartDate],
		[SUL].[EndDate]
	FROM
		[dbo].[ecDomainSvcsCycles] AS [DNC]
	INNER JOIN
		[dbo].[ecSvcsUsageLog] AS [SUL]
	ON
		[SUL].[ServiceID] = [DNC].[ServiceID]
	AND
		[SUL].[SvcCycleID] = [DNC].[SvcCycleID]
	WHERE
		[DNC].[ServiceID] = @ServiceID;
END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetDomainNameSvc]
	@ActorID int,
	@ServiceID int
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecService]
	WHERE
		[ServiceID] = @ServiceID;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		[SVC].*,
		[DMSVC].[ProductID],
		[DMSVC].[DomainID],
		[DMSVC].[PluginID],
		[DMSVC].[FQDN],
		[DMSVC].[PropertyNames],
		[DMSVC].[PropertyValues],
		[DMSVC].[SvcCycleID],
		[SPS].[DisplayName] AS [ProviderName],
		[DMSC].[CycleName],
		[DMSC].[BillingPeriod],
		[DMSC].[PeriodLength],
		[DMSC].[SetupFee],
		[DMSC].[RecurringFee],
		[DMSC].[Currency]
	FROM
		[dbo].[ecService] AS [SVC]
	INNER JOIN
		[dbo].[ecDomainSvcs] AS [DMSVC]
	ON
		[DMSVC].[ServiceID] = [SVC].[ServiceID]
	LEFT JOIN
		[dbo].[ecSupportedPlugins] AS [SPS]
	ON
		[SPS].[PluginID] = [DMSVC].[PluginID]
	LEFT JOIN
		[dbo].[ecDomainSvcsCycles] AS [DMSC]
	ON
		[DMSC].[SvcCycleID] = [DMSVC].[SvcCycleID]
	WHERE
		[SVC].[ServiceID] = @ServiceID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetCustomersServicesPaged]
	@ActorID int,
	@UserID int,
	@IsReseller bit,
	@MaximumRows int,
	@StartRowIndex int
AS
BEGIN
	-- check user parent
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END

	SET NOCOUNT ON;

	DECLARE @EndIndex int;

	SET @EndIndex = @MaximumRows + @StartRowIndex;
	SET @StartRowIndex = @StartRowIndex + 1;

	IF @IsReseller = 1
	BEGIN
		WITH [SERVICES] AS (
			SELECT
				ROW_NUMBER() OVER(ORDER BY [Created] DESC) AS [RowIndex], * FROM [dbo].[ContractsServicesDetailed]
			WHERE
				[ResellerID] = @UserID
		)

		SELECT
			* FROM [SERVICES]
		WHERE
			[RowIndex] BETWEEN @StartRowIndex AND @EndIndex ORDER BY [Created] DESC;
		-- exit
		RETURN;
	END;
	
	WITH [SERVICES] AS (
		SELECT
			ROW_NUMBER() OVER(ORDER BY [Created] DESC) AS [RowIndex], * FROM [dbo].[ContractsServicesDetailed]
		WHERE
			[CustomerID] = @UserID
	)

	SELECT
		* FROM [SERVICES]
	WHERE
		[RowIndex] BETWEEN @StartRowIndex AND @EndIndex ORDER BY [Created] DESC;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetCustomersServicesCount]
	@ActorID int,
	@UserID int,
	@IsReseller bit,
	@Result int OUTPUT
AS
BEGIN
	-- check user parent
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END

	SET NOCOUNT ON;

	IF @IsReseller = 1
	BEGIN
		SELECT
			@Result = COUNT([ServiceID]) FROM [dbo].[ContractsServicesDetailed]
		WHERE
			[ResellerID] = @UserID;
		RETURN;
	END
	
	SELECT
		@Result = COUNT([ServiceID]) FROM [dbo].[ContractsServicesDetailed]
	WHERE
		[CustomerID] = @UserID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[UpdateIPAddresses]
(
	@xml ntext,
	@PoolID int,
	@ServerID int,
	@SubnetMask varchar(15),
	@DefaultGateway varchar(15),
	@Comments ntext
)
AS
BEGIN
	SET NOCOUNT ON;

	IF @ServerID = 0
	SET @ServerID = NULL

	DECLARE @idoc int
	--Create an internal representation of the XML document.
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

	-- update
	UPDATE IPAddresses SET
		ServerID = @ServerID,
		PoolID = @PoolID,
		SubnetMask = @SubnetMask,
		DefaultGateway = @DefaultGateway,
		Comments = @Comments
	FROM IPAddresses AS IP
	INNER JOIN OPENXML(@idoc, '/items/item', 1) WITH 
	(
		AddressID int '@id'
	) as PV ON IP.AddressID = PV.AddressID

	-- remove document
	exec sp_xml_removedocument @idoc
END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[UpdateIPAddress]	
(
	@AddressID int,
	@ServerID int,
	@ExternalIP varchar(24),
	@InternalIP varchar(24),
	@PoolID int,
	@SubnetMask varchar(15),
	@DefaultGateway varchar(15),
	@Comments ntext
)
AS
BEGIN
	IF @ServerID = 0
	SET @ServerID = NULL

	UPDATE IPAddresses SET
		ExternalIP = @ExternalIP,
		InternalIP = @InternalIP,
		ServerID = @ServerID,
		PoolID = @PoolID,
		SubnetMask = @SubnetMask,
		DefaultGateway = @DefaultGateway,
		Comments = @Comments
	WHERE AddressID = @AddressID

	RETURN
END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecSetSvcsUsageRecordsClosed]
	@ActorID int,
	@XmlSvcs xml,
	@Result int OUTPUT
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- result is ok
	SET @Result = 0;
	-- update all svc records
	UPDATE
		[dbo].[ecSvcsUsageLog]
	SET
		[PeriodClosed] = 1
	WHERE
		[ServiceID] IN (
			SELECT [SXML].[Data].value('@id','int') FROM @XmlSvcs.nodes('/records/record') [SXML]([Data]));

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetStorefrontProductsInCategory]
	@ResellerID int,
	@CategoryID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		[P].*
	FROM
		[dbo].[ecProduct] AS [P]
	INNER JOIN
		[dbo].[ecProductCategories] AS [PC]
	ON
		[P].[ProductID] = [PC].[ProductID]
	WHERE
		[PC].[CategoryID] = @CategoryID
	AND
		[P].[ResellerID] = @ResellerID
	AND
		[P].[Enabled] = 1;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetStorefrontHostingPlanAddons]
	@ResellerID int,
	@PlanID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		[P].*,
		[HA].*,
		[HA].[2COID] AS [TCOID]
	FROM
		[dbo].[ecHostingAddons] AS [HA]
	INNER JOIN
		[dbo].[ecProduct] AS [P]
	ON
		[P].[ProductID] = [HA].[ProductID]
	INNER JOIN
		[dbo].[ecAddonProducts] AS [AP]
	ON
		[P].[ProductID] = [AP].[AddonID]
	WHERE
		[AP].[ResellerID] = @ResellerID
	AND
		[P].[Enabled] = 1
	AND
		[AP].[ProductID] = @PlanID;

END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetServiceHandlersResponsesByReseller]
	@ResellerID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
		* FROM [ServiceHandlersResponsesDetailed]
	WHERE
		[ResellerID] = @ResellerID AND [ErrorMessage] IS NULL
END























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


























CREATE PROCEDURE [dbo].[ecGetResellerTopLevelDomain]
	@ResellerID int,
	@TLD nvarchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		[P].*,
		[Tlds].[PluginID],
		[Tlds].[WhoisEnabled]
	FROM
		[dbo].[ecProduct] AS [P]
	INNER JOIN
		[dbo].[ecTopLevelDomains] AS [Tlds]
	ON
		[Tlds].[ProductID] = [P].[ProductID]
	WHERE
		[P].[ResellerID] = @ResellerID
	AND
		[Tlds].[TopLevelDomain] = @TLD;

END





























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetServiceSuspendDate]
	@ActorID int,
	@ServiceID int,
	@SuspendDate datetime OUTPUT
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecService]
	WHERE
		[ServiceID] = @ServiceID;
	SELECT
		@IssuerID = [ResellerID] FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END

	SET NOCOUNT ON;

    SELECT @SuspendDate = ISNULL(MAX([EndDate]), GETDATE()) FROM [dbo].[ecSvcsUsageLog]
		WHERE [ServiceID] = @ServiceID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetServicesToInvoice] 
	@ActorID int,
	@ResellerID int,
	@TodayDate datetime,
	@DaysOffset int
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @ResellerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END

	SET NOCOUNT ON;

	DECLARE @Svcs TABLE(
		[ServiceID] int NOT NULL,
		[MaxStartDate] datetime NOT NULL,
		[MaxEndDate] datetime NOT NULL
	);

	-- filter service that don't have corresponding unpaid invoice and expired
	INSERT INTO
		@Svcs ([ServiceID], [MaxStartDate], [MaxEndDate])
	SELECT
		[SUL].[ServiceID], MAX([SUL].[StartDate]), MAX([SUL].[EndDate]) FROM [dbo].[ecSvcsUsageLog] AS [SUL]
	INNER JOIN
		[dbo].[ecService] AS [S] ON [SUL].[ServiceID] = [S].[ServiceID]
	INNER JOIN
		[dbo].[ecContracts] AS [C] ON [C].[ContractID] = [S].[ContractID]
	WHERE
		[C].[ResellerID] = @ResellerID
	AND
		ISNULL([SUL].[PeriodClosed], 0) = 0
	GROUP BY
		[SUL].[ServiceID];

	SELECT
		[S].* FROM [dbo].[ecService] AS [S]
	INNER JOIN
		@Svcs AS [SVCS] ON [S].[ServiceID] = [SVCS].[ServiceID]
	WHERE
		[S].[Status] = 1 AND @DaysOffset >= DATEDIFF(d, @TodayDate, [SVCS].[MaxEndDate]) 
	ORDER BY
		[ContractID];

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetProductHighlights]
	@ResellerID int,
	@ProductID int
AS
BEGIN
	
	SELECT
		[PH].[HighlightText]
	FROM
		[dbo].[ecProductsHighlights] AS [PH]
	INNER JOIN
		[dbo].[ecProduct] AS [P]
	ON
		[PH].[ProductID] = [P].[ProductID]
	WHERE
		[P].[ProductID] = @ProductID
	AND
		[P].[ResellerID] = @ResellerID
	ORDER BY
		[SortOrder];

END

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetProductCategoriesIds]
	@UserID int,
	@ProductID int
AS
BEGIN

	SELECT [CategoryID] FROM [dbo].[ecProductCategories] WHERE [ResellerID] = @UserID AND [ProductID] = @ProductID;	

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE [dbo].[ecGetProductCategories]
	@UserID int,
	@ProductID int
AS
BEGIN

    SELECT
		[C].*
	FROM
		[dbo].[ecCategories] AS [C]
	INNER JOIN
		[dbo].[ecProductCategories] AS [PC]
	ON
		[C].[CategoryID] = [PC].[CategoryID]
	WHERE
		[PC].[ProductID] = @ProductID
	AND
		[PC].[ResellerID] = @UserID;

END






































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetHostingPlansTaken]
	@ActorID int,
	@UserID int
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

    SELECT [PlanID] FROM [dbo].[ecHostingPlans] WHERE [ResellerID] = @UserID;

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetHostingPlanCycles]
	@UserID int,
	@ProductID int
AS
BEGIN

	SELECT
		[BC].*, [HPC].[ProductID], [HPC].[SetupFee], [HPC].[RecurringFee]  FROM [dbo].[ecHostingPlansBillingCycles] AS [HPC]
	INNER JOIN
		[dbo].[ecBillingCycles] AS [BC] ON [BC].[CycleID] = [HPC].[CycleID]
	INNER JOIN
		[dbo].[ecProduct] AS [PR] ON [HPC].[ProductID] = [PR].[ProductID]
	WHERE
		[PR].[ResellerID] = @UserID AND [HPC].[ProductID] = @ProductID
	ORDER BY
		[HPC].[SortOrder];

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetHostingPlan]
	@UserID int,
	@ProductID int
AS
BEGIN

    SELECT
		[PR].*,
		[HP].[PlanID],
		[HP].[UserRole],
		[HP].[InitialStatus],
		[HP].[DomainOption]
	FROM
		[dbo].[ecHostingPlans] AS [HP]
	INNER JOIN
		[dbo].[ecProduct] AS [PR]
	ON
		[PR].[ProductID] = [HP].[ProductID]
	WHERE
		[HP].[ResellerID] = @UserID
	AND
		[HP].[ProductID] = @ProductID;

END

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetHostingPackageSvcHistory]
	@ActorID int,
	@ServiceID int
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecService]
	WHERE
		[ServiceID] = @ServiceID;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		[HPC].[CycleName],
		[HPC].[BillingPeriod],
		[HPC].[PeriodLength],
		[HPC].[SetupFee],
		[HPC].[RecurringFee],
		[HPC].[Currency],
		[SUL].[StartDate],
		[SUL].[EndDate]
	FROM
		[dbo].[ecHostingPackageSvcsCycles] AS [HPC]
	INNER JOIN
		[dbo].[ecSvcsUsageLog] AS [SUL]
	ON
		[SUL].[ServiceID] = [HPC].[ServiceID]
	AND
		[SUL].[SvcCycleID] = [HPC].[SvcCycleID]
	WHERE
		[HPC].[ServiceID] = @ServiceID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetHostingPackageSvc] 
	@ActorID int,
	@ServiceID int
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecService]
	WHERE
		[ServiceID] = @ServiceID;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END

	-- 
	SET NOCOUNT ON;

    SELECT
		[SVC].*,
		[HPSVC].[ProductID],
		[HPSVC].[PlanID],
		[HPSVC].[PackageID],
		[HPSVC].[UserRole],
		[HPSVC].[InitialStatus],
		[HPSVC].[SvcCycleID],
		[HPSC].[CycleName],
		[HPSC].[BillingPeriod],
		[HPSC].[PeriodLength],
		[HPSC].[SetupFee],
		[HPSC].[RecurringFee],
		[HPSC].[Currency]
	FROM
		[dbo].[ecService] AS [SVC]
	INNER JOIN
		[dbo].[ecHostingPackageSvcs] AS [HPSVC]
	ON
		[HPSVC].[ServiceID] = [SVC].[ServiceID]
	INNER JOIN
		[dbo].[ecHostingPackageSvcsCycles] AS [HPSC]
	ON
		[HPSC].[SvcCycleID] = [HPSVC].[SvcCycleID]
	WHERE
		[SVC].[ServiceID] = @ServiceID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetHostingAddonSvcHistory]
	@ActorID int,
	@ServiceID int
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecService]
	WHERE
		[ServiceID] = @ServiceID;
	SELECT
		@IssuerID = ISNULL([CustomerID],[ResellerID]) FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this information', 16, 1);
		RETURN;
	END
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		[HAC].[CycleName],
		[HAC].[BillingPeriod],
		[HAC].[PeriodLength],
		[HAC].[SetupFee],
		[HAC].[CyclePrice] AS [RecurringFee],
		[HAC].[Currency],
		[SUL].[StartDate],
		[SUL].[EndDate]
	FROM
		[dbo].[ecHostingAddonSvcsCycles] AS [HAC]
	INNER JOIN
		[dbo].[ecSvcsUsageLog] AS [SUL]
	ON
		[SUL].[ServiceID] = [HAC].[ServiceID]
	AND
		[SUL].[SvcCycleID] = [HAC].[SvcCycleID]
	WHERE
		[HAC].[ServiceID] = @ServiceID;

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[ecGetAddonProductsIds]
	@UserID int,
	@ProductID int
AS
BEGIN
	
	SELECT 
		[P].[ProductID]
	FROM
		[dbo].[ecProduct] AS [P]
		INNER JOIN [dbo].[ecAddonProducts] AS [ATP]
		ON [P].[ProductID] = [ATP].[ProductID]
	WHERE
		[ATP].[AddonID] = @ProductID
		AND
		[P].[ResellerID] = @UserID;	

END































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE [dbo].[ecGetAddonProducts] 
	@UserID int,
	@ProductID int
AS
BEGIN

    SELECT 
		[P].*
	FROM
		[dbo].[ecProduct] AS [P]
		INNER JOIN [dbo].[ecAddonProducts] AS [ATP]
		ON [P].[ProductID] = [ATP].[ProductID]
	WHERE
		[ATP].[AddonID] = @ProductID
		AND
		[P].[ResellerID] = @UserID;

END




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




































CREATE PROCEDURE [dbo].[ecDeleteProduct] 
	@ActorID int,
	@UserID int,
	@ProductID int,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

BEGIN TRAN RMV_PRODUCT    
	-- remove product
	DELETE FROM [dbo].[ecProduct] WHERE [ProductID] = @ProductID AND [ResellerID] = @UserID;
	-- check errors
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;
	-- workaround for cyclic cascades
	DELETE FROM [dbo].[ecAddonProducts] WHERE [ProductID] = @ProductID AND [ResellerID] = @UserID;
	-- check errors
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;
	-- set result ok
	SET @Result = 0;
	-- commit actions
	COMMIT TRAN RMV_PRODUCT;
	-- exit routine
	RETURN;

ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN RMV_PRODUCT;
	RETURN;
END

END








































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO














CREATE PROCEDURE [dbo].[AddIPAddress]	
(
	@AddressID int OUTPUT,
	@ServerID int,
	@ExternalIP varchar(24),
	@InternalIP varchar(24),
	@PoolID int,
	@SubnetMask varchar(15),
	@DefaultGateway varchar(15),
	@Comments ntext
)
AS
BEGIN
	IF @ServerID = 0
	SET @ServerID = NULL

	INSERT INTO IPAddresses (ServerID, ExternalIP, InternalIP, PoolID, SubnetMask, DefaultGateway, Comments)
	VALUES (@ServerID, @ExternalIP, @InternalIP, @PoolID, @SubnetMask, @DefaultGateway, @Comments)

	SET @AddressID = SCOPE_IDENTITY()

	RETURN
END

















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecAddDomainNameSvc] 
	@ContractID nvarchar(50),
	@ParentID int,
	@ProductID int,
	@FQDN nvarchar(64),
	@CycleID int,
	@Currency nvarchar(10),
	@PropertyNames ntext,
	@PropertyValues ntext,
	@Result int OUTPUT
AS
BEGIN
	DECLARE @ResellerID int;
	SELECT
		@ResellerID = [ResellerID] FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	
BEGIN TRAN ADD_TLD_SVC
	-- add service
	INSERT INTO [dbo].[ecService]
		([ContractID], [ParentID], [ServiceName], [TypeID], [Status], [Created])
	VALUES
		(@ContractID, @ParentID, @FQDN, 3, 0, GETDATE());
	-- check error
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;
	-- obtain result
	SET @Result = SCOPE_IDENTITY();

	-- if product and cycle are defined
	IF @ProductID > 0 OR @CycleID > 0
	BEGIN
		DECLARE @SvcCycleID int;
		-- insert svc life-cycle
		INSERT INTO [dbo].[ecDomainSvcsCycles]
		(
			[ServiceID],
			[CycleName],
			[BillingPeriod],
			[PeriodLength],
			[SetupFee],
			[RecurringFee],
			[Currency]
		)
		SELECT
			@Result,
			[BC].[CycleName],
			[BC].[BillingPeriod],
			[BC].[PeriodLength],
			[TLDC].[SetupFee],
			[TLDC].[RecurringFee],
			@Currency
		FROM
			[dbo].[ecTopLevelDomainsCycles] AS [TLDC]
		INNER JOIN
			[dbo].[ecBillingCycles] AS [BC]
		ON
			[BC].[CycleID] = [TLDC].[CycleID]
		WHERE
			[TLDC].[CycleID] = @CycleID
		AND
			[TLDC].[ProductID] = @ProductID
		AND
			[BC].[ResellerID] = @ResellerID;
		-- check error
		IF @@ROWCOUNT = 0
			GOTO ERROR_HANDLE;
		-- obtain result
		SET @SvcCycleID = SCOPE_IDENTITY();

		-- add domain details
		INSERT INTO [dbo].[ecDomainSvcs]
		(
			[ServiceID],
			[ProductID],
			[PluginID],
			[FQDN],
			[SvcCycleID],
			[PropertyNames],
			[PropertyValues]
		)
		SELECT
			@Result,
			@ProductID,
			[PluginID],
			@FQDN,
			@SvcCycleID,
			@PropertyNames,
			@PropertyValues
		FROM
			[dbo].[ecTopLevelDomains]
		WHERE
			[ProductID] = @ProductID
		AND
			[ResellerID] = @ResellerID;
		-- check error
		IF @@ROWCOUNT = 0
			GOTO ERROR_HANDLE;
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[ecDomainSvcs]
		(
			[ServiceID],
			[ProductID],
			[FQDN],
			[SvcCycleID],
			[PropertyNames],
			[PropertyValues]
		)
		SELECT @Result, NULL, @FQDN, NULL, @PropertyNames, @PropertyValues;
		-- check error
		IF @@ROWCOUNT = 0
			GOTO ERROR_HANDLE;
	END

	-- commit
	COMMIT TRAN ADD_TLD_SVC;
	-- exit
	RETURN;

-- error handler
ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN ADD_TLD_SVC;
	RETURN;
END

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetRawServicesByServerID
(
	@ActorID int,
	@ServerID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

-- resource groups
SELECT
	GroupID,
	GroupName
FROM ResourceGroups
WHERE @IsAdmin = 1
ORDER BY GroupOrder

-- services
SELECT
	S.ServiceID,
	S.ServerID,
	S.ServiceName,
	S.Comments,
	RG.GroupID,
	PROV.DisplayName AS ProviderName
FROM Services AS S
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
WHERE
	S.ServerID = @ServerID
	AND @IsAdmin = 1
ORDER BY RG.GroupOrder

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetQuotas
AS
SELECT
	Q.GroupID,
	Q.QuotaID,
	RG.GroupName,
	Q.QuotaDescription,
	Q.QuotaTypeID
FROM Quotas AS Q
INNER JOIN ResourceGroups AS RG ON Q.GroupID = RG.GroupID
ORDER BY RG.GroupOrder, Q.QuotaOrder
RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE GetProviderServiceQuota
(
	@ProviderID int
)
AS

SELECT TOP 1
	Q.QuotaID,
	Q.GroupID,
	Q.QuotaName,
	Q.QuotaDescription,
	Q.QuotaTypeID,
	Q.ServiceQuota
FROM Providers AS P
INNER JOIN Quotas AS Q ON P.GroupID = Q.GroupID
WHERE P.ProviderID = @ProviderID AND Q.ServiceQuota = 1


RETURN




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE [dbo].[ecGetTopLevelDomainsPaged]
	@ActorID int,
	@UserID int,
	@MaximumRows int,
	@StartRowIndex int
AS
BEGIN

	DECLARE @EndIndex int;

	SET @EndIndex = @MaximumRows + @StartRowIndex;
	SET @StartRowIndex = @StartRowIndex + 1;

	WITH [TldsCTE] AS (
		SELECT
			ROW_NUMBER() OVER(ORDER BY [Created] DESC) AS [RowIndex],
			*
		FROM 
			[dbo].[ecProduct]
		WHERE
			[ResellerID] = @UserID
		AND
			[TypeID] = 3 -- Top Level Domain
	)

	SELECT 
		[TldsCTE].*,
		[Tlds].[PluginId],
		[PLG].[DisplayName]
	FROM
		[TldsCTE]
	INNER JOIN
		[dbo].[ecTopLevelDomains] AS [Tlds]
	ON
		[Tlds].[ProductID] = [TldsCTE].[ProductID]
	INNER JOIN
		[dbo].[ecSupportedPlugins] AS [PLG]
	ON
		[PLG].[PluginID] = [Tlds].[PluginID]
	WHERE
		[TldsCTE].[RowIndex] BETWEEN @StartRowIndex AND @EndIndex;
    
END
































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetTopLevelDomainCycles]
	@UserID int,
	@ProductID int
AS
BEGIN

	SELECT
		[BC].*,
		[TLDC].[ProductID],
		[TLDC].[SetupFee],
		[TLDC].[RecurringFee],
		[TLDC].[TransferFee]
	FROM
		[dbo].[ecTopLevelDomainsCycles] AS [TLDC]
	INNER JOIN
		[dbo].[ecBillingCycles] AS [BC]
	ON
		[BC].[CycleID] = [TLDC].[CycleID]
	INNER JOIN
		[dbo].[ecProduct] AS [PR]
	ON
		[TLDC].[ProductID] = [PR].[ProductID]
	WHERE
		[PR].[ResellerID] = @UserID
	AND
		[TLDC].[ProductID] = @ProductID
	ORDER BY
		[TLDC].[SortOrder];	

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


























CREATE PROCEDURE [dbo].[ecGetTopLevelDomain]
	@ActorID int,
	@UserID int,
	@ProductID int
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

	SELECT
		[P].*,
		[Tlds].[PluginID],
		[Tlds].[WhoisEnabled],
		[PLG].[DisplayName]
	FROM
		[dbo].[ecProduct] AS [P]
	INNER JOIN
		[dbo].[ecTopLevelDomains] AS [Tlds]
	ON
		[Tlds].[ProductID] = [P].[ProductID]
	INNER JOIN
		[dbo].[ecSupportedPlugins] AS [PLG]
	ON
		[PLG].[PluginID] = [Tlds].[PluginID]
	WHERE
		[P].[ResellerID] = @UserID
	AND
		[P].[ProductID] = @ProductID;

END





























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecGetSvcsSuspendDateAligned]
	@ResellerID int,
	@SvcsXml xml,
	@DefaultValue datetime,
	@Result datetime OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		@Result = MAX([SUL].[EndDate]) FROM [dbo].[ecSvcsUsageLog] AS [SUL]
	INNER JOIN
		[dbo].[ecService] AS [S] ON [SUL].[ServiceID] = [S].[ServiceID]
	INNER JOIn
		[dbo].[ecContracts] AS [C] ON [C].[ContractID] = [S].[ContractID]
	WHERE
		[C].[ResellerID] = @ResellerID
		AND
		[S].[ServiceID] IN (SELECT [SXML].[Data].value('@id','int') FROM @SvcsXml.nodes('/Svcs/Svc') [SXML]([Data]));
	
	-- result is empty	
	SET @Result = ISNULL(@Result, @DefaultValue);

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[GetService]
(
	@ActorID int,
	@ServiceID int
)
AS

SELECT
	ServiceID,
	Services.ServerID,
	ProviderID,
	ServiceName,
	ServiceQuotaValue,
	ClusterID,
	Services.Comments,
	Servers.ServerName
FROM Services INNER JOIN Servers ON Services.ServerID = Servers.ServerID
WHERE
	ServiceID = @ServiceID

RETURN 







GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE GetProviderByServiceID
(
	@ServiceID int
)
AS
SELECT
	P.ProviderID,
	P.GroupID,
	P.DisplayName,
	P.EditorControl,
	P.ProviderType
FROM Services AS S
INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
WHERE
	S.ServiceID = @ServiceID

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE GetServers
(
	@ActorID int
)
AS
-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	S.ServerID,
	S.ServerName,
	S.ServerUrl,
	(SELECT COUNT(SRV.ServiceID) FROM Services AS SRV WHERE S.ServerID = SRV.ServerID) AS ServicesNumber,
	S.Comments,
	PrimaryGroupID,
	S.ADEnabled
FROM Servers AS S
WHERE VirtualServer = 0
AND @IsAdmin = 1
ORDER BY S.ServerName

-- services
SELECT
	S.ServiceID,
	S.ServerID,
	S.ProviderID,
	S.ServiceName,
	S.Comments
FROM Services AS S
INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
INNER JOIN ResourceGroups AS RG ON P.GroupID = RG.GroupID
WHERE @IsAdmin = 1
ORDER BY RG.GroupOrder

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE GetServicesByServerIDGroupName
(
	@ActorID int,
	@ServerID int,
	@GroupName nvarchar(50)
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	S.ServiceID,
	S.ServerID,
	S.ServiceName,
	S.Comments,
	S.ServiceQuotaValue,
	RG.GroupName,
	PROV.DisplayName AS ProviderName
FROM Services AS S
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
WHERE
	S.ServerID = @ServerID AND RG.GroupName = @GroupName
	AND @IsAdmin = 1
ORDER BY RG.GroupOrder

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE GetServicesByServerID
(
	@ActorID int,
	@ServerID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)


SELECT
	S.ServiceID,
	S.ServerID,
	S.ServiceName,
	S.Comments,
	S.ServiceQuotaValue,
	RG.GroupName,
	S.ProviderID,
	PROV.DisplayName AS ProviderName
FROM Services AS S
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
WHERE
	S.ServerID = @ServerID
	AND @IsAdmin = 1
ORDER BY RG.GroupOrder

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[GetServicesByGroupName]
(
	@ActorID int,
	@GroupName nvarchar(100)
)
AS
-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	S.ServiceID,
	S.ServiceName,
	S.ServerID,
	S.ServiceQuotaValue,
	SRV.ServerName,
	S.ProviderID,
    PROV.ProviderName,
	S.ServiceName + ' on ' + SRV.ServerName AS FullServiceName
FROM Services AS S
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN ResourceGroups AS RG ON PROV.GroupID = RG.GroupID
WHERE
	RG.GroupName = @GroupName
	AND @IsAdmin = 1
RETURN 













GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE GetServicesByGroupID
(
	@ActorID int,
	@GroupID int
)
AS
-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	S.ServiceID,
	S.ServiceName,
	S.ServerID,
	S.ServiceQuotaValue,
	SRV.ServerName,
	S.ProviderID,
	S.ServiceName+' on '+SRV.ServerName AS FullServiceName
FROM Services AS S
INNER JOIN Providers AS PROV ON S.ProviderID = PROV.ProviderID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
WHERE
	PROV.GroupID = @GroupID
	AND @IsAdmin = 1
RETURN 




































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE [dbo].[ecAddServiceUsageRecord]
	@ActorID int,
	@ServiceID int,
	@SvcCycleID int,
	@StartDate datetime,
	@EndDate datetime,
	@Result int OUTPUT
AS
BEGIN
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SET @Result = 0;

    INSERT INTO [dbo].[ecSvcsUsageLog]
	(
		[ServiceID],
		[SvcCycleID],
		[StartDate],
		[EndDate]
	)
	VALUES
	(
		@ServiceID,
		@SvcCycleID,
		@StartDate,
		@EndDate
	);
	
END

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecUpdateTopLevelDomain]
	@ActorID int,
	@UserID int,
	@ProductID int,
	@TopLevelDomain nvarchar(10),
	@ProductSku nvarchar(50),
	@TaxInclusive bit,
	@PluginID int,
	@Enabled bit,
	@WhoisEnabled bit,
	@DomainCyclesXml xml,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

BEGIN TRAN UPDATE_DOMAIN
	-- insert product first
	UPDATE
		[dbo].[ecProduct]
	SET
		[ProductName] = @TopLevelDomain,
		[ProductSKU] = @ProductSku,
		[Enabled] = @Enabled,
		[TaxInclusive] = @TaxInclusive
	WHERE
		[ResellerID] = @UserID
	AND
		[ProductID] = @ProductID;

	-- save top level domain details
	UPDATE
		[dbo].[ecTopLevelDomains]
	SET
		[TopLevelDomain] = @TopLevelDomain,
		[PluginID] = @PluginID,
		[WhoisEnabled] = @WhoisEnabled
	WHERE
		[ResellerID] = @UserID
	AND
		[ProductID] = @ProductID;
	-- check errors
	IF @@ERROR <> 0
	BEGIN
		GOTO ERROR_HANDLE;
	END

/*
XML Format:

<DomainCycles>
	<Cycle ID="2" SetupFee="2.99" RecurringFee="3.99" TransferFee="3.99" TCOID="1005" SortOrder="1" />
</DomainCycles>
*/
	-- cleanup cycles
	DELETE FROM [dbo].[ecTopLevelDomainsCycles] WHERE [ProductID] = @ProductID;
	-- insert cycles
	INSERT INTO [dbo].[ecTopLevelDomainsCycles]
	(
		[ProductID],
		[CycleID],
		[SetupFee],
		[RecurringFee],
		[TransferFee],
		[SortOrder]
	)
	SELECT
		@ProductID,
		[SXML].[Data].value('@ID','int'),
		[SXML].[Data].value('@SetupFee','money'),
		[SXML].[Data].value('@RecurringFee','money'),
		[SXML].[Data].value('@TransferFee','money'),
		[SXML].[Data].value('@SortOrder','int')
	FROM @DomainCyclesXml.nodes('/DomainCycles/Cycle') [SXML]([Data]);
	-- check errors
	IF @@ERROR <> 0
	BEGIN
		GOTO ERROR_HANDLE;
	END
	--
	SET @Result = 0;
	--
	COMMIT TRAN UPDATE_DOMAIN;
	-- 
	RETURN;

ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN UPDATE_DOMAIN;
	RETURN;
END

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE UpdateVirtualGroups
(
	@ServerID int,
	@Xml ntext
)
AS


/*
XML Format:

<groups>
	<group id="16" distributionType="1" bindDistributionToPrimary="1"/>
</groups>

*/

BEGIN TRAN
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- delete old virtual groups
DELETE FROM VirtualGroups
WHERE ServerID = @ServerID

-- update HP resources
INSERT INTO VirtualGroups
(
	ServerID,
	GroupID,
	DistributionType,
	BindDistributionToPrimary
)
SELECT
	@ServerID,
	GroupID,
	DistributionType,
	BindDistributionToPrimary
FROM OPENXML(@idoc, '/groups/group',1) WITH 
(
	GroupID int '@id',
	DistributionType int '@distributionType',
	BindDistributionToPrimary bit '@bindDistributionToPrimary'
) as XRG

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN
RETURN 

































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecUpdateHostingPlanSvc]
	@ActorID int,
	@ServiceID int,
	@ProductID int,
	@PlanName nvarchar(255),
	@Status int,
	@PlanID int,
	@PackageID int,
	@UserRole int,
	@InitialStatus int,
	@Result int OUTPUT
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecService]
	WHERE
		[ServiceID] = @ServiceID;
	SELECT
		@IssuerID = [ResellerID] FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this action', 16, 1);
		RETURN;
	END

BEGIN TRAN UPD_HPLAN_SVC
	-- update plan svc
	UPDATE
		[dbo].[ecService]
	SET
		[ServiceName] = @PlanName,
		[Status] = @Status,
		[Modified] = GETDATE()
	WHERE
		[ServiceID] = @ServiceID;

	-- check error
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;
	-- update package svc
	IF @PackageID < 1
		SET @PackageID = NULL;
	--
	UPDATE
		[dbo].[ecHostingPackageSvcs]
	SET
		[ProductID] = @ProductID,
		[PlanID] = @PlanID,
		[PackageID] = @PackageID,
		[UserRole] = @UserRole,
		[InitialStatus] = @InitialStatus
	WHERE
		[ServiceID] = @ServiceID;
	-- check error
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;

	-- set result ok
	SET @Result = 0;
	-- commit changes
	COMMIT TRAN UPD_HPLAN_SVC;
	-- exit
	RETURN;

-- error handler
ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN UPD_HPLAN_SVC;
	RETURN;
END

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecUpdateHostingPlan]
	@ActorID int,
	@UserID int,
	@ProductID int,
	@PlanName nvarchar(255),
	@ProductSku nvarchar(50),
	@TaxInclusive bit,
	@PlanID int,
	@UserRole int,
	@InitialStatus int,
	@DomainOption int,
	@Enabled bit,
	@PlanDescription ntext,
	@PlanCyclesXml xml,
	@PlanHighlightsXml xml,
	@PlanCategoriesXml xml,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

BEGIN TRAN UPDATE_PLAN
	-- update product first
	UPDATE
		[dbo].[ecProduct]
	SET
		[ProductName] = @PlanName,
		[ProductSKU] = @ProductSku,
		[Description] = @PlanDescription,
		[Enabled] = @Enabled,
		[TaxInclusive] = @TaxInclusive
	WHERE
		[ResellerID] = @UserID AND [ProductID] = @ProductID AND [TypeID] = 1;

	-- update hosting plan details
	UPDATE
		[dbo].[ecHostingPlans]
	SET
		[PlanID] = @PlanID,
		[UserRole] = @UserRole,
		[InitialStatus] = @InitialStatus,
		[DomainOption] = @DomainOption
	WHERE
		[ResellerID] = @UserID AND [ProductID] = @ProductID;
	-- check errors
	IF @@ERROR <> 0
	BEGIN
		GOTO ERROR_HANDLE;
	END

/*
XML Format:

<PlanCycles>
	<Cycle ID="2" SetupFee="2.99" RecurringFee="3.99" SortOrder="1" />
</PlanCycles>
*/
	-- cleanup cycles
	DELETE FROM [dbo].[ecHostingPlansBillingCycles] WHERE [ProductID] = @ProductID;
	-- insert cycles
	INSERT INTO [dbo].[ecHostingPlansBillingCycles]
		([ProductID], [CycleID], [SetupFee], [RecurringFee], [SortOrder])
	SELECT
		@ProductID,[SXML].[Data].value('@ID','int'),[SXML].[Data].value('@SetupFee','money'),
		[SXML].[Data].value('@RecurringFee','money'), [SXML].[Data].value('@SortOrder','int')
	FROM @PlanCyclesXml.nodes('/PlanCycles/Cycle') [SXML]([Data]);
	-- check errors
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;

/*
XML Format:

<PlanHighlights>
	<Item Text="My Highlight Text" SortOrder="0" />
</PlanHighlights>
*/
	-- cleanup highlights
	DELETE FROM [dbo].[ecProductsHighlights] WHERE [ProductID] = @ProductID;
	-- insert cycles
	INSERT INTO [dbo].[ecProductsHighlights]
		([ProductID], [HighlightText], [SortOrder])
	SELECT
		@ProductID, [SXML].[Data].value('@Text','nvarchar(255)'), [SXML].[Data].value('@SortOrder','int')
	FROM @PlanHighlightsXml.nodes('/PlanHighlights/Item') [SXML]([Data]);
	-- check errors
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;

/*
XML Format:

<PlanCategories>
	<Category ID="2" />
</PlanCategories>
*/
	-- cleanup categories
	DELETE FROM [dbo].[ecProductCategories] WHERE [ProductID] = @ProductID;
	-- insert categories
	INSERT INTO [dbo].[ecProductCategories]
		([ProductID], [CategoryID], [ResellerID])
	SELECT
		@ProductID, [SXML].[Data].value('@ID','int'), @UserID
	FROM @PlanCategoriesXml.nodes('/PlanCategories/Category') [SXML]([Data]);
	-- check errors
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;
	--
	SET @Result = 0;
	-- commit changes
	COMMIT TRAN UPDATE_PLAN;
	-- exit
	RETURN;

ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN UPDATE_PLAN;
	RETURN;
END

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

















CREATE PROCEDURE [dbo].[ecUpdateHostingAddonSvc]
	@ActorID int,
	@ServiceID int,
	@ProductID int,
	@AddonName nvarchar(255),
	@Status int,
	@PlanID int,
	@PackageAddonID int,
	@Recurring bit,
	@DummyAddon bit,
	@Result int OUTPUT
AS
BEGIN
	DECLARE @IssuerID int, @ContractID nvarchar(50);
	SELECT
		@ContractID = [ContractID] FROM [dbo].[ecService]
	WHERE
		[ServiceID] = @ServiceID;
	SELECT
		@IssuerID = [ResellerID] FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @IssuerID) = 0
	BEGIN
		RAISERROR('You are not allowed to access this action', 16, 1);
		RETURN;
	END

	IF @PlanID < 1
		SET @PlanID = NULL;

BEGIN TRAN UPD_HADDON_SVC
	-- update addon svc
	UPDATE
		[dbo].[ecService]
	SET
		[ServiceName] = @AddonName,
		[Status] = @Status,
		[Modified] = GETDATE()
	WHERE
		[ServiceID] = @ServiceID;

	-- check error
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;
	-- update addon svc
	IF @PackageAddonID < 1
		SET @PackageAddonID = NULL;
	--
	UPDATE
		[dbo].[ecHostingAddonSvcs]
	SET
		[ProductID] = @ProductID,
		[PlanID] = @PlanID,
		[PackageAddonID] = @PackageAddonID,
		[Recurring] = @Recurring,
		[DummyAddon] = @DummyAddon
	WHERE
		[ServiceID] = @ServiceID;
	-- check error
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;

	-- set result ok
	SET @Result = 0;
	-- commit changes
	COMMIT TRAN UPD_HADDON_SVC;
	-- exit
	RETURN;

-- error handler
ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN UPD_HADDON_SVC;
	RETURN;
END    
END



















GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecAddHostingPlanSvc]
	@ContractID nvarchar(50),
	@ProductID int,
	@PlanName nvarchar(255),
	@CycleID int,
	@Currency nvarchar(10),
	@Result int OUTPUT
AS
BEGIN
	DECLARE @ResellerID int;
	SELECT
		@ResellerID = [ResellerID] FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;

BEGIN TRAN ADD_HPLAN_SVC
	-- add service
	INSERT INTO [dbo].[ecService]
		([ContractID], [ServiceName], [TypeID], [Status], [Created])
	VALUES
		(@ContractID, @PlanName, 1, 0, GETDATE());
	-- check error
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;
	-- obtain result
	SET @Result = SCOPE_IDENTITY();

	DECLARE @SvcCycleID int;
	-- insert svc life-cycle
	INSERT INTO [dbo].[ecHostingPackageSvcsCycles]
	(
		[ServiceID],
		[CycleName],
		[BillingPeriod],
		[PeriodLength],
		[SetupFee],
		[RecurringFee],
		[Currency]
	)
	SELECT
		@Result,
		[BC].[CycleName],
		[BC].[BillingPeriod],
		[BC].[PeriodLength],
		[HPBC].[SetupFee],
		[HPBC].[RecurringFee],
		@Currency
	FROM
		[dbo].[ecHostingPlansBillingCycles] AS [HPBC]
	INNER JOIN
		[dbo].[ecBillingCycles] AS [BC]
	ON
		[BC].[CycleID] = [HPBC].[CycleID]
	WHERE
		[HPBC].[CycleID] = @CycleID
	AND
		[HPBC].[ProductID] = @ProductID
	AND
		[BC].[ResellerID] = @ResellerID;
	-- check error
	IF @@ROWCOUNT = 0
		GOTO ERROR_HANDLE;
	-- obtain result
	SET @SvcCycleID = SCOPE_IDENTITY();
	
	-- add plan details
	INSERT INTO [dbo].[ecHostingPackageSvcs]
	(
		[ServiceID],
		[ProductID],
		[PlanID],
		[UserRole],
		[InitialStatus],
		[SvcCycleID]
	)
	SELECT
		@Result,
		@ProductID,
		[PlanID],
		[UserRole],
		[InitialStatus],
		@SvcCycleID
	FROM
		[dbo].[ecHostingPlans]
	WHERE
		[ProductID] = @ProductID
	AND
		[ResellerID] = @ResellerID;
	-- check error
	IF @@ROWCOUNT = 0
		GOTO ERROR_HANDLE;
	

	-- commit tran
	COMMIT TRAN ADD_HPLAN_SVC;
	-- exit
	RETURN;
-- error handler
ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN ADD_HPLAN_SVC;
	RETURN;
END

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecAddHostingPlan]
	@ActorID int,
	@UserID int,
	@PlanName nvarchar(255),
	@ProductSku nvarchar(50),
	@TaxInclusive bit,
	@PlanID int,
	@UserRole int,
	@InitialStatus int,
	@DomainOption int,
	@Enabled bit,
	@PlanDescription ntext,
	@PlanCyclesXml xml,
	@PlanHighlightsXml xml,
	@PlanCategoriesXml xml,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

BEGIN TRAN ADD_PLAN
	-- insert product first
	INSERT INTO [dbo].[ecProduct]
		([ProductName], [ProductSKU], [TypeID], [TaxInclusive], [Description], [Created], [Enabled], [ResellerID])
	VALUES
		(@PlanName, @ProductSku, 1, @TaxInclusive, @PlanDescription, GETDATE(), @Enabled, @UserID);
	-- set product id created
	SET @Result = SCOPE_IDENTITY();
	-- save hosting plan details
	INSERT INTO [dbo].[ecHostingPlans]
		([ProductID], [ResellerID], [PlanID], [UserRole], [InitialStatus], [DomainOption])
	VALUES
		(@Result, @UserID, @PlanID, @UserRole, @InitialStatus, @DomainOption);
	-- check errors
	IF @@ERROR <> 0
	BEGIN
		GOTO ERROR_HANDLE;
	END

/*
XML Format:

<PlanCycles>
	<Cycle ID="2" SetupFee="2.99" RecurringFee="3.99" SortOrder="1" />
</PlanCycles>
*/
	-- insert cycles
	INSERT INTO [dbo].[ecHostingPlansBillingCycles]
		([ProductID], [CycleID], [SetupFee], [RecurringFee], [SortOrder])
	SELECT
		@Result, [SXML].[Data].value('@ID','int'), [SXML].[Data].value('@SetupFee','money'),
		[SXML].[Data].value('@RecurringFee','money'), [SXML].[Data].value('@SortOrder','int')
	FROM @PlanCyclesXml.nodes('/PlanCycles/Cycle') [SXML]([Data])
	-- check errors
	IF @@ERROR <> 0
	BEGIN
		GOTO ERROR_HANDLE;
	END

/*
XML Format:

<PlanHighlights>
	<Item Text="My Highlight Text" SortOrder="0" />
</PlanHighlights>
*/
	-- insert cycles
	INSERT INTO [dbo].[ecProductsHighlights]
		([ProductID], [HighlightText], [SortOrder])
	SELECT
		@Result, [SXML].[Data].value('@Text','nvarchar(255)'), [SXML].[Data].value('@SortOrder','int')
	FROM @PlanHighlightsXml.nodes('/PlanHighlights/Item') [SXML]([Data])
	-- check errors
	IF @@ERROR <> 0
	BEGIN
		GOTO ERROR_HANDLE;
	END
/*
XML Format:

<PlanCategories>
	<Category ID="2" />
</PlanCategories>
*/
	-- insert categories
	INSERT INTO [dbo].[ecProductCategories]
		([ProductID], [CategoryID], [ResellerID])
	SELECT
		@Result, [SXML].[Data].value('@ID','int'), @UserID FROM @PlanCategoriesXml.nodes('/PlanCategories/Category') [SXML]([Data])
	-- check errors
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;
	-- commit changes
	COMMIT TRAN ADD_PLAN;
	-- return result
	RETURN;

ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN ADD_PLAN;
	RETURN;
END

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecAddHostingAddonSvc]
	@ContractID nvarchar(50),
	@ParentID int,
	@ProductID int,
	@Quantity int,
	@AddonName nvarchar(255),
	@CycleID int,
	@Currency nvarchar(10),
	@Result int OUTPUT
AS
BEGIN
	DECLARE @ResellerID int;
	SELECT
		@ResellerID = [ResellerID] FROM [dbo].[ecContracts]
	WHERE
		[ContractID] = @ContractID;

BEGIN TRAN ADD_ADDON_SVC
	--
	INSERT INTO [dbo].[ecService]
		([ContractID], [ParentID], [ServiceName], [TypeID], [Status], [Created])
	VALUES
		(@ContractID, @ParentID, @AddonName, 2, 0, GETDATE());
	-- obtain result
	SET @Result = SCOPE_IDENTITY();

	DECLARE @SvcCycleID int;
	-- insert svc life-cycle
	IF @CycleID > 0
	BEGIN
		INSERT INTO [dbo].[ecHostingAddonSvcsCycles]
		(
			[ServiceID],
			[CycleName],
			[BillingPeriod],
			[PeriodLength],
			[SetupFee],
			[CyclePrice],
			[Currency]
		)
		SELECT
			@Result,
			[BC].[CycleName],
			[BC].[BillingPeriod],
			[BC].[PeriodLength],
			[HAC].[SetupFee],
			[HAC].[RecurringFee],
			@Currency
		FROM
			[dbo].[ecHostingAddonsCycles] AS [HAC]
		INNER JOIN
			[dbo].[ecBillingCycles] AS [BC]
		ON
			[BC].[CycleID] = [HAC].[CycleID]
		WHERE
			[HAC].[CycleID] = @CycleID
		AND
			[HAC].[ProductID] = @ProductID
		AND
			[BC].[ResellerID] = @ResellerID;
		-- check error
		IF @@ROWCOUNT = 0
			GOTO ERROR_HANDLE;
		-- obtain result
		SET @SvcCycleID = SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[ecHostingAddonSvcsCycles]
		(
			[ServiceID],
			[CycleName],
			[BillingPeriod],
			[PeriodLength],
			[SetupFee],
			[CyclePrice],
			[Currency]
		)
		SELECT
			@Result,
			NULL,
			NULL,
			NULL,
			[SetupFee],
			[OneTimeFee],
			@Currency
		FROM
			[dbo].[ecHostingAddons]
		WHERE
			[ProductID] = @ProductID
		AND
			[ResellerID] = @ResellerID;
		-- check error
		IF @@ROWCOUNT = 0
			GOTO ERROR_HANDLE;
		-- obtain result
		SET @SvcCycleID = SCOPE_IDENTITY();
	END

	-- insert addon svc details
	INSERT INTO [dbo].[ecHostingAddonSvcs]
	(
		[ServiceID],
		[ProductID],
		[PlanID],
		[Quantity],
		[Recurring],
		[DummyAddon],
		[SvcCycleID]
	)
	SELECT
		@Result,
		@ProductID,
		[PlanID],
		@Quantity,
		[Recurring],
		[DummyAddon],
		@SvcCycleID
	FROM
		[dbo].[ecHostingAddons]
	WHERE
		[ProductID] = @ProductID
	AND
		[ResellerID] = @ResellerID;
	-- check error
	IF @@ROWCOUNT = 0
		GOTO ERROR_HANDLE;

	-- commit tran
	COMMIT TRAN ADD_ADDON_SVC;
	-- exit
	RETURN;
-- error handler
ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN ADD_ADDON_SVC;
	RETURN;
END

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecAddHostingAddon]
	@ActorID int,
	@UserID int,
	@AddonName nvarchar(255),
	@ProductSku nvarchar(50),
	@TaxInclusive bit,
	@Enabled bit,
	@PlanID int,
	@Recurring bit,
	@DummyAddon bit,
	@Countable bit,
	@Description ntext,
	@AddonCyclesXml xml,
	@AssignedProductsXml xml,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END
	-- dummy addon clause
	IF @DummyAddon = 1
		SET @PlanID = NULL;

BEGIN TRAN ADD_ADDON
	DECLARE @XmlDocID int;
	SET @XmlDocID = NULL;

	-- insert product first
	INSERT INTO [dbo].[ecProduct]
		([ProductName], [ProductSKU], [TypeID], [TaxInclusive], [Description], [Created], [Enabled], [ResellerID])
	VALUES
		(@AddonName, @ProductSku, 2, @TaxInclusive, @Description, GETDATE(), @Enabled, @UserID);
	-- check errors
	IF @@ERROR <> 0
	BEGIN
		GOTO ERROR_HANDLE;
	END
	-- set product id created
	SET @Result = SCOPE_IDENTITY();
	-- insert hosting addon details
	INSERT INTO [dbo].[ecHostingAddons]
		([ProductID], [PlanID], [Recurring], [ResellerID], [DummyAddon], [Countable])
	VALUES
		(@Result, @PlanID, @Recurring, @UserID, @DummyAddon, @Countable);
	-- check errors
	IF @@ERROR <> 0
	BEGIN
		GOTO ERROR_HANDLE;
	END
/*
XML Format:

<PlanCycles>
	<Cycle ID="2" SetupFee="2.99" RecurringFee="3.99" SortOrder="1" />
</PlanCycles>
*/
	-- save hosting addon cycles
	IF @Recurring = 1
		-- insert cycles
		INSERT INTO [dbo].[ecHostingAddonsCycles]
			([ProductID], [CycleID], [SetupFee], [RecurringFee], [SortOrder])
		SELECT
			@Result, [SXML].[Data].value('@ID','int'), [SXML].[Data].value('@SetupFee','money'),
			[SXML].[Data].value('@RecurringFee','money'), [SXML].[Data].value('@SortOrder','int')
		FROM @AddonCyclesXml.nodes('/PlanCycles/Cycle') [SXML]([Data])
	ELSE
		UPDATE
			[dbo].[ecHostingAddons]
		SET
			[SetupFee] = [SXML].[Data].value('@SetupFee','money'),
			[OneTimeFee] = [SXML].[Data].value('@RecurringFee','money') 
		FROM
			@AddonCyclesXml.nodes('/PlanCycles/Cycle') [SXML]([Data])
		WHERE
			[ResellerID] = @UserID AND [ProductID] = @Result;
	-- check errors
	IF @@ERROR <> 0
	BEGIN
		GOTO ERROR_HANDLE;
	END
/*
XML Format:

<AssignedProducts>
	<Product ID="25" />
</AssignedProducts>
*/
	-- insert cycles
	INSERT INTO [dbo].[ecAddonProducts]
		([AddonID], [ProductID], [ResellerID])
	SELECT
		@Result, [SXML].[Data].value('@ID','int'), @UserID
	FROM
		@AssignedProductsXml.nodes('/AssignedProducts/Product') [SXML]([Data])
	-- check errors
	IF @@ERROR <> 0
	BEGIN
		GOTO ERROR_HANDLE;
	END
	--
	COMMIT TRAN ADD_ADDON;
	-- 
	RETURN;
	

ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN ADD_ADDON;
	RETURN;	
END

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




















CREATE PROCEDURE [dbo].[ecAddTopLevelDomain]
	@ActorID int,
	@UserID int,
	@TopLevelDomain nvarchar(10),
	@ProductSku nvarchar(50),
	@TaxInclusive bit,
	@PluginID int,
	@Enabled bit,
	@WhoisEnabled bit,
	@DomainCyclesXml xml,
	@Result int OUTPUT
AS
BEGIN
	-- check actor user rights
	IF [dbo].[CheckUserParent](@ActorID, @UserID) = 0
	BEGIN
		SET @Result = -1;
		RAISERROR('You are not allowed to access this account', 16, 1);
		RETURN;
	END

BEGIN TRAN ADD_DOMAIN
	-- insert product first
	INSERT INTO [dbo].[ecProduct]
	(
		[ProductName],
		[ProductSKU],
		[TypeID],
		[Description],
		[Created],
		[Enabled],
		[ResellerID],
		[TaxInclusive]
	)
	VALUES
	(
		@TopLevelDomain,
		@ProductSku,
		3, -- Domain Name type
		NULL,
		GETDATE(),
		@Enabled,
		@UserID,
		@TaxInclusive
	);

	-- set product id created
	SET @Result = SCOPE_IDENTITY();

	-- save top level domain details
	INSERT INTO [dbo].[ecTopLevelDomains]
	(
		[ProductID],
		[ResellerID],
		[TopLevelDomain],
		[PluginID],
		[WhoisEnabled]
	)
	VALUES
	(
		@Result,
		@UserID,
		@TopLevelDomain,
		@PluginID,
		@WhoisEnabled
	);
	-- check errors
	IF @@ERROR <> 0
	BEGIN
		GOTO ERROR_HANDLE;
	END

/*
XML Format:

<DomainCycles>
	<Cycle ID="2" SetupFee="2.99" RecurringFee="3.99" TransferFee="3.99" SortOrder="1" />
</DomainCycles>
*/
	-- insert cycles
	INSERT INTO [dbo].[ecTopLevelDomainsCycles]
	(
		[ProductID],
		[CycleID],
		[SetupFee],
		[RecurringFee],
		[TransferFee],
		[SortOrder]
	)
	SELECT
		@Result,
		[SXML].[Data].value('@ID','int'),
		[SXML].[Data].value('@SetupFee','money'),
		[SXML].[Data].value('@RecurringFee','money'),
		[SXML].[Data].value('@TransferFee','money'),
		[SXML].[Data].value('@SortOrder','int')
	FROM @DomainCyclesXml.nodes('/DomainCycles/Cycle') [SXML]([Data]);
	-- check errors
	IF @@ERROR <> 0
		GOTO ERROR_HANDLE;
	--
	COMMIT TRAN ADD_DOMAIN;
	-- 
	RETURN;

ERROR_HANDLE:
BEGIN
	SET @Result = -1;
	ROLLBACK TRAN ADD_DOMAIN;
	RETURN;
END

END
























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE DeleteCluster
(
	@ClusterID int
)
AS

-- reset cluster in services
UPDATE Services
SET ClusterID = NULL
WHERE ClusterID = @ClusterID

-- delete cluster
DELETE FROM Clusters
WHERE ClusterID = @ClusterID
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

































CREATE PROCEDURE UpdateService
(
	@ServiceID int,
	@ServiceName nvarchar(50),
	@Comments ntext,
	@ServiceQuotaValue int,
	@ClusterID int
)
AS

IF @ClusterID = 0 SET @ClusterID = NULL

UPDATE Services
SET
	ServiceName = @ServiceName,
	ServiceQuotaValue = @ServiceQuotaValue,
	Comments = @Comments,
	ClusterID = @ClusterID
WHERE ServiceID = @ServiceID

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
































CREATE PROCEDURE [dbo].[GetAllServers]
(
	@ActorID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

SELECT
	S.ServerID,
	S.ServerName,
	S.ServerUrl,
	(SELECT COUNT(SRV.ServiceID) FROM VirtualServices AS SRV WHERE S.ServerID = SRV.ServerID) AS ServicesNumber,
	S.Comments
FROM Servers AS S
WHERE @IsAdmin = 1
ORDER BY S.VirtualServer, S.ServerName



































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE AddVirtualServices
(
	@ServerID int,
	@Xml ntext
)
AS

/*
XML Format:

<services>
	<service id="16" />
</services>

*/

BEGIN TRAN
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- update HP resources
INSERT INTO VirtualServices
(
	ServerID,
	ServiceID
)
SELECT
	@ServerID,
	ServiceID
FROM OPENXML(@idoc, '/services/service',1) WITH 
(
	ServiceID int '@id'
) as XS
WHERE XS.ServiceID NOT IN (SELECT ServiceID FROM VirtualServices WHERE ServerID = @ServerID)

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN
RETURN


































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO






























CREATE PROCEDURE dbo.UpdateServiceProperties
(
	@ServiceID int,
	@Xml ntext
)
AS

-- delete old properties
BEGIN TRAN
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- Execute a SELECT statement that uses the OPENXML rowset provider.
DELETE FROM ServiceProperties
WHERE ServiceID = @ServiceID
AND PropertyName IN
(
	SELECT PropertyName
	FROM OPENXML(@idoc, '/properties/property', 1)
	WITH (PropertyName nvarchar(50) '@name')
)

INSERT INTO ServiceProperties
(
	ServiceID,
	PropertyName,
	PropertyValue
)
SELECT
	@ServiceID,
	PropertyName,
	PropertyValue
FROM OPENXML(@idoc, '/properties/property',1) WITH 
(
	PropertyName nvarchar(50) '@name',
	PropertyValue nvarchar(1000) '@value'
) as PV

-- remove document
exec sp_xml_removedocument @idoc

COMMIT TRAN
RETURN 





























GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetVirtualServices
(
	@ActorID int,
	@ServerID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)

-- virtual groups
SELECT
	VRG.VirtualGroupID,
	RG.GroupID,
	RG.GroupName,
	ISNULL(VRG.DistributionType, 1) AS DistributionType,
	ISNULL(VRG.BindDistributionToPrimary, 1) AS BindDistributionToPrimary
FROM ResourceGroups AS RG
LEFT OUTER JOIN VirtualGroups AS VRG ON RG.GroupID = VRG.GroupID AND VRG.ServerID = @ServerID
WHERE
	@IsAdmin = 1
ORDER BY RG.GroupOrder

-- services
SELECT
	VS.ServiceID,
	S.ServiceName,
	S.Comments,
	P.GroupID,
	P.DisplayName,
	SRV.ServerName
FROM VirtualServices AS VS
INNER JOIN Services AS S ON VS.ServiceID = S.ServiceID
INNER JOIN Servers AS SRV ON S.ServerID = SRV.ServerID
INNER JOIN Providers AS P ON S.ProviderID = P.ProviderID
WHERE
	VS.ServerID = @ServerID
	AND @IsAdmin = 1

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




























CREATE PROCEDURE [dbo].[GetVirtualServers]
(
	@ActorID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)


SELECT
	S.ServerID,
	S.ServerName,
	S.ServerUrl,
	(SELECT COUNT(SRV.ServiceID) FROM VirtualServices AS SRV WHERE S.ServerID = SRV.ServerID) AS ServicesNumber,
	S.Comments,
	PrimaryGroupID
FROM Servers AS S
WHERE
	VirtualServer = 1
	AND @IsAdmin = 1
ORDER BY S.ServerName

RETURN 






































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





























CREATE PROCEDURE GetServiceProperties
(
	@ActorID int,
	@ServiceID int
)
AS


SELECT ServiceID, PropertyName, PropertyValue
FROM ServiceProperties
WHERE
	ServiceID = @ServiceID

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






























CREATE PROCEDURE GetAvailableVirtualServices
(
	@ActorID int,
	@ServerID int
)
AS

-- check rights
DECLARE @IsAdmin bit
SET @IsAdmin = dbo.CheckIsUserAdmin(@ActorID)


SELECT
	S.ServerID,
	S.ServerName,
	S.Comments
FROM Servers AS S
WHERE
	VirtualServer = 0 -- get only physical servers
	AND @IsAdmin = 1

-- services
SELECT
	ServiceID,
	ServerID,
	ProviderID,
	ServiceName,
	Comments
FROM Services
WHERE
	ServiceID NOT IN (SELECT ServiceID FROM VirtualServices WHERE ServerID = @ServerID)
	AND @IsAdmin = 1

RETURN 





































GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO































CREATE PROCEDURE [dbo].[DeleteVirtualServices]
(
	@ServerID int,
	@Xml ntext
)
AS

/*
XML Format:

<services>
	<service id="16" />
</services>

*/

BEGIN TRAN
DECLARE @idoc int
--Create an internal representation of the XML document.
EXEC sp_xml_preparedocument @idoc OUTPUT, @xml

-- update HP resources
DELETE FROM VirtualServices
WHERE ServiceID IN (
SELECT
	ServiceID
FROM OPENXML(@idoc, '/services/service',1) WITH 
(
	ServiceID int '@id'
) as XS)
AND ServerID = @ServerID

-- remove document
EXEC sp_xml_removedocument @idoc

COMMIT TRAN
RETURN



































GO
ALTER TABLE [dbo].[ScheduleParameters]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleParameters_Schedule] FOREIGN KEY([ScheduleID])
REFERENCES [dbo].[Schedule] ([ScheduleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ScheduleParameters] CHECK CONSTRAINT [FK_ScheduleParameters_Schedule]
GO
ALTER TABLE [dbo].[ExchangeAccounts]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeAccounts_ServiceItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExchangeAccounts] CHECK CONSTRAINT [FK_ExchangeAccounts_ServiceItems]
GO
ALTER TABLE [dbo].[ExchangeAccounts] ADD  CONSTRAINT [DF__ExchangeA__Creat__59B045BD]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[HostingPlans]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlans_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HostingPlans] CHECK CONSTRAINT [FK_HostingPlans_Packages]
GO
ALTER TABLE [dbo].[HostingPlans]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlans_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
GO
ALTER TABLE [dbo].[HostingPlans] CHECK CONSTRAINT [FK_HostingPlans_Servers]
GO
ALTER TABLE [dbo].[HostingPlans]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlans_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[HostingPlans] CHECK CONSTRAINT [FK_HostingPlans_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Users] FOREIGN KEY([OwnerID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Demo]  DEFAULT ((0)) FOR [IsDemo]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsPeer]  DEFAULT ((0)) FOR [IsPeer]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_HtmlLetters]  DEFAULT ((1)) FOR [HtmlMail]
GO
ALTER TABLE [dbo].[Packages]  WITH CHECK ADD  CONSTRAINT [FK_Packages_HostingPlans] FOREIGN KEY([PlanID])
REFERENCES [dbo].[HostingPlans] ([PlanID])
GO
ALTER TABLE [dbo].[Packages] CHECK CONSTRAINT [FK_Packages_HostingPlans]
GO
ALTER TABLE [dbo].[Packages]  WITH CHECK ADD  CONSTRAINT [FK_Packages_Packages] FOREIGN KEY([ParentPackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[Packages] CHECK CONSTRAINT [FK_Packages_Packages]
GO
ALTER TABLE [dbo].[Packages]  WITH CHECK ADD  CONSTRAINT [FK_Packages_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
GO
ALTER TABLE [dbo].[Packages] CHECK CONSTRAINT [FK_Packages_Servers]
GO
ALTER TABLE [dbo].[Packages]  WITH CHECK ADD  CONSTRAINT [FK_Packages_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Packages] CHECK CONSTRAINT [FK_Packages_Users]
GO
ALTER TABLE [dbo].[Packages] ADD  CONSTRAINT [DF_Packages_OverrideQuotas]  DEFAULT ((0)) FOR [OverrideQuotas]
GO
ALTER TABLE [dbo].[ServiceItems]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItems_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[ServiceItems] CHECK CONSTRAINT [FK_ServiceItems_Packages]
GO
ALTER TABLE [dbo].[ServiceItems]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItems_ServiceItemTypes] FOREIGN KEY([ItemTypeID])
REFERENCES [dbo].[ServiceItemTypes] ([ItemTypeID])
GO
ALTER TABLE [dbo].[ServiceItems] CHECK CONSTRAINT [FK_ServiceItems_ServiceItemTypes]
GO
ALTER TABLE [dbo].[ServiceItems]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItems_Services] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Services] ([ServiceID])
GO
ALTER TABLE [dbo].[ServiceItems] CHECK CONSTRAINT [FK_ServiceItems_Services]
GO
ALTER TABLE [dbo].[Domains]  WITH CHECK ADD  CONSTRAINT [FK_Domains_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Domains] CHECK CONSTRAINT [FK_Domains_Packages]
GO
ALTER TABLE [dbo].[Domains]  WITH CHECK ADD  CONSTRAINT [FK_Domains_ServiceItems_MailDomain] FOREIGN KEY([MailDomainID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
GO
ALTER TABLE [dbo].[Domains] CHECK CONSTRAINT [FK_Domains_ServiceItems_MailDomain]
GO
ALTER TABLE [dbo].[Domains]  WITH CHECK ADD  CONSTRAINT [FK_Domains_ServiceItems_WebSite] FOREIGN KEY([WebSiteID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
GO
ALTER TABLE [dbo].[Domains] CHECK CONSTRAINT [FK_Domains_ServiceItems_WebSite]
GO
ALTER TABLE [dbo].[Domains]  WITH CHECK ADD  CONSTRAINT [FK_Domains_ServiceItems_ZoneItem] FOREIGN KEY([ZoneItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
GO
ALTER TABLE [dbo].[Domains] CHECK CONSTRAINT [FK_Domains_ServiceItems_ZoneItem]
GO
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_AllowedForHosting]  DEFAULT ((0)) FOR [HostingAllowed]
GO
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_SubDomainID]  DEFAULT ((0)) FOR [IsSubDomain]
GO
ALTER TABLE [dbo].[Domains] ADD  CONSTRAINT [DF_Domains_IsInstantAlias]  DEFAULT ((0)) FOR [IsInstantAlias]
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD  CONSTRAINT [FK_Schedule_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Schedule] CHECK CONSTRAINT [FK_Schedule_Packages]
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD  CONSTRAINT [FK_Schedule_ScheduleTasks] FOREIGN KEY([TaskID])
REFERENCES [dbo].[ScheduleTasks] ([TaskID])
GO
ALTER TABLE [dbo].[Schedule] CHECK CONSTRAINT [FK_Schedule_ScheduleTasks]
GO
ALTER TABLE [dbo].[PrivateIPAddresses]  WITH CHECK ADD  CONSTRAINT [FK_PrivateIPAddresses_ServiceItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PrivateIPAddresses] CHECK CONSTRAINT [FK_PrivateIPAddresses_ServiceItems]
GO
ALTER TABLE [dbo].[PackagesTreeCache]  WITH CHECK ADD  CONSTRAINT [FK_PackagesTreeCache_Packages] FOREIGN KEY([ParentPackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[PackagesTreeCache] CHECK CONSTRAINT [FK_PackagesTreeCache_Packages]
GO
ALTER TABLE [dbo].[PackagesTreeCache]  WITH CHECK ADD  CONSTRAINT [FK_PackagesTreeCache_Packages1] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[PackagesTreeCache] CHECK CONSTRAINT [FK_PackagesTreeCache_Packages1]
GO
ALTER TABLE [dbo].[ExchangeAccountEmailAddresses]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeAccountEmailAddresses_ExchangeAccounts] FOREIGN KEY([AccountID])
REFERENCES [dbo].[ExchangeAccounts] ([AccountID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExchangeAccountEmailAddresses] CHECK CONSTRAINT [FK_ExchangeAccountEmailAddresses_ExchangeAccounts]
GO
ALTER TABLE [dbo].[PackageIPAddresses]  WITH CHECK ADD  CONSTRAINT [FK_PackageIPAddresses_IPAddresses] FOREIGN KEY([AddressID])
REFERENCES [dbo].[IPAddresses] ([AddressID])
GO
ALTER TABLE [dbo].[PackageIPAddresses] CHECK CONSTRAINT [FK_PackageIPAddresses_IPAddresses]
GO
ALTER TABLE [dbo].[PackageIPAddresses]  WITH CHECK ADD  CONSTRAINT [FK_PackageIPAddresses_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PackageIPAddresses] CHECK CONSTRAINT [FK_PackageIPAddresses_Packages]
GO
ALTER TABLE [dbo].[PackageIPAddresses]  WITH CHECK ADD  CONSTRAINT [FK_PackageIPAddresses_ServiceItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
GO
ALTER TABLE [dbo].[PackageIPAddresses] CHECK CONSTRAINT [FK_PackageIPAddresses_ServiceItems]
GO
ALTER TABLE [dbo].[PackageAddons]  WITH CHECK ADD  CONSTRAINT [FK_PackageAddons_HostingPlans] FOREIGN KEY([PlanID])
REFERENCES [dbo].[HostingPlans] ([PlanID])
GO
ALTER TABLE [dbo].[PackageAddons] CHECK CONSTRAINT [FK_PackageAddons_HostingPlans]
GO
ALTER TABLE [dbo].[PackageAddons]  WITH CHECK ADD  CONSTRAINT [FK_PackageAddons_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PackageAddons] CHECK CONSTRAINT [FK_PackageAddons_Packages]
GO
ALTER TABLE [dbo].[HostingPlanQuotas]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlanQuotas_HostingPlans] FOREIGN KEY([PlanID])
REFERENCES [dbo].[HostingPlans] ([PlanID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HostingPlanQuotas] CHECK CONSTRAINT [FK_HostingPlanQuotas_HostingPlans]
GO
ALTER TABLE [dbo].[HostingPlanQuotas]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlanQuotas_Quotas] FOREIGN KEY([QuotaID])
REFERENCES [dbo].[Quotas] ([QuotaID])
GO
ALTER TABLE [dbo].[HostingPlanQuotas] CHECK CONSTRAINT [FK_HostingPlanQuotas_Quotas]
GO
ALTER TABLE [dbo].[PackageQuotas]  WITH CHECK ADD  CONSTRAINT [FK_PackageQuotas_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[PackageQuotas] CHECK CONSTRAINT [FK_PackageQuotas_Packages]
GO
ALTER TABLE [dbo].[PackageQuotas]  WITH CHECK ADD  CONSTRAINT [FK_PackageQuotas_Quotas] FOREIGN KEY([QuotaID])
REFERENCES [dbo].[Quotas] ([QuotaID])
GO
ALTER TABLE [dbo].[PackageQuotas] CHECK CONSTRAINT [FK_PackageQuotas_Quotas]
GO
ALTER TABLE [dbo].[Quotas]  WITH CHECK ADD  CONSTRAINT [FK_Quotas_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Quotas] CHECK CONSTRAINT [FK_Quotas_ResourceGroups]
GO
ALTER TABLE [dbo].[Quotas]  WITH CHECK ADD  CONSTRAINT [FK_Quotas_ServiceItemTypes] FOREIGN KEY([ItemTypeID])
REFERENCES [dbo].[ServiceItemTypes] ([ItemTypeID])
GO
ALTER TABLE [dbo].[Quotas] CHECK CONSTRAINT [FK_Quotas_ServiceItemTypes]
GO
ALTER TABLE [dbo].[Quotas] ADD  CONSTRAINT [DF_ResourceGroupQuotas_QuotaOrder]  DEFAULT ((1)) FOR [QuotaOrder]
GO
ALTER TABLE [dbo].[Quotas] ADD  CONSTRAINT [DF_ResourceGroupQuotas_QuotaTypeID]  DEFAULT ((2)) FOR [QuotaTypeID]
GO
ALTER TABLE [dbo].[Quotas] ADD  CONSTRAINT [DF_Quotas_ServiceQuota]  DEFAULT ((0)) FOR [ServiceQuota]
GO
ALTER TABLE [dbo].[CRMUsers]  WITH CHECK ADD  CONSTRAINT [FK_CRMUsers_ExchangeAccounts] FOREIGN KEY([AccountID])
REFERENCES [dbo].[ExchangeAccounts] ([AccountID])
GO
ALTER TABLE [dbo].[CRMUsers] CHECK CONSTRAINT [FK_CRMUsers_ExchangeAccounts]
GO
ALTER TABLE [dbo].[CRMUsers] ADD  CONSTRAINT [DF_Table_1_CreateDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[CRMUsers] ADD  CONSTRAINT [DF_CRMUsers_ChangedDate]  DEFAULT (getdate()) FOR [ChangedDate]
GO
ALTER TABLE [dbo].[HostingPlanResources]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlanResources_HostingPlans] FOREIGN KEY([PlanID])
REFERENCES [dbo].[HostingPlans] ([PlanID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HostingPlanResources] CHECK CONSTRAINT [FK_HostingPlanResources_HostingPlans]
GO
ALTER TABLE [dbo].[HostingPlanResources]  WITH CHECK ADD  CONSTRAINT [FK_HostingPlanResources_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[HostingPlanResources] CHECK CONSTRAINT [FK_HostingPlanResources_ResourceGroups]
GO
ALTER TABLE [dbo].[PackagesDiskspace]  WITH CHECK ADD  CONSTRAINT [FK_PackagesDiskspace_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[PackagesDiskspace] CHECK CONSTRAINT [FK_PackagesDiskspace_Packages]
GO
ALTER TABLE [dbo].[PackagesDiskspace]  WITH CHECK ADD  CONSTRAINT [FK_PackagesDiskspace_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[PackagesDiskspace] CHECK CONSTRAINT [FK_PackagesDiskspace_ResourceGroups]
GO
ALTER TABLE [dbo].[PackagesBandwidth]  WITH CHECK ADD  CONSTRAINT [FK_PackagesBandwidth_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[PackagesBandwidth] CHECK CONSTRAINT [FK_PackagesBandwidth_Packages]
GO
ALTER TABLE [dbo].[PackagesBandwidth]  WITH CHECK ADD  CONSTRAINT [FK_PackagesBandwidth_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[PackagesBandwidth] CHECK CONSTRAINT [FK_PackagesBandwidth_ResourceGroups]
GO
ALTER TABLE [dbo].[BlackBerryUsers]  WITH CHECK ADD  CONSTRAINT [FK_BlackBerryUsers_ExchangeAccounts] FOREIGN KEY([AccountId])
REFERENCES [dbo].[ExchangeAccounts] ([AccountID])
GO
ALTER TABLE [dbo].[BlackBerryUsers] CHECK CONSTRAINT [FK_BlackBerryUsers_ExchangeAccounts]
GO
ALTER TABLE [dbo].[BlackBerryUsers] ADD  CONSTRAINT [DF_BlackBerryUsers_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ServiceItemProperties]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItemProperties_ServiceItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServiceItemProperties] CHECK CONSTRAINT [FK_ServiceItemProperties_ServiceItems]
GO
ALTER TABLE [dbo].[PackageResources]  WITH CHECK ADD  CONSTRAINT [FK_PackageResources_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
GO
ALTER TABLE [dbo].[PackageResources] CHECK CONSTRAINT [FK_PackageResources_Packages]
GO
ALTER TABLE [dbo].[PackageResources]  WITH CHECK ADD  CONSTRAINT [FK_PackageResources_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[PackageResources] CHECK CONSTRAINT [FK_PackageResources_ResourceGroups]
GO
ALTER TABLE [dbo].[ExchangeOrganizations]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeOrganizations_ServiceItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExchangeOrganizations] CHECK CONSTRAINT [FK_ExchangeOrganizations_ServiceItems]
GO
ALTER TABLE [dbo].[ExchangeOrganizationDomains]  WITH CHECK ADD  CONSTRAINT [FK_ExchangeOrganizationDomains_ServiceItems] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ServiceItems] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExchangeOrganizationDomains] CHECK CONSTRAINT [FK_ExchangeOrganizationDomains_ServiceItems]
GO
ALTER TABLE [dbo].[ExchangeOrganizationDomains] ADD  CONSTRAINT [DF_ExchangeOrganizationDomains_IsHost]  DEFAULT ((0)) FOR [IsHost]
GO
ALTER TABLE [dbo].[OCSUsers] ADD  CONSTRAINT [DF_OCSUsers_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[OCSUsers] ADD  CONSTRAINT [DF_OCSUsers_ChangedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[ecBillingCycles] ADD  CONSTRAINT [DF_ecBillingCycles_Created]  DEFAULT (getdate()) FOR [Created]
GO
ALTER TABLE [dbo].[ResourceGroups] ADD  CONSTRAINT [DF_ResourceGroups_GroupOrder]  DEFAULT ((1)) FOR [GroupOrder]
GO
ALTER TABLE [dbo].[ecSupportedPluginLog] ADD  CONSTRAINT [DF_ecSpacePluginLog_Created]  DEFAULT (getdate()) FOR [Created]
GO
ALTER TABLE [dbo].[ecProductType] ADD  CONSTRAINT [DF_ecProductType_Created]  DEFAULT (getutcdate()) FOR [Created]
GO
ALTER TABLE [dbo].[ecCategory]  WITH CHECK ADD  CONSTRAINT [FK_ecCategory_ecCategory] FOREIGN KEY([ParentID])
REFERENCES [dbo].[ecCategory] ([CategoryID])
GO
ALTER TABLE [dbo].[ecCategory] CHECK CONSTRAINT [FK_ecCategory_ecCategory]
GO
ALTER TABLE [dbo].[ecCategory] ADD  CONSTRAINT [DF_EC_Categories_Level]  DEFAULT ((0)) FOR [Level]
GO
ALTER TABLE [dbo].[ecCategory] ADD  CONSTRAINT [DF_ecCategory_Created]  DEFAULT (getutcdate()) FOR [Created]
GO
ALTER TABLE [dbo].[ecCategory] ADD  CONSTRAINT [DF_EC_Categories_CategoryOrder]  DEFAULT ((0)) FOR [ItemOrder]
GO
ALTER TABLE [dbo].[ecContracts] ADD  CONSTRAINT [DF__ecContrac__Opene__668030F6]  DEFAULT (getdate()) FOR [OpenedDate]
GO
ALTER TABLE [dbo].[ecContracts] ADD  CONSTRAINT [DF__ecContrac__Balan__6774552F]  DEFAULT ((0)) FOR [Balance]
GO
ALTER TABLE [dbo].[ecServiceHandlersResponses] ADD  CONSTRAINT [DF_ecServiceHandlerResponses_Received]  DEFAULT (getdate()) FOR [Received]
GO
ALTER TABLE [dbo].[ecCustomersPayments] ADD  CONSTRAINT [DF_EC_Payments_PaymentDate]  DEFAULT (getdate()) FOR [Created]
GO
ALTER TABLE [dbo].[ecCustomersPayments] ADD  CONSTRAINT [DF_EC_Payments_GatewayID]  DEFAULT ((0)) FOR [PluginID]
GO
ALTER TABLE [dbo].[ecInvoice] ADD  CONSTRAINT [DF_EC_Invoices_CreatedDate]  DEFAULT (getutcdate()) FOR [Created]
GO
ALTER TABLE [dbo].[ecInvoice] ADD  CONSTRAINT [DF_ecInvoice_Total]  DEFAULT ((0)) FOR [Total]
GO
ALTER TABLE [dbo].[ecInvoice] ADD  CONSTRAINT [DF_ecInvoice_SubTotal]  DEFAULT ((0)) FOR [SubTotal]
GO
ALTER TABLE [dbo].[ecInvoice] ADD  CONSTRAINT [DF__ecInvoice__Taxat__4119A21D]  DEFAULT ((0)) FOR [TaxationID]
GO
ALTER TABLE [dbo].[ecSystemTriggers] ADD  CONSTRAINT [DF_ecSystemTriggers_TriggerID]  DEFAULT (newid()) FOR [TriggerID]
GO
ALTER TABLE [dbo].[ecPaymentProfiles] ADD  CONSTRAINT [DF_ecPaymentProfiles_Created]  DEFAULT (getdate()) FOR [Created]
GO
ALTER TABLE [dbo].[ServiceItemTypes]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItemTypes_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[ServiceItemTypes] CHECK CONSTRAINT [FK_ServiceItemTypes_ResourceGroups]
GO
ALTER TABLE [dbo].[ServiceItemTypes] ADD  CONSTRAINT [DF_ServiceItemTypes_TypeOrder]  DEFAULT ((1)) FOR [TypeOrder]
GO
ALTER TABLE [dbo].[ServiceItemTypes] ADD  CONSTRAINT [DF_ServiceItemTypes_Importable]  DEFAULT ((1)) FOR [Importable]
GO
ALTER TABLE [dbo].[ServiceItemTypes] ADD  CONSTRAINT [DF_ServiceItemTypes_Backup]  DEFAULT ((1)) FOR [Backupable]
GO
ALTER TABLE [dbo].[ScheduleTaskParameters]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleTaskParameters_ScheduleTasks] FOREIGN KEY([TaskID])
REFERENCES [dbo].[ScheduleTasks] ([TaskID])
GO
ALTER TABLE [dbo].[ScheduleTaskParameters] CHECK CONSTRAINT [FK_ScheduleTaskParameters_ScheduleTasks]
GO
ALTER TABLE [dbo].[ScheduleTaskParameters] ADD  CONSTRAINT [DF_ScheduleTaskParameters_ParameterOrder]  DEFAULT ((0)) FOR [ParameterOrder]
GO
ALTER TABLE [dbo].[ResourceGroupDnsRecords]  WITH CHECK ADD  CONSTRAINT [FK_ResourceGroupDnsRecords_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ResourceGroupDnsRecords] CHECK CONSTRAINT [FK_ResourceGroupDnsRecords_ResourceGroups]
GO
ALTER TABLE [dbo].[ResourceGroupDnsRecords] ADD  CONSTRAINT [DF_ResourceGroupDnsRecords_RecordOrder]  DEFAULT ((1)) FOR [RecordOrder]
GO
ALTER TABLE [dbo].[ecInvoiceItems]  WITH CHECK ADD  CONSTRAINT [FK_ecInvoiceItems_ecInvoice] FOREIGN KEY([InvoiceID])
REFERENCES [dbo].[ecInvoice] ([InvoiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecInvoiceItems] CHECK CONSTRAINT [FK_ecInvoiceItems_ecInvoice]
GO
ALTER TABLE [dbo].[ecInvoiceItems] ADD  CONSTRAINT [DF_ecInvoiceItems_Total]  DEFAULT ((0)) FOR [Total]
GO
ALTER TABLE [dbo].[ecInvoiceItems] ADD  CONSTRAINT [DF_ecInvoiceItems_SubTotal]  DEFAULT ((0)) FOR [SubTotal]
GO
ALTER TABLE [dbo].[ecInvoiceItems] ADD  CONSTRAINT [DF_ecInvoiceItems_UnitPrice]  DEFAULT ((0)) FOR [UnitPrice]
GO
ALTER TABLE [dbo].[ecInvoiceItems] ADD  CONSTRAINT [DF_ecInvoiceItems_Processed]  DEFAULT ((0)) FOR [Processed]
GO
ALTER TABLE [dbo].[ecPaymentMethods]  WITH CHECK ADD  CONSTRAINT [FK_ecPaymentMethods_ecSupportedPlugins] FOREIGN KEY([PluginID])
REFERENCES [dbo].[ecSupportedPlugins] ([PluginID])
GO
ALTER TABLE [dbo].[ecPaymentMethods] CHECK CONSTRAINT [FK_ecPaymentMethods_ecSupportedPlugins]
GO
ALTER TABLE [dbo].[ecService]  WITH CHECK ADD  CONSTRAINT [FK_ecService_ecProductType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[ecProductType] ([TypeID])
GO
ALTER TABLE [dbo].[ecService] CHECK CONSTRAINT [FK_ecService_ecProductType]
GO
ALTER TABLE [dbo].[ecService] ADD  CONSTRAINT [DF_ecService_Status]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[ecService] ADD  CONSTRAINT [DF_SpaceInstance_CreatedDate]  DEFAULT (getutcdate()) FOR [Created]
GO
ALTER TABLE [dbo].[ecService] ADD  CONSTRAINT [DF_SpaceInstance_ModifiedDate]  DEFAULT (getutcdate()) FOR [Modified]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Users]
GO
ALTER TABLE [dbo].[Comments] ADD  CONSTRAINT [DF_Comments_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[GlobalDnsRecords]  WITH CHECK ADD  CONSTRAINT [FK_GlobalDnsRecords_IPAddresses] FOREIGN KEY([IPAddressID])
REFERENCES [dbo].[IPAddresses] ([AddressID])
GO
ALTER TABLE [dbo].[GlobalDnsRecords] CHECK CONSTRAINT [FK_GlobalDnsRecords_IPAddresses]
GO
ALTER TABLE [dbo].[GlobalDnsRecords]  WITH CHECK ADD  CONSTRAINT [FK_GlobalDnsRecords_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GlobalDnsRecords] CHECK CONSTRAINT [FK_GlobalDnsRecords_Packages]
GO
ALTER TABLE [dbo].[GlobalDnsRecords]  WITH CHECK ADD  CONSTRAINT [FK_GlobalDnsRecords_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
GO
ALTER TABLE [dbo].[GlobalDnsRecords] CHECK CONSTRAINT [FK_GlobalDnsRecords_Servers]
GO
ALTER TABLE [dbo].[GlobalDnsRecords]  WITH CHECK ADD  CONSTRAINT [FK_GlobalDnsRecords_Services] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Services] ([ServiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GlobalDnsRecords] CHECK CONSTRAINT [FK_GlobalDnsRecords_Services]
GO
ALTER TABLE [dbo].[PackageServices]  WITH CHECK ADD  CONSTRAINT [FK_PackageServices_Packages] FOREIGN KEY([PackageID])
REFERENCES [dbo].[Packages] ([PackageID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PackageServices] CHECK CONSTRAINT [FK_PackageServices_Packages]
GO
ALTER TABLE [dbo].[PackageServices]  WITH CHECK ADD  CONSTRAINT [FK_PackageServices_Services] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Services] ([ServiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PackageServices] CHECK CONSTRAINT [FK_PackageServices_Services]
GO
ALTER TABLE [dbo].[Servers]  WITH CHECK ADD  CONSTRAINT [FK_Servers_ResourceGroups] FOREIGN KEY([PrimaryGroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[Servers] CHECK CONSTRAINT [FK_Servers_ResourceGroups]
GO
ALTER TABLE [dbo].[Servers] ADD  CONSTRAINT [DF_Servers_DisplayName]  DEFAULT ('') FOR [ServerUrl]
GO
ALTER TABLE [dbo].[Servers] ADD  CONSTRAINT [DF_Servers_VirtualServer]  DEFAULT ((0)) FOR [VirtualServer]
GO
ALTER TABLE [dbo].[Servers] ADD  CONSTRAINT [DF_Servers_ADEnabled]  DEFAULT ((0)) FOR [ADEnabled]
GO
ALTER TABLE [dbo].[Providers]  WITH CHECK ADD  CONSTRAINT [FK_Providers_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[Providers] CHECK CONSTRAINT [FK_Providers_ResourceGroups]
GO
ALTER TABLE [dbo].[ecProduct]  WITH CHECK ADD  CONSTRAINT [FK_ecProduct_ecProductType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[ecProductType] ([TypeID])
GO
ALTER TABLE [dbo].[ecProduct] CHECK CONSTRAINT [FK_ecProduct_ecProductType]
GO
ALTER TABLE [dbo].[ecProduct] ADD  CONSTRAINT [DF_ecProduct_Created]  DEFAULT (getutcdate()) FOR [Created]
GO
ALTER TABLE [dbo].[UserSettings]  WITH CHECK ADD  CONSTRAINT [FK_UserSettings_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserSettings] CHECK CONSTRAINT [FK_UserSettings_Users]
GO
ALTER TABLE [dbo].[ScheduleTaskViewConfiguration]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleTaskViewConfiguration_ScheduleTaskViewConfiguration] FOREIGN KEY([TaskID])
REFERENCES [dbo].[ScheduleTasks] ([TaskID])
GO
ALTER TABLE [dbo].[ScheduleTaskViewConfiguration] CHECK CONSTRAINT [FK_ScheduleTaskViewConfiguration_ScheduleTaskViewConfiguration]
GO
ALTER TABLE [dbo].[Services]  WITH CHECK ADD  CONSTRAINT [FK_Services_Clusters] FOREIGN KEY([ClusterID])
REFERENCES [dbo].[Clusters] ([ClusterID])
GO
ALTER TABLE [dbo].[Services] CHECK CONSTRAINT [FK_Services_Clusters]
GO
ALTER TABLE [dbo].[Services]  WITH CHECK ADD  CONSTRAINT [FK_Services_Providers] FOREIGN KEY([ProviderID])
REFERENCES [dbo].[Providers] ([ProviderID])
GO
ALTER TABLE [dbo].[Services] CHECK CONSTRAINT [FK_Services_Providers]
GO
ALTER TABLE [dbo].[Services]  WITH CHECK ADD  CONSTRAINT [FK_Services_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
GO
ALTER TABLE [dbo].[Services] CHECK CONSTRAINT [FK_Services_Servers]
GO
ALTER TABLE [dbo].[VirtualServices]  WITH CHECK ADD  CONSTRAINT [FK_VirtualServices_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualServices] CHECK CONSTRAINT [FK_VirtualServices_Servers]
GO
ALTER TABLE [dbo].[VirtualServices]  WITH CHECK ADD  CONSTRAINT [FK_VirtualServices_Services] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Services] ([ServiceID])
GO
ALTER TABLE [dbo].[VirtualServices] CHECK CONSTRAINT [FK_VirtualServices_Services]
GO
ALTER TABLE [dbo].[VirtualGroups]  WITH CHECK ADD  CONSTRAINT [FK_VirtualGroups_ResourceGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ResourceGroups] ([GroupID])
GO
ALTER TABLE [dbo].[VirtualGroups] CHECK CONSTRAINT [FK_VirtualGroups_ResourceGroups]
GO
ALTER TABLE [dbo].[VirtualGroups]  WITH CHECK ADD  CONSTRAINT [FK_VirtualGroups_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VirtualGroups] CHECK CONSTRAINT [FK_VirtualGroups_Servers]
GO
ALTER TABLE [dbo].[ServiceDefaultProperties]  WITH CHECK ADD  CONSTRAINT [FK_ServiceDefaultProperties_Providers] FOREIGN KEY([ProviderID])
REFERENCES [dbo].[Providers] ([ProviderID])
GO
ALTER TABLE [dbo].[ServiceDefaultProperties] CHECK CONSTRAINT [FK_ServiceDefaultProperties_Providers]
GO
ALTER TABLE [dbo].[ecProductsHighlights]  WITH CHECK ADD  CONSTRAINT [FK_ecProductsHighlights_ecProduct] FOREIGN KEY([ProductID])
REFERENCES [dbo].[ecProduct] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecProductsHighlights] CHECK CONSTRAINT [FK_ecProductsHighlights_ecProduct]
GO
ALTER TABLE [dbo].[ecProductCategories]  WITH CHECK ADD  CONSTRAINT [FK_EC_ProductsToCategories_EC_Categories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[ecCategory] ([CategoryID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecProductCategories] CHECK CONSTRAINT [FK_EC_ProductsToCategories_EC_Categories]
GO
ALTER TABLE [dbo].[ecProductCategories]  WITH CHECK ADD  CONSTRAINT [FK_EC_ProductsToCategories_EC_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[ecProduct] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecProductCategories] CHECK CONSTRAINT [FK_EC_ProductsToCategories_EC_Products]
GO
ALTER TABLE [dbo].[IPAddresses]  WITH CHECK ADD  CONSTRAINT [FK_IPAddresses_Servers] FOREIGN KEY([ServerID])
REFERENCES [dbo].[Servers] ([ServerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IPAddresses] CHECK CONSTRAINT [FK_IPAddresses_Servers]
GO
ALTER TABLE [dbo].[ecAddonProducts]  WITH CHECK ADD  CONSTRAINT [FK_ecAddonProducts_ecProduct] FOREIGN KEY([AddonID])
REFERENCES [dbo].[ecProduct] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecAddonProducts] CHECK CONSTRAINT [FK_ecAddonProducts_ecProduct]
GO
ALTER TABLE [dbo].[ecHostingPlansBillingCycles]  WITH CHECK ADD  CONSTRAINT [FK_ecHostingPlansBillingCycles_ecBillingCycles] FOREIGN KEY([CycleID])
REFERENCES [dbo].[ecBillingCycles] ([CycleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecHostingPlansBillingCycles] CHECK CONSTRAINT [FK_ecHostingPlansBillingCycles_ecBillingCycles]
GO
ALTER TABLE [dbo].[ecHostingPlansBillingCycles]  WITH CHECK ADD  CONSTRAINT [FK_ecHostingPlansBillingCycles_ecProduct] FOREIGN KEY([ProductID])
REFERENCES [dbo].[ecProduct] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecHostingPlansBillingCycles] CHECK CONSTRAINT [FK_ecHostingPlansBillingCycles_ecProduct]
GO
ALTER TABLE [dbo].[ecHostingPlansBillingCycles] ADD  CONSTRAINT [DF_ecPlanBillingCycles_SetupFee]  DEFAULT ((0)) FOR [SetupFee]
GO
ALTER TABLE [dbo].[ecHostingPlansBillingCycles] ADD  CONSTRAINT [DF_ecPlanBillingCycles_RecurringFee]  DEFAULT ((0)) FOR [RecurringFee]
GO
ALTER TABLE [dbo].[ecHostingPlans]  WITH CHECK ADD  CONSTRAINT [FK_ecHostingPlans_ecProduct] FOREIGN KEY([ProductID])
REFERENCES [dbo].[ecProduct] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecHostingPlans] CHECK CONSTRAINT [FK_ecHostingPlans_ecProduct]
GO
ALTER TABLE [dbo].[ecSvcsUsageLog]  WITH CHECK ADD  CONSTRAINT [FK_ecSvcsUsageLog_ecService] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[ecService] ([ServiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecSvcsUsageLog] CHECK CONSTRAINT [FK_ecSvcsUsageLog_ecService]
GO
ALTER TABLE [dbo].[ecHostingAddons]  WITH CHECK ADD  CONSTRAINT [FK_ecHostingAddons_ecProduct] FOREIGN KEY([ProductID])
REFERENCES [dbo].[ecProduct] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecHostingAddons] CHECK CONSTRAINT [FK_ecHostingAddons_ecProduct]
GO
ALTER TABLE [dbo].[ecDomainSvcsCycles]  WITH CHECK ADD  CONSTRAINT [FK_ecDomainsSvcsCycles_ecService] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[ecService] ([ServiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecDomainSvcsCycles] CHECK CONSTRAINT [FK_ecDomainsSvcsCycles_ecService]
GO
ALTER TABLE [dbo].[ecDomainSvcs]  WITH CHECK ADD  CONSTRAINT [FK_ecDomainsSvcs_ecService] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[ecService] ([ServiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecDomainSvcs] CHECK CONSTRAINT [FK_ecDomainsSvcs_ecService]
GO
ALTER TABLE [dbo].[ecTopLevelDomainsCycles]  WITH CHECK ADD  CONSTRAINT [FK_ecTopLevelDomainsCycles_ecProduct] FOREIGN KEY([ProductID])
REFERENCES [dbo].[ecProduct] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecTopLevelDomainsCycles] CHECK CONSTRAINT [FK_ecTopLevelDomainsCycles_ecProduct]
GO
ALTER TABLE [dbo].[ecTopLevelDomains]  WITH CHECK ADD  CONSTRAINT [FK_ecTopLevelDomains_ecProduct] FOREIGN KEY([ProductID])
REFERENCES [dbo].[ecProduct] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecTopLevelDomains] CHECK CONSTRAINT [FK_ecTopLevelDomains_ecProduct]
GO
ALTER TABLE [dbo].[ecHostingPackageSvcsCycles]  WITH CHECK ADD  CONSTRAINT [FK_ecPackagesSvcsCycles_ecService] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[ecService] ([ServiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecHostingPackageSvcsCycles] CHECK CONSTRAINT [FK_ecPackagesSvcsCycles_ecService]
GO
ALTER TABLE [dbo].[ecHostingPackageSvcs]  WITH CHECK ADD  CONSTRAINT [FK_ecPackagesSvcs_ecService] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[ecService] ([ServiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecHostingPackageSvcs] CHECK CONSTRAINT [FK_ecPackagesSvcs_ecService]
GO
ALTER TABLE [dbo].[ecHostingAddonSvcsCycles]  WITH CHECK ADD  CONSTRAINT [FK_ecAddonPackagesSvcsCycles_ecService] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[ecService] ([ServiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecHostingAddonSvcsCycles] CHECK CONSTRAINT [FK_ecAddonPackagesSvcsCycles_ecService]
GO
ALTER TABLE [dbo].[ecHostingAddonSvcs]  WITH CHECK ADD  CONSTRAINT [FK_ecAddonPackagesSvcs_ecService] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[ecService] ([ServiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecHostingAddonSvcs] CHECK CONSTRAINT [FK_ecAddonPackagesSvcs_ecService]
GO
ALTER TABLE [dbo].[ecHostingAddonsCycles]  WITH CHECK ADD  CONSTRAINT [FK_ecHostingAddonsCycles_ecBillingCycles] FOREIGN KEY([CycleID])
REFERENCES [dbo].[ecBillingCycles] ([CycleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecHostingAddonsCycles] CHECK CONSTRAINT [FK_ecHostingAddonsCycles_ecBillingCycles]
GO
ALTER TABLE [dbo].[ecHostingAddonsCycles]  WITH CHECK ADD  CONSTRAINT [FK_ecHostingAddonsCycles_ecProduct] FOREIGN KEY([ProductID])
REFERENCES [dbo].[ecProduct] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ecHostingAddonsCycles] CHECK CONSTRAINT [FK_ecHostingAddonsCycles_ecProduct]
GO
ALTER TABLE [dbo].[ServiceProperties]  WITH CHECK ADD  CONSTRAINT [FK_ServiceProperties_Services] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Services] ([ServiceID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServiceProperties] CHECK CONSTRAINT [FK_ServiceProperties_Services]
GO
