----------------------------------------------------------------------------------------------------

-- 是否是Buffer类技能
function CheckSkillBuffer()
    return true
end

-- Buffer speed
function AddBufferSpeed()
    Speed = Speed + 1
    print("速度增加")
    return true
end


-- Buffer Attack
function AddBufferAttack()
    Damage = Damage + 1
    print("攻击增加")
    return true
end


-- Buffer Defence
function AddBufferDefence()
    Damage = Damage - 1
    print("防御增加")
    return true
end


-- Buffer ElementAttack
function AddBufferElementAttack()
    SkillDamage = SkillDamage + 10
    print("魔攻增加")
    return true
end


-- Buffer ElementDefence
function AddBufferElementDefence()
    SkillDamage = SkillDamage - 10
    print("魔防增加")
    return true
end

-- Buffer Accuracy
function AddBufferAccuracy()

    print("命中率增加")
    return true
end

-- Buffer Critical
function AddBufferCritical()

    print("暴击率增加")
    return true
end

-- Buffer CriticalDamage
function AddBufferCriticalDamage()

    print("暴击伤害增加")
    return true
end




----------------------------------------------------------------------------------------------------


-- 是否是deBuffer类技能
function CheckSkillDeBuffer()
    return true
end




-- Buffer speed
function AddDeBufferSpeed()
    Speed = Speed - 1
    print("速度降低")
    return true
end


-- Buffer Attack
function AddDeBufferAttack()
    Damage = Damage - 1
    print("攻击降低")
    return true
end


-- Buffer Defence
function AddDeBufferDefence()
    Damage = Damage + 1
    print("防御降低")
    return true
end


-- Buffer ElementAttack
function AddDeBufferElementAttack()
    SkillDamage = SkillDamage - 10
    print("魔攻降低")
    return true
end


-- Buffer ElementDefence
function AddDeBufferElementDefence()
    SkillDamage = SkillDamage + 10
    print("魔防降低")
    return true
end

-- Buffer Accuracy
function AddDeBufferAccuracy()

    print("命中率降低")
    return true
end

-- Buffer Critical
function AddDeBufferCritical()

    print("暴击率降低")
    return true
end

-- Buffer CriticalDamage
function AddDeBufferCriticalDamage()

    print("暴击伤害降低")
    return true
end



function AddDeBufferFreeze()

    print("冰冻")
    return true
end
function AddDeBufferBurn()

    print("灼烧")
    return true
end
function AddDeBufferBlood()

    print("流血")
    return true
end
function AddDeBufferSeal()

    print("封印")
    return true
end
function AddDeBufferStun()

    print("眩晕")
    return true
end
function AddDeBufferChaos()

    print("混乱")
    return true
end
function AddDeBufferParalysis()

    print("麻痹")
    return true
end
function AddDeBufferSleep()

    print("睡眠")
    return true
end






