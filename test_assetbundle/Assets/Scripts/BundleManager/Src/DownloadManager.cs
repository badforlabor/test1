using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Uri = System.Uri;
using LitJson;
using MLGame;

/**
 * DownloadManager is a runtime class for asset steaming and WWW management.
 */ 
public class DownloadManager : MonoBehaviour 
{
    // 不同的文件加载方式
    static string LOADFOLDER = "iLingYou-g/";



	/**
	 * Get the error string of WWW request.
	 * @return The error string of WWW. Return null if WWW request succeed or still in processing.
	 */ 
	public string GetError(string url)
	{
		if(!ConfigLoaded)
			return null;
		
		url = formatUrl(url);
		if(failedRequest.ContainsKey(url))
			return failedRequest[url].www.error;
		else
			return null;
	}
	
	/**
	 * Test if the url is already requested.
	 */
	public bool IsUrlRequested(string url)
	{
		if(!ConfigLoaded)
		{
			return isInBeforeInitList(url);
		}
		else
		{
			url = formatUrl(url);
			bool isRequested = isInWaitingList(url) || processingRequest.ContainsKey(url) || succeedRequest.ContainsKey(url) || failedRequest.ContainsKey(url);
			return isRequested;
		}
	}

    static string ReplaceSlashWithBlackspace(string abc)
    {
        if(string.IsNullOrEmpty(abc))
        {
            return "";
        }
        return abc.Replace('/', '-').Replace(' ', '+');
    }

	/**
	 * Get WWW instance of the url.
	 * @return Return null if the WWW request haven't succeed.
	 */ 
	public WWW GetWWW(string url)
    {
        url = ReplaceSlashWithBlackspace(url);
        //Debug.Log("[pak-bundle] get www url:" + url);
		if(!ConfigLoaded)
			return null;
		
		url = formatUrl(url);
        url += "." + bmConfiger.bundleSuffix;
		
		if(succeedRequest.ContainsKey(url))
		{
			prepareDependBundles(url);
			return succeedRequest[url].www;
		}
		else
			return null;
	}

    static string ReplaceSlashWithBlackspace2(string abc)
    {
        return abc.Replace('/', '-');
    }
    // url为全路径名，形如：Assets/Resources/prefab/particleeffects/baodian.prefab
    public WWW GetWWWFully(string url)
    {
        System.Single curTime = Time.realtimeSinceStartup;
        WWW www = GetWWWFully1(url);
        if (www == null || !www.isDone)
        {
            Debug.Log("[pak-bundle] ---- get www fully failed! time-cost=" + (Time.realtimeSinceStartup - curTime));
        }
        else
        {
            //Debug.Log("[pak-bundle] ---- get www fully succes! time-cost=" + (Time.realtimeSinceStartup - curTime));
        }
        return www;
    }

    Dictionary<string, AssetBundle> DownloadedAB = new Dictionary<string, AssetBundle>();


    public bool IsDownloaded(string url)
    {
        return DownloadedURLs.ContainsKey(url);
    }

    public void AddWWW(string url, WWW www)
    {
        if (www == null || !www.isDone || www.assetBundle == null)
        {
            MLLogger.LogError("[pak-bundle] add invalide www:" + url);
            return;
        }
        DownloadedURLs.Add(url, www);
        //DownloadedAB.Add(url, www.assetBundle);
        //www.assetBundle.Unload(false);
    }
    public WWW GetWWWFully1(string url)
    {
        // 直接执行WWW(url)创建，并且做yield下载

        if (DownloadedURLs.ContainsKey(url))
        {
            return DownloadedURLs[url];
        }

        return null;

        // 将依赖的内容全部加在到内存中

        if (bundles != null)
        {
            foreach (var bundle in bundles)
            {
                if (bundle.name == url)
                {
                    foreach (var dep in bundle.dependAssets)
                    {
                        GetWWWFully1(dep);   
                    }
                    break;
                }
            }
        }

        // 然后再加载自己！
        string newurl = ReplaceSlashWithBlackspace(url);
        newurl = formatUrl(newurl + ".assetbundle");
        var www = new WWW(newurl);
        //WWW www = null;
        //www = WWW.LoadFromCacheOrDownload(newurl, 1);
        //StartCoroutine(YieldDownURL(www));

        //while (www == null || www.assetBundle == null || !www.isDone)
        //{
        //    Debug.Log("wait down load assert:" + newurl);
        //}
        //yield return www;
        // 保证www执行完毕！
        Debug.Log("wait down load assert:" + newurl);
        DownloadedURLs.Add(url, www);

        //Debug.Log("[pak-bundle]---get www fully, url=" + url + ", IsDone=" + www.isDone);

        return www;
    }

