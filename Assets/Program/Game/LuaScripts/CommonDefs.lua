
--Load C# Global Funcs
local LuaGlobalFunc = CS.LuaSharp.LuaGlobalFunc
for k, v in pairs(LuaGlobalFunc) do
    if type(v)=="function" then
        _G[k]=v
        LuaGlobalFunc.ConsoleLog("Load Global Func:"..tostring(k))
    end
end

table.nkeys = function(t)
    local count = 0
    for _ in pairs(t) do
        count = count + 1
    end
    return count
end

function print( ... )
    local t={...}
    for k,v in pairs(t) do
        t[k]=tostring(v)
    end
    local str=table.concat(t,"     ")
    ConsoleLog(str)
end

function ldv(t)
    local info={}
    _ldv(t,info,0,0)
    LogOnWnd(table.concat( info))
end

function _ldv(t,info,cnt,indent)
    local CNT_LIMIT=20
    for k,v in pairs(t) do
        for i=1,indent do table.insert(info,"    ") end
        table.insert(info,string.format("[%s]=%s\n",tostring(k),tostring(v)))

        cnt=cnt+1
        if cnt>CNT_LIMIT then break end

        if type(v)=="table" then
            _ldv(v,info,cnt,indent+1)
        end
    end
end

-------------------------------------外部调用
g_ExternalCalledFuncTable={}
function LuaRegisterFunc2CSarp(callName,func)
    if g_ExternalCalledFuncTable[callName] then
        error("重复注册g_ExternalCalledFuncTable")
    end
    g_ExternalCalledFuncTable[callName]=func
end
function ExternalCall(funcName,...)
    if not g_ExternalCalledFuncTable[funcName] then
        error("g_ExternalCalledFuncTable未定义："..funcName)
        return
    end
    g_ExternalCalledFuncTable[funcName](...)
end
