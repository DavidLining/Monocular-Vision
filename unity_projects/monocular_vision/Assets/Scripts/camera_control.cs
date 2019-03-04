using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_control : MonoBehaviour {
    public float near = 20.0f;
    public float far = 100.0f;

    public float sensitivetyZ = 2f;
    public float sensitivityX = 1.0f;
    public float sensitivityY = 1.0f;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        //鼠标右键实现视角转动，类似第一人称视角  
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse X") * sensitivityX;
            float rotationY = Input.GetAxis("Mouse Y") * sensitivityY;

            transform.Rotate(-rotationY, rotationX, 0);
          //  Quaternion target = Quaternion.Euler(rotationX, rotationY, 0);
          //  transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 5.0f);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(sensitivityX, 0, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(-sensitivityX, 0, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, sensitivityY, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, -sensitivityY, 0, Space.Self);
        }



        //键盘按钮←和→实现视角水平旋转  
        if (Input.GetAxis("Horizontal") != 0)
        {
            float rotationZ = Input.GetAxis("Horizontal") * sensitivetyZ;
            transform.Rotate(0, 0, rotationZ);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);

        }
    }
}
