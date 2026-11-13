namespace WaveShooter.Features.Spawners.Physics
{
    using WaveShooter.Features.Spawners;

    /// <summary>
    /// Мишень.
    /// Удаляется если по ней попали снарядом.
    /// </summary>
    public class DestroyTrigger : AbstractGenericTrigger<SpawnItem>
    {
        protected override void OnTriggerEnterSuccessfully(SpawnItem targetComponent) => Destroy(gameObject);
    }
}
