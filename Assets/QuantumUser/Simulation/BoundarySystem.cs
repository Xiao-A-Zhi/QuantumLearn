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
                // Teleport是Transform2D组件的方法，用于瞬移实体到新的位置
                filter.Transform->Teleport(frame, newPosition);
            }
        }

        public bool IsOutOfBounds(FPVector2 position, FPVector2 mapExtends, out FPVector2 newPosition)
        {
            newPosition = position;

            if (position.X >= -mapExtends.X && position.X <= mapExtends.X &&
                position.Y >= -mapExtends.Y && position.Y <= mapExtends.Y)
            {
                // 在边界内
                return false;
            }

            // 超出边界，计算新的位置
            if (position.X < -mapExtends.X)
            {
                newPosition.X = mapExtends.X;
            }
            else if (position.X > mapExtends.X)
            {
                newPosition.X = -mapExtends.X;
            }

            // 处理Y轴边界
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