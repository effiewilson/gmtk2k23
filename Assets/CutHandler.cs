using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutHandler : MonoBehaviour
{
    public float DamageLevel = 0;

    void Update()
    {
        if (transform.parent == null && GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }

    public void AddDamage(float dmg)
    {
        this.DamageLevel += dmg;
    }

    public void TestAgainstPlane3(Vector3 point1, Vector3 point2, Vector3 point3)
    {
        //Transform cut plane into object coordinates!
        Vector3 xp1 = transform.InverseTransformPoint(point1);
        Vector3 xp2 = transform.InverseTransformPoint(point2);
        Vector3 xp3 = transform.InverseTransformPoint(point3);

        Mesh mesh = GetComponent<MeshCollider>().sharedMesh;

        var cut = new Plane(xp1, xp2, xp3);

        int total = 0;

        for (int n = 0; n < mesh.vertices.Length; n+=10)
        {
            if (cut.GetSide(mesh.vertices[n]))
            {
                total++;
            }
        }
        print(string.Format("{0} of {1} vertices on the same side", total, mesh.vertices.Length));

        float sideFrac = total*10f / mesh.vertices.Length;

        DamageLevel += Mathf.Min(sideFrac, 1-sideFrac);
    }
    /*
    public void TestAgainstPlane(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var q1 = transform.TransformPoint(p1);
        var q2 = transform.TransformPoint(p2);
        var q3 = transform.TransformPoint(p3);

        TestAgainstPlane(p1, new Plane(p1, p2, p3));
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
            var vertex = transform.TransformPoint(vert);
            if (plane.GetSide(vertex))
            {
                total++;
            }
        }
        print(string.Format("{0} of {1} cut", total, _mesh.vertices.Length));
        float frac = total * 1f / _mesh.vertices.Length;
        return frac;
    }

    public bool CheckCut(Vector3 position, Plane plane, out Vector3 hit)
    {
        for (int n = 0; n < _mesh.triangles.Length; n += 3)
        {
            var p1 = _mesh.vertices[_mesh.triangles[n]];
            var p2 = _mesh.vertices[_mesh.triangles[n+1]];
            var p3 = _mesh.vertices[_mesh.triangles[n+2]];

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
    }*/
}
