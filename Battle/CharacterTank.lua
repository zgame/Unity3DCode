-- 坦克

CharacterTank = {}

function CharacterTank:New()
    o = Character:New()         -- 继承关系

    local super_mt = getmetatable(o)
    setmetatable(CharacterTank, super_mt)   -- 当方法在子类中查询不到时，再去父类中去查找。
    o.super = setmetatable({}, super_mt) -- 这样设置后，可以通过self.super.method(self, ...) 调用父类的已被覆盖的方法。

    setmetatable(o, self)
    self.__index = self

    self.type = "Tank"

    return o
end

-- 函数重载
function CharacterTank:showHp()
    print("Tank ...")
    self.super.showHp(self)
    print("tank hp"..self.hp)
end


-- 自己独有函数
function CharacterTank:showType()
    print("type"..self.type)
end
