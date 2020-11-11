using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyBigZombie : Ennemy
{
    int step = 3;

    [Header("/ BIG ZOMBIE /")]
    [SerializeField] float blastRate = 0.5f;

    public override void Shoot()
    {
        step = 3;
        StartCoroutine(Blast());
    }

    IEnumerator Blast()
    {
        while (step > 0)
        {
            step--;
            yield return new WaitForSeconds(blastRate);
            GameObject shot = Instantiate(bullet, shootPos.position, transform.rotation);
            float random = Random.Range(-0.1f, 0.1f);
            shot.GetComponent<Rigidbody2D>().velocity = (-transform.right + new Vector3(0, random)) * bulletSpeed;
        }
    }
}
