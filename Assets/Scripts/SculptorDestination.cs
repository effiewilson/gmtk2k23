using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class SculptorDestination : MonoBehaviour
    {
        public GameObject pivotObject;


        private void OnEnable()
        {
            PickRandomDestination();
        }

        public void PickRandomDestination()
        {
            Vector3 movement = new Vector3(0, 1, 0);

            
            
            transform.RotateAround(pivotObject.transform.position, movement, Random.Range(0,360));
            transform.LookAt(pivotObject.transform);
        }
    }
}