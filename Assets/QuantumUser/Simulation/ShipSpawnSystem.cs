using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation
{
    // 玩家生成系统，SystemSignalsOnly表示该系统只响应信号，类似于事件驱动，ISignalOnPlayerAdded是一个信号接口，当有玩家加入时触发
    [Preserve]
    public unsafe class ShipSpawnSystem : SystemSignalsOnly, ISignalOnPlayerAdded
    {
        public void OnPlayerAdded(Frame frame, PlayerRef player, bool firstTime)
        {
            RuntimePlayer data = frame.GetPlayerData(player);

            // resolve the reference to the avatar prototype.
            var entityPrototypAsset = frame.FindAsset<EntityPrototype>(data.PlayerAvatar);

            // Create a new entity for the player based on the prototype.
            var shipEntity = frame.Create(entityPrototypAsset);

            // Create a PlayerLink component. Initialize it with the player. Add the component to the player entity.
            frame.Add(shipEntity, new PlayerLink() { PlayerRef = player });
        }
    }
}