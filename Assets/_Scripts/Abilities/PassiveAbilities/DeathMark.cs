using System;
using System.Collections.Generic;

public class DeathMark : PassiveAbilityMono
{
    public DeathMarkInstance Prefab;

    private byte _currentMarks = 0;
    private readonly List<Enemy> markedEnemies = new();
    private bool _isCapped = true;

    private void OnProjectileHitEnemy(Type type, Enemy enemy, float damage, InstantiatedAbilityMono projectile)
    {
        if (markedEnemies.Contains(enemy)) return;
        if (_isCapped && _currentMarks >= Ability.GetStat("Maximum marks")) return;

        if (GameData.LowValue < Ability.GetStat("Chance to mark"))
        {
            var instance = Instantiate(Prefab, EnemyManager.Instance.transform);
            markedEnemies.Add(enemy);
            instance.Init(enemy, Ability.GetStat("Damage"), Ability.GetStat("Explosion Radius"));
            _currentMarks += 1;

            instance.OnDeath += () => 
            {
                _currentMarks -= 1;
                AudioManager.instance.PlaySFX(AudioClips[0]);
                markedEnemies.Remove(enemy);
            };
        }
    }

    public override void Activate()
    {
        GameData.player.AbilityHolder.OnEnemyHit += OnProjectileHitEnemy;
    }
}
