using UnityEngine;
using System.Collections;
using SHGame;

public class GameInfo : MonoBehaviour
{
    void Awake()
    {
#if UNITY_IPHONE
        SHGameManagerBase.Singleton.Platform = RuntimePlatform.IPhonePlayer;
#elif UNITY_ANDROID
        SHGameManagerBase.Singleton.Platform = RuntimePlatform.Android;
#else
        SHGameManagerBase.Singleton.Platform = RuntimePlatform.WindowsEditor;
#endif


        SHGameManagerBase.Singleton.Awake();
    }

    // Use this for initialization
    void Start()
    {
        SHGameManagerBase.Singleton.Start();
    }

    // Update is called once per frame
    void Update()
    {
        SHGameManagerBase.Singleton.Update(Time.deltaTime);
    }

    void FixedUpdate()
    {
        SHGameManagerBase.Singleton.FixedUpdate(Time.fixedDeltaTime);
    }

    void OnGUI()
    {
        SHGameManagerBase.Singleton.OnGUI();
    }
    void OnDestroy()
    {
        SHGameManagerBase.Singleton.OnDestroy();
    }
}