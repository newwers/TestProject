using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(AAA());
        }
    }

    IEnumerator AAA()
    {
        int index = 0;
        float _time = 0f;
        _time = Time.time;
        while (Time.time - _time <=1f)
        {

            index++;
            yield return null;
        }
        print(index);
        _time = Time.time;
        index = 0;

    }
}
