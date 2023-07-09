using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CuttableMesh : MonoBehaviour
{
    private Mesh _mesh;
    public float SmashFraction = 0.75f;

    private bool _finished = false;

    private MeshVolumeTracker volume;

    public CutHandler alsoCheck;

    // Start is called before the first frame update
    void Start()
    {
        this._mesh = GetComponent<MeshFilter>().mesh;
        this.volume = GetComponent<MeshVolumeTracker>();
    }

    private void Update()
    {
        if (!_finished && volume.FractionalVolume < SmashFraction)
        {
            StartCoroutine(SmashRemains());
        }
    }

    private IEnumerator SmashRemains()
    {
        _finished = true;
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomOffset = transform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
            Vector3 randomOrientation = new Vector3(Random.Range(-20, 20f), Random.Range(-60, 60f), Random.Range(-120, 120));
            CutMesh(new Plane(randomOrientation, randomOffset), randomOffset);
            yield return new WaitForFixedUpdate();
        }
    }

    public void RandomCut()
    {
        Vector3 randomOffset = transform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
        Vector3 randomOrientation = new Vector3(Random.Range(89f, 91f), Random.Range(-80f, 100f), Random.Range(-1, 1));

        StartCoroutine(DoDelayedCut(randomOffset, new Plane(randomOrientation, randomOffset)));
    }

    private IEnumerator DoDelayedCut(Vector3 position, Plane plane)
    {
        DrawPlane(position, plane.normal, Color.green);
        yield return new WaitForSeconds(2f);
        DrawPlane(position, plane.normal, Color.yellow);
        yield return new WaitForSeconds(2f);
        DrawPlane(position, plane.normal, Color.red);
        yield return new WaitForSeconds(2f);

        CutMesh(plane, position);
    }

    private void DrawPlane(Vector3 position, Vector3 normal, Color color)
    {
        Vector3 v3;

        if (normal.normalized != Vector3.forward)
            v3 = 8f*Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = 8f*Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;

        Debug.DrawLine(corner0, corner2, color, 2f);
        Debug.DrawLine(corner1, corner3, color, 2f);
        Debug.DrawLine(corner0, corner1, color, 2f);
        Debug.DrawLine(corner1, corner2, color, 2f);
        Debug.DrawLine(corner2, corner3, color, 2f);
        Debug.DrawLine(corner3, corner0, color, 2f);
    }

    public void CutMesh(Plane cut, Vector3 planeCentre)
    {
        var rpp0 = planeCentre;
        var rpp1 = RandomPointOnPlane(planeCentre, cut.normal, 5f);
        var rpp2 = RandomPointOnPlane(planeCentre, cut.normal, 5f);

        alsoCheck?.TestAgainstPlane3(rpp0, rpp1, rpp2);

        //Transform cut plane into object coordinates!
        Vector3 xp1 = transform.InverseTransformPoint(rpp0);
        Vector3 xp2 = transform.InverseTransformPoint(rpp1);
        Vector3 xp3 = transform.InverseTransformPoint(rpp2);

        cut = new Plane(xp1, xp2, xp3);

        MeshSlice leftSlice = new(cut);
        MeshSlice rightSlice = new(cut);

        var tris = GetComponent<MeshFilter>().mesh.triangles;

        for (int n=0; n<tris.Length; n+=3)
        {
            var sA = cut.GetSide(_mesh.vertices[tris[n]]);
            var sB = cut.GetSide(_mesh.vertices[tris[n+1]]);
            var sC = cut.GetSide(_mesh.vertices[tris[n+2]]);

            int count = 0;
            count += sA ? 1 : 0;
            count += sB ? 1 : 0;
            count += sC ? 1 : 0;

            if (count == 0)
            {
                rightSlice.AddTriangle(_mesh.vertices[tris[n]], _mesh.vertices[tris[n + 1]], _mesh.vertices[tris[n + 2]],
                                      _mesh.normals[tris[n]] , _mesh.normals[tris[n + 1]],  _mesh.normals[tris[n + 2]],
                                      _mesh.uv[tris[n]],       _mesh.uv[tris[n + 1]],       _mesh.uv[tris[n + 2]]);
                continue;
            }
            else if (count == 3)
            {
                leftSlice.AddTriangle(_mesh.vertices[tris[n]], _mesh.vertices[tris[n + 1]], _mesh.vertices[tris[n + 2]],
                                      _mesh.normals[tris[n]], _mesh.normals[tris[n + 1]], _mesh.normals[tris[n + 2]],
                                      _mesh.uv[tris[n]], _mesh.uv[tris[n + 1]], _mesh.uv[tris[n + 2]]);
                continue;
            }

            //0->a alone, 1-> b alone, 2-> c alone
            var raycastIndex = sB == sC ? 0 : sA == sC ? 1 : 2;

            //Raycast from the one cut off
            Ray ray1 = new();
            Ray ray2 = new();
            // Toward the other two...
            Vector3 oldVert1 = _mesh.vertices[tris[n + ((raycastIndex + 1) % 3)]];
            Vector3 oldVert2 = _mesh.vertices[tris[n + ((raycastIndex + 2) % 3)]];

            Vector3 oldNorm0 = _mesh.normals[tris[n + raycastIndex]];
            Vector3 oldNorm1 = _mesh.normals[tris[n + ((raycastIndex + 1) % 3)]];
            Vector3 oldNorm2 = _mesh.normals[tris[n + ((raycastIndex + 2) % 3)]];
            Vector2 oldUV0 = _mesh.uv[tris[n + raycastIndex]];
            Vector2 oldUV1 = _mesh.uv[tris[n + ((raycastIndex + 1) % 3)]];
            Vector2 oldUV2 = _mesh.uv[tris[n + ((raycastIndex + 2) % 3)]];
            
            ray1.origin = _mesh.vertices[tris[n + raycastIndex]];
            ray2.origin = _mesh.vertices[tris[n + raycastIndex]]; 

            ray1.direction = oldVert1 - ray1.origin;
            cut.Raycast(ray1, out var hit1);
            float t1 = hit1 / ray1.direction.magnitude;

            ray2.direction = oldVert2 - ray2.origin;
            cut.Raycast(ray2, out var hit2);
            float t2 = hit2 / ray2.direction.magnitude;

            Vector3 newVert1 = ray1.origin + ray1.direction.normalized * hit1;
            Vector3 newVert2 = ray2.origin + ray2.direction.normalized * hit2;
            
            Vector3 newNorm1 = Vector3.Lerp(oldNorm0, oldNorm1, t1);
            Vector3 newNorm2 = Vector3.Lerp(oldNorm0, oldNorm2, t2);

            Vector3 newUV1 = Vector2.Lerp(oldUV0, oldUV2, t1);
            Vector3 newUV2 = Vector2.Lerp(oldUV0, oldUV2, t2);

            //If only one was on the left...
            if (count == 1)
            {
                leftSlice.AddTriangle(ray1.origin, newVert1, newVert2,
                                      oldNorm0,    newNorm1, newNorm2,
                                      oldUV0,      newUV1,   newUV2);
                
                rightSlice.AddTriangle(newVert1, oldVert1, oldVert2,
                                        newNorm1, oldNorm1, oldNorm2,
                                        newUV1,   oldUV1,   oldUV2);

                 rightSlice.AddTriangle(newVert1, oldVert2, newVert2,
                                        newNorm1, oldNorm2, newNorm2,
                                        newUV1,   oldUV2,   newUV2);

                 rightSlice.AddEdge(newVert1, newVert2, newNorm1, newNorm2, newUV1, newUV2);
                 leftSlice.AddEdge(newVert2, newVert1, newNorm2, newNorm1, newUV2, newUV1);
            }
            else if(count == 2)
            {
                rightSlice.AddTriangle(ray1.origin, newVert1, newVert2,
                                    oldNorm0, newNorm1, newNorm2,
                                    oldUV0, newUV1, newUV2);

                leftSlice.AddTriangle(newVert1, oldVert1, oldVert2,
                                    newNorm1, oldNorm1, oldNorm2,
                                    newUV1, oldUV1, oldUV2);

                leftSlice.AddTriangle(newVert1, oldVert2, newVert2,
                                    newNorm1, oldNorm2, newNorm2,
                                    newUV1, oldUV2, newUV2);

                leftSlice.AddEdge(newVert1, newVert2, newNorm1, newNorm2, newUV1, newUV2);
                rightSlice.AddEdge(newVert2, newVert1, newNorm2, newNorm1, newUV2, newUV1);
            }

            leftSlice.AddEdge(newVert1, newVert2, newNorm1, newNorm2, newUV1, newUV2);

        }

        CreateNewObjects(leftSlice, rightSlice);
    }

    private static Vector3 RandomPointOnPlane(Vector3 position, Vector3 normal, float radius)
    {
        Vector3 randomPoint;

        do
        {
            randomPoint = Vector3.Cross(Random.insideUnitSphere, normal);
        } while (randomPoint == Vector3.zero);

        randomPoint.Normalize();
        randomPoint *= radius;
        randomPoint += position;

        return randomPoint;
    }

    private void CreateNewObjects(MeshSlice A, MeshSlice B)
    {
        Vector3 posA = A.AverageVertexPosition;
        Vector3 posB = B.AverageVertexPosition;

        //print(string.Format("Average vertex positions A=({0:0.0}, {1:0.0}, {2:0.0}), B={3:0.0}, {4:0.0}, {5:0.0}", posA.x, posA.y, posA.z,posB.x, posB.y, posB.z));
        print(string.Format("Average vertex dist origin A={0:0.00},    B={1:0.00}", posA.magnitude, posB.magnitude));

        Vector3 dirPush;
        Mesh keep, chuck;

        if (A.VertexCount > B.VertexCount)
        {
            keep  = A.GetMesh();
            chuck = B.GetMesh();
            dirPush = posB-posA;
            print(string.Format("Kept A"));
        }
        else
        {
            keep  = B.GetMesh();
            chuck = A.GetMesh();
            dirPush = posA - posB;
            print(string.Format("Kept A"));
        }
        dirPush.Normalize();

        if (!_finished)
        {
            //This mesh
            this._mesh = keep;
            GetComponent<MeshFilter>().mesh = this._mesh;
            GetComponent<MeshCollider>().sharedMesh = this._mesh;
            volume.CalculateVolume(this._mesh);
        }
        else
        {
            transform.DetachChildren();
            MakeDiscardMesh(keep, -dirPush);
            GetComponent<MeshFilter>().mesh = null;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().enabled = false;
            StartCoroutine(DestroyAfterSeconds(gameObject, 5f));
        }
        MakeDiscardMesh(chuck, dirPush);
    }

    private void MakeDiscardMesh(Mesh mesh, Vector3 pushAway)
    {
        if (_finished) pushAway *= 2;

        //The discarded mesh
        GameObject chuckObj = new GameObject("Superfluous Material");

        chuckObj.transform.position = transform.position;
        chuckObj.transform.rotation = transform.rotation;
        chuckObj.transform.localScale = transform.localScale;
        Vector3 pushOffset = chuckObj.transform.InverseTransformDirection(0.1f * pushAway);

        chuckObj.transform.position -= pushOffset;

        var filter = chuckObj.AddComponent<MeshFilter>();
        var renderer = chuckObj.AddComponent<MeshRenderer>();
        renderer.material = this.GetComponent<MeshRenderer>().material;
        filter.mesh = mesh;
        var collider = chuckObj.AddComponent<MeshCollider>();
        collider.convex = true;
        var rb = chuckObj.AddComponent<Rigidbody>();

        if (_finished)
        {
            rb.AddForce(10f * pushAway);
        }

        StartCoroutine(DestroyAfterSeconds(chuckObj, 3.5f));
    }

    IEnumerator DestroyAfterSeconds(GameObject go, float fSecs)
    {
        yield return new WaitForSeconds(fSecs);

        GameObject.Destroy(go);
    }
}
