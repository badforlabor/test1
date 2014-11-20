
/*
 * 测试例子：
 *      定时通知（比如，9点，12点，18点，21点）
 *      游戏启动时，取消通知
 * 
 * 
 * 
 * **/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class notify : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ClearSchedule();
	}

    void MakeNotify(string title, int hour, int min = 0)
    {
#if UNITY_IPHONE
        DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, min, 0);
        // 时间如果不对，会失败
        if (time > DateTime.Now)
        {
            time.AddDays(1);
        }
        LocalNotification noti = new LocalNotification();
        noti.alertAction = "游戏：";
        noti.alertBody = title;
        noti.fireDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, min, 0);
        noti.hasAction = true;

        // 每天都提醒
        noti.repeatCalendar = CalendarIdentifier.ChineseCalendar;
        noti.repeatInterval = CalendarUnit.Day;

        noti.soundName = LocalNotification.defaultSoundName;
#if false
        // useInfo这个成员变量不知道有神马用啊！
        Dictionary<string, string> userInfo = new Dictionary<string, string>();
        userInfo.Add("1", noti.alertBody);
        noti.userInfo = userInfo;
#endif
        NotificationServices.ScheduleLocalNotification(noti);

        // 测试
        //NotificationServices.PresentLocalNotificationNow(noti);
#endif
    }
    void ClearSchedule()
    {
#if UNITY_IPHONE
        NotificationServices.CancelAllLocalNotifications();
#endif
    }
    void SetSchedule()
    {
        MakeNotify("9点有活动哦", 9);
        MakeNotify("17点有活动哦", 17);
        MakeNotify("17:10点有活动哦", 17, 10);
        MakeNotify("17:20点有活动哦", 17, 20);
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SetSchedule();
        }
        else
        {
            ClearSchedule();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnGUI()
    {
#if UNITY_IPHONE
        Rect area = new Rect(10, 10, Screen.width, 20);
        GUI.Label(area, "remote=" + NotificationServices.remoteNotificationCount);
#endif
    }
}
