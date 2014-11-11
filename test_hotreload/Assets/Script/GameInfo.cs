using UnityEngine;
using System.Collections;
using System.Reflection;
using System.IO;

public class GameInfo : MonoBehaviour {

    string Result = "";
	// Use this for initialization
	void Start () {
//#if UNITY_ANDROID
        DirectoryInfo di = System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/labor");
        if (di != null)
        {
            Result = di.FullName;
        }
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
        GUI.Label(area, "1+2=" + (MLGame.Utils.Add(1,2)));

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

    }
}
