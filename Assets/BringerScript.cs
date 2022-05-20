using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BringerScript : MonoBehaviour
{
    public float maxHealth;
    public Slider healthbar;
    public Slider barraHeroe;
    private float mHealth;
    private float temp;
    public GameObject Heroe;
    public Animator mAnimator;
    private Rigidbody2D mRigidBody;
    private bool moverse = false;
    public float speedBoss = 2f;
    private Vector3 posInicial;
    private bool atacando = false;
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
    {
        temp += Time.deltaTime;
        Debug.Log(temp);
        var step = speedBoss * Time.deltaTime;

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
        if(Vector3.Distance(transform.position, Heroe.transform.position)<= 10f)
        {
            healthbar.gameObject.SetActive(true);
            Debug.Log("dentro del area");
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
        if (Vector3.Distance(transform.position, Heroe.transform.position) <= 3f && temp>=3f)
        {

            Debug.Log("ataca");
            mAnimator.SetBool("Atacando", true);
            moverse = true;
            atacando = true;
            temp = 0f;
        }
        else
        {
            mAnimator.SetBool("Atacando", false);
            atacando = false;


        }
        
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
