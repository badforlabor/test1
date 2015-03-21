package com.baidu.frontia.demo.storage;

import java.util.List;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import com.baidu.frontia.Frontia;
import com.baidu.frontia.FrontiaData;
import com.baidu.frontia.FrontiaQuery;
import com.baidu.frontia.api.FrontiaStorage;
import com.baidu.frontia.api.FrontiaStorageListener;
import com.baidu.frontia.api.FrontiaStorageListener.DataInfoListener;
import com.baidu.frontia.api.FrontiaStorageListener.DataOperationListener;
import com.baidu.frontia.demo.R;

public class AppDataActivity extends Activity {

	private FrontiaStorage mCloudStorage;

	private TextView mResultTextView;
    private TextView[] views;
    private TextView mOpInfo;

    private Button saveDataButton;
    private Button deleteDataButton;
    private Button updateDataButton;
    private Button findDataButton;
    
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setupViews();

		mCloudStorage = Frontia.getStorage();
	}
	
	private void setupViews() {
		setContentView(R.layout.app_data_storage);

		mResultTextView = (TextView) findViewById(R.id.appDataResult);
		mOpInfo = (TextView)findViewById(R.id.appDataOp);
        views = new TextView[4];
        int[] viewIds = new int[]{R.id.appDataView1, R.id.appDataView2, R.id.appDataView3, R.id.appDataView4};
        for(int i=0;i<4;i++){
            views[i] = (TextView) findViewById(viewIds[i]);
        }

		saveDataButton = (Button) findViewById(R.id.saveData);
		saveDataButton.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
                clearViews();
				saveData();
			}

		});

		deleteDataButton = (Button) findViewById(R.id.deleteData);
		deleteDataButton.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
                clearViews();
				deleteData();
			}

		});

		updateDataButton = (Button) findViewById(R.id.uploadData);
		updateDataButton.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
                clearViews();
                updateData();
			}

		});

		findDataButton = (Button) findViewById(R.id.findData);
		findDataButton.setOnClickListener(new View.OnClickListener() {

			@Override
			public void onClick(View v) {
                clearViews();
				findData();
			}

		});

	}
	
	private void clearViews(){
        mResultTextView.setText("");
        for(int i=0;i<4;i++){
            views[i].setText("");
        }
    }

	protected void saveData() {
		final FrontiaData[] datas = new FrontiaData[4];

        datas[0] = new FrontiaData();
        datas[0].put("animal", "Panda");
        datas[0].put("number", 71);

        datas[1] = new FrontiaData();
        datas[1].put("animal", "Cat");
        datas[1].put("number", 300);

        datas[2] = new FrontiaData();
        datas[2].put("animal", "Dog");
        datas[2].put("number", 50);

        datas[3] = new FrontiaData();
        datas[3].put("animal", "Dragon");
        datas[3].put("number", 0);

        for(int i=0;i<4;i++){
            final int idx = i;

            mCloudStorage.insertData(datas[i],
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
		//FrontiaQuery中有很多查询条件，你可以尝试多种查询条件，相当于sql语句中的where
		FrontiaQuery q1 = new FrontiaQuery();
        q1.equals("animal", "Panda");
        FrontiaQuery q2 = new FrontiaQuery();
        q2.equals("animal", "Dragon");
        FrontiaQuery q3 = new FrontiaQuery();
        q3.equals("animal", "Cat");
        FrontiaQuery q4 = new FrontiaQuery();
        q4.equals("animal", "Dog");
        FrontiaQuery query = q1.or(q2).or(q3).or(q4);

		mCloudStorage.deleteData(
						query,
						new DataOperationListener() {

							@Override
							public void onSuccess(long count) {
								if (null != mResultTextView) {
									mResultTextView.setText("delete " + count
											+ " data.");
								}
							}

							@Override
							public void onFailure(int errCode, String errMsg) {
								if (null != mResultTextView) {
									mResultTextView.setText("errCode:"
											+ errCode + ", errMsg:" + errMsg);
								}
							}
						});
	}

	protected void updateData() {
        FrontiaQuery query = new FrontiaQuery();
        query.lessThan("number", 20);

        FrontiaData newData = new FrontiaData();
        newData.put("number",20);
        newData.put("animal", "Dog");

        mCloudStorage.updateData(
						query,
						newData,
						new DataOperationListener() {

							@Override
							public void onSuccess(long count) {
								if (null != mResultTextView) {
									mResultTextView.setText("update " + count
											+ " data.");
								}
							}

							@Override
							public void onFailure(int errCode, String errMsg) {
								if (null != mResultTextView) {
									mResultTextView.setText("errCode:"
											+ errCode + ", errMsg:" + errMsg);
								}
							}
						});
	}

	protected void findData() {
		//空的FrontiaQuery表示query所有的数据(具有可读权限数据才能被查到)
		FrontiaQuery query = new FrontiaQuery();

		mCloudStorage.findData(query,
				new DataInfoListener() {

					@Override
					public void onSuccess(List<FrontiaData> dataList) {
						if (null != mResultTextView) {
                            StringBuilder sb = new StringBuilder();
                            int i = 0;
                            for(FrontiaData d : dataList){
                                sb.append(i).append(":").append(d.toJSON().toString()).append("\n");
                                i++;
                            }
							mResultTextView.setText("find data\n"
									+ sb.toString());
						}
					}

					@Override
					public void onFailure(int errCode, String errMsg) {
						if (null != mResultTextView) {
							mResultTextView.setText("errCode:" + errCode
									+ ", errMsg:" + errMsg);
						}
					}
				});

	}

}
