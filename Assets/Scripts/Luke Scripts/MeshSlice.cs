using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MeshSlice
{
    private List<Triangle> _triangles = new();
    private List<Vector3> _vertices = new();
    private List<Vector3> _normals = new();
    private List<Vector2> _uvs = new();

    private List<Vector3[]> _edges = new();
    private List<Vector3[]> _edgeNormals = new();
    private List<Vector2[]> _edgeUVs = new();

    private Bounds _bounds = new();

    public Plane CutPlane { get; private set; }

    public MeshSlice(Plane cutPlane)
    {
        this.CutPlane = cutPlane;
    }

    public int VertexCount => _vertices.Count;
    public Vector3 AverageVertexPosition
    {
        get
        {
            Vector3 ans = new Vector3();
            foreach (var vert in _vertices)
            {
                ans += vert;
            }
            ans /= _vertices.Count;
            return ans;
        }
    }
    public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 n1, Vector3 n2, Vector3 n3, Vector2 u1, Vector2 u2, Vector2 u3)
    {
        int offset = _vertices.Count;
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);

        _bounds.min = Vector3.Min(_bounds.min, v1);
        _bounds.min = Vector3.Min(_bounds.min, v2);
        _bounds.min = Vector3.Min(_bounds.min, v3);

        _triangles.Add(new Triangle(offset, offset + 1, offset + 2));

        _normals.Add(n1);
        _normals.Add(n2);
        _normals.Add(n3);
        _uvs.Add(u1);
        _uvs.Add(u2);
        _uvs.Add(u3);
    }

    public void AddEdge(Vector3 v1, Vector3 v2, Vector3 n1, Vector3 n2, Vector2 u1, Vector2 u2)
    {
        _edges.Add(new Vector3[]{ v1, v2 });
        _edgeNormals.Add(new Vector3[] { n1, n2 });
        _edgeUVs.Add(new Vector2[] { u1, u2 });
    }

    private void AddEdgeTriangles()
    {
        Vector3 midpoint = new Vector3();
        Vector3 midNorm = new Vector3();

        for(int i=0; i<_edges.Count; i++)
        {
            midpoint.x += _edges[i][0].x;
            midpoint.y += _edges[i][0].y;
            midpoint.z += _edges[i][0].z;

            midNorm.x += _edgeNormals[i][0].x;
            midNorm.y += _edgeNormals[i][0].y;
            midNorm.z += _edgeNormals[i][0].z;
        }
        midpoint/=_edges.Count;
        midNorm.Normalize();

        for(int i=0; i<_edges.Count; i++)
        {
            AddTriangle(_edges[i][0], _edges[i][1], midpoint, _edgeNormals[i][0], _edgeNormals[i][1], midNorm, _edgeUVs[i][0], _edgeUVs[i][1], Vector2.zero);
        }
        _edges.Clear();
    }

    public Mesh GetMesh()
    {
        AddEdgeTriangles();

        var tris  = new int[_triangles.Count * 3];
        var verts = new int[_vertices.Count];

        for (int i = 0; i < _triangles.Count; i++)
        {
            int idx = i * 3;

            tris[idx + 0] = _triangles[i].a;
            tris[idx + 1] = _triangles[i].b;
            tris[idx + 2] = _triangles[i].c;
        }

        Mesh newMesh = new Mesh();
        newMesh.vertices = _vertices.ToArray();
        newMesh.triangles = tris;
        newMesh.RecalculateBounds();
        newMesh.normals = _normals.ToArray();
        newMesh.uv = _uvs.ToArray();
        return newMesh;
    }
}
