package com.baidu.frontia.demo.lbs;

import java.util.List;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.TextView;

import com.baidu.frontia.Frontia;
import com.baidu.frontia.FrontiaUser;
import com.baidu.frontia.api.FrontiaAuthorization;
import com.baidu.frontia.api.FrontiaAuthorizationListener;
//import com.baidu.frontia.api.FrontiaLbs;
//import com.baidu.frontia.api.FrontiaLbsListener;
//import com.baidu.frontia.api.FrontiaLbsListener.FrontiaLocation;
//import com.baidu.frontia.api.FrontiaLbsListener.FrontiaNearUser;
//import com.baidu.frontia.api.FrontiaLbsListener.FrontiaPOI;
//import com.baidu.frontia.api.FrontiaLbsListener.GetCurrentLocationListener;
import com.baidu.frontia.demo.R;

public class LBSActivity extends Activity {

//	private static final String TAG = "LBSActivity";
//	private Button location;
//	private Button nearPOI;
//	private Button nearUser;
//	private TextView info;
//	@Override
//	protected void onCreate(Bundle savedInstanceState) {
//		super.onCreate(savedInstanceState);
//		setContentView(R.layout.lbs);
//		location = (Button)findViewById(R.id.location);
//		nearPOI = (Button)findViewById(R.id.nearPOI);
//		nearUser = (Button)findViewById(R.id.nearUser);
//		info = (TextView)findViewById(R.id.info);
//		final FrontiaLbs lbs = Frontia.getLbs();
//		
//		location.setOnClickListener(new OnClickListener() {
//			
//			@Override
//			public void onClick(View v) {
//				lbs.getCurrentLocation(new GetCurrentLocationListener() {
//					
//					@Override
//					public void onFailure(int errCode, String errMsg) {
//						info.setText("fail to get current location: code=" + errCode + " msg=" + errMsg);
//					}
//
//					@Override
//					public void onSuccess(FrontiaLocation loc) {
//						StringBuilder buf = new StringBuilder();
//                                buf.append("country: ").append(loc.getCountry()).append("\n")
//                                .append("district: ").append(loc.getDistrict()).append("\n")
//                                .append("province: ").append(loc.getProvince()).append("\n")
//                                .append("city: ").append(loc.getCity()).append(" code")
//                                .append(loc.getCityCode()).append("\n")
//                                .append("street: ").append(loc.getStreet()).append(" #")
//                                .append(loc.getStreetNumber()).append("\n");
//                        info.setText(buf.toString());
//					}
//				});
//			}
//		});
//		nearPOI.setOnClickListener(new OnClickListener() {
//			
//			@Override
//			public void onClick(View v) {
//				lbs.getNearPOIs(new FrontiaLbsListener.GetNearPOIsListener() {
//                    @Override
//                    public void onFailure(int errCode, String errMsg) {
//                        info.setText("fail to get near pois : code=" + errCode + " msg=" + errMsg);
//                    }
//
//					@Override
//					public void onSuccess(List<FrontiaPOI> poiList) {
//						 StringBuilder buf = new StringBuilder();
//	                        for(FrontiaPOI poi : poiList){
//	                            buf.append(poi.getName()).append("\n");
//	                        }
//	                        info.setText(buf.toString());
//						
//					}
//                });
//				
//			}
//		});
//		nearUser.setOnClickListener(new OnClickListener() {
//			
//			@Override
//			public void onClick(View v) {
//				FrontiaAuthorization auth = Frontia.getAuthorization();
//
//                auth.authorize(LBSActivity.this, FrontiaAuthorization.MediaType.BAIDU.toString(), new FrontiaAuthorizationListener.AuthorizationListener(){
//                    @Override
//                    public void onSuccess(FrontiaUser user) {
//                        Frontia.setCurrentAccount(user);
//
//
//                        lbs.getNearFrontiaUsers(100, 50, new FrontiaLbsListener.GetNearUsersListener() {
//
//                            @Override
//                            public void onFailure(int errCode, String errMsg) {
//                            	info.setText("fail to get near users: code=" + errCode + " msg=" + errMsg);
//                            }
//
//							@Override
//							public void onSuccess(List<FrontiaNearUser> userList) {
//								StringBuilder buf = new StringBuilder();
//                                for (FrontiaNearUser user : userList){
//                                    buf.append("user ").append(user.getAccountId()).append("\n");
//                                }
//                                info.setText(buf.toString());
//							}
//                        });
//                    }
//
//                    @Override
//                    public void onFailure(int errCode, String errMsg) {
//                    	info.setText("fail to get near users: code=" + errCode + " msg=" + errMsg);
//                    }
//
//                    @Override
//                    public void onCancel() {
//                    	info.setText("no auth user to get near users");
//                    }
//                    //
//                });
//				
//			}
//		});
//	}

}
