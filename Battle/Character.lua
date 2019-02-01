
-- 角色总类

Character = {}
function Character:New()
    c = {
        attack = 1,         --攻击
        defence = 0,        -- 防御
        speed = 1,          -- 速度
        hp = 10,            -- 血量
        hpMax = 10,         -- 血量最大值

        critical = 1,       -- 暴击率
        miss = 1,           -- 闪避率
        breakArmor = 1,          -- 破甲
        reduce = 1,             -- 减伤

    }
    setmetatable(c, self)
    self.__index = self
    return c
end

function Character:showHp()
    print("hp"..self.hp)
end