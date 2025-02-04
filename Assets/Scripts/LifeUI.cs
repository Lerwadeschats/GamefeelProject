using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    [SerializeField]
    List<Image> lifeDisplay;
    int currentLife=3;
    // Update is called once per frame
    public void UpdateDisplay()
    {
        currentLife--;
        lifeDisplay[currentLife].color= new Color(1, 1, 1, 0);
    }
}
