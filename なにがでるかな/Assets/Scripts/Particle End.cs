using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEnd : MonoBehaviour
{
     ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = this.GetComponent<particleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(particle = isStopped)
        {
            Destroy(this.gameObject);
        }
    }
}
