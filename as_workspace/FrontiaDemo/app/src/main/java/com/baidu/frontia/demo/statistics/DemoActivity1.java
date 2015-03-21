package com.baidu.frontia.demo.statistics;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;

import com.baidu.frontia.Frontia;
import com.baidu.frontia.api.FrontiaStatistics;
import com.baidu.frontia.demo.Conf;
import com.baidu.frontia.demo.R;
import com.baidu.mobstat.SendStrategyEnum;

public class DemoActivity1 extends Activity {

	private Button btn_exception;
	private Button btn_event;
	private Button btn_event_duration;
	private Button btn_event_start;
	private Button btn_event_end;

    private FrontiaStatistics stat;
    
    private static final String TAG = "StatisticDemo";

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.statistics);

        stat = Frontia.getStatistics();

        stat.setAppDistributionChannel("X-men", true);//设置应用发布渠道
        stat.setSessionTimeout(50);//测试时，可以使用1秒钟session过期，这样不断的启动退出会产生大量日志。
        stat.enableExceptionLog();//开启异常日志
        stat.setReportId(Conf.reportId);//reportId必须在mtj网站上注册生成，该设置也可以在AndroidManifest.xml中填写

        //第一个参数为日志发送策略
        //第二个参数为日志发送策略设备周期性发送
        //第三个参数为日志发送间隔
        //第四个参数为是否只在wifi情况下发送日志
        stat.start(SendStrategyEnum.SET_TIME_INTERVAL, 10, 10, true);
		btn_exception = (Button) findViewById(R.id.btn_excep);
		btn_event = (Button) findViewById(R.id.btn_event);
		btn_event_duration = (Button) findViewById(R.id.btn_event_duration);

		btn_event_start = (Button) findViewById(R.id.btn_event_start);
		btn_event_end = (Button) findViewById(R.id.btn_event_end);

		btn_exception.setOnClickListener(new OnClickListener() {
			public void onClick(View v) {
				//此处10/0，会发生异常，用来验证上传异常日志
				Log.w(TAG, 10 / 0 + "");
			}
		});

		btn_event.setOnClickListener(new OnClickListener() {
			public void onClick(View v) {

				FrontiaStatistics.Event event = new FrontiaStatistics.Event(Conf.eventId, "提醒");
                stat.logEvent(event);
			}
		});

		/**
		 * 自定义事件的第一种方法，写入某个事件的持续时长
		 */
		btn_event_duration.setOnClickListener(new OnClickListener() {
			public void onClick(View v) {

				FrontiaStatistics.Event event = new FrontiaStatistics.Event(Conf.eventId, "提醒");
                event.setDuration(100); // 事件时长100毫秒
                stat.logEventDuration(event);

			}
		});

		
		 //自定义事件的第二种方法：事件的时长由Frontia统计，而不是开发者统计。
		 
        final FrontiaStatistics.Event asyncEvent = new FrontiaStatistics.Event(Conf.eventId, "提醒");

		btn_event_start.setOnClickListener(new OnClickListener() {
			public void onClick(View v) {
                stat.eventStart(asyncEvent);
            }
		});

		
		 //自定义事件的第二种方法，自己定义该事件的起始时间和结束时间
		 
		btn_event_end.setOnClickListener(new OnClickListener() {
			public void onClick(View v) {
                stat.eventEnd(asyncEvent);

			}
		});
		
	}

	public void onResume() {
		Log.w(TAG, "Activity1.OnResume()");
		super.onResume();

		/**
		 * 页面起始（每个Activity中都需要添加，如果有继承的父Activity中已经添加了该调用，那么子Activity中务必不能添加）
		 * 不能与StatService.onPageStart一级onPageEnd函数交叉使用
		 */
		stat.pageviewStart(this, "DemoActivity1");
	}

	public void onPause() {
		super.onPause();
        Log.w(TAG, "Activity1.onPause()");

		/**
		 * 页面结束（每个Activity中都需要添加，如果有继承的父Activity中已经添加了该调用，那么子Activity中务必不能添加）
		 * 不能与StatService.onPageStart一级onPageEnd函数交叉使用
		 */
		stat.pageviewEnd(this, null);
	}
}