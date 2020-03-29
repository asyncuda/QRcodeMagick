using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public GameObject Title1, Title2, Title3,exit;//Title1=なにがでるかな、Title2=プレイ人数、Title3=難易度、exit=やめる

    // Start is called before the first frame update
    void Start()
    {
        StartTitle();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Exit();
        }
    }

    public void StartTitle()//なにがでるかな表示
    {
        Title1.SetActive(true);
        Title2.SetActive(false);
        Title3.SetActive(false);
        exit.SetActive(false);
    }

    public void ButtonAppear()//何人で遊ぶか表示
    {
        Title1.SetActive(false);
        Title2.SetActive(true);
        Title3.SetActive(false);
        exit.SetActive(true);
    }

    public void buttonReset()//難易度選択表示
    {
        Title1.SetActive(false);
        Title2.SetActive(false);
        Title3.SetActive(true);
        exit.SetActive(true);
    }
    public void Exit()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }
}
