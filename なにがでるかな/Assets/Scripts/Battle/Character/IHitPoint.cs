using UniRx;

public interface IHitPoint
{
    ReactiveProperty<float> CurrentHitPoint { get; }

    ReactiveProperty<float> MaxHitPoint { get; }

    ReadOnlyReactiveProperty<float> LifePercentage { get; }

    void Kill();
}
