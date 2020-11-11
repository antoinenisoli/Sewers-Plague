using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionShield : Potion
{
    Shield playerShield;
    [Header("Shield potion")]
    [SerializeField] float duration = 5;

    public override void Collapse()
    {
        playerShield = FindObjectOfType<Shield>();
        playerShield.StartCoroutine(playerShield.Invincibility(duration));
        base.Collapse();
    }
}
