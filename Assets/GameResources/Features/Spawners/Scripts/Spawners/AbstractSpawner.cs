namespace WaveShooter.Features.Spawners
{
    using System;
    using UnityEngine;
    using WaveShooter.Features.Container;

    /// <summary>
    /// Абстрактный спаунер.
    /// Знает только про пул и жизненный цикл SpawnItem.
    /// </summary>
    public abstract class AbstractSpawner : ProviderCharacter
    {
        protected abstract AbstractPool Pool { get; }

        protected sealed override void OnDestroy()
        {
            OnFacadeDisposed();
            base.OnDestroy();
        }

        /// <summary>
        /// Взять из пула и запустить.
        /// </summary>
        protected SpawnItem SpawnFromPool(Vector3 position, Quaternion rotation, Action<SpawnItem> beforeLaunch = null)
        {
            if (Pool == null)
            {
                Debug.LogError($"Spawner '{name}': пул не задан", this);
                return null;
            }

            if (!Pool.HasFreeObject) return null;

            SpawnItem item = Pool.Get(position, rotation);
            if (item == null) return null;

            item.SetCooldownHandler(ReleaseItem);
            beforeLaunch?.Invoke(item);
            item.Launch();
            return item;
        }

        /// <summary>
        /// Возврат в пул.
        /// </summary>
        protected virtual void ReleaseItem(SpawnItem item)
        {
            if (item == null || Pool == null) return;
            Pool.Release(item);
        }
    }
}