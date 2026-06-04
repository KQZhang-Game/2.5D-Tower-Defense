一、项目简介
    本项目是一款 3D 塔防游戏，玩家通过部署不同类型的炮塔（高台/地面）阻止敌人抵达终点。动态阻挡塔 是游戏的特色机制——阻挡塔可以暂停进入其碰撞范围的敌人，并在自身被摧毁时释放所有被挡敌人，形成「拦截+恢复」的策略玩法。
   游戏使用 Unity 2022.3 开发，代码采用 MVC 分层架构，通过 XML 配置表驱动数值，并实现对象池、事件总线等设计模式。
二、核心玩法
    1.资源系统：每秒自动增加金币，建造炮塔消耗金币。
    2.炮塔类型：范围攻击塔，阻挡塔
    3.敌人类型：普通怪，重甲怪，快速怪
    4.胜负条件：胜利、所有敌人生成完毕且被消灭，玩家生命>0;失败：生命减至0。
三、技术架构
   1.代码分为五个主要层次，职责清晰：
   （1）View 层：包含 BasePanel、GamePanel、SettingPanel、WinPanel、LostPanel。负责 UI 显示与用户交互，不直接处理业务逻辑。
   （2）Controller 层：包括 GameManager、EnemyManager、HandleManager、BulletPool、PlatformManager、LineWriter、Move。负责游戏流程控制、单位生成与管理、对象池、移动逻辑等。
   （3）Model 层：包含 UnitBase（抽象基类）、TowerBase、EnemyBase、Bullet、DeployableCube、SpawnPoint 以及各种配置数据结构（TowerConfig、EnemyConfig、MusicData）。存放游戏数据实体和配置定义。
   （4）Services 层：AudioManager 和 GameDataManager。提供音频控制、配置加载与数据持久化服务。
   （5）Utils 层：Singleton、DamageCalculator、XmlDataMgr、PlayerPrefsTool 等。提供通用工具、伤害计算公式、XML 序列化辅助。
   2.模块解耦方式（UnityEvent + 外部订阅）
   目前没有引入全局事件总线，而是使用 Unity 原生的 UnityEvent 配合外部订阅实现模块间通信。具体做法：
   （1）每个需要对外通知的模块（如 EnemyManager）声明公有的 UnityEvent 成员（例如 OnWin、OnLost、OnEnterBlueDoor）。
   （2）外部模块（如 GameManager、WinPanel）在 Awake 或 Start 中通过 AddListener 订阅这些事件。
   （3）当内部条件满足时，模块调用 OnWin?.Invoke() 触发事件，所有订阅者按注册顺序执行。
   （4）为避免内存泄漏，订阅者在 OnDestroy 中必须调用 RemoveListener。
   3.对象池
   子弹池（BulletPool） 和 血条池（UIHpBarPool）：基于 UnityEngine.Pool.ObjectPool 实现，避免频繁创建和销毁带来的 GC 压力。子弹射出后经过一定延迟自动回收；血条跟随单位移动，单位销毁时回收到池中。
   4.配置驱动
   炮塔和敌人的数值存储在 XML 文件中（位于 StreamingAssets 目录）。XmlDataMgr 负责读写 XML，GameDataManager 在游戏启动时加载所有配置并缓存到字典中，供运行时快速查询。
四、阻挡机制说明
   1.设计目的：阻挡塔用于在关键位置拦截敌人，暂时阻止其继续前进，为其他输出型炮塔争取攻击时间。同时通过阻挡次数限制和死亡释放机制，避免出现无限拦截或敌人永久卡死的问题。
   2.实现方案
   （1）部署瞬间重叠检测：在 Deploy() 中主动调用 CheckOverlapEnemyOnDeploy()，使用 Physics.OverlapSphere 获取所有重叠敌人并立即处理，弥补 OnTriggerEnter 可能漏掉的情况。
   （2）有限阻挡次数：从配置表中读取 BlockCount，每成功阻挡一个敌人，blockCount 减 1；同时监听该敌人的 OnDead 事件，敌人死亡时 blockCount 加 1，实现可回收的拦截次数。
   （3）被挡敌人列表与恢复：使用 List<Move> 记录被当前塔暂停的敌人。当塔被摧毁时（OnDie 方法），遍历列表调用每个敌人的 ContinueMove()，恢复其移动。
   （4）多塔协作防重复：在处理新敌人时，先检查 moves.Contains(moveComp)，防止同一个敌人被多个塔重复暂停。
