namespace Unitskills.Features.Lesson6.Physics
{
    using Unitskills.Features.Lesson6.Spawners;

    /// <summary>
    /// Мишень. Вошедший кубик сразу завершает кулдаун.
    /// Удалить его или вернуть в пул решает спаунер.
    /// </summary>
    public class SpawnItemTrigger : AbstractGenericTrigger<SpawnItem>
    {
        protected override void OnTriggerEnterSuccessfully(SpawnItem targetComponent) => targetComponent.Complete();
    }
}
