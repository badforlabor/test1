using UnityEngine;
using System.Collections;

public class autofacetocamera : MonoBehaviour {

    public Camera _camera = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (_camera == null)
        {
            Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
            int layermask = 1 << gameObject.layer;
            foreach (var c in cameras)
            {
                if (c != null && c.enabled && ((c.cullingMask & layermask) > 0))
                {
                    _camera = c;
                    break;
                }
            }
        }

        if (_camera != null)
        {
            transform.rotation = _camera.transform.rotation;
        }
	}
}
