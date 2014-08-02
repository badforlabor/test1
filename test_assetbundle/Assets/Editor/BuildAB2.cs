using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System;
using LitJson;


class BuildAB2
{
    static string LOADFOLDER = "iLingYou-j2/";
    static string postFix = ".assetbundle";


    public class BundleBuildState
    {
        public string bundleName = "";
        public int version = -1;
        public long size = -1;
        public long changeTime = -1;
        public string[] lastBuildDependencies = null;
    }
    public class BundleData
    {
        /**
         * Name of the bundle. The name should be uniqle in all bundles
         */
        public string name = "";

        /**
         * List of pathes included. The path can be directories.
         */
        public List<string> includs = new List<string>();

        /**
         * List of pathes currently depended. One asset can be depended but not included in the Includes list
         */
        public List<string> dependAssets = new List<string>();

        /**
         * Is this bundle a scene bundle?
         */
        public bool sceneBundle = false;

        /**
         * Default download priority of this bundle.
         */
        public int priority = 0;

        /**
         * Parent name of this bundle.
         */
        public string parent = "";

        /**
         * Childrens' name of this bundle
         */
        public List<string> children = new List<string>();
    }

    [Serializable]
    public class FileData
    {
        public string MD5;
        public int Version;
    }

    [Serializable]
    public class MLFileDataConfig
    {
        public Dictionary<string, FileData> MD5Files = new Dictionary<string, FileData>();
    }

    static public void saveObjectToJsonFile<T>(T data, string path)
    {
        TextWriter tw = new StreamWriter(path);
        if (tw == null)
        {
            Debug.LogError("Cannot write to " + path);
            return;
        }

        string jsonStr = JsonFormatter.PrettyPrint(JsonMapper.ToJson(data));

        tw.Write(jsonStr);
        tw.Flush();
        tw.Close();

        AssetDatabase.ImportAsset(path);
    }

