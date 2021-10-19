using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _health;

    [SerializeField] 
    private Image[] hearts;

    [SerializeField] 
    private Sprite fullHeart;

    [SerializeField] 
    private Sprite emptyHeart;

    public bool RemoveOneHealth()
    {
        if (_health <= 0) return false;
        hearts[_health-1].sprite = emptyHeart;
        _health--;
        return true;
    }

    public bool ResetHealth()
    {
        if (health > 0) return false;
        _health = hearts.Length;
        hearts.All(heart => heart.sprite = fullHeart);
        return true;
    }

    public int health
    {
        get
        {
            return _health;
        }
    }
    
    
}
