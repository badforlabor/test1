package com.baidu.frontia.demo.acl;

import java.util.List;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.TextView;

import com.baidu.frontia.Frontia;
import com.baidu.frontia.FrontiaACL;
import com.baidu.frontia.FrontiaAccount;
import com.baidu.frontia.FrontiaData;
import com.baidu.frontia.FrontiaFile;
import com.baidu.frontia.FrontiaQuery;
import com.baidu.frontia.FrontiaRole;
import com.baidu.frontia.FrontiaRole.CommonOperationListener;
import com.baidu.frontia.FrontiaUser;
import com.baidu.frontia.api.FrontiaAuthorization;
import com.baidu.frontia.api.FrontiaAuthorization.MediaType;
import com.baidu.frontia.api.FrontiaAuthorizationListener.AuthorizationListener;
import com.baidu.frontia.api.FrontiaStorage;
import com.baidu.frontia.api.FrontiaStorageListener;
import com.baidu.frontia.api.FrontiaStorageListener.DataInfoListener;
import com.baidu.frontia.api.FrontiaStorageListener.DataOperationListener;
import com.baidu.frontia.api.FrontiaStorageListener.FileListListener;
import com.baidu.frontia.api.FrontiaStorageListener.FileOperationListener;
import com.baidu.frontia.api.FrontiaStorageListener.FileProgressListener;
import com.baidu.frontia.api.FrontiaStorageListener.FileTransferListener;
import com.baidu.frontia.demo.Conf;
import com.baidu.frontia.demo.R;

public class ACLActivity extends Activity {
	
	private final static String TAG = "ACL";
	private FrontiaAuthorization authorization;
	private FrontiaStorage storage;

	
	private TextView info;
	private Button uploadFile;
	private Button downloadFile;
	private Button deleteFile;
	private Button listFile;
	private Button insertData;
	private Button updateData;
	private Button deleteData;
	private Button findData;
	private Button createRole;
	private Button findRole;
	private Button delRole;
	
