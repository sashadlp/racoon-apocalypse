using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class racoon : MonoBehaviour
{
    [SerializeField] private InputActionAsset Actions;
    private InputAction moveAction, jumpAction, attackAction;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jump = 8f;
    private Vector2 direction;
    private bool grounded;

    
    [SerializeField] private GameObject projectil;                
    [SerializeField] private float speedProjectil = 10f;           
    [SerializeField] private float reloadTime = 0.5f;             
    private GameObject projectilSave;           
    
    private float reloadCountdown = 0f;                     

    [SerializeField] private int vie = 5;
    private int vieMax;
    private Slider barreDevie;
    private Vector3 posBase;

    private Rigidbody2D rb;
    private SpriteRenderer monSprite;
    private Animator anim;
    private CapsuleCollider2D monColl;
    private Collider2D[] colls;

    private void OnEnable() 
    {
        Actions.Enable();
        
        moveAction = Actions.FindAction("Move");
        moveAction.started += moveCheck;
        moveAction.canceled += moveCheck;

        jumpAction = Actions.FindAction("Jump");
        jumpAction.started += jumpCheck;
        
        attackAction = Actions.FindAction("Attack");
        if (attackAction != null)
        {
            attackAction.started += TryAttack;
        }
    }

    private void OnDisable()
    {
        Actions.Disable();
    }

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        monColl = GetComponent<CapsuleCollider2D>();
        monSprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        vieMax = vie;
        posBase = transform.position;

        GameObject lifeBarObj = GameObject.FindWithTag("LifeBar");
        if (lifeBarObj != null)
        {
            barreDevie = lifeBarObj.GetComponent<Slider>();
            updateBar();
        }
        else
        {
            Debug.LogError("Attention : Aucun objet avec le Tag 'LifeBar' n'a été trouvé dans la scène !");
        }
    }

    void Update() 
    {
        groundCheck();
        animCheck();

        if (reloadCountdown > 0f) 
        {
            reloadCountdown -= Time.deltaTime;
        }

        rb.linearVelocityX = speed * direction.x;
    }

    void moveCheck(InputAction.CallbackContext phase) 
    {
        direction = phase.ReadValue<Vector2>();

        if (direction.x > 0) {
            monSprite.flipX = false;
        }
        if (direction.x < 0) {
            monSprite.flipX = true;
        }

        if (phase.canceled) {
            direction = Vector2.zero;
        }
    }

    void jumpCheck(InputAction.CallbackContext phase) 
    {
        if (grounded) {
            rb.linearVelocityY = jump;
        }        
    }

    void groundCheck() 
    {
        grounded = false;
        colls = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, monColl.size.y / 2 - monColl.size.x * 0.35f, 0) + (Vector3)monColl.offset, monColl.size.x * 0.45f);
        foreach (Collider2D coll in colls) { 
            if (coll != monColl && !coll.isTrigger) {
                grounded = true; 
                break;
            }
        }
    }

    void TryAttack(InputAction.CallbackContext phase)
    {
        if (reloadCountdown <= 0f && projectil != null)
        {
            ExecuteAttack();
        }
    }

    void ExecuteAttack()
    {
        if (anim != null)
        {
            anim.SetTrigger("attackDIST"); 
        }

        projectilSave = Instantiate(projectil, transform.position, Quaternion.identity);        
        Rigidbody2D projRb = projectilSave.GetComponent<Rigidbody2D>();
        SpriteRenderer projSprite = projectilSave.GetComponent<SpriteRenderer>();

        if (!monSprite.flipX) 
        {                                                                          
            projRb.linearVelocity = new Vector2(speedProjectil, 0);    
        }
        else 
        {                                                                           
            projRb.linearVelocity = new Vector2(-speedProjectil, 0);  
            if (projSprite != null)
            {
                projSprite.flipX = true; 
            }                             
        }

        reloadCountdown = reloadTime;               
    }

    public void takeDamage(int damage)
    {
        vie -= damage;
        if (vie <= 0) { 
            transform.position = posBase;
            vie = vieMax;
        }
        updateBar();
    }

    void updateBar() 
    {
        if (barreDevie != null)
        {
            barreDevie.value = (float)vie / (float)vieMax;
        }
    }

    void OnTriggerEnter2D(Collider2D truc) 
    {
       
        if (truc.CompareTag("Respawn")) {
            posBase = transform.position;
        }
        if (truc.CompareTag("Kill")) {
            takeDamage(vieMax);
        }
    }

    void animCheck() 
    {
        if (anim != null)
        {
            anim.SetFloat("velocityX", Mathf.Abs(rb.linearVelocityX));
            anim.SetFloat("velocityY", rb.linearVelocityY);
            anim.SetBool("grounded", grounded);
        }
    }

    private void OnDrawGizmosSelected() 
    {
        if (monColl == null) {
            monColl = GetComponent<CapsuleCollider2D>();
        }
        if (monColl != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position - new Vector3(0, monColl.size.y / 2 - monColl.size.x * 0.35f, 0) + (Vector3)monColl.offset, monColl.size.x * 0.45f);
        }
    }
}