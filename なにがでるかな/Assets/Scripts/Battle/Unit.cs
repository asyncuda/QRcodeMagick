using System.Collections;
using UnityEngine;
using DG.Tweening;
using NUnit.Framework.Constraints;

public class Unit : MonoBehaviour
{
    public int hp;

    public int hpmax;

    public string Attribute;

    public GameObject DamageText;

    [SerializeField] private GameObject _Fire;

    [SerializeField] private GameObject _Water;

    [SerializeField] private GameObject _Plant;

    [SerializeField] private GameObject _Ground;


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

    public void MagicEffect(int power, string magickattr)
    {
        Debug.Log(magickattr);
        //switchの前にデフォルト値が入っていないと例外が出るのでとりあえずのデフォルト値
        GameObject particleObj = _Fire;
        
        switch (magickattr)
        {
            case "FIRE":
                particleObj = _Fire;
                break;
            case "WATER":
                particleObj = _Water;
                break;
            case "PLANT":
                particleObj = _Plant;
                break;
            case "GROUND":
                particleObj = _Ground;
                break;
        }
        Shot(particleObj, transform.position, transform.position * Vector2.left);
        
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
        main.duration = 10.0f;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy;
    }

    private void Shot(GameObject ParticleObj, Vector3 from, Vector3 to)
	{
        GameObject obj = Instantiate(ParticleObj);
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();

        ps.Stop();

        var main = ps.main; MainConfig(out main);
        ps.transform.position = from;

        ps.Play();

        ps.transform.localScale = Vector3.one;
        
        //「ギューンと溜めて等速直線運動」パターン：3秒で10倍に拡大→1秒で座標toまで移動→破棄
        Sequence directLinearMotion = DOTween.Sequence().Append(ps.transform.DOScale(new Vector2(10f, 10f), 3f)).Append(ps.transform.DOMove(to, 1f));

        //DOTweenアニメーション終了後、アニメーションを破棄・オブジェクトを消す
        directLinearMotion.AppendCallback(() => { ps.DOKill(); Destroy(obj); });
        
    }

}
