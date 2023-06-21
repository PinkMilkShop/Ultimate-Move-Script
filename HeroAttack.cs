using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroAttack : MonoBehaviour, ISavedProgress
    {
        public HeroAnimator Animator;
        public CharacterController CharacterController;
        private IInputService _input;

        private static int _layerMask;
        private Collider[] _hits = new Collider[3];
        private HeroStats _stats;

        private void Awake()
        {
            _input = AllServices.Container.Single<IInputService>();

            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if (_input.AttackButtonUp() && !Animator.IsAttacking)
            {
                Animator.PlayAttack();
            }
        }

        void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
            {
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            }
        }

        public void LoadProgress(PlayerProgress progress) =>
            _stats = progress.HeroStats;

        private int Hit()
        {
            return Physics.OverlapSphereNonAlloc(StartPosition() + transform.forward, _stats.DamageRadius, _hits,
                _layerMask);
        }

        private Vector3 StartPosition()
        {
            return new Vector3(transform.position.x, CharacterController.center.y / 2, transform.position.z);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
        }
    }
}