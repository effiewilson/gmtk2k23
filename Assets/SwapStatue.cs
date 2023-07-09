using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapStatue : MonoBehaviour
{
    public Mesh Alt1;
    public Mesh Alt2;

    public MeshFilter target;

    // Start is called before the first frame update
    void OnEnable()
    {
        int selection = Random.RandomRange(0, 2);

        if (selection == 0) return;

        var mc = target.GetComponent<MeshCollider>();

        if (selection == 1)
        {
            target.sharedMesh = Alt1;
            mc.sharedMesh = Alt1;
            mc.inflateMesh = true;
        }
        else if(selection == 2)
        {
            target.sharedMesh = Alt2;
            mc.sharedMesh = Alt2;
            mc.inflateMesh = true;
        }

    }
}
