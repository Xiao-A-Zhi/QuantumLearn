using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation
{
    [Preserve]
    public unsafe class BoundarySystem : SystemMainThreadFilter<BoundarySystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform2D* Transform;
        }

        public override void Update(Frame frame, ref Filter filter)
        {
            var config = frame.FindAsset(frame.RuntimeConfig.GameConfig);
            if (IsOutOfBounds(filter.Transform->Position, config.MapExtends, out var newPosition))
            {
                filter.Transform->Teleport(frame, newPosition); // 屏幕穿越
            }
        }

        public bool IsOutOfBounds(FPVector2 position, FPVector2 mapExtends, out FPVector2 newPosition)
        {
            newPosition = position;

            if (position.X >= -mapExtends.X && position.X <= mapExtends.X &&
                position.Y >= -mapExtends.Y && position.Y <= mapExtends.Y)
            {
                return false;
            }

            if (position.X < -mapExtends.X)
            {
                newPosition.X = mapExtends.X;
            }
            else if (position.X > mapExtends.X)
            {
                newPosition.X = -mapExtends.X;
            }

            if (position.Y < -mapExtends.Y)
            {
                newPosition.Y = mapExtends.Y;
            }
            else if (position.Y > mapExtends.Y)
            {
                newPosition.Y = -mapExtends.Y;
            }

            return true;
        }
    }
}