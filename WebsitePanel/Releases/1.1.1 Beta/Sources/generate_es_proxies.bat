SET WSDL="C:\Program Files (x86)\Microsoft WSE\v3.0\Tools\WseWsdl3.exe"
SET WSE_CLEAN=..\Tools\WseClean.exe
SET SERVER_URL=http://localhost:9002

%WSDL% %SERVER_URL%/esExchangeHostedEdition.asmx /out:.\WebsitePanel.EnterpriseServer.Client\ExchangeHostedEditionProxy.cs /namespace:WebsitePanel.EnterpriseServer /type:webClient
%WSE_CLEAN% .\WebsitePanel.EnterpriseServer.Client\ExchangeHostedEditionProxy.cs