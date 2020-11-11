using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    Shield shield;
    Medic medic;
    SpellsManager spells;
    Rigidbody2D rb;

    [Header("PLAYER")]    
    public float speed;
    public float startSpeed;    
    float xInput;
    float yInput;
    Vector2 move;
    public bool Stunned;    

    [Header("Dash")]
    [SerializeField] GameObject DashAnim;
    [SerializeField] float dashForce;
    public bool canDash;
    bool endCooldown;
    public bool isDashing;
    [SerializeField] float dashTimer;
    [SerializeField] float dashCooldown;
    Vector3 _previousPos;
    Vector3 _currentPos;
    public Vector3 MoveDirection {
        get
        {
            return (_currentPos - _previousPos).normalized;
        }
    }

    [Header("VFX")]
    [SerializeField] ParticleSystem DashVFX;
    [SerializeField] ParticleSystem stunFX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        shield = GetComponentInChildren<Shield>();
        spells = GetComponentInChildren<SpellsManager>();
        startSpeed = speed;
        endCooldown = true;
    }

    private void Start()
    {
        EventManager.current.OnEndGame += Stun;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && !shield.useShield && !isDashing) //the hero si stunned by a bullet
        {
            StartCoroutine(Stun(0.3f));
            Destroy(collision.gameObject);
        }
    }

    void GetInputs()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    void Move()
    {
        anim.SetFloat("Speed", rb.velocity.magnitude);
        _previousPos = _currentPos;
        _currentPos = transform.position;
        move = new Vector2(xInput, yInput);

        if (!isDashing && !Stunned)
        {
            rb.velocity = move.normalized * speed;
        }
        
        if (Stunned)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void Dash()
    {
        if (Input.GetButtonDown("Dash") && canDash)
        {
            GainMana(-1);
            spells.DashIcon.GetComponentInChildren<Animator>().SetTrigger("Input");
            DashVFX.Play();
            StartCoroutine(DashStop());

            if (move.sqrMagnitude > 0)
            {
                Impulse(MoveDirection);
            }
            else
            {
                Impulse(Vector2.right);
            }
        }

        if (endCooldown && !isDashing && !Stunned && !shield.useShield && spells.CurrentMana > 0)
        {
            canDash = true;
        }
        else
        {
            canDash = false;
        }
    }

    void Impulse(Vector2 direction)
    {
        rb.AddForce(direction * dashForce, ForceMode2D.Impulse);
    }

    public void GainMana(int mana)
    {
        spells.CurrentMana += mana;
    }

    IEnumerator DashStop()
    {
        endCooldown = false;
        isDashing = true;
        yield return new WaitForSeconds(dashTimer);

        if (isDashing)
        {
            isDashing = false;
        }
        yield return new WaitForSeconds(dashCooldown);
        if (!endCooldown)
        {
            endCooldown = true;
        }
    }     
    
    public IEnumerator Stun(float time)
    {
        if (!Stunned && !shield.isInvincible)
        {
            stunFX.Play();
            CamShake camShake = Camera.main.GetComponent<CamShake>();
            camShake.StartCoroutine(camShake.Shake(0.1f, 0.05f));
            Stunned = true;

            yield return new WaitForSeconds(time);
            if (Stunned)
            {
                Stunned = false;
            }
        }
    }

    void Animation()
    {
        if (Stunned)
        {
            anim.speed = 0;
        }
        else
        {
            anim.speed = 1;
        }

        anim.SetBool("UseShield", shield.useShield);
    }

    void Stun()
    {
        Stunned = true;
    }

    private void Update()
    {
        GetInputs();
        Dash();
        Animation();        
    }

    private void FixedUpdate()
    {
        Move();
    }
}
