/**
 * 音频列表界面，查看录音的数量。
 */
package com.labor.memento;


import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.widget.ListView;

public class RecordListActivity extends Activity {
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.record_list_activity);
		
		SetListData();
	}
	void SetListData()
	{
		ListView list1 = (ListView)this.findViewById(R.id.listView1);
		Log.i("list", "width=" + list1.getWidth() + ", height=" + list1.getHeight());
		list1.setAdapter(new ListRecordAdapter(this));
		Log.i("list", "22 width=" + list1.getWidth() + ", height=" + list1.getHeight());
	}
}
