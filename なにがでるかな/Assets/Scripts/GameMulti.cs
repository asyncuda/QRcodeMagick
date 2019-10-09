using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMulti : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //OnRetry関数が実行されたら、sceneを読み込む
    public void OnRetry()
    {
        //ButtonSceneを自分の読み込みたいscene名に変える
        SceneManager.LoadScene("RuleScene");
    }
}
