using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        _beam = transform.Find("Beam").gameObject;
        _beam.SetActive(false);
    }
    public void OnActivation()
    {
        _coroutine = StartCoroutine(Charge());
        _chargingEffect.Play(true);
        print("eznjfkea");
    }

    public void OnCancel()
    {
        if(_coroutine != null)
        {
            _chargingEffect.Play(false);
            StopCoroutine(_coroutine);
        }
    }

    IEnumerator Charge()
    {
        //Start effect
        yield return new WaitForSeconds(_chargingTime);
        LaunchLaser();
        yield return null;
    }

    void LaunchLaser()
    {
        _coroutine = null;
        StartCoroutine(ActivationDuration());


    }

    IEnumerator ActivationDuration()
    {
        _beam.SetActive(true);

        yield return new WaitForSeconds(_duration);
        _beam.SetActive(false);
    }
}
