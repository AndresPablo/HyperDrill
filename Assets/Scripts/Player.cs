using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class Player : MonoBehaviour
{
    [SerializeField]Transform targetJump;
    [SerializeField]GameObject drill;
    [SerializeField]Collider2D myCollider;
    [SerializeField]Collider2D drillCollider;
    [SerializeField]TrailRenderer trail;
    [SerializeField] SpriteRenderer graficos;
    [SerializeField] GameObject muerteVFX;
    public float jumpSpeed;
    public float jumpDistance;
    public float rayDist = .2f;
    [Space]
    public int maxFuel = 10;
    public int fuel;
    public bool moving;
    public bool canJump;
    public float costPerUnit = 20f;
    public float jumpCost;
    [Header("Sonido")]
    [SerializeField] AudioClip SFX_Salto;
    [Header("DEBUG")]
    public bool infiniteFuel = false;
    public bool altCost;
    

    Rigidbody2D rb;
    Vector2 jumpPos;
    GameManager gm;

    #region  EVENTS
    public delegate void EmptyVoidDelegate();
    public static event EmptyVoidDelegate OnPlayerMoves;
    public static event EmptyVoidDelegate OnPlayerStop;
    public static event EmptyVoidDelegate OnPlayerDeath;
    public static event EmptyVoidDelegate OnPlayerRefuel;
    public static event EmptyVoidDelegate OnPlayerEmpty;
    #endregion
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
        gm = GameManager.instance;
        GameManager.OnGameStart += Spawn;
    }

    void Spawn(){
        graficos.enabled = true;
        drillCollider.enabled = false;
        myCollider.enabled = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        fuel = maxFuel; 
        canJump = true;
        moving = false;
        enabled = true;
        trail.Clear();
        trail.emitting = true;
        targetJump.gameObject.SetActive(true);
        muerteVFX.SetActive(false);
    }

    void Update()
    {
        if(!moving){
            // Not movig
            LookAtMouse();
            jumpDistance = Vector2.Distance(transform.position, jumpPos);

            if(canJump)
                CalculateJumpCost();

            if(canJump && Input.GetMouseButtonDown(0)){
                Jump();
                targetJump.gameObject.SetActive(false);
            }
        }else{
            /// Moving

            // Mining Ray
            // DrillRay();

            // Check Stop Distance 
            if(Vector2.Distance(transform.position, jumpPos) < .25f){
                Stop();
                targetJump.gameObject.SetActive(true);
            }
        }
    }

    // DEPRECATED
    public void DrillRay(){
        int layer_mask = LayerMask.GetMask("MineLayer");
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, transform.right, rayDist, layer_mask);
        Debug.DrawRay(transform.position, transform.right, Color.green);
        
        if(hit){
            Debug.Log(hit.collider.gameObject.name);

            if(hit.collider.gameObject.tag == "Asteroid"){
                Destroy(hit.collider.gameObject);
            }
        }
    }

    void CheckStuck()
    {
        OutOfFuel();
    }

    public void CalculateJumpCost(){
        float dist = Vector2.Distance(targetJump.position, transform.position);
        jumpCost = dist * costPerUnit;
    }

    public void Stop(){
        ApagarColliders();
        CancelInvoke("CheckStuck");
        drillCollider.enabled = false;
        moving = false;
        drill.SetActive(false);
        CheckFuel();
        if(fuel > 0){
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        GameManager.instance.ResetCombo();
        if(OnPlayerStop != null)
            OnPlayerStop();
    }

    public void Jump(){
        PrenderColliders();
        //rb.velocity = new Vector2(rb.velocity.x * 3f, rb.velocity.y * 3);
        drillCollider.enabled = true;
        moving = true;
        rb.AddForce(((Vector2)targetJump.position - (Vector2) transform.position).normalized * jumpSpeed);
        jumpPos = targetJump.position;
        drill.SetActive(true);
        if(!infiniteFuel)
        {
            if(altCost)
                fuel--;
            //else
            //fuel -= jumpCost;
        }

        AudioManager.instance.PlayOneShot(SFX_Salto);

        // Revisa si pasamos demasiado tiempo sin poder movernos
        Invoke("CheckStuck", 15f);

        if(OnPlayerMoves != null)
            OnPlayerMoves();        
    }

    void OutOfFuel(){
        gm.OutOfFuel();
        rb.angularVelocity = rb.angularVelocity /3;
        rb.AddTorque(Random.Range(-2,+2));
        trail.emitting = false;
        drillCollider.enabled = false;
    }

    public void CheckFuel(){
        if(fuel <= 0){
            OutOfFuel();
            canJump = false;
        }else{
            canJump = true;
        }
    }

    public void Refuel(int amount = 1){
        fuel += amount;
        if(OnPlayerRefuel != null)
            OnPlayerRefuel();
    }

    public void RefuelFull()
    {
        fuel = maxFuel;
        canJump = true;
        if(OnPlayerRefuel != null)
            OnPlayerRefuel();
    }

    void PrenderColliders()
    {
        myCollider.enabled = true;
        drillCollider.enabled = true;
    }

    void ApagarColliders()
    {
        myCollider.enabled = false;
        drillCollider.enabled = false;
    }

    void LookAtMouse(){
        Vector2 direction = ((Vector2)targetJump.position - (Vector2) transform.position).normalized;
        transform.right = direction;
    }

    public void Kill(){
        drillCollider.enabled = false;
        trail.emitting = false;
        graficos.enabled = false;
        muerteVFX.SetActive(false);
    }




}
