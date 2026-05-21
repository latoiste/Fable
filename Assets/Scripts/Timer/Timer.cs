using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField, Range(0,120)] private float durationSeconds;
    private float current;
    private float percentage => current/durationSeconds;
    private bool timerEnd = false;
    public Action<float> onTimerUpdate;

    void Awake()
    {
        current = durationSeconds;
    }

    void Update()
    {
        if (timerEnd) return;
        
        current = Mathf.Clamp(current - Time.deltaTime, 0, durationSeconds);

        onTimerUpdate?.Invoke(percentage);
        
        if (current <= 0) timerEnd = true;
    }

    public void Pause() => Time.timeScale = 0f;
    public void Resume() => Time.timeScale = 1f;
}