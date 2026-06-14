using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField, Range(0,120)] private float durationSeconds;
    private float current;
    private float percentage => current/durationSeconds;
    private bool timerEnd = false;
    private bool paused = true;
    public event Action<float> OnTimerUpdate;
    public event Action OnTimerEnd;

    void Awake()
    {
        current = durationSeconds;
    }

    void Update()
    {
        if (timerEnd || paused) return;
        
        current = Mathf.Clamp(current - Time.deltaTime, 0, durationSeconds);

        OnTimerUpdate?.Invoke(percentage);
        
        if (current <= 0) {
            timerEnd = true;
            OnTimerEnd.Invoke();
        }
    }

    public void AddTime(int seconds)
    {
        current = Mathf.Clamp(current + seconds, 0, durationSeconds);
    }

    public void Pause() => paused = true;
    public void Resume() => paused = false;
}