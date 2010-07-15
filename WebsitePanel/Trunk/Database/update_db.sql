USE [WebsitePanel_build]
GO

-- update zone template for "WEB" services (IIS 6 and IIS 7) to support "Al unassigned"
update [dbo].[ResourceGroupDnsRecords]
set RecordData = '[IP]'
where GroupID = 2 /* WEB */ and RecordType = 'A'
GO