    // 从磁盘加载
    public AssetBundle CreateFromFile(string url)
    {
        AssetBundle ret = CreateFromFile2(url);

        if (ret == null)
        {
            MLLogger.LogError("[pak-bundle] create from file:" + url + ", ret=" + (ret == null ? "none" : ret.name));
        }
        else
        {
            MLLogger.LogDebug("[pak-bundle] create from file:" + url + ", ret=" + (ret == null ? "none" : ret.name));
        }
        
        return ret;
    }
    public AssetBundle CreateFromFile2(string url)
    {
        if (DownloadedABs.ContainsKey(url))
        {
            return DownloadedABs[url];
        }

        if (bundles != null)
        {
            foreach (var bundle in bundles)
            {
                if (bundle.name == url)
                {
                    foreach (var dep in bundle.dependAssets)
                    {
                        System.Diagnostics.Debug.Assert( CreateFromFile2(dep) != null );
                    }
                    break;
                }
            }
        }

        string newurl = ReplaceSlashWithBlackspace(url) + ".assetbundle";
        string newurl2 = formatUrl(newurl);
        
#if UNITY_IPHONE
        if (newurl2.StartsWith("file:///"))
        {
            newurl2 = newurl2.Substring(7);
        }
#elif UNITY_ANDROID
        if (newurl2.StartsWith("jar:///"))
        {
            newurl2 = newurl2.Substring(6);
        }
#else
        if (newurl2.StartsWith("file:///"))
        {
            newurl2 = newurl2.Substring(8);
        }
#endif

        if (DownloadedABs.ContainsKey(url))
        {
            return DownloadedABs[url];
        }

        // 将raw里面的文件拷贝到临时目录里面
        string finalurl = Application.persistentDataPath + "/" + newurl;
        File.Copy(newurl2, finalurl, true);
        newurl = finalurl;

        MLLogger.LogDebug("[pak-bundle] create from file 2, new url=" + newurl + ", 2=" + newurl2);
        AssetBundle ab = AssetBundle.CreateFromFile(newurl);

        DownloadedABs.Add(url, ab);

        return ab;
    }



    IEnumerator YieldDownURL(WWW www)
    {
        yield return (www);
    }
    IEnumerator Sleep(float a)
    {
        yield return new WaitForSeconds(a);
    }

    // !< 获取配置信息
    public BMConfiger Config
    {
        get
        {
            return bmConfiger;
        }
    }
	
	public IEnumerator WaitDownload(string url)
	{
		yield return StartCoroutine( WaitDownload(url, -1) );
	}
	
	/**
	 * Coroutine for download waiting. 
	 * You should use it like this,
	 * yield return StartCoroutine(DownloadManager.Instance.WaitDownload("bundle1.assetbundle"));
	 * If this url didn't been requested, the coroutine will start a new request.
	 */ 
	public IEnumerator WaitDownload(string url, int priority)
	{
		while(!ConfigLoaded)
			yield return null;
		
		url = formatUrl(url);
		WWWRequest request = new WWWRequest();
		request.url = url;
		request.priority = priority;
		download(request);
		
		while(isDownloadingWWW(url))
			yield return null;
	}

