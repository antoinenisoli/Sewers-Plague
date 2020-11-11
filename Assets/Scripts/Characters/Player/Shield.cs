using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Shield : MonoBehaviour
{
    Collider2D myCollider;
    PlayerController player;
    Animator playerAnim;

    [Header("Shield Properties")]
    [SerializeField] float slowSpeed;
    [SerializeField] int manaGain = 1;
    public bool isInvincible;

    [Header("Parade perfect")]
    [SerializeField] float range;
    [SerializeField] float PerfectRange = 0.3f;
    bool perfectParade;
    PotionThrowable thisPotion;
    [SerializeField] LayerMask bulletLayer;
    public bool detectBullet;
    GameObject detectedBullet;
    public bool useShield;
    [SerializeField] Transform impactPos;

    [Header("VFX")]
    [SerializeField] GameObject impactVFX;
    [SerializeField] GameObject auraFx;
    [SerializeField] ParticleSystem perfectVFX;
    
    [Header("Audio")]
    [SerializeField] AudioSource paradePerfectSound;
    [SerializeField] AudioSource paradeSound;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
        Gizmos.DrawRay(transform.position, transform.right * range);
    }

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        player = GetComponentInParent<PlayerController>();
        playerAnim = player.GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet") && myCollider.enabled)
        {
            paradeSound.Play();

            if (!isInvincible)
            {
                if (detectBullet && perfectParade)
                {
                    player.GainMana(manaGain);
                    perfectVFX.Play();
                    paradePerfectSound.Play();
                }
                else
                {
                    Instantiate(impactVFX, impactPos.position, Quaternion.identity);
                }

                Destroy(other.gameObject);
            }
            else
            {
                Rigidbody2D bullet = other.GetComponent<Rigidbody2D>();
                bullet.gameObject.tag = "AllyBullet";
                bullet.velocity = -bullet.velocity - Vector2.right * -5;
                bullet.transform.localEulerAngles = new Vector3(0, -180, 0);
            }
        }

        PotionThrowable potion = other.GetComponentInParent<PotionThrowable>();
        if (potion)
        {
            thisPotion = potion;
            if (thisPotion.canBreak && perfectParade)
            {
                thisPotion.Throw();
                paradeSound.Play();
            }
        }
    }

    public IEnumerator Invincibility(float duration)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            yield return new WaitForSeconds(duration);
            if (isInvincible)
            {
                isInvincible = false;
            }
        }
    }

    void BulletDetection()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, transform.lossyScale, 0, transform.right, range, bulletLayer);
        detectBullet = hit;

        if (detectBullet)
        {
            print("PERFECT !");
        }

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Bullet"))
            {
                detectedBullet = hit.transform.gameObject;
            }
        }        
    }

    void SlowMode()
    {
        if (useShield && !isInvincible)
        {
            player.speed = slowSpeed;
        }
        else
        {
            player.speed = player.startSpeed;
        }
    }

    public void StartPerfect()
    {
        StartCoroutine(Perfect());
    }

    public IEnumerator Perfect()
    {
        if (!perfectParade)
        {
            perfectParade = true;
            yield return new WaitForSeconds(PerfectRange);
            if (perfectParade)
            {
                perfectParade = false;
            }
        }
    }

    void Parade()
    {
        if (isInvincible && !perfectParade)
        {
            useShield = true;
        }
        else 
        {
            if (player.Stunned)
            {
                useShield = false;
                perfectParade = false;
            }
            else
            {
                useShield = Input.GetButton("UseShield");
            }
        }

        auraFx.SetActive(isInvincible);
        playerAnim.SetBool("ParadePerfect", perfectParade);
        if (Input.GetButtonDown("UseShield"))
        {
            StartPerfect();
        }

        myCollider.enabled = useShield;
    }

    private void Update()
    {
        Parade();
        SlowMode();
    }

    private void FixedUpdate()
    {
        BulletDetection();
    }
}
