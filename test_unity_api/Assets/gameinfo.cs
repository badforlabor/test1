using UnityEngine;
using System.Collections;

public class gameinfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Object gotemplate = Resources.Load("effect1");
        GameObject.Instantiate(gotemplate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
