using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation
{
    // 玩家生成系统，SystemSignalsOnly表示该系统只响应信号，类似于事件驱动，ISignalOnPlayerAdded是一个默认的信号接口，当有玩家加入时触发
    [Preserve]
    public unsafe class ShipSpawnSystem : SystemSignalsOnly, ISignalOnPlayerAdded
    {
        public void OnPlayerAdded(Frame frame, PlayerRef player, bool firstTime)
        {
            RuntimePlayer data = frame.GetPlayerData(player); // 读取玩家运行时数据（包含外观/原型引用）

            // 解析玩家飞船的原型引用（从资源表中查找）
            var entityPrototypAsset = frame.FindAsset<EntityPrototype>(data.PlayerAvatar);

            // 基于原型创建实体
            var shipEntity = frame.Create(entityPrototypAsset);

            // 绑定 PlayerLink 组件，标记该实体属于哪个玩家
            frame.Add(shipEntity, new PlayerLink() { PlayerRef = player });
        }
    }
}