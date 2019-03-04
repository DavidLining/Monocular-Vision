using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class draw_lines_h : MonoBehaviour
{
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    private int lengthOfLineRenderer = 40;
    private float start_x = -100.0f;

    // Use this for initialization
    void Start()
    {

        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));

 //       UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath("Assets/Materials/Blue.mat");
 //       Material mat = obj as Material;

        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = lengthOfLineRenderer;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;
        lineRenderer.useWorldSpace = true;

    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        var points = new Vector3[lengthOfLineRenderer];
        int group_size = lengthOfLineRenderer / 4;
        for (int i = 0; i < group_size; i++)
        {
            points[4 * i] = new Vector3(start_x + 2 * i * 10.0f, -100.0f, 100.0f);
            points[4 * i + 1] = new Vector3(start_x + 2 * i * 10.0f, 100.0f, 100.0f);
            points[4 * i + 2] = new Vector3(start_x + (2 * i + 1) * 10.0f, 100.0f, 100.0f);
            points[4 * i + 3] = new Vector3(start_x + (2 * i + 1) * 10.0f, -100.0f, 100.0f);
        }
        lineRenderer.SetPositions(points);

    }
}
