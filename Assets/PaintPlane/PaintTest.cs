using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintTest : MonoBehaviour {

    public Image _image;
    public RawImage _rawImage;
    public Texture2D _texture;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Texture2D texture = CreateTexture(500, 300);
            byte[] pixels = new byte[500*300*4];
            for (int i = 0; i < 100; i++)
            {
                pixels[i * 4] = 1;
            }
            texture.LoadRawTextureData(pixels);
            _rawImage.texture = texture;
        }
	}

    private Texture2D CreateTexture(int width,int height)
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        //设置贴图模型
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;
        return texture;
    }
}
