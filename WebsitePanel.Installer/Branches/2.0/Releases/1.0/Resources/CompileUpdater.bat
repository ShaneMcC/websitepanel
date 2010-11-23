@ECHO OFF
set basedir=
IF %1.==. GOTO NoArg
set basedir=%1
:NoArg
set sourcedir=%basedir%WebsitePanel.Updater\bin
set targetdir=%basedir%WebsitePanel.Installer

"%basedir%ILMerge.exe" "%sourcedir%\WebsitePanel.Updater.exe" "%sourcedir%\..\Lib\Ionic.Zip.Reduced.dll" /out:%targetdir%\Updater.exe
del %targetdir%\Updater.pdb 