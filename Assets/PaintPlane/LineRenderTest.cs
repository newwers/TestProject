using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineRenderTest : MonoBehaviour {

    [Header("lineRender组件的预制体游戏物体")]
    public GameObject LineRenderPrefab;
    /// <summary>
    /// 预制体上的lineRenderer对象
    /// </summary>
    private LineRenderer lineRenderer;
    private LineRenderItem item;

    Ray ray;
    RaycastHit hit;

    [Header("第一个点,用来计算距离")]
    private Vector3 startPoint;
    [Header("第二个点")]
    private Vector3 endPoint;
    [Header("生成新的点的临界值距离")]
    public float NewPointThresholdDistance = 0.1f;
    [Header("两点之间的距离")]
    public float distance;
    [Header("画笔的宽度")]
    public float width;
    [Header("画笔的颜色")]
    public Color color;
    [Header("存放所有LineRender对象")]
    public List<GameObject> LineRenderGameObjects = new List<GameObject>();

    public UIContainer Container;
    /// <summary>
    /// 最后绘制的三个点
    /// </summary>
    Vector3 p1;
    Vector3 p2;
    Vector3 p3;
    /// <summary>
    /// 由三个点计算出来的两个向量
    /// </summary>
    Vector3 v1;
    Vector3 v2;

    [Header("最后三个点计算的角度")]
    /// <summary>
    /// 最后三个点计算的角度
    /// </summary>
    public float Angle;

    private void Start()
    {
        Container.ClearAllButton.onClick.AddListener(ClearAllLineRender);
    }


    // Update is called once per frame
    void FixedUpdate () {

        Container.FPSText.text = (1f/Time.smoothDeltaTime).ToString("0");

        if (Input.GetMouseButtonDown(0))
        {
            //记录鼠标坐标点,记录的是碰撞到plane上的点，而不是鼠标的位置
            startPoint = GetHitInfoPositon();

            //每次点击生成一个linerender物体
            GameObject go = CreateLineRenderPrefab();
            LineRenderGameObjects.Add(go);
            item.Index++;

            AddLineRenderPosition();

        }

        if (Input.GetMouseButton(0))
        {

            endPoint = GetHitInfoPositon();
            distance = Vector3.Distance(startPoint, endPoint);

            //再这边判断具体,
            if (distance > NewPointThresholdDistance )
            {
                
                    item.Index++;
                    AddLineRenderPosition();

                    //将第二个点同步到第一个点
                    startPoint = endPoint;
                
                //如果转角太大,重新生成一个lineRender
                if (CalcDrawLineAngle(120f))
                {
                    //生成前,先删除掉自己lineRender最后第二个点的位置,
                    Vector3 lastPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 2);
                    lineRenderer.positionCount--;
                    //生成lineRender
                    GameObject go = CreateLineRenderPrefab();
                    LineRenderGameObjects.Add(go);
                    

                    item.Index++;
                    AddLineRenderPosition();
                    //设置新创建的第一个点为上一个lineRender最后第二个点
                    lineRenderer.SetPosition(0, lastPosition);
                }
            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            item.Index++;
            AddLineRenderPosition();
        }
    }
    /// <summary>
    /// 创建一个lineRender组件的预制体
    /// </summary>
    /// <returns>返回改创建的游戏物体对象</returns>
    private GameObject CreateLineRenderPrefab()
    {
        GameObject go = Instantiate(LineRenderPrefab);
        item = go.GetComponent<LineRenderItem>();
        lineRenderer = go.GetComponent<LineRenderItem>().lineRenderer;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        
        return go;

    }

    private void AddLineRenderPosition()
    {
        lineRenderer.positionCount++;

        GetHitInfoPositon();
        //print("Input.mousePosition:" + Input.mousePosition + ",vector3:" + hit.point);
        lineRenderer.SetPosition(item.Index-1, hit.point);
    }
    /// <summary>
    /// 发射射线进行碰撞检测,同时返回碰撞到的位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetHitInfoPositon()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        return hit.point;
    }

    public void ClearAllLineRender()
    {
        if (LineRenderGameObjects.Count > 0)
        {
            for (int i = 0; i < LineRenderGameObjects.Count; i++)
            {
                //这边如果频繁摧毁,可以考虑对象池
                Destroy(LineRenderGameObjects[i]);
            }
        }
    }

    /// <summary>
    /// 通过两个点计算距离后，如果大于threshold的5倍就再次进行中间加点,没用
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <param name="threshold"></param>
    public void AddTweenPoint(Vector3 startPoint,Vector3 endPoint,float threshold)
    {
        //计算出两个点的中间的点进行差值
        Vector3 midPoint = (startPoint + endPoint) / 2;
        if (Vector3.Distance(startPoint,midPoint) > threshold * 5)
        {
            //如果差值后，距离还是大于临界值的5倍,就继续重复此步操作
            AddTweenPoint(startPoint, endPoint, threshold);
            //添加点
            lineRenderer.positionCount++;
            item.Index++;
            lineRenderer.SetPosition(item.Index - 1, midPoint);
        }
        else
        {
            // 添加点
            lineRenderer.positionCount++;
            item.Index++;
            lineRenderer.SetPosition(item.Index - 1, midPoint);
        }
        //一个点分两段,都要进行计算
        if (Vector3.Distance(midPoint, endPoint) > threshold * 5)
        {
            //如果差值后，距离还是大于临界值的5倍,就继续重复此步操作
            AddTweenPoint(midPoint, endPoint, threshold);
            //添加点
            lineRenderer.positionCount++;
            item.Index++;
            lineRenderer.SetPosition(item.Index - 1, midPoint);
        }
        else
        {
            // 添加点
            lineRenderer.positionCount++;
            item.Index++;
            lineRenderer.SetPosition(item.Index - 1, midPoint);
        }
    }

    /// <summary>
    /// 检测到达一定角度时,生成一个新的点
    /// </summary>
    /// <returns><c>true</c>, if draw line angle was calculated, <c>false</c> otherwise.</returns>
    public bool CalcDrawLineAngle(float minAngle = 165f,float MaxAngle = 180f)
    {
        if (item.Index >= 3)
        {
            Vector3[] vector3s = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(vector3s);
            for (int i = 0; i < vector3s.Length; i++)
            {
                //取到最后的三个点当做向量计算角度
                p1 = vector3s[vector3s.Length - 1];
                p2 = vector3s[vector3s.Length - 2];
                p3 = vector3s[vector3s.Length - 3];

                v1 = p1 - p2;
                v2 = p2 - p3;

                Angle = Vector3.Angle(v1.normalized, v2.normalized);
                if (Angle >= minAngle && Angle <= MaxAngle)
                {
                    print(Angle);
                    return true;
                }
            }
        }

        return false;
    }



}
