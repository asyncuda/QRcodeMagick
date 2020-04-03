using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

using Magick;

public class BaseWizardComponent : MonoBehaviour, IHitPoint, IWizard
{
    public ReactiveProperty<float>          CurrentHitPoint { get; protected set; }

    public ReactiveProperty<float>          MaxHitPoint     { get; protected set; }

    public ReadOnlyReactiveProperty<float>  LifePercentage  { get; protected set; }

    public MagicSkill                       LastQRMagick    { get; protected set; }

    public ReactiveProperty<string>         Name            { get; protected set; }

    public ReactiveProperty<string>         Attribute       { get; protected set; }


    [SerializeField] private string         TestName        = string.Empty;

    [SerializeField] private float          TestHp          = 0f;

    [SerializeField] private string         TestAttribute   = string.Empty;


    protected virtual void Awake()
    {
        Attribute = new ReactiveProperty<string>(TestAttribute);

        Name = new ReactiveProperty<string>(TestName);

        MaxHitPoint = new ReactiveProperty<float>(TestHp);

        CurrentHitPoint = new ReactiveProperty<float>(TestHp);
    }


    protected virtual void Start()
    {
        LifePercentage = Observable.CombineLatest(CurrentHitPoint, MaxHitPoint)
            .Where(_ => _[0] <= _[1])
            .Select(_ => _[0] / _[1] * 100.0f)
            .ToReadOnlyReactiveProperty();

        MaxHitPoint
              .Where(mhp => mhp < CurrentHitPoint.Value)
              .Subscribe(mhp => CurrentHitPoint.Value = mhp);

        CurrentHitPoint
            .Where(chp => MaxHitPoint.Value < chp)
            .Select(chp => MaxHitPoint.Value);

        LifePercentage
            .Subscribe(_ => Debug.Log(CurrentHitPoint.Value + "/" + MaxHitPoint.Value + " : " + _ + "%"));
    }

    public virtual void Kill()
    {
        CurrentHitPoint.Value = 0.0f;
    }

    public virtual void CallingMagickSkill()
    {
        LastQRMagick = new MagicSkill(string.Empty);
    }

    public virtual void UsingMagickSkill(GameObject target)
    {
        float damage = LastQRMagick.Power * ((this.Attribute.Value == LastQRMagick.Attribute) ? 1.5f : 1f);

        IHitPoint targetHp = target.GetComponent<IHitPoint>();

        targetHp.CurrentHitPoint.Value -= damage;
    }

    public virtual void UsingMagickSkill()
    {
        UsingMagickSkill(this.gameObject);
    }
}