    List<string> PendingUrls = new List<string>();
    public void PendingDownload(string url)
    { 
        // 传入的url不是全路径，而是Assets-Resource-prefab-1.prefab 格式

        if (false && PendingUrls.Count > 100)
        {
            return;
        }

        if (!url.ToLower().Contains("resources-"))
        {
            return;
        }

        if (PendingUrls.Contains(url))
        {
            return;
        }
        PendingUrls.Add(url);

        return;

        if (!bundleDict.ContainsKey(url) )
        {
            return;
        }
        if(PendingUrls.Contains(url))
        {
            return;
        }

        if (false)
        {
            // 依次下载url的依赖
            List<string> urls = new List<string>();
            RecursiveDownload2(ref urls, url);

            PendingUrls.AddRange(urls);
        }
        else
        {
            PendingUrls.Add(url);
        }

    }
    public void DoDownload()
    {
        foreach (var url in PendingUrls)
        {
            // 开始真正的下载：
            WWWRequest request = new WWWRequest();
            request.url = formatUrl(url + ".assetbundle");
            request.priority = 0;
            addRequestToWaitingList(request);
        }
    }
    public void RecursiveDownload2(ref List<string> urls, string url)
    {
        // 已经放在列表里面了，那么就过！
        if (PendingUrls.Contains(url) || urls.Contains(url))
        {
            return;
        }

        if (!bundleDict.ContainsKey(url))
        {
            return;
        }

        // 先下载依赖的部分，然后再下载自己！
        foreach (var dep in bundleDict[url].dependAssets)
        {
            RecursiveDownload2(ref urls, ReplaceSlashWithBlackspace(dep));
        }
        if (!urls.Contains(url))
        {
            urls.Add(url);
        }
    }


	public void StartDownload(string url)
	{
		StartDownload(url, -1);
	}
	
	/**
	 * Start a new download request.
	 * @param url The url for download. Can be a absolute or relative url.
	 * @param priority Priority for this request.
	 */ 
	public void StartDownload(string url, int priority)
	{
        // url的格式是dirA-DirB-fileC.txt
        //Debug.Log("[pak-bundle] start download:" + url);
		WWWRequest request = new WWWRequest();
        request.url = formatUrl(url);
		request.priority = priority;

		if(!ConfigLoaded)
		{
			if(!isInBeforeInitList(url))
				requestedBeforeInit.Add(request);
		}
		else
			download(request);
	}
	
	/**
	 * Stop a request.
	 */ 
	public void StopDownload(string url)
	{
		if(!ConfigLoaded)
		{
			requestedBeforeInit.RemoveAll(x => x.url == url);
		}
		else
		{
			url = formatUrl(url);
			
			waitingRequests.RemoveAll(x => x.url == url);
			
			if(processingRequest.ContainsKey(url))
			{
				processingRequest[url].www.Dispose();
				processingRequest.Remove(url);
			}
		}
	}
	
	/**
	 * Dispose a finished WWW request.
	 */ 
	public void DisposeWWW(string url)
	{
		url = formatUrl(url);
		StopDownload(url);
		
		if(succeedRequest.ContainsKey(url))
		{
			succeedRequest[url].www.Dispose();
			succeedRequest.Remove(url);
		}
		
		if(failedRequest.ContainsKey(url))
		{
			failedRequest[url].www.Dispose();
			failedRequest.Remove(url);
		}
	}
	
	/**
	 * This function will stop all request in processing.
	 */ 
	public void StopAll()
	{
		requestedBeforeInit.Clear();
		waitingRequests.Clear();
		
		foreach(WWWRequest request in processingRequest.Values)
			request.www.Dispose();
		
		processingRequest.Clear();
	}
	
