using System.Collections;
using UnityEngine;

/// <summary>
/// The Katana class represents a projectile ability that creates a slash effect. 
/// The class supports both regular and evolved versions of the slash, with the evolved version including additional particle effects.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Katana : InstantiatedAbilityMono
{
    [SerializeField] private ParticleSystem _evoParticles;

    private SpriteRenderer spriteRenderer;
    
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = new Vector2(GameData.player.SpriteRenderer.flipX ? -1 : 1, 0);
        spriteRenderer.flipX = GameData.player.SpriteRenderer.flipX;
        transform.position = new Vector2(GameData.player.transform.position.x, GameData.player.transform.position.y) + (direction * 2);
    }

    protected override void FixedUpdate() { }

    protected override void Start()
    {
        if (Ability.IsEvolved)
        {
            StartCoroutine(EvoSlashCoroutine());
        }
        else
        {
            StartCoroutine(Slash());
        }

        base.Start();
    }

    private IEnumerator Slash()
    {
        float elapsedTime = 0f;

        while (spriteRenderer.color.a > 0)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(Color.white, Color.clear, elapsedTime / Ability.ActiveTime);

            yield return null;
        }
        
        Destroy(gameObject);
    }

    private IEnumerator EvoSlashCoroutine()
    {
        float animationElapsedTime = 0f;
        Quaternion particleDirection = Quaternion.FromToRotation(Vector2.right, direction);

        Instantiate(_evoParticles, transform.position, particleDirection);

        while (spriteRenderer.color.a > 0f)
        {
            animationElapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(Color.white, Color.clear, animationElapsedTime / Ability.ActiveTime);

            yield return null;
        }

        Destroy(gameObject);
    }
}
