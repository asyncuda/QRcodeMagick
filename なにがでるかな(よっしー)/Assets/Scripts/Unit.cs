using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int hp=1;
    [SerializeField] public int hpmax = 500;
    [SerializeField] public int at;
    public GameObject DamageText;

    // Start is called before the first frame update
    void Start()
    {
        hp = hpmax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ondamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
        }
        Instantiate(DamageText, new Vector3(transform.position.x,transform.position.y-0.3f, 0), transform.rotation).GetComponent<TextMesh>().text = damage.ToString();
    }
    

    public int Magic(int attack)
    {
        int magic = Random.Range(1, 5);
        int ransuu = Random.Range(1, 21);
        float DAMAGE = 0f;

        switch (magic)
        {
            case 1:
                Debug.Log("Magic1");
                DAMAGE = attack * 1.8f + ransuu;
                break;
            case 2:
                Debug.Log("Magic2");
                DAMAGE = attack * 1.3f + ransuu;
                break;
            case 3:
                Debug.Log("Magic3");
                DAMAGE = attack * 0.7f + ransuu;
                break;
            case 4:
                Debug.Log("Magic4");
                DAMAGE = attack * 0.3f + ransuu;
                break;
        }
        return (int)DAMAGE;
    }
    
}

