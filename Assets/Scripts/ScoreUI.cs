using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] UnityEvent _onUpdateScore;

    private void Start()
    {
        UpdateScore();
    }
    public void UpdateScore()
    {
        if (GameManager.Instance.Score > 0)
        {
            _onUpdateScore.Invoke();
            Debug.Log("caca");
        }
        text.text = " " + GameManager.Instance.Score * 100;
    }
}