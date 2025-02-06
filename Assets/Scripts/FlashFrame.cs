using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashFrame : MonoBehaviour
{
    [SerializeField] Material fullScreen;

    private void OnEnable()
    {
        fullScreen.SetFloat("_Toggle", 0);
    }
    private void OnDisable()
    {
        fullScreen.SetFloat("_Toggle", 1);
    }
}