	/**
	 * Get download progress of bundles.
	 * All bundle dependencies will be counted too.
	 * This method can only used on self built bundles.
	 */ 
	public float ProgressOfBundles(string[] bundlefiles)
	{
		if(!ConfigLoaded)
			return 0f;
		
		List<string> bundles = new List<string>();
		foreach(string bundlefile in bundlefiles)
		{
			if(!bundlefile.EndsWith( "." + bmConfiger.bundleSuffix, System.StringComparison.OrdinalIgnoreCase))
			{
				Debug.LogWarning("ProgressOfBundles only accept bundle files. " + bundlefile + " is not a bundle file.");
				continue;
			}
			
			bundles.Add(Path.GetFileNameWithoutExtension(bundlefile));
		}
		
		HashSet<string> allInludeBundles = new HashSet<string>();
		foreach(string bundle in bundles)
		{
			foreach(string depend in getDependList(bundle))
			{
				if(!allInludeBundles.Contains(depend))
					allInludeBundles.Add(depend);
			}
			
			if(!allInludeBundles.Contains(bundle))
				allInludeBundles.Add(bundle);
		}
		
		long currentSize = 0;
		long totalSize = 0;
		foreach(string bundleName in allInludeBundles)
		{
			if(!buildStatesDict.ContainsKey(bundleName))
			{
				Debug.LogError("Cannot get progress of [" + bundleName + "]. It's not such bundle in bundle build states list.");		
				continue;
			}
				
			long bundleSize = buildStatesDict[bundleName].size;
			totalSize += bundleSize;
			
			string url = formatUrl( bundleName + "." + bmConfiger.bundleSuffix );
			if(processingRequest.ContainsKey(url))
				currentSize += (long)(processingRequest[url].www.progress * bundleSize);
			
			if(succeedRequest.ContainsKey(url))
				currentSize += bundleSize;
		}
		
		if(totalSize == 0)
			return 0;
		else
			return ((float)currentSize)/totalSize;
	}

	/**
	 * Check if the config files downloading finished.
	 */
	public bool ConfigLoaded
	{
		get
		{
			return bundles != null && buildStates != null && bmConfiger != null;
		}
	}

	/**
	 * Get list of the built bundles. 
	 * Before use this, please make sure ConfigLoaded is true.
	 */ 
	public BundleData[] BuiltBundles
	{
		get
		{
			if(bundles == null)
				return null;
			else
				return bundles.ToArray();
		}
	}
    public int WaitingRequestCnt
    {
        get { return this.waitingRequests.Count; }
    }


	/**
	 * Get list of the BuildStates. 
	 * Before use this, please make sure ConfigLoaded is true.
	 */ 
	public BundleBuildState[] BuildStates
	{
		get
		{
			if(buildStates == null)
				return null;
			else
				return buildStates.ToArray();
		}
	}

