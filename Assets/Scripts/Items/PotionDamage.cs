using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDamage : PotionThrowable
{
    [Header("Damage potion")]
    [SerializeField] int amount = 1;
    [SerializeField] float explosionRadius = 1;
    [SerializeField] float stunDuration = 0.5f;
    [SerializeField] LayerMask playerMask;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public override void Effect(Character target)
    {
        if (isThrowed)
        {
            target.TakeDamage(amount);
            base.Effect(target);
        }
    }

    public override IEnumerator ThrowPotion()
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;

        while (transform.position != nextPos)
        {
            if (closestEnnemy == null)
            {
                nextPos = new Vector3(10, transform.position.y);
            }
            else
            {
                nextPos = closestEnnemy.transform.position;
            }

            markInstance.transform.position = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, nextPos, superSpeed * Time.deltaTime);

            Debug.DrawLine(transform.position, nextPos);
            yield return null;
        }
    }

    public override void Collapse()
    {
        Collider2D target = Physics2D.OverlapCircle(transform.position, explosionRadius, playerMask);        

        if (target != null)
        {
            PlayerController player = target.GetComponent<PlayerController>();
            player.StartCoroutine(player.Stun(stunDuration));
        }

        base.Collapse();
    }
}
