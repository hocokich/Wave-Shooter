namespace WaveShooter.Features.Spawners.Projectiles
{
    using UnityEngine;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Spawners.Extenstions;

    /// <summary>
    /// Толкает снаряд вперёд от камеры. Вызывается после сброса скорости.
    /// </summary>
    public class ProjectileImpulse : MonoBehaviour
    {
        [SerializeField, Min(0)] protected float force = 16.0f;

        /// <summary>
        /// Добавить импульс по взгляду переданной камеры.
        /// </summary>
        public virtual void Apply(Camera cameraView)
        {
            Rigidbody body = GetComponent<Rigidbody>();
            ProviderCharacter pc = GetComponent<ProviderCharacter>();

            if (body == null || cameraView == null || pc == null)
            {
                return;
            }

            pc.Container.GetRayDirAndRot(out Vector3 direction, out Quaternion rotation);
            
            body.rotation = rotation;
            body.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}
