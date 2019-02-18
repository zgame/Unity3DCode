local BTNode = bt.BTNode
local BTCondition = class("BTCondition", BTNode)

--------------------
-- BTCondition【条件判断结点】 -- 椭圆形

function BTCondition:ctor(name, precondition, properties)
	BTNode.ctor(self, name, precondition, properties)
	self.assertValue = properties.assertValue		-- 没用
end


function BTCondition:tick(delta)
	local assertValue = handler(self.database, self.database[self.properties.assertFunc])()
	if assertValue then
		return bt.BTResult.Ended
	else
		return bt.BTResult.Running
	end
end

return BTCondition