    public static string GetMD5HashFromFile(string fileName)
    {
        try
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return "";
        }
    }

    static public List<string> LoadAllFilesAtPath(string path)
    {
        List<string> result = new List<string>();

        string[] fileEntries = Directory.GetFiles(path);
        foreach (string fileName in fileEntries)
        {
            // 记录相对Assets路径
            result.Add(fileName);
        }

        // 子目录
        string[] dirEntries = Directory.GetDirectories(path);
        foreach (string dir in dirEntries)
        {
            List<string> subDir = LoadAllFilesAtPath(dir);
            result.AddRange(subDir);
        }

        return result;
    }

    public static List<string> GetNeedBuildAsset(ref MLFileDataConfig md5Config)
    {
        //加载上一次的md5文件，没有也不用担心，会自动生成的,如果文件的位置变了，那就悲剧了,是否需要支持位置变了也OK，资源有重名咋弄？
        TextAsset file = AssetDatabase.LoadAssetAtPath("Assets/AssetBundleMD5.txt", typeof(TextAsset)) as TextAsset;
        if (false && file != null)
        {
            //md5Config = Util.DeserializeObject(file.bytes) as MLFileDataConfig;
        }

        if (md5Config == null)
        {
            md5Config = new MLFileDataConfig();
        }

        //计算MD5值
        List<string> Standby = new List<string>();
        List<string> fileEntries = LoadAllFilesAtPath(Application.dataPath + "/Resources/");//Resources/
        foreach (string dir in fileEntries)
        {
            //跳过meta文件
            int index = dir.IndexOf(".meta");
            if (index != -1)
            {
                continue;
            }

            //prefab文件的MD5可能没有变化，但是不代表他所引用的资源也没有改变
            Standby.Add(dir);

            string md5 = GetMD5HashFromFile(dir);
            FileData oldFileData = null;
            md5Config.MD5Files.TryGetValue(dir, out oldFileData);
            if (oldFileData == null
                || oldFileData.MD5 != md5)
            {
                //更新文件版本号
                FileData fd = null;
                if (md5Config.MD5Files.TryGetValue(dir, out fd))
                {
                    fd.MD5 = md5;
                    fd.Version = fd.Version++;
                }
                else//添加新文件MD5
                {
                    md5Config.MD5Files.Add(dir, new FileData() { MD5 = md5, Version = 1 });
                }

            }
        }

        return Standby;
    }

    static BuildTarget FetchBuildTarget()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        if (args.Length > 1)
        {
            if (args[1] == "0")//ios
            {
                return BuildTarget.iPhone;
            }
            else if (args[1] == "1")//android
            {
                return BuildTarget.Android;
            }
        }
        return BuildTarget.StandaloneWindows;
    }



    // 去掉后缀，比如".dll", ".cs", ".prefab"
    static string CutPostfix(string abc)
    {
        return abc.Substring(0, abc.LastIndexOf("."));
    }
    const string prefix = "Assets/Resources/";
    // 转化为相对路径
    static string CutPrefix(string abc)
    {
        int idx = (abc.IndexOf(prefix));
        if (idx != -1)
        {
            return abc.Substring(idx).Replace("\\", "/");
        }
        else
        {
            return abc;
        }
    }
    static List<string> ProcessDep(string me, string[] deps)
    {
        List<string> ret = new List<string>();
        foreach (var dep in deps)
        {
            if (dep == me)
                continue;
            if (FilterFile(dep))
                continue;
            ret.Add(dep);
        }
        return ret;
    }
    // 将"/"转化为" "
    static string ReplaceSlashWithBlackspace(string abc)
    {
        return abc.Replace('/', '-').Replace('\\', '-');
    }
    // 过滤一些非法的文件，比如“cs”, "dll", "meta"
    static bool FilterFile(string abc)
    {
        if (string.IsNullOrEmpty(abc))
        {
            return true;
        }

        string[] ext = new string[] { ".cs", ".dll", ".meta" };
        foreach (string e in ext)
        {
            if (abc.EndsWith(e))
            {
                return true;
            }
        }

        // 包含空格也不行！
        if (false && abc.Contains(" "))
        {
            return true;
        }

        return false;
    }

    static BundleData Find(List<BundleData> config1, string abc)
    {
        if (config1 == null)
        {
            return null;
        }
        foreach (var conf1 in config1)
        {
            if (conf1.name == abc)
            {
                return conf1;
            }
        }
        return null;
    }

    static string GetBundleName(string str)
    {
        string str2 = str.ToLower();
        const string prefix = "assets/resources/";
        if (str2.StartsWith(prefix))
        {
            str2 = str2.Substring(prefix.Length);
            str2 = str2.Substring(0, str2.LastIndexOf("."));
            return str2;
        }
        else
        {
            return null;
        }
    }


    static void PakAssetBundleRecursively(HashSet<string> AllDepends, string outputPath, ref HashSet<string> processed, ref List<BundleData> config1, ref int cnt, string leaf, ref HashSet<BundleData> parents)
    {
        if (processed.Contains(leaf))
        {
            return;
        }

        processed.Add(leaf);

        // 处理叶子自身，先push，在build，pop放在最最外层
        BuildPipeline.PushAssetDependencies();
        cnt++;

        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(leaf, typeof(UnityEngine.Object));
        if (obj)// && ref1.name.ToLower().EndsWith("skill_skill.csv"))
        {
            string explicit_name = null;

            if (false)//PrefabUtility.GetPrefabType(obj) != PrefabType.None)
            {
                explicit_name = GetBundleName(leaf);
                Debug.Log("[pak-bundle] explicit name=" + explicit_name);
            }
            if (string.IsNullOrEmpty(explicit_name))// && obj is UnityEngine.GameObject)
            {
                if (true)//ref1.name.ToLower().Contains("/config/"))
                {
                    Debug.Log("[pak-anti] process :" + obj.name + ", leaf=" + leaf);
                    BuildPipeline.BuildAssetBundle(obj, null, outputPath + ReplaceSlashWithBlackspace(leaf) + postFix,
                                        BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.UncompressedAssetBundle,
                                        BuildTarget.StandaloneWindows);
                    //BuildPipeline.BuildAssetBundle(obj, null, outputPath + ReplaceSlashWithBlackspace(ref1.name) + postFix,
                    //                    BuildAssetBundleOptions.CollectDependencies
                    //                        | BuildAssetBundleOptions.DeterministicAssetBundle 
                    //                        | BuildAssetBundleOptions.UncompressedAssetBundle,
                    //                    BuildTarget.StandaloneWindows);
                }
                else
                {
                    Debug.Log("[pak-anti] process :" + obj.name);
                }
            }
            else
            {
                // resource下的文件，都以全称的形式指定吧
                UnityEngine.Object[] objs = new UnityEngine.Object[1];
                objs[0] = obj;
                string[] obj_names = new string[1];
                obj_names[0] = explicit_name;
                BuildPipeline.BuildAssetBundleExplicitAssetNames(objs, obj_names, outputPath + ReplaceSlashWithBlackspace(leaf) + postFix);
            }
            //Resources.UnloadUnusedAssets();

            // 如果发现自己是根节点，那么pop一下，节省内存
            obj = null;
            System.GC.Collect();
        }

        // 获取所有依赖于leaf的，准备处理，不过处理之间先断绝父子关系！这样父亲变成孤独老人有相当于叶子了！
        List<BundleData> AntiDep = new List<BundleData>();
        bool bfind = false;
        foreach (var conf1 in config1)
        {
            if (conf1.dependAssets.Contains(leaf))
            {
                bfind = true;
                conf1.dependAssets.Remove(leaf);
                AntiDep.Add(conf1);
            }
        }
        if (!AllDepends.Contains(leaf))
        {
            System.Diagnostics.Debug.Assert(bfind == false);
            cnt--;
            BuildPipeline.PopAssetDependencies();
        }

        // 有正在处理的，就不处理了
        if (parents != null)
        {
            foreach (var pa in parents)
                AntiDep.Remove(pa);
        }

        // 没有人依赖自己，自己也不依赖其他人，表示自己孤独了，那么自毁吧！
        //RemoveBundleData(ref config1, leaf);

        // 处理AntiDep
        AntiDep.Sort(delegate(BundleData a, BundleData b)
        {
            if (a.dependAssets.Count == 0)
                return -1;
            else if (b.dependAssets.Count == 0)
                return 1;
            else
                return 0;
        });

        // 标记自己正在被处理
        foreach (var anti in AntiDep)
        {
            parents.Add(anti);
        }

        foreach (var anti in AntiDep)
        {
            ProcessAntiDep(AllDepends, outputPath, ref processed, ref config1, ref cnt, anti, ref parents);
        }
    }


    static void RemoveBundleData(ref List<BundleData> config1, string me)
    {
        foreach (var conf1 in config1)
        {
            if (conf1.name == me)
            {
                config1.Remove(conf1);
                return;
            }
        }
    }
    static BundleData GetBundleData(List<BundleData> config1, string dep)
    {
        foreach (var conf1 in config1)
        {
            if (conf1.name == dep)
            {
                return conf1;
            }
        }
        return null;
    }
    static void ProcessAntiDep(HashSet<string> AllDepends, string outputPath, ref HashSet<string> processed, ref List<BundleData> config1, ref int cnt, BundleData anti, ref HashSet<BundleData> parents)
    {
        if (anti == null || processed.Contains(anti.name))
        {
            return;
        }

        // 先处理它依赖的，处理完毕后，自己就变成了叶子节点，就可以处理自己了
        List<string> deps = new List<string>();
        deps.AddRange(anti.dependAssets);
        foreach (var dep in deps)
        {
            ProcessAntiDep(AllDepends, outputPath, ref processed, ref config1, ref cnt, GetBundleData(config1, dep), ref parents);
        }

        PakAssetBundleRecursively(AllDepends, outputPath, ref processed, ref config1, ref cnt, anti.name, ref parents);
        parents.Remove(anti);
    }
    [MenuItem("AssetBundle/打包2")]
    static void BuildAssetBundle2()
    {
        // string[] args = System.Environment.GetCommandLineArgs();

        bool bdo = EditorUtility.DisplayDialog("打散包", "确定要开始打散包吗？需要40分钟哦！", "确定", "取消");
        if (!bdo)
        {
            return;
        }

        BuildAssetBundle_auto2();
    }
    static void BuildAssetBundle_auto2()
    {

        Debug.Log("[pak-bundle] ***** begin build assetbundle!");

        Single startTime = Time.realtimeSinceStartup;
        Single curTime = Time.realtimeSinceStartup;

        int index = Application.dataPath.IndexOf("Assets");
        string basePath = Application.dataPath.Substring(0, index) + "AssetBundles/";
        //string basePath = Application.streamingAssetsPath;
        string outputPath = basePath + "/Standalones/" + LOADFOLDER;
        string outputPath_conf = outputPath;// basePath + "/Standalones/";

        Directory.CreateDirectory(outputPath);
        Debug.Log("[pak-bundle] ***** output dir=" + outputPath);

        //BuildTarget bt = BuildTarget.StandaloneWindows;

        // 收集要打包的文件
        curTime = Time.realtimeSinceStartup;
        Debug.Log("[pak-bundle] ***** step1. collect files.");
        MLFileDataConfig md5Config = null;
        List<string> Standby = GetNeedBuildAsset(ref md5Config);
        if (Standby.Count == 0)
        {
            Debug.Log("[pak-bundle] no files!");
            return;
        }
        Debug.Log("[pak-bundle] ***** step1. collect files. time-cost=" + (Time.realtimeSinceStartup - curTime));

        // 即将生成config1文件，json类型
        // 收集信息
        List<BundleData> config1 = new List<BundleData>();
        List<BundleBuildState> config2 = new List<BundleBuildState>();
        string[] pathNames = new string[1];
        HashSet<string> processedFiles = new HashSet<string>();
        List<string> dependonFiles = new List<string>();
        HashSet<string> AllDepends = new HashSet<string>();

        curTime = Time.realtimeSinceStartup;
        Debug.Log("[pak-bundle] ***** step2. collect config information.");
        foreach (var sb in Standby)
        {
            // 过滤掉一些非法的
            if (FilterFile(sb))
            {
                continue;
            }
            // 将形如"F:\war\shared\02-Editor\Assets\Resources\urls.txt"的转化为"Assets\Resources\urls.txt"
            string sb1 = CutPrefix(sb);

            if (processedFiles.Contains(sb1))
                continue;
            else
                processedFiles.Add(sb1);


            string sb2 = sb1;// CutPostfix(sb1);

            BundleData bd = new BundleData();
            bd.name = sb2;  // 将斜杠“/”转化为空格“ ”
            bd.includs.Add(sb1);    // 自己的全路径名
            pathNames[0] = sb1;
            if (true)
            {
                string[] deps = AssetDatabase.GetDependencies(pathNames);
                deps = ProcessDep(sb1, deps).ToArray();
                dependonFiles.AddRange(deps);
                bd.dependAssets.AddRange(deps);
            }
            config1.Add(bd);

            BundleBuildState bs = new BundleBuildState();
            bs.bundleName = ReplaceSlashWithBlackspace(sb1);
            bs.size = (new System.IO.FileInfo(sb)).Length;
            config2.Add(bs);
        }

        do
        {
            Debug.Log("[pak-bundle] dependon files, length=" + dependonFiles.Count);
            List<string> dependonFiles2 = new List<string>();
            foreach (var dep in dependonFiles)
            {
                AllDepends.Add(dep);
            }

            foreach (var sb in dependonFiles)
            {
                string sb1 = (sb);

                if (processedFiles.Contains(sb))
                    continue;
                else
                    processedFiles.Add(sb);


                string sb2 = sb1;// CutPostfix(sb1);

                BundleData bd = new BundleData();
                bd.name = sb2;  // 将斜杠“/”转化为空格“ ”
                bd.includs.Add(sb1);    // 自己的全路径名
                pathNames[0] = sb1;
                string[] deps = AssetDatabase.GetDependencies(pathNames);
                deps = ProcessDep(sb1, deps).ToArray();
                dependonFiles2.AddRange(deps);
                //if (deps.Length > 0)
                //{
                //    Debug.LogError("[pak-bundle]  build asset bundle");
                //}

                bd.dependAssets.AddRange(deps);
                config1.Add(bd);

                BundleBuildState bs = new BundleBuildState();
                bs.bundleName = ReplaceSlashWithBlackspace(sb1);
                bs.size = (new System.IO.FileInfo(sb)).Length;
                config2.Add(bs);
            }
            dependonFiles = dependonFiles2;
        }
        while (dependonFiles != null && dependonFiles.Count > 0);

        Debug.Log("[pak-bundle] ***** step2. collect config information. time-cost=" + (Time.realtimeSinceStartup - curTime));
        //return;

        // 将config1序列化到文件中。json格式

        List<BundleData> config11 = new List<BundleData>();
        List<BundleBuildState> config22 = new List<BundleBuildState>();
        foreach (var conf1 in config1)
        {
            if (conf1.name.StartsWith("Assets/Resources/"))
            {
                config11.Add(conf1);
            }
        }
        foreach (var conf2 in config2)
        {
            if (conf2.bundleName.StartsWith("Assets-Resources-"))
            {
                config22.Add(conf2);
            }
        }

        Debug.Log("[pak-bundle] config11.length=" + config11.Count + ", config22.length=" + config22.Count);
        Debug.Log("[pak-bundle] config1.length=" + config1.Count + ", config2.length=" + config2.Count);

        curTime = Time.realtimeSinceStartup;
        Debug.Log("[pak-bundle] ***** step3. write file, config information.");
        saveObjectToJsonFile(config1, outputPath_conf + "BundleData.txt");
        saveObjectToJsonFile(config2, outputPath_conf + "BuildStates.txt");
        Debug.Log("[pak-bundle] ***** step3. write file, config information. time-cost=" + (Time.realtimeSinceStartup - curTime));

        //return;

        // 将config1中的文件，全部单独做BuildAssetBundle
        // 注意   文件夹固定为:AssetBundles\Standalones
        //        文件名为："prefab particleEffect baodian"而不是"prefab/particleEffect/baodian"
        // 制作过程中，另一个策略是，先处理不依赖的包，再处理依赖的包
        curTime = Time.realtimeSinceStartup;
        Debug.Log("[pak-bundle] ***** step4. write file, all assetbundle file.");

        #region method-2
#if true
        Dictionary<string, HashSet<string>> table = new Dictionary<string, HashSet<string>>();
        HashSet<string> processed = new HashSet<string>();
        List<BundleData> config1_bak = new List<BundleData>();

        // 保存一个备份，之后要破坏config1
        config1_bak.AddRange(config1);

        int totalcount = 0;
        foreach (var conf1 in config1)
        {
            if (processed.Contains(conf1.name))
            {
                continue;
            }
            int cnt = 0;
            // 找到一个只有被依赖，没有依赖的，即叶子
            if (conf1.dependAssets.Count == 0)
            {
                HashSet<BundleData> parents = new HashSet<BundleData>();
                PakAssetBundleRecursively(AllDepends, outputPath, ref processed, ref config1, ref cnt, conf1.name, ref parents);

                System.Diagnostics.Debug.Assert(parents.Count == 0);
                totalcount += cnt;

                while (cnt > 0)
                {
                    BuildPipeline.PopAssetDependencies();
                    cnt--;
                }

                continue;
            }
        }

        Debug.Log("total count=" + totalcount);

#endif
        #endregion

        #region method-1
#if false      
            HashSet<string> processed = new HashSet<string>();

            List<BundleData> finalproc = new List<BundleData>();
            foreach (var conf1 in config1)
            {
                // 只处理没有任何人依赖于它的
                bool bfind = AllDepends.Contains(conf1.name);

                if (!bfind)
                {
                    finalproc.Add(conf1);
                }
            }


            int max_cnt = 10;
            foreach (var conf1 in finalproc)
            {
                int cnt = 0;
                PakAssetBundleRecursively(conf1, config1, ref processed, outputPath, ref cnt);
                while (cnt > 0)
                {
                    cnt--;
                    BuildPipeline.PopAssetDependencies();
                }

                // 测试用
                if (false && conf1.dependAssets.Count > 0)
                {
                    max_cnt--;
                    if (max_cnt == 0)
                        break;
                }
            }
#endif
        #endregion

        Debug.Log("[pak-bundle] ***** step4. write file, all assetbundle file. time-cost=" + (Time.realtimeSinceStartup - curTime));
        Debug.Log("[pak-bundle] ***** total time-cost=" + (Time.realtimeSinceStartup - startTime));

    }





}

