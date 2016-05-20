using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MeshGenerator : MonoBehaviour
{

    public BezierCurve path;
    public BezierCurve outline;
	public int resolution = 100;

    private static int numOutlinePoints = 30;

    // Use this for initialization
    void Start()
    {
        GetComponent<MeshFilter>().mesh = CreateMesh(resolution);
		DestroyImmediate (gameObject.GetComponent<MeshCollider> ());
		gameObject.AddComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    Mesh CreateMesh(int resolution)
    {
        var m = new Mesh();
        m.name = "ScriptedMesh";

        var vertices = new List<Vector3>();
        var texCoords = new List<Vector2>();
        var colors = new List<Color>();
        
        var dt = 1f / resolution;

       for (var t = 0.0f; t < 1.0f; t += dt)
        {
            var p = path.GetPointAt(t);
            var p1 = path.GetPointAt(t + dt);

            var originalAlign = new Vector3(0f, -1f, 0f).normalized;
            var derivative = (p1 - p).normalized;

            var q = Quaternion.LookRotation(derivative, Vector3.up);
            outline.transform.rotation = q;

            for (var tt = 0.0f; tt < 1f; tt += (1f/numOutlinePoints)) {
                var o = outline.GetPointAt(tt);
               
                vertices.Add(p + o);
                texCoords.Add(new Vector2(t, tt));
                colors.Add(Color.blue);
            }

            // Add more verts to close seam
            vertices.Add(p + outline.GetPointAt(0f));
            texCoords.Add(new Vector2(t, 1f));
            colors.Add(Color.blue);
        }

        // Generate indices
        List<int> indices = new List<int>();
        for (var i = 0; i < resolution; i++) {
            var fakePoints = numOutlinePoints + 1;
            for (var j = 0; j < numOutlinePoints; j++) {
                int idx0 = i * fakePoints + (j);
                int idx1 = i * fakePoints + (j + 1);
                int idx2 = i * fakePoints + (j + 1 + fakePoints);
                int idx3 = i * fakePoints + (j + fakePoints);

                // Forward Facing triangles
                indices.Add(idx0);
                indices.Add(idx3);
                indices.Add(idx1);
                //Debug.Log(String.Format("Triangle {0},{1},{2}", idx0, idx3, idx1));

                indices.Add(idx1);
                indices.Add(idx3);
                indices.Add(idx2);
                //Debug.Log(String.Format("Triangle {0},{1},{2}", idx1, idx3, idx2));
            }


        }

        m.vertices = vertices.ToArray();
        m.triangles = indices.ToArray();
        m.colors = colors.ToArray();
        m.uv = texCoords.ToArray();

        m.RecalculateNormals();

        return m;
    }
}

