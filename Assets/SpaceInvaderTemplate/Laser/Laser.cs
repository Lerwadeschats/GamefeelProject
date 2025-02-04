using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    Collider2D collider;

    [SerializeField]
    float _chargingTime = 3f;

    [SerializeField]
    float _duration = 0.5f;

    Coroutine _coroutine;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }
    public void OnActivation()
    {
        _coroutine = StartCoroutine(Charge());
    }

    public void OnCancel()
    {
        if(_coroutine != null)
        {
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
        
    }

    IEnumerator ActivationDuration()
    {
        collider.enabled = true;
        yield return new WaitForSeconds(_duration);
        collider.enabled = false;
    }
}
