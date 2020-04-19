using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class CpuWizard : BaseWizardComponent
{
    public override void CallingMagickSkill()
    {
        this.LastQRMagick = new Magick.MagicSkill(DateTime.Now.ToString());
    }
}
