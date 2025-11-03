using Photon.Deterministic;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation
{
    // 游戏全局配置：小行星生成与地图参数（使用 FP 固定点，保证锁步确定性）
    public class AsteroidsGameConfig : AssetObject
    {
        [Header("Asteroids configuration")]
        [Tooltip("Prototype reference to spawn asteroids")]
        public AssetRef<EntityPrototype> AsteroidPrototype; // 生成小行星时使用的实体原型引用
        [Tooltip("Speed applied to the asteroid when spawned")]
        public FP AsteroidInitialSpeed = 8; // 小行星生成时赋予的线速度（标量）
        [Tooltip("Minimum torque applied to the asteroid when spawned")]
        public FP AsteroidInitialTorqueMin = 7; // 生成时的最小扭矩/自旋
        [Tooltip("Maximum torque applied to the asteroid when spawned")]
        public FP AsteroidInitialTorqueMax = 20; // 生成时的最大扭矩/自旋
        [Tooltip("Distance to the center of the map. This value is the radius in a random circular location where the asteroid is spawned")]
        public FP AsteroidSpawnDistanceToCenter = 20; // 从地图中心的生成半径（在该半径的圆周/环带随机位置生成）
        [Tooltip("Amount of asteroids spawned in level 1. In each level, the number os asteroids spawned is increased by one")]
        public int InitialAsteroidsCount = 5; // 第1关小行星数量；每升1级数量+1
        
        [Header("Map configuration")]
        [Tooltip("Total size of the map. This is used to calculate when an entity is outside de gameplay area and then wrap it to the other side")]
        public FPVector2 GameMapSize = new FPVector2(25, 25); // 地图总尺寸（用于判断越界并实现屏幕穿越）
        public FPVector2 MapExtends => _mapExtends; // 地图半尺寸（方便边界计算）
        private FPVector2 _mapExtends;

        public override void Loaded(IResourceManager resourceManager, Native.Allocator allocator)
        {
            base.Loaded(resourceManager, allocator);
            _mapExtends = GameMapSize / 2; // 资源加载完成后预计算半尺寸，避免运行时重复计算
        }
    }
}