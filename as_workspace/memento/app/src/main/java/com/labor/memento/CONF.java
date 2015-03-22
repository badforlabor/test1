package com.labor.memento;

public class CONF {
	public final static String API_KEY = "iG2ffdkYaq8kIjrSfvjMcUrf";
	
	// 貌似获取文件信息或者上传文件时，只能传到自己的根目录下。
	public final static String ROOT_DIR = "/apps/FrontiaDevDemo";
	
	public final static String LOCAL_ROOT_DIR = "/sdcard/FrontiaDevDemo";
	
	// 测试下载文件：
	public final static String DOWNLOAD_FILE = "/apps/FrontiaDevDemo/helloworld.txt";	
	// 测试上传文件：
	public final static String UPLOAD_FILE = "/sdcard/FrontiaDevDemo/helloworld.txt";
	// 测试录音
	public final static String REC_FILE = "/sdcard/FrontiaDevDemo/rec-temp.3gp";

    // 子APP ROOT文件夹
    public final static String ROOT_MEMENTO_DIR = "/apps/FrontiaDevDemo/memento";
    // 录音文件
    public final static String ROOT_MEMETO_AUDIO_DIR = "/apps/FrontiaDevDemo/memento/audio";
    // db文件
    public final static String DB_MEMENTO_FILE = "/apps/FrontiaDevDemo/memento/memento.db";
}
