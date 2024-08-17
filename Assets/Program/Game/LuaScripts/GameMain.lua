
function OnUpdate()
	--print("OnUpdate")
end

function OnUpdateEnd()

end


function OnKeyDown(keycode)
	print(keycode) --TODO:把keycode枚举搬过来，然后做一份lookup table，在每帧结束清理下
end


--uaRegisterFunc2CSarp("OnAwake",OnAwake)
--LuaRegisterFunc2CSarp("OnStart",OnStart)
LuaRegisterFunc2CSarp("OnUpdate","OnUpdate")
LuaRegisterFunc2CSarp("OnKeyDown","OnKeyDown")
LuaRegisterFunc2CSarp("OnUpdateEnd","OnUpdateEnd")
