@echo off
%systemroot%\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe WebsitePanel.VmConfig.exe
del WebsitePanel.VmConfig.InstallLog
del WebsitePanel.VmConfig.InstallState
del InstallUtil.InstallLog