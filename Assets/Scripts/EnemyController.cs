
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float maxHealth;
    public Slider healthbar;
    public Slider barraHeroe;
    private float mHealth;

    private void Start()
    {
        mHealth = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            // Hubo una colision
            mHealth -= maxHealth * 0.25f;
            healthbar.value -= 0.25f;
            barraHeroe.value +=0.25f;

            if (mHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
