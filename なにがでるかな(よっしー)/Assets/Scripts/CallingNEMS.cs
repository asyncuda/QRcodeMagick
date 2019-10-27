using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QRCodeTranslator;
using System;

public class CallingNEMS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		//引数：string QRコードの文字列、int 挑戦するコース、string 敵の属性、string データベースのパス
		var nems = new NewTowelExtendedMagicSkill("qrCodeString", 3, "FIRE", Application.streamingAssetsPath+"/spells.db");
		OutPut(nems);

		var nems2 = new NewTowelExtendedMagicSkill("hoge", 1, null, Application.streamingAssetsPath + "/spells.db");
		OutPut(nems2);
	}

	void OutPut(NewTowelExtendedMagicSkill obj)
	{
		Debug.Log(obj.Spell1);
		Debug.Log(obj.Spell2);
		Debug.Log(obj.Power);
		Debug.Log(obj.Attribute);
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}