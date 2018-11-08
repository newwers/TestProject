using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B : MonoBehaviour {

    private void Awake()
    {
        print("B.Awake");
        GameObject go = new GameObject();
        A a = go.AddComponent<A>();
        a.Start();

    }

    // Use this for initialization
    void Start () {
        print("B.Start");
    }
	
	
}
