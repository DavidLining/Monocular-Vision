using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveResult : MonoBehaviour {
    Text coord_text;
    FileReadWrite frw;
    public static bool if_save = true;
    string result = "";
    // Use this for initialization
    void Start () {
        frw = new FileReadWrite();
        coord_text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        result = "camera1:\n" + coord_text.text;
        string curr_dir = System.Environment.CurrentDirectory;
        if (if_save)
        {
            if (File.Exists(curr_dir + "\\" + "Coordinates_last.txt"))
            {
                string pre_data = frw.ReadFileAll(curr_dir, "Coordinates_last.txt");
                frw.CreateFile(curr_dir, "Coordinates_sec_last.txt", pre_data);
            }
            if (File.Exists(curr_dir + "\\" + "Coordinates.txt"))
            {
                string pre_data = frw.ReadFileAll(curr_dir, "Coordinates.txt");
                frw.CreateFile(curr_dir, "Coordinates_last.txt", pre_data);

            }
            frw.CreateFile(curr_dir, "Coordinates.txt", result);
        }
    }
    public static void set_save_status(bool status)
    {
        if_save = status;
    }
}
