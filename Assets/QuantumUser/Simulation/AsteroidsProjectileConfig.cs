using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    // 子弹配置：生成速度与生存时间（使用 FP 固定点）
    public class AsteroidsProjectileConfig : AssetObject
    {
        [Tooltip("Speed applied to the projectile when spawned")]
        public FP ProjectileInitialSpeed = 15; // 子弹生成时的初速度（沿发射方向）

        [Tooltip("Time until destroy the projectile")]
        public FP ProjectileTTL = 1; // 子弹生存时间（到期自动销毁）
    }
}