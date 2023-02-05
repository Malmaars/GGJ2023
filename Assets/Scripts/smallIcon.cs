using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallIcon : MonoBehaviour
{
    SpriteRenderer spriteRen;

    private void Awake()
    {
        spriteRen = GetComponent<SpriteRenderer>();
    }
    public void CheckForTile()
    {
        spriteRen.enabled = false;
        Collider2D[] hitCol = Physics2D.OverlapCircleAll(transform.position, 0.01f);
        foreach (Collider2D col in hitCol)
        {
            if(col.gameObject.GetComponent<GunGridTile>() != null)
            {
                //we're on a tile, show yourself
                spriteRen.enabled = true;
            }
        }
    }

    public void Hide()
    {
        spriteRen.enabled = false;
    }
}
