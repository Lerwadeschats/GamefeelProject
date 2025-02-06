using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    [SerializeField]
    List<Image> lifeDisplay;
    int currentLife = 3;
    int maxLife = 5;
    // Update is called once per frame

    private void Awake()
    {
        for (int i = 0; i < maxLife; i++) {
            if (i < currentLife) {
                lifeDisplay[i].color = new Color(1, 1, 1, 1);
            } else {
                lifeDisplay[i].color = new Color(1, 1, 1, 0);
            }
        }
    }
    
    public int GetMaxLife()
    {
        return maxLife;
    }
    
    public int GetCurrentLife()
    {
        return currentLife;
    }
    
    public void UpdateDisplay()
    {
        if (currentLife == 0) {
            Debug.Log("Game Over");
        }
        
        currentLife--;
        Debug.Log($"Life: {currentLife} / {maxLife} Display: {lifeDisplay.Count}");
        lifeDisplay[currentLife].color= new Color(1, 1, 1, 0);
        if (currentLife < 0) {
            currentLife = 0;
        }
    }

    public void UpdateDisplay(int health)
    {
        currentLife = health;
        for (int i = 0; i < maxLife; i++) {
            if (i < currentLife) {
                lifeDisplay[i].color = new Color(1, 1, 1, 1);
            } else {
                lifeDisplay[i].color = new Color(1, 1, 1, 0);
            }
        }
    }
}
