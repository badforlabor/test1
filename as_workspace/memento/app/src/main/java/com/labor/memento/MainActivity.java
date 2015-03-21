package com.labor.memento;

import java.io.File;

import android.content.Intent;
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
		Log.i("storage", "??????? getDataDirectory??"+Environment.getDataDirectory());
		Log.i("storage", "??????? getDownloadCacheDirectory??"+Environment.getDownloadCacheDirectory());
		Log.i("storage", "??????? getExternalStorageDirectory??"+Environment.getExternalStorageDirectory());
		Log.i("storage", "??????? DIRECTORY_DOWNLOADS??"+Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS));
		Log.i("storage", "??????? getExternalStorageState??"+Environment.getExternalStorageState());
	
		// ?????Щ????
		File dir = new File(CONF.LOCAL_ROOT_DIR);
		if(!dir.exists())
		{			
			dir.mkdir();
			Log.i("storage", "??????????" + CONF.LOCAL_ROOT_DIR);
		}
		else
		{			
			Log.i("storage", "??????????" + CONF.LOCAL_ROOT_DIR);
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
						RecordList.class);
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
			View rootView = inflater.inflate(R.layout.fragment_main, container,
					false);
			RootView = rootView;
			
			Button btn1 = (Button)rootView.findViewById(R.id.button1);
			btn1.setOnClickListener(new OnClickListener() {
				
				@Override
				public void onClick(View v) {
					
					Log.e("123", "123");
					
					Intent intent = new Intent(PlaceholderFragment.this.getActivity().getApplicationContext(), 
								PersonalActivity.class);
					startActivity(intent);
				}
			});
			
			Button btn2 = (Button)RootView.findViewById(R.id.button2);
			btn2.setOnClickListener(new OnClickListener() {
				
				@Override
				public void onClick(View arg0) {
					// TODO Auto-generated method stub

					Intent intent = new Intent(PlaceholderFragment.this.getActivity().getApplicationContext(), 
								RecordActivity.class);
					startActivity(intent);
				}
			});
			
			return rootView;
		}
	}

}
