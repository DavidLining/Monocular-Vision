using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class get_coordinate : MonoBehaviour {
    SphereCollider Sphere_Red, Sphere_Blue;
    Vector3 sv1Position, sv2Position;
    Vector3 pv1Position, pv2Position;
    Camera mainCamera;
    Text coord_text;
    // Use this for initialization
    void Start () {
        //    coord_text = GameObject.Find("Canvas/Text").GetComponent<Text>();
        coord_text = GetComponent<Text>();
        Sphere_Blue = GameObject.Find("Sphere_Blue").GetComponent<SphereCollider>();
        Sphere_Red = GameObject.Find("Sphere_Red").GetComponent<SphereCollider>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 camVector = mainCamera.transform.position;

        sv1Position = new Vector3(Sphere_Red.gameObject.transform.position.x, Sphere_Red.gameObject.transform.position.y, Sphere_Red.gameObject.transform.position.z);
        sv2Position = new Vector3(Sphere_Blue.gameObject.transform.position.x, Sphere_Blue.gameObject.transform.position.y, Sphere_Blue.gameObject.transform.position.z);


        pv1Position = mainCamera.WorldToScreenPoint(sv1Position);
        pv2Position = mainCamera.WorldToScreenPoint(sv2Position);
        pv1Position[2] = 1.0f;
        pv2Position[2] = 1.0f;
        float length = (pv1Position - pv2Position).magnitude;
        string sv = "3D:" + "\n" + "Red:" + sv1Position.ToString("F8") + "\n" + "Blue:" + sv2Position.ToString("F8") + "\n";
        string pv = "2D:" + "\n" + "Red:" + pv1Position.ToString("F8") + "\n" + "Blue:" + pv2Position.ToString("F8") + "\n";
        string len = "Img Length:" + length.ToString("F8") + "\n";
        string screeninfo = "Screen width and height: " + Screen.width + ", " + Screen.height + "\n";

        string eulerAngles = "Camera Euler:" + button_handle.rotate_angles.ToString("F8") + "\n";
        string unity_eulerAngles = "Camera Unity Euler:" + mainCamera.transform.eulerAngles.ToString("F8") + "\n";
        coord_text.text = sv + pv + screeninfo + eulerAngles + unity_eulerAngles;
      //  coord_text.gameObject.SetActive(true);
        Debug.Log("Refresh");
    }
}
