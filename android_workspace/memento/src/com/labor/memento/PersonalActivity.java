package com.labor.memento;

import java.util.ArrayList;

import com.baidu.frontia.Frontia;
import com.baidu.frontia.FrontiaAccount;
import com.baidu.frontia.FrontiaUser;
import com.baidu.frontia.api.FrontiaAuthorization;
import com.baidu.frontia.api.FrontiaPersonalStorage;
import com.baidu.frontia.api.FrontiaAuthorization.MediaType;
import com.baidu.frontia.api.FrontiaAuthorizationListener.AuthorizationListener;

import android.app.Activity;
import android.os.Bundle;
import android.widget.TextView;

public class PersonalActivity extends Activity {

	private FrontiaPersonalStorage mCloudStorage;
	private FrontiaAuthorization authorization;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		
		ArrayList<String> list = new ArrayList<String>();
		list.add("basic");
		list.add("netdisk");
		
		mCloudStorage = Frontia.getPersonalStorage();
		authorization = Frontia.getAuthorization();
		FrontiaAccount currentAccount = Frontia.getCurrentAccount();
		if(null != currentAccount && FrontiaAccount.Type.USER == currentAccount.getType()) {
		    FrontiaUser user = (FrontiaUser) currentAccount;
		    if(!MediaType.BAIDU.toString().equals(user.getPlatform())) {
		        authorization.bindBaiduOAuth(this, list, new AuthorizationListener() {

                    @Override
                    public void onSuccess(FrontiaUser user) {
                        setupViews();
                    }

                    @Override
                    public void onFailure(int errCode, String errMsg) {
                        TextView view = new TextView(PersonalActivity.this);
                        view.setText("failure");
                        setContentView(view);
                    }

                    @Override
                    public void onCancel() {
                        TextView view = new TextView(PersonalActivity.this);
                        view.setText("player canceled");
                        setContentView(view);
                    }
		            
		        });
		    } else {
		        setupViews();
		    }
		    
		} else {
		    authorization.authorize(this,MediaType.BAIDU.toString(),list,new AuthorizationListener() {
		        
		        @Override
		        public void onSuccess(FrontiaUser arg0) {
		            Frontia.setCurrentAccount(arg0);
		            setupViews();
		        }
		        
		        @Override
		        public void onFailure(int arg0, String arg1) {
		            TextView view = new TextView(PersonalActivity.this);
		            view.setText("failure 2");
		            setContentView(view);
		        }
		        
		        @Override
		        public void onCancel() {
		            TextView view = new TextView(PersonalActivity.this);
		            view.setText("cancel 2");
		            setContentView(view);
		        }
		    });
		}
	}
	void setupViews()
	{
		setContentView(R.layout.personal_activity);
	}
}
