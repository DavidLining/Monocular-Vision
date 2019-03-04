using System.Collections;

using System.Collections.Generic;

using UnityEngine;

public class draw_rectangle : MonoBehaviour
{

    public static GameObject go;

    public static MeshFilter mf;

    public static MeshRenderer mr;

    public static Shader shader;

    // Update is called once per frame

    void Update()
    {


        ToDrawRectangleSolid(transform, transform.localPosition, 100, 10);

    }

    private static GameObject CreateMesh(List<Vector3> vertices)

    {

        int[] triangles;

        Mesh mesh = new Mesh();

        int triangleAmount = vertices.Count - 2;

        triangles = new int[3 * triangleAmount];

        //根据三角形的个数，来计算绘制三角形的顶点顺序（索引）   

        //顺序必须为顺时针或者逆时针     

        for (int i = 0; i < triangleAmount; i++)

        {

            triangles[3 * i] = 0;//固定第一个点      

            triangles[3 * i + 1] = i + 1;

            triangles[3 * i + 2] = i + 2;

        }

        if (go == null)

        {

            go = new GameObject("mesh");

            go.transform.position = new Vector3(0, 0, 0);//让绘制的图形上升一点，防止被地面遮挡  

            mf = go.AddComponent<MeshFilter>();

            mr = go.AddComponent<MeshRenderer>();

            shader = Shader.Find("Unlit/Color");

        }

        mesh.vertices = vertices.ToArray();

        mesh.triangles = triangles;

        mf.mesh = mesh;

        mr.material.shader = shader;

        mr.material.color = Color.red;

        return go;

    }

    //绘制实心长方形  

    //以长方形的底边中点为攻击方位置(从俯视角度来看)  

    public static void ToDrawRectangleSolid(Transform t, Vector3 bottomMiddle, float length, float width)

    {

        List<Vector3> vertices = new List<Vector3>();

        vertices.Add(bottomMiddle - t.right * (width / 2));

        vertices.Add(bottomMiddle - t.right * (width / 2) + t.forward * length);

        vertices.Add(bottomMiddle + t.right * (width / 2) + t.forward * length);

        vertices.Add(bottomMiddle + t.right * (width / 2));

        CreateMesh(vertices);

    }

}