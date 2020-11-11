using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionHeal : PotionThrowable
{
    [Header("Healing potion")]
    [SerializeField] int smallAmount = 1;
    [SerializeField] int amount = 3;

    public override void Collapse()
    {
        if (isThrowed)
        {
            medic.Heal(amount);
        }
        else
        {
            medic.Heal(smallAmount);            
        }

        base.Collapse();
    }

    public override IEnumerator ThrowPotion()
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;

        while (transform.position != nextPos)
        {
            nextPos = medic.transform.position;
            markInstance.transform.position = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, nextPos, superSpeed * Time.deltaTime);

            Debug.DrawLine(transform.position, nextPos);
            yield return null;
        }
    }
}
