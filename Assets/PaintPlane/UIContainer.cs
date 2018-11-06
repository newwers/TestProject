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

    public Color PaintColor;

    public LineRenderTest lineRenderTest;

}
