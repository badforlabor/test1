package com.labor.memento;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
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

        long id = this.getIntent().getLongExtra("tag", 0);
        RecordInfo ri = RecordInfo.GetRecord(id);
        TextView txt = (TextView)findViewById(R.id.rec_co_type);
        txt.setText("tag=" + ri.tag);

        ListView list = (ListView)findViewById(R.id.rec_co_list);
        list.setAdapter(new ListRecordContentAdapter(this, ri));

        Log.i("storage", "intent-tag=" + getIntent().getLongExtra("tag", 0));
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        MyMediaPlayer.Singleton(this).StopPlay();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {

        CharSequence str = ("RECORD");
        menu.add(0, 0, 0, str);

        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch(item.getItemId()){
            case 0:
            {
                Intent intent = new Intent(this.getApplicationContext(),
                        RecordActivity.class);
                intent.putExtra("parent", this.getClass().getName());
                startActivity(intent);
            }
                break;
        }

        return super.onOptionsItemSelected(item);
    }
}
