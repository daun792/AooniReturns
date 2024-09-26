using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimePanel : UIBase
{
    [SerializeField] TextMeshProUGUI timeTMP;
    [Tooltip("unit: Minute")] [SerializeField] float remainTime = 2f;

    private float intervalTime;
    private float givenTime;

    public float Remaining => remainTime;

    public (int, int) Elapsed
    {
        get
        {
            var elapsed = givenTime - remainTime;
            var min = Mathf.FloorToInt(elapsed / 60f);
            var sec = Mathf.FloorToInt(elapsed % 60f);
            return (min, sec);
        }
    }

    public override void Init()
    {
        remainTime *= 60f;
        givenTime = remainTime;
        intervalTime = Time.time;

        UpdateText();
    }

    private void Update()
    {
        if (Time.time - intervalTime < 1f) return;

        intervalTime = Time.time;
        remainTime--;

        UpdateText();
    }

    private void UpdateText()
    {
        int min = Mathf.FloorToInt(remainTime / 60f);
        int sec = Mathf.FloorToInt(remainTime % 60f);

        timeTMP.text = string.Format("{0:0}:{1:00}", min, sec);
    }
}
