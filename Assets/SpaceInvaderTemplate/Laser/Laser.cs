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
    float _timeBeforeFire = 3.7f;

    [SerializeField]
    float _duration = 0.5f;

    [SerializeField]
    GameObject _chargeEffectObj;
    
    ParticleSystem _chargingEffect;
    ParticleSystem _chargingBall;
    ParticleSystem _bigHeart;

    [SerializeField]
    ParticleSystem[] _laserReadyEffects;

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
        if (GameManager.Instance._LaserVFX)
        {
            _chargingEffect.Play();
            _chargingBall.Play();
        }
    }

    IEnumerator Charge()
    {
        yield return new WaitForSeconds(_chargingTime);
        _isReady = true;
        EventManager.Instance.onLaserReady?.Invoke();
        if (GameManager.Instance._LaserVFX)
        {
            _chargingEffect.Stop();
            _chargingBall.Pause();
        }
        UpdateLaserReadyEffects(true);
    }

    public void OnRelease()
    {
        if (_isReady)
        {
            if (GameManager.Instance._LaserVFX)
            {
                _chargingBall.Clear();
                _chargingBall.Stop();
            }
            _coroutine = null;
            UpdateLaserReadyEffects(false);
            StartCoroutine(ActivationDuration());
            
        }
        else
        {
            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
                if (GameManager.Instance._LaserVFX)
                {
                    _chargingEffect.Stop();
                    _chargingBall.Clear();
                    _chargingBall.Stop();
                }
            }
        }
        
    }

    IEnumerator ActivationDuration()
    {
        
        
        EventManager.Instance.onLaserFire?.Invoke();

        yield return new WaitForSeconds(_timeBeforeFire);
        _bigHeart.Play();
        _beam.SetActive(true);
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



    public void UpdateLaserReadyEffects(bool isReady)
    {
        if (GameManager.Instance._LaserVFX)
        {
            foreach (ParticleSystem particle in _laserReadyEffects)
            {
                if (isReady)
                    particle.Play(true);

                else particle.Stop(true);
            }
        }
    }
}
