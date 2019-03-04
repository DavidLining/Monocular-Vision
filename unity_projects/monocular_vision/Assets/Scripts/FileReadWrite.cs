using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class FileReadWrite
{

    //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。  
    public static readonly string PathURL =
#if UNITY_ANDROID   //安卓  
		"jar:file://" + Application.dataPath + "!/assets/";  
#elif UNITY_IPHONE  //iPhone  
		Application.dataPath + "/Raw/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台  
        "file://" + Application.dataPath + "/StreamingAssets/";
#else
		string.Empty;  
#endif

    //切记，你的二进制文件一定要放在StreamingAssets ！！！！！！
#if UNITY_EDITOR
    string filepath = Application.dataPath + "/StreamingAssets" + "/my.xml";
#elif UNITY_IPHONE
		string filepath = Application.dataPath +"/Raw"+"/my.xml";
#elif UNITY_ANDROID
		string filepath = "jar:file://" + Application.dataPath + "!/assets/"+"/my.xml;
#endif


    //文本中每行的内容  
    ArrayList infoall;
    //皮肤资源，这里用于显示中文  
    public GUISkin skin;
    void Start()
    {
        //Debug.Log("当前文件路径:" + Application.persistentDataPath);
        ////删除文件  
        //DeleteFile(Application.persistentDataPath, "FileName.txt");
        ////创建文件，共写入3次数据  
        //CreateFile(Application.persistentDataPath, "FileName.txt", "dingxiaowei");
        //CreateFile(Application.persistentDataPath, "FileName.txt", "丁小未");
        ////CreateFile(Application.persistentDataPath ,"Filename.assetbundle","丁小未");  
        ////下载模型  
        //StartCoroutine(loadasset("http://192.168.1.180/3DShowResource/Products/AssetBundles/HX_DY02.assetbundle"));
        ////得到文本中每一行的内容  
        //infoall = LoadFile(Application.persistentDataPath, "FileName.txt");

    }

    //写入模型到本地  
    IEnumerator loadasset(string url)
    {
        WWW w = new WWW(url);
        yield return w;
        if (w.isDone)
        {
            byte[] model = w.bytes;
            int length = model.Length;
            //写入模型到本地  
            CreateModelFile(Application.persistentDataPath, "Model.assetbundle", model, length);
        }
    }

    void CreateModelFile(string path, string name, byte[] info, int length)
    {
        //文件流信息  
        //StreamWriter sw;  
        Stream sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            //如果此文件不存在则创建  
            sw = t.Create();
        }
        else
        {
            //如果此文件存在则打开  
            //sw = t.Append();  
            return;
        }
        //以行的形式写入信息  
        //sw.WriteLine(info);  
        sw.Write(info, 0, length);
        //关闭流  
        sw.Close();
        //销毁流  
        sw.Dispose();
    }

    /** 
   * path：文件创建目录 
   * name：文件的名称 
   *  info：写入的内容 
   */
    public void CreateFile(string path, string name, string info)
    {
        //文件流信息  
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            //如果此文件不存在则创建  
            sw = t.CreateText();
        }
        else
        {
            //如果此文件存在则打开  
           // sw = t.AppendText();
            sw = t.CreateText();
        }
        //以行的形式写入信息  
        sw.WriteLine(info);
        //关闭流  
        sw.Close();
        //销毁流  
        sw.Dispose();
    }



    /** 
   * 读取文本文件 
   * path：读取文件的路径 
   * name：读取文件的名称 
   */
    ArrayList LoadFile(string path, string name)
    {
        //使用流的形式读取  
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            //路径与名称未找到文件则直接返回空  
            return null;
        }
        string line;
        ArrayList arrlist = new ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            //一行一行的读取  
            //将每一行的内容存入数组链表容器中  
            arrlist.Add(line);
        }
        //关闭流  
        sr.Close();
        //销毁流  
        sr.Dispose();
        //将数组链表容器返回  
        return arrlist;
    }

    /** 
   * 读取文本文件 
   * path：读取文件的路径 
   * name：读取文件的名称 
   */
    public string ReadFileAll(string path, string name)
    {
        //使用流的形式读取  
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            //路径与名称未找到文件则直接返回空  
            return null;
        }
        string data;
        data = sr.ReadToEnd();
        //关闭流  
        sr.Close();
        //销毁流  
        sr.Dispose();
        //将数组链表容器返回  
        return data;
    }


    //读取模型文件  
//    IEnumerator LoadModelFromLocal(string path, string name)
//    {
//        string s = null;
//#if UNITY_ANDROID
//		s = "jar:file://"+path+"/"+name;  
//#elif UNITY_IPHONE
//		s = path+"/"+name;  
//#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
//        s = "file://" + path + "/" + name;
//#endif
//        WWW w = new WWW(s);
//        yield return w;
//        if (w.isDone)
//        {
//            Instantiate(w.assetBundle.mainAsset);
//        }
//    }


    /** 
   * path：删除文件的路径 
   * name：删除文件的名称 
   */

    void DeleteFile(string path, string name)
    {
        File.Delete(path + "//" + name);
    }

    //void OnGUI()
    //{
    //    //用新的皮肤资源，显示中文  
    //    GUI.skin = skin;
    //    //读取文件中的所有内容  
    //    foreach (string str in infoall)
    //    {
    //        //绘制在屏幕当中  
    //        GUILayout.Label(str);
    //    }
    //    if (GUILayout.Button("加载模型"))
    //    {
    //        StartCoroutine(LoadModelFromLocal(Application.persistentDataPath, "Model.assetbundle"));
    //    }
    //}
}
