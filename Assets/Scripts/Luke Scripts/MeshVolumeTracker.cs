using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshVolumeTracker : MonoBehaviour
{
    private float initialVolume;
    public float Volume { get; private set; }
    public float FractionalVolume { 
        get
        {
            return Mathf.Min(1, Volume/initialVolume);
        }
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        initialVolume = CalculateVolume(GetComponent<MeshFilter>().mesh);
    }

    public float CalculateVolume(Mesh mesh)
    {
        float vol = 0;

        for (int n = 0; n < mesh.triangles.Length; n += 3)
        {
            Vector3 p1 = mesh.vertices[mesh.triangles[n]];
            Vector3 p2 = mesh.vertices[mesh.triangles[n + 1]];
            Vector3 p3 = mesh.vertices[mesh.triangles[n + 2]];

            vol += SignedVolumeOfTriangle(p1, p2, p3);
        }
        print(string.Format("Relative volume now {0:0.00}", vol/initialVolume));
        Volume = vol;
        return vol;
    }

    private static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var v321 = p3.x * p2.y * p1.z;
        var v231 = p2.x * p3.y * p1.z;
        var v312 = p3.x * p1.y * p2.z;
        var v132 = p1.x * p3.y * p2.z;
        var v213 = p2.x * p1.y * p3.z;
        var v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }
}