	// Privats
	IEnumerator Start() 
	{
		// Initial download urls
		initRootUrl();

		// Try to get Url redirect file
		yield return StartCoroutine( DownloadConfigerFile("BMRedirect.txt") );
		
		if(curConfigWWW.error == null)
		{
			// Redirect download
			string downloadPathStr = BMUtility.InterpretPath(curConfigWWW.text, curPlatform);
			Uri downloadUri = new Uri(downloadPathStr);
			downloadRootUrl = downloadUri.AbsoluteUri;
		}

        yield return StartCoroutine(DownloadConfigerFile(LOADFOLDER + "Assets-Resources-Urls.txt.assetbundle"));

        if (!System.String.IsNullOrEmpty(curConfigWWW.error))
        {
            Debug.Log("Download assetbundle failed after retried for 3 times.\nError: " + curConfigWWW.error);
        }
        else
        {
            Debug.Log("Download assetbundle succ after retried for 3 times!");
        }
		
		// Download the bundle data file
        yield return StartCoroutine(DownloadConfigerFile(LOADFOLDER + "BundleData.txt"));

        if (!System.String.IsNullOrEmpty(curConfigWWW.error))
        {
            Debug.Log("Download BundleData.txt 2 failed after retried for 3 times.\nError: " + curConfigWWW.error);
        }
        else
        {
            Debug.Log("Download BundleData.txt 2 succ after retried for 3 times!");
        }
		
        
		if(!System.String.IsNullOrEmpty(curConfigWWW.error))
		{
			Debug.LogError("Download BundleData.txt failed after retried for 3 times.\nError: " + curConfigWWW.error);
			yield break;
		}
		
		bundles = JsonMapper.ToObject< List< BundleData > >(curConfigWWW.text);
		foreach(var bundle in bundles)
		{
			bundleDict.Add(ReplaceSlashWithBlackspace(bundle.name), bundle);
		}
		
		// Download the bundle build states file
        yield return StartCoroutine(DownloadConfigerFile(LOADFOLDER + "BuildStates.txt"));

        if (!System.String.IsNullOrEmpty(curConfigWWW.error))
		{
			Debug.LogError("Download BuildStates.txt failed after retried for 3 times.\nError: " + curConfigWWW.error);
			yield break;
		}
		
		buildStates = JsonMapper.ToObject< List< BundleBuildState > >(curConfigWWW.text);
		foreach(var buildState in buildStates)
		{
			buildStatesDict.Add(buildState.bundleName, buildState);
		}
		
		// Download Bundle Manager confieration
        yield return StartCoroutine(DownloadConfigerFile(LOADFOLDER + "BMConfiger.txt"));

        if (!System.String.IsNullOrEmpty(curConfigWWW.error))
		{
			Debug.LogError("Download BMConfiger.txt failed after retried for 3 times.\nError: " + curConfigWWW.error);
			yield break;
		}
		
		bmConfiger = JsonMapper.ToObject< BMConfiger >(curConfigWWW.text);

        // !< trigger config loaded callback
        if (mConfigLoadedCb != null)
        {
            mConfigLoadedCb();
        }

        MLGame.MLLogger.LogDebug("[pak-bundle] allready go!");
		
		// Start download for requests before init
		foreach(WWWRequest request in requestedBeforeInit)
		{
			download(request);
		}
	}

    public delegate void ConfigloadedCallback();
    ConfigloadedCallback mConfigLoadedCb = null;

    public void RegisterConfigLoaddedCb(ConfigloadedCallback cb)
    {
        if (ConfigLoaded)
        {
            if (cb != null)
            {
                cb();
            }
        }
        else
        {
            mConfigLoadedCb = cb;
        }
    }
    
	IEnumerator DownloadConfigerFile(string fileName)
	{
        MLLogger.LogDebug("[pak-bundle] download config file:" + Path.Combine(downloadRootUrl, fileName));
		curConfigWWW = null;
		for(int retryTime = 0; retryTime < 3; ++retryTime)
		{
			curConfigWWW = new WWW( Path.Combine( downloadRootUrl, fileName ) );
			yield return curConfigWWW; 
			
			if(System.String.IsNullOrEmpty(curConfigWWW.error))
				break;
		}
	}

