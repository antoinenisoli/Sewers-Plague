using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Spell
{
    public string Name;
    public string ButtonName;
    public int potionIndex;
    public int ManaCost = 0;
    public Image Icon;
    public ParticleSystem VFX;

    public void Execute()
    {
        VFX.Play();
        Icon.GetComponentInChildren<Animator>().SetTrigger("Input");
    }
}
