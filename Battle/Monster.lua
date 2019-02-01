-- 怪物


Monster = {}

function Monster:New()
    o = Character:New()         -- 继承关系
    setmetatable(o, self)
    self.__index = self

    self.type = "Monster"
    return o
end

-- 函数重载
function Monster:showHp()
    print("monster hp"..self.hp)
end

-- 自己独有函数
function Monster:showType()
    print("type"..self.type)
end
