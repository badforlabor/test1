package com.labor.memento;

import java.util.ArrayList;

import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

public class ContentAdapter extends BaseAdapter {
	private Context context;
	ArrayList<Integer> list = new ArrayList<Integer>();
	
	public ContentAdapter(Context context)
	{
		this.context = context;	
		list.add(1);
		list.add(3);
		list.add(2);
		list.add(4);
		list.add(1);
		list.add(3);
		list.add(2);
		list.add(4);
		list.add(1);
		list.add(3);
		list.add(2);
		list.add(4);
	}
	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return list.size();
	}

	@Override
	public Object getItem(int arg0) {
		// TODO Auto-generated method stub
		return list.get(arg0);
	}

	@Override
	public long getItemId(int arg0) {
		// TODO Auto-generated method stub
		return list.get(arg0);
	}

	@Override
	public View getView(int arg0, View view, ViewGroup parent) {
		// TODO Auto-generated method stub
		
		if(view == null)
		{
			try
			{
				view = LayoutInflater.from(context).inflate(R.layout.liaotian, parent, false);
				TextView t1 = (TextView)view.findViewById(R.id.name);
				t1.setText("name-" + list.get(arg0));
				
				view.setClickable(true);
				view.setOnClickListener(new OnClickListener() {
					
					@Override
					public void onClick(View arg0) {
						// TODO Auto-generated method stub
						Log.i("list", "click list!");
					}
				});	
			}
			catch(Exception e)
			{
				Log.i("list", "exception=" + e.getMessage());
			}
		}
		
		
		return view;
	}

}
