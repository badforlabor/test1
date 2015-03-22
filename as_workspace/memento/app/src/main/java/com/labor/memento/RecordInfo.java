package com.labor.memento;

/**
 * Created by harold on 2015/3/22.
 * 录音文件的信息，存贮：ID（主键），标签，记录时间，文件名（多个文件）
 */
public class RecordInfo {
    long id;
    String tag = "none";
    long date;
    String fileName = "";
    public RecordInfo(String tag){
        this.tag = tag;
        date = System.currentTimeMillis();
        fileName = "" + date + ".3gp";
    }
    public void SaveTmpRecordFile(){
        if(id == 0)        {
            return;
        }

        // 读取录音文件，然后存起来
        MainActivity.FileUtil.MakeDir(CONF.ROOT_MEMETO_AUDIO_DIR);
        MainActivity.FileUtil.CopyFile(CONF.REC_FILE, CONF.ROOT_MEMETO_AUDIO_DIR + "/" + fileName);
    }

    public static class DBUtil{

    }
}
