using UnityEngine;

public class Target : MonoBehaviour, IHittable
{
    // Start is called before the first frame update

    public float health = 50f;


    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    //Interface Implementations
    public void GetHitted(int damage)
    {
        TakeDamage(damage);
    }
}