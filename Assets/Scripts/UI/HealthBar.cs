using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : Bar
{
    Medic medic;

    private void Start()
    {
        medic = FindObjectOfType<Medic>();
    }

    private void Update()
    {
        Fill(medic.CurrentHealth, medic.MaxHealth);
    }
}
