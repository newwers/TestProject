using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIController : MonoBehaviour {


    public UIContainer Container;

    private LineRenderTest lineRenderTest;

    private void Start()
    {
        lineRenderTest = Container.lineRenderTest;
        Container.PaintWidth_Slider.onValueChanged.AddListener(OnPaintWidthChange);
        Container.NewPointThreshold_Slider.onValueChanged.AddListener(OnNewPointSliderChange);

        Container.PaintWidthNumber_Text.text = (lineRenderTest.width * 10f).ToString();
        Container.NewPointthreshold_Text.text = (lineRenderTest.NewPointThresholdDistance * 10f).ToString();
    }

    private void OnNewPointSliderChange(float value)
    {
        Container.NewPointthreshold_Text.text = value.ToString();
        lineRenderTest.NewPointThresholdDistance = value * 0.1f;
    }

    private void OnPaintWidthChange(float value)
    {
        lineRenderTest.width = value * 0.1f;
        Container.PaintWidthNumber_Text.text = value.ToString();
    }
}
