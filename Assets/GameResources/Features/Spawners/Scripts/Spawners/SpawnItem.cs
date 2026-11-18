namespace WaveShooter.Features.Spawners
{
    using System;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Префаб ждёт кулдаун и сообщает об этом.
    /// Удаление или возврат в пул решает тот, кто его заспаунил.
    /// </summary>
    public class SpawnItem : MonoBehaviour
    {
        /// <summary>
        /// Запустили.
        /// </summary>
        public event Action onLaunch = delegate { };

        /// <summary>
        /// Удалили.
        /// </summary>
        public event Action onComplete = delegate { };

        [SerializeField, Min(0)] protected float cooldown = 2.0f;

        protected Coroutine coroutine = default;

        protected bool completed = false;

        protected Action<SpawnItem> cooldownHandler = delegate { };

        /// <summary>
        /// Подменить действие на конец кулдауна.
        /// </summary>
        /// <param name="handler">Удаление, возврат в пул или пустая операция.</param>
        public virtual void SetCooldownHandler(Action<SpawnItem> handler)
        {
            cooldownHandler = handler ?? delegate { };
        }

        /// <summary>
        /// Запустить кулдаун заново. Предыдущая корутина перезаписывается.
        /// </summary>
        public virtual void Launch()
        {
            onLaunch.Invoke();

            completed = false;
            ResetMotion();

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(WaitCooldown());
        }

        /// <summary>
        /// Закончить жизнь итема сразу и вызвать действие спаунера.
        /// </summary>
        public virtual void Complete()
        {
            if (completed)
            {
                return;
            }

            completed = true;

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            cooldownHandler(this);
            onComplete.Invoke();
        }

        protected virtual void OnDisable()
        {
            if (coroutine == null)
            {
                return;
            }

            StopCoroutine(coroutine);
            coroutine = null;
        }

        protected IEnumerator WaitCooldown()
        {
            yield return new WaitForSeconds(cooldown);
            coroutine = null;
            Complete();
        }

        protected virtual void ResetMotion()
        {
            Rigidbody body = GetComponent<Rigidbody>();

            if (body == null)
            {
                return;
            }

            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }
    }
}
