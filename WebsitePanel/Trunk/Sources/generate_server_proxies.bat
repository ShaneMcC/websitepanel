SET WSDL="C:\Program Files (x86)\Microsoft WSE\v3.0\Tools\WseWsdl3.exe"
SET WSE_CLEAN=..\Tools\WseClean.exe
SET SERVER_URL=http://localhost:9003

%WSDL% %SERVER_URL%/WebServer.asmx /out:.\WebsitePanel.Server.Client\WebServerProxy.cs /namespace:WebsitePanel.Providers.Web /type:webClient /fields
%WSE_CLEAN% .\WebsitePanel.Server.Client\WebServerProxy.cs

pause