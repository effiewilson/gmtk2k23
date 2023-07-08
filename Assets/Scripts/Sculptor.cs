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
            // Vector3 oldRota?
            
            // Quaternion.
            // parent.transform.rotation = Quaternion.Euler(new Vector3(parent.transform.rotation.x, Random.rotation.y, parent.transform.rotation.z));
            parent.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(-angleRange,angleRange), Random.Range(0,360), 0));
            // parent.transform.rotation = new Vector3(parent.transform.rotation.x, parent.transform.rotation.y, parent.transform.rotation.z));
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