using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesctructableStatue : MonoBehaviour
{
    public Transform solidStatue, fracturedStatue;

    private void Start()
    {
        solidStatue.gameObject.SetActive(true);
        fracturedStatue.gameObject.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.D)) Fracture();
    }

    private void Fracture()
    {
        solidStatue.gameObject.SetActive(false);
        fracturedStatue.gameObject.SetActive(true);
    }
}
