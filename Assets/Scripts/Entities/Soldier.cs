using UnityEngine;

public class Soldier : MonoBehaviour
{
    public int ownerId; // player id

    public int entityId;
    public float health;
    public float maxHealth;

    public float speed;
    public float attackSpeed;

    public float attackDamage;
    public float attackRange;

    public Vector2 currentPosition = Vector2.zero;

    /// <summary>
    /// Called when the soldier dies. It will broadcast a sound, increment player stats and also
    /// call despawn to remove the entity from the game.
    /// </summary>
    public void Kill()
    {

    }

    public void Despawn()
    {

    }

    public void Spawn()
    {

    }

    public void Attack(Soldier target)
    {

    }

    public void AttacK(Building target)
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
