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
        QrMagickEffect.Add("FIRE", new ParticleAnimation(_Fire, Color.red));
        QrMagickEffect.Add("WATER", new ParticleAnimation(_Water, Color.blue));
        QrMagickEffect.Add("PLANT", new ParticleAnimation(_Plant, Color.green));
        QrMagickEffect.Add("GROUND", new ParticleAnimation(_Ground, Color.yellow));
        QrMagickEffect.Add("HEEL", new ParticleAnimation(_Heel, Color.green));

        QrMagickEffect["FIRE"].Effect = QrMagickEffect["FIRE"].Shot;
        QrMagickEffect["WATER"].Effect = QrMagickEffect["WATER"].Shot;
        QrMagickEffect["PLANT"].Effect = QrMagickEffect["PLANT"].Shot;
        QrMagickEffect["GROUND"].Effect = QrMagickEffect["GROUND"].Meteo;
        QrMagickEffect["HEEL"].Effect = QrMagickEffect["HEEL"].Heel;
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
        yield return null;
//        yield return QrMagickEffect["FIRE"].Shot(this.transform.position, this.transform.position * Vector2.left, power);
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

    private Dictionary<string, ParticleAnimation> QrMagickEffect = new Dictionary<string, ParticleAnimation>(); 

    [SerializeField] private GameObject _Fire;

    [SerializeField] private GameObject _Water;

    [SerializeField] private GameObject _Plant;

    [SerializeField] private GameObject _Ground;

    [SerializeField] private GameObject _Heel;
}
