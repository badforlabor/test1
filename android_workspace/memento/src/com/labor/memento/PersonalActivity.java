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
	
	// ��֤�˻�
	void authority()
	{
		ArrayList<String> list = new ArrayList<String>();
		list.add("basic");
		list.add("netdisk");
		
		mCloudStorage = Frontia.getPersonalStorage();
		authorization = Frontia.getAuthorization();
		FrontiaAccount currentAccount = Frontia.getCurrentAccount();
		// �����½������ô����Ҫ�û������˻�����
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
			// ��������������˺�����
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
	
	// ��ʾ����
	void setupViews()
	{
		setContentView(R.layout.personal_activity);
		
		// ��ʾ�洢�ռ��С
//		Quota();
		
		// ��ʾ�ļ��б�
//		ListFile();
		
		// �ļ����ݣ����أ�ɾ�����ϴ�
//		DownloadFile();
//		DeleteFile();
		UploadFile();
	}
	
	// ��ȡ��ҵ������Ϣ
	void Quota()
	{
		mCloudStorage.quota(new QuotaListener() {
			
			@Override
			public void onSuccess(QuotaResult arg0) {
				// TODO Auto-generated method stub
				Log.i("storage", "�����Ϣ��" + arg0.getTotal() + ", ��ʹ�ã�" + arg0.getUsed());
			}
			
			@Override
			public void onFailure(int arg0, String arg1) {
				// TODO Auto-generated method stub

				Log.e("storage", "��ȡ���ʧ�ܣ�");
			}
		});
	}

	// ��ʾ�����ļ�
	void ListFile()
	{
		mCloudStorage.list(CONF.ROOT_DIR, "name", "asc", new FileListListener() {
			
			@Override
			public void onSuccess(List<FileInfoResult> arg0) {
				// TODO Auto-generated method stub
				Log.i("storage", "��ȡ�ļ��б�ɹ���" + arg0.size());
				
				for(FileInfoResult file : arg0)
				{
					Log.i("storage", "�ļ���" + file.getPath());
				}
				
			}
			
			@Override
			public void onFailure(int arg0, String arg1) {
				// TODO Auto-generated method stub
				Log.e("storage", "��ȡ�ļ��б����" + arg0 + ", msg=" + arg1);
			}
		});
		
	}
	
	// �����ļ�
	void DownloadFile()
	{
		// ����ĵñ�֤Ŀ���ļ�·�����ڣ������ʧ�ܣ�
		mCloudStorage.downloadFile(CONF.DOWNLOAD_FILE, CONF.UPLOAD_FILE, new FileProgressListener() {
			
			@Override
			public void onProgress(String arg0, long arg1, long arg2) {
				// TODO Auto-generated method stub
				Log.i("storage", "���ؽ��ȣ� �ļ���" + arg0 + ", �Ѵ��䣺" + arg1 + "���ܴ�С��" + arg2);
			}
		},new FileTransferListener() {
			
			@Override
			public void onSuccess(String arg0, String arg1) {
				// TODO Auto-generated method stub
				Log.i("storage", "���سɹ���" + arg0 + "�����غ���ļ���Ϊ��" + arg1);
			}
			
			@Override
			public void onFailure(String arg0, int arg1, String arg2) {
				// TODO Auto-generated method stub
				Log.e("storage", "����ʧ�ܣ�" + arg0 + "�������룺" + arg1 + "�� �ļ�����" + arg2);
			}
		});
		
	}
	
	// ɾ���ļ�
	void DeleteFile()
	{
		mCloudStorage.deleteFile(CONF.DOWNLOAD_FILE, new FileOperationListener() {
			
			@Override
			public void onSuccess(String arg0) {
				// TODO Auto-generated method stub
				Log.i("storage", "ɾ���ļ��ɹ���" + arg0);
			}
			
			@Override
			public void onFailure(String arg0, int arg1, String arg2) {
				// TODO Auto-generated method stub
				Log.e("storage", "ɾ���ļ�ʧ�ܣ�" + arg0 + "�������룺" + arg1 + ", " + arg2);
			}
		});
	}
	
	// �޸��ļ����ݣ����ϴ�
	void UploadFile()
	{
		// ��ȡ�ļ���Ȼ���޸��������ϴ�
		try
		{
			FileOutputStream file = new FileOutputStream(CONF.UPLOAD_FILE, true);
			StringBuffer sb = new StringBuffer();
			sb.append("�޸Ĺ���ģ�");
			file.write(sb.toString().getBytes());	
			file.close();
		}
		catch(Exception e)
		{}
		
		mCloudStorage.uploadFile(CONF.UPLOAD_FILE, CONF.DOWNLOAD_FILE, new FileProgressListener() {
			
			@Override
			public void onProgress(String arg0, long arg1, long arg2) {
				// TODO Auto-generated method stub
				Log.i("storage", "�ϴ����ȣ�" + arg0 + "�����ϴ���" + arg1 + "���ܴ�С��" + arg2);
			}
		}, new FileUploadListener() {
			
			@Override
			public void onSuccess(String arg0, FileInfoResult arg1) {
				// TODO Auto-generated method stub
				Log.i("storage", "�ϴ���ɣ�" +arg0 + ", �ļ�·����" + arg1.getPath());
			}
			
			@Override
			public void onFailure(String arg0, int arg1, String arg2) {
				// TODO Auto-generated method stub
				Log.e("storage", "�ϴ�ʧ�ܣ�" + arg0 + ", �����룺" + arg1 + ", " + arg2);
			}
		});
	}
}
