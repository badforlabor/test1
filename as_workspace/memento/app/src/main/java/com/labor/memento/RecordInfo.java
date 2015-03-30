package com.labor.memento;

import android.support.annotation.NonNull;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;
import java.util.ListIterator;

/**
 * Created by harold on 2015/3/22.
 * 录音文件的信息，存贮：ID（主键），标签，记录时间，文件名（多个文件）
 */
public class RecordInfo {
    long id;
    String tag = "none";
    long date;
    String fileName = "";

    public RecordInfo(){
        id = 0; // 非法的
    }

    public RecordInfo(String tag) {
        id = Utils.DBUtil.RadioID;
        this.tag = tag;
        date = System.currentTimeMillis();
        fileName = date + ".3gp";
    }

    public void SaveTmpRecordFile() {
        if (id == 0) {
            return;
        }

        // 读取录音文件，然后存起来
        Utils.FileUtil.MakeDir(CONF.LOCAL_ROOT_MEMENTO_AUDIO_DIR);
        Utils.FileUtil.CopyFile(CONF.REC_FILE, CONF.LOCAL_ROOT_MEMENTO_AUDIO_DIR + "/" + fileName);

        // 告知数据库
        Utils.DBUtil.Insert(this);

        records.add(this);
    }

    public static ArrayList<RecordInfo> records = new ArrayList<RecordInfo>();
    public static void InitDB(){
        records = Utils.DBUtil.SelectAll();
    }
    public static RecordInfo GetRecord(long id){
        for(int i=0; i<records.size(); i++){
            if(records.get(i).id == id){
                return records.get(i);
            }
        }
        return null;
    }

}
