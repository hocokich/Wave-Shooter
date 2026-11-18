namespace WaveShooter.Features.Spawners.Projectiles.Effects
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Эффект уменьшения снаряда.
    /// </summary>
    public class ShrinkingProjectile : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 1)] protected float shrinkSpeed = 0.5f;
        [SerializeField] protected SpawnItem spawnItem;
        
        protected Coroutine shrinkCoroutine = default;
        protected Vector3 originalScale = default;

        protected virtual void Awake()
        {
            if (spawnItem == null)
                spawnItem = GetComponent<SpawnItem>();

            originalScale = transform.localScale;
        }
        
        protected virtual void OnEnable()
        {
            spawnItem.onLaunch += StartShrink;
            spawnItem.onComplete += StopShrink;
        }
        protected virtual void OnDisable()
        {   
            StopShrink();

            if(spawnItem != null)
            {
                spawnItem.onLaunch -= StartShrink;
                spawnItem.onComplete -= StopShrink;
            }
        }

        protected virtual void StartShrink()
        {
            if(shrinkCoroutine != null)
            {
                StopCoroutine(shrinkCoroutine);
                shrinkCoroutine = null;
            }
            shrinkCoroutine = StartCoroutine(Shrinking());
        }
        protected virtual void StopShrink()
        {
            if(shrinkCoroutine != null)
            {
                StopCoroutine(shrinkCoroutine);
                shrinkCoroutine = null;
            }
            
            transform.localScale = originalScale;
        }
        protected virtual IEnumerator Shrinking()
        {
            float shrink;
            Vector3 step;

            while (transform.localScale.x > 0f)
            {
                shrink = Time.deltaTime * shrinkSpeed;
                step = new Vector3(shrink, shrink, shrink);
                
                transform.localScale = Vector3.Max(Vector3.zero, transform.localScale - step);
                
                yield return null;
            }
        }
    }
}
