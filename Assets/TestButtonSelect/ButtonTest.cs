using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour {


    public Button _button;


	// Use this for initialization
	void Start () {
        _button.onClick.AddListener(() => {
            print("1");
            _button.Select();
        });

	}
	
	
}
