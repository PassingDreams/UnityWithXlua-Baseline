
function OnUpdate()
	--print("OnUpdate")
end

function OnUpdateEnd()
end


-----------------Input Handle
g_InputManager=CS.InputManager.Instance

function OnKeyDown()
	if g_InputManager:GetKey(EnumKeyCode.LeftControl) then
		if g_InputManager:GetKeyDown(EnumKeyCode.G) then
			CS.GMWnd.Instance:Toggle()
		end
	end
end


--uaRegisterFunc2CSarp("OnAwake",OnAwake)
--LuaRegisterFunc2CSarp("OnStart",OnStart)
LuaRegisterFunc2CSarp("OnUpdate","OnUpdate")
LuaRegisterFunc2CSarp("OnKeyDown","OnKeyDown")
LuaRegisterFunc2CSarp("OnUpdateEnd","OnUpdateEnd")
