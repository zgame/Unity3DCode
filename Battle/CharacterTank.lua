-- 坦克

CharacterTank = {}

function CharacterTank:New()
    o = Character:New()         -- 继承关系
    setmetatable(o, self)
    self.__index = self

    self.type = "Tank"
    return o
end

-- 函数重载
function CharacterTank:showHp()
    print("tank hp"..self.hp)
end


-- 自己独有函数
function CharacterTank:showType()
    print("type"..self.type)
end
