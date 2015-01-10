using UnityEngine;
using System.Collections;

public class SHTestNav : MonoBehaviour {

	// Use this for initialization
	void Start () {
        NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent>();
        nav.SetDestination(new Vector3(0,0,4.48f));
	}
	
	// Update is called once per frame
	void Update () {
        NavMeshAgent nav = gameObject.GetComponent<NavMeshAgent>();
        GameObject go = GameObject.Find("ar_lieren");
        if (go != null)
        {
            nav.SetDestination(go.transform.position);
        }
	}
}
