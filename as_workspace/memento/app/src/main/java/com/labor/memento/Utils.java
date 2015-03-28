package com.labor.memento;

import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.util.Log;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;

/**
 * Created by harold on 2015/3/23.
 * <p/>
 * <p/>
 * SQLite3支持 NULL、INTEGER、REAL（浮点数字）、TEXT(字符串文本)和BLOB(二进制对象)数据类型，虽然它支持的类型虽然只有五种，但实际上sqlite3也接受varchar(n)、char(n)、decimal(p,s) 等数据类型，只不过在运算或保存时会转成对应的五种数据类型。
 */
public class Utils {

    public static void Init() {
        DBUtil.Init();
    }

    public static class DBUtil {

        public static long RadioID = 0;

        public static void Init() {

            // 构建数据库
            try {
                SQLiteDatabase db = SQLiteDatabase.openOrCreateDatabase(CONF.LOCAL_DB_MEMENTO_FILE, null);
                db.execSQL("CREATE TABLE IF NOT EXISTS recordinfo" +
                        "(_id INTEGER PRIMARY KEY AUTOINCREMENT, tag VARCHAR, date BIGINT, fileName TEXT)");
                Cursor cursor = db.rawQuery("SELECT count(_id) from recordinfo", null);
                RadioID = cursor.moveToNext() ? cursor.getInt(0) : 0;
                db.close();
            } catch (Exception e) {
                Log.e("api-test", e.getMessage());
            }
        }

        public static void Insert(RecordInfo ri) {
            SQLiteDatabase db = SQLiteDatabase.openDatabase(CONF.LOCAL_DB_MEMENTO_FILE, null, Context.MODE_PRIVATE, null);
            db.execSQL("INSERT INTO recordinfo VALUES(NULL, ?, ?)", new Object[]{ri.tag, ri.date, ri.fileName});
            db.close();
            RadioID++;
        }

        public static void Update(RecordInfo ri) {
            SQLiteDatabase db = SQLiteDatabase.openDatabase(CONF.LOCAL_DB_MEMENTO_FILE, null, Context.MODE_PRIVATE, null);
            db.execSQL("UPDATE recordinfo SET tag='?', date=?, filename='?' where id=" + ri.id, new Object[]{ri.tag, ri.date, ri.fileName});
            db.close();
        }
    }


    public static class FileUtil {
        public static void MakeDir(String fullpath) {
            try {
                File dir = new File(fullpath);
                if (!dir.exists()) {
                    dir.mkdirs();
                }
            }
            catch(Exception e){
                Log.e("api-test", "io exception=" + e.getMessage());
            }
        }

        public static void CopyFile(String src, String dest) {
            try {
                FileInputStream in = new FileInputStream(src);
                File file = new File(dest);
                if (!file.exists())
                    file.createNewFile();
                FileOutputStream out = new FileOutputStream(file);
                int c;
                byte buffer[] = new byte[1024];
                while ((c = in.read(buffer)) != -1) {
//                    for (int i = 0; i < c; i++)
//                        out.write(buffer[i]);
                    out.write(buffer, 0, c);
                }
                in.close();
                out.close();
            } catch (Exception e) {
                Log.e("api-test", "io exception=" + e.getMessage());
            }
        }
        public static void RemoveFile(String fullpath){
            try{
                File file = new File(fullpath);
                if(file.exists()){
                    file.delete();
                }
            }
            catch(Exception e){
                Log.e("api-test", "io exception=" + e.getMessage());
            }
        }
    }
}
