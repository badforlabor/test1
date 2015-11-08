@echo off
:: svn检索的目录
set svnRootPath=H:\workspace\war_pure\shared\02-Editor\Assets\Art

:: 保存svn信息到这个文件中
set tmpFile=C:\1.log

:: svn检索
echo > %tmpFile%
svn status %svnRootPath% > %tmpFile%

:: 删掉没有版本号信息的文件
setlocal enabledelayedexpansion
for /f "delims=" %%e in (%tmpFile%) do (
set var=%%e
if "!var:~0,1!"=="?" (
set "var=!var:?       =!"
set "varFolder=!var!\."
echo !varFolder!
:: 如果是文件夹，那么删掉文件夹，否则当成文件去删除
if exist !varFolder! (rmdir /s/q "!var!") else del /f/q "!var!"
)
)
