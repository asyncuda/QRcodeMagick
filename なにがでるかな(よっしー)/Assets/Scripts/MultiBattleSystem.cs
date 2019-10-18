using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiBattleSystem : MonoBehaviour
{
    public Unit Player1;
    public Unit Player2;

    public GameObject QRreadinfo;

    bool ContinueGame;  //ゲーム続行

    [SerializeField] Text[] PlayerHP = new Text[2];　//　hpのUI表示
    [SerializeField] Text battlelog;

    // Start is called before the first frame update
    void Start()
    {
        QRreadinfo.SetActive(false);
        ContinueGame = true;
        PlayerHP[0].text = "HP "+(Player1.hpmax).ToString()+"/"+(Player1.hpmax).ToString();
        PlayerHP[1].text = "HP "+(Player2.hpmax).ToString()+"/"+(Player2.hpmax).ToString();
        StartCoroutine(battle());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Quit();
        }
    }

    IEnumerator battle()
    {
        yield return new WaitForSeconds(1f);
        while (ContinueGame)
        {
            yield return Player1_action();
            if (Player2.hp <= 0)
            {
                battlelog.text = "Player1の勝ち！";
                ContinueGame = false;
                yield break;
            }

            yield return Player2_action();
            if (Player1.hp <= 0)
            {
                battlelog.text = "Player2の勝ち！";
                ContinueGame = false;
                yield break;
            }
        }
        yield break;
    }

    IEnumerator Player1_action()
    {
        battlelog.text = "Player1のターン";
        QRreadinfo.SetActive(true);

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        QRreadinfo.SetActive(false);

        Player2.Ondamage(Player1.Magic(Player1.at));
        PlayerHP[1].text = "HP "+(Player2.hp).ToString()+"/"+(Player2.hpmax).ToString();
        yield return new WaitForSeconds(1f);
        
        yield break;
    }

    IEnumerator Player2_action()
    {
        battlelog.text = "Player2のターン";
        QRreadinfo.SetActive(true);

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        QRreadinfo.SetActive(false);

        Player1.Ondamage(Player2.Magic(Player2.at));
        PlayerHP[0].text = "HP "+(Player1.hp).ToString()+"/"+(Player1.hpmax).ToString();
        yield return new WaitForSeconds(1f);

        yield break;
    }

    void Quit()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }
}
