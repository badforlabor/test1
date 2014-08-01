using UnityEngine;
using System.Collections;

public class TestAssetBundle : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        UnityEngine.Object templ = Resources.Load("ring");
        GameObject go = UnityEngine.Object.Instantiate(templ) as GameObject;

        System.Diagnostics.Debug.Assert(go != null);

        go.transform.localRotation = new Quaternion(0,-90,0,0);

        yield return null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
