using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using QRCodeTranslator;
using MagickSkill = QRCodeTranslator.NewTowelExtendedMagicSkill;


public class BattleSystem : MonoBehaviour
{
    public Unit Player;
    public Unit Enemy;

    public GameObject QRreadinfo;//QRをよみこませてね！のやつ
    public GameObject magictext;

    public static string WinnerName;
    public static int countTurn = 1;

    private QRReader qr;

    [SerializeField] Text[] HP = new Text[2];

    private Color MyOrange = new Color(255.0f / 255.0f, 165f / 255f, 0f);

    private readonly string DataBasePath = Application.streamingAssetsPath + @"\spells.db";

    // Start is called before the first frame update
    void Start()
    {
        countTurn = 1;
        BattleSet();
        QRreadinfo.SetActive(false);
        magictext.SetActive(false);

        HP[0].text = (Player.name).ToString()+"\n"+(Player.hpmax).ToString()+" / "+(Player.hpmax).ToString();
        HP[1].text = (Enemy.name).ToString()+"\n"+(Enemy.hpmax).ToString()+" / "+(Enemy.hpmax).ToString();

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
                Enemy.Attribute = "";
                Enemy.name = "ほうきおばけ";
                break;
            case 2:
                Enemy.hpmax = ushort.MaxValue;
                Enemy.Attribute = "PLANT";
                Enemy.name = "パンプキントーテム";
                break;
            case 3:
                Enemy.hpmax = int.MaxValue;
                Enemy.Attribute = "FIRE";
                Enemy.name = "コープスソウル";
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
        while (true)
        {
            yield return Player_action();

            if (Enemy.hp <= 0)
            {
                WinnerName = Player.name;
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("GameOverScene");

                yield break;
            }

            yield return new WaitForSeconds(0.4f);

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
                WinnerName = Enemy.name;
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene("GameOverScene");

                yield break;
            }
            countTurn += 1;
        }
    }
    IEnumerator Player_action()
    {
        HP[0].color = MyOrange;
        HP[1].color = Color.white;

        HP[0].color = MyOrange;
        HP[1].color = Color.white;

        QRreadinfo.SetActive(true);

        string code;

        while ((code = qr.InputLine()) == "")
        {
            yield return null;
        }
        var ms = new NewTowelExtendedMagicSkill(code, DataBasePath);

        magictext.SetActive(true);
        magictext.GetComponent<Text>().text = (ms.Spell1 + "." + ms.Spell2);

        Debug.Log("QR code is :" + code);

        QRreadinfo.SetActive(false);

        yield return AttackAndEffect(Player, Enemy, ms, Button.level);

        HP[1].text = (Enemy.name).ToString()+"\n"+(Enemy.hp).ToString()+" / "+(Enemy.hpmax).ToString();
        magictext.SetActive(false);

        yield break;
    }
    IEnumerator Player2_action()
    {
        HP[0].color = Color.white;
        HP[1].color = MyOrange;
        QRreadinfo.SetActive(true);

        string code;

        while ((code = qr.InputLine()) == "")
        {
            yield return null;
        }
        var ms = new NewTowelExtendedMagicSkill(code, DataBasePath);

        magictext.SetActive(true);
        magictext.GetComponent<Text>().text = (ms.Spell1 + "." + ms.Spell2);

        Debug.Log("QR code is :" + code);

        QRreadinfo.SetActive(false);

        yield return AttackAndEffect(Enemy, Player, ms);

        HP[0].text = (Player.name).ToString()+"\n"+(Player.hp).ToString() + " / " + (Player.hpmax).ToString();

        magictext.SetActive(false);

        yield break;
    }

    IEnumerator Enemy_action()
    {   
        HP[1].color = MyOrange;
        HP[0].color = Color.white;

        var ms = new NewTowelExtendedMagicSkill(DateTime.Now.ToString(), DataBasePath);

        magictext.SetActive(true);
        magictext.GetComponent<Text>().text = (ms.Spell1 + "." + ms.Spell2);

        yield return AttackAndEffect(Enemy, Player, ms);

        HP[0].text = (Player.name).ToString()+"\n"+(Player.hp).ToString()+" / "+(Player.hpmax).ToString();
        magictext.SetActive(false);

        yield break;
    }

    IEnumerator AttackAndEffect(Unit Attacker, Unit Opponent, MagickSkill ms, int level = 4)
    {
        yield return Attacker.MagicEffect(ms.Power, ms.Attribute);
        int damage = Attacker.Magic(ms.Power, level, ms.Attribute, Opponent.Attribute);
        Opponent.Ondamage(damage);
        yield return Opponent.OndamageEffect(damage);
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
