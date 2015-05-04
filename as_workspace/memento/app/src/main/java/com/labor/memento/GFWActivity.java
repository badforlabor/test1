package com.labor.memento;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.TextView;

import java.util.Date;

/**
 * Created by harold on 2015/4/5.
 * 密码页面
 */
public class GFWActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_gfw);

        TextView txt = (TextView)this.findViewById(R.id.txtlock);
        txt.setOnClickListener(new View.OnClickListener() {

            // 连续点击7次才可以解锁
            long last = -1;
            int cnt = 0;

            @Override
            public void onClick(View v) {

                Log.i(CONF.API_TEST, "onclick:" + System.currentTimeMillis() + ", " + (System.currentTimeMillis() - last) + ", cnt=" + cnt);
                if(last == -1 || last + 500 > System.currentTimeMillis() ){
                    last = System.currentTimeMillis();
                    cnt ++ ;
                    if(cnt < 7){
                        return;
                    }
                }
                else{
                    last = -1;
                    cnt = 0;
                    return;
                }


                // 返回到主界面
                Intent intent = new Intent(v.getContext(), MainActivity.class);
                // 清除页面堆栈，这样返回主界面之后再次点击返回就不会再次回到这个界面了
                intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                startActivity(intent);
            }
        });

    }
}
