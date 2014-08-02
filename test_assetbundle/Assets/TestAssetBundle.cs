using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestAssetBundle : MonoBehaviour {

    static string LOADFOLDER = "iLingYou-j2/";

    Dictionary<string, WWW> dict = new Dictionary<string, WWW>();

	// Use this for initialization
	void Start () {

        //StartCoroutine(LoadFromResource());
        //StartCoroutine(LoadFromAssetbundle());
        //StartCoroutine(Func());
        StartCoroutine(LoadRing());
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
