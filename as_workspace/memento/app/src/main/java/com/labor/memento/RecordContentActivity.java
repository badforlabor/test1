package com.labor.memento;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.widget.ListView;
import android.widget.TextView;

/**
 * Created by harold on 2015/3/28.
 */
public class RecordContentActivity extends Activity{

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_record_content);

        TextView txt = (TextView)findViewById(R.id.rec_co_type);
        txt.setText("test");

        ListView list = (ListView)findViewById(R.id.rec_co_list);
        list.setAdapter(new ListRecordContentAdapter(this));

        Log.i("storage", "intent-tag=" + getIntent().getStringExtra("tag"));
    }
}
