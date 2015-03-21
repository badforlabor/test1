/**
 * 音频列表界面，查看录音的数量。
 */
package com.labor.memento;

import java.util.ArrayList;


import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

public class RecordList extends Activity {
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
		list1.setAdapter(new ContentAdapter(this));
		Log.i("list", "22 width=" + list1.getWidth() + ", height=" + list1.getHeight());
	}
}
