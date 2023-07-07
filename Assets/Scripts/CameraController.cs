using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance => _instance;
        private static CameraController _instance;

        public Rigidbody rb;
        
        private MainControls _controls;
        public float moveSpeed = 5f;
        
        private Vector2 _movement;
        
        void Awake()
        {
            _controls = new MainControls();
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }
        
        
        private void OnEnable()
        {
            _controls.Enable();
            _controls.DefaultMap.Move.performed += MovementOnPerformed;
            _controls.DefaultMap.Move.canceled += MovementOnCanceled;
            // _controls.MainAction.Movement.performed += MovementOnPerformed;
            // _controls.MainAction.Movement.canceled += MovementOnCanceled;
            // _controls.MainAction.Interact.performed += Interact;
        }

        private void OnDisable()
        {
            // _controls.MainAction.Movement.performed -= MovementOnPerformed;
            // _controls.MainAction.Movement.canceled -= MovementOnCanceled;
            // _controls.MainAction.Interact.performed -= Interact;
            _controls.DefaultMap.Move.performed -= MovementOnPerformed;
            _controls.DefaultMap.Move.canceled -= MovementOnCanceled;
            _controls.Disable();
        }
        
        
        private void MovementOnCanceled(InputAction.CallbackContext context)
        {
            // movementController.StopMoving();
        }

        private void MovementOnPerformed(InputAction.CallbackContext context)
        {
            _movement =  context.ReadValue<Vector2>();

            // rb.MoveRotation(rb.rotation + movement * );
            // transform.Rotate();
            // movementController.Move(context.ReadValue<Vector2>());
        }

        private void FixedUpdate()
        {
            Vector3 movement = new Vector3(0, _movement.x, 0);
            Quaternion deltaRotation = Quaternion.Euler(movement * moveSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}