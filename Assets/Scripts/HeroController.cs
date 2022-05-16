
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class HeroController : MonoBehaviour
{

    public Slider barraHeroe;
    public Texture2D cursor;
    public Texture2D cursor2;
    public bool poderActivado = false;
    public Camera mainCamera;
    public float speedDash;
    public GameObject explosion;

    [Header("Movement")]
    public float moveSpeed;
    public float accel;
    public float deccel;
    public float speedExp;

    [Header("Jump")]
    public float raycastDistance;
    public float jumpForce;
    public float fallMultiplier;
    public int jumpCount = 0;

    [Header("Fire")]
    public GameObject fireball; //prefab
    private Transform mFireballPoint;

    private Rigidbody2D mRigidBody;
    private float mMovement;
    private Animator mAnimator;
    private SpriteRenderer mSpriteRenderer;
    

    private void Start()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mFireballPoint = transform.Find("FireballPoint");
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
        explosion.SetActive(false);
    }

    private void Update()
    {
        if(IsOnAir() && (mRigidBody.velocity.y< 0))
        {
            mAnimator.SetBool("IsFalling", true);
        }
        else
        {
            mAnimator.SetBool("IsFalling", false);
        }
        if(transform.position.y<-10f )
        {
            barraHeroe.gameObject.SetActive(false);
            Destroy(gameObject);
            explosion.transform.position = transform.position;
            explosion.SetActive(true);
            Debug.Log("destruido");
        }
        barraHeroe.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, barraHeroe.transform.position.y);   
        if (Input.GetMouseButton(1) && poderActivado == true)
        {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
            Debug.Log("Cancelar poder");
            poderActivado = false;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (barraHeroe.value == 1)
            {
                Cursor.SetCursor(cursor2, Vector2.zero, CursorMode.ForceSoftware);
                poderActivado = true;
              //  Debug.Log("Poder!");
            }
            else
            {
                Debug.Log("No poder");
            }
        }
            if (!IsOnAir())
        {
            jumpCount = 0;
        }
        mMovement = Input.GetAxis("Horizontal");
        mAnimator.SetInteger("Move", mMovement == 0f ? 0 : 1);
        
        if (mMovement < 0f)
        {
            //mSpriteRenderer.flipX = true;
            transform.rotation = Quaternion.Euler(
                0f,
                180f,
                0f
            );
        } else if (mMovement > 0)
        {
            //mSpriteRenderer.flipX = false;
            transform.rotation = Quaternion.Euler(
                0f,
                0f,
                0f
            );
        }

        bool isOnAir = IsOnAir();
        if (Input.GetButtonDown("Jump") && jumpCount< 1)
        {
            mRigidBody.velocity = new Vector2(mRigidBody.velocity.x, 0);
            Jump();
            Debug.Log("salto");
        }

        if (Input.GetButtonDown("Fire1"))
        {

            if (!poderActivado)
            {
                Fire();

            }
            else if (Input.GetMouseButton(0) && poderActivado == true)
            {
                //mAnimator.SetBool("IsDashing", true);
                StartCoroutine("DashScript");
                
            }
        }
    }


    private void FixedUpdate()
    {
        Move();

        if (mRigidBody.velocity.y < 0)
        {
            // Esta cayendo
            mRigidBody.velocity += (fallMultiplier - 1) * 
                Time.fixedDeltaTime * Physics2D.gravity;
        }
    }

    private void Move()
    {
        float targetSpeed = mMovement * moveSpeed;
        float speedDif = targetSpeed - mRigidBody.velocity.x;
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? accel : deccel;
        float movement = Mathf.Pow(
            accelRate * Mathf.Abs(speedDif),
            speedExp
        ) * Mathf.Sign(speedDif);

        mRigidBody.AddForce(movement * Vector2.right);
    }

    private void Jump()
    {
        jumpCount += 1;

        mRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        mAnimator.SetBool("IsJumping", true);


    }
    IEnumerator DashScript()
    {
        mAnimator.SetBool("IsDashing", true);
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);

        yield return new WaitForSeconds(0.2f);
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Debug.Log("Poder!");
        barraHeroe.value = 0;
        if (mousePos.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(
            0f,
            180f,
            0f);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x - 5f, transform.position.y, transform.position.z), speedDash);
            //mAnimator.SetBool("IsDashing", false);
        }
        else
        {
            transform.rotation = Quaternion.Euler(
            0f,
            0f,
            0f

        );
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z), speedDash);
            //mAnimator.SetBool("IsDashing", false);
        }
        //Teletransporte();
        poderActivado = false;

        yield return new WaitForSeconds(0.3f);

        mAnimator.SetBool("IsDashing", false);
    }

    public bool IsOnAir()
    {
        Transform rayCastOrigin = transform.Find("RaycastPoint");
        RaycastHit2D hit = Physics2D.Raycast(
            rayCastOrigin.position,
            Vector2.down,
            raycastDistance
        );
        mAnimator.SetBool("IsJumping", !hit);

        /*Color rayColor;
        if (hit)
        {
            rayColor = Color.red;
        }else
        {
            rayColor = Color.blue;
        }
        Debug.DrawRay(rayCastOrigin.position, Vector2.down * raycastDistance, rayColor);*/
        return !hit;
        //return hit == null ? true : false;
        
    }
    private void Fire()
    {
        if(!poderActivado)
        {
            mFireballPoint.GetComponent<ParticleSystem>().Play(); // ejecutamos PS
            GameObject obj = Instantiate(fireball, mFireballPoint);
            obj.transform.parent = null;
        }
  
        
    }

    public Vector3 GetDirection()
    {
        return new Vector3(
            transform.rotation.y == 0f ? 1f : -1f,
            0f,
            0f
        );
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Plataforma")
        {
            StartCoroutine("Muerte");
        }
    }
    IEnumerator Muerte()
    {

        yield return new WaitForSeconds(0.3f);

        barraHeroe.gameObject.SetActive(false);
        Destroy(gameObject);
        explosion.transform.position = transform.position;
        explosion.SetActive(true);
        Debug.Log("destruido");
        
    }
}
