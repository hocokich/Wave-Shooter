namespace WaveShooter.Features.Enemy
{
    using System;
    using WaveShooter.Features.Spawners;
    using WaveShooter.Features.Spawners.Physics;

    /// <summary>
    /// Уничтожение врага.
    /// </summary>
    public class DestroyEnemy : DestroyTrigger
    {
        public event Action onDestroyEnemy = delegate { };
        protected override void OnTriggerEnterSuccessfully(SpawnItem targetComponent)
        {
            onDestroyEnemy.Invoke();
            base.OnTriggerEnterSuccessfully(targetComponent);
        }
    }
}
