require("Buffer")
-------------------------------发呆，跑动-----------------------------------------
function PlayerIdle()
    print("玩家发呆一下，马上敌人就出来了")
    return true
end

-- 距离的判断
function CheckDistance()
    if Distance > 1 then
        print("敌人距离好远， 我要跑过去")
        return true    -- 距离还远, 需要跑过去
    else
        print("敌人距离近了， 可以揍他了")
        return false
    end
end

-- 往目的地跑过去
function PlayerRun()
    if Distance > 1 then
        Distance = Distance - Speed
        print("跑步ing，距离:"..Distance)
        return false
    else
        print("跑到目的地")
        return true
    end
end

-- 生成敌人
function EnemyBorn()
    if EnemyType == nil then
        EnemyType = 1
        EnemyHP = EnemyMaxHP
        print("出现敌人")
    else
        print("已经有敌人，不用生成")
    end
end


-- 有没有敌人
function CheckEnemyNotExist()
    if EnemyType == nil then
        print("没有敌人")
        return true
    else
        print("敌人存在")
        return false
    end
end

-------------------------------攻击----------------------------------------

-- 敌人是否死亡
function CheckEnemyLive()
    if EnemyType == nil then
        print("没有敌人，不用攻击")
        return false
    end

    if EnemyHP > 0 then
        print("有敌人， 揍他")
        return true     -- 敌人没死，打他
    else
        print("敌人已经死了，不打了")
        EnemyType = nil
        return false
    end
end



-- 玩家普通攻击
function PlayerAttack()
    if EnemyHP > 0 then
        EnemyHP = EnemyHP - Damage
        print("敌人被[  普通攻击  ]打了，剩余血量"..EnemyHP)
    --else
    --    print("敌人被[ 普通攻击 ]杀死了")
    --    EnemyType = nil
    end
    return true
end

-- 玩家释放技能
function PlayerSkillAttack()
    if EnemyHP > 0 then
        EnemyHP = EnemyHP - SkillDamage
        print("敌人被[ 技能 ]打了，剩余血量"..EnemyHP)
    --else
    --    print("敌人被[ 技能 ]杀死了")
    --    EnemyType = nil
    end
    return true
end



-------------------------------玩家-----------------------------------------
-- 玩家受到攻击
function PlayerHit()
    if HP > 0 then
        HP = HP - Damage
        print("玩家 受到攻击, hp "..HP)
    else
        print("玩家 死了")
        PlayerState = StateDead
    end
    return true
end


-- 玩家是否已经死亡

function CheckPlayerAlive()
    if PlayerState == StateDead then
        print("玩家已经死亡")
        return false
    else
        print("玩家活着")
        return true
    end
end


-- 躲避
function RunAway()
    print("跑开躲避")
end

-------------------------------调试-----------------------------------------

-- 调试用开关
function GetTrue()
    return true
end

function GetFalse()
    return false
end





-------------------------------技能-----------------------------------------

-- 是否是技能是否带伤害
function CheckSkillHasDamage()
    return true
end

-- 是否是恢复类技能
function CheckSkillRecoverHp()
    return true
end

-- 恢复血量
function RecoverHp()
    local reHp = MaxHP - HP
    HP = HP + ReHp
    if HP > MaxHP then
        HP = MaxHP
    end
    print("成功恢复血量:"..reHp)
    return true
end



-------------------------------绑定关系-----------------------------------------

Binder = {

    PlayerIdle = PlayerIdle,
    CheckDistance = CheckDistance,
    PlayerRun = PlayerRun,
    CheckEnemyLive = CheckEnemyLive,
    CheckPlayerAlive = CheckPlayerAlive,
    EnemyBorn = EnemyBorn,
    CheckEnemyNotExist = CheckEnemyNotExist,

    GetTrue = GetTrue,
    GetFalse = GetFalse,
    RunAway = RunAway,

    PlayerAttack = PlayerAttack,
    PlayerSkillAttack = PlayerSkillAttack,
    PlayerHit = PlayerHit,


    -- skill
    CheckSkillHasDamage = CheckSkillHasDamage,
    CheckSkillRecoverHp = CheckSkillRecoverHp,
    RecoverHp = RecoverHp,
    CheckSkillBuffer = CheckSkillBuffer,
    CheckSkillDeBuffer = CheckSkillDeBuffer,

    -- buffer
    AddBufferSpeed = AddBufferSpeed,
    AddBufferAttack = AddBufferAttack,
    AddBufferDefence = AddBufferDefence,
    AddBufferElementAttack = AddBufferElementAttack,
    AddBufferElementDefence = AddBufferElementDefence,
    AddBufferAccuracy = AddBufferAccuracy,
    AddBufferCritical = AddBufferCritical,
    AddBufferCriticalDamage = AddBufferCriticalDamage,

    -- deBuffer
    AddDeBufferSpeed = AddDeBufferSpeed,
    AddDeBufferAttack = AddDeBufferAttack,
    AddDeBufferDefence = AddDeBufferDefence,
    AddDeBufferElementAttack = AddDeBufferElementAttack,
    AddDeBufferElementDefence = AddDeBufferElementDefence,
    AddDeBufferAccuracy = AddDeBufferAccuracy,
    AddDeBufferCritical = AddDeBufferCritical,
    AddDeBufferCriticalDamage = AddDeBufferCriticalDamage,
    AddDeBufferFreeze = AddDeBufferFreeze,
    AddDeBufferBurn = AddDeBufferBurn,
    AddDeBufferBlood = AddDeBufferBlood,
    AddDeBufferSeal = AddDeBufferSeal,
    AddDeBufferStun = AddDeBufferStun,
    AddDeBufferChaos = AddDeBufferChaos,
    AddDeBufferParalysis = AddDeBufferParalysis,
    AddDeBufferSleep = AddDeBufferSleep,



}

