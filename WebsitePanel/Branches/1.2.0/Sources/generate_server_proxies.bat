SET WSDL="C:\Program Files (x86)\Microsoft WSE\v3.0\Tools\WseWsdl3.exe"
SET WSE_CLEAN=..\Tools\WseClean.exe
SET SERVER_URL=http://localhost:9003

%WSDL% %SERVER_URL%/VirtualizationServerForPrivateCloud.asmx /out:.\WebsitePanel.Server.Client\VirtualizationServerForPrivateCloudProxy.cs /namespace:WebsitePanel.Providers.VirtualizationForPC /type:webClient /fields
%WSE_CLEAN% .\WebsitePanel.Server.Client\VirtualizationServerForPrivateCloudProxy.cs

pause