using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    /// <summary>
    /// 飞船配置，通过AsteroidsShip.qtn链接到实体原型
    /// </summary>
    public class AsteroidsShipConfig : AssetObject
    {
        [Tooltip("The speed that the ship turns with added as torque")]
        public FP ShipTurnSpeed = 8; // 船体旋转速度（通过加扭矩实现）

        [Tooltip("The speed that the ship accelerates using add force")]
        public FP ShipAceleration = 6; // 船体前进加速度（沿自身 Up 方向施力）

        [Tooltip("Time interval between ship shots")]
        public FP FireInterval = FP._0_10; // 开火冷却时间（每次射击后重置）

        [Tooltip("Displacement of the projectile spawn position related to the ship position")]
        public FP ShotOffset = 1; // 子弹出生点相对船体的前向偏移

        [Tooltip("Prototype reference to spawn ship projectiles")]
        public AssetRef<EntityPrototype> ProjectilePrototype; // 用于生成子弹的原型引用
    }
}