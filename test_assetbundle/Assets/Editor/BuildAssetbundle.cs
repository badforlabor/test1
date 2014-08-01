using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class BuildAssetbundle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [MenuItem("AssetBundle/打包")]
    static void BuildAssetBundles()
    {
        string Output = Application.dataPath + "/../AssetBundle/a";
        
        Directory.CreateDirectory(Output);

        BuildPipeline.PushAssetDependencies();
        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath("Assets/AuroraRing.tga", typeof(UnityEngine.Object));
        BuildPipeline.BuildAssetBundle(obj, null, Output + "/2.assetbundle");

        BuildPipeline.PushAssetDependencies();
        obj = AssetDatabase.LoadAssetAtPath("Assets/Materials/AuroraRing.mat", typeof(UnityEngine.Object));
        BuildPipeline.BuildAssetBundle(obj, null, Output + "/3.assetbundle");
        BuildPipeline.PopAssetDependencies();

        BuildPipeline.PopAssetDependencies();
    }

}
