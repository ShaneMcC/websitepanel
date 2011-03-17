@echo off
%systemroot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe WebsitePanel.VmConfig.exe
del WebsitePanel.VmConfig.InstallLog
del WebsitePanel.VmConfig.InstallState
del InstallUtil.InstallLog