local BTAction = bt.BTAction
local BTWaitAction = class("BTWaitAction", BTAction)

function BTWaitAction:enter()
	--BTAction.enter(self)
	self._duration = 0
	--self._endTime = self.database:getBattleValue(self.properties.key)
	self._endTime = self.properties.key
	--local ret = handler(self.database, self.database[self.properties.key])(self.timer, self.properties)

	--print("self._endTime,"..self._endTime)
end

function BTWaitAction:exit()
	--BTAction.exit(self)
end

function BTWaitAction:execute(delta)
	self._duration = self._duration + delta
	local result = self._duration < self._endTime and bt.BTResult.Running or bt.BTResult.Ended
	print("BTWaitAction 时间在等待 .... result  "..result)
	return result
		
end
return BTWaitAction