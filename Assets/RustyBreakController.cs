using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RustyBreakController : MonoBehaviour
{
    [Range(0, 1)]
    public float percent;
    public GameObject rustyPiece;

    public LayerMask mask;

    [SerializeField] float minScale, maxScale;
    [SerializeField] Transform meshTransform;

    Camera cam;

    int rustLayer, objectLayer;

    void Start()
    {
        rustLayer = LayerMask.NameToLayer("Rust");
        objectLayer = LayerMask.NameToLayer("Object");

        cam = Camera.main;
        Vector3 meshCenter = meshTransform.position;
        Mesh mesh = meshTransform.GetComponent<MeshFilter>().mesh;
        List<Vector3> vs = new List<Vector3>(mesh.vertices);
        List<Vector3> ns = new List<Vector3>(mesh.normals);


        int[] idx = new int[mesh.vertexCount];
        for (int i = 0; i < idx.Length; ++i) idx[i] = i;
        System.Random rnd = new System.Random();
        var idxs = idx.OrderBy(x => rnd.Next()).Take((int)(percent * (float)mesh.vertexCount)).ToList();

        for (int i = 0; i < idxs.Count; i++)
        {
            Transform t = Instantiate(rustyPiece, meshCenter + vs[idxs[i]], Quaternion.identity).transform;
            t.up = ns[idxs[i]];
            t.Rotate(Vector3.up, Random.value * 360);
            t.localScale = new Vector3(Random.Range(minScale, maxScale), t.localScale.y, Random.Range(minScale, maxScale));
            t.parent = meshTransform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DropRust();
        }
        Vector3 fw = Camera.main.transform.forward;
        print(Vector3.Angle(Vector3.back, new Vector3(fw.x, 0, fw.z)));

    }

    public void DropRust()
    {
        Vector3 camPosition = cam.transform.position;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 5, mask);

        float objectDist = 999, rustDist = 999;
        int rustIndex = -1;
        for (int i = 0; i < hits.Length; ++i)
        {
            float dist = Vector3.Distance(camPosition, hits[i].point);
            if (hits[i].transform.gameObject.layer == rustLayer)
            {
                if (dist < rustDist)
                {
                    rustDist = dist;
                    rustIndex = i;
                }
            }
            else if (dist < objectDist)objectDist = dist; 
        }

        if (rustIndex != -1 && rustDist < objectDist)
        {
            GameObject rust = hits[rustIndex].transform.gameObject;
            RustStone rs = rust.GetComponent<RustStone>();
            Chisel.instance.SetRust(rs);
        }
    }
}
