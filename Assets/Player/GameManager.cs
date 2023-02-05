using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cursor, reticle;
    //one of the two is active. Never both
    public Player topDownPlayer;
    public GameObject gridPlayer;

    public Transform gunTransform;

    GameStateMachine stateMachine;
    public GunGridManager gridManager;

    public GameObject collecitbleTileParent;

    public Transform[] tileSpawnLocations;
    public GameObject fileMenu;
    public Sprite brokeFileMenu;

    public GameObject GameOverMenu;

    private void Awake()
    {
        //stateMachine = new GameStateMachine()
        GunManager.Initialize();
        EnemyHandler.Initialize();
        GunManager.Initialize();
        GunManager.tileParent = collecitbleTileParent;
        GunManager.spawnLocations = tileSpawnLocations;
        GunManager.fileMenu = fileMenu;
        GunManager.brokeFileMenu = brokeFileMenu;

        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchPlayerState();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && gridPlayer.activeSelf)
        {
            SwitchPlayerState();
        }

        if(topDownPlayer.playerHealth.healthAmount <= 0)
        {
            GameOverMenu.SetActive(true);
        }
    }

    void SwitchPlayerState()
    {
        //turn off one of the player systems, turn on the other

        if (topDownPlayer.enabled)
        {
            //switch to gridplayer
            gridPlayer.SetActive(true);
            topDownPlayer.enabled = false;

            //place the menu just above the gun
            gridPlayer.transform.position = gunTransform.position + new Vector3(0, 3.2f, -3);

            //activate cursor, disable reticle
            cursor.SetActive(true);
            reticle.SetActive(false);
        }

        else if (gridPlayer.activeSelf)
        {
            gridManager.UpdateGunProperties();
            gridPlayer.SetActive(false);
            topDownPlayer.enabled = true;

            cursor.SetActive(false);
            reticle.SetActive(true);
        }
    }
}
