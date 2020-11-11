using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medic : Character
{
    bool stopped;
    Vector2 destination;
    Potion potion;
    bool inIdle;

    [Header("Idle")]
    [SerializeField] float idleDuration;
    [SerializeField] float idlePropbability = 10;

    [Header("Potions spawn")]
    [SerializeField] float potionRate;
    [SerializeField] List<GameObject> items;
    [SerializeField] Transform spawnArea;
    [SerializeField] float randomCoordinate = 4;
    [SerializeField] Transform pos01, pos02;

    [Header("Sounds")]
    [SerializeField] AudioSource hitSound;

    private void OnDrawGizmos()
    {
        if (spawnArea != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector2(spawnArea.position.x + randomCoordinate, spawnArea.transform.position.y), new Vector2(spawnArea.position.x - randomCoordinate, spawnArea.transform.position.y));
            Gizmos.DrawLine(new Vector2(spawnArea.position.x, spawnArea.transform.position.y + randomCoordinate), new Vector2(spawnArea.position.x, spawnArea.transform.position.y - randomCoordinate));
        }
    }

    public override void Start()
    {
        base.Start();
        StartCoroutine(Cycle());
        StartCoroutine(MovePattern());

        transform.position = pos01.position;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    public override void Death()
    {
        base.Death();
        EventManager.current.GameOver();
    }

    void Move()
    {
        if (transform.position == pos01.position)
        {
            destination = pos02.position;
        }
        else if (transform.position == pos02.position)
        {
            destination = pos01.position;
        }

        if (!stopped && !inIdle && !isDead)
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    public Vector3 RandomDestination()
    {        
        float random = Random.Range(-randomCoordinate, randomCoordinate);
        Vector3 generated = new Vector3(spawnArea.transform.position.x + random, spawnArea.transform.position.y + random);
        return generated;
    }

    public void SpawnPotion(int potionIndex, Vector3 target)
    {
        GameObject spawned = Instantiate(items[potionIndex], transform.position, Quaternion.identity);        
        potion = spawned.GetComponent<Potion>();
        potion.nextPos = target;
        potion.medic = this; 

        Vector3 calculatedVelocity = CalculateVelocity(target, transform.position, potion.fallSpeed);
        potion.rb.velocity = calculatedVelocity;
    }

    IEnumerator MovePattern()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            int random = Random.Range(0, 100);

            if (random < idlePropbability)
            {
                inIdle = true;
                yield return new WaitForSeconds(idleDuration);
                inIdle = false;
            }
        }
    }

    IEnumerator RandomBombThrow()
    {
        if (!stopped)
        {
            stopped = true;            
            yield return new WaitForSeconds(0.2f);
            if (!isDead)
            {
                SpawnPotion(0, RandomDestination());
            }
            yield return new WaitForSeconds(0.5f);
            stopped = false;
        }
    }

    IEnumerator Cycle()
    {
        while(!isDead)
        {
            yield return new WaitForSeconds(potionRate);
            StartCoroutine(RandomBombThrow());
        }
    }

    Vector3 CalculateVelocity(Vector2 target, Vector2 origin, float time)
    {
        Vector2 distance = target - origin;
        Vector2 distanceXZ = distance;
        distanceXZ.y = 0;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time;

        Vector2 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    public override void TakeDamage(int amount)
    {
        if (!isDead)
        {
            CurrentHealth -= amount;

            if (!hitSound.isPlaying)
            {
                hitSound.Play();
            }

            StartCoroutine(Hit());
        }
    }

    public override void Update()
    {
        base.Update();
        anim.SetBool("Throw", stopped);
        anim.SetBool("Stopped", inIdle);
        Move();
    }
}
