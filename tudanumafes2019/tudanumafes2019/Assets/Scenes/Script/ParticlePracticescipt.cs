using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ParticlePracticescipt : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

        ParticleSystem ps = GetComponent<ParticleSystem>();

        var main = ps.main;

        main.startLifetime = 10.0f;
        main.startSize = 4.0f;
        main.startDelay = 4.0f;

        var em = ps.emission;

        em.enabled = true;

        em.type = ParticleSystemEmissionType.Time;

        em.SetBursts(
            new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(2.0f,100),
                new ParticleSystem.Burst(4.0f,100)
            });

        int attack=0;
        if (attack >= 100)
        {
            main.startSize = 7.0f;
        }
        else if (50 <= attack && attack < 100)
        {
            main.startSize = 4.0f;
        }
        else if(attack < 50)
        {
            main.startSize = 2.0f;
        }






    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject obj)
    {
        Debug.Log("衝突");
    }

    public void attack()
    {
        
    }

   
}
