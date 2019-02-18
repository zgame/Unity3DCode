

local ai = {
  custom_nodes = {} --[[table: 0000000000359340]],
  display = {
    camera_x = 764,
    camera_y = 492.5,
    camera_z = 0.5,
    x = -972,
    y = -288
  } --[[table: 0000000000359300]],
  id = "cd875fde-1a5a-4116-810d-b3368cfafd8a",
  nodes = {
    ["0a1f0640-680d-48bf-8ea4-4e86dc16c5e6"] = {
      display = {
        x = 384,
        y = -360
      } --[[table: 00000000003569c0]],
      id = "0a1f0640-680d-48bf-8ea4-4e86dc16c5e6",
      name = "BTWaitAction",
      properties = {
        key = "afterAttackDelay",
        precondition = ""
      } --[[table: 0000000000356940]],
      title = "等待<key>时间"
    } --[[table: 0000000000356900]],
    ["1b19365e-8cf4-41f2-831d-dc3ee45f7627"] = {
      display = {
        x = -348,
        y = 156
      } --[[table: 0000000000358f40]],
      id = "1b19365e-8cf4-41f2-831d-dc3ee45f7627",
      name = "BTRunAction",
      properties = {
        operation = "setColddown",
        precondition = ""
      } --[[table: 0000000000356ec0]],
      title = "执行<operation>函数"
    } --[[table: 0000000000356e80]],
    ["1bff277f-95b5-4812-8043-f53b7f2a3f22"] = {
      display = {
        x = 144,
        y = -600
      } --[[table: 0000000000356380]],
      id = "1bff277f-95b5-4812-8043-f53b7f2a3f22",
      name = "BTRunAction",
      properties = {
        operation = "aim",
        precondition = ""
      } --[[table: 0000000000356340]],
      title = "执行<operation>函数"
    } --[[table: 00000000003562c0]],
    ["1ea46290-acf5-4438-b0b6-8700e028fb70"] = {
      children = {
        "281b6c4d-0657-4bad-8414-828d0aac1fe2",
        "f1c398a0-7766-43f5-a0e8-18c8e1073419",
        "1b19365e-8cf4-41f2-831d-dc3ee45f7627"
      } --[[table: 0000000000356c80]],
      display = {
        x = -660,
        y = 48
      } --[[table: 0000000000356c40]],
      id = "1ea46290-acf5-4438-b0b6-8700e028fb70",
      name = "BTSequence",
      properties = {
        operation = "",
        precondition = ""
      } --[[table: 0000000000356c00]],
      title = "Seq: 冷却时间"
    } --[[table: 0000000000356b80]],
    ["6cebbbac-4e37-49fc-859f-90e0c6ddd09c"] = {
      children = {
        "0a1f0640-680d-48bf-8ea4-4e86dc16c5e6",
        "7c72bcd2-9443-4288-9f48-d77e418f1ecf"
      } --[[table: 00000000003568c0]],
      display = {
        x = 132,
        y = -384
      } --[[table: 0000000000356880]],
      id = "6cebbbac-4e37-49fc-859f-90e0c6ddd09c",
      name = "BTSequence",
      properties = {
        operation = "",
        precondition = "isAfterAttack"
      } --[[table: 0000000000356840]],
      title = "Seq: 攻击后摇"
    } --[[table: 00000000003567c0]],
    ["7c72bcd2-9443-4288-9f48-d77e418f1ecf"] = {
      display = {
        x = 372,
        y = -276
      } --[[table: 0000000000356a80]],
      id = "7c72bcd2-9443-4288-9f48-d77e418f1ecf",
      name = "BTRunAction",
      properties = {
        operation = "setAfterAttack",
        precondition = ""
      } --[[table: 0000000000356a40]],
      title = "执行<operation>函数"
    } --[[table: 0000000000356a00]],
    ["7ecad81b-6c13-4a1f-a603-904200fa9161"] = {
      children = {
        "3756adc5-3721-42ba-8e80-f8c2ea19c7a9",
        "6cebbbac-4e37-49fc-859f-90e0c6ddd09c",
        "be6c59d4-8ebc-4fe5-8480-6880e2a87e6b"
      } --[[table: 00000000003564c0]],
      display = {
        x = -72,
        y = -408
      } --[[table: 0000000000356480]],
      id = "7ecad81b-6c13-4a1f-a603-904200fa9161",
      name = "BTPrioritySelector",
      properties = {
        precondition = ""
      } --[[table: 0000000000356440]],
      title = "Pri: 进入攻击"
    } --[[table: 00000000003563c0]],
    ["9e465abf-02a2-4dad-8ae1-dd7e9ff24cdc"] = {
      children = {
        "edadfbc7-45af-48ad-ba59-59a900663eaf",
        "1bff277f-95b5-4812-8043-f53b7f2a3f22"
      } --[[table: 0000000000356180]],
      display = {
        x = -48,
        y = -588
      } --[[table: 0000000000356140]],
      id = "9e465abf-02a2-4dad-8ae1-dd7e9ff24cdc",
      name = "BTParallel",
      properties = {
        precondition = "notAimed"
      } --[[table: 0000000000356100]],
      title = "Pal: 瞄准"
    } --[[table: 00000000003560c0]],
    ["9f1b34d7-66a9-4715-8a3d-d99362a5a35f"] = {
      children = {} --[[table: 0000000000355f40]],
      display = {
        x = -336,
        y = -684
      } --[[table: 0000000000340110]],
      id = "9f1b34d7-66a9-4715-8a3d-d99362a5a35f",
      name = "BTParallel",
      properties = {
        precondition = "isUnderControl"
      } --[[table: 00000000003400d0]],
      title = "Pal: 塔被控制"
    } --[[table: 0000000000340050]],
    ["9fcba779-b88b-479d-ad51-7673732d40f8"] = {
      display = {
        x = 0,
        y = -204
      } --[[table: 0000000000359140]],
      id = "9fcba779-b88b-479d-ad51-7673732d40f8",
      name = "BTRunAction",
      properties = {
        operation = "findTarget",
        precondition = ""
      } --[[table: 0000000000359100]],
      title = "执行<operation>函数"
    } --[[table: 00000000003590c0]],
    ["54a0e0b0-da89-4b3d-8743-e506d8e4f428"] = {
      children = {
        "9fcba779-b88b-479d-ad51-7673732d40f8",
        "eb0ccda9-b9fe-4e78-a33c-9ed902f510f1"
      } --[[table: 0000000000359080]],
      display = {
        x = -228,
        y = -156
      } --[[table: 0000000000359040]],
      id = "54a0e0b0-da89-4b3d-8743-e506d8e4f428",
      name = "BTParallel",
      properties = {
        precondition = "targetNotValid"
      } --[[table: 0000000000359000]],
      title = "Pal: 当前无锁定单位"
    } --[[table: 0000000000358f80]],
    ["281b6c4d-0657-4bad-8414-828d0aac1fe2"] = {
      display = {
        x = -372,
        y = -60
      } --[[table: 0000000000356d80]],
      id = "281b6c4d-0657-4bad-8414-828d0aac1fe2",
      name = "BTCondition",
      properties = {
        assertFunc = "notColddown",
        assertValue = "",
        precondition = ""
      } --[[table: 0000000000356d40]],
      title = "判断：是否冷却中"
    } --[[table: 0000000000356cc0]],
    ["762d0a1f-302f-45b7-8b23-787edf2286af"] = {
      display = {
        x = 360,
        y = -444
      } --[[table: 0000000000356780]],
      id = "762d0a1f-302f-45b7-8b23-787edf2286af",
      name = "BTRunAction",
      properties = {
        operation = "hurtTarget",
        precondition = ""
      } --[[table: 0000000000356740]],
      title = "执行<operation>函数"
    } --[[table: 0000000000356700]],
    ["3756adc5-3721-42ba-8e80-f8c2ea19c7a9"] = {
      children = {
        "a7cf511c-21e2-42b7-b501-b7f5aeb99a96",
        "a037b3ca-468b-4a3c-a63b-6dab1196967a",
        "762d0a1f-302f-45b7-8b23-787edf2286af"
      } --[[table: 00000000003565c0]],
      display = {
        x = 132,
        y = -468
      } --[[table: 0000000000356580]],
      id = "3756adc5-3721-42ba-8e80-f8c2ea19c7a9",
      name = "BTSequence",
      properties = {
        operation = "",
        precondition = "isBeforeAttack"
      } --[[table: 0000000000356540]],
      title = "Seq: 已冷却"
    } --[[table: 0000000000356500]],
    ["6602deb3-e491-47a5-bc3b-0cdb8f3b0317"] = {
      children = {
        "9e465abf-02a2-4dad-8ae1-dd7e9ff24cdc",
        "7ecad81b-6c13-4a1f-a603-904200fa9161"
      } --[[table: 0000000000356080]],
      display = {
        x = -288,
        y = -492
      } --[[table: 0000000000356040]],
      id = "6602deb3-e491-47a5-bc3b-0cdb8f3b0317",
      name = "BTPrioritySelector",
      properties = {
        precondition = "targetValid"
      } --[[table: 0000000000356000]],
      title = "Pri:当前有锁定单位"
    } --[[table: 0000000000355f80]],
    ["842198ed-a49e-4ec5-b75d-a6874d5ff07a"] = {
      children = {
        "9f1b34d7-66a9-4715-8a3d-d99362a5a35f",
        "6602deb3-e491-47a5-bc3b-0cdb8f3b0317",
        "54a0e0b0-da89-4b3d-8743-e506d8e4f428"
      } --[[table: 0000000000340010]],
      display = {
        x = -612,
        y = -552
      } --[[table: 000000000033ffd0]],
      id = "842198ed-a49e-4ec5-b75d-a6874d5ff07a",
      name = "BTPrioritySelector",
      properties = {
        precondition = ""
      } --[[table: 000000000033ff90]],
      title = "BTPrioritySelector"
    } --[[table: 000000000033ff10]],
    ["a7cf511c-21e2-42b7-b501-b7f5aeb99a96"] = {
      display = {
        x = 360,
        y = -588
      } --[[table: 00000000003592c0]],
      id = "a7cf511c-21e2-42b7-b501-b7f5aeb99a96",
      name = "BTRunAction",
      properties = {
        operation = "fire",
        precondition = ""
      } --[[table: 0000000000359280]],
      title = "执行<operation>函数"
    } --[[table: 0000000000359240]],
    ["a037b3ca-468b-4a3c-a63b-6dab1196967a"] = {
      display = {
        x = 384,
        y = -516
      } --[[table: 00000000003566c0]],
      id = "a037b3ca-468b-4a3c-a63b-6dab1196967a",
      name = "BTWaitAction",
      properties = {
        key = "beforeAttackDelay",
        precondition = ""
      } --[[table: 0000000000356640]],
      title = "等待<key>时间"
    } --[[table: 0000000000356600]],
    ["be6c59d4-8ebc-4fe5-8480-6880e2a87e6b"] = {
      display = {
        x = 144,
        y = -276
      } --[[table: 0000000000356b40]],
      id = "be6c59d4-8ebc-4fe5-8480-6880e2a87e6b",
      name = "BTRunAction",
      properties = {
        operation = "idle",
        precondition = ""
      } --[[table: 0000000000356b00]],
      title = "执行<operation>函数"
    } --[[table: 0000000000356ac0]],
    ["eb0ccda9-b9fe-4e78-a33c-9ed902f510f1"] = {
      display = {
        x = 0,
        y = -120
      } --[[table: 0000000000359200]],
      id = "eb0ccda9-b9fe-4e78-a33c-9ed902f510f1",
      name = "BTRunAction",
      properties = {
        operation = "idle",
        precondition = ""
      } --[[table: 00000000003591c0]],
      title = "执行<operation>函数"
    } --[[table: 0000000000359180]],
    ["edadfbc7-45af-48ad-ba59-59a900663eaf"] = {
      display = {
        x = 144,
        y = -672
      } --[[table: 0000000000356280]],
      id = "edadfbc7-45af-48ad-ba59-59a900663eaf",
      name = "BTWaitAction",
      properties = {
        key = "aimTime",
        precondition = ""
      } --[[table: 0000000000356240]],
      title = "等待<key>时间"
    } --[[table: 00000000003561c0]],
    ["ee4f070b-b52f-4f50-84ae-4c35999b11ec"] = {
      children = {
        "842198ed-a49e-4ec5-b75d-a6874d5ff07a",
        "1ea46290-acf5-4438-b0b6-8700e028fb70"
      } --[[table: 000000000033fed0]],
      display = {
        x = -816,
        y = -288
      } --[[table: 000000000033fe90]],
      id = "ee4f070b-b52f-4f50-84ae-4c35999b11ec",
      name = "BTParallel",
      properties = {
        precondition = "",
        shouldEndWhenOneEnd = "true"
      } --[[table: 000000000033fe10]],
      title = "BTParallel"
    } --[[table: 000000000033fdd0]],
    ["f1c398a0-7766-43f5-a0e8-18c8e1073419"] = {
      display = {
        x = -372,
        y = 72
      } --[[table: 0000000000356e40]],
      id = "f1c398a0-7766-43f5-a0e8-18c8e1073419",
      name = "BTWaitAction",
      properties = {
        key = "atkCD",
        precondition = ""
      } --[[table: 0000000000356e00]],
      title = "等待<key>时间"
    } --[[table: 0000000000356dc0]]
  } --[[table: 000000000033fd90]],
  properties = {} --[[table: 000000000033fd50]],
  root = "ee4f070b-b52f-4f50-84ae-4c35999b11ec",
  scope = "tree",
  title = "防御塔AI",
  version = "0.3.0"
} --[[table: 000000000033fd10]] 



 return ai