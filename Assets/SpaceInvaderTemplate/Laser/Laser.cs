using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Laser : MonoBehaviour
{
    GameObject _beam;

    [SerializeField]
    float _chargingTime = 3f;

    [SerializeField]
    float _duration = 0.5f;

    [SerializeField]
    ParticleSystem _chargingEffect;

    Coroutine _coroutine;

    bool _isReady;
    

    private void Awake()
    {
        _beam = transform.Find("Beam").gameObject;
        _beam.SetActive(false);
    }
    public void OnActivation()
    {
        _coroutine = StartCoroutine(Charge());
        _chargingEffect.Play(true);
    }

    IEnumerator Charge()
    {
        yield return new WaitForSeconds(_chargingTime);
        _isReady = true;
        EventManager.Instance.onLaserReady?.Invoke();
    }

    public void OnRelease()
    {
        if (_isReady)
        {
            _coroutine = null;
            StartCoroutine(ActivationDuration());
        }
        else
        {
            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _chargingEffect.Stop(true);
            }
        }
        
    }

    IEnumerator ActivationDuration()
    {
        _beam.SetActive(true);
        yield return new WaitForSeconds(_duration);
        _beam.SetActive(false);
        EventManager.Instance.onPlayerExhausted?.Invoke();
    }
}
