using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MeshGenerator : MonoBehaviour
{

    public BezierCurve path;
    public BezierCurve outline;

    public int resolution;
    public int outlineResolution;

    private int realResolution;
    private int realOutlineResolution;

    // Use this for initialization
    void Start()
    {
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
        realResolution = resolution + 1;
        realOutlineResolution = outlineResolution + 1;

        GetComponent<MeshFilter>().mesh = CreateMesh();
        DestroyImmediate(gameObject.GetComponent<MeshCollider>());
        gameObject.AddComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    Mesh CreateMesh()
    {
        var baseRot = outline.transform.rotation;
        var m = new Mesh();
        m.name = "ScriptedMesh";

        var vertices = new List<Vector3>();
        var texCoords = new List<Vector2>();
        var colors = new List<Color>();

        var t = 0.0f;
        var dt = 1f/resolution;

        for (int segment = 0; segment < resolution; segment++)
        {
            var p = path.GetPointAt(t);
            var p1 = path.GetPointAt(t + dt/2f);

            var derivative = (p1 - p).normalized;

            var q = Quaternion.LookRotation(derivative, Vector3.up);
            outline.transform.rotation = q;

            for (var tt = 0.0f; tt < 1f; tt += (1f / outlineResolution))
            {
                var o = outline.GetPointAt(tt);

                vertices.Add(p + o);
                texCoords.Add(new Vector2(t, tt));
                colors.Add(Color.blue);
            }

            // Add more verts to close seam
            vertices.Add(p + outline.GetPointAt(0f));
            texCoords.Add(new Vector2(t, 1f));
            colors.Add(Color.blue);

            t += dt;
        }

        Debug.Log(vertices.Count);

        // Close path
        outline.transform.rotation = baseRot;
        vertices.AddRange(vertices.GetRange(0, realOutlineResolution));
		for (var i = 0; i < outlineResolution; i++) {
			texCoords.Add (new Vector2 (1f, i / (float)outlineResolution));
		}
		texCoords.Add (Vector2.one);
        //texCoords.AddRange(texCoords.GetRange(0, realOutlineResolution));
        colors.AddRange(colors.GetRange(0, realOutlineResolution));

        // Generate indices
        List<int> indices = new List<int>();
        for (var i = 0; i < resolution; i++)
        {
            for (var j = 0; j < outlineResolution; j++)
            {
                int idx0 = i * realOutlineResolution + (j);
                int idx1 = i * realOutlineResolution + (j + 1);
                int idx2 = i * realOutlineResolution + (j + 1 + realOutlineResolution);
                int idx3 = i * realOutlineResolution + (j + realOutlineResolution);

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

