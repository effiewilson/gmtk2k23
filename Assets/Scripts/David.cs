using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class David : MonoBehaviour
    {
        public static David Instance => _instance;
        private static David _instance;

        public Rigidbody rb;
        
        private MainControls _controls;
        public float moveSpeed = 5f;
        
        private Vector2 _movement;

        private bool _buttonHeld;
        private bool _jumping;
        private bool _shaking;
        private bool _charging;


        private Sequence jumpSequence;

        private float _charge = 0f;


        public TextMeshProUGUI jumpDisplay;
        public ChargeDisplay chargeDisplay;
        
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

            jumpSequence = DOTween.Sequence();
            
        }
        
        
        private void OnEnable()
        {
            _controls.Enable();
            _controls.DefaultMap.Move.performed += MovementOnPerformed;
            _controls.DefaultMap.Move.canceled += MovementOnCanceled;
            _controls.DefaultMap.Confirm.performed += ChargePressed;
            _controls.DefaultMap.Confirm.canceled += ChargeReleased;
        }

        private void OnDisable()
        {
            _controls.DefaultMap.Move.performed -= MovementOnPerformed;
            _controls.DefaultMap.Move.canceled -= MovementOnCanceled;
            _controls.DefaultMap.Confirm.performed -= ChargePressed;
            _controls.DefaultMap.Confirm.canceled -= ChargeReleased;
            _controls.Disable();
        }
        
        
        private void MovementOnCanceled(InputAction.CallbackContext context)
        {
            if (_charging)
            {
                _movement = new Vector2();
            }
        }

        private void MovementOnPerformed(InputAction.CallbackContext context)
        {
            if (_charging)
            {
                _movement =  context.ReadValue<Vector2>();
            }
        }

        private void ResetRotation(InputAction.CallbackContext context)
        {
            // transform.DOShakeRotation(0.5f, 10, 20, 25);
            // transform.rotation = Quaternion.Euler(new Vector3());
            // rb.DOJump(transform.position, 3, 1, 1f);
        }

        private void ChargePressed(InputAction.CallbackContext context)
        {
            _buttonHeld = true;
            if (!_jumping)
            {
                _charging = true;
            }
            
            
            
            
            
            // jumpSequence.SetLoops()
            // jumpSequence = DOTween.Sequence()
            //     .Append(transform.DOShakeRotation(0.1f, 10, 20, 25, false, ShakeRandomnessMode.Harmonic))
            //     .SetLoops(-1);
            // .OnComplete(() => FinishShake());


        }
        
        private void ChargeReleased(InputAction.CallbackContext context)
        {
            _buttonHeld = false;
            _charging = false;
            if (!_jumping)
            {
                _jumping = true;
                jumpSequence = DOTween.Sequence();
                jumpSequence.Append(rb.DOJump(transform.position, 3, 1, 1f))
                    .Join(transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, _charge, 0), 1f))
                    .OnComplete(() =>
                        FinishJump());
            }


            // jumpSequence.SetLoops(1);
            // jumpSequence.Pause();
            // jumpSequence.Kill();



        }

        private void FixedUpdate()
        {
            if (!_jumping)
            {
                UpdateCharge();
            }
        }

        private void FinishJump()
        {
            _jumping = false;
            _charge = 0f;
            _movement = new Vector2();
            chargeDisplay.ClearSlider();
            if (_buttonHeld)
            {
                _charging = true;
            }
        }

        private void FinishShake()
        {
            _shaking = false;
        }


        private void UpdateCharge()
        {
            _charge += _movement.x * moveSpeed *10 * Time.fixedDeltaTime;
            _charge = Math.Clamp(_charge, -180, 180);
            
            jumpDisplay.text = _charge.ToString();
            if (_movement.x > -0.1 && _movement.x < 0.1)
            {
                DecayCharge();
            }
            
            chargeDisplay.UpdateSlider(_charge);
        }

        private void DecayCharge()
        {
            if (_charge > 0)
            {
                _charge = Math.Clamp(_charge - moveSpeed * 10 * Time.fixedDeltaTime, 0, _charge);
            }

            if (_charge < 0)
            {
                _charge = Math.Clamp(_charge + moveSpeed * 10 * Time.fixedDeltaTime, _charge, 0);
            }
        }
    }
}