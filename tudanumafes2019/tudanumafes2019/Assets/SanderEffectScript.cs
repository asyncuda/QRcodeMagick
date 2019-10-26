using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanderEffectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();

        var main = ps.main;

        main.startLifetime = 2.0f;
        main.gravityModifier=0.5f;
        main.simulationSpeed = 2.2f;

        var em = ps.emission;

        em.enabled = true;

        em.type = ParticleSystemEmissionType.Time;

        em.SetBursts(
            new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(2.0f,100),
                new ParticleSystem.Burst(4.0f,100)
            });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
