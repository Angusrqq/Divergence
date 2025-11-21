using System;
using System.Collections.Generic;

public class DeathMark : PassiveAbilityMono
{
    public DeathMarkInstance Prefab;
    [NonSerialized] public Stat Damage = 20f;
    [NonSerialized] public Stat ExplosionRadius = 15f;
    private float _markChance = 0.1f;
    private byte _availableMarks = 1;
    private byte _currentMarks = 0;
    private List<Enemy> markedEnemies = new();
    private bool _isCapped = true;

    private void OnProjectileHitEnemy(Type type, Enemy enemy, float damage, InstantiatedAbilityMono projectile)
    {
        if(markedEnemies.Contains(enemy)) return;
        if(_isCapped && _currentMarks >= _availableMarks) return;
        if (GameData.LowValue < _markChance)
        {
            var instance = Instantiate(Prefab, EnemyManager.Instance.transform);
            markedEnemies.Add(enemy);
            instance.Init(enemy, Damage, ExplosionRadius);
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
    public override void Upgrade()
    {
        _markChance += 0.1f;
        _availableMarks += 2;
        ExplosionRadius += 5f;
        Damage += 5f;
        PassiveAbilityHandler handler = GetComponentInParent<PassiveAbilityHandler>();
        if(handler.Level == handler.MaxLevel) _isCapped = false;
    }
}
