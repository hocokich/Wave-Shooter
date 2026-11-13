namespace WaveShooter.Features.Spawners.Projectiles.Effects
{
    using UnityEngine;

    /// <summary>
    /// Эффект. 
    /// Снаряд рандомно красится при выстреле.
    /// </summary>
    public class RandomColoredProjectile : MonoBehaviour
    {
        [SerializeField] protected Renderer rendererView = default;
        [SerializeField] protected SpawnItem warningSpawnItem;
        
        protected virtual void Awake()
        {
            if(rendererView == null)
                rendererView = GetComponent<Renderer>();
        }   
        protected virtual void Start()
        {
            if (warningSpawnItem == null)
                warningSpawnItem = GetComponent<SpawnItem>();
        }
        protected virtual void OnEnable() => warningSpawnItem.onLaunch += PaintRandom;
        protected virtual void OnDisable()
        {   
            if(warningSpawnItem != null)
                warningSpawnItem.onLaunch -= PaintRandom;
        }

        protected virtual void PaintRandom()
        {
            if (rendererView == null)
            {
                return;
            }

            Color color = Random.ColorHSV();
            rendererView.material.SetColor("_BaseColor", color);
            rendererView.material.SetColor("_Color", color);
        }

    }
}
