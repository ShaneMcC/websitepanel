@ECHO OFF

fciv.exe "..\Build\debug\WebsitePanelInstaller11.msi" -sha1 > "..\Build\debug\SHA1.log"
fciv.exe "..\Build\release\WebsitePanelInstaller11.msi" -sha1 > "..\Build\release\SHA1.log"

PAUSE
