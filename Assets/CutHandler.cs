using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutHandler : MonoBehaviour
{
    private Mesh _mesh;
    public float DamageLevel = 0;
    void Start()
    {
        this._mesh = GetComponent<MeshFilter>().mesh;
    }

    public void TestAgainstPlane(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var q1 = transform.TransformPoint(p1);
        var q2 = transform.TransformPoint(p2);
        var q3 = transform.TransformPoint(p3);

        TestAgainstPlane(q1, new Plane(q3, q2, q1));
    }

    public void TestAgainstPlane(Vector3 position, Plane plane)
    {
        DamageLevel += GetCutFraction(plane);
    }

    private float GetCutFraction(Plane plane)
    {
        int total = 0;
        foreach(var vert in _mesh.vertices)
        {
            if (plane.GetSide(vert))
            {
                total++;
            }
        }
        print(string.Format("{0} of {1} cut", total, _mesh.vertices.Length));

        print(string.Format("{0} of {1} cut", total, _mesh.vertices.Length));

        float frac = total * 1f / _mesh.vertices.Length;
        return frac;
    }

    public bool CheckCut(Vector3 position, Plane plane, out Vector3 hit)
    {
        for (int n = 0; n < _mesh.triangles.Length; n += 3)
        {
            var p1 = _mesh.vertices[_mesh.triangles[n]];
            var p2 = _mesh.vertices[_mesh.triangles[n]];
            var p3 = _mesh.vertices[_mesh.triangles[n]];

            bool noSplit = plane.GetSide(p1) == plane.GetSide(p2) && plane.GetSide(p2) == plane.GetSide(p3);

            //If they aren't all on the same side, 
            if (!noSplit)
            {
                hit = (p1 + p2 + p3 / 3);
                return true;
            }
        }
        hit = Vector3.zero;
        return false;
    }
}
