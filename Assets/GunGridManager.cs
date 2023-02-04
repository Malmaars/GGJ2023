using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGridManager : MonoBehaviour
{
    public GunGridTile[,] grid = new GunGridTile[3,3];
    public Transform gridParent;
    // Start is called before the first frame update
    void Awake()
    {
        //we initialize the grid here because why not

        GunGridTile[] gridChildren = gridParent.GetComponentsInChildren<GunGridTile>();

        int countingIndex = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int k = 0; k < 3; k++)
            {
                grid[i, k] = gridChildren[countingIndex];
                grid[i, k].LetMeKnowMyGridPos(i, k);
                countingIndex++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CheckIfTileIsOccupied(GunGridTile _tile)
    {
        if(_tile.TileableOnThisTile != null || _tile.tileableOverlap != null || _tile.overlapped)
        {
            return true;
        }

        return false;
    }

    public void UpdateGunProperties()
    {
        List<Shootable> toReturn = new List<Shootable>();
        for(int i = 0; i < 3; i++)
        {
            for(int k = 0; k < 3; k++)
            {
                //check what the tileable on that tile is and what its effects are
                if (grid[i,k].TileableOnThisTile != null)
                {
                    Tileable temp = grid[i, k].TileableOnThisTile;

                    if (temp.tileType == TileableType.thing)
                    {
                        //it's a shootable thing, check the area around it if it has any decorations, and at it to the list                    

                        //also check for decorations around it

                        temp.thisShootable.ResetToBase();
                        //now that we have a 2d array, we can check the tile around this
                        for(int p = -1; p <= 1; p++)
                        {
                            for (int t = -1; t <= 1; t ++)
                            {
                                if(t != 0 && p != 0)
                                {
                                    continue;
                                }

                                if (i + t < 0 || i + t >= grid.GetLength(0) || k + p < 0 || k + p >= grid.GetLength(1))
                                {
                                    continue;
                                }

                                if(grid[i + t, k + p].TileableOnThisTile == null)
                                {
                                    continue;
                                }

                                Tileable decoraterTemp = grid[i + t, k + p].TileableOnThisTile;

                                if(decoraterTemp.tileType == TileableType.decoration)
                                {
                                    Debug.Log("Decorating...");
                                    //apply a little decoration :)
                                    temp.thisShootable.Decorate(decoraterTemp.damage, decoraterTemp.speed);
                                }
                            }
                        }

                        toReturn.Add(temp.thisShootable);
                    }

                    else if(temp.tileType == TileableType.decoration)
                    {
                    }
                    //pass it through
                }
            }
        }

        Debug.Log(toReturn.Count);
        GunManager.UpdateBulletToShoot(toReturn.ToArray());
    }
}
