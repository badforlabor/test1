/****************************************************************************
Copyright (c) 2013-2014,Dalian-LingYOU tech.
This is not a free-ware .DO NOT use it without any authorization.
*
* date : 12/9/2015 3:59:49 PM
* author : Labor
* purpose : 示例：运行lua脚本
****************************************************************************/
#include <src/lua.hpp>  
#include <iostream>  

/*打开Lua所需要的库*/
#pragma comment(lib,"lua51.lib")  
extern "C"
{
#include <src/lua.h>  
#include <src/lualib.h>  
}


int main(int argc, char* argv[])
{
	lua_State *L = luaL_newstate(); /*创建一个解释器句柄*/
	luaL_openlibs(L);             /*打开所有的Lua库*/

	
#if LUA_FILE
	int error = luaL_loadfile(L, "E:\\liubo\\my-file\\github\\test1\\learn_lua\\test_lua\\Debug\\script.lua"); /*调入Lua脚本文件*/
#else
	const char* script = "print(\"hello world\")";
	int error = luaL_loadstring(L, script);
#endif
	if (error)
	{
		printf("%s", lua_tostring(L, -1));
	}

	lua_pcall(L, 0, 0, 0); /*执行Lua脚本*/
	lua_close(L);       /*关闭句柄*/
	system("pause");
	return 0;
}