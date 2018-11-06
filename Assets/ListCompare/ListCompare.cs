using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListCompare : MonoBehaviour {


    public List<string> s1 = new List<string>();
    public List<string> s2 = new List<string>();

	// Use this for initialization
	void Start () {
        s1.Add("E");
        s1.Add("#F");
        s1.Add("#G");
        s1.Add("A");

        s1.Sort();
        //s2.Add("C");
        //s2.Add("D");
        //s2.Add("E");
        //s2.Add("F");

        s2.Add("A");
        s2.Add("E");
        s2.Add("F#");
        s2.Add("#G");

        s2.Sort();

        bool eq = false;

        for (int i = 0; i < s1.Count; i++)
        {
            if (!s1[i].Equals(s2[i]))
            {
                eq = false;
                break;
            }
            else
            {
                eq = true;
            }
        }

        print(eq);
	}
	
	
}
