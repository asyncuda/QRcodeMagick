using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_picture : MonoBehaviour
{
    SpriteRenderer MainSpriteRenderer;

    public Sprite BroomGhost;
    public Sprite PumpkinTotem;
    public Sprite CorpseSeoul;
    public Sprite green; //ここまで画像

    int levelselect;

    // Start is called before the first frame update
    void Start()
    {
        levelselect = Button.level;//Button.csで決定したレベルの値を代入します
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        ChangeState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeState()
    {
        if(levelselect==1)
            MainSpriteRenderer.sprite = BroomGhost;
        if(levelselect==2)
            MainSpriteRenderer.sprite = PumpkinTotem;
        if(levelselect==3)
            MainSpriteRenderer.sprite = CorpseSeoul;
        if (levelselect == 4)
            MainSpriteRenderer.sprite = green;
    }
}
