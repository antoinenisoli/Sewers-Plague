using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class SpellsManager : MonoBehaviour
{
    PlayerController player;
    Medic medic;

    [Header("MANA")]
    int currentMana;
    public int CurrentMana
    {
        get => currentMana;
        set
        {
            if (value > MaxMana)
            {
                value = MaxMana;
            }

            if (value < 0)
            {
                value = 0;
            }

            currentMana = value;
        }
    }
    [SerializeField] int maxMana = 100;
    public int MaxMana { get => maxMana; set => maxMana = value; }

    [Header("SPELLS")]
    [SerializeField] List<Spell> spellsList;

    [Header("UI")]
    [SerializeField] Color GreyColor;
    public Image DashIcon;

    private void Awake()
    {
        medic = FindObjectOfType<Medic>();
        player = GetComponent<PlayerController>();
    }

    void DisplayDash()
    {
        if (player.canDash)
        {
            DashIcon.color = Color.white;
        }
        else
        {
            DashIcon.color = GreyColor;
        }
    }

    void DoSpell(Spell spell)
    {
        if (CurrentMana >= spell.ManaCost)
        {
            spell.Icon.color = Color.white;

            if (Input.GetButtonDown(spell.ButtonName))
            {
                spell.Execute();
                CurrentMana -= spell.ManaCost;
                medic.SpawnPotion(spell.potionIndex, transform.position);
            }
        }
        else
        {
            spell.Icon.color = GreyColor;
        }
    }

    private void Update()
    {
        if (!medic.isDead)
        {
            foreach (Spell spell in spellsList)
            {
                DoSpell(spell);
            }

            DisplayDash();
        }
    }
}
