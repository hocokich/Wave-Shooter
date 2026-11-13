namespace WaveShooter.Features.Spawners.Physics
{
    using WaveShooter.Features.Spawners;

    /// <summary>
    /// Мишень. Вошедший кубик сразу завершает кулдаун.
    /// Удалить его или вернуть в пул решает спаунер.
    /// </summary>
    public class SpawnItemTrigger : AbstractGenericTrigger<SpawnItem>
    {
        protected override void OnTriggerEnterSuccessfully(SpawnItem targetComponent) => targetComponent.Complete();
    }
}
