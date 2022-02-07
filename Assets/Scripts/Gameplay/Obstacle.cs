using UnityEngine;

public class Obstacle : MonoBehaviour, IHittable
{
    [SerializeField] int hp;

    //Methods
    void GetDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    //Interface Implementations
    public void GetHitted(int damage)
    {
        GetDamage(damage);
    }
}