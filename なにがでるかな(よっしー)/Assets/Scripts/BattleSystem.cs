using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour
{
    public Unit Player;
    public Unit Enemy;

    bool ContinueGame;

    [SerializeField] Text[] HP = new Text[2];
    [SerializeField] Text QRinfo;
    [SerializeField] Text battlelog;

    // Start is called before the first frame update
    void Start()
    {
        QRinfo.enabled = false;
        ContinueGame = true;
        HP[0].text = "HP "+(Player.hpmax).ToString()+"/"+(Player.hpmax).ToString();
        HP[1].text = "HP "+(Enemy.hpmax).ToString()+"/"+(Enemy.hpmax).ToString();
        StartCoroutine(battle());
    }

    // Update is called once per frame
    void Update()
    {
        
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

            yield return Enemy_action();
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
        battlelog.text = "Playerのターン";
        QRinfo.enabled = true;

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        QRinfo.enabled = false;

        Enemy.Ondamage(Player.Magic(Player.at));
        HP[1].text = "HP "+(Enemy.hp).ToString()+"/"+(Enemy.hpmax).ToString();
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


}
