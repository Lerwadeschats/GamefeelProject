using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUIFinal : MonoBehaviour
{
    [SerializeField]TMP_Text text;
   [SerializeField] int _score=0;
    float timer = 0;

    private void Update()
    {
        if (timer < 3)
        {
            timer += Time.deltaTime/3;
        }
        if (timer > 4)
        {
            timer = 4;
        }
        _score = (int)Mathf.Lerp(0, GameManager.Instance.Score,timer/3) ;
        text.text = " " + _score*1000;
    }
}
