using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGridTile : MonoBehaviour
{
    public Tileable tileableOverlap;
    int xPos, yPos;
    public Tileable TileableOnThisTile;
    GunGridManager gridManager;

    public bool overlapped;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GunGridManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TileableOnThisTile != null && TileableOnThisTile.PickedUp)
        {
            RemoveTileable();
        }
    }

    public void RemoveTileable()
    {
        if (TileableOnThisTile.extraOffsets.Length > 0)
        {
            for (int i = 0; i < TileableOnThisTile.extraOffsets.Length; i++)
            {
                //give each tile an overlap bool;
                gridManager.grid[xPos + Mathf.RoundToInt(TileableOnThisTile.extraOffsets[i].y), yPos + Mathf.RoundToInt(TileableOnThisTile.extraOffsets[i].x)].overlapped = false;
            }
        }

        TileableOnThisTile.onTile = false;
        TileableOnThisTile = null;
    }

    public void LetMeKnowMyGridPos(int _xPos, int _yPos)
    {
        xPos = _xPos;
        yPos = _yPos;
    }

    public void SnapTo(Transform _toSnap)
    {
        _toSnap.position = transform.position;

        //check if it overlaps others as well
    }

    public void GetTileableInfo(Tileable _tile)
    {
        TileableOnThisTile = _tile;

        if (TileableOnThisTile.tileType == TileableType.decoration)
            TileableOnThisTile.checkSmallIcons();
    }

    public bool CheckForOverlap()
    {
        if (TileableOnThisTile.extraOffsets.Length > 0)
        {
            for (int i = 0; i < TileableOnThisTile.extraOffsets.Length; i++)
            {
                //check for each offset a tile
                if (xPos + Mathf.RoundToInt(TileableOnThisTile.extraOffsets[i].y) < 0 || yPos + Mathf.RoundToInt(TileableOnThisTile.extraOffsets[i].x) < 0 || yPos + Mathf.RoundToInt(TileableOnThisTile.extraOffsets[i].x) > gridManager.grid.GetLength(1) || xPos + Mathf.RoundToInt(TileableOnThisTile.extraOffsets[i].y) > gridManager.grid.GetLength(1))
                {
                    return true;
                }

                if (gridManager.CheckIfTileIsOccupied(gridManager.grid[xPos + Mathf.RoundToInt(TileableOnThisTile.extraOffsets[i].y), yPos + Mathf.RoundToInt(TileableOnThisTile.extraOffsets[i].x)]))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void SetOverlap()
    {
        if (TileableOnThisTile.extraOffsets.Length > 0)
        {
            for (int i = 0; i < TileableOnThisTile.extraOffsets.Length; i++)
            {
                //give each tile an overlap bool;
                gridManager.grid[xPos + Mathf.RoundToInt(TileableOnThisTile.extraOffsets[i].y), yPos + Mathf.RoundToInt(TileableOnThisTile.extraOffsets[i].x)].overlapped = true;
            }
        }
    }

    void Reject()
    {
        TileableOnThisTile.onTile = false;
        TileableOnThisTile = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(TileableOnThisTile != null || tileableOverlap != null)
        {
            return;
        }

        if(gridManager.CheckIfTileIsOccupied(gridManager.grid[xPos, yPos]))
        {
            return;
        }

        if (collision.gameObject.tag == "Tileable")
        {
            if (Vector2.Distance(transform.position, collision.transform.position) < 0.5f)
            {
                GetTileableInfo(collision.gameObject.GetComponent<Tileable>());
                TileableOnThisTile.onTile = true;
                if (CheckForOverlap())
                {
                    Debug.Log("REJECT");
                    Reject();
                    return;
                }
                SetOverlap();
                SnapTo(collision.transform);

            }
        }
    }

}
