using System;
using System.Collections;
using Challenge;
using UnityEngine;
using UnityEngine.UI;

public class QuestTimer : MonoBehaviour, ITimer
{
    [SerializeField] private Challenge_UI _challengeUI;
    private TimeSpan target;

    [Header("Timer_Text")]
    [SerializeField] private Text timerText;

    private void Awake()
    {
        Set_TargetTime();
        StartCoroutine(Timer());
    }

    public IEnumerator Timer()
    {
        while (true)
        {
            var delta_target = target - TimeSpan.FromSeconds(1);
            timerText.text = delta_target.ToString(@"h\:mm\:ss");
            target -= TimeSpan.FromSeconds(1);
            if (delta_target < TimeSpan.FromSeconds(1))
                break;
                
            yield return new WaitForSecondsRealtime(1.0f);
        }
        Action();
    }

    public void Action()
    {
        _challengeUI.Timer_Reset();
        Set_TargetTime();
        PlayerPrefs.SetInt("chal_count", 1);
        StartCoroutine(Timer());
    }

    public void Set_TargetTime()
    {
        DateTime _dateTime_now = DateTime.UtcNow;
        TimeSpan delte = new TimeSpan(_dateTime_now.Hour, _dateTime_now.Minute, _dateTime_now.Second);
        target = DateTime.Today.AddDays(1).Subtract(DateTime.UtcNow);
    }
}
