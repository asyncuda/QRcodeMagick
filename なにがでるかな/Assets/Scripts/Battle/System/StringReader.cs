using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class StringReader : MonoBehaviour
{
    private readonly Subject<string> onEntered = new Subject<string>();

    public IObservable<string> OnEntered => onEntered;

    private string code = string.Empty;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\n' || c == '\r')
            {
                onEntered.OnNext(code);
                code = string.Empty;
                break;
            }
            else
            {
                code += c;
            }
        }
    }
}
