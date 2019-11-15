using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public int hp;

    public int hpmax;

    public string Attribute;

    public GameObject DamageText;

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update() { }

    public void Ondamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
        }
    }

    public int Magic(int power, int level, string at_attr, string def_attr)
    {
        return (int)(power * (SelectLevel(level) * CompAttr(at_attr, def_attr)));
    }

    public IEnumerator OndamageEffect(int damage)
    {
        Instantiate(DamageText, new Vector3(transform.position.x, transform.position.y - 1f, 0), transform.rotation).GetComponent<TextMesh>().text = damage.ToString();

        yield return new WaitForSeconds(1f);
    }

    public IEnumerator MagicEffect(int power, string magickattr)
    {
        Debug.Log(magickattr);
        if (magickattr == "FIRE")
        {
            yield return Shot(_Fire, this.transform.position, this.transform.position * Vector2.left, power);
        }
        else if (magickattr == "WATER")
        {
            yield return Shot(_Water, this.transform.position, this.transform.position * Vector2.left, power);
        }
        else if (magickattr == "PLANT")
        {
            yield return Shot(_Plant, this.transform.position, this.transform.position * Vector2.left, power);
        }
        else if (magickattr == "GROUND")
        {
            yield return Shot(_Ground, this.transform.position, this.transform.position * Vector2.left, power);
        }
    }

    private float SelectLevel(int level)
    {
        float game_speed = 3.0f;
        float level_power = 0.0f;
        switch (level)
        {
            case 1: level_power = 1.2f * (float)short.MaxValue / 18004f; break;
            case 2: level_power = 0.9f * (float)ushort.MaxValue / 18004f; break;
            case 3: level_power = 0.7f * (float)int.MaxValue / 18004f; break;
            default: level_power = 1.0f; break;
        }

        return game_speed * level_power;
    }

    private float CompAttr(string attack, string defence)
    {
        if (attack == "FIRE")
        {
            if (defence == "PLANT") return 1.5f;
            else if (defence == "WATER") return 0.5f;
            else return 1.0f;
        }
        else if (attack == "WATER")
        {
            if (defence == "FIRE") return 1.5f;
            else if (defence == "PLANT") return 0.5f;
            else return 1.0f;
        }
        else if (attack == "PLANT")
        {
            if (defence == "WATER") return 1.5f;
            else if (defence == "FIRE") return 0.5f;
            else return 1.0f;
        }
        else return 1.0f;
    }

    private void MainConfig(out ParticleSystem.MainModule main)
    {
        main.duration = 2.0f;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy;
    }

    public IEnumerator Shot(GameObject ParticleObj, Vector2 from, Vector2 to, int power)
    {
        GameObject obj = Instantiate(ParticleObj);
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();

        ps.Stop();

        var main = ps.main; MainConfig(out main);
        ps.transform.position = from;

        ps.Play();

        // ギュイーンと貯めてエフェクト拡大
        ps.transform.localScale = Vector3.one;
        for (int i = 0; i < 60; i++)
        {
            // Vector3.one is 単位ベクトル(1, 1, 1)
            ps.transform.localScale += Vector3.one;

            yield return null;
        }

        // ドーンと放つ(等速直線運動)
        Vector3 go = (to - from) / 30.0f;
        while (Vector3.Distance(to, ps.transform.position) > 0.1f)
        {
            ps.transform.position += go;
            yield return null;
        }

        // エフェクトの終了を待つ
        for (int time = 0; time < 60 && ps.isPlaying; time++) yield return null;

        Destroy(obj);
    }


    [SerializeField] private GameObject _Fire;

    [SerializeField] private GameObject _Water;

    [SerializeField] private GameObject _Plant;

    [SerializeField] private GameObject _Ground;
}
