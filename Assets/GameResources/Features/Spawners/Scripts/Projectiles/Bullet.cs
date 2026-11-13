
namespace WaveShooter.Features.Spawners
{
    using System;
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Префаб ждёт кулдаун и сообщает об этом.
    /// Удаление или возврат в пул решает тот, кто его заспаунил.
    /// </summary>
    public class Bullet : SpawnItem
    {
        /// <summary>
        /// Урон снаряда.
        /// </summary>
        public int Damage => damage;
        [SerializeField, Range(1, 100)] protected int damage = 50;
    }
}
