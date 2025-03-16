using UnityEngine;
using Mirror;
using System.Collections;

public class Soldier : NetworkBehaviour
{
    [SyncVar] public int ownerId;
    [SyncVar] public float health;

    public float maxHealth = 100f;
    public float speed = 3f;
    public float attackSpeed = 1f;
    public float attackDamage = 10f;
    public AudioClip deathSound;

    private bool isAttacking;
    private bool isMoving;

    public override void OnStartServer()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CmdMoveTo(targetPos);
        }
    }

    [Command]
    public void CmdMoveTo(Vector2 targetPosition)
    {
        RpcMove(targetPosition);
    }

    [ClientRpc]
    private void RpcMove(Vector2 targetPosition)
    {
        if (isMoving) return;
        StartCoroutine(MoveRoutine(targetPosition));
    }

    private IEnumerator MoveRoutine(Vector2 targetPosition)
    {
        isMoving = true;
        while ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
    }

    [Server]
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            Kill();
    }

    [Server]
    public void Kill()
    {
        RpcHandleDeath();
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    private void RpcHandleDeath()
    {
        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
    }

    [Command]
    public void CmdAttack(NetworkIdentity targetIdentity)
    {
        if (targetIdentity.TryGetComponent(out Soldier enemySoldier))
            StartCoroutine(AttackRoutine(enemySoldier));
        else if (targetIdentity.TryGetComponent(out Building enemyBuilding))
            StartCoroutine(AttackRoutine(enemyBuilding));
    }

    private IEnumerator AttackRoutine(Soldier target)
    {
        isAttacking = true;
        while (target != null && health > 0 && target.health > 0)
        {
            target.TakeDamage(attackDamage);
            yield return new WaitForSeconds(1f / attackSpeed);
        }
        isAttacking = false;
    }

    private IEnumerator AttackRoutine(Building target)
    {
        isAttacking = true;
        while (target != null && health > 0 && target.CurrentHealth > 0)
        {
            target.CmdTakeDamage(attackDamage);
            yield return new WaitForSeconds(1f / attackSpeed);
        }
        isAttacking = false;
    }

    public void UpdateEntity()
    {
        if (!isServer) return;

        if (health <= 0)
            Kill();
    }
}