    int timer_cnt = 0;
    bool bStop_timer_cnt = false;
    float timer_loaded = 0;
    
	
	void Update () 
	{
		if(!ConfigLoaded)
			return;
		
		// Check if any WWW is finished or errored
		List<string> newFinisheds = new List<string>();
		List<string> newFaileds = new List<string>();
		foreach(WWWRequest request in processingRequest.Values)
		{
			if(request.www.error != null)
			{
				if(request.triedTimes - 1 < bmConfiger.downloadRetryTime)
				{
					// Retry download
					request.CreatWWW();
				}
				else
				{
					newFaileds.Add( request.url );
					Debug.LogError("Download " + request.url + " failed for " + request.triedTimes + " times.\nError: " + request.www.error);
				}
			}
			else if(request.www.isDone)
			{
                request.NotifyDone();
				newFinisheds.Add( request.url );
			}
		}
		
		// Move complete bundles out of downloading list
		foreach(string finishedUrl in newFinisheds)
		{
			succeedRequest.Add(finishedUrl, processingRequest[finishedUrl]);
			//var bundle = processingRequest[finishedUrl].www.assetBundle;
			processingRequest.Remove(finishedUrl);
		}

        if (waitingRequests.Count > 0 || processingRequest.Count > 0)
        {
            timer_loaded += Time.deltaTime;
        }


        timer_cnt++;
        if (timer_cnt % 20 == 19 && !bStop_timer_cnt)
        {
            Debug.Log("[pak-bundle] download manager update. processingReq.length=" + processingRequest.Count
                    + ", succeedReq.length=" + succeedRequest.Count
                    + ", waiting.length=" + waitingRequests.Count
                    + ", cost-time=" + timer_loaded);
        }
        bStop_timer_cnt = (waitingRequests.Count == 0 && processingRequest.Count == 0);
		
		// Move failed bundles out of downloading list
		foreach(string finishedUrl in newFaileds)
		{
			if(!failedRequest.ContainsKey(finishedUrl))
				failedRequest.Add(finishedUrl, processingRequest[finishedUrl]);
			processingRequest.Remove(finishedUrl);
		}
		
		// Start download new bundles
		int waitingIndex = 0;
		while( processingRequest.Count < bmConfiger.downloadThreadsCount && 
			   waitingIndex < waitingRequests.Count)
		{
			WWWRequest curRequest = waitingRequests[waitingIndex++];
			
			bool canStartDownload = curRequest.bundleData == null || isBundleDependenciesReady( curRequest.bundleData.name );
			if(canStartDownload)
			{
				curRequest.CreatWWW();
                processingRequest.Add(curRequest.url, curRequest);
                waitingRequests.Remove(curRequest);
			}
		}
	}
	
	bool isBundleDependenciesReady(string bundleName)
	{
		List<string> dependencies = getDependList(bundleName);

        if (dependencies == null) 
        {
            return false;
        }

		foreach(string dependBundle in dependencies)
		{
			string url = formatUrl(dependBundle + "." + bmConfiger.bundleSuffix);
			if(!succeedRequest.ContainsKey(url))
				return false;
		}
		
		return true;
	}

	void prepareDependBundles(string url)
	{
		string bundleName = Path.GetFileNameWithoutExtension(url);
		List<string> dependencies = getDependList(bundleName);
		foreach(string dependBundle in dependencies)
		{
			string dependUrl = formatUrl(dependBundle + "." + bmConfiger.bundleSuffix);
			if(succeedRequest.ContainsKey(dependUrl))
			{
				#pragma warning disable 0168
				var assertBundle = succeedRequest[dependUrl].www.assetBundle;
				#pragma warning restore 0168
			}
		}
	}
	
	// This private method should be called after init
	void download(WWWRequest request)
	{
		request.url = formatUrl(request.url);
		
		if(isDownloadingWWW(request.url) || succeedRequest.ContainsKey(request.url))
			return;
		
		if(isBundleUrl(request.url))
		{
			string bundleName = Path.GetFileNameWithoutExtension(request.url);
			if(!bundleDict.ContainsKey(bundleName))
			{
				Debug.LogError("Cannot download bundle [" + bundleName + "]. It's not in the bundle config.");
				return;
			}
			
			List<string> dependlist = getDependList(bundleName);
			foreach(string bundle in dependlist)
			{
				string bundleUrl = formatUrl(bundle + "." + bmConfiger.bundleSuffix);
				if(!processingRequest.ContainsKey(bundleUrl) && !succeedRequest.ContainsKey(bundleUrl))
				{
					WWWRequest dependRequest = new WWWRequest();
					dependRequest.bundleData = bundleDict[bundle];
					dependRequest.bundleBuildState = buildStatesDict[bundle];
					dependRequest.url = bundleUrl;
					dependRequest.priority = dependRequest.bundleData.priority;
					addRequestToWaitingList(dependRequest);
				}
			}
			
			request.bundleData = bundleDict[bundleName];
			request.bundleBuildState = buildStatesDict[bundleName];
			if(request.priority == -1)
				request.priority = request.bundleData.priority;  // User didn't change the default priority
			addRequestToWaitingList(request);
		}
		else
		{
			if(request.priority == -1)
				request.priority = 0; // User didn't give the priority
			addRequestToWaitingList(request);
		}
	}
	
