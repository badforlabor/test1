package com.labor.memento;

import android.app.ActivityManager;
import android.app.Application;
import android.content.Context;
import android.media.AudioManager;
import android.media.MediaPlayer;
import android.view.View;

import java.io.IOException;

/**
 * Created by harold on 2015/3/29.
 */
public class MyMediaPlayer {
    private MyMediaPlayer(){}
    private final static MyMediaPlayer myMediaPlayer = new MyMediaPlayer();
    public static MyMediaPlayer Singleton(Context context) {
        myMediaPlayer.AM = (AudioManager)context.getApplicationContext().getSystemService(Context.AUDIO_SERVICE);
        return myMediaPlayer;
    }

    private AudioManager AM = null;
    private MediaPlayer player = null;

    public void PlaySound(String fullname){
        PlaySoundImpl(false, fullname);
    }
    public void PlaySound(boolean incall, String fullname){
        PlaySoundImpl(incall, fullname);
    }
    public void StopPlay(){

        if(player != null){
            try {
                player.stop();
            } catch (Exception e) {
                e.printStackTrace();
            }
            player = null;
            OnStopAudio();
        }
    }
    public boolean IsPlaying(){
        return player != null;
    }
    private void PlaySoundImpl(boolean incall, String fullname){
        if (player == null) {
            // 设置使用听筒播放声音
            if(incall){

                AudioManager am = AM;
                am.setMode(AudioManager.MODE_IN_CALL);
            }

            player = new MediaPlayer();
            try {
                player.setOnCompletionListener(new MediaPlayer.OnCompletionListener() {

                    @Override
                    public void onCompletion(MediaPlayer arg0) {
                        // TODO Auto-generated method stub
                        player = null;
                        OnStopAudio();
                    }
                });
                player.setDataSource(fullname);
                player.prepare();
                player.start();
            } catch (IllegalArgumentException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            } catch (SecurityException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            } catch (IllegalStateException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            } catch (IOException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }

        }
        else{
            StopPlay();
        }
    }
    void OnStopAudio(){
        AudioManager am = AM;
        am.setMode(AudioManager.MODE_NORMAL);
    }

}
