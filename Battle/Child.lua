child = class("child",parent)

function child:ctor()
    self.name = "child"
    self.tmp = 10
end


function child:ShowTmp()
    print(self.name.."  "..self.tmp)
end


function child:ShowName()
    print(self.name)
end


function child:SuperName()
    self.super.ShowName(self)
end