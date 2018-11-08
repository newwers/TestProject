using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContainer : MonoBehaviour {


    public Button ClearAllButton;
    public Text FPSText;

    [Header("画笔宽度")]
    public Slider PaintWidth_Slider;
    public Text PaintWidthNumber_Text;

    [Header("点生成的临界值")]
    public Slider NewPointThreshold_Slider;
    public Text NewPointthreshold_Text;

    [Header("转角处角度判断临界值")]
    public Slider AngleThreshold_Slider;
    public Text AngleThreshold_Text;

    [Header("撤销和恢复按钮")]
    public Button UndoButton;
    public Button RedoButton;

    [Header("音符图片组件")]
    public Image[] NoteImages;

    public Color PaintColor;

    public LineRenderTest lineRenderTest;

    public UIController Controller;




}
