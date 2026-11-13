namespace WaveShooter.Features.CamController
{
    using System.Collections;
    using UnityEngine;
    using WaveShooter.Features.Facade;

    /// <summary>
    /// Контроллер вращения камеры и персонажа.
    /// </summary>
    public class LookController : GenericModuleFacade<CharacterFacade>
    {
        [Header("Настройки чувствительности")]
        [SerializeField] protected float sensitivity = 0.1f;
        [SerializeField] protected float minPitch = -80f;
        [SerializeField] protected float maxPitch = 80f;

        [Header("Ссылки на персонажа и камеру")]
        [SerializeField] protected Transform characterBody;
        [SerializeField] protected Transform cameraPivot;

        private Vector2 lookVector = Vector2.zero;
        private float currentPitch = 0f;
        private Coroutine lookRoutine;

        protected override void OnInited()
        {
            if (characterBody == null) characterBody = transform;
            if (cameraPivot == null && Camera.main != null) cameraPivot = Camera.main.transform;

            if (cameraPivot != null)
                currentPitch = cameraPivot.localEulerAngles.x;
        }
        protected override void OnDisposed()
        {
            if (lookRoutine != null)
            {
                StopCoroutine(lookRoutine);
                lookRoutine = null;
            }
            lookVector = Vector2.zero;
        }

        /// <summary>
        /// Вызывается из InputActionLook
        /// </summary>
        public void SetLookVector(Vector2 vector)
        {
            lookVector = vector;

            if (lookVector.sqrMagnitude > 0.01f && lookRoutine == null)
            {
                lookRoutine = StartCoroutine(LookRoutine());
            }
        }

        protected IEnumerator LookRoutine()
        {
            while (lookVector.sqrMagnitude > 0.01f)
            {
                float mouseX = lookVector.x * sensitivity;
                float mouseY = lookVector.y * sensitivity;

                if (characterBody != null)
                    characterBody.Rotate(Vector3.up * mouseX);

                currentPitch -= mouseY;
                currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

                if (cameraPivot != null)
                    cameraPivot.localRotation = Quaternion.Euler(currentPitch, 0f, 0f);

                lookVector = Vector2.zero;
                
                yield return null; 
            }
            lookRoutine = null;
        }
    }
}