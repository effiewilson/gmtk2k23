using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class SculptorMover : MonoBehaviour
    {
        Rigidbody rigidbody;
        public SculptorDestination destination;
        public float moveSpeed = 5f;

        public Animator animator;
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        [FormerlySerializedAs("sculpture")] public GameObject sculptureHypocentre;
        private static readonly int ChiselAnimation = Animator.StringToHash("chisel");

        public GameObject telegraph;
        
        private void Awake()
        {
            
            rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Vector3 destinationPosition = destination.transform.position;

            transform.LookAt(destination.transform);
            transform.position =
                Vector3.MoveTowards(transform.position, destinationPosition, moveSpeed * Time.deltaTime);
            if (!Arrived())
            {
                animator.ResetTrigger(ChiselAnimation);
                animator.SetBool(IsMoving, true);
                // transform.position =
                    // Vector3.MoveTowards(transform.position, destinationPosition, moveSpeed * Time.deltaTime);
                
                telegraph.SetActive(false);
            }
            else
            {
                telegraph.SetActive(true);
                animator.SetBool(IsMoving, false);
                transform.LookAt(sculptureHypocentre.transform);
            }
            
        }

        private bool Arrived()
        {
            return Vector3.Distance(transform.position, destination.transform.position) < 0.1;
        }

        private void Chisel()
        {
            animator.SetTrigger(ChiselAnimation);
            
        }
    }
}