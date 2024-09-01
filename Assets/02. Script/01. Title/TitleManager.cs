using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TitleManager : ViewManager
{
    private void Start()
    {
        App.Manager.Sound.PlayBGM("BGM_Title");
    }
}
