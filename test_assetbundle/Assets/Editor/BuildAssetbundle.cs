using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System;
using LitJson;

public class BuildAssetbundle : MonoBehaviour {

    static string ReplaceSlashWithBlackspace(string abc)
    {
        return abc.Replace('/', '-').Replace('\\', '-');
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [MenuItem("AssetBundle/打包")]
    static void BuildAssetBundles()
    {
        bool bdo = EditorUtility.DisplayDialog("", "手动写的依赖关系，确定打包吗？", "OK");
        if (!bdo)
        {
            return;
        }
        Do3();
    }
    static void Do3()
    {
        string Output = Application.dataPath + "/../AssetBundles/Standalones/iLingYou-h";

        Directory.CreateDirectory(Output);

        List<string> one = new List<string>();
        one.Add("Assets/Resources/ring.prefab");
        string[] deps = AssetDatabase.GetDependencies(one.ToArray());
        List<UnityEngine.Object> objs = new List<UnityEngine.Object>();

        List<AssetBundle> abs = new List<AssetBundle>();

        BuildPipeline.PushAssetDependencies();
        AssetBundle ab = AssetBundle.CreateFromFile(Output + "/" + "Assets-AuroraRing.tga.assetbundle");
        if (ab == null)
        {
            Debug.LogError("wo ca!");
        }
        abs.Add(ab);
        BuildPipeline.PushAssetDependencies();
        abs.Add(AssetBundle.CreateFromFile(Output + "/" + "Assets-AuroraSpikeyRing.tga.assetbundle"));
        BuildPipeline.PushAssetDependencies();
        abs.Add(AssetBundle.CreateFromFile(Output + "/" + "Assets-caodui_01.tga.assetbundle"));
        BuildPipeline.PushAssetDependencies();
        abs.Add(AssetBundle.CreateFromFile(Output + "/" + "Assets-Materials-AuroraRing.mat.assetbundle"));
        BuildPipeline.PushAssetDependencies();
        abs.Add(AssetBundle.CreateFromFile(Output + "/" + "Assets-Materials-AuroraSpikeyRing.mat.assetbundle"));
        BuildPipeline.PushAssetDependencies();
        abs.Add(AssetBundle.CreateFromFile(Output + "/" + "Assets-Materials-caodui_01.mat.assetbundle"));
        BuildPipeline.PushAssetDependencies();
        abs.Add(AssetBundle.CreateFromFile(Output + "/" + "Assets-Resources-ring-1.prefab.assetbundle"));
        BuildPipeline.PushAssetDependencies();
        abs.Add(AssetBundle.CreateFromFile(Output + "/" + "Assets-Resources-ring-2.prefab.assetbundle"));

        BuildPipeline.PushAssetDependencies();
        UnityEngine.Object obj2 = AssetDatabase.LoadAssetAtPath("Assets/Resources/ring.prefab", typeof(UnityEngine.Object));
        BuildPipeline.BuildAssetBundle(obj2, null, Output + "/Assets-Resources-ring.prefab.assetbundle");
        BuildPipeline.PopAssetDependencies();
        foreach (var dep in deps)
        {
            BuildPipeline.PopAssetDependencies();
        }
        foreach (var abb in abs)
        {
            if (abb)
            {
                abb.Unload(true);
            }
        }
        abs.Clear();


    }

    // 失败
    static void Do2()
    {
        string Output = Application.dataPath + "/../AssetBundles/Standalones/iLingYou-h";

        Directory.CreateDirectory(Output);

        List<string> one = new List<string>();
        one.Add("Assets/Resources/ring.prefab");
        string[] deps = AssetDatabase.GetDependencies(one.ToArray());
        List<UnityEngine.Object> objs = new List<UnityEngine.Object>();
        foreach (var dep in deps)
        {
            BuildPipeline.PushAssetDependencies();
            //objs.Add(AssetDatabase.LoadAssetAtPath(dep, typeof(UnityEngine.Object)));
            //DebugTools.Assert(obj != null);
            Debug.Log("dep:" + dep);
        }
        UnityEngine.Object obj2 = AssetDatabase.LoadAssetAtPath("Assets/Resources/ring.prefab", typeof(UnityEngine.Object));
        BuildPipeline.BuildAssetBundle(obj2, null, Output + "/Assets-Resources-ring.prefab.assetbundle");
        foreach (var dep in deps)
        {
            BuildPipeline.PopAssetDependencies();
        }

    }
    static void Do1()
    {
        string Output = Application.dataPath + "/../AssetBundles/Standalones/iLingYou-h";
        
        Directory.CreateDirectory(Output);

        List<string> one = new List<string>();
        one.Add("Assets/Resources/ring.prefab");
        string[] deps = AssetDatabase.GetDependencies(one.ToArray());
        foreach (var dep in deps)
        {
            Debug.Log("dep:" + dep);
        }
        Debug.Log("0------Memory:" + System.GC.GetTotalMemory(false));
        BuildPipeline.PushAssetDependencies();
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath("Assets/AuroraRing.tga", typeof(UnityEngine.Object));
        BuildPipeline.BuildAssetBundle(obj, null, Output + "/2.assetbundle");
        Debug.Log("1------Memory:" + System.GC.GetTotalMemory(false));
        obj = null;
        System.GC.Collect();
        Debug.Log("2------Memory:" + System.GC.GetTotalMemory(false));

        BuildPipeline.PushAssetDependencies();
        obj = AssetDatabase.LoadAssetAtPath("Assets/Materials/AuroraRing.mat", typeof(UnityEngine.Object));
        BuildPipeline.BuildAssetBundle(obj, null, Output + "/3.assetbundle");
        BuildPipeline.PopAssetDependencies();

        BuildPipeline.PopAssetDependencies();
    }





}
