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
    GameObject _chargeEffectObj;
    
    ParticleSystem _chargingEffect;
    ParticleSystem _chargingBall;
    ParticleSystem _bigHeart;

    Coroutine _coroutine;

    bool _isReady;

    Material _material;

    [SerializeField]
    AnimationCurve _beamEffectCurve, _beamSizeCurve;
    

    private void Awake()
    {
        _beam = transform.Find("Beam").gameObject;
        _material = _beam.GetComponent<SpriteRenderer>().material;
        _chargingBall = _chargeEffectObj.transform.GetChild(0).GetComponent<ParticleSystem>();
        _chargingEffect = _chargeEffectObj.transform.GetChild(1).GetComponent<ParticleSystem>();
        _bigHeart = _chargeEffectObj.transform.GetChild(2).GetComponent<ParticleSystem>();
        _beam.SetActive(false);
        
    }
    public void OnActivation()
    {
        _coroutine = StartCoroutine(Charge());
        _chargingEffect.Play();
        _chargingBall.Play();
    }

    IEnumerator Charge()
    {
        yield return new WaitForSeconds(_chargingTime);
        _isReady = true;
        EventManager.Instance.onLaserReady?.Invoke();
        _chargingEffect.Stop();
        _chargingBall.Pause();
       
    }

    public void OnRelease()
    {
        if (_isReady)
        {
            _chargingBall.Clear();
            _chargingBall.Stop();
            _coroutine = null;
            StartCoroutine(ActivationDuration());
            
        }
        else
        {
            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _chargingEffect.Stop();
                _chargingBall.Clear();
                _chargingBall.Stop();

            }
        }
        
    }

    IEnumerator ActivationDuration()
    {
        _bigHeart.Play();
        _beam.SetActive(true);
        EventManager.Instance.onLaserFire?.Invoke();
        float timer = 0;
        while(timer < _duration)
        {
            timer += Time.deltaTime;
            print(_material.GetFloat("_Power"));
            _material.SetFloat("_Power", _beamEffectCurve.Evaluate(timer/_duration));
            _material.SetFloat("_Size", _beamSizeCurve.Evaluate(timer / _duration));
            yield return null;
        }
        _beam.SetActive(false);
        _isReady = false;
        EventManager.Instance.onPlayerExhausted?.Invoke();
    }
}
