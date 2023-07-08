using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnyCam : MonoBehaviour
{
    public float rotationSpeed = 0.1f;


    void Update()
    {
        transform.eulerAngles += new Vector3(0, Time.deltaTime * rotationSpeed, 0);
    }
}
