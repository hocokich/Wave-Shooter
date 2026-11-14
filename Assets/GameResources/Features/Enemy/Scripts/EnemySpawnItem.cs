namespace WaveShooter.Features.Enemy
{
    using WaveShooter.Features.Spawners;

    /// <summary>
    /// Префаб врага
    /// </summary>
    public class EnemySpawnItem : SpawnItem
    {
        public override void Launch()
        {
            cooldown = float.MaxValue;
            base.Launch();
        }
    }
}