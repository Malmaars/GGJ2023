using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : IPoolable
{
    public GameObject body;
    public bool active { get; set; }

    public int baseDamage;
    public float baseSpeed;

    public int damage;
    public float speed;

    public void SetStats(int _damage, float _speed)
    {
        baseDamage = _damage;
        baseSpeed = _speed;

        damage = baseDamage;
        speed = baseSpeed;
    }

    //maybe other parameters as well
    public void Decorate(int _damage, float _speed)
    {
        damage += _damage;
        speed += _speed;
    }

    public void ResetToBase()
    {
        damage = baseDamage;
        speed = baseSpeed;
    }

    public virtual void OnEnableObject()
    {
        if (body != null)
            body.SetActive(true);
    }
    public virtual void OnDisableObject()
    {
        if (body != null)
            body.SetActive(false);
    }

}
