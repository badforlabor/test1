using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
/**
角色方向：
    前
    左
    右
    后
    左前
    右前
    左后
    右后
 
 */

public class auto_anim{

    [MenuItem("liubo/自动anim")]
    static void AutoAnimFor2dSprite()
    {
        float anim_time = 1.0f; // 1秒
        int frame = 8;          // 8帧
        string spritePath = "Assets/art/战将之魂变身_W1.png";
        string folder = "role1_anim/";
        string auto_anim_name = "auto_idle_";

        UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(spritePath);
        List<Sprite> sprites = new List<Sprite>();

        for (int i = 0; i < objs.Length; i++)
        {
            Debug.Log("image:" + objs[i].name + ", type=" + objs[i].GetType().Name);
        }

        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i] is Sprite)
            {
                sprites.Add(objs[i] as Sprite);
            }
        }

        if (sprites.Count == 0 || sprites.Count % frame != 0)
        {
            Debug.LogError("中断了，贴图的数量不对，总数是：" + sprites.Count + ", 每个动画的数量：" + frame);
            return;
        }


        //动画长度是按秒为单位，1/10就表示1秒切10张图片，根据项目的情况可以自己调节
        float frameTime = anim_time / frame;
        int anim_count = sprites.Count / frame;
        for (int j = 0; j < anim_count; j++)
        {
            string anim_name = auto_anim_name + j;        

            // 自动创建anim文件。
            AnimationClip clip = new AnimationClip();
            AnimationUtility.SetAnimationType(clip, ModelImporterAnimationType.Generic);
            EditorCurveBinding curveBinding = new EditorCurveBinding();
            curveBinding.type = typeof(SpriteRenderer);
            curveBinding.path = "";
            curveBinding.propertyName = "m_Sprite";

            // 创建帧数据
            ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[frame];
            for (int i = 0; i < keyFrames.Length; i++)
            {
                Sprite sprite = sprites[j * frame + i] as Sprite;
                keyFrames[i] = new ObjectReferenceKeyframe();
                keyFrames[i].time = frameTime * i;
                keyFrames[i].value = sprite;
            }

            //动画帧率，30比较合适
            clip.frameRate = 30;

            AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);

            Directory.CreateDirectory("Assets/art/" + folder);
            AssetDatabase.CreateAsset(clip, "Assets/art/" + folder + anim_name + ".anim");
        }
        
        AssetDatabase.SaveAssets();

    }

}
