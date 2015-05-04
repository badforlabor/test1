package com.labor.memento;

import java.text.DateFormat;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.Date;

import android.content.Context;
import android.content.Intent;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

public class ListRecordAdapter extends BaseAdapter {
	private Context context;
	ArrayList<RecordInfo> list = new ArrayList<RecordInfo>();
	
	public ListRecordAdapter(Context context)
	{
		this.context = context;
        list = RecordInfo.records;
        Collections.sort(list, new Comparator<RecordInfo>() {
            @Override
            public int compare(RecordInfo lhs, RecordInfo rhs) {
                // id由大到小
                return -1 * (lhs.id > rhs.id ? 1 : (lhs.id < rhs.id ? -1 : 0));
            }
        });
	}
	@Override
	public int getCount() {
		return list.size();
	}

	@Override
	public Object getItem(int arg0) {
		return list.get(arg0);
	}

	@Override
	public long getItemId(int arg0) {
		return list.get(arg0).id;
	}

	@Override
	public View getView(int arg0, View view, ViewGroup parent) {
		
		if(view == null)
		{
			try
			{
				view = LayoutInflater.from(context).inflate(R.layout.liaotian, parent, false);
			}
			catch(Exception e)
			{
				Log.i("list", "exception=" + e.getMessage());
			}
		}
        try
        {
            RecordInfo ri = list.get(arg0);
            TextView t1 = (TextView)view.findViewById(R.id.name);
            t1.setText("tag-" + ri.tag);
            t1 = (TextView)view.findViewById(R.id.lastmsg);
            t1.setText("name=" + ri.fileName);
            t1 = (TextView)view.findViewById(R.id.time);
            t1.setText("" + DateFormat.getDateInstance().format(new Date(ri.date)));

            view.setTag(ri.id);
            view.setClickable(true);
            view.setOnClickListener(new OnClickListener() {

                @Override
                public void onClick(View arg0) {
                    // TODO Auto-generated method stub
                    Log.i("list", "click list!");

                    Intent intent = new Intent(arg0.getRootView().getContext(),
                            RecordContentActivity.class);
                    intent.putExtra("tag", ((long)arg0.getTag()));
                    arg0.getRootView().getContext().startActivity(intent);
                }
            });
        }
        catch(Exception e)
        {
            Log.i("list", "exception=" + e.getMessage());
        }
		
		return view;
	}

}
