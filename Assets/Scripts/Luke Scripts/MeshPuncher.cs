using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshPuncher : MonoBehaviour
{

    public Transform punchFlare;
    public float radius = 0.2f;
    private Mesh _mesh;
    // Start is called before the first frame update
    void Start()
    {
        var _mesh = new Mesh();
        var refMesh = this._mesh = GetComponent<MeshFilter>().mesh;

        _mesh.vertices = refMesh.vertices; 
        _mesh.normals = refMesh.normals;
        _mesh.uv = refMesh.uv;
        _mesh.triangles = refMesh.triangles;
        _mesh.bounds = refMesh.bounds;
    }


    public void RandomPunch()
    {
        var RandomRayOrigin = transform.position + new Vector3(0,0,10f) + Random.onUnitSphere;
        Vector3 randomtarget = transform.position+Random.onUnitSphere * 1f;
        Physics.Raycast(RandomRayOrigin, randomtarget - RandomRayOrigin, out var hitInfo, 20f);

        if (hitInfo.collider == this.GetComponent<MeshCollider>())
        {
            var hitPos = hitInfo.point;
            var hitNormal = hitInfo.normal;
            hitPos+=0.2f*radius*(RandomRayOrigin - hitPos).normalized;



            StartCoroutine(DoDelayedPunch(RandomRayOrigin, hitPos, hitNormal, this.radius));
        }

        
    }

    private IEnumerator DoDelayedPunch(Vector3 origin, Vector3 position, Vector3 hitNormal, float radius)
    {

        DrawRay(origin, position, Color.green);
        yield return new WaitForSeconds(2f);
        DrawRay(origin, position, Color.yellow);
        yield return new WaitForSeconds(2f);
        DrawRay(origin, position, Color.red);
        yield return new WaitForSeconds(2f);

        PunchMesh(position, hitNormal, radius);
    }

    private void DrawRay(Vector3 origin, Vector3 endpoint, Color color)
    {
        Debug.DrawLine(origin, endpoint, color, 2f);
    }


    public void PunchMesh(Vector3 impact, Vector3 hitNormal, float radius)
    {
        punchFlare.GetComponent<ParticleSystem>().Stop();
        if (punchFlare != null)
        {
            punchFlare.position = impact;
            punchFlare.LookAt(hitNormal);
            punchFlare.GetComponent<ParticleSystem>().Play();
        }

        var targetMesh = GetComponent<MeshFilter>().sharedMesh;

        var verts = targetMesh.vertices;

        for (int n=0; n < verts.Length; n++)
        {
            var vert = transform.TransformPoint(verts[n])-0.5f*radius*transform.TransformDirection(hitNormal);
            if ((vert - impact).sqrMagnitude < radius * radius)
            {
                vert = impact + radius * (vert-impact).normalized;
                verts[n] = transform.InverseTransformPoint(vert);
            }
        }

        targetMesh.vertices = verts;
        GetComponent<MeshFilter>().mesh = targetMesh;
        _mesh = targetMesh;

        GetComponent<MeshCollider>().sharedMesh = _mesh;
        GetComponent<MeshVolumeTracker>().CalculateVolume(_mesh);
    }
}
