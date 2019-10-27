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

    // Start is called before the first frame update
    void Start()
    {
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        ChangeState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeState()
    {
        if (Button.level == 1)
        {
            MainSpriteRenderer.sprite = BroomGhost;
            this.transform.localScale = new Vector3(1, 1, 1);   
        }
        if (Button.level == 2)
        {
            MainSpriteRenderer.sprite = PumpkinTotem;
            this.transform.localScale = new Vector3(2, 2, 1);
        }
        if (Button.level == 3)
        {
            MainSpriteRenderer.sprite = CorpseSeoul;
            this.transform.localScale = new Vector3(3, 3, 1);
        }
        if (Button.level == 4)
        {
            MainSpriteRenderer.sprite = green;
            this.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
