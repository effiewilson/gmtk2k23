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
            transform.RotateAround(pivotObject.transform.position, Vector3.up, Random.Range(0,360));
            transform.LookAt(pivotObject.transform);
        }
    }
}