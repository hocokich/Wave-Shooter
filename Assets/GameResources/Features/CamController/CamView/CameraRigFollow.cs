namespace WaveShooter.Features.CamController
{
    using UnityEngine;

    /// <summary>
    /// Прикрепляет камеру к игроку.
    /// </summary>
    public sealed class CameraRigFollow : MonoBehaviour
    {
        [SerializeField] private Transform target = default;
        [SerializeField] private Vector3 offset = new Vector3(0f, 0.75f, 0f);

        private void LateUpdate()
        {
            if (target == null) return;

            transform.position = target.position + target.TransformDirection(offset);
            transform.rotation = Quaternion.Euler(0f, target.eulerAngles.y, 0f);
        }
    }
}