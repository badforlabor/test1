@echo off
:: svn������Ŀ¼
set svnRootPath=H:\workspace\war_pure\shared\02-Editor\Assets\Art

:: ����svn��Ϣ������ļ���
set tmpFile=C:\1.log

:: svn����
echo > %tmpFile%
svn status %svnRootPath% > %tmpFile%

:: ɾ��û�а汾����Ϣ���ļ�
setlocal enabledelayedexpansion
for /f "delims=" %%e in (%tmpFile%) do (
set var=%%e
if "!var:~0,1!"=="?" (
set "var=!var:?       =!"
set "varFolder=!var!\."
echo !varFolder!
:: ������ļ��У���ôɾ���ļ��У����򵱳��ļ�ȥɾ��
if exist !varFolder! (rmdir /s/q "!var!") else del /f/q "!var!"
)
)
