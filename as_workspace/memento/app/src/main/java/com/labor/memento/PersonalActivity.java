package com.labor.memento;

import java.io.FileOutputStream;
import java.util.ArrayList;
import java.util.List;

import com.baidu.frontia.Frontia;
import com.baidu.frontia.FrontiaAccount;
import com.baidu.frontia.FrontiaUser;
import com.baidu.frontia.api.FrontiaAuthorization;
import com.baidu.frontia.api.FrontiaPersonalStorage;
import com.baidu.frontia.api.FrontiaAuthorization.MediaType;
import com.baidu.frontia.api.FrontiaAuthorizationListener.AuthorizationListener;
import com.baidu.frontia.api.FrontiaPersonalStorageListener.FileInfoResult;
import com.baidu.frontia.api.FrontiaPersonalStorageListener.FileListListener;
import com.baidu.frontia.api.FrontiaPersonalStorageListener.FileOperationListener;
import com.baidu.frontia.api.FrontiaPersonalStorageListener.FileProgressListener;
import com.baidu.frontia.api.FrontiaPersonalStorageListener.FileTransferListener;
import com.baidu.frontia.api.FrontiaPersonalStorageListener.FileUploadListener;
import com.baidu.frontia.api.FrontiaPersonalStorageListener.QuotaListener;
import com.baidu.frontia.api.FrontiaPersonalStorageListener.QuotaResult;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.widget.TextView;

public class PersonalActivity extends Activity {

	private FrontiaPersonalStorage mCloudStorage;
	private FrontiaAuthorization authorization;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		authority();
	}
	
	// 验证账户
	void authority()
	{
		ArrayList<String> list = new ArrayList<String>();
		list.add("basic");
		list.add("netdisk");
		
		mCloudStorage = Frontia.getPersonalStorage();
		authorization = Frontia.getAuthorization();
		FrontiaAccount currentAccount = Frontia.getCurrentAccount();
		// 如果登陆过，那么不需要用户输入账户密码
		if(null != currentAccount && FrontiaAccount.Type.USER == currentAccount.getType()) {
		    FrontiaUser user = (FrontiaUser) currentAccount;
		    if(!MediaType.BAIDU.toString().equals(user.getPlatform())) {
		        authorization.bindBaiduOAuth(this, list, new AuthorizationListener() {

                    @Override
                    public void onSuccess(FrontiaUser user) {
                        Log.i("storage", "userid=" + user.getId());
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
			// 否则让玩家输入账号密码
		    authorization.authorize(this,MediaType.BAIDU.toString(),list,new AuthorizationListener() {
		        
		        @Override
		        public void onSuccess(FrontiaUser arg0) {
                    Log.i("storage", "userid=" + arg0.getId());
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
	
	// 显示界面
	void setupViews()
	{
		setContentView(R.layout.personal_activity);
		
		// 显示存储空间大小
//		Quota();
		
		// 显示文件列表
//		ListFile();
		
		// 文件操纵，下载，删除，上传
//		DownloadFile();
//		DeleteFile();
		UploadFile();
	}
	
	// 获取玩家的配额信息
	void Quota()
	{
		mCloudStorage.quota(new QuotaListener() {
			
			@Override
			public void onSuccess(QuotaResult arg0) {
				// TODO Auto-generated method stub
				Log.i("storage", "配额信息：" + arg0.getTotal() + ", 已使用：" + arg0.getUsed());
			}
			
			@Override
			public void onFailure(int arg0, String arg1) {
				// TODO Auto-generated method stub

				Log.e("storage", "获取配额失败！");
			}
		});
	}

	// 显示所有文件
	void ListFile()
	{
		mCloudStorage.list(CONF.ROOT_DIR, "name", "asc", new FileListListener() {
			
			@Override
			public void onSuccess(List<FileInfoResult> arg0) {
				// TODO Auto-generated method stub
				Log.i("storage", "获取文件列表成功：" + arg0.size());
				
				for(FileInfoResult file : arg0)
				{
					Log.i("storage", "文件：" + file.getPath());
				}
				
			}
			
			@Override
			public void onFailure(int arg0, String arg1) {
				// TODO Auto-generated method stub
				Log.e("storage", "获取文件列表错误：" + arg0 + ", msg=" + arg1);
			}
		});
		
	}
	
	// 下载文件
	void DownloadFile()
	{
		// 必须的得保证目标文件路径存在，否则会失败！
		mCloudStorage.downloadFile(CONF.DOWNLOAD_FILE, CONF.UPLOAD_FILE, new FileProgressListener() {
			
			@Override
			public void onProgress(String arg0, long arg1, long arg2) {
				// TODO Auto-generated method stub
				Log.i("storage", "下载进度， 文件：" + arg0 + ", 已传输：" + arg1 + "，总大小：" + arg2);
			}
		},new FileTransferListener() {
			
			@Override
			public void onSuccess(String arg0, String arg1) {
				// TODO Auto-generated method stub
				Log.i("storage", "下载成功：" + arg0 + "，下载后的文件名为：" + arg1);
			}
			
			@Override
			public void onFailure(String arg0, int arg1, String arg2) {
				// TODO Auto-generated method stub
				Log.e("storage", "下载失败：" + arg0 + "，错误码：" + arg1 + "， 文件名：" + arg2);
			}
		});
		
	}
	
	// 删除文件
	void DeleteFile()
	{
		mCloudStorage.deleteFile(CONF.DOWNLOAD_FILE, new FileOperationListener() {
			
			@Override
			public void onSuccess(String arg0) {
				// TODO Auto-generated method stub
				Log.i("storage", "删除文件成功：" + arg0);
			}
			
			@Override
			public void onFailure(String arg0, int arg1, String arg2) {
				// TODO Auto-generated method stub
				Log.e("storage", "删除文件失败：" + arg0 + "，错误码：" + arg1 + ", " + arg2);
			}
		});
	}
	
	// 修改文件内容，并上传
	void UploadFile()
	{
		// 读取文件，然后修改内容再上传
		try
		{
			FileOutputStream file = new FileOutputStream(CONF.UPLOAD_FILE, true);
			StringBuffer sb = new StringBuffer();
			sb.append("修改过后的！");
			file.write(sb.toString().getBytes());	
			file.close();
		}
		catch(Exception e)
		{}
		
		mCloudStorage.uploadFile(CONF.UPLOAD_FILE, CONF.DOWNLOAD_FILE, new FileProgressListener() {
			
			@Override
			public void onProgress(String arg0, long arg1, long arg2) {
				// TODO Auto-generated method stub
				Log.i("storage", "上传进度：" + arg0 + "，已上传：" + arg1 + "，总大小：" + arg2);
			}
		}, new FileUploadListener() {
			
			@Override
			public void onSuccess(String arg0, FileInfoResult arg1) {
				// TODO Auto-generated method stub
				Log.i("storage", "上传完成：" +arg0 + ", 文件路径：" + arg1.getPath());
			}
			
			@Override
			public void onFailure(String arg0, int arg1, String arg2) {
				// TODO Auto-generated method stub
				Log.e("storage", "上传失败：" + arg0 + ", 错误码：" + arg1 + ", " + arg2);
			}
		});
	}
}
