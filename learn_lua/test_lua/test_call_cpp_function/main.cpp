/****************************************************************************
Copyright (c) 2013-2014,Dalian-LingYOU tech.
This is not a free-ware .DO NOT use it without any authorization.
*
* date : 12/9/2015 3:59:49 PM
* author : Labor
* purpose : 示例：lua调用c函数，c函数调用lua函数
****************************************************************************/
#define _LUA_CALL_CPP_FUNCTION 0

#include <src/lua.hpp>  
#include <iostream>  

/*打开Lua所需要的库*/
#pragma comment(lib,"lua51.lib")  
extern "C"
{
#include <src/lua.h>  
#include <src/lualib.h>  
}
#if _CALL_CPP_FUNCTION
static int average(lua_State *L)
{
	//返回栈中元素的个数  
	int n = lua_gettop(L);
	double sum = 0;
	int i;
	for (i = 1; i <= n; i++)
	{
		if (!lua_isnumber(L, i))
		{
			lua_pushstring(L, "Incorrect argument to 'average'");
			lua_error(L);
		}
		sum += lua_tonumber(L, i);
	}
	/* push the average */
	lua_pushnumber(L, sum / n);
	/* push the sum */
	lua_pushnumber(L, sum);

	/* return the number of results */
	return 2;
}

int main(int argc, char* argv[])
{
	lua_State *L = luaL_newstate(); /*创建一个解释器句柄*/
	luaL_openlibs(L);             /*打开所有的Lua库*/

	// 将c++函数注册到lua中
	lua_register(L, "average", average);

	const char* script =	"avg, sum = average(10, 20, 30, 40, 50)\n"	\
							"print(\"The average is \", avg)\n"	\
							"print(\"The sum is \", sum)\n";

	int error = luaL_loadstring(L, script);
	if (error)
	{
		printf("%s", lua_tostring(L, -1));
	}

	lua_pcall(L, 0, 0, 0); /*执行Lua脚本*/
	lua_close(L);       /*关闭句柄*/
	system("pause");
	return 0;
}
#else

// c++调用lua函数
int luaadd(lua_State *L, int x, int y)
{
	int sum;
	/* the function name */
	lua_getglobal(L, "add");        int nTop = lua_gettop(L); //得到栈的元素个数。栈顶的位置。  
	/* the first argument */
	lua_pushnumber(L, x);           nTop = lua_gettop(L);
	/* the second argument */
	lua_pushnumber(L, y);           nTop = lua_gettop(L);
	/* call the function with 2
	arguments, return 1 result */
	lua_call(L, 2, 1);              nTop = lua_gettop(L);
	/* get the result */
	sum = (int)lua_tonumber(L, -1); nTop = lua_gettop(L);
	/*清掉返回值*/
	lua_pop(L, 1);                  nTop = lua_gettop(L);
	/*取出脚本中的变量z的值*/
	lua_getglobal(L, "z");          nTop = lua_gettop(L);
	int z = (int)lua_tonumber(L, 1); nTop = lua_gettop(L);
	lua_pop(L, 1);                  nTop = lua_gettop(L);

	//没调通  
	/*lua_pushnumber(L, 4);         nTop = lua_gettop(L);
	lua_setglobal(L, "r");          nTop = lua_gettop(L);
	int r = (int)lua_tonumber(L, 1);nTop = lua_gettop(L);*/
	return sum;
}
int main(int argc, char* argv[])
{
	lua_State *L = luaL_newstate(); /*创建一个解释器句柄*/
	luaL_openlibs(L);             /*打开所有的Lua库*/
	
	const char* script =	"function add ( x, y )\n"	\
							"return x + y\n"	\
							"end\n";

	int error = luaL_loadstring(L, script);
	if (error)
	{
		printf("%s", lua_tostring(L, -1));
	}
	lua_pcall(L, 0, 0, 0); /*虽然没有主函数，也得执行Lua脚本！*/

	int sum = luaadd(L, 1, 2);
	printf("1+2=%d\N", sum);

	lua_close(L);       /*关闭句柄*/
	system("pause");
	return 0;
}
#endif