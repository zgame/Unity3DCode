


-- 游戏状态

StateIdle = 1       -- 待机
StateRun = 2        -- 跑
StateAttack = 3     -- 攻击
StateSkill = 4      -- 技能
StateHit = 5        -- 挨打
StateDead = 6       -- 死亡


-- 玩家状态

PlayerState = nil   -- 当前状态
EnemyType = nil     -- 敌人

Distance = 5        -- 敌人距离
Speed = 1           -- 跑步速度
EnemyHP = 100
EnemyMaxHP = 100
HP = 100
MaxHP = 100
Damage = 2          -- 普通伤害
SkillDamage = 10    -- 技能伤害
ReHp = 50      -- 回血


-- Buffer
BufferSpeed = 1    -- 移动加速
BufferAttack = 2    -- 增加攻击
BufferDefence = 3    -- 增加防御
BufferElementAttack = 4    -- 增加元素攻击伤害
BufferElementDefence = 5    -- 增加元素防御力
BufferAccuracy = 6    -- 增加命中率
BufferCritical = 7    -- 增加暴击率
BufferCriticalDamage = 8    -- 增加暴击伤害



-- DeBuffer
DeBufferFreeze = 1  -- 冰冻
DeBufferBurn = 2    -- 灼烧
DeBufferBlood = 3    -- 流血
DeBufferSeal = 4    -- 封印
DeBufferStun = 5    -- 眩晕
DeBufferChaos = 6    -- 混乱
DeBufferParalysis = 7    -- 麻痹
DeBufferSleep = 8    -- 睡眠

DeBufferSpeed = 9    -- 减速
DeBufferAttack = 10    -- 虚弱 ，降低对方攻击
DeBufferDefence = 11    -- 破甲 ，降低对方防御
DeBufferElementAttack = 12    -- 魔功 ，降低对方元素攻击伤害
DeBufferElementDefence = 13    -- 魔防 ，降低对方元素防御力
DeBufferAccuracy = 14    -- 降低命中率
DeBufferCritical = 15    -- 降低暴击率
DeBufferCriticalDamage = 16    -- 降低暴击伤害














-- Run多少回合
RunTurn = 60



