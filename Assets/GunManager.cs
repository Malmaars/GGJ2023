using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class will take care of how the gun functions based on the grid the player filled in. Should be active at all times
public static class GunManager
{
    public static Shootable[] bulletsToShoot = new Shootable[0];

    public static void Initialize()
    {
        //    bulletsToShoot[0].SetStats(1, 15);
        //    bulletsToShoot[1].SetStats(1, 5);
    }


    public static void UpdateBulletToShoot(Shootable[] newBulletsToShoot)
    {
        bulletsToShoot = newBulletsToShoot;
    }
}
