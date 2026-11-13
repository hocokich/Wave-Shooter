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
        [SerializeField] protected SpawnItem warningSpawnItem;
        
        protected Coroutine shrinkCoroutine = default;
        protected Vector3 originalScale = default;

        protected virtual void Awake() => originalScale = transform.localScale;
        protected virtual void Start()
        {
            if (warningSpawnItem == null)
                warningSpawnItem = GetComponent<SpawnItem>();
        }
        protected virtual void OnEnable()
        {
            warningSpawnItem.onLaunch += StartShrink;
            warningSpawnItem.onComplete += StopShrink;
        }
        protected virtual void OnDisable()
        {   
            StopShrink();

            if(warningSpawnItem != null)
            {
                warningSpawnItem.onLaunch -= StartShrink;
                warningSpawnItem.onComplete -= StopShrink;
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
