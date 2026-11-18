namespace WaveShooter.Features.Enemy
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Раскрашивает врага в зависимости от скорости.
    /// </summary>
    public sealed class ColoringEnemy : MonoBehaviour
    {
        [SerializeField] private Color slowColor = Color.green;
        [SerializeField] private Color fastColor = Color.red;

        [SerializeField] private Renderer rendererView = default;
        private EnemyMovement enemyMovement = default;

        private void Awake()
        {
            enemyMovement = GetComponent<EnemyMovement>();

            enemyMovement.onSpeedChanged += SetColor;
        }
        private void OnDestroy()
        {
            if(enemyMovement != null)
            {
                enemyMovement.onSpeedChanged -= SetColor;
                enemyMovement = null;
            }
        }

        private void SetColor(float currentSpeed)
        {
            float minSpeed = enemyMovement.MinSpeed;
            float maxSpeed = enemyMovement.MaxSpeed;
            
            Coloring(minSpeed, maxSpeed, currentSpeed);
        }

        private void Coloring(float minSpeed, float maxSpeed, float currentSpeed)
        {
            float t = Mathf.InverseLerp(minSpeed, maxSpeed, currentSpeed);

            Color newColor = Color.Lerp(slowColor, fastColor, t);

            rendererView.material.SetColor("_BaseColor", newColor);
            rendererView.material.SetColor("_Color", newColor);
        }
    }
}