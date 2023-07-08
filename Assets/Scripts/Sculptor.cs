using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Sculptor : MonoBehaviour
    {
        public LineRenderer lineRenderer;


        public float beamWidth;
        public GameObject target;

        public GameObject parent;

        private float _timer = 5f;
        public float maxTime = 5f;

        public float angleRange = 20f;

        public SculptorDestination destination;
        private void Update()
        {
            DrawStraightLine(transform.position, target.transform.position);
            _timer += Time.deltaTime;
            if (_timer > maxTime)
            {
                Relocate();
                _timer = 0;
            }
            
            
            
        }

        // [Button]
        public void Relocate()
        {
            // parent.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(-angleRange,angleRange), Random.Range(0,360), 0));
        }
        
        public void DrawStraightLine(Vector3 sp, Vector3 ep)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, sp);
            lineRenderer.SetPosition(1, ep);
            lineRenderer.startWidth = beamWidth;
            lineRenderer.endWidth = beamWidth;
            
        }
    }
}