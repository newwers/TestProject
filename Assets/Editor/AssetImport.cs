using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetImport :  AssetPostprocessor{


    /// <summary>
    /// 在音频资源被导入前调用
    /// </summary>
    void OnPreprocessAudio()
    {
        
        AudioImporter audioImporter = (AudioImporter)assetImporter;
        //关闭预加载
        audioImporter.preloadAudioData = false;
        Debug.Log("调用OnPreprocessAudio:" + assetImporter.assetPath);
    }

    /// <summary>
    /// 当音频被导入后
    /// </summary>
    public void OnPostprocessAudio (AudioClip audioClip)
    {
        //AudioImporter importer = (AudioImporter)assetImporter;

        Debug.Log("调用 OnPostprocessAudio:" +audioClip.name);
    }



    /// <summary>
    /// 在Texture被导入前调用
    /// </summary>
    void OnPreprocessTexture()
    {
        TextureImporter importer = assetImporter as TextureImporter;
        Debug.Log("OnPreprocessTexture: " + importer.assetPath);
        //设置导入的图片类型都为Sprite,而不是Default.可以根据导入的文件路径,来区分设置不同的图片类型,由于获取不到文件名,只能获取到导入的路径
        importer.textureType = TextureImporterType.Sprite;
        //测试时,name打印不出来
        //if (assetPath.Contains("_bumpmap"))
        //{
        //    TextureImporter textureImporter = (TextureImporter)assetImporter;
        //    textureImporter.convertToNormalmap = true;
        //}
    }

    #region 未测试,或没有被调用的函数



    /// <summary>
    /// 在模型被导入前调用
    /// </summary>
    void OnPreprocessModel()
    {
        Debug.Log("调用 OnPreprocessModel:" + assetImporter.name);
        //if (assetPath.Contains("@"))
        //{
        //    ModelImporter modelImporter = assetImporter as ModelImporter;
        //    modelImporter.importMaterials = false;
        //}
    }

    /// <summary>
    /// 当模型导入完成后
    /// </summary>
    /// <param name="g">The green component.</param>
    void OnPostprocessModel(GameObject g)
    {
        Debug.Log(g.name);
    }

    /// <summary>
    /// 当sprint被导入完成时
    /// </summary>
    /// <param name="texture">Texture.</param>
    /// <param name="sprites">Sprites.</param>
    void OnPostProcessSprites(Texture2D texture, Sprite[] sprites)
    {
        foreach (var item in sprites)
        {
            Debug.Log("Sprites: " + item.name);
        }
        Debug.Log("texture: " + texture.name);
    }

    /// <summary>
    /// 当Texture被导入完成时  (实际测试,导入png图片没有被调用)
    /// </summary>
    /// <param name="texture">Texture.</param>
    void OnPostProcessTexture(Texture2D texture)
    {
        Debug.Log("texture: " + texture.name);

    }

    /// <summary>
    /// 在模型的动画被导入前调用
    /// </summary>
    void OnPreprocessAnimation()
    {
        ModelImporter modelImporter = assetImporter as ModelImporter;
        //modelImporter.clipAnimations = modelImporter.defaultClipAnimations;
        Debug.Log(modelImporter.name);
    }

    /// <summary>
    /// 在资源被导入前调用
    /// </summary>
    void OnPreprocessAsset()
    {
        //if (assetImporter.importSettingsMissing)
        //{
        //    ModelImporter modelImporter = assetImporter as ModelImporter;
        //    if (modelImporter != null)
        //    {
        //        if (!assetPath.Contains("@"))
        //            modelImporter.importAnimation = false;
        //        modelImporter.importMaterials = false;
        //    }
        //}
        Debug.Log(assetImporter.name);
    }
    #endregion
}
