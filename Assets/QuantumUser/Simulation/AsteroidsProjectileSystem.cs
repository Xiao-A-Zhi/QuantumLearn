using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation
{
    [Preserve]
    public unsafe class AsteroidsProjectileSystem : SystemMainThreadFilter<AsteroidsProjectileSystem.Filter>,
        ISignalAsteroidsShipShoot, ISignalOnCollisionProjectileHitShip, ISignalOnCollisionProjectileHitAsteroid
    {
        // 筛选：遍历子弹实体
        public struct Filter
        {
            public EntityRef Entity;
            public AsteroidsProjectile* Projectile; // 子弹配置Component，可用于筛选子弹实体，含 TTL/Owner
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            // 递减寿命，<=0 则销毁
            filter.Projectile->TTL -= frame.DeltaTime;
            if (filter.Projectile->TTL <= 0)
            {
                frame.Destroy(filter.Entity);
            }
        }

        // 飞船发射子弹信号回调
        public void AsteroidsShipShoot(Frame frame, EntityRef owner, FPVector2 spawnPosition,
            AssetRef<EntityPrototype> projectilePrototype)
        {
            var projectileEntity = frame.Create(projectilePrototype);
            Transform2D* projectileTransform = frame.Unsafe.GetPointer<Transform2D>(projectileEntity);
            Transform2D* ownerTransform = frame.Unsafe.GetPointer<Transform2D>(owner);

            projectileTransform->Rotation = ownerTransform->Rotation;
            projectileTransform->Position = spawnPosition;

            AsteroidsProjectile* projectile = frame.Unsafe.GetPointer<AsteroidsProjectile>(projectileEntity);
            var config = frame.FindAsset(projectile->ProjectileConfig);
            projectile->TTL = config.ProjectileTTL;
            projectile->Owner = owner;

            PhysicsBody2D* body = frame.Unsafe.GetPointer<PhysicsBody2D>(projectileEntity);
            body->Velocity = ownerTransform->Up * config.ProjectileInitialSpeed; // 沿朝向发射
        }

        // 子弹与飞船碰撞信号回调
        public void OnCollisionProjectileHitShip(Frame frame, CollisionInfo2D info, AsteroidsProjectile* projectile,
            AsteroidsShip* ship)
        {
            if (projectile->Owner == info.Other) // 避免子弹击中自己
            {
                info.IgnoreCollision = true;
                return;
            }

            frame.Destroy(info.Entity);
        }

        // 子弹与小行星碰撞信号回调
        public void OnCollisionProjectileHitAsteroid(Frame frame, CollisionInfo2D info, AsteroidsProjectile* projectile,
            AsteroidsAsteroid* asteroid)
        {
            // 如果有子小行星的ProtoType，则触发创建小行星的信号，并将当前碰撞的小行星实体作为父实体传入
            if (asteroid->ChildAsteroid != null)
            {
                frame.Signals.SpawnAsteroid(asteroid->ChildAsteroid, info.Other);
                frame.Signals.SpawnAsteroid(asteroid->ChildAsteroid, info.Other);
            }

            frame.Destroy(info.Entity);
            frame.Destroy(info.Other);
        }
    }
}