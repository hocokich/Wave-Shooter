namespace Unitskills.Features.Lesson6.Physics
{
    using Unitskills.Features.Lesson6.Spawners;

    /// <summary>
    /// Мишень.
    /// Удаляется если по ней попали снарядом.
    /// </summary>
    public class DestroyTrigger : AbstractGenericTrigger<SpawnItem>
    {
        protected override void OnTriggerEnterSuccessfully(SpawnItem targetComponent) => Destroy(gameObject);
    }
}
