package com.baidu.frontia.demo;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;

import com.baidu.frontia.Frontia;
import com.baidu.frontia.demo.acl.ACLActivity;
import com.baidu.frontia.demo.lbs.LBSActivity;
//import com.baidu.frontia.demo.push.PushActivity;
import com.baidu.frontia.demo.role.AccountActivity;
import com.baidu.frontia.demo.share.ShareActivity;
import com.baidu.frontia.demo.social.SocialActivity;
import com.baidu.frontia.demo.statistics.DemoActivity1;
import com.baidu.frontia.demo.storage.AppDataActivity;
import com.baidu.frontia.demo.storage.AppFileActivity;
import com.baidu.frontia.demo.storage.PersonalFileActivity;

public class FrontiaActivity extends Activity {

	private Button authBtn;
	private Button appDataBtn;
	private Button appFileBtn;
	private Button personalFileBtn;
	private Button pushBtn;
	private Button statBtn;
	private Button accountBtn;
	private Button aclBtn;
	private Button lbsBtn;
	private Button shareBtn;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		setContentView(R.layout.activity_main);
		Frontia.init(this.getApplicationContext(), Conf.APIKEY);
		
		//just for silent update function check
		//FrontiaImpl.get().setSlientUpdateDebugDomain("http://10.26.165.13:8909/build/packet");
        setupViews();
	}

	private void setupViews() {
		authBtn = (Button)findViewById(R.id.authBtn);
		appDataBtn = (Button)findViewById(R.id.appDataBtn);
		appFileBtn = (Button)findViewById(R.id.appFileBtn);
		personalFileBtn = (Button)findViewById(R.id.personalFileBtn);
		pushBtn = (Button)findViewById(R.id.pushBtn);
		statBtn = (Button)findViewById(R.id.statBtn);
		accountBtn = (Button)findViewById(R.id.accountBtn);
		aclBtn = (Button)findViewById(R.id.acl);
		lbsBtn = (Button)findViewById(R.id.lbsBtn);
		shareBtn = (Button)findViewById(R.id.shareBtn);
		
		authBtn.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				Intent intent = new Intent(FrontiaActivity.this,SocialActivity.class);
				startActivity(intent);
			}
		});
		appDataBtn.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				Intent intent = new Intent(FrontiaActivity.this,AppDataActivity.class);
				startActivity(intent);
			}
		});
		appFileBtn.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				Intent intent = new Intent(FrontiaActivity.this,AppFileActivity.class);
				startActivity(intent);
			}
		});
		personalFileBtn.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				Intent intent = new Intent(FrontiaActivity.this,PersonalFileActivity.class);
				startActivity(intent);
			}
		});
		pushBtn.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
//				Intent intent = new Intent(FrontiaActivity.this,PushActivity.class);
//				startActivity(intent);
			}
		});
		statBtn.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				Intent intent = new Intent(FrontiaActivity.this,DemoActivity1.class);
				startActivity(intent);
			}
		});
		accountBtn.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				Intent intent = new Intent(FrontiaActivity.this,AccountActivity.class);
				startActivity(intent);
			}
		});
		aclBtn.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				Intent intent = new Intent(FrontiaActivity.this,ACLActivity.class);
				startActivity(intent);
			}
		});
		lbsBtn.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				Intent intent = new Intent(FrontiaActivity.this,LBSActivity.class);
				startActivity(intent);
			}
		});

		shareBtn.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				Intent intent = new Intent(FrontiaActivity.this,ShareActivity.class);
				startActivity(intent);
			}
		});
	}

}
