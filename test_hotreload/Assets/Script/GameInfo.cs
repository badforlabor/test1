using UnityEngine;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

public class GameInfo : MonoBehaviour {

    string Result = "";
    int addedResult = 0;
	// Use this for initialization
	void Start () {
//#if UNITY_ANDROID
        DirectoryInfo di = System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/labor");
        if (di != null)
        {
            Result = di.FullName;
        }
        addedResult = MLGame.Utils.Add(1, 2);
//#else
//        System.IO.Directory.CreateDirectory(Application.dataPath + "/../labor/");
//#endif
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        Rect area = new Rect(10, 10, Screen.width, 20);
        GUI.Label(area, "123123");

        area.yMin += 20;
        area.yMax += 20;
        GUI.Label(area, "1+2=" + (addedResult));

        area.yMin += 20;
        area.yMax += 20;
        GUI.Label(area, "dataPath=" + Application.dataPath);

        area.yMin += 20;
        area.yMax += 20;
        GUI.Label(area, "persistentDataPath=" + Application.persistentDataPath);

        area.yMin += 20;
        area.yMax += 20;
        GUI.Label(area, "streamingAssetsPath=" + Application.streamingAssetsPath);

        area.yMin += 20;
        area.yMax += 20;
        GUI.Label(area, "temporaryCachePath=" + Application.temporaryCachePath);

        area.yMin += 20;
        area.yMax += 20;
        GUI.Label(area, "result=" + Result);

        area.yMin += 20;
        area.yMax += 20;
        GUI.Label(area, "Util.result=" + MLGame.Utils.Result);


#if false
        List<string> files = new List<string>();
        GetAllFiles(Application.dataPath, ref files, false);
        if (files.Count == 0)
        {
            GetAllFiles(Application.dataPath + "!", ref files, false);
        }
        GUI.Label(area, "files=" + files.Count);
        foreach (var file in files)
        {
            area.yMin += 20;
            area.yMax += 20;
            GUI.Label(area, "file=" + file);
        }
#endif

    }

    static void GetAllFiles(string dir, ref List<string> files, bool meta)
    {
        if (!Directory.Exists(dir))
        {
            return;
        }
        string[] curfiles = Directory.GetFiles(dir);
        foreach (var file in curfiles)
        {
            if (!meta && file.EndsWith(".meta"))
            {

            }
            else
            {
                files.Add(file.Replace('\\', '/'));
            }
        }
        curfiles = Directory.GetDirectories(dir);
        foreach (var file in curfiles)
        {
            if (Directory.Exists(file))
            {
                GetAllFiles(file, ref files, meta);
            }
        }
    }

}
