using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileableType
{
    thing,
    decoration
}

public enum ShootableType
{
    bullet,
    grenade,
    laser,
    fire,
    ice
}

public class Tileable : MonoBehaviour
{
    public TileableType tileType;
    public ShootableType shootType;

    public int damage;
    public float speed;

    public Shootable thisShootable;

    public GameObject body;
    Collider2D thisCol;

    public smallIcon[] smallIcons;
    public GameObject smallIconParent;

    public SpriteRenderer Block;
    public SpriteRenderer Icon;

    public bool onTile;

    public bool PickedUp;

    public bool wrong;

    public Vector2[] extraOffsets;

    // Start is called before the first frame update
    void Awake()
    {
        thisCol = GetComponent<Collider2D>();
        body = this.gameObject;

        SetShootable();

        thisShootable.SetStats(damage, speed);

        
    }

    private void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            wrong = true;
            StartCoroutine(WrongAnimator());
        }
    }

    void SetShootable()
    {
        switch (shootType)
        {
            case ShootableType.bullet:
                thisShootable = new BullletShootable();
                break;

            case ShootableType.fire:
                thisShootable = new FireShootable();
                break;

            case ShootableType.grenade:
                thisShootable = new GrenadeShootable();
                break;
        }

    }
    public void checkSmallIcons()
    {
        Collider2D[] hitCol = Physics2D.OverlapCircleAll(transform.position, 0.01f);
        foreach (Collider2D col in hitCol)
        {
            if (col.gameObject.GetComponent<GunGridTile>() != null)
            {
                //we're on a tile, show yourself
                foreach (smallIcon icon in smallIcons)
                {
                    icon.CheckForTile();
                }
            }
        }
    }

    public void HideAllSmallIcons()
    {
        foreach(smallIcon icon in smallIcons)
        {
            icon.Hide();
        }
    }

    public void OnGrab()
    {
        thisCol.enabled = false;
        PickedUp = true;
        Block.sortingOrder = 3;
        Icon.sortingOrder = 4;
        thisShootable.ResetToBase();
        HideAllSmallIcons();
    }

    public void OnRelease()
    {
        thisCol.enabled = true;
        PickedUp = false;
        Block.sortingOrder = 1;
        Icon.sortingOrder = 2;

        if(tileType == TileableType.decoration)
        {

            //add little smaller icons to surrounding tiles
        }
    }

    


    IEnumerator WrongAnimator()
    {
        int multiplier = 1;

        float timer = 0.5f;

        float miniTimer = 0.05f;
        while (wrong)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + Time.deltaTime * 300 * multiplier));

            miniTimer -= Time.deltaTime;
            if(miniTimer <= 0)
            {
                miniTimer = 0.05f;
                multiplier *= -1;
            }

            yield return new WaitForSeconds(0f);
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
                wrong = false;
            }

        }
    }
}
