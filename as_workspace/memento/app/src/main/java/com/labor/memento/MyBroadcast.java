package com.labor.memento;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

/**
 * Created by harold on 2015/4/5.
 */
public class MyBroadcast extends BroadcastReceiver {

//    MainActivity mMain = null;
    public MyBroadcast(MainActivity ma){
//        mMain = ma;
    }

    @Override
    public void onReceive(Context context, Intent intent) {
        Log.i(CONF.API_TEST, "action=" + intent.getAction() + ", context=" + context.toString());
//        mMain.SetAction(intent.getAction());

        if(intent.getAction() == Intent.ACTION_SCREEN_OFF){
            Intent go = new Intent(context, GFWActivity.class);
            context.startActivity(go);
        }

    }
}
