using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using QRCodeTranslator;

public class BattleSystem : MonoBehaviour
{
    public Unit Player;
    public Unit Enemy;

    public GameObject QRreadinfo;//QRをよみこませてね！のやつ

    bool ContinueGame;

    private QRReader qr;

    [SerializeField] Text[] HP = new Text[2];
    [SerializeField] Text battlelog;

    private Color MyOrange = new Color(255.0f / 255.0f, 165f / 255f, 0f);


    // Start is called before the first frame update
    void Start()
    {
        BattleSet();
        QRreadinfo.SetActive(false);
        ContinueGame = true;
        HP[0].text = (Player.hpmax).ToString()+" / "+(Player.hpmax).ToString();
        HP[1].text = (Enemy.hpmax).ToString()+" / "+(Enemy.hpmax).ToString();

        qr = QRreadinfo.GetComponent<QRReader>();

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

    void BattleSet()
    {
        Player.hpmax = 18004;
        Player.name = "Blue";
        switch (Button.level)
        {
            case 1:
                Enemy.hpmax = short.MaxValue;
                Enemy.name = "ほうきおばけ";
                break;
            case 2:
                Enemy.hpmax = ushort.MaxValue;
                Enemy.name = "パンプキントーテム";
                break;
            case 3:
                Enemy.hpmax = int.MaxValue;
                Enemy.name = "コープスソウル";
                Enemy.at = 5000;
                break;
            case 4:
                Enemy.hpmax = 18004;
                Enemy.name = "Green";
                break;
        }
        Player.hp = Player.hpmax;
        Enemy.hp = Enemy.hpmax;
    }

    IEnumerator battle()
    {
        while (ContinueGame)
        {
            yield return Player_action();

            if (Enemy.hp <= 0)
            {
                battlelog.text = Player.name + " win!";
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
                if (Button.level == 4)
                {
                    battlelog.text = Enemy.name + " win!";
                }
                else
                {
                    battlelog.text = Player.name + " lose...";
                }
                ContinueGame = false;
                yield break;
            }
        }
        yield break;
    }
    IEnumerator Player_action()
    {
        battlelog.text = "Blueのターン";

        HP[0].color = MyOrange;
        HP[1].color = Color.white;

        QRreadinfo.SetActive(true);

        string code;

        while ((code = qr.InputLine()) == "")
        {
            yield return null;
        }
        //var MagickSkill = new NewTowelExtendedMagicSkill(code, false, "Fire", Application.streamingAssetsPath + "/spells.db");

        Debug.Log("QR code is :" + code);

        QRreadinfo.SetActive(false);

        yield return Enemy.Ondamage(Player.Magic(Player.at));
        HP[1].text = (Enemy.hp).ToString()+" / "+(Enemy.hpmax).ToString();
        
        yield break;
    }


    IEnumerator Player2_action()
    {
        battlelog.text = "Greenのターン";
        HP[0].color = Color.white;
        HP[1].color = MyOrange;
        QRreadinfo.SetActive(true);

        string code;

        while ((code = qr.InputLine()) == "")
        {
            yield return null;
        }
        //var MagickSkill = new NewTowelExtendedMagicSkill(code, false, "Fire", Application.streamingAssetsPath + "/spells.db");

        Debug.Log("QR code is :" + code);

        QRreadinfo.SetActive(false);

        Player.Ondamage(Enemy.Magic(Enemy.at));
        HP[0].text = (Player.hp).ToString() + " / " + (Player.hpmax).ToString();

        yield break;
    }

    IEnumerator Enemy_action()
    {
        battlelog.text = "Enemyのターン";
        HP[1].color = MyOrange;
        HP[0].color = Color.white;

        yield return Player.Ondamage(Enemy.Magic(Enemy.at));
        HP[0].text = (Player.hp).ToString()+" / "+(Player.hpmax).ToString();
       
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
