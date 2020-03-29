using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAnimation : MonoBehaviour
{
    public GameObject ParticleObj { get; private set; }

    public Color color { get; private set; }

    public Func<Vector2, Vector2, int, IEnumerator> Effect;

    public void Start()
    {
            
    }

    public ParticleAnimation(GameObject psobj, Color cl)
    {
        this.ParticleObj = psobj;
        this.color = cl;
    }

    private void MainConfig(out ParticleSystem.MainModule main)
    {
        main.duration = 2.0f;
        main.scalingMode = ParticleSystemScalingMode.Hierarchy;
    }

    public IEnumerator Shot(Vector2 from, Vector2 to, int power)
    {
        GameObject obj = Instantiate(ParticleObj);
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();

        ps.Stop();

        var main = ps.main; MainConfig(out main);
        ps.transform.position = from;

        ps.Play();

        // ギュイーンと貯めてエフェクト拡大
        ps.transform.localScale = Vector3.one;
        for (int i = 0; i < 60; i++)
        {
            // Vector3.one is 単位ベクトル(1, 1, 1)
            ps.transform.localScale += Vector3.one;

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
        for (int time = 0; time < 60 && ps.isPlaying; time++) yield return null;

        Destroy(obj);
    }

    public IEnumerator Meteo(Vector2 from, Vector2 to, int power)
    {
        yield return Shot(from, to - Vector2.left * 3, power);

        yield return Shot(from, to, power);

        yield return Shot(from, to + Vector2.left * 3, power);
    }

    public IEnumerator Heel(Vector2 from, Vector2 to, int power)
    {
        GameObject obj = Instantiate(ParticleObj);
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();

        ps.Stop();

        var main = ps.main; MainConfig(out main);

        ps.transform.position = from;
        ps.transform.localScale = Vector2.one * 3.0f;

        /* 自身にエフェクトを舞い散らす */
        ps.Play();

        for (int time = 0; time < 60 && ps.isPlaying; time++) yield return null;

        Destroy(obj);
    }
}
