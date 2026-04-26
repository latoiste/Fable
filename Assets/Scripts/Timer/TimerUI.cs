using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField]
    private Timer timer;

    [SerializeField]
    private Slider slider;

    void Start()
    {
        if (timer == null) throw new Exception($"Attribute timer in {this} cannot be null");
        if (slider == null) throw new Exception($"Attribute slider in {this} cannot be null");

        timer.onTimerUpdate += UpdateSliderValue;
    }

    private void UpdateSliderValue(float value) => slider.value = value;
}
