using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class will take care of how the gun functions based on the grid the player filled in. Should be active at all times
public static class GunManager
{
    public static Shootable[] bulletsToShoot = new Shootable[0];

    public static List<GameObject> ownedTiles;

    public static GameObject tileParent;
    public static Transform[] spawnLocations;

    public static GameObject fileMenu;

    public static Sprite brokeFileMenu;

    static int spawnIndex;

    public static void Initialize()
    {
        ownedTiles = new List<GameObject>();
        spawnIndex = 0;
        //    bulletsToShoot[0].SetStats(1, 15);
        //    bulletsToShoot[1].SetStats(1, 5);
    }


    public static void UpdateBulletToShoot(Shootable[] newBulletsToShoot)
    {
        bulletsToShoot = newBulletsToShoot;
    }

    public static void AddTileToCollection(GameObject _toAdd)
    {
        ownedTiles.Add(_toAdd);
        GameObject temp = Object.Instantiate(_toAdd, tileParent.transform);
        temp.transform.position = spawnLocations[0].position;
        spawnIndex++;

        //if(spawnIndex > 2)
        //{
        //    Debug.Log("CHANGE APPEARNACe");
        //    fileMenu.GetComponent<SpriteRenderer>().sprite = brokeFileMenu;
        //}
    }
}
