parent = class("parent")

function parent:ctor()
    self.name = "parent"
    self.tmp = 0
end


function parent:ShowTmp()
    print(self.name.."  "..self.tmp)
end


function parent:ShowName()
    print(self.name)
end

function parent:Show()
    print("parent")

end