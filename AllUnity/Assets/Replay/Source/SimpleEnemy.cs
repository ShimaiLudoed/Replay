using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public int health = 30;
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Enemy took {damage} damage, remaining health: {health}");
        
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
