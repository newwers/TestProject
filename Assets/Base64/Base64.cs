using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Base64 : MonoBehaviour {

    public string signature = "eyJzaWQiOiIkMnkkMTAkNnVBYlYzbmlEVVNkUlFXaDNQRlNFLmIzQ0ZDb1pNRUZCRnYzeHp4V0pXUlhvb2tCVHh3Q3kiLCJleHBpcmVkX2luIjoxNTM5NzU5NzQwLCJrZXkiOiJDRUQ4QUFBMkY4RjkxQTBBNzY0MjJDMTk2N0QwMkUwMCJ9";
    public string signature2 = "MUFCMDRDRDZEQTE0MjRBMjAyMDgwRDU0QkE4MjU2Mjk=";
	// Use this for initialization
	void Start () {
        
        string result2 = Base64Decode(signature);
        print("解密完:" + result2);
        //string encode = Base64Encode(result);
        //print("解密再加密后:" + encode);
        //string encode1 = Base64Encode(signature);
        //print("直接加密后:" + encode1);

        string result = Base64Decode(signature2);
        print("解密完:" + result);
	}
	
	
    /// <summary>
    /// base64解码,默认UTF-8格式
    /// </summary>
    /// <param name="signature">Signature.</param>
    public string Base64Decode(string signature)
    {
        string decode = "";
        //先将base64格式的字符串转换成字节数组
        byte[] bytes = Convert.FromBase64String(signature);
        //再将字节数组通过utf-8转化为字符串
        decode = Encoding.UTF8.GetString(bytes);
        return decode;
    }

    /// <summary>
    /// base64加密
    /// </summary>
    /// <returns>The encode.</returns>
    /// <param name="signature">Signature.</param>
    public string Base64Encode(string signature)
    {
        string encode = "";
        //先将字符串通过utf-8格式转化成字节数组
        byte[] bytes = Encoding.UTF8.GetBytes(signature);
        //再将字节数组转化成ba64的字符串
        encode = Convert.ToBase64String(bytes);
        return encode;
    }
}
