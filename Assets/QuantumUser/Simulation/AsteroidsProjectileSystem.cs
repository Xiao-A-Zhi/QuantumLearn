using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation
{
    // 子弹系统类，用于触发创建子弹实体的逻辑
    [Preserve]
    public unsafe class
        AsteroidsProjectileSystem : SystemMainThreadFilter<AsteroidsProjectileSystem.Filter>,
        ISignalAsteroidsShipShoot // ISignalAsteroidsShipShoot接口是用来处理飞船射击信号的，属于自己定义的信号接口，在AsteroidsProjectile.qtn中定义AsteroidsShipShoot，通过接口实现AsteroidsShipShoot方法，在实际的逻辑System中调用
    {
        // 子弹实体筛选器
        public struct Filter
        {
            public EntityRef Entity;
            public AsteroidsProjectile* Projectile;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            // 每帧检测子弹的存活时间，如果时间到则销毁子弹实体
            filter.Projectile->TTL -= frame.DeltaTime;
            if (filter.Projectile->TTL <= 0)
            {
                frame.Destroy(filter.Entity);
            }
        }

        public void AsteroidsShipShoot(Frame frame, EntityRef owner, FPVector2 spawnPosition,
            AssetRef<EntityPrototype> projectilePrototype)
        {
            // 创建子弹实体
            EntityRef projectileEntity = frame.Create(projectilePrototype);
            // 获取子弹的Transform2D组件指针
            Transform2D* projectileTransform = frame.Unsafe.GetPointer<Transform2D>(projectileEntity);
            // 获取拥有者（发射子弹的飞船）的Transform2D组件指针
            Transform2D* ownerTransform = frame.Unsafe.GetPointer<Transform2D>(owner);

            // 设置子弹的旋转和位置
            projectileTransform->Rotation = ownerTransform->Rotation;
            projectileTransform->Position = spawnPosition;

            // 初始化子弹的属性
            AsteroidsProjectile* projectile = frame.Unsafe.GetPointer<AsteroidsProjectile>(projectileEntity);
            var config = frame.FindAsset(projectile->ProjectileConfig);
            // 设置子弹的存活时间和拥有者
            projectile->TTL = config.ProjectileTTL;
            projectile->Owner = owner;

            // 设置子弹的初始速度
            PhysicsBody2D* body = frame.Unsafe.GetPointer<PhysicsBody2D>(projectileEntity);
            body->Velocity = ownerTransform->Up * config.ProjectileInitialSpeed;
        }
    }
}