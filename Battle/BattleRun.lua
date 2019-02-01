




-- 启动战斗
function RunBattle()
    local character = Character:New()
    character:showHp()


    local tank = CharacterTank:New()
    tank:showType()
    tank:showHp()
    local monster = Monster:New()
    monster:showType()
    monster:showHp()

end