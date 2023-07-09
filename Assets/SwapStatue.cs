using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapStatue : MonoBehaviour
{
    public Mesh Alt1;
    public Mesh Alt2;

    public MeshFilter target;

    public Vector3 Alt1Position;
    public Vector3 Alt2Position;

    public Vector3 Alt1Rotation;
    public Vector3 Alt2Rotation;

    // Start is called before the first frame update
    void OnEnable()
    {
        int selection = Random.RandomRange(0, 3);

        print("Selection is " + selection.ToString());

        if (selection == 0) return;

        var mc = target.GetComponent<MeshCollider>();

        if (selection == 1)
        {
            target.sharedMesh = Alt1;
            mc.sharedMesh = Alt1;
            mc.inflateMesh = true;

            target.transform.localPosition = Alt1Position;
            target.transform.localEulerAngles = Alt1Rotation;
        }
        else if(selection == 2)
        {
            target.sharedMesh = Alt2;
            mc.sharedMesh = Alt2;
            mc.inflateMesh = true;

            target.transform.localPosition    = Alt2Position;
            target.transform.localEulerAngles = Alt2Rotation;
        }

    }
}
