using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class SculptorTelegraph : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        public float beamWidth;
        public GameObject target;

        private void Update()
        {
            DrawStraightLine(transform.position, target.transform.position);
        }

        private void OnDisable()
        {
            lineRenderer.positionCount = 0;
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