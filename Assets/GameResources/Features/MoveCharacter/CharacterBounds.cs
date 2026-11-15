namespace WaveShooter.Features.MoveCharacter
{
    using UnityEngine;

    /// <summary>
    /// Ограничивает перемещение персонажа в заданных координатах.
    /// </summary>
    public class CharacterBounds : MonoBehaviour
    {
        [Header("Границы перемещения")]
        [SerializeField] protected float limitX = 20f;
        [SerializeField] protected float limitZ = 20f;

        protected virtual void LateUpdate()
        {
            Vector3 clampedPosition = transform.position;

            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -limitX, limitX);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, -limitZ, limitZ);

            transform.position = clampedPosition;
        }
    }
}