	private FrontiaAccount mUser;
	private FrontiaData[] mData;
	private FrontiaFile[] mFile;
	private FrontiaRole[] mRole;
	private TextView views[];
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		authorization = Frontia.getAuthorization();
		storage = Frontia.getStorage();
		//鍥犱负鍥犱负浣跨敤acl鐨勬椂鍊欓渶瑕佹寚瀹氫竴涓狥rontiaAccount,鎵�互鎴戜滑杩欓噷鐢ㄧ櫨搴﹁处鍙风櫥褰曪紝浜х敓涓�釜account~~
		authorization.authorize(this, MediaType.BAIDU.toString(), new AuthorizationListener() {
			
			@Override
			public void onSuccess(FrontiaUser arg0) {
				mUser = arg0;
				Frontia.setCurrentAccount(arg0);
				setContentView(R.layout.acl);
				setupViews();
				constructData();
			}
			
			@Override
			public void onFailure(int arg0, String arg1) {
				Log.d(TAG,"閿欒鐮佷负"+arg0+",閿欒淇℃伅涓�"+arg1);
			}
			
			@Override
			public void onCancel() {
				Log.d(TAG,"cancel");
			}
		});

	}
	private void setupViews() {
		info = (TextView)findViewById(R.id.info);
 		uploadFile = (Button)findViewById(R.id.uploadFile);
 		downloadFile = (Button)findViewById(R.id.downloadFile);
 		deleteFile = (Button)findViewById(R.id.deleteFile);
 		listFile = (Button)findViewById(R.id.listFile);
		insertData = (Button)findViewById(R.id.insertData);
		updateData = (Button)findViewById(R.id.updateData);
		deleteData = (Button)findViewById(R.id.deleteData);
		findData = (Button)findViewById(R.id.findData);
		createRole = (Button)findViewById(R.id.createRole);
		findRole = (Button)findViewById(R.id.findRole);
		delRole = (Button)findViewById(R.id.delRole);
		
		
		views = new TextView[4];
        int[] viewIds = new int[]{R.id.info1, R.id.info2, R.id.info3, R.id.info4};
        for(int i=0;i<4;i++){
            views[i] = (TextView) findViewById(viewIds[i]);
        }
		
        uploadFile.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				clearViews();
				uploadFile();
			}
		});
        
        downloadFile.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				clearViews();
				downloadFile();
			}
		});
       deleteFile.setOnClickListener(new OnClickListener() {
		
		@Override
		public void onClick(View v) {
			clearViews();
			deleteFile();
		}
       }) ;
       listFile.setOnClickListener(new OnClickListener() {
		
		@Override
		public void onClick(View v) {
			clearViews();
			list();
		}
	});
       insertData.setOnClickListener(new OnClickListener() {
		
		@Override
		public void onClick(View v) {
			clearViews();
			insertData();
		}
	});
       updateData.setOnClickListener(new OnClickListener() {
		
		@Override
		public void onClick(View v) {
			clearViews();
			updateData();
		}
	});
       deleteData.setOnClickListener(new OnClickListener() {
		
		@Override
		public void onClick(View v) {
			clearViews();
			deleteData();
		}
	});
       findData.setOnClickListener(new OnClickListener() {
		
		@Override
		public void onClick(View v) {
			clearViews();
			findData();
		}
	});
       createRole.setOnClickListener(new OnClickListener() {
		
		@Override
		public void onClick(View v) {
			clearViews();
			createRoleWithAcl();
		}
	});
       findRole.setOnClickListener(new OnClickListener() {
		
		@Override
		public void onClick(View v) {
			clearViews();
			findRoleWithAcl();
		}
	});
       delRole.setOnClickListener(new OnClickListener() {
		
		@Override
		public void onClick(View v) {
			clearViews();
			delRoleWithAcl();
		}
	});
       
	}
	
	
	private void clearViews(){
        info.setText("");
        for(int i=0;i<4;i++){
            views[i].setText("");
        }
    }
	
	/*
	 * 鏋勯�鐨勬暟鎹腑锛屽洓涓狥rontiaData锛屽洓涓狥rontiaFile锛屽洓涓狥rontiaRole
	 * 鏉冮檺鍒嗗埆涓哄彲璇诲彲鍐欙紝鍙涓嶅彲鍐欙紝鍙啓涓嶅彲璇伙紝涓嶅彲璇讳笉鍙啓
	 * 濡傛灉涓嶇粰瀵硅薄璁剧疆ACL,鎴栬�璁剧疆浜嗙┖鐨凙CL,鍒欒瀵硅薄鏉冮檺涓哄彲璇诲彲鍐�
	 */
	private void constructData(){
		mData = new FrontiaData[4];
		mFile = new FrontiaFile[4];
		mRole = new FrontiaRole[4];
		
		FrontiaACL rwACL = new FrontiaACL();
		rwACL.setPublicReadable(true);
		rwACL.setPublicWritable(true);
		
		FrontiaACL rACL = new FrontiaACL();
		rACL.setAccountReadable(mUser, true);
		rACL.setAccountWritable(mUser, false);
		
		FrontiaACL wACL = new FrontiaACL();
		wACL.setAccountReadable(mUser, false);
		wACL.setAccountWritable(mUser, true);
		
		FrontiaACL acl = new FrontiaACL();
		acl.setAccountReadable(mUser, false);
		acl.setAccountWritable(mUser, false);
		
		FrontiaData data = new FrontiaData();
		data.put("name", "canRW");
		data.put("age", 20);
		data.setACL(rwACL);
		mData[0] = data;
		
		data = new FrontiaData();
		data.put("name", "canR");
		data.put("age", 30);
		data.setACL(rACL);
		mData[1] = data;
		
		data = new FrontiaData();
		data.put("name", "canW");
		data.put("age", 40);
		data.setACL(wACL);
		mData[2] = data;
		
		data = new FrontiaData();
		data.put("name", "cannotRW");
		data.put("age", 50);
		data.setACL(acl);
		mData[3] = data;
		
		FrontiaFile file = new FrontiaFile();
		file.setACL(rwACL);
		mFile[0] = file;
		
		file = new FrontiaFile();
		file.setACL(rACL);
		mFile[1] = file;
		
		file = new FrontiaFile();
		file.setACL(wACL);
		mFile[2] = file;
		
		file = new FrontiaFile();
		file.setACL(acl);
		mFile[3] = file;
				
		for(int i=0;i<4;i++){
            mFile[i].setNativePath(Conf.LOCAL_FILE_NAME);
            mFile[i].setRemotePath("[" + i + "]"+Conf.APP_STORAGE_FILE_NAME);
        }
		
		FrontiaRole role1 = new FrontiaRole("role_read_notWrite");
		role1.setACL(rACL);
        FrontiaRole role2 = new FrontiaRole("role_notRead_write");
        role2.setACL(wACL);
        FrontiaRole role3 = new FrontiaRole("role_read_write");
        role3.setACL(rwACL);
        FrontiaRole role4 = new FrontiaRole("role_notRead_notWrite");
        role4.setACL(acl);
        mRole[0] = role3;
        mRole[1] = role1;
        mRole[2] = role2;
        mRole[3] = role4;
	}
	
	private void uploadFile(){
		for(int i=0;i<4;i++){
            final int idx = i;

            storage.uploadFile(mFile[i],
                    new FileProgressListener() {
                        @Override
                        public void onProgress(String source, long bytes, long total) {
                            views[idx].setText(source + " upload......:"
                                    + bytes * 100 / total + "%");
                        }
                    },
                    new FileTransferListener() {
                        @Override
                        public void onSuccess(String source, String newTargetName) {

                            mFile[idx].setRemotePath(newTargetName);
                            views[idx].setText(source + " uploaded as "
                                    + newTargetName + " in the cloud.");
                        }

                        @Override
                        public void onFailure(String source, int errCode, String errMsg) {
                            views[idx].setText(source + " errCode:"
                                    + errCode + ", errMsg:" + errMsg);
                        }
                    }
            );
        }
	}
	
	protected void downloadFile() {

		//鍙互鎴愬姛涓嬭浇涓や釜鏂囦欢锛屽洜涓鸿繕鏈夋枃浠舵病鏈夎鐨勬潈闄�

        for(int i=0;i<4;i++){
            final int idx = i;

            storage.downloadFile(mFile[i], new FileProgressListener() {

                        @Override
                        public void onProgress(String source, long bytes, long total) {
                            views[idx].setText(source + " download......:"
                                        + bytes * 100 / total + "%");

                        }

                    }, new FileTransferListener() {

                        @Override
                        public void onSuccess(String source, String newTargetName) {

                            views[idx].setText(source + " downloaded as "
                                        + newTargetName + " in the local.");

                        }

                        @Override
                        public void onFailure(String source, int errCode,
                                              String errMsg) {
                            views[idx].setText(source + " errCode:"
                                        + errCode + ", errMsg:" + errMsg);

                        }

                    });
        }
	}

	
	protected void list() {
		//鍙互鑾峰彇涓や釜鏂囦欢锛屽洜涓鸿繕鏈夋枃浠舵病鏈夎鐨勬潈闄�
		storage.listFiles(new FileListListener() {

			@Override
			public void onSuccess(List<FrontiaFile> list) {
				//搴旇鍙互list鍑烘潵涓や釜鏂囦欢锛屽洜涓轰笂浼犵殑鍥涗釜鏂囦欢涓彧鏈変袱涓槸鍙鐨�
				StringBuilder sb = new StringBuilder();
				for (FrontiaFile info : list) {

					sb.append(info.getRemotePath()).append('\n').append("size: ")
							.append(info.getSize()).append('\n')
							.append("modified time: ")
							.append(info.getModifyTime().toString())
							.append('\n')
                    .append(info.getMD5()).append('\n').append(info.isDir()).append('\n');

				}
				if (null != info) {
					info.setText(sb.toString());
				}
			}

			@Override
			public void onFailure(int errCode, String errMsg) {
				if (null != info) {
					info.setText("errCode:" + errCode + ", errMsg:"
							+ errMsg);
				}
			}

		});

	}

	protected void deleteFile() {

		//鍙互鍒犻櫎涓や釜鏂囦欢锛屽洜涓鸿繕鏈変袱涓枃浠舵病鏈夊啓鐨勬潈闄�

        for(int i=0;i<4;i++){
            final int idx = i;

            storage.deleteFile(mFile[i],
                    new FileOperationListener() {

                        @Override
                        public void onSuccess(String source) {
                            views[idx].setText(source + " is deleted");
                        }

                        @Override
                        public void onFailure(String source, int errCode,
                                              String errMsg) {
                            views[idx].setText(source + " errCode:"
                                    + errCode + ", errMsg:" + errMsg);
                        }

                    });
        }
	}
	
	private void insertData(){
		for(int i=0;i<4;i++){
            final int idx = i;

            storage.insertData(mData[i],
                    new FrontiaStorageListener.DataInsertListener() {

                        @Override
                        public void onSuccess() {
                            views[idx].setText("save data success\n");
                        }

                        @Override
                        public void onFailure(int errCode, String errMsg) {
                            views[idx].setText("errCode:" + errCode
                                    + ", errMsg:" + errMsg);
                        }

                    });
        }
	}
	
	protected void deleteData() {
		//鏍规嵁鏉′欢鍙互鏌ュ埌鍥涗釜鏁版嵁锛屼絾鏄叿鏈夊啓鏉冮檺鐨勬暟鎹彧鏈変袱涓紝鎵�互鍙兘鍒犻櫎涓や釜鏁版嵁
		FrontiaQuery query = new FrontiaQuery();
		query.lessThan("age", 100);

		storage.deleteData(
						query,
						new DataOperationListener() {

							@Override
							public void onSuccess(long count) {
								if (null != info) {
									info.setText("delete " + count
											+ " data.");
								}
							}

							@Override
							public void onFailure(int errCode, String errMsg) {
								if (null != info) {
									info.setText("errCode:"
											+ errCode + ", errMsg:" + errMsg);
								}
							}
						});
	}

	protected void updateData() {
		//鏍规嵁鏉′欢鍙互鏌ュ埌涓や釜鏁版嵁锛屼絾鏄叿鏈夊啓鏉冮檺鐨勬暟鎹彧鏈�涓紝鎵�互鍙兘鏇存柊涓�釜鏁版嵁
        FrontiaQuery query = new FrontiaQuery();
        query.lessThan("age", 40);

		FrontiaData data = new FrontiaData();
		data.put("name", "updated");
		data.put("age", 80);
        storage.updateData(
						query,
						data,
						new DataOperationListener() {

							@Override
							public void onSuccess(long count) {
								if (null != info) {
									info.setText("update " + count
											+ " data.");
								}
							}

							@Override
							public void onFailure(int errCode, String errMsg) {
								if (null != info) {
									info.setText("errCode:"
											+ errCode + ", errMsg:" + errMsg);
								}
							}
						});
	}

	protected void findData() {
		//鏍规嵁鏉′欢鍙互鏌ュ埌涓や釜鏁版嵁锛屽洜涓哄彧鏈変袱涓暟鎹叿鏈夊彲璇绘潈闄�
		FrontiaQuery query = new FrontiaQuery();

		storage.findData(query,
				new DataInfoListener() {

					@Override
					public void onSuccess(List<FrontiaData> dataList) {
						if (null != info) {
                            StringBuilder sb = new StringBuilder();
                            int i = 0;
                            for(FrontiaData d : dataList){
                                sb.append(i).append(":").append(d.toJSON().toString()).append("\n");
                            }
                            info.setText("find data\n"
									+ sb.toString());
						}
					}

					@Override
					public void onFailure(int errCode, String errMsg) {
						if (null != info) {
							info.setText("errCode:" + errCode
									+ ", errMsg:" + errMsg);
						}
					}
				});

	}
	
	private void createRoleWithAcl(){
        for(int i = 0;i < 4;i++){
        	final int idx = i;
        	mRole[i].create(new CommonOperationListener() {
				
				@Override
				public void onSuccess() {
					views[idx].setText(mRole[idx].getId()+" saved");
				}
				
				@Override
				public void onFailure(int errCode, String errMsg) {
					views[idx].setText("errCode:" + errCode
                            + ", errMsg:" + errMsg);
				}
			});
        }
        
    }
	
	private void findRoleWithAcl(){
		//涓や釜鍏锋湁鍙鏉冮檺鐨剅ole灏嗚鏌ュ埌
		FrontiaRole.fetch(new FrontiaRole.FetchRoleListener() {
            @Override
            public void onSuccess(List<FrontiaRole> roleList) {
                if (null != info) {
                    StringBuilder buf = new StringBuilder();
                    for (FrontiaRole role : roleList) {
                        buf.append(role.getId()).append("\n");

                        List<FrontiaAccount> accounts = role.getMembers();
                        for (FrontiaAccount account : accounts) {
                            buf.append("    ").append(account.getId()).append("\n");
                        }
                    }
                    info.setText(buf.toString());
                }
            }

            @Override
            public void onFailure(int errCode, String errMsg) {
                if (null != info) {
                	info.setText("errCode:" + errCode
                            + ", errMsg:" + errMsg);
                }
            }
        });

    }
	
	private void delRoleWithAcl(){
	
		//涓や釜鍏锋湁鍙啓鏉冮檺鐨剅ole灏嗚鍒犻櫎
		for(int i = 0;i < 4;i++){
        	final int idx = i;
        	mRole[i].delete(new CommonOperationListener() {
				
				@Override
				public void onSuccess() {
					views[idx].setText(mRole[idx].getId()+" removed");
				}
				
				@Override
				public void onFailure(int errCode, String errMsg) {
					views[idx].setText("errCode:" + errCode
                            + ", errMsg:" + errMsg);
				}
			});
        }
		
    }

}
