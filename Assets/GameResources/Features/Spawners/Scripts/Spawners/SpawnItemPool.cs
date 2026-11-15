namespace WaveShooter.Features.Spawners
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Пул префабов. Свободные лежат выключенными, занятые выдаются в мир.
    /// Если свободных нет, создаётся ещё один.
    /// </summary>
    public class SpawnItemPool : MonoBehaviour
    {
        [SerializeField] protected SpawnItem itemPrefab = default;

        [SerializeField, Min(0)] protected int preloadCount = 4;

        [SerializeField] protected Transform poolRoot = default;

        protected Queue<SpawnItem> ready = new Queue<SpawnItem>();

        protected virtual void Awake()
        {
            if (poolRoot == null)
            {
                poolRoot = transform;
            }

            for (int index = 0; index < preloadCount; index++)
            {
                SpawnItem item = Create();

                if (item == null)
                {
                    return;
                }

                ready.Enqueue(item);
            }
        }

        /// <summary>
        /// Взять итем и поставить его в точку спауна.
        /// </summary>
        public virtual SpawnItem Get(Vector3 position, Quaternion rotation)
        {
            SpawnItem item = ready.Count > 0 ? ready.Dequeue() : Create();

            if (item == null)
            {
                return null;
            }

            item.transform.SetParent(null);
            item.transform.SetPositionAndRotation(position, rotation);
            item.gameObject.SetActive(true);
            return item;
        }

        /// <summary>
        /// Вернуть итем в пул.
        /// </summary>
        public virtual void Release(SpawnItem item)
        {
            if (item == null || !item.gameObject.activeSelf)
            {
                return;
            }

            item.gameObject.SetActive(false);
            item.transform.SetParent(poolRoot);
            ready.Enqueue(item);
        }

        protected virtual SpawnItem Create()
        {
            if (itemPrefab == null)
            {
                Debug.LogError("Префаб пула не задан");
                return null;
            }

            SpawnItem item = Instantiate(itemPrefab, poolRoot);
            item.gameObject.SetActive(false);
            return item;
        }
    }
}
