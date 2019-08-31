using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public abstract class ObserverUnsubscriber<TType>: MonoBehaviour
{
    private List<Observer<TType>> _observers;
    private Observer<TType> _observer;

    public void connect(List<Observer<TType>> observers, Observer<TType> observer)
    {
        this._observers = observers;
        this._observer = observer;
    }

    public void Dispose()
    {
        if (_observer != null && _observers.Contains(_observer))
            _observers.Remove(_observer);
        Destroy(this);
    }
}

public class FloatObsUnsubscriber : ObserverUnsubscriber<float>
{
    private List<FloatObserver> _observers;
    private FloatObserver _observer;

    public void connect(List<FloatObserver> observers, FloatObserver observer)
    {
        this._observers = observers;
        this._observer = observer;
    }
}
public class IntObsUnsubscriber : ObserverUnsubscriber<int>
{
    private List<IntObserver> _observers;
    private IntObserver _observer;

    public void connect(List<IntObserver> observers, IntObserver observer)
    {
        this._observers = observers;
        this._observer = observer;
    }
}
public class GameObjectObsUnsubscriber : ObserverUnsubscriber<GameObject>
{
    private List<GameObjectObserver> _observers;
    private GameObjectObserver _observer;

    public void connect(List<GameObjectObserver> observers, GameObjectObserver observer)
    {
        this._observers = observers;
        this._observer = observer;
    }
}
public abstract class AdjusterUnsubscriber<TType> : MonoBehaviour
{
    private List<Adjuster<TType>> _observers;
    private Adjuster<TType> _observer;

    public void connect(List<Adjuster<TType>> observers, Adjuster<TType> observer)
    {
        this._observers = observers;
        this._observer = observer;
    }

    public void Dispose()
    {
        if (_observer != null && _observers.Contains(_observer))
            _observers.Remove(_observer);
        Destroy(this);
    }
}

public class AdjusterFloatUnsubscriber : AdjusterUnsubscriber<float>
{
    private List<FloatAdjuster> _observers;
    private FloatAdjuster _observer;

    public void connect(List<FloatAdjuster> observers, FloatAdjuster observer)
    {
        this._observers = observers;
        this._observer = observer;
    }
}
*/
