using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public SpriteRenderer[] Healths;

    public int healthAmount;

    private void Awake()
    {
        healthAmount = Healths.Length;
    }

    public void RemoveOrAddHealth(int amountToRemoveOrAdd)
    {
        healthAmount += amountToRemoveOrAdd;

        if (healthAmount > Healths.Length)
            healthAmount = Healths.Length;

        if(healthAmount == 0)
        {
            //game over
        }

        for(int i = 0; i < Healths.Length; i++)
        {
            if( i >= healthAmount)
            {
                Healths[i].sprite = emptyHeart;
            }

            else
            {
                Healths[i].sprite = fullHeart;
            }
        }
    }
}
