using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionThrowable : Potion
{
    SpriteRenderer spr;
    SpawnEnnemies spawner;

    [Header("--THROW--")]
    [SerializeField] protected float superSpeed = 14;
    [SerializeField] Color breakColor;
    
    [Header("Target")]
    public Ennemy closestEnnemy;
    [SerializeField] float distanceToTarget;

    public override void Start()
    {
        spawner = FindObjectOfType<SpawnEnnemies>();
        spr = GetComponentInChildren<SpriteRenderer>();
        base.Start();
    }    

    public virtual void Effect(Character target)
    {
        Collapse();
    }    

    void FindClosestEnnemy()
    {
        float distanceToClosest = Mathf.Infinity;
        closestEnnemy = null;

        if (spawner.currentEnnemies.Length > 0)
        {
            foreach (Ennemy mob in spawner.currentEnnemies)
            {
                distanceToTarget = (mob.transform.position - transform.position).sqrMagnitude;

                if (distanceToTarget < distanceToClosest)
                {
                    distanceToClosest = distanceToTarget;
                    closestEnnemy = mob;
                }
            }

            if (closestEnnemy != null)
            {
                Debug.DrawLine(transform.position, closestEnnemy.transform.position, Color.red);
            }
        }
        else
        {
            closestEnnemy = null;
        }
    }    

    public virtual IEnumerator ThrowPotion()
    {
        yield return null;
    }

    public override void CheckDistance()
    {
        base.CheckDistance();        

        if (canBreak)
        {
            spr.color = breakColor;
        }
        else
        {
            spr.color = Color.white;
        }
    }

    public void Throw()
    {
        isThrowed = true;
        canBreak = false;
        StartCoroutine(ThrowPotion());
    }

    private void Update()
    {
        CheckDistance();
        FindClosestEnnemy();
    }
}
