




-- 启动战斗
function RunBattle()
    local character = Character:New()
    character:showHp()


    local tank = CharacterTank:New()
    tank:showType()
    tank:showHp()
    tank:showHpC()
    local monster = Monster:New()
    monster:showType()
    monster:showHp()
    --monster:showHpC()



    print("-----------------------------")

    local parent1 = parent:new()
    parent1:ShowName()
    parent1:ShowTmp()
    parent1:Show()

    local child1 = child:new()
    child1:ShowName()
    child1:ShowTmp()
    child1:SuperName()


end