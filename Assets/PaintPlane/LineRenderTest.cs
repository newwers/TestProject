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
    /// <summary>
    /// 存放lineRender的位置
    /// </summary>
    private List<Vector3> lineRenderPositionList = new List<Vector3>();
    [Header("判断插值临界值的角度")]
    [Range(0f,180f)]
    public float ThresholdAngle;

    private void Start()
    {
        Container.ClearAllButton.onClick.AddListener(ClearAllLineRender);
    }


    // Update is called once per frame
    void Update () {

        Container.FPSText.text = (1f/Time.smoothDeltaTime).ToString("0");

        if (EventSystem.current.IsPointerOverGameObject())
        {
            //如果在UGUI上,返回不绘制,有问题,当鼠标按住从UI上移到画板上时,出现问题
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //记录鼠标坐标点,记录的是碰撞到plane上的点，而不是鼠标的位置
            startPoint = GetHitInfoPositon();

            //每次点击生成一个linerender物体
            GameObject go = CreateLineRenderPrefab();
            LineRenderGameObjects.Add(go);
            //向操作记录里面添加一条记录,每次添加一条记录后,就不能执行恢复操作了,恢复按钮需要灰态不可点击
            Container.Controller.AddStack(StackType.Create, go);
        }

        if (Input.GetMouseButton(0))
        {

            endPoint = GetHitInfoPositon();
            distance = Vector3.Distance(startPoint, endPoint);

            //再这边判断具体,
            if (distance > NewPointThresholdDistance)
            {
                #region 通过两点之间添加差值进行实现

                //进行差值计算的时候,在直线时,不需要进行差值,只有在拐角处进行差值

                if (CalcDrawLineAngle(ThresholdAngle))
                {
                    List<Vector3> posList = GetPosList(startPoint, endPoint, NewPointThresholdDistance);

                    lineRenderer.positionCount += posList.Count;
                    //获取之前的数组,加上新的插值的数组,然后设置到linerender上
                    lineRenderPositionList.AddRange(posList);

                    lineRenderer.SetPositions(lineRenderPositionList.ToArray());
                }
                else
                {
                    //加上一点
                    lineRenderPositionList.Add(endPoint);
                    lineRenderer.positionCount++;
                    lineRenderer.SetPositions(lineRenderPositionList.ToArray());
                }



                //将第二个点同步到第一个点
                startPoint = endPoint;

                #endregion



                #region 通过判断角度新增游戏物体的方式进行实现



                ////如果转角太大,重新生成一个lineRender
                //if (CalcDrawLineAngle(120f))
                //{
                //    //生成前,先删除掉自己lineRender最后第二个点的位置,
                //    Vector3 lastPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 2);
                //    lineRenderer.positionCount--;
                //    //生成lineRender,这边生成的游戏物体,应该作为上一个物体的子物体,不添加到list里面,跟随父物体被删除时一起删除
                //    GameObject go = CreateLineRenderPrefab();
                //    LineRenderGameObjects.Add(go);


                //    item.Index++;
                //    AddLineRenderPosition();
                //    //设置新创建的第一个点为上一个lineRender最后第二个点
                //    lineRenderer.SetPosition(0, lastPosition);
                //}

                #endregion
            }

        }

        //鼠标松开
        if (Input.GetMouseButtonUp(0))
        {
            //鼠标松开后,清空记录的坐标
            lineRenderPositionList.Clear();

            //在预支上加上boxcollider组件
            BoxCollider capsuleCollider = lineRenderer.gameObject.AddComponent<BoxCollider>();
        }
    }
    /// <summary>
    /// 创建一个lineRender组件的预制体
    /// </summary>
    /// <returns>返回改创建的游戏物体对象</returns>
    public GameObject CreateLineRenderPrefab()
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
        //print(hit.collider.name);
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



    public List<Vector3> GetPosList(Vector3 startPos,Vector3 endPos,float dis)
    {
        List<Vector3> posList = new List<Vector3>();
        Vector3 midPos = (startPos + endPos) / 2;
        if (Vector3.Distance(startPos, midPos) > dis)
        {
            List<Vector3> temp1 = this.GetPosList(startPos, midPos, dis);
            List<Vector3> temp2 = this.GetPosList(midPos, endPos, dis);
            for (int i = temp1.Count-1; i >=0;--i){
                posList.Insert(0,temp1[i]);
            }
            posList.AddRange(temp2);
        } else{
            //posList.Add(startPos);
            posList.Add(midPos);
            posList.Add(endPos);
        }
        return posList;
    } 

    /// <summary>
    /// 检测到达一定角度时,生成一个新的点
    /// </summary>
    /// <returns><c>true</c>, if draw line angle was calculated, <c>false</c> otherwise.</returns>
    public bool CalcDrawLineAngle(float minAngle = 165f,float MaxAngle = 180f)
    {
        if (lineRenderPositionList.Count >= 3)
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
                    //print(Angle);
                    return true;
                }
            }
        }

        return false;
    }



}
