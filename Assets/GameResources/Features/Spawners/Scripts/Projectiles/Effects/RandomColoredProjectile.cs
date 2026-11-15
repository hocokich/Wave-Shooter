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
        [SerializeField] protected SpawnItem spawnItem;
        
        protected virtual void Awake()
        {
            if(rendererView == null)
                rendererView = GetComponent<Renderer>();

            if(spawnItem == null)
                spawnItem = GetComponent<SpawnItem>();
        }   

        protected virtual void OnEnable() => spawnItem.onLaunch += PaintRandom;
        protected virtual void OnDisable()
        {   
            if(spawnItem != null)
                spawnItem.onLaunch -= PaintRandom;
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
