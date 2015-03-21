package com.baidu.frontia.demo.role;

import java.util.List;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import com.baidu.frontia.FrontiaAccount;
import com.baidu.frontia.FrontiaRole;
import com.baidu.frontia.FrontiaUser;
import com.baidu.frontia.FrontiaUserQuery;
import com.baidu.frontia.demo.R;

public class AccountActivity extends Activity {

    private TextView mResultTextView;
    private FrontiaRole mRole;
    private Activity mActivity;
    
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		mActivity = this;
		setupViews();
	}

	private void setupViews(){
        setContentView(R.layout.role_user);

        mResultTextView = (TextView) findViewById(R.id.role_user_ret_view);

        Button b = (Button) findViewById(R.id.create_role_button);
        b.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	clearView();
                createRole();
            }
        });

        b = (Button) findViewById(R.id.modify_role_button);
        b.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	clearView();
                modifyRole();
            }
        });

        b = (Button) findViewById(R.id.del_role_button);
        b.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	clearView();
                delRole();
            }
        });

        b = (Button) findViewById(R.id.find_role_button);
        b.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	clearView();
                listRole();
            }
        });

        b = (Button) findViewById(R.id.describe_role_button);
        b.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
            	clearView();
                describeRole();
            }
        });

        b = (Button) findViewById(R.id.find_user_button);
        b.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
            	clearView();
                listUser();;
            }
        });

    }

    protected void clearView() {
    	mResultTextView.setText("");
	}

	private void createRole(){
        mRole = new FrontiaRole("baidu");
        FrontiaRole subRole1 = new FrontiaRole("mco");
        //FrontiaUser curUser = (FrontiaUser)Frontia.getCurrentAccount();

        FrontiaUser nullUser = new FrontiaUser(null);
        //mRole.addMember(curUser);
        mRole.addMember(nullUser);
        mRole.addMember(subRole1);
        FrontiaRole subRole2 = new FrontiaRole("cloud");
        mRole.addMember(subRole2);

        mRole.create(new FrontiaRole.CommonOperationListener() {

            @Override
            public void onSuccess() {
                if (null != mResultTextView) {
                    mResultTextView.setText("role saved");
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

    

    private void modifyRole(){
        if(mRole == null){
            mRole = new FrontiaRole("baidu");
        }

        mRole.addMember(new FrontiaRole("search"));
        mRole.update(new FrontiaRole.CommonOperationListener() {
            @Override
            public void onSuccess() {
                if (null != mResultTextView) {
                    mResultTextView.setText("role modified");
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

    

    private void delRole(){
        if(mRole == null){
            mRole = new FrontiaRole("baidu");
        }
        mRole.delete(new FrontiaRole.CommonOperationListener() {
            @Override
            public void onSuccess() {
                if (null != mResultTextView) {
                    mResultTextView.setText("role deleted");
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

    private void listRole(){

        FrontiaRole.fetch(new FrontiaRole.FetchRoleListener() {
            @Override
            public void onSuccess(List<FrontiaRole> roleList) {
                if (null != mResultTextView) {
                    StringBuilder buf = new StringBuilder();
                    for (FrontiaRole role : roleList) {
                        buf.append(role.getId()).append("\n");

                        List<FrontiaAccount> accounts = role.getMembers();
                        for (FrontiaAccount account : accounts) {
                            buf.append("    ").append(account.getId()).append("\n");
                        }
                    }
                    mResultTextView.setText(buf.toString());
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

    private void describeRole(){

        if(null == mRole){
            mRole = new FrontiaRole("baidu");
        }

        mRole.describe(new FrontiaRole.DescribeRoleListener() {
            @Override
            public void onSuccess(FrontiaRole role) {
                if (null != mResultTextView) {
                    StringBuilder buf = new StringBuilder();
                    buf.append(role.getId()).append("\n");

                        List<FrontiaAccount> accounts = role.getMembers();
                        for (FrontiaAccount account : accounts) {
                            buf.append("    ").append(account.getId()).append("\n");
                        }

                    mResultTextView.setText(buf.toString());
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

    

    private void listUser(){

        FrontiaUserQuery query = new FrontiaUserQuery();

        FrontiaUser.findUsers(query, new FrontiaUser.FetchUserListener() {
            @Override
            public void onSuccess(List<FrontiaUser.FrontiaUserDetail> userList) {
                if (null != mResultTextView) {
                    StringBuilder buf = new StringBuilder();
                    for (FrontiaUser.FrontiaUserDetail user : userList) {
                        String result = "user: " + user.getId() + "\n"
                                + "  username:" + user.getName() + "\n"
                                + "  birthday:" + user.getBirthday() + "\n"
                                + "  city:" + user.getCity() + "\n"
                                + "  province:" + user.getProvince() + "\n"
                                + "  sex:" + user.getSex() + "\n"
                                + "  pic url:" + user.getHeadUrl() + "\n";
                        buf.append(result);
                    }
                    mResultTextView.setText(buf.toString());
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
