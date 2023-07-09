using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragForce : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody targetBody;
    public Camera camera;
    private bool _dragging = false;
    private Vector3 startPos;
    private Vector3 centreOffset;

    public float forceMult = 0.1f;

    void Start()
    {
        
    }

    private void Update()
    {
        if (_dragging)
        {
            Vector3 currentPosition = Input.mousePosition;
            var force = camera.transform.TransformPoint(currentPosition) - camera.transform.TransformPoint(startPos);

            float mag = force.magnitude;
            mag /= Camera.current.pixelWidth;
            mag *= 0.1f;
            float rootmag = Mathf.Sqrt(mag);
            //print(string.Format("Magnitude:: {0:0.0},      sqrt:: {1:0.0}", mag, rootmag));
            force *= Mathf.Clamp(rootmag/(mag+1), 0f,100f);

            force*=forceMult;
     

            var TransformedCentreOffset = camera.transform.TransformDirection(centreOffset);
            //print(string.Format("TransformedCentreOffset :({0:0.0},{1:0.0},{2:0.0})", TransformedCentreOffset.x, TransformedCentreOffset.y, TransformedCentreOffset.z));
            var offset = targetBody.transform.position + TransformedCentreOffset;

            //float rootMag = Mathf.Sqrt(offset.magnitude);

            //offset.Normalize();
            //offset*=rootMag;

            targetBody.AddForceAtPosition(force, offset, ForceMode.Force);

            //targetBody.AddForce(offset, ForceMode.Force);
        }
    }
  
    public void OnPointerExit()
    {
        _dragging = false;
        //print("RELEASE!");
    }
    public void OnLeftMouseUp()
    {
        _dragging = false;
        //print("RELEASE!");
    }
    public void OnLeftMouseDown()
    {
        if (_dragging) return;

        var rt = GetComponent<RectTransform>();
        _dragging = true;
        startPos = Input.mousePosition;
        centreOffset = startPos - rt.position;
        centreOffset.x /= Camera.current.pixelWidth;
        centreOffset.y /= Camera.current.pixelWidth;
        centreOffset.x *= 60f;
        centreOffset.y *= 60f;

        //print(string.Format("Offset from centre: ({0:0.0}, {1:0.0}, {2:0.0}).", centreOffset.x, centreOffset.y, centreOffset.z));
    }
}
