using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int health;

    [SerializeField] 
    private Image[] hearts;

    [SerializeField] 
    private Sprite fullHeart;

    [SerializeField] 
    private Sprite emptyHeart;

    public bool RemoveOneHealth()
    {
        if (health <= 0) return false;
        hearts[health-1].sprite = emptyHeart;
        return true;
    }

    public bool ResetHealth()
    {
        if (health > 0) return false;
        hearts.All(heart => heart.sprite = fullHeart);
        return true;
    }
}
