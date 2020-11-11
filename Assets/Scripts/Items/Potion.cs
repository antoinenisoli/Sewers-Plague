using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion : Projectile
{
    [HideInInspector]
    public Medic medic;

    [Header("--POTION--")]
    [SerializeField] protected AudioSource breakSound;
    protected CamShake camShake;

    [Header("Curve control")]
    public Rigidbody2D rb;
    public float fallSpeed = 2.65f;
    protected bool isThrowed;

    [Header("Destination")]
    public bool canBreak;
    protected float currentDistance;
    [SerializeField] protected float breakDistance = 1;
    public Vector3 nextPos;
    protected Vector3 startPos;

    [Header("VFX")]
    [SerializeField] protected GameObject targetMark;
    protected GameObject markInstance;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject trail;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        camShake = Camera.main.GetComponent<CamShake>();
    }    

    public virtual void Start()
    {
        markInstance = Instantiate(targetMark, nextPos, Quaternion.identity);
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(10f);
        Collapse();
    }

    public override void Collapse()
    {
        trail.transform.parent = null;
        breakSound.transform.parent = trail.transform;
        breakSound.Play();
        Trail trailScript = trail.GetComponent<Trail>();
        trailScript.StartCoroutine(trailScript.AutoDestroy());
        
        Destroy(markInstance);
        Destroy(gameObject);        
    }

    public void OnDestroy()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    public virtual void CheckDistance()
    {
        currentDistance = Vector2.Distance(transform.position, nextPos);
        canBreak = currentDistance < breakDistance;

        if ((canBreak && transform.position.y < nextPos.y && !isThrowed) || currentDistance <= 0.1f)
        {
            Collapse();
        }
    }

    private void Update()
    {
        CheckDistance();
    }
}
