using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation
{
    // 统一处理小行星相关碰撞的系统
    [Preserve]
    public unsafe class AsteroidsCollisionsSystem : SystemSignalsOnly, ISignalOnCollisionEnter2D
    {
        /// <summary>
        /// 碰撞后调用对应的信号处理函数
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="info"></param>
        public void OnCollisionEnter2D(Frame frame, CollisionInfo2D info)
        {
            // Projectile is colliding with something
            if (frame.Unsafe.TryGetPointer<AsteroidsProjectile>(info.Entity, out var projectile))
            {
                if (frame.Unsafe.TryGetPointer<AsteroidsShip>(info.Other, out var ship))
                {
                    frame.Signals.OnCollisionProjectileHitShip(info, projectile, ship);
                }
                else if (frame.Unsafe.TryGetPointer<AsteroidsAsteroid>(info.Other, out var asteroid))
                {
                    frame.Signals.OnCollisionProjectileHitAsteroid(info, projectile, asteroid);
                }
            }

            // Ship is colliding with something
            else if (frame.Unsafe.TryGetPointer<AsteroidsShip>(info.Entity, out var ship))
            {
                if (frame.Unsafe.TryGetPointer<AsteroidsAsteroid>(info.Other, out var asteroid))
                {
                    frame.Signals.OnCollisionAsteroidHitShip(info, ship, asteroid);
                }
            }
        }
    }
}