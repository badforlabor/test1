package com.labor.memento;

import android.app.Activity;
import android.app.ProgressDialog;
import android.os.Bundle;
import android.widget.ArrayAdapter;
import android.widget.Spinner;

/**
 * Created by harold on 2015/3/27.
 */
public class RecordInfoActivity extends Activity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_record_info);

        Spinner spinner = (Spinner)findViewById(R.id.ri_spinner01);
        String[] str = {"1", "2", "3"};
        ArrayAdapter aa = new ArrayAdapter(this, android.R.layout.simple_spinner_item, str);
        spinner.setAdapter(aa);


    }
}
