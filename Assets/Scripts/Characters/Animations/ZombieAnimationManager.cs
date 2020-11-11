using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationManager : MonoBehaviour
{
    Ennemy zombie;

    private void Awake()
    {
        zombie = GetComponentInParent<Ennemy>();
    }

    public void Shoot()
    {
        zombie.Shoot();
    }

    public void Die()
    {
        Destroy(zombie.gameObject);
    }
}
