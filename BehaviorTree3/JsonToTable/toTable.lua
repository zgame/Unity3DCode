function io.readfile(path)
    local file = io.open(path, "rb")
    if file then
        local content = file:read("*a")
        io.close(file)
        return content
    end
    return nil
end
function io.writefile(path,data)
    local file = io.open(path, "wb")
    if file then
        file:write(data)
        file:flush()
        io.close(file)
        return true
    end
    return nil
end


function io.exists(path)
    local file = io.open(path, "r")
    if file then
        io.close(file)
        return true
    end
    return false
end


json = require("Json")
serpent = require("serpent")


-- 将json文件转换成lua格式
function jsonChangeToTable(name)
    local path = name..".json"
    if io.exists(path) then
        local data = json.decode(io.readfile(path))
        local block_str = serpent.block(data)

        --print(block_str)
        block_str = "\n\nlocal "..name.." = " .. block_str.." \n\n\n\n return "..name

        io.writefile(name..".lua",block_str)

    else
        print("load err")
    end
end


function main()
	print(arg[1])
    jsonChangeToTable(arg[1])     -- 转换ai文件
end

main()