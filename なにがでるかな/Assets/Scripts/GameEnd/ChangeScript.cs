using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScript : MonoBehaviour
{
    public Text click;
    public Text winner;
    public Text TurnResult;
    // Start is called before the first frame update
    void Start()
    {
        int turn = BattleSystem.countTurn;
        TurnResult.text = "かかったターン   " + turn.ToString();
        winner.text = BattleSystem.WinnerName + "のかち！";

        click.enabled = false;
        winner.enabled = false;
        TurnResult.enabled = false;

        StartCoroutine(ApeearText());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Quit();

        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Title");
        }

    }

    IEnumerator ApeearText()
    {
        yield return new WaitForSeconds(1f);
        winner.enabled = true;
       
        yield return new WaitForSeconds(1f);
        TurnResult.enabled = true;
        
        yield return new WaitForSeconds(1f);
        click.enabled = true;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying=false;
#else
        Application.Quit();
#endif
    }
}
