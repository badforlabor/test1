package com.baidu.frontia.demo.storage;

import java.util.List;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import com.baidu.frontia.Frontia;
import com.baidu.frontia.FrontiaFile;
import com.baidu.frontia.api.FrontiaStorage;
import com.baidu.frontia.api.FrontiaStorageListener.FileListListener;
import com.baidu.frontia.api.FrontiaStorageListener.FileOperationListener;
import com.baidu.frontia.api.FrontiaStorageListener.FileProgressListener;
import com.baidu.frontia.api.FrontiaStorageListener.FileTransferListener;
import com.baidu.frontia.demo.Conf;
import com.baidu.frontia.demo.R;

public class AppFileActivity extends Activity {

	private FrontiaStorage mCloudStorage;

	private TextView mResultTextView;
	private TextView mInfoView;

    private FrontiaFile mFile;
    private Button listButton;
    private Button uploadFileButton;
    private Button downloadFileButton;
    private Button deleteFileButton;
    
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setupViews();

		mFile = new FrontiaFile();
		mFile.setNativePath(Conf.LOCAL_FILE_NAME);
		mFile.setRemotePath(Conf.APP_STORAGE_FILE_NAME);
		mCloudStorage = Frontia.getStorage();
	}
	
	private void setupViews() {
		setContentView(R.layout.app_file_storage);

		mResultTextView = (TextView)findViewById(R.id.appFileResult);
		mInfoView = (TextView)findViewById(R.id.retView);
		
		uploadFileButton = (Button) findViewById(R.id.uploadFile);
        uploadFileButton.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
                clearViews();
                uploadFile();
			}
		});

        downloadFileButton = (Button) findViewById(R.id.downloadFile);
		downloadFileButton.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
                clearViews();
                downloadFile();
			}

		});

		listButton = (Button) findViewById(R.id.list);
		listButton.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
                clearViews();
                list();
			}

		});

		deleteFileButton = (Button) findViewById(R.id.deleteFile);
		deleteFileButton.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
                clearViews();
				deleteFile();
			}

		});

	}

    private void clearViews(){
        mResultTextView.setText("");
        mInfoView.setText("");
    }

    protected void uploadFile() {
    	mCloudStorage.uploadFile(mFile,
                new FileProgressListener() {
                    @Override
                    public void onProgress(String source, long bytes, long total) {
                    	mInfoView.setText(source + " upload......:"
                                + bytes * 100 / total + "%");
                    }
                },
                new FileTransferListener() {
                    @Override
                    public void onSuccess(String source, String newTargetName) {
                    	mFile.setRemotePath(newTargetName);
                        mInfoView.setText(source + " uploaded as "
                                + newTargetName + " in the cloud.\n提示:如果服务器端有同名文件，则上传文件会按时间戳重命名哦~");
                    }

                    @Override
                    public void onFailure(String source, int errCode, String errMsg) {
                    	mInfoView.setText(source + " errCode:"
                                + errCode + ", errMsg:" + errMsg);
                    }
                }
        );
	}

	protected void downloadFile() {

		mCloudStorage.downloadFile(mFile, new FileProgressListener() {

            @Override
            public void onProgress(String source, long bytes, long total) {
                mInfoView.setText(source + " download......:"
                            + bytes * 100 / total + "%");

            }

        }, new FileTransferListener() {

            @Override
            public void onSuccess(String source, String newTargetName) {

            	mInfoView.setText(source + " downloaded as "
                            + newTargetName + " in the local.\n提示：如果下载文件在本地有同名文件，则下载的文件会被按时间戳重命名哦~");

            }

            @Override
            public void onFailure(String source, int errCode,
                                  String errMsg) {
            	mInfoView.setText(source + " errCode:"
                            + errCode + ", errMsg:" + errMsg);

            }

        });
	}

	protected void list() {
		mCloudStorage.listFiles(new FileListListener() {

			@Override
			public void onSuccess(List<FrontiaFile> list) {
				StringBuilder sb = new StringBuilder();
				for (FrontiaFile info : list) {

					sb.append(info.getRemotePath()).append('\n').append("size: ")
							.append(info.getSize()).append('\n')
							.append("modified time: ")
							.append(info.getModifyTime().toString())
							.append('\n')
                    .append(info.getMD5()).append('\n').append(info.isDir()).append('\n');

				}
				if (null != mResultTextView) {
					mResultTextView.setText(sb.toString());
				}
			}

			@Override
			public void onFailure(int errCode, String errMsg) {
				if (null != mResultTextView) {
					mResultTextView.setText("errCode:" + errCode + ", errMsg:"
							+ errMsg);
				}
			}

		});

	}

	protected void deleteFile() {

		mCloudStorage.deleteFile(mFile,
                new FileOperationListener() {

                    @Override
                    public void onSuccess(String source) {
                       mInfoView.setText(source + " is deleted");
                    }

                    @Override
                    public void onFailure(String source, int errCode,
                                          String errMsg) {
                    	mInfoView.setText(source + " errCode:"
                                + errCode + ", errMsg:" + errMsg);
                    }

                });
	}

	
}
