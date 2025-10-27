using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation
{
    [Preserve]
    public unsafe class AsteroidsShipSystem : SystemMainThreadFilter<AsteroidsShipSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform2D* Transform;
            public PhysicsBody2D* Body;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            filter.Body->AddForce(filter.Transform->Up);
        }

        private void UpdateShipMovement(Frame frame, ref Filter filter, Input* input)
        {
            
        }
    }
}