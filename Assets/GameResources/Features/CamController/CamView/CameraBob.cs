namespace WaveShooter.Features.CamController
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Input.InputModules;

    /// <summary>
    /// Покачивание головой при движении.
    /// </summary>
    public sealed class CameraBob : ProviderCharacter
    {
        [Header("Настройки покачивания")]
        [SerializeField, Range(6f, 12f)] private float Speed = 6f;
        [SerializeField, Range(0f, 0.2f)] private float Amount = 0.1f;
        [SerializeField, Range(1f, 2f)] private float sprintMultiplier = 1.5f;

        private Vector3 originPosition;
        private float bobTimer = 0f;
        private bool isSprinting = false;
        
        private Coroutine activeRoutine;
        private InputAction moveAction;
        private InputAction sprintAction;

        protected override void OnFacadeInited()
        {
            originPosition = transform.localPosition;

            var inputSys = Container.FacadeInstance.GetModuleFacade<InputSystemModule>();
            if (inputSys != null && inputSys.InputActions != null)
            {
                moveAction = inputSys.InputActions.Gameplay.Move;
                sprintAction = inputSys.InputActions.Gameplay.Sprint;

                moveAction.started += OnStartMoving;
                moveAction.canceled += OnStopMoving;

                sprintAction.started += OnSprintStart;
                sprintAction.canceled += OnSprintStop;
            }
        }

        protected override void OnFacadeDisposed()
        {
            if (moveAction != null)
            {
                moveAction.started -= OnStartMoving;
                moveAction.canceled -= OnStopMoving;
            }
            if (sprintAction != null)
            {
                sprintAction.started -= OnSprintStart;
                sprintAction.canceled -= OnSprintStop;
            }
            
            if (activeRoutine != null) StopCoroutine(activeRoutine);
            transform.localPosition = originPosition;
        }

        private void OnSprintStart(InputAction.CallbackContext context) => isSprinting = true;
        private void OnSprintStop(InputAction.CallbackContext context) => isSprinting = false;

        private void OnStartMoving(InputAction.CallbackContext context)
        {
            if (activeRoutine != null) StopCoroutine(activeRoutine);
            activeRoutine = StartCoroutine(BobRoutine());
        }

        private void OnStopMoving(InputAction.CallbackContext context)
        {
            if (activeRoutine != null) StopCoroutine(activeRoutine);
            activeRoutine = StartCoroutine(ReturnRoutine());
        }

        private IEnumerator BobRoutine()
        {
            while (true)
            {
                float currentSpeed = isSprinting ? Speed * sprintMultiplier : Speed;
                float currentAmount = isSprinting ? Amount * sprintMultiplier : Amount;

                bobTimer += Time.deltaTime * currentSpeed;

                float xOffset = Mathf.Sin(bobTimer) * currentAmount;
                float yOffset = Mathf.Cos(bobTimer * 2f) * currentAmount;

                transform.localPosition = originPosition + new Vector3(xOffset, yOffset, 0f);
                yield return null;
            }
        }

        private IEnumerator ReturnRoutine()
        {
            bobTimer = 0f;
            while (Vector3.Distance(transform.localPosition, originPosition) > 0.001f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, originPosition, Time.deltaTime * 10f);
                yield return null;
            }
            transform.localPosition = originPosition;
            activeRoutine = null;
        }
    }
}