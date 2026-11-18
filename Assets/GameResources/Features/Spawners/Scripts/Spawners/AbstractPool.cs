namespace WaveShooter.Features.Spawners
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Абстрактный пул. 
    /// Свободные объекты лежат выключенными в очереди, занятые уходят в мир. 
    /// 
    /// Если пул пуст, то создаёт новый экземпляр префаба (если наследник не запретил).
    /// </summary>
    public abstract class AbstractPool : MonoBehaviour
    {
        [SerializeField] protected SpawnItem itemPrefab = default;
        [SerializeField, Min(0)] protected int preloadCount = 4;
        [SerializeField] protected Transform poolRoot = default;

        protected readonly Queue<SpawnItem> ready = new Queue<SpawnItem>();

        /// <summary>
        /// Есть ли свободный объект / можно ли создать новый.
        /// </summary>
        public virtual bool HasFreeObject => true;

        protected virtual void Awake()
        {
            if (poolRoot == null) poolRoot = transform;

            for (int i = 0; i < preloadCount; i++)
            {
                SpawnItem item = Create();
                if (item == null) return;
                ready.Enqueue(item);
            }
        }

        /// <summary>
        /// Возвращает экземпляр префаба.
        /// С указанной позицией и поворотом.
        /// 
        /// Вызывается в AbstractSpawner.
        /// </summary>
        public virtual SpawnItem Get(Vector3 position, Quaternion rotation)
        {
            SpawnItem item = ready.Count > 0 ? ready.Dequeue() : Create();
            if (item == null) return null;

            item.transform.SetParent(null);
            item.transform.SetPositionAndRotation(position, rotation);
            item.gameObject.SetActive(true);
            return item;
        }

        /// <summary>
        /// Возвращает объект в пул.
        /// Вызывается в AbstractSpawner.
        /// </summary>
        public virtual void Release(SpawnItem item)
        {
            if (item == null || !item.gameObject.activeSelf) return;

            item.gameObject.SetActive(false);
            item.transform.SetParent(poolRoot);
            ready.Enqueue(item);
        }

        protected virtual SpawnItem Create()
        {
            if (itemPrefab == null)
            {
                Debug.LogError($"Pool '{name}': префаб не задан", this);
                return null;
            }

            SpawnItem item = Instantiate(itemPrefab, poolRoot);
            item.gameObject.SetActive(false);
            return item;
        }
    }
}