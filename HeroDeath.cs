using System;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroHealth))]
    public class HeroDeath : MonoBehaviour
    {
        public HeroHealth Health;

        public HeroMove Move;
        public HeroAttack Attack;
        public HeroAnimator Animator;

        public GameObject DeathFX;
        private bool _heroIsDead;

        private void Start() =>
            Health.HealthChanged += HealthChanged;

        private void OnDestroy() =>
            Health.HealthChanged -= HealthChanged;

        private void HealthChanged()
        {
            if (!_heroIsDead && Health.Current <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            _heroIsDead = true;
            Move.enabled = false;
            Attack.enabled = false;
            Animator.PlayDeath();
            Instantiate(DeathFX, transform.position, Quaternion.identity);
        }
    }
}