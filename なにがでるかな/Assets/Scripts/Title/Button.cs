using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public Title title;
    public static int level; //Enemy_picture.csで参照します(1＝レベル : 2=レベル2 : 3=レベル3 : 4=Multi)

    SpriteRenderer MainSpriteRenderer;//追加

    public Sprite Onmouse;//追加
    public Sprite Nonmouse;//追加

    // Start is called before the first frame update
    void Start()
    {
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();//追加
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void Highlighted()
    {
        MainSpriteRenderer.sprite = Onmouse;//変更した、変更の仕方は敵の画像を変えた時と同じ。Enemy_picuture.csと同じ。おそらくイベントトリガーの使い方を知らないからエラー
    }
    public void DisHighlighted()
    {
        MainSpriteRenderer.sprite = Nonmouse;//変更した
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
   
    public void ReturnTitle()
    {
        title.StartTitle();
    }

    public void ReturnSelectPlay()
    {
        title.ButtonAppear();
    }
    
    public void PushExit()
    {
        title.Exit();
    }
}
