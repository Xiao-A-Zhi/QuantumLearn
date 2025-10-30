using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation
{
    // 飞船控制系统，SystemMainThreadFilter表示该系统在主线程上运行，并且使用过滤器来选择特定的实体进行处理
    [Preserve]
    public unsafe class AsteroidsShipSystem : SystemMainThreadFilter<AsteroidsShipSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform2D* Transform;
            public PhysicsBody2D* Body;
            public AsteroidsShip* AsteroidsShip; // 标记为飞船实体，用于过滤
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            // gets the input for the player controlling this ship
            Input* input = default;
            if (frame.Unsafe.TryGetPointer<PlayerLink>(filter.Entity, out var playerLink))
            {
                input = frame.GetPlayerInput(playerLink->PlayerRef);
            }

            UpdateShipMovement(frame, ref filter, input);
        }

        private void UpdateShipMovement(Frame frame, ref Filter filter, Input* input)
        {
            FP shipAcceleration = 7;
            FP turnSpeed = 8;

            if (input->Up)
            {
                filter.Body->AddForce(filter.Transform->Up * shipAcceleration);
            }

            if (input->Left)
            {
                filter.Body->AddTorque(turnSpeed);
            }

            if (input->Right)
            {
                filter.Body->AddTorque(-turnSpeed);
            }

            filter.Body->AngularVelocity = FPMath.Clamp(filter.Body->AngularVelocity, -turnSpeed, turnSpeed);
        }
    }
}