using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Text;

public class StringReader : MonoBehaviour
{
    private readonly Subject<string> onEntered = new Subject<string>();

    public IObservable<string> OnEntered => onEntered;

    private StringBuilder code = new StringBuilder();

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\n' || c == '\r')
            {
                onEntered.OnNext(code.ToString());
                code = new StringBuilder(code.Length);
                break;
            }
            else
            {
                code.Append(c);
            }
        }
    }
}
