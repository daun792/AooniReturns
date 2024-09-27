using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NoticePanel : UIBase
{
    [SerializeField] TextMeshProUGUI noticeTMP;
    private RectTransform noticeRect;

    private const string beforeGameStart = "잠시 뒤에 숙주 오니가 결정됩니다.\n서로 멀리 떨어지세요!";
    private const string countDown = "{0}초 남았습니다!";
    private const string becomeOni = "{0}님이 숙주오니가 되었습니다!";

    public override void Init()
    {
        noticeRect = noticeTMP.GetComponent<RectTransform>();

        noticeTMP.text = string.Empty;
    }

    private void PlayTMPAnim()
    {
        noticeRect.DOKill();

        noticeRect.DOScale(Vector3.zero, 0.5f).From();
    }

    public void NoticeBeforeGameStart()
    {
        base.OpenPanel();

        noticeTMP.text = beforeGameStart;
        PlayTMPAnim();
    }

    public void NoticeCountDown()
    {
        base.OpenPanel();

        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        int time = 10;
        while (time > 0)
        {
            noticeTMP.text = string.Format(countDown, time--);
            PlayTMPAnim();

            yield return new WaitForSeconds(1);
        }

        noticeTMP.text = string.Empty;
    }

    public void NoticeBecomeOni()
    {
        base.OpenPanel();

        noticeTMP.text = string.Format(becomeOni, 1);
        PlayTMPAnim();
    }
}
