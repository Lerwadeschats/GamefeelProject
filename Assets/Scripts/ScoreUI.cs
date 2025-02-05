using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]TMP_Text text;

    private void Start()
    {
        UpdateScore();
    }
    public void UpdateScore()
    {
        text.text = " " + GameManager.Instance.Score;
    }
}
