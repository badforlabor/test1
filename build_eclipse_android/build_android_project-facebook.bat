
:: ˵��������Ǳ���unity3d�ġ�

set AndroidRootPath=E:\android\android-sdk-23.0.2-windows\platforms\android-21
set exePath=E:\android\android-sdk-23.0.2-windows\build-tools\android-5.0
::set exePath=E:\android\adt-bundle-windows-x86-20140702\sdk\build-tools\android-4.4W
set apkBuilderPath=E:\android\android-sdk-23.0.2-windows\tools
set signapkPath=H:\workspace\war_merge\shared\00-Editor\ant\signapk
set workPath=E:\liubo\my-file\github\test1\test_fb\proj\test_fb
set facebookPath=E:\liubo\my-file\github\test1\test_fb\proj\facebook
set classPath=com\joymefun\sdk
set classFacebookPath=com\facebook\android
::set sdkpath=com\lingyou\sdk
set appname=test_fb
set appname_nosign=%appname%_nosign
set appname_notalign=%appname%_notalign

goto step6

:step1
echo "��1�� ����R.java"
rmdir /s/q %workPath%\gen\%classPath%
mkdir %workPath%\gen\%classPath%
::%exePath%\aapt package -f -m -J %workPath%\gen -S %workPath%\res -I %AndroidRootPath%\android.jar -M %workPath%\AndroidManifest.xml
%exePath%\aapt package -f -m -J %workPath%\gen -S %workPath%\res -S %facebookPath%\res -I %AndroidRootPath%\android.jar -M %workPath%\AndroidManifest.xml --auto-add-overlay
:: ����facebook�� R.java
%exePath%\aapt package -f -m -J %workPath%\gen -S %workPath%\res -S %facebookPath%\res -I %AndroidRootPath%\android.jar -M %facebookPath%\AndroidManifest.xml --auto-add-overlay --non-constant-id

:step2
echo "��2�� ����*.java"
rmdir /s/q %workPath%\bin 
mkdir %workPath%\bin
::javac -encoding UTF-8 -target 1.7 -bootclasspath %AndroidRootPath%\android.jar -cp %workPath%\libs\* -d %workPath%\bin %workPath%\gen\%classPath%\*.java %workPath%\src\%classPath%\*.java %workPath%\src\%sdkpath%\*.java
javac -encoding UTF-8 -target 1.7 -bootclasspath %AndroidRootPath%\android.jar -cp %workPath%\libs\* -d %workPath%\bin %workPath%\gen\%classPath%\*.java %workPath%\gen\%classFacebookPath%\*.java %workPath%\src\%classPath%\*.java

::@pause

:step3
echo "��3�� ����classes.dex"
::%exePath%\dx --dex --output=%workPath%\bin\classes.dex %workPath%\bin %workPath%\libs
%exePath%\dx --dex --output=%workPath%\bin\classes.dex %workPath%\bin %workPath%\libs %facebookPath%\libs

:step4
echo "��4�� ��assets��res���"
::%exePath%\aapt package -f -A %workPath%\assets -S %workPath%\res -I %AndroidRootPath%\android.jar -M %workPath%\AndroidManifest.xml  -F %workPath%\bin\%appname%.ab
%exePath%\aapt package -f -A %workPath%\assets -S %workPath%\res -S %facebookPath%\res -I %AndroidRootPath%\android.jar -M %workPath%\AndroidManifest.xml -F %workPath%\bin\%appname%.ab --auto-add-overlay

:step5
echo "��5�� ����δǩ����apk"
del %workPath%\bin\%appname_nosign%.apk
%apkBuilderPath%\apkbuilder.bat %workPath%\bin\%appname_nosign%.apk -v -u -z %workPath%\bin\%appname%.ab -f %workPath%\bin\classes.dex -rf %workPath%\src -rj %workPath%\libs -nf %workPath%\libs

:step6
echo ��6�� ��apkǩ��
del %workPath%\bin\%appname_notalign%.apk
java -jar %signapkPath%\signapk.jar %signapkPath%\platform.x509.pem %signapkPath%\platform.pk8 %workPath%\bin\%appname_nosign%.apk %workPath%\bin\%appname_notalign%.apk

:step7
echo ��7����4�ֽڶ���
del %workPath%\bin\%appname%.apk
%exePath%\zipalign -v 4 %workPath%\bin\%appname_notalign%.apk %workPath%\bin\%appname%.apk

:step8
::echo ɾ����ʱ�ļ�
::del %workPath%\bin\%appname_notalign%.apk
::del %workPath%\bin\%appname_nosign%.apk
