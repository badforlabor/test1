/****************************************************************************
Copyright (c) 2013-2014,Dalian-LingYOU tech.
This is not a free-ware .DO NOT use it without any authorization.
*
* date : 12/9/2015 3:59:49 PM
* author : Labor
* purpose : ʾ��������lua�ű�
****************************************************************************/
#include <src/lua.hpp>  
#include <iostream>  

/*��Lua����Ҫ�Ŀ�*/
#pragma comment(lib,"lua51.lib")  
extern "C"
{
#include <src/lua.h>  
#include <src/lualib.h>  
}


int main(int argc, char* argv[])
{
	lua_State *L = luaL_newstate(); /*����һ�����������*/
	luaL_openlibs(L);             /*�����е�Lua��*/

	
#if LUA_FILE
	int error = luaL_loadfile(L, "E:\\liubo\\my-file\\github\\test1\\learn_lua\\test_lua\\Debug\\script.lua"); /*����Lua�ű��ļ�*/
#else
	const char* script = "print(\"hello world\")";
	int error = luaL_loadstring(L, script);
#endif
	if (error)
	{
		printf("%s", lua_tostring(L, -1));
	}

	lua_pcall(L, 0, 0, 0); /*ִ��Lua�ű�*/
	lua_close(L);       /*�رվ��*/
	system("pause");
	return 0;
}