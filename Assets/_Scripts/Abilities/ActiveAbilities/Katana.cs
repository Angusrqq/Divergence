using System.Collections;
using UnityEngine;

/// <summary>
/// The Katana class represents a projectile ability that creates a slash effect. 
/// It inherits from InstantiatedAbilityMono and handles the visual representation and animation of the slash.
/// The class supports both regular and evolved versions of the slash, with the evolved version including additional particle effects.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Katana : InstantiatedAbilityMono
{
    [SerializeField] private ParticleSystem _evoParticles;

    private SpriteRenderer spriteRenderer;
    
    /// <summary>
    /// Initializes the projectile's components and sets its initial position and direction.
    /// This method is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = new Vector2(GameData.player.SpriteRenderer.flipX ? -1 : 1, 0);
        spriteRenderer.flipX = GameData.player.SpriteRenderer.flipX;
        transform.position = new Vector2(GameData.player.transform.position.x, GameData.player.transform.position.y) + (direction * 2);
    }

    protected override void FixedUpdate() { }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    /// <remarks>
    /// If the ability is evolved, calls the EvoSlash coroutine, otherwise calls the Slash coroutine.
    /// </remarks>
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

    /// <summary>
    /// Coroutine for the slash animation.
    /// </summary>
    /// <remarks>
    /// Gradually changes the alpha value of the sprite renderer's color from white to transparent over the ability's active time.
    /// Destroys the game object when the animation is finished.
    /// </remarks>
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

    /// <summary>
    /// Coroutine for the evolved slash animation.
    /// </summary>
    /// <remarks>
    /// Instantiates the evolved particle system at the projectile's position and direction.
    /// Gradually changes the alpha value of the sprite renderer's color from white to transparent over the ability's active time.
    /// Destroys the game object when the animation is finished.
    /// </remarks>
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
