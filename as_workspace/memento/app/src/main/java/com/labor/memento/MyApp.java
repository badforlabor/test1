package com.labor.memento;

import android.app.Application;
import android.content.Intent;
import android.content.IntentFilter;
import android.util.Log;

/**
 * Created by harold on 2015/4/5.
 */
public class MyApp extends Application {

    @Override
    public void onCreate() {
        super.onCreate();
        Log.i(CONF.API_TEST, "onCreate app");
    }

    @Override
    public void onLowMemory() {
        super.onLowMemory();
        Log.i(CONF.API_TEST, "onLowMemory");
    }

    @Override
    public void onTerminate() {
        super.onTerminate();
        Log.i(CONF.API_TEST, "onTerminate");
    }
}
