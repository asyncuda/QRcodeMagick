using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{
    public Unit Player;
    public Unit Enemy;

    public GameObject QRreadinfo;//QRをよみこませてね！のやつ

    bool ContinueGame;

    [SerializeField] Text[] HP = new Text[2];
    [SerializeField] Text battlelog;

    // Start is called before the first frame update
    void Start()
    {
        QRreadinfo.SetActive(false);
        ContinueGame = true;
        HP[0].text = "HP "+(Player.hpmax).ToString()+"/"+(Player.hpmax).ToString();
        HP[1].text = "HP "+(Enemy.hpmax).ToString()+"/"+(Enemy.hpmax).ToString();
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
            yield return Player_action();
            if (Enemy.hp <= 0)
            {
                battlelog.text = "You win!";
                ContinueGame = false;
                yield break;
            }
            if (Button.level == 4)
            {
                yield return Player2_action();
            }
            else
            {
                yield return Enemy_action();
            }
            if (Player.hp <= 0)
            {
                battlelog.text = "You lose...";
                ContinueGame = false;
                yield break;
            }

        }
        yield break;
    }

    IEnumerator Player_action()
    {
        battlelog.text = "Blueのターン";

        QRreadinfo.SetActive(true);

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }


        QRreadinfo.SetActive(false);

        Enemy.Ondamage(Player.Magic(Player.at));
        HP[1].text = "HP "+(Enemy.hp).ToString()+"/"+(Enemy.hpmax).ToString();
        yield return new WaitForSeconds(1f);

        
        yield break;
    }

    IEnumerator Player2_action()
    {
        battlelog.text = "Greenのターン";
        QRreadinfo.SetActive(true);

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        QRreadinfo.SetActive(false);

        Player.Ondamage(Enemy.Magic(Enemy.at));
        HP[0].text = "HP " + (Player.hp).ToString() + "/" + (Player.hpmax).ToString();
        yield return new WaitForSeconds(1f);

        yield break;
    }

    IEnumerator Enemy_action()
    {
        battlelog.text = "Enemyのターン";
        yield return new WaitForSeconds(1f);

        Player.Ondamage(Enemy.Magic(Enemy.at));
        HP[0].text = "HP "+(Player.hp).ToString()+"/"+(Player.hpmax).ToString();
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
