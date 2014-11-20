using UnityEngine;
using System.Collections;

public class AlarmReceiver : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

  AndroidJavaObject nativeObj =null;
  void OnGUI(){


      if (nativeObj != null)
      {
          Rect area = new Rect(10, 110, Screen.width, 20);
          GUI.Label(area, "alarm succ!");
      }
      else
      {
          Rect area = new Rect(10, 110, Screen.width, 20);
          GUI.Label(area, "alarm failed!");
      }

#if UNITY_ANDROID
    if (GUI.Button(new Rect(Screen.width*0.5f-90.0f, 100.0f, 180.0f, 100.0f), "Create Notification")){
      if (nativeObj ==null)
        nativeObj =new AndroidJavaObject("com.macaronics.notification.AlarmReceiver");

      nativeObj.CallStatic("startAlarm", new object[4]{"THIS IS NAME3", "THIS IS TITLE3", "THIS IS LABEL", 10});


    }
#endif
  }
}
