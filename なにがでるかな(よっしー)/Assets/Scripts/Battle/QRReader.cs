using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QRReader : MonoBehaviour
{
    private string code = "";

    private string buff = ""; 

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\n' || c == '\r')
            {
                code = buff;
                buff = "";
                break;
            }
            else
            {
                buff += c;
            }
        }
    }

    public string InputLine()
    {
        string value = code;
        code = "";
        return value;
    }

}
