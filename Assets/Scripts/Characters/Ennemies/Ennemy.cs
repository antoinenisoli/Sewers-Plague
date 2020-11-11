using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : Character
{
    SpawnEnnemies spawner;
    protected Sounds sounds;

    [Header("--ENNEMY--")]
    public SpawnSlot mySlot;
    public bool Stopped;

    [Header("Shoot")]
    [SerializeField] protected float fireRate;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform shootPos;

    [Header("Sounds")]
    public int soundIndex;

    public override void Start()
    {
        base.Start();
        StartCoroutine(ShootingPattern());
        spawner = FindObjectOfType<SpawnEnnemies>();
        sounds = GetComponentInChildren<Sounds>();

        if (!sounds.growlList[soundIndex].isPlaying)
        {
            sounds.growlList[soundIndex].Play();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AllyBullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }

        PotionThrowable potion = other.GetComponentInParent<PotionThrowable>();
        if (potion)
        {
            potion.Effect(this);
        }
    }

    IEnumerator ShootingPattern()
    {
        while (!isDead && !gameOver)
        {
            yield return new WaitForSeconds(Random.Range(fireRate - 2, fireRate + 1));
            LaunchShoot();
        }
    }

    public IEnumerator ReachPosition(Transform direction)
    {
        while (transform.position != direction.position && !isDead)
        {            
            transform.position = Vector3.MoveTowards(transform.position, direction.position, speed * Time.deltaTime);
            yield return null;
        }  
        
        if (transform.position == direction.position)
        {
            Stopped = true;
        }
    }

    public override void Death()
    {
        base.Death();
        EventManager.current.AddScore();
        int rdm = Random.Range(0, sounds.deathList.Count);
        
        if (!sounds.deathList[rdm].isPlaying)
        {
            sounds.deathList[rdm].Play();
        }

        mySlot.isOccupied = false;
        spawner.slotList.Add(mySlot);
    }

    public override void TakeDamage(int amount)
    {
        if (!isHit && !isDead)
        {
            CurrentHealth -= amount;
            StartCoroutine(Hit());
            int rdm = Random.Range(0, sounds.impactList.Count);
            
            if (!sounds.impactList[rdm].isPlaying)
            {
                sounds.impactList[rdm].Play();
            }
        }
    }

    public void LaunchShoot()
    {
        if (!isDead)
        {
            anim.SetTrigger("Shoot");
        }
    }

    public virtual void Shoot()
    {
        GameObject shot = Instantiate(bullet, shootPos.position, transform.rotation);        
        float random = Random.Range(-0.1f, 0.1f);
        shot.GetComponent<Rigidbody2D>().velocity = (-transform.right + new Vector3(0,random)) * bulletSpeed;
    }

    public override void Update()
    {
        base.Update();
        anim.SetBool("Stopped", Stopped);
    }
}
