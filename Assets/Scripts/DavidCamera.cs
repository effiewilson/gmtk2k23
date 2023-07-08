using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class DavidCamera : MonoBehaviour
    {
        public static DavidCamera Instance => _instance;
        private static DavidCamera _instance;
        
        private MainControls _controls;
        
        public float moveSpeed = 5f;
        private Vector2 _movement;

        public Rigidbody rotationParent;
        public GameObject pivotObject;
        
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
            // _controls.DefaultMap.Confirm.performed += ResetRotation;
        }

        private void OnDisable()
        {
            _controls.DefaultMap.Move.performed -= MovementOnPerformed;
            _controls.DefaultMap.Move.canceled -= MovementOnCanceled;
            _controls.Disable();
        }

        
        private void MovementOnPerformed(InputAction.CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>();
        }
        
        private void MovementOnCanceled(InputAction.CallbackContext context)
        {
            _movement = new Vector2();
        }
        
        private void FixedUpdate()
        {
            if (_movement.x > _movement.y * 2)
            {
                _movement.y = 0;
            }
            
            Vector3 movement = new Vector3(0, -1*_movement.x, 0);
            // Quaternion deltaRotation = Quaternion.Euler(movement * moveSpeed * Time.fixedDeltaTime);
            // rotationParent.transform.DORotate()
            // rotationParent.MoveRotation(rotationParent.rotation * deltaRotation);
            
            
            transform.RotateAround(pivotObject.transform.position, movement, moveSpeed * Time.fixedDeltaTime);
        }
        
    }
}