	bool isInWaitingList(string url)
	{
		foreach(WWWRequest request in waitingRequests)
			if(request.url == url)
				return true;
		
		return false;
	}
	
	void addRequestToWaitingList(WWWRequest request)
	{
		if(succeedRequest.ContainsKey(request.url) || isInWaitingList(request.url))
			return;
		
		int insertPos = waitingRequests.FindIndex(x => x.priority < request.priority);
		insertPos = insertPos == -1 ? waitingRequests.Count : insertPos;
		waitingRequests.Insert(insertPos, request);
	}
	
	bool isDownloadingWWW(string url)
	{
		foreach(WWWRequest request in waitingRequests)
			if(request.url == url)
				return true;
		
		return processingRequest.ContainsKey(url);
	}
	
	bool isInBeforeInitList(string url)
	{
		foreach(WWWRequest request in requestedBeforeInit)
		{
			if(request.url == url)
				return true;
		}

		return false;
	}

	List<string> getDependList(string bundle)
	{
		if(!ConfigLoaded)
		{
			Debug.LogError("getDependList() should be call after download manger inited");
			return null;
		}
		
		List<string> res = new List<string>();

        bundle = ReplaceSlashWithBlackspace(bundle);
		if(!bundleDict.ContainsKey(bundle))
		{
			Debug.LogError("Cannot find parent bundle [" + bundle + "], Please check your bundle config.");
			return res;
		}
			
		while(bundleDict[bundle].parent != "")
		{
			bundle = bundleDict[bundle].parent;
			if(bundleDict.ContainsKey(bundle))
			{
				res.Add(bundle);
			}
			else
			{
				Debug.LogError("Cannot find parent bundle [" + bundle + "], Please check your bundle config.");
				break;
			}
		}
		
		res.Reverse();
		return res;
	}
	
