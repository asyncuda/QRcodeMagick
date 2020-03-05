using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public GameObject TitleLogo,single, multi, exit, level1, level2, level3;

    // Start is called before the first frame update
    void Start()
    {
        TitleLogo.SetActive(true);
        single.SetActive(false);
        multi.SetActive(false);
        exit.SetActive(false);
        level1.SetActive(false);
        level2.SetActive(false);
        level3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonAppear()
    {
        TitleLogo.SetActive(false);
        single.SetActive(true);
        multi.SetActive(true);
        exit.SetActive(true);
    }

    public void buttonReset()
    {
        single.SetActive(false);
        multi.SetActive(false);
        exit.SetActive(false);
        level1.SetActive(true);
        level2.SetActive(true);
        level3.SetActive(true);
    }
}
