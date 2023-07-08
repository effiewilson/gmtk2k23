using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistForce : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody targetBody;

    public float wobbleFreq = 0.5f;
    public float wobbleAmp = 0.2f;
    private bool _dragging = false;
    private Vector3 startPos;

    public float forceMult = 0.1f;

    private float _timeTwisting;

    void Start()
    {

    }

    private void Update()
    {
        if (_dragging)
        {
            Vector3 currentPosition = Input.mousePosition;
            var force = currentPosition - startPos;
            _timeTwisting += Time.deltaTime;

            Vector3 torque = new Vector3(0, force.x, 0) * Time.deltaTime;

            targetBody.AddTorque(forceMult*torque, ForceMode.Force);

            var wobbleForce = wobbleAmp*Mathf.Sin(_timeTwisting * wobbleFreq);

            targetBody.AddForce(wobbleForce * transform.right);
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
        _timeTwisting = 0;
        //print(string.Format("Offset from centre: ({0:0.0}, {1:0.0}, {2:0.0}).", centreOffset.x, centreOffset.y, centreOffset.z));
    }
}
