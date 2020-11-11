using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    private void Start()
    {
        EventManager.current.OnVictory += Collapse;
    }

    private void OnDestroy()
    {
        EventManager.current.OnVictory -= Collapse;
    }

    public override void Collapse()
    {
        Destroy(gameObject);
    }
}
