package com.labor.memento;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;

import java.util.ArrayList;

/**
 * Created by harold on 2015/3/28.
 */
public class ListRecordContentAdapter extends BaseAdapter {

    Context mContext = null;

    ArrayList<Integer> list = new ArrayList<Integer>();

    public ListRecordContentAdapter(Context context){
        mContext = context;
        list.add(1);
        list.add(3);
    }

    @Override
    public int getCount() {
        return list.size();
    }

    @Override
    public Object getItem(int position) {
        return list.get(position);
    }

    @Override
    public long getItemId(int position) {
        return list.get(position);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        if(convertView == null)
        {
            try {
                convertView = LayoutInflater.from(mContext).inflate(R.layout.list_record_content, parent, false);
                Button btn = (Button) convertView.findViewById(R.id.list_rec_co_btn_play);
                btn.setTag("" + list.get(position));
                btn.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        Log.i("storage", "tag=" + v.getTag());
                    }
                });
            } catch (Exception e) {
                Log.e("storage", "get view failed");
            }
        }


        return convertView;
    }
}
