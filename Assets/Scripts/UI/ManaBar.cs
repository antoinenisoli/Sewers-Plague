using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBar : Bar
{
    SpellsManager spells;

    private void Start()
    {
        spells = FindObjectOfType<SpellsManager>();
    }

    private void Update()
    {
        Fill(spells.CurrentMana, spells.MaxMana);
    }
}
