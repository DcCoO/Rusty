﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Range(0, 1)]
    public float percent;
    public GameObject rustyPiece;

    public LayerMask mask;

    public float minScale, maxScale;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
        Vector3 meshCenter = transform.position;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
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
            t.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit; 
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10, mask))
            {
                Destroy(hit.transform.gameObject);
            }
        }
    }

}
