using Quantum.QuantumUser.Simulation;

namespace Quantum
{
    public partial class RuntimeConfig
    {
        public AssetRef<AsteroidsGameConfig> GameConfig; // 运行时引用的游戏配置资源（小行星与地图参数）
    }
}