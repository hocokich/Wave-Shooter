namespace WaveShooter.Features.Spawners.Projectiles
{
    using UnityEngine;

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
            ProviderCharacterComponent pcc = GetComponent<ProviderCharacterComponent>();

            if (body == null || cameraView == null || pcc == null)
            {
                return;
            }

            pcc.Container.GetRayDirAndRot(out Vector3 direction, out Quaternion rotation);
            
            body.rotation = rotation;
            body.AddForce(direction * force, ForceMode.Impulse);
        }
    }
}
