serpent = require("Utils.serpent")
require("Utils.global")
require("BehaviorTree3.BTInit")

require("BattleState")
require("BattleFunc")


--init btroot
local btroot
function init()
    local binder = Binder
    btroot = bt.loadFromJson("zsw2.json", binder)
    --btroot = bt.LoadFromLua("zsw1", binder)
    btroot:activate(binder)


    --print(serpent.block(btroot))
    --print("-------------------------------------------------------------")

end

--tick per frame 
function update(dt)
    print("-----------------------------------------------------------------------------------------------------------")
    if btroot:evaluate() then
        btroot:tick(dt)
    end
end

init()

for i = 1, RunTurn do
    update(0.1)
end
