using System;
using CodeBase.Camera;
using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
    public class HeroMove : MonoBehaviour, ISavedProgress, ISavedProgressReader
    {
        public CharacterController CharacterController;
        public float movementSpeed;

        private IInputService _inputService;
        private UnityEngine.Camera _camera;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
        }

        private void Start()
        {
            _camera = UnityEngine.Camera.main;
        }

        private void Update()
        {
            Vector3 movementVector = Vector3.zero;
            if (_inputService.axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.axis);
                movementVector.y = 0f;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;

            CharacterController.Move(movementSpeed * movementVector * Time.deltaTime);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
            {
                Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
                if (savedPosition != null)
                {
                    WarpHeroPosition(to: savedPosition);
                }
            }
        }

        private void WarpHeroPosition(Vector3Data to)
        {
            CharacterController.enabled = false;
            transform.position = to.AsUnityVector().AddY(CharacterController.height);
            CharacterController.enabled = true;
        }

        private static string CurrentLevel()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}