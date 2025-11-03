using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation
{
    // ISignalSpawnAsteroid是一个自定义的信号接口，用于触发小行星生成事件
    [Preserve]
    public unsafe class AsteroidsWaveSpawnerSystem : SystemSignalsOnly, ISignalSpawnAsteroid,
        ISignalOnComponentRemoved<AsteroidsAsteroid>
    {
        public override void OnInit(Frame frame)
        {
            // 开始时生成第一波
            SpawnAsteroidWave(frame);
        }

        /// <summary>
        /// 生成单个小行星
        /// </summary>
        public void SpawnAsteroid(Frame frame, AssetRef<EntityPrototype> childPrototype, EntityRef parent)
        {
            AsteroidsGameConfig config = frame.FindAsset(frame.RuntimeConfig.GameConfig);
            EntityRef asteroid = frame.Create(childPrototype);
            Transform2D* asteroidTransform = frame.Unsafe.GetPointer<Transform2D>(asteroid);
            // 根据是否有父实体，决定小行星的生成位置
            if (parent == EntityRef.None)
            {
                asteroidTransform->Position = GetRandomEdgePointOnCircle(frame, config.AsteroidSpawnDistanceToCenter);
            }
            else
            {
                asteroidTransform->Position = frame.Get<Transform2D>(parent).Position;
            }

            asteroidTransform->Rotation = GetRandomRotation(frame);

            if (frame.Unsafe.TryGetPointer<PhysicsBody2D>(asteroid, out var body))
            {
                body->Velocity = asteroidTransform->Up * config.AsteroidInitialSpeed;
                body->AddTorque(frame.RNG->Next(config.AsteroidInitialTorqueMin, config.AsteroidInitialTorqueMax));
            }
        }

        /// <summary>
        /// 生成一波小行星
        /// </summary>
        private void SpawnAsteroidWave(Frame frame)
        {
            AsteroidsGameConfig config = frame.FindAsset(frame.RuntimeConfig.GameConfig);
            for (int i = 0; i < frame.Global->AsteroidsWaveCount + config.InitialAsteroidsCount; i++)
            {
                // 生成小行星，不需要指定父实体
                SpawnAsteroid(frame, config.AsteroidPrototype, EntityRef.None);
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

        public void OnRemoved(Frame frame, EntityRef entity, AsteroidsAsteroid* component)
        {
            if (frame.ComponentCount<AsteroidsAsteroid>() <= 1)
            {
                SpawnAsteroidWave(frame);
            }
        }
    }
}