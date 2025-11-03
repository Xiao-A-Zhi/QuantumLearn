using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation
{
    // 防裁剪（IL2CPP/Linker）
    [Preserve]
    public unsafe class AsteroidsCollisionsSystem : SystemSignalsOnly, ISignalOnCollisionEnter2D
    {
        // 处理 2D 碰撞进入：按组件判断双方类型并分支处理
        public void OnCollisionEnter2D(Frame frame, CollisionInfo2D info)
        {
            // 本方是 Projectile
            if (frame.Unsafe.TryGetPointer<AsteroidsProjectile>(info.Entity, out _))
            {
                if (frame.Unsafe.TryGetPointer<AsteroidsShip>(info.Other, out _))
                {
                    // Projectile 击中 Ship（TODO：伤害/销毁/特效/计分）
                }
                else if (frame.Unsafe.TryGetPointer<AsteroidsAsteroid>(info.Other, out _))
                {
                    // Projectile 击中 Asteroid（TODO：分裂/破碎/小陨石生成）
                }
            }
            // 本方是 Ship
            else if (frame.Unsafe.TryGetPointer<AsteroidsShip>(info.Entity, out _))
            {
                if (frame.Unsafe.TryGetPointer<AsteroidsAsteroid>(info.Other, out _))
                {
                    // Asteroid 撞到 Ship（TODO：扣血/无敌帧/重生）
                }
            }
        }
    }
}