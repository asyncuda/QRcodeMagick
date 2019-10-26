using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public Title title;
    public static int level; //Enemy_picture.csで参照します(1＝レベル : 2=レベル2 : 3=レベル3 : 4=Multi)

    public GameObject ChildText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Exit();
        }
    }

    public void Startsingle()
    {
        title.buttonReset();//ボタンをsingle,Multi選択からレベル設定へ変更します
    }
    public void StartMulti()
    {
        level = 4;
        SceneManager.LoadScene("Battle");
    }

    public void Exit()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }

    public void Highlighted()
    {
        ChildText.GetComponent<Text>().color = Color.yellow;
    }
    public void DisHighlighted()
    {
        ChildText.GetComponent<Text>().color = Color.white;
    }

    public void Level1()
    {
        level = 1;
        SceneManager.LoadScene("Battle");
    }
    public void Level2()
    {
        level = 2;
        SceneManager.LoadScene("Battle");
    }
    public void Level3()
    {
        level = 3;
        SceneManager.LoadScene("Battle");
    }
}
