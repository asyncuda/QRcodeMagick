using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damagetext : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-80, 80), Random.Range(100, 180), 0));
        StartCoroutine(DestroyObject());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(this.gameObject);
    }
}
