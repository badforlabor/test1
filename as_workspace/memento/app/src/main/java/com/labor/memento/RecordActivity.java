/**
 * 录音界面，
 */
package com.labor.memento;

import java.io.IOException;

import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.content.Intent;
import android.media.AudioManager;
import android.media.MediaPlayer;
import android.media.MediaPlayer.OnCompletionListener;
import android.media.MediaRecorder;
import android.os.Bundle;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.Spinner;
import android.widget.TextView;

/**
 * @author harold
 */
public class RecordActivity extends Activity {

    // 录音api
    private MediaRecorder recorder = null;
    private MediaPlayer player = null;
    TextView label_tip = null;

    String parent = "";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.record_activity);

        parent = getIntent().getStringExtra("parent");

        label_tip = (TextView)this.findViewById(R.id.rec_label_tip);
        Button btnTest = (Button) this.findViewById(R.id.rec_testbtn);
        btnTest.setOnTouchListener(new View.OnTouchListener() {
            float y = 0;
            @Override
            public boolean onTouch(View v, MotionEvent event) {

                if (event.getAction() != MotionEvent.ACTION_MOVE) {
                    Log.i("api-test", "event=" + event.getAction() + ", x=" + event.getRawX() + ", y=" + event.getRawY());
                }

                if(event.getAction() == MotionEvent.ACTION_DOWN){
                    // 开始录音
                    StartRecord();
                    y = event.getRawY();
                }
                else if(event.getAction() == MotionEvent.ACTION_UP){
                    // 停止录音
                    StopRecord((event.getRawY() - y < - 100));
                    y = 0;
                }
                else if(event.getAction() == MotionEvent.ACTION_MOVE){
                    if(event.getRawY() - y < -100){
                        label_tip.setText("松开之后即可取消录音");
                    }
                }

                return false;
            }
        });


        // 开始录音
        Button btn1 = (Button) this.findViewById(R.id.rec_start);
        btn1.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View arg0) {
                // TODO Auto-generated method stub
                StartRecord();
            }
        });

        // 停止录音
        Button btn2 = (Button) this.findViewById(R.id.rec_stop);
        btn2.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View arg0) {
                // TODO Auto-generated method stub
                StopRecord();
            }
        });

        // 播放
        Button btn3 = (Button) this.findViewById(R.id.rec_play);
        btn3.setOnClickListener(new OnClickListener() {

            @Override
            public void onClick(View arg0) {
                // TODO Auto-generated method stub
                PlayRecord();
            }
        });
    }

    @Override
    protected void onStop() {
        super.onStop();

        Log.i("api-test", "activity stop!");
    }

    @Override
    protected void onPause() {
        super.onPause();

        Log.i("api-test", "activity pause!");

        // 停止录音
        StopRecord();
    }

    void StopRecord(){
        StopRecord(true);
    }
    void StopRecord(boolean notsaved){
        if (recorder != null) {
            label_tip.setText("结束录音");
            recorder.stop();
            recorder.release();
            recorder = null;

            // 将文件拷贝到日志目录
            if(!notsaved){
                PendingSaveRecord();
            }
        }
    }

    void StartRecord(){

        if (recorder == null) {
            label_tip.setText("开始录音");
            recorder = new MediaRecorder();
            recorder.setAudioSource(MediaRecorder.AudioSource.MIC);
            recorder.setOutputFormat(MediaRecorder.OutputFormat.THREE_GPP);
            recorder.setOutputFile(CONF.REC_FILE);
            recorder.setAudioEncoder(MediaRecorder.AudioEncoder.AMR_NB);
            recorder.setMaxFileSize(1000 * 1000 * 4);   // 1M
            recorder.setMaxDuration(10 * 60 * 1000);    // 10分钟
            recorder.setOnInfoListener(new MediaRecorder.OnInfoListener() {
                @Override
                public void onInfo(MediaRecorder mr, int what, int extra) {
                    Log.i("api-test", "recorder-what=" + what + ", amp=" + mr.getMaxAmplitude());
                }
            });
            try {
                recorder.prepare();
            } catch (IllegalStateException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            } catch (IOException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
            recorder.start();
        }
    }

    void PlayRecord(){
        PlayRecord(true, CONF.REC_FILE);
    }
    // 播放开关（点击一下播放，再点击一下停止播放）
    // incall:使用听筒播放声音；fullname：路径全名
    protected  void PlayRecord(boolean incall, String fullname){

        if(!MyMediaPlayer.Singleton(this).IsPlaying()){
            label_tip.setText("播放录音");
            MyMediaPlayer.Singleton(this).PlaySound(incall, fullname);
        }
        else{
            label_tip.setText("结束播放！");
            MyMediaPlayer.Singleton(this).StopPlay();
        }
    }

    void PendingSaveRecord(){
        final Dialog dialog = new Dialog(this);
        dialog.setContentView(R.layout.activity_record_info);
        dialog.setTitle("录音信息");

        final Spinner spinner = (Spinner)dialog.findViewById(R.id.ri_spinner01);
        String[] str = CONF.CATEGORY_RECORD;
        ArrayAdapter aa = new ArrayAdapter(dialog.getContext(), android.R.layout.simple_spinner_item, str);
        spinner.setAdapter(aa);

        Button ok = (Button)dialog.findViewById(R.id.ri_ok);
        ok.setOnClickListener(new OnClickListener() {
            Dialog tmp = dialog;

            @Override
            public void onClick(View v) {
                RecordInfo ri = new RecordInfo("test");
                ri.SaveTmpRecordFile();
                tmp.dismiss();

                if(parent == ""){
                    // 返回到主界面
                    Intent intent = new Intent(v.getContext(), MainActivity.class);
                    // 清除页面堆栈，这样返回主界面之后再次点击返回就不会再次回到这个界面了
                    intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                    startActivity(intent);
                }
                else{
                    // 返回录音内容界面
                    Intent intent = new Intent(v.getContext(), RecordContentActivity.class);
                    intent.putExtra("tag", ri.id);
                    // 清除页面堆栈，这样返回主界面之后再次点击返回就不会再次回到这个界面了
                    intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                    startActivity(intent);
                }
            }
        });

        dialog.show();
    }
    @Override
    protected void onDestroy() {
        super.onDestroy();
        MyMediaPlayer.Singleton(this).StopPlay();
    }
}
