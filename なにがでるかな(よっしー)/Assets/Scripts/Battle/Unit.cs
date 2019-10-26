using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public int hp=1;
    public int hpmax = 500;
    [SerializeField]public int at = 1200;
    public GameObject DamageText;
    public GameObject DamageEffect;


    bool turn = true;


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

        ParticleSystem ps = Instantiate(
                 DamageEffect,
                 new Vector3(-transform.position.x, transform.position.y),
                 transform.rotation
                 ).GetComponent<ParticleSystem>();

        ps.Stop();

        var main = ps.main;
        var trans = ps.transform;
        main.duration = 1.8f;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy;

        ps.Play();

        float goalx = this.transform.position.x;
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

        Instantiate(DamageText, new Vector3(transform.position.x, transform.position.y - 1f, 0), transform.rotation).GetComponent<TextMesh>().text = damage.ToString();

        yield return new WaitForSeconds(1f);

        yield break;

    }

    public int Magic(int attack)
    {
        int magic = Random.Range(1, 5);
        int ransuu = Random.Range(1, 21);
        float DAMAGE = 0f;

        switch (magic)
        {
            case 1:
                DAMAGE = attack * 1.8f + ransuu;
                break;
            case 2:
                DAMAGE = attack * 1.3f + ransuu;
                break;
            case 3:
                DAMAGE = attack * 0.7f + ransuu;
                break;
            case 4:
                DAMAGE = attack * 0.3f + ransuu;
                break;
        }
        return (int)DAMAGE;
    }
    
}

