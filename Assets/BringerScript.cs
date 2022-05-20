using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BringerScript : MonoBehaviour
{
    public float maxHealth;
    public Slider healthbar;
    public Slider barraHeroe;
    public float mHealth;
    private float temp;
    public GameObject Heroe;
    public Animator mAnimator;
    private Rigidbody2D mRigidBody;
    private bool moverse = false;
    public float speedBoss = 2f;
    private Vector3 posInicial;
    private bool atacando = false;
    private bool valid = false;
    public GameObject colAtaque;
    // Start is called before the first frame update
    void Start()
    {

        mHealth = maxHealth;
        posInicial = transform.position;
        mRigidBody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {            var step = speedBoss * Time.deltaTime;

        if (Heroe != null)
        {
            temp += Time.deltaTime;
            //  Debug.Log(temp);
             step = speedBoss * Time.deltaTime;
            if (!mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                colAtaque.SetActive(false);
            }
            if (moverse && !mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                transform.position = Vector3.MoveTowards(transform.position, Heroe.transform.position, step);
                if (transform.position.x > Heroe.transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(
                        0f,
                        0f,
                        0f
                    );
                }
                else if (transform.position.x < Heroe.transform.position.x)

                {
                    transform.rotation = Quaternion.Euler(
                        0f,
                        180f,
                        0f
                    );
                }
            }
            if (Vector3.Distance(transform.position, Heroe.transform.position) <= 10f && Heroe != null)
            {
                healthbar.gameObject.SetActive(true);
                // Debug.Log("dentro del area");
                mAnimator.SetBool("Caminando", true);
                moverse = true;
            }
            else
            {
                healthbar.gameObject.SetActive(false);

                mAnimator.SetBool("Caminando", false);
                moverse = false;
                transform.position = Vector3.MoveTowards(transform.position, posInicial, step);

            }
            if (Vector3.Distance(transform.position, Heroe.transform.position) <= 3f && temp >= 3f && !valid)
            {
                valid = true;
                Debug.Log("ataca");
                mAnimator.SetBool("Atacando", true);
                Invoke("funcA", 0.5f);


            }
            else
            {
                mAnimator.SetBool("Atacando", false);
                atacando = false;
                

            }
        }
        else
        {

            moverse = false;
            transform.position = Vector3.MoveTowards(transform.position, posInicial, step);
            if (transform.position.x > posInicial.x)
            {
                transform.rotation = Quaternion.Euler(
                    0f,
                    0f,
                    0f
                );
            }
            else if (transform.position.x < posInicial.x)

            {
                transform.rotation = Quaternion.Euler(
                    0f,
                    180f,
                    0f
                );
            }
            if(transform.position!= posInicial)
            {
                mAnimator.SetBool("Caminando", true);

            }
            if(transform.position == posInicial)
            {
                mAnimator.SetBool("Caminando", false);

            }
        }

    }
    private void funcA()
    {
        
        moverse = true;
        atacando = true;
        temp = 0f;
        colAtaque.SetActive(true);
        valid = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            // Hubo una colision
            mHealth -= maxHealth * 0.02f;
            healthbar.value -= 0.02f;
            barraHeroe.value += 0.25f;

            if (mHealth <= 0)
            {
                Destroy(gameObject);
                healthbar.gameObject.SetActive(false);

            }
        }
    }
}
