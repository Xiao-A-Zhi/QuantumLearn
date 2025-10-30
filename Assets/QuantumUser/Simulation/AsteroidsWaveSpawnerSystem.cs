using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation
{
    [Preserve]
    public unsafe class AsteroidsWaveSpawnerSystem : SystemSignalsOnly // 继承自SystemSignalsOnly，表示该系统只响应信号
    {
        public override void OnInit(Frame frame)
        {
            // 初始化时生成第一波小行星
            SpawnAsteroidWave(frame);
        }

        /// <summary>
        /// 生成小行星实体
        /// </summary>
        public void SpawnAsteroid(Frame frame, AssetRef<EntityPrototype> childPrototype)
        {
            // 获取游戏配置，FindAsset效率较高，适合频繁访问的资源
            AsteroidsGameConfig config = frame.FindAsset(frame.RuntimeConfig.GameConfig);
            // 创建小行星实体
            EntityRef asteroid = frame.Create(childPrototype);
            // 获取小行星的Transform2D组件指针
            Transform2D* asteroidTransform = frame.Unsafe.GetPointer<Transform2D>(asteroid);

            // 设置小行星的旋转和位置
            asteroidTransform->Position = GetRandomEdgePointOnCircle(frame, config.AsteroidSpawnDistanceToCenter);
            asteroidTransform->Rotation = GetRandomRotation(frame);

            // Unsafe获取，获取PhysicsBody2D组件指针，更加高效，但不安全
            if (frame.Unsafe.TryGetPointer<PhysicsBody2D>(asteroid, out var body))
            {
                // 设置小行星的初始速度
                body->Velocity = asteroidTransform->Up * config.AsteroidInitialSpeed;
                // 设置小行星的初始扭矩，Frame.RNG用于生成随机数
                body->AddTorque(frame.RNG->Next(config.AsteroidInitialTorqueMin, config.AsteroidInitialTorqueMax));
            }
        }

        /// <summary>
        /// 根据当前波数生成一波小行星
        /// </summary>
        private void SpawnAsteroidWave(Frame frame)
        {
            AsteroidsGameConfig config = frame.FindAsset(frame.RuntimeConfig.GameConfig);
            // 根据当前波数和初始小行星数量生成小行星，frame.Global用于存储全局状态，在ECS系统不在单独的系统中赋予状态，因此需要使用frame.Global
            for (int i = 0; i < frame.Global->AsteroidsWaveCount + config.InitialAsteroidsCount; i++)
            {
                SpawnAsteroid(frame, config.AsteroidPrototype);
            }

            frame.Global->AsteroidsWaveCount++;
        }

        public static FP GetRandomRotation(Frame frame)
        {
            return frame.RNG->Next(0, 360);
        }

        public static FPVector2 GetRandomEdgePointOnCircle(Frame frame, FP radius)
        {
            return FPVector2.Rotate(FPVector2.Up * radius, frame.RNG->Next() * FP.PiTimes2);
        }
    }
}