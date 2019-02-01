

-- 计算伤害
function DamageCalculate(attack, defence, hp)
    local damage = attack - defence
    if hp > damage then
        hp = hp - damage
    else
        hp = 0
    end
    return damage, hp
end



