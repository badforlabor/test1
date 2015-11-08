#!bin/sh

svnRootPath=/Users/lingyoukeji/.hudson/jobs/ios_trunk_un_ab/workspace/Assets/Art

tmpFile=/Users/lingyoukeji/liubo/1.log

echo > ${tmpFile}
svn status ${svnRootPath} > ${tmpFile}

#cat ${tmpFile}
cat "${tmpFile}" | while read line
do
	if [ "${line:0:1}" = "?" ];then
		#echo "${line:8}"
		var="${line:8}"
		#echo "${var}"
		if [ -d "${var}" ];then 
			rm -rf "${var}"
		else
			rm -f "${var}"
		fi
	fi
#echo ${line}
#var="${line}"
#echo "${var}"
#var=${var:8}
#echo ${var}
#if "!var:~0,1!"="?"(
#"var=!var:?       =!"
#"varFolder=!var!\."
#echo !varFolder!
#if exist !varFolder! (rmdir /s/q "!var!") else del /f/q "!var!"
#)
done