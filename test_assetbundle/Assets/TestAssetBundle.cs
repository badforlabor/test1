using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestAssetBundle : MonoBehaviour {

    static string LOADFOLDER = "iLingYou-j2/";

    Dictionary<string, WWW> dict = new Dictionary<string, WWW>();

    float CostTime = 0;

	// Use this for initialization
	void Start () {

        //StartCoroutine(LoadFromResource());
        //StartCoroutine(LoadFromAssetbundle());
        //StartCoroutine(Func());
        //StartCoroutine(LoadRing());

        StartCoroutine(LoadDM2());

    }
    void OnGUI()
    {
        Rect area = new Rect(10, 10, 200, 20);
        GUI.Label(area, "CostTime: " + CostTime);

        area.yMin += 50;
        area.yMax += 50;
        GUI.Label(area, "Total Memory:" + System.GC.GetTotalMemory(false));

        area.yMin += 50;
        area.yMax += 50;
        if (GUI.Button(area, "Unload Create"))
        {
            UnLoadCreates();
        }

        area.yMin += 50;
        area.yMax += 50;
        if (GUI.Button(area, "force Unload Create"))
        {
            ForceUnLoadCreates();
        }

        area.yMin += 50;
        area.yMax += 50;
        if (GUI.Button(area, "Unload WWW"))
        {
            UnLoadWWW();
        }

    }
    void UnLoadCreates()
    {
        DownloadManager dl = DownloadManager.Instance;
        dl.FreeAllCreates();
    }
    void ForceUnLoadCreates()
    {
        DownloadManager dl = DownloadManager.Instance;
        dl.FreeAllCreates(true);
    }
    void UnLoadWWW()
    { 
    
    }

    IEnumerator LoadDM2()
    {
        DownloadManager dl = DownloadManager.Instance;

        while (!dl.ConfigLoaded)
        {
            yield return new WaitForSeconds(2);
        }

        //dl.StartDownload("Assets-Resources-ring.prefab.assetbundle");

        //string MainAsset = "Assets-Resources-ring.prefab";
        string OrigMainAsset = "Assets/Resources/Prefab/MapEditor/Rooms/huianchengzhen/huianchengzhen01.prefab";
        string MainAsset =  DownloadManager.ReplaceSlashWithBlackspace(OrigMainAsset);

        float BeginTime = Time.realtimeSinceStartup;
#if true
        dl.PendingDownload(MainAsset);
        yield return StartCoroutine(dl.DoCoDownload());
        WWW www = dl.GetWWW(MainAsset);
        UnityEngine.Object cls = www.assetBundle.mainAsset;
#elif true
        dl.PendingDownload(MainAsset);
        dl.DoDownload();

        while (dl.IsDownloading())
        {
            //yield return new WaitForSeconds(1);
            yield return new WaitForFixedUpdate();
        }
        //yield return StartCoroutine(dl.WaitDownload("1"));
        WWW www = dl.GetWWW(MainAsset);
        UnityEngine.Object cls = www.assetBundle.mainAsset;
#else
        UnityEngine.Object cls = dl.CreateFromFile(OrigMainAsset).mainAsset;
#endif

        UnityEngine.GameObject go = UnityEngine.Object.Instantiate(cls) as GameObject;
        DebugTools.Assert(go != null);
        go.transform.localRotation = new Quaternion(0, -90, 0, 0);
        Debug.Log("************** total time=" + (Time.realtimeSinceStartup - BeginTime));
        CostTime = (Time.realtimeSinceStartup - BeginTime);

        yield return null;
    }

    IEnumerator LoadDM1()
    {
        DownloadManager dl = DownloadManager.Instance;

        while (!dl.ConfigLoaded)
        {
            yield return new WaitForSeconds(2);
        }

        //dl.StartDownload("Assets-Resources-ring.prefab.assetbundle");

        dl.PendingDownload("Assets-Resources-ring.prefab");
        dl.DoDownload();

        while (dl.IsDownloading())
        {
            yield return new WaitForSeconds(3);
        }
        //yield return StartCoroutine(dl.WaitDownload("1"));
        WWW www = dl.GetWWW("Assets-Resources-ring.prefab");
        UnityEngine.Object cls = www.assetBundle.mainAsset;
        UnityEngine.GameObject go = UnityEngine.Object.Instantiate(cls) as GameObject;
        DebugTools.Assert(go != null);
        go.transform.localRotation = new Quaternion(0, -90, 0, 0);

        yield return null;
    }

    
    IEnumerator LoadRing()
    {
        yield return StartCoroutine(LoadAB("Assets-AuroraRing.tga.assetbundle"));

        yield return StartCoroutine(LoadAB("Assets-AuroraSpikeyRing.tga.assetbundle"));
        yield return StartCoroutine(LoadAB("Assets-caodui_01.tga.assetbundle"));
        yield return StartCoroutine(LoadAB("Assets-Materials-AuroraRing.mat.assetbundle"));
        yield return StartCoroutine(LoadAB("Assets-Materials-AuroraSpikeyRing.mat.assetbundle"));
        yield return StartCoroutine(LoadAB("Assets-Materials-caodui_01.mat.assetbundle"));

        yield return StartCoroutine(LoadAB("Assets-Resources-ring-1.prefab.assetbundle"));
        yield return StartCoroutine(LoadAB("Assets-Resources-ring-2.prefab.assetbundle"));
        yield return StartCoroutine(LoadAB("Assets-Resources-ring.prefab.assetbundle"));

        yield return StartCoroutine(LoadAB("Assets-Resources-grass.prefab.assetbundle"));

        WWW www = dict["Assets-Resources-ring.prefab.assetbundle"];

        UnityEngine.Object cls = www.assetBundle.mainAsset;

        UnityEngine.GameObject go = UnityEngine.Object.Instantiate(cls) as GameObject;
        DebugTools.Assert(go != null);
        go.transform.localRotation = new Quaternion(0, -90, 0, 0);


        yield return null;
    }

    IEnumerator LoadAB(string file)
    {
        string path = Application.dataPath
                        + "/../AssetBundles/Standalones/iLingYou-g/";

        WWW www = WWW.LoadFromCacheOrDownload("file:///" + path + file, -1);
        yield return www;
        DebugTools.Assert(www != null && www.isDone && www.assetBundle != null);

        dict.Add(file, www);

        yield return null;
    }


    IEnumerator Func()
    {
        yield return StartCoroutine(Func_A());
        yield return StartCoroutine(Func_B());
        yield return StartCoroutine(Func_A());

        Debug.Log("----------- End!" + Time.realtimeSinceStartup);
        yield return null;
    }
    IEnumerator LoadFromResource()
    {
        UnityEngine.Object templ = Resources.Load("ring");
        GameObject go = UnityEngine.Object.Instantiate(templ) as GameObject;

        System.Diagnostics.Debug.Assert(go != null);

        go.transform.localRotation = new Quaternion(0, -90, 0, 0);

        yield return null;
    }

    IEnumerator LoadFromAssetbundle()
    {
        string path = Application.dataPath.Substring(0, Application.dataPath.IndexOf("Assets"))
                        + "AssetBundles/Standalones/iLingYou-g/";

        if (System.IO.File.Exists(path))
        {
            Debug.Log("cannot find :" + path);
        }

        WWW www = WWW.LoadFromCacheOrDownload("file:///" + path + "Assets-AuroraRing.tga.assetbundle", -1);
        yield return www;
        if (www.error != null)
        {
            Debug.Log("error=" + www.error);
            AssetBundle ab = AssetBundle.CreateFromFile("file:///" + path + "Assets-AuroraRing.tga.assetbundle");
            System.Diagnostics.Debug.Assert(ab != null);
        }
        else
        {
            Debug.Log("assetbundle =" + www.assetBundle.name);
        }

        System.Diagnostics.Debug.Assert(www != null);

        yield return null;
    }

    IEnumerator Func_A()
    {
        Debug.Log("---------------- A." + Time.realtimeSinceStartup);
        yield return new WaitForSeconds(1);
    }

    IEnumerator Func_B()
    {
        Debug.Log("---------------- B." + +Time.realtimeSinceStartup);
        yield return new WaitForSeconds(1);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
