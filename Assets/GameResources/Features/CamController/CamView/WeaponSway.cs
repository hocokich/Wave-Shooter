namespace WaveShooter.Features.ShootingCharacter.View
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using WaveShooter.Features.Container;
    using WaveShooter.Features.Input.InputModules;

    /// <summary>
    /// Покачивание оружия при движении.
    /// </summary>
    public sealed class WeaponSway : ProviderCharacter
    {
        [Header("Настройки покачивания")]
        [SerializeField, Range(0f, 0.1f)] private float swayAmount = 0.03f;
        [SerializeField, Range(0f, 0.1f)] private float bobAmount = 0.03f;
        [SerializeField, Range(1f, 2f)] private float sprintMultiplier = 1.5f;

        private Vector3 originPos;
        private Quaternion originRot;
        
        private float timer;
        private float swayTimer;
        private bool isSprinting = false;
        private Quaternion targetSway = Quaternion.identity;

        private Coroutine bobRoutine;
        private Coroutine swayRoutine;
        
        private InputAction moveAction;
        private InputAction sprintAction;
        private InputAction lookAction;

        protected override void OnFacadeInited()
        {
            originPos = transform.localPosition;
            originRot = transform.localRotation;

            var inputSys = Container.FacadeInstance.GetModuleFacade<InputSystemModule>();
            if (inputSys != null && inputSys.InputActions != null)
            {
                moveAction = inputSys.InputActions.Gameplay.Move;
                sprintAction = inputSys.InputActions.Gameplay.Sprint;
                lookAction = inputSys.InputActions.Gameplay.Look;

                moveAction.started += OnStartMoving;
                moveAction.canceled += OnStopMoving;
                
                sprintAction.started += OnSprintStart;
                sprintAction.canceled += OnSprintStop;

                lookAction.performed += OnLook;
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
            if (lookAction != null)
            {
                lookAction.performed -= OnLook;
            }
            
            if (bobRoutine != null) StopCoroutine(bobRoutine);
            if (swayRoutine != null) StopCoroutine(swayRoutine);
            
            transform.localPosition = originPos;
            transform.localRotation = originRot;
        }

        private void OnSprintStart(InputAction.CallbackContext ctx) => isSprinting = true;
        private void OnSprintStop(InputAction.CallbackContext ctx) => isSprinting = false;

        private void OnLook(InputAction.CallbackContext context)
        {
            Vector2 delta = context.ReadValue<Vector2>();
            targetSway = Quaternion.AngleAxis(-delta.y * swayAmount, Vector3.right) * 
                        Quaternion.AngleAxis(delta.x * swayAmount, Vector3.up);
            
            swayTimer = 0.05f;
            
            if (swayRoutine == null)
                swayRoutine = StartCoroutine(SwayRoutine());
        }

        private void OnStartMoving(InputAction.CallbackContext context)
        {
            if (bobRoutine != null) StopCoroutine(bobRoutine);
            bobRoutine = StartCoroutine(BobRoutine());
        }

        private void OnStopMoving(InputAction.CallbackContext context)
        {
            if (bobRoutine != null) StopCoroutine(bobRoutine);
            bobRoutine = StartCoroutine(ReturnRoutine());
        }

        private IEnumerator SwayRoutine()
        {
            while (true)
            {
                if (swayTimer > 0)
                {
                    swayTimer -= Time.deltaTime;
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, originRot * targetSway, Time.deltaTime * 15f);
                }
                else
                {
                    targetSway = Quaternion.identity;
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, originRot, Time.deltaTime * 10f);
                    
                    if (Quaternion.Angle(transform.localRotation, originRot) < 0.1f)
                    {
                        transform.localRotation = originRot;
                        swayRoutine = null;
                        yield break;
                    }
                }
                yield return null;
            }
        }

        private IEnumerator BobRoutine()
        {
            while (true)
            {
                float amt = isSprinting ? bobAmount * sprintMultiplier : bobAmount;
                float speed = isSprinting ? 15f : 10f;

                timer += Time.deltaTime * speed;
                
                Vector3 targetPos = originPos + new Vector3(Mathf.Sin(timer) * amt, Mathf.Cos(timer * 2f) * amt, 0f);
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 15f);
                
                yield return null;
            }
        }

        private IEnumerator ReturnRoutine()
        {
            timer = 0f;
            while (Vector3.Distance(transform.localPosition, originPos) > 0.001f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, Time.deltaTime * 10f);
                yield return null;
            }
            transform.localPosition = originPos;
            bobRoutine = null;
        }
    }
}