using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [HideInInspector]
    public Animator anim;

    protected SpriteRenderer spr;
    protected bool gameOver;
    protected bool isHit;

    [Header("Character")]
    public bool isDead;
    [SerializeField] protected float speed;    
    int currentHealth;
    [SerializeField] int maxHealth;    
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int CurrentHealth { get => currentHealth;
        set
        {
            if (value > MaxHealth)
            {
                value = MaxHealth;
            }

            if (value <= 0)
            {
                value = 0;
                Death();
            }

            currentHealth = value;
        }
    }

    public void Awake()
    {
        CurrentHealth = MaxHealth;
        anim = GetComponentInChildren<Animator>();
    }

    public virtual void Start()
    {
        EventManager.current.OnEndGame += GameOver;
        spr = GetComponentInChildren<SpriteRenderer>();
    }

    public void GameOver()
    {
        gameOver = true;
    }

    public virtual void Death()
    {
        isDead = true;
    }

    public void Heal(int amount)
    {
        if (!isDead)
        {
            CurrentHealth += amount;
        }
    }

    public virtual void TakeDamage(int amount)
    {
        if (!isDead)
        {
            CurrentHealth -= amount;
            StartCoroutine(Hit());
        }
    }

    public virtual void Update()
    {
        if (anim != null)
        {
            anim.SetBool("Dead", isDead);
        }
    }

    public IEnumerator Hit()
    {
        isHit = true;        
        spr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spr.color = Color.white;
        isHit = false;
    }
}
