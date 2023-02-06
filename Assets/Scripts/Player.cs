using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float baseSpeed;
    public float moveSpeed;
    public float timeBetweenShots;
    float shotTimer;

    public float stunDuration;
    float stunTimer;

    public int health;

    public Animator playerAnimator;
    public Animator gunAnimator;
    public Collider2D playerCol;

    public Transform gun;

    public GameObject bulletPrefab;
    //shoot the bullets from here
    public Transform barrel;

    ObjectPool<BullletShootable> bulletPool;
    List<BullletShootable> activeBullets;

    ObjectPool<GrenadeShootable> grenadetPool;
    List<GrenadeShootable> activeGrenades;

    ObjectPool<RocketShootable> rocketPool;
    List<RocketShootable> activeRockets;

    public PlayerHealth playerHealth;

    public float gunDistanceFromPlayer;

    PlayerInputActions controls;

    public AudioSource source;
    public AudioClip gunShot;

    bool immune;
    // Start is called before the first frame update
    void Awake()
    {
        shotTimer = timeBetweenShots;
        controls = new PlayerInputActions();
        InputDistributor.inputActions = controls;
        InputManager.Initialize(
            new InputAction[]
            {
                controls.Player.Move,
                controls.Player.Shoot
            });

        //inputManager.AddActionToInput(InputDistributor.inputActions.Player.Move, Move);
        //InputManager.AddActionToInput(InputDistributor.inputActions.Player.Shoot, Shoot);

        bulletPool = new ObjectPool<BullletShootable>();
        activeBullets = new List<BullletShootable>();

        grenadetPool = new ObjectPool<GrenadeShootable>();
        activeGrenades = new List<GrenadeShootable>();

        rocketPool = new ObjectPool<RocketShootable>();
        activeRockets = new List<RocketShootable>();

        moveSpeed = baseSpeed;
    }


    private void OnEnable()
    {
        InputManager.WhenEnabled();
        //InputManager.AddActionToInput(InputDistributor.inputActions.Player.Shoot, Shoot);

    }

    private void OnDisable()
    {
        InputManager.WhenDisabled();
        //InputManager.RemoveActionFromInput(InputDistributor.inputActions.Player.Shoot, Shoot);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth.healthAmount <= 0)
        {
            return;
        }
        stunTimer -= Time.deltaTime;
        shotTimer-= Time.deltaTime;
        //we're moving the gameobject this script is on.
        Move();
        MoveGun(gun);

        for(int i = 0; i < activeBullets.Count; i++)
        {
            activeBullets[i].LogicUpdate();
            if (activeBullets[i].hit)
            {
                bulletPool.ReturnObjectToPool(activeBullets[i]);
                activeBullets.Remove(activeBullets[i]);
                i--;
            }
        }

        for (int i = 0; i < activeGrenades.Count; i++)
        {
            activeGrenades[i].LogicUpdate();
            if (activeGrenades[i].hit)
            {
                grenadetPool.ReturnObjectToPool(activeGrenades[i]);
                activeGrenades.Remove(activeGrenades[i]);
                i--;
            }
        }

        for (int i = 0; i < activeRockets.Count; i++)
        {
            activeRockets[i].LogicUpdate();
            if (activeRockets[i].hit)
            {
                rocketPool.ReturnObjectToPool(activeRockets[i]);
                activeRockets.Remove(activeRockets[i]);
                i--;
            }
        }

        if (Input.GetMouseButton(0))
        {
            Shoot();
        }

        if (Input.GetMouseButtonDown(1))
        {
            playerAnimator.SetTrigger("Roll");
            StartCoroutine(DodgeRoll());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(1);
        }
    }

    void Move()
    {
        if(stunTimer > 0)
        {
            return;
        }
        Vector2 inputDirection = InputDistributor.inputActions.Player.Move.ReadValue<Vector2>();

        if (inputDirection.x == 0 && inputDirection.y == 0)
        {
            playerAnimator.SetBool("Running", false);
        }

        else
        {
            playerAnimator.SetBool("Running", true);
        }
        //Debug.Log(inputDirection);

        float Horizontal = inputDirection.x * moveSpeed * Time.deltaTime;
        float Vertical = inputDirection.y * moveSpeed * Time.deltaTime;

        transform.position += new Vector3(Horizontal, Vertical, 0);
    }

    void MoveGun(Transform _toMove)
    {
        Vector2 transformPosition2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToMouse = mousePosition - transformPosition2D;
        _toMove.position = transformPosition2D + directionToMouse.normalized * gunDistanceFromPlayer;
        _toMove.right = directionToMouse.normalized;

        if(_toMove.rotation.eulerAngles.z > 90 && _toMove.rotation.eulerAngles.z < 270)
        {
            _toMove.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            _toMove.GetComponent<SpriteRenderer>().flipY = false;
        }
    }

    public void Shoot()
    {
        if (shotTimer < 0 && !immune && GunManager.bulletsToShoot.Length > 0 && stunTimer < 0)
        {

            StartCoroutine(shootBurst());
            shotTimer = timeBetweenShots;
            source.PlayOneShot(gunShot);
        }
    }

    public void ShootShootable(Shootable _toShoot)
    {
        if(_toShoot.GetType() == typeof(BullletShootable))
        {
            BullletShootable newBullet = bulletPool.RequestItem();
            Vector2 transformPosition2D = new Vector2(transform.position.x, transform.position.y);
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = mousePosition - transformPosition2D;
            newBullet.SetStats(_toShoot.damage, _toShoot.speed);
            newBullet.MakeBullet(barrel.transform.position, directionToMouse);
            activeBullets.Add(newBullet);
        }

        if (_toShoot.GetType() == typeof(GrenadeShootable))
        {
            GrenadeShootable newGrenade = grenadetPool.RequestItem();
            Vector2 transformPosition2D = new Vector2(transform.position.x, transform.position.y);
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = mousePosition - transformPosition2D;
            newGrenade.SetStats(_toShoot.damage, _toShoot.speed);
            newGrenade.MakeBullet(barrel.transform.position, directionToMouse);
            activeGrenades.Add(newGrenade);
        }

        if (_toShoot.GetType() == typeof(RocketShootable))
        {
            RocketShootable newRocket = rocketPool.RequestItem();
            Vector2 transformPosition2D = new Vector2(transform.position.x, transform.position.y);
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 directionToMouse = mousePosition - transformPosition2D;
            newRocket.SetStats(_toShoot.damage, _toShoot.speed);
            newRocket.MakeBullet(barrel.transform.position, directionToMouse);
            activeRockets.Add(newRocket);
        }
    }

    public void TakeDamage(int _damageAmount)
    {
        if (immune)
        {
            return;
        }

        stunTimer = stunDuration;
        health -= _damageAmount;
        playerHealth.RemoveOrAddHealth(-_damageAmount);

        playerAnimator.SetTrigger("Hit");
    }

    IEnumerator shootBurst()
    {
        gunAnimator.SetTrigger("Shoot");
        for (int i = 0; i < GunManager.bulletsToShoot.Length; i++)
        {
            ShootShootable(GunManager.bulletsToShoot[i]);
            yield return new WaitForSeconds(0.1f);
        }

        gunAnimator.SetTrigger("Reload");
    }

    IEnumerator DodgeRoll()
    {
        moveSpeed += 2;
        //playerCol.enabled = false;
        immune = true;
        yield return new WaitForSeconds(0.8f);
        immune = false;
        //playerCol.enabled = true;
        moveSpeed = baseSpeed;
    }
}
