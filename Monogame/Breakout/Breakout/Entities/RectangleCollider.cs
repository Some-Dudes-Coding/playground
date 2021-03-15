using System;

using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace Breakout {
    class RectangleCollider : ICollisionActor {
        public event Action<CollisionEventArgs> OnCollisionDetected;

        public IShapeF Bounds { get; }

        public RectangleCollider(RectangleF bounds) {
            Bounds = bounds;
        }

        public void OnCollision(CollisionEventArgs collisionInfo) {
            OnCollisionDetected?.Invoke(collisionInfo);
        }
    }
}
