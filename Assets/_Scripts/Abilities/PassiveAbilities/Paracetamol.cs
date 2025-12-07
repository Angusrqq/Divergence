public class Paracetamol : PassiveAbilityMono
{
    public override void Activate()
    {
        GameData.player.OnCrystalPickup += Heal;
    }

    private void Heal(int exp)
    {
        float healingByPercent = GameData.LowValueRoll(0.01f, 0.03f) * GameData.player.DamageableEntity.MaxHealth;
        GameData.player.DamageableEntity.Heal(this, exp + healingByPercent, GetType());
    }
}
