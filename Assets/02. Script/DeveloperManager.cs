using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperManager : MonoBehaviour
{
    [SerializeField] float delayTime = 5f;

    private void Start()
    {
        StartCoroutine(StartLoading());
    }

    private IEnumerator StartLoading()
    {
        yield return new WaitForSeconds(delayTime);

        App.LoadScene(SceneName.Title);
    }
}
