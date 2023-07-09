using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class SculptorMover : MonoBehaviour
    {
        //public SculptorDestination destination;
        public float moveSpeed = 5f;

        public Animator animator;
        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        [FormerlySerializedAs("sculpture")] public GameObject sculptureHypocentre;
        private static readonly int ChiselAnimation = Animator.StringToHash("chisel");

        public GameObject chiselTelegraph;
        public GameObject sliceTelegraph;

        public Transform statueTarget;
        public float offsetRadius = 4f;
        private Vector3 _destinationOffset;

        private float _warningTimer;
        public float chiselWarningTime = 3f; 
        public float sliceWarningTime = 5f;

        private bool _hasMovementTarget;
        private bool _chiselling;
        private bool _slicing;

        private Vector3 _slicePlaneCentre; 
        private Plane _slicePlane;
        private Ray _chiselRay;

        private void Awake()
        {
            
        }

        private void FixedUpdate()
        {
            if (statueTarget == null)
            {
                transform.LookAt(sculptureHypocentre.transform, Vector3.up);
            }


            //If attacking, all we need to know is that the telegraphs are on and the timer is running.
            if (_chiselling || _slicing)
            {
                _hasMovementTarget = false;
                animator.SetBool(IsMoving, false);
                _warningTimer += Time.fixedDeltaTime;

                if (_chiselling && _warningTimer > chiselWarningTime)
                {
                    ChiselStatue();
                    _chiselling = false;
                }
                else if (_chiselling && chiselWarningTime - _warningTimer < 1f)
                {
                    chiselTelegraph.GetComponent<LineRenderer>().SetColors(Color.red, Color.red);
                }
                if (_slicing && _warningTimer > sliceWarningTime)
                {
                    SliceStatue();
                    _slicing = false;
                }
                else if (_slicing && sliceWarningTime - _warningTimer < 1f)
                {
                    sliceTelegraph.GetComponent<LineRenderer>().SetColors(Color.red, Color.red);
                }
            }
           
            //Otherwise, we should be moving somewhere
            else if (!_hasMovementTarget)
            {
                GenerateNewTargetOffset();
                _hasMovementTarget = true;
                //Good time to switch things off, because we'll be moving until we get to it
                chiselTelegraph.SetActive(false);
                sliceTelegraph.SetActive(false);
            }
            //If we're not attackand and we do have a target, but we've not arrived, head forward.
            else if (!Arrived())
            {
                var relDest = new Vector3(statueTarget.position.x, 0, statueTarget.position.z) + _destinationOffset;
                Debug.DrawLine(transform.position, relDest);
                transform.LookAt(relDest, Vector3.up);

                animator.ResetTrigger(ChiselAnimation);
                animator.SetBool(IsMoving, true);
                transform.position += transform.forward * Time.fixedDeltaTime;
            }
            //Otherwise, set up attack
            else
            {
                var vol = statueTarget.GetComponent<MeshVolumeTracker>().FractionalVolume;
                _slicing = vol*vol > Random.value;
                _chiselling = !_slicing;
                _warningTimer = 0;

                if (_chiselling) {
                    //We need a random ray for where we chisel

                    var lr = chiselTelegraph.GetComponent<LineRenderer>();
                    _chiselRay = new Ray(transform.position, (statueTarget.position + Random.insideUnitSphere)-transform.position);
                    lr.SetColors(Color.blue, Color.blue);
                    lr.SetPosition(0, _chiselRay.origin);
                    lr.SetPosition(1, _chiselRay.origin+_chiselRay.direction*15f);
                    chiselTelegraph.SetActive(true);
                    transform.LookAt(new Vector3(statueTarget.position.x, 0, statueTarget.position.z), Vector3.up);
                }
                if (_slicing)
                {
                    var lr = sliceTelegraph.GetComponent<LineRenderer>();
                    lr.SetColors(Color.blue, Color.blue);
                    _slicePlaneCentre = statueTarget.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
                    transform.LookAt(new Vector3(_slicePlaneCentre.x, transform.position.y, _slicePlaneCentre.z), Vector3.up);
                    Vector3 randomOrientation = new Vector3(Random.Range(89f, 91f), Random.Range(-80f, 100f), transform.eulerAngles.y + Random.Range(-1, 1));
                    _slicePlane = new Plane(randomOrientation, _slicePlaneCentre);




                    lr.SetPositions(GetLinePointsShowingPlane(_slicePlaneCentre, _slicePlane.normal, 2f));
                    sliceTelegraph.SetActive(true);
                    
                }
            }
            
        }

        private Vector3[] GetLinePointsShowingPlane(Vector3 centrePosition, Vector3 normal, float planeSize)
        {
            Vector3 v3;

            if (normal.normalized != Vector3.forward)
                v3 = planeSize * Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
            else
                v3 = planeSize * Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude;

            var corner0 = centrePosition + v3;
            var corner2 = centrePosition - v3;
            var q = Quaternion.AngleAxis(90.0f, normal);
            v3 = q * v3;
            var corner1 = centrePosition + v3;
            var corner3 = centrePosition - v3;

            return new Vector3[] { corner0, corner1, corner2, corner3, corner0, corner2};
        }

        private void GenerateNewTargetOffset()
        {
            var rc = Random.insideUnitCircle.normalized;
            this._destinationOffset = offsetRadius*new Vector3(rc.x, 0, rc.y);
        }

        private void ChiselStatue()
        {
            Physics.Raycast(_chiselRay, out var hit, 15f);
            var puncher = hit.transform.GetComponent<MeshPuncher>();
            puncher?.PunchMesh(hit.point, hit.normal, puncher.radius);
        }

        private void SliceStatue()
        {
            statueTarget.GetComponent<CuttableMesh>().CutMesh(_slicePlane, _slicePlaneCentre);
        }

        private bool Arrived()
        {
            return Vector3.Distance(transform.position, new Vector3(statueTarget.position.x, 0, statueTarget.position.z) + _destinationOffset) < 1f;
        }

        private void Chisel()
        {
            animator.SetTrigger(ChiselAnimation);
            
        }
    }
}