package com.labor.memento;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.TextView;

import java.text.DateFormat;
import java.util.ArrayList;
import java.util.Date;

/**
 * Created by harold on 2015/3/28.
 */
public class ListRecordContentAdapter extends BaseAdapter {

    Context mContext = null;

    ArrayList<String> list = new ArrayList<String>();
    RecordInfo mRI = null;

    public ListRecordContentAdapter(Context context, RecordInfo ri){
        mContext = context;
        mRI = ri;
        list.add(ri.fileName);
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
        return (position);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        if(convertView == null)
        {
            try {
                convertView = LayoutInflater.from(mContext).inflate(R.layout.list_record_content, parent, false);

                TextView time = (TextView)convertView.findViewById(R.id.list_rec_co_time);
                time.setText(DateFormat.getDateInstance().format(new Date(mRI.date)));

                Button btn = (Button) convertView.findViewById(R.id.list_rec_co_btn_play);
                btn.setTag("" + list.get(position));
                btn.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        Log.i("storage", "tag=" + v.getTag());
                        MyMediaPlayer.Singleton(v.getContext()).PlaySound(true, CONF.LOCAL_ROOT_MEMENTO_AUDIO_DIR + "/" + v.getTag());
                    }
                });

                btn = (Button) convertView.findViewById(R.id.list_rec_co_btn_playloud);
                btn.setTag("" + list.get(position));
                btn.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        Log.i("storage", "tag=" + v.getTag());
                        MyMediaPlayer.Singleton(v.getContext()).PlaySound(false, CONF.LOCAL_ROOT_MEMENTO_AUDIO_DIR + "/" + v.getTag());
                    }
                });

            } catch (Exception e) {
                Log.e("storage", "get view failed");
            }
        }


        return convertView;
    }
}
