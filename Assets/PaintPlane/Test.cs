using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public LineRenderTest LineRenderTest;

    public Stack<int> stack = new Stack<int>();

	// Use this for initialization
	void Start () {

        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        stack.Push(4);

        for (int i = 0; i < stack.Count; i++)
        {
            print(stack.Peek());
        }

        //while (stack.Count>0)
        //{
        //    print(stack.Pop());
        //}



    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(AAA());//78-82左右执行次数
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

    void TestLine()
    {
        List<Vector3> vector3s = LineRenderTest.GetPosList(new Vector3(3, 5, 0), new Vector3(7, 20, 0), 0.1f);

        GameObject go = LineRenderTest.CreateLineRenderPrefab();
        LineRenderer lineRenderer = go.GetComponent<LineRenderer>();
        lineRenderer.positionCount += vector3s.Count;
        lineRenderer.SetPositions(vector3s.ToArray());

        var temp = LineRenderTest.GetPosList(new Vector3(7, 20, 0), new Vector3(10, 13, 0), 0.1f);
        vector3s.AddRange(temp);
        lineRenderer.positionCount += temp.Count;
        lineRenderer.SetPositions(vector3s.ToArray());
    }
}
