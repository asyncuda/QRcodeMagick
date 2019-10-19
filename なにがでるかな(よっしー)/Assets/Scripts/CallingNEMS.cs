using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QRCodeTranslator;

public class CallingNEMS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		var nems = new NewTowelExtendedMagicSkill("qrCodeString", true, "FIRE", Application.streamingAssetsPath+"/spell.db");
		Debug.Log(Application.streamingAssetsPath);
		Debug.Log(nems.Spell1);
		Debug.Log(nems.Spell2);
		Debug.Log(nems.Power);
		Debug.Log(nems.Attribute);
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
