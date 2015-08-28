#!bin/sh
# 编译unity3d导出的android工程

AndroidRootPath=/Users/gangfang/AndroidSDK/android-sdk-macosx/platforms/android-21
exePath=/Users/gangfang/AndroidSDK/android-sdk-macosx/build-tools/android-5.0
apkBuilderPath=/Users/gangfang/AndroidSDK/android-sdk-macosx/tools
signapkPath=/Users/gangfang/liubo/test_fps/android_project/ant/signapk
workPath=/Users/gangfang/liubo/test_fps/android_project/test_fps
classPath=com/liubo/test
appname=test_fps
appname_nosign=${appname}_nosign
appname_notalign=${appname}_notalign

#goto step6


#:step1
echo "第一步，生成 R.java"
rm -rf ${workPath}/gen
mkdir -pv ${workPath}/gen
${exePath}/aapt package -f -m -J ${workPath}/gen -S ${workPath}/res -I ${AndroidRootPath}/android.jar -M ${workPath}/AndroidManifest.xml


#:step2
echo "第二步，编译 *.java"
rm -rf ${workPath}/bin
mkdir -pv ${workPath}/bin
javac -encoding UTF-8 -target 1.7 -bootclasspath ${AndroidRootPath}/android.jar -cp ${workPath}/libs/unity-classes.jar -d ${workPath}/bin ${workPath}/gen/${classPath}/*.java ${workPath}/src/${classPath}/*.java

#:step3
echo "第三步，生成 classes.dex"
${exePath}/dx --dex --output=${workPath}/bin/classes.dex ${workPath}/bin ${workPath}/libs


#:step4
echo "第四步，将assets、res打包"
${exePath}/aapt package -f -A ${workPath}/assets -S ${workPath}/res -I ${AndroidRootPath}/android.jar -M ${workPath}/AndroidManifest.xml  -F ${workPath}/bin/${appname}.ab

#:step5
echo "第五步，生成未签名的apk"
rm -rf ${workPath}/bin/${appname_nosign}.apk
sh ${apkBuilderPath}/apkbuilder.sh ${workPath}/bin/${appname_nosign}.apk -v -u -z ${workPath}/bin/${appname}.ab -f ${workPath}/bin/classes.dex -rf ${workPath}/src -rj ${workPath}/libs -nf ${workPath}/libs

#:step6
echo "第六步，将apk签名"
rm -rf ${workPath}/bin/${appname_notalign}.apk
java -jar ${signapkPath}/signapk.jar ${signapkPath}/platform.x509.pem ${signapkPath}/platform.pk8 ${workPath}/bin/${appname_nosign}.apk ${workPath}/bin/${appname_notalign}.apk

#:step7
echo "第七步，4字节对齐"
rm -rf ${workPath}/bin/${appname}.apk
${exePath}/zipalign -v 4 ${workPath}/bin/${appname_notalign}.apk ${workPath}/bin/${appname}.apk

#:step8
#echo "第八步，删掉临时文件"
#rm -rf ${workPath}/bin/${appname_notalign}.apk
#rm -rf ${workPath}/bin/${appname_nosign}.apk
