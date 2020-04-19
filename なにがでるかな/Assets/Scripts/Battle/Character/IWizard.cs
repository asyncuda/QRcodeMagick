using UnityEngine;
using Magick;

interface IWizard
{
    MagicSkill LastQRMagick { get; }

    void CallingMagickSkill();


    void UsingMagickSkill(GameObject target);

    void UsingMagickSkill();
}
