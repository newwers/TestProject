using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 堆存放操作的类型
/// </summary>
public enum StackType
{
    /// <summary>
    /// 创建物体
    /// </summary>
    Create,
    /// <summary>
    /// 移动物体
    /// </summary>
    Move,
    /// <summary>
    /// 删除物体
    /// </summary>
    Delete
}

[Serializable]
public class StackClass{
    /// <summary>
    /// 操作的类型
    /// </summary>
    public StackType stackType;
    /// <summary>
    /// 操作的游戏对象
    /// </summary>
    public GameObject gameObject;
    /// <summary>
    /// 物体移动后的上一个位置,在物体移动的时候需要记录
    /// </summary>
    public Vector3 previousPosition;
}

public class UIController : MonoBehaviour {
    


    public UIContainer Container;

    private LineRenderTest lineRenderTest;

    /// <summary>
    /// 存放操作的堆,根据操作类型和对应的游戏物体进行操作
    /// </summary>
    public List<StackClass> stack=new List<StackClass>();
    /// <summary>
    /// 记录操作指向的索引
    /// </summary>
    public int stackIndex = 0;

    private void Start()
    {
        lineRenderTest = Container.lineRenderTest;
        Container.PaintWidth_Slider.onValueChanged.AddListener(OnPaintWidthChange);
        Container.NewPointThreshold_Slider.onValueChanged.AddListener(OnNewPointSliderChange);
        Container.AngleThreshold_Slider.onValueChanged.AddListener(OnAngleSliderChange);
        //更新UI，同时会触发对应的change事件
        Container.PaintWidth_Slider.value = lineRenderTest.width * 10f;
        Container.NewPointThreshold_Slider.value = lineRenderTest.NewPointThresholdDistance * 10f;
        Container.AngleThreshold_Slider.value = Container.lineRenderTest.ThresholdAngle;

        //撤销和恢复按钮
        Container.UndoButton.onClick.AddListener(OnUndoButtonClick);
        Container.RedoButton.onClick.AddListener(OnRedoButtonClick);
    }

    /// <summary>
    /// 恢复按钮
    /// </summary>
    private void OnRedoButtonClick()
    {
        //恢复按钮
        //就是根据当前操作到的索引加1
        //还需要判断索引是否到最后了
        //还有就是撤销后进行修改,那么恢复按钮就没用了
        //无法恢复或者无法撤销的时候按钮需要灰态
        if (stackIndex < stack.Count)
        {
            Redo(stack[stackIndex]);
        }
        else
        {
            //当操作操作记录到最后时,恢复按钮不可点击
            Container.RedoButton.interactable = false;
        }

    }
    /// <summary>
    /// 撤销按钮
    /// </summary>
    private void OnUndoButtonClick()
    {
        //撤销按钮的实现需要自身维护一个队列来供操作,
        //当进行的操作例如画笔绘画和音符创建和移动后,
        //需要将这些操作都放到这个队列中
        //当进行撤销时,后进的先出的操作进行
        //当撤销时如果又进行了例如画笔绘制操作,那么无法恢复
        if (stackIndex >= 1 && stackIndex <= stack.Count)
        {
            Undo(stack[stackIndex - 1]);
            print("操作的索引为:" + stackIndex);
        }
        else
        {
            //当操作操作记录到第一个时,撤销按钮不可点击
            Container.UndoButton.interactable = false;
        }

    }
    /// <summary>
    /// 撤销实现函数,将需要
    /// </summary>
    /// <param name="stackClass">操作实体类型</param>
    private void Undo(StackClass stackClass)
    {
        //根据类型进行反向操作
        switch (stackClass.stackType)
        {
            case StackType.Create:
                //如果撤销的试创建物体的操作,那么就删除这个物体,不进行删除,而是进行隐藏
                //Destroy(stackClass.gameObject);
                stackClass.gameObject.SetActive(false);
                break;

            case StackType.Move:
                //如果是移动,那么撤销后需要获取到上一个物体的位置
                stackClass.gameObject.transform.position = stackClass.previousPosition;
                break;

            case StackType.Delete:
                //如果是撤销删除,那么就在原本的位置生成出来,

                break;
        }
        //操作索引减少
        stackIndex--;
        //只有撤销操作后,才可以点击恢复按钮
        Container.RedoButton.interactable = true;
    }

    private void Redo(StackClass stackClass)
    {
        //根据类型进行重新实现
        switch (stackClass.stackType)
        {
            case StackType.Create:
                //如果是创建物体的操作,
                //Instantiate(stackClass.gameObject, stackClass.gameObject.transform.position, stackClass.gameObject.transform.rotation);
                stackClass.gameObject.SetActive(true);
                break;

            case StackType.Move:
                //如果是移动
                stackClass.gameObject.transform.position = stackClass.gameObject.transform.position;
                break;

            case StackType.Delete:
                //如果是恢复删除,那么就摧毁这个物体

                break;
        }
        //操作索引+1
        stackIndex++;
    }

    /// <summary>
    /// 角度变化slider改变时
    /// </summary>
    /// <param name="value">Value.</param>
    private void OnAngleSliderChange(float value)
    {
        Container.AngleThreshold_Text.text = value.ToString();
        Container.lineRenderTest.ThresholdAngle = value;
    }
    /// <summary>
    /// 新的点产生的距离改变的slider
    /// </summary>
    /// <param name="value">Value.</param>
    private void OnNewPointSliderChange(float value)
    {
        Container.NewPointthreshold_Text.text = value.ToString();
        lineRenderTest.NewPointThresholdDistance = value * 0.1f;
    }
    /// <summary>
    /// 画笔大小slider改变时
    /// </summary>
    /// <param name="value">Value.</param>
    private void OnPaintWidthChange(float value)
    {
        lineRenderTest.width = value * 0.1f;
        Container.PaintWidthNumber_Text.text = value.ToString();
    }

    /// <summary>
    /// 向堆栈记录里面,添加一条操作,每次添加一条记录后,就不能执行恢复操作了,恢复按钮需要灰态不可点击
    /// </summary>
    /// <param name="stackType">Stack type.</param>
    /// <param name="go">Go.</param>
    /// <param name="previousPosition">Previous position.</param>
    public void AddStack(StackType stackType,GameObject go,Vector3 previousPosition =default(Vector3))
    {
        StackClass stackClass = new StackClass();
        stackClass.gameObject = go;
        stackClass.stackType = stackType;
        //struct数据类型的默认值,通过Default进行给默认值
        stackClass.previousPosition = previousPosition;
        //如果堆栈中存在对应索引的数值,那么覆盖过去
        if (stack.Count > stackIndex )
        {
            Destroy(stack[stackIndex].gameObject);
            stack[stackIndex] = stackClass;
        }
        else
        {
            //否则就添加
            stack.Add(stackClass);
        }

        stackIndex++;
        print("当前索引为:" + stackIndex);

        //每次添加操作记录后,恢复按钮都不可点击
        Container.RedoButton.interactable = false;
        Container.UndoButton.interactable = true;
    }
}
