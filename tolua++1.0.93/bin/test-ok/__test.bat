:: ���е�pkg�ļ���������Ŀ¼ \src\tests
:: ����tolua�Ĺ���pkg�ļ���h�ļ�������ͬһĿ¼�¡�

echo off
:: debug�汾��
::set tolua=tolua++_d.exe
:: release�汾��
set tolua=tolua++.exe
echo on


%tolua% -o 1.cpp tarray.pkg
%tolua% -o 2.cpp tclass.pkg
%tolua% -o 3.cpp tconstant.pkg
%tolua% -o 4.cpp tdirective.pkg

:: ������д����
%tolua% -o 5.cpp tdirectivepkg.pkg


%tolua% -o 6.cpp tfunction.pkg
%tolua% -o 7.cpp tmodule.pkg
%tolua% -o 8.cpp tnamespace.pkg
%tolua% -o 9.cpp tvariable.pkg

pause