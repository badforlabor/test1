/**
 * 录音界面，
 */
package com.labor.memento;

import java.io.IOException;

import android.app.Activity;
import android.media.MediaPlayer;
import android.media.MediaPlayer.OnCompletionListener;
import android.media.MediaRecorder;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;

/**
 * @author harold
 *
 */
public class RecordActivity extends Activity {
	
	// 录音api
	private MediaRecorder recorder = null;
	private MediaPlayer player = null;
	
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		
		setContentView(R.layout.record_activity);
		
		// 开始录音
		Button btn1 = (Button)this.findViewById(R.id.rec_start);
		btn1.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
				if(recorder == null)
				{
					recorder = new MediaRecorder();
					recorder.setAudioSource(MediaRecorder.AudioSource.MIC);
					recorder.setOutputFormat(MediaRecorder.OutputFormat.THREE_GPP);
					recorder.setOutputFile(CONF.REC_FILE);
					recorder.setAudioEncoder(MediaRecorder.AudioEncoder.AMR_NB);
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
		});
		
		// 停止录音
		Button btn2 = (Button)this.findViewById(R.id.rec_stop);
		btn2.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
				if(recorder != null)
				{
					recorder.stop();	
					recorder = null;
				}
			}
		});
		
		// 播放
		Button btn3 = (Button)this.findViewById(R.id.rec_play);
		btn3.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
				if(player == null)
				{
					player = new MediaPlayer();
					try {
						player.setOnCompletionListener(new OnCompletionListener() {
							
							@Override
							public void onCompletion(MediaPlayer arg0) {
								// TODO Auto-generated method stub
								player = null;
							}
						});
						player.setDataSource(CONF.REC_FILE);
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
			}
		});
	}
}
