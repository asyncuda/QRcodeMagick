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

    public GameObject DamageEffect;

    // Start is called before the first frame update
    void Start() { }

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

    public IEnumerator MagicEffect(int power, string attr)
    {
        yield return Heel(DamageEffect, this.transform.position, power);
        //yield return Shot(DamageEffect, this.transform.position, this.transform.position * Vector2.left, power);
    }

    private float SelectLevel(int level)
    {
        switch (level)
        {
            case 1: return 1.0f;
            case 2: return 1.5f;
            case 3: return 50000f;
            default: return 1.0f;
        }
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

    public void MainConfig(out ParticleSystem.MainModule main)
    {
        main.duration = 2.0f;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy;
    }

    public IEnumerator Heel(GameObject effect, Vector2 target, int power)
    {
        GameObject obj = Instantiate(effect);
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();

        ps.Stop();

        ps.transform.position = target;
        var main = ps.main; MainConfig(out main);

        ps.Play();

        while (ps.isPlaying) yield return null;

        Destroy(obj);
    }

    public IEnumerator Shot(GameObject effect, Vector2 from, Vector2 to, int power)
    {
        GameObject obj = Instantiate(effect);
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();

        ps.Stop();

        ps.transform.position = from;
        var main = ps.main; MainConfig(out main);

        ps.Play();

        // ギュイーンと貯めてエフェクト拡大
        ps.transform.localScale = Vector3.one;
        for (int i = 0; i < 60; i++)
        {
            // one is 単位ベクトル(1, 1, 1)
            ps.transform.localScale += Vector3.one / 60;
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
        while (ps.isPlaying) yield return null;

        Destroy(obj);
    } 
}
