serpent = require("Utils.serpent")
require("Utils.global")
require("BehaviorTree3.BTInit")

require("BattleState")
require("BattleFunc")


--init btroot
local btroot
function init()
    local binder = {
        --isUnderControl = function()
        --    return true
        --end, targetValid = function()
        --    return true
        --end, notAimed = function()
        --    return true
        --end, isBeforeAttack = function()
        --    return true
        --end, isAfterAttack = function()
        --    return true
        --end,
        preseq = function()
            print("preseq")
            return true
        end,

        preshow1 = function()
            print("pre show 1")
            return true
        end,
        preshow2 = function()
            print("pre show 2")
            --return false
        end,

        prepri = function()
            print("prepri")
            return false
        end,


        run1 = function()
            print("run 1")
            return true
        end,
        run2 = function()
            print("run 2")
            return false
        end,
        run3 = function()
            print("run 3")
            return true
        end,
        run4 = function()
            print("run 4")
            return false
        end,

        wait10 = function()
            print("wait 10")
            return true
        end,

        show1 = function()
            print("show 1")
            return true
        end,
        show2 = function()
            print("show 2")
            return true
        end,

        assertFunc = function()
            print("assertFunc")
            return true
        end,

    }
    btroot = bt.loadFromJson("zsw1.json", binder)
    --btroot = bt.LoadFromLua("zsw1", binder)
    btroot:activate(binder)


    --print(serpent.block(btroot))
    print("-------------------------------------------------------------")

end

--tick per frame 
function update(dt)
    print("**********run****************")
    if btroot:evaluate() then
        --print("*******tick start*********")
        btroot:tick(dt)
        print("*******tick end*********")
    end
end

init()
print("=======================================================")

for i = 1, 16 do
    update(0.1)
end
