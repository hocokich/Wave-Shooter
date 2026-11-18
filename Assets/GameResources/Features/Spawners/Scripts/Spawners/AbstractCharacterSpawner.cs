namespace WaveShooter.Features.Spawners
{
    using WaveShooter.Features.Container;
    using UnityEngine;

    /// <summary>
    /// Спаунер персонажа.
    /// </summary>
    public abstract class AbstractCharacterSpawner : ProviderCharacter
    {
        [SerializeField] protected SpawnItem itemPrefab = default;

        [SerializeField, Min(0)] protected float spawnDistance = 11.0f;

        protected override void OnFacadeInited(){}

        protected sealed override void OnDestroy()
        {
            OnFacadeDisposed();
            base.OnDestroy();
        }
        protected abstract SpawnItem Spawn(Vector3 position, Quaternion rotation);

    }
}
