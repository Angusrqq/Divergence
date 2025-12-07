using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SlowdownForceField : InstantiatedAbilityMono
{
    private StatModifier _slowdownModifier = new(-0.5f, StatModifierType.Percent, GameData.player);
    
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        doesDamage = false;
    }

    protected override void FixedUpdateLogic()
    {
        rb.position = GameData.player.transform.position;
        transform.RotateAround(transform.position, Vector3.forward, 2);
    }

    public override void EnemyCollision(Enemy enemy)
    {
        enemy.moveSpeed.AddModifier(_slowdownModifier);
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.moveSpeed.RemoveModifier(_slowdownModifier);
        }
    }
}
