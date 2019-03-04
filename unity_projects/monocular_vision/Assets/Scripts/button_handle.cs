using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class button_handle : MonoBehaviour {
    private GameObject setEuler_btn;
    private GameObject angle_X_inputF, angle_Y_inputF, angle_Z_inputF;
    private Camera mainCamera;
    public static Vector3 rotate_angles = new Vector3(0, 0, 0);
    // Use this for initialization
    void Start () {
        setEuler_btn = GameObject.Find("Canvas/SetEulerBtn");
        angle_X_inputF = GameObject.Find("InputXAngle");
        angle_Y_inputF = GameObject.Find("InputYAngle");
        angle_Z_inputF = GameObject.Find("InputZAngle");
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        setEuler_btn.GetComponent<Button>().onClick.AddListener(handleBtn_setEuler);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.M))
        {
            float angle_x_f = UnityEngine.Random.Range(-30.0f, 30.0f);
            float angle_y_f = UnityEngine.Random.Range(-30.0f, 30.0f);
            float angle_z_f = UnityEngine.Random.Range(-180.0f, 180.0f);
            mainCamera.transform.eulerAngles = new Vector3(0, 0, 0);

            mainCamera.transform.Rotate(new Vector3(0.0f, 0.0f, angle_z_f));
            mainCamera.transform.Rotate(new Vector3(angle_x_f, 0.0f, 0.0f));
            mainCamera.transform.Rotate(new Vector3(0.0f, angle_y_f, 0.0f));
            rotate_angles = new Vector3(angle_x_f, angle_y_f, angle_z_f);
        }


    }

    private void handleBtn_setEuler()
    {
        string angle_x = angle_X_inputF.GetComponent<InputField>().text;
        string angle_y = angle_Y_inputF.GetComponent<InputField>().text;
        string angle_z = angle_Z_inputF.GetComponent<InputField>().text;


        if (angle_x == "")
        {
            angle_x = "0.0";
        }

        if (angle_y == "")
        {
            angle_y = "0.0";
        }

        if (angle_z == "")
        {
            angle_z = "0.0";
        }
        mainCamera.transform.eulerAngles = new Vector3(0, 0, 0);


        float angle_x_f = float.Parse(angle_x);
        float angle_y_f = float.Parse(angle_y);
        float angle_z_f = float.Parse(angle_z);


        mainCamera.transform.Rotate(new Vector3(0.0f, 0.0f, angle_z_f));
        mainCamera.transform.Rotate(new Vector3(angle_x_f, 0.0f, 0.0f));
        mainCamera.transform.Rotate(new Vector3(0.0f, angle_y_f, 0.0f));
        rotate_angles = new Vector3(angle_x_f, angle_y_f, angle_z_f);
        // mainCamera.transform.rotation = Quaternion.Euler(0.0f, 0.0f, float.Parse(angle_z));
        //  mainCamera.transform.rotation = Quaternion.Euler(float.Parse(angle_x), 0.0f, 0.0f);
        //  mainCamera.transform.rotation = Quaternion.Euler(0.0f, float.Parse(angle_y), 0.0f);
        // mainCamera.transform.eulerAngles = new Vector3(float.Parse(angle_x), float.Parse(angle_y), float.Parse(angle_z));
        Debug.Log(mainCamera.transform.position);
    }


}
