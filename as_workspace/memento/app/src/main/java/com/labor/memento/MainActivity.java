package com.labor.memento;

import java.io.File;

import android.app.AlertDialog;
import android.app.Dialog;
import android.app.KeyguardManager;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.Bundle;
import android.os.Environment;
import android.support.v4.app.Fragment;
import android.support.v7.app.ActionBarActivity;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.Toast;

import com.baidu.frontia.Frontia;
import com.baidu.frontia.FrontiaApplication;

public class MainActivity extends ActionBarActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        if (savedInstanceState == null) {
            getSupportFragmentManager().beginTransaction()
                    .add(R.id.container, new PlaceholderFragment()).commit();
        }


        FrontiaApplication.initFrontiaApplication(getApplicationContext());
        Frontia.init(getApplicationContext(), CONF.API_KEY);

        // ????Щ???
        Log.i("storage", "??????? getDataDirectory??" + Environment.getDataDirectory());
        Log.i("storage", "??????? getDownloadCacheDirectory??" + Environment.getDownloadCacheDirectory());
        Log.i("storage", "??????? getExternalStorageDirectory??" + Environment.getExternalStorageDirectory());
        Log.i("storage", "??????? DIRECTORY_DOWNLOADS??" + Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS));
        Log.i("storage", "??????? getExternalStorageState??" + Environment.getExternalStorageState());

        // ?????Щ????
        File dir = new File(CONF.LOCAL_ROOT_DIR);
        if (!dir.exists()) {
            dir.mkdir();
            Log.i("storage", "??????????" + CONF.LOCAL_ROOT_DIR);
        } else {
            Log.i("storage", "??????????" + CONF.LOCAL_ROOT_DIR);
        }
        Utils.FileUtil.MakeDir(CONF.LOCAL_ROOT_MEMENTO_AUDIO_DIR);

        Utils.Init();


        IntentFilter filter = new IntentFilter();
        filter.addAction(Intent.ACTION_SCREEN_ON);
        filter.addAction(Intent.ACTION_SCREEN_OFF);
        registerReceiver(new MyBroadcast(null), filter);


        Intent go = new Intent(this, GFWActivity.class);
        this.startActivity(go);
    }

    public static class FileUtil {
        public static void MakeDir(String fullpath) {
            File dir = new File(fullpath);
            if (!dir.exists()) {
                dir.mkdirs();
            }
        }

        public static void CopyFile(String src, String dst) {

        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {

        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();
        if (id == R.id.action_settings) {

            // ???б?
            Intent intent = new Intent(getApplicationContext(),
                    RecordListActivity.class);
            startActivity(intent);

            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    /**
     * A placeholder fragment containing a simple view.
     */
    public static class PlaceholderFragment extends Fragment {

        public PlaceholderFragment() {
        }

        View RootView = null;

        @Override
        public View onCreateView(LayoutInflater inflater, ViewGroup container,
                                 Bundle savedInstanceState) {
            final View rootView = inflater.inflate(R.layout.fragment_main, container,
                    false);
            RootView = rootView;

            Button btn1 = (Button) rootView.findViewById(R.id.button1);
            btn1.setOnClickListener(new OnClickListener() {

                @Override
                public void onClick(View v) {

                    Log.e("123", "123");

                    Intent intent = new Intent(PlaceholderFragment.this.getActivity().getApplicationContext(),
                            PersonalActivity.class);
                    startActivity(intent);
                }
            });

            Button btn2 = (Button) RootView.findViewById(R.id.button2);
            btn2.setOnClickListener(new OnClickListener() {

                @Override
                public void onClick(View arg0) {
                    // TODO Auto-generated method stub

                    Intent intent = new Intent(PlaceholderFragment.this.getActivity().getApplicationContext(),
                            RecordActivity.class);
                    startActivity(intent);
                }
            });

            Button btn3 = (Button) RootView.findViewById(R.id.main_btn_list);
            btn3.setOnClickListener(new OnClickListener() {
                @Override
                public void onClick(View v) {
                    if (false) {
                        // 小测试
                        CharSequence cs = "123";
                        Toast.makeText(PlaceholderFragment.this.getActivity().getApplicationContext(),
                                cs, Toast.LENGTH_SHORT).show();
                    } else if (false) {
                        // 小测试
                        Dialog dialog = new Dialog(PlaceholderFragment.this.getActivity());
                        dialog.setContentView(R.layout.activity_record_info);
                        dialog.setTitle("标题名");
                        dialog.show();
                    } else if (false) {
                        // 小测试
                        Context currentContext = PlaceholderFragment.this.getActivity();
                        LayoutInflater li = (LayoutInflater)currentContext.getSystemService(LAYOUT_INFLATER_SERVICE);
                        View layout = li.inflate(R.layout.activity_record_info, null);
                        AlertDialog.Builder builder = new AlertDialog.Builder(currentContext);
                        builder.setView(layout);
                        builder.create().show();
                    } else if (false) {
//                        Intent intent = new Intent(PlaceholderFragment.this.getActivity().getApplicationContext(),
//                                RecordInfoActivity.class);
//                        startActivity(intent);
                    } else if(false){
                        Intent intent = new Intent(PlaceholderFragment.this.getActivity().getApplicationContext(),
                                RecordContentActivity.class);
                        startActivity(intent);
                    } else {
                        // 打开录音列表
                        Intent intent = new Intent(PlaceholderFragment.this.getActivity().getApplicationContext(),
                                RecordListActivity.class);
                        startActivity(intent);
                    }
                }
            });


            return rootView;
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        Log.i(CONF.API_TEST, "onResume");
    }

    @Override
    protected void onRestart() {
        super.onRestart();
        Log.i(CONF.API_TEST, "onRestart");
    }

    @Override
    protected void onStart() {
        super.onStart();
        Log.i(CONF.API_TEST, "onStart"+ IsLocked());
    }

    @Override
    protected void onStop() {
        super.onStop();
        Log.i(CONF.API_TEST, "onStop"+ IsLocked());
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        Log.i(CONF.API_TEST, "onDestroy"+ IsLocked());
    }

    @Override
    protected void onPause() {
        super.onPause();
        Log.i(CONF.API_TEST, "onPause"+ IsLocked() + ", context=" + this.toString());
    }

    @Override
    public void onLowMemory() {
        super.onLowMemory();
        Log.i(CONF.API_TEST, "onLowMemory" + IsLocked());
    }
    public String IsLocked(){
        KeyguardManager km = (KeyguardManager)this.getApplicationContext().getSystemService(Context.KEYGUARD_SERVICE);
        if(km != null && km.inKeyguardRestrictedInputMode()){
//            return "lock!";
        }
//        return "unlock!";
        return curAction;
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);


        Log.i(CONF.API_TEST, "onActivityResult");
    }

    @Override
    protected void onUserLeaveHint() {
        super.onUserLeaveHint();
        Log.i(CONF.API_TEST, "onUserLeaveHint");
    }

    String curAction = "";
    public void SetAction(String ac){
        curAction = ac;
    }


}
