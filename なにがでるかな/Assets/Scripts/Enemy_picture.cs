using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_picture : MonoBehaviour
{
    SpriteRenderer MainSpriteRenderer;

    public Sprite BroomGhost;
    public Sprite PumpkinTotem;
    public Sprite CorpseSeoul;

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
        MainSpriteRenderer.sprite = BroomGhost;
    }
}
