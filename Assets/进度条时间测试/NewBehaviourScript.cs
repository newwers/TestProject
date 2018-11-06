using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {

    public float time1 = 46.123f;

    public float time2 = 77.58f;

    public Text Text1;
    public Text Text2;

	// Use this for initialization
	void Start () {
        //Text1.text = time1.ToString("0:00");//0:46
        //Text2.text = time2.ToString("0:00");//0:78

        Text1.text =  string.Format("{0}:{1:00}", (int)time1 / 60, time1 % 60);
        Text2.text = string.Format("{0}:{1:00}", (int)time2 / 60, time2 % 60);
	}
	
}
