using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFinish : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("Button click");
    }
    public void EndGame()
    { 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying=false;
#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
#endif
    }
    // Start is called before the first frame update
    void Start()
    {

    }
     
    // Update is called once per frame
    void Update()
    {
      
    }

}
