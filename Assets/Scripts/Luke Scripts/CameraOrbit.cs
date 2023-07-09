using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    public float speed;
    public float minDist = 2f;
    public float maxDist = 4f;

    public Transform centre;

    public InputAction move;

    private float _targetDistance;

    // Start is called before the first frame update
    void Start()
    {
        _targetDistance = (transform.position - centre.position).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = move.ReadValue<Vector2>() * speed;

        _targetDistance -= movement.y;

        _targetDistance = Mathf.Clamp(_targetDistance, minDist, maxDist);

        float fSpeed  = speed * Time.deltaTime;
        
        transform.LookAt(centre, Vector3.up);
        transform.Translate(speed * Time.deltaTime * movement.x * transform.right);

        transform.position = (transform.position - centre.position).normalized * _targetDistance;
    }
}