	BuildPlatform getRuntimePlatform()
	{
		if(	Application.platform == RuntimePlatform.WindowsPlayer ||
			Application.platform == RuntimePlatform.OSXPlayer )
		{
			return BuildPlatform.Standalones;
		}
		else if(Application.platform == RuntimePlatform.OSXWebPlayer ||
				Application.platform == RuntimePlatform.WindowsWebPlayer)
		{
			return BuildPlatform.WebPlayer;
		}
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return BuildPlatform.IOS;
		}
		else if(Application.platform == RuntimePlatform.Android)
		{
			return BuildPlatform.Android;
		}
		else
		{
			Debug.LogError("Platform " + Application.platform + " is not supported by BundleManager.");
			return BuildPlatform.Standalones;
		}
	}
	
	void initRootUrl()
	{
		TextAsset downloadUrlText = (TextAsset)Resources.Load("Urls");
		BMUrls bmUrl = JsonMapper.ToObject<BMUrls>(downloadUrlText.text);

		if( Application.platform == RuntimePlatform.WindowsEditor ||
		   Application.platform == RuntimePlatform.OSXEditor )
		{
			curPlatform = bmUrl.bundleTarget;
		}
		else
		{
			curPlatform = getRuntimePlatform();
		}

		if(manualUrl == "")
		{
			string downloadPathStr;
			if(bmUrl.downloadFromOutput)
				downloadPathStr = bmUrl.GetInterpretedOutputPath(curPlatform);
			else
				downloadPathStr = bmUrl.GetInterpretedDownloadUrl(curPlatform);
				
			Uri downloadUri = new Uri(downloadPathStr);
			downloadRootUrl = downloadUri.AbsoluteUri;
		}
		else
		{
			string downloadPathStr = BMUtility.InterpretPath(manualUrl, curPlatform);
			Uri downloadUri = new Uri(downloadPathStr);
			downloadRootUrl = downloadUri.AbsoluteUri;
		}
        MLGame.MLLogger.LogDebug("[pak-bundle] downloadRootUrl=" + downloadRootUrl);
	}
	
	public string formatUrl(string urlstr)
	{
		Uri url;
		if(!isAbsoluteUrl(urlstr))
		{
            url = new Uri(new Uri(downloadRootUrl + "/" + LOADFOLDER), urlstr);
		}
		else
			url = new Uri(urlstr);
		
		return url.AbsoluteUri;
	}
	
	bool isAbsoluteUrl(string url)
	{
	    Uri result;
	    return Uri.TryCreate(url, System.UriKind.Absolute, out result);
	}
	
	bool isBundleUrl(string url)
	{
		return string.Compare(Path.GetExtension(url), "." + bmConfiger.bundleSuffix, System.StringComparison.OrdinalIgnoreCase)  == 0;
	}
	
	// Members
	List<BundleData> bundles = null;
	List<BundleBuildState> buildStates = null;
	BMConfiger bmConfiger = null;

    HashSet<string> processed = new HashSet<string>();
    Dictionary<string, WWW> DownloadedURLs = new Dictionary<string, WWW>();
    Dictionary<string, AssetBundle> DownloadedABs = new Dictionary<string, AssetBundle>();
	
	string downloadRootUrl = null;
	WWW curConfigWWW = null;
	BuildPlatform curPlatform;
	
	Dictionary<string, BundleData> bundleDict = new Dictionary<string, BundleData>();
	Dictionary<string, BundleBuildState> buildStatesDict = new Dictionary<string, BundleBuildState>();
	
	// Request members
	Dictionary<string, WWWRequest> processingRequest = new Dictionary<string, WWWRequest>();
	Dictionary<string, WWWRequest> succeedRequest = new Dictionary<string, WWWRequest>();
	Dictionary<string, WWWRequest> failedRequest = new Dictionary<string, WWWRequest>();
	List<WWWRequest> waitingRequests = new List<WWWRequest>();
	List<WWWRequest> requestedBeforeInit = new List<WWWRequest>();

	static DownloadManager instance = null;
	static string manualUrl = "";
	
	/**
	 * Get instance of DownloadManager.
	 * This prop will create a GameObject named Downlaod Manager in scene when first time called.
	 */ 
	public static DownloadManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("Downlaod Manager").AddComponent<DownloadManager> ();
				DontDestroyOnLoad(instance.gameObject);
			}
 
			return instance;
		}
	}

	public static void SetManualUrl(string url)
	{
		if(instance != null)
		{
			Debug.LogError("Cannot use SetManualUrl after accessed DownloadManager.Instance. Make sure call SetManualUrl before access to DownloadManager.Instance.");
			return;
		}

		manualUrl = url;
	}
	
	class WWWRequest
	{
		public string url = "";
		public int triedTimes = 0;
		public int priority = 0;
		public BundleData bundleData = null;
		public BundleBuildState bundleBuildState = null;
		public WWW www = null;

        // 记录性能的
        public float BeginTime;

        public void NotifyDone()
        {
            Debug.Log("[pak-bundle] www request is done.bundle=" + ((www == null || www.assetBundle == null) ? "none" : www.assetBundle.mainAsset.name) 
                    +  " begin=" + BeginTime + ",end=" + Time.time + ", cost-time=" + (Time.time - BeginTime));
        }

		public void CreatWWW()
		{	
			triedTimes++;
			
			if(true || DownloadManager.instance.bmConfiger.useCache && bundleBuildState != null)
				www = WWW.LoadFromCacheOrDownload(url, 0);//bundleBuildState.version);
			else
				www = new WWW(url);

            BeginTime = Time.time;
		}
	}
}