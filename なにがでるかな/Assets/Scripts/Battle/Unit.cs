using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public int hp=1;
    public int hpmax = 500;

    public string Attribute;

    [SerializeField]public int at = 1200;

    public GameObject DamageText;
    public GameObject DamageEffect;

    public GameObject[] DamageEffectList = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Ondamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
        }

        Instantiate(DamageText, new Vector3(transform.position.x, transform.position.y - 1f, 0), transform.rotation).GetComponent<TextMesh>().text = damage.ToString();

        yield return new WaitForSeconds(1f);
    }
    public IEnumerator EffectMoving(int code)
    {
        DamageEffect = DamageEffectList[code % DamageEffectList.Length];

        ParticleSystem ps = Instantiate(
                 DamageEffect,
                 new Vector3(transform.position.x, transform.position.y),
                 transform.rotation
                 ).GetComponent<ParticleSystem>();

        ps.Stop();

        var main = ps.main;
        var trans = ps.transform;
        main.duration = 1.8f;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy;

        ps.Play();

        float goalx = -this.transform.position.x;
        float progress = (goalx - ps.transform.position.x) / 30f;

        float max_scale = 10;
        for (float i = 1; i <= max_scale; i += (max_scale / 60))
        {
            trans.localScale = new Vector3(i, i, i);
            yield return null;
        }

        while (Mathf.Abs(goalx - ps.transform.position.x) > 0.1)
        {
            ps.transform.position = new Vector3(ps.transform.position.x + progress, ps.transform.position.y);
            yield return null;
        }

        while (ps.isPlaying == true)
        {
            yield return null;
        }

        yield return null;

        /*
        IEnumerator Attack()
        {
            yield break;
        }

        IEnumerator Heel()
        {
            yield break;
        }
        */
    }
    public int Magic(int power, int level, string at_attr, string def_attr)
    {


        StartCoroutine(EffectMoving(power));

        return (int)(power * (SelectLevel(level) * CompAttr(at_attr, def_attr)));
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
}
