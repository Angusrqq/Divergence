using System.Collections;
using UnityEngine;

public class Katana : InstantiatedAbilityMono
{
    [SerializeField] private ParticleSystem m_evoParticles;
    private ParticleSystem m_PSInstance;
    private SpriteRenderer spriteRenderer;
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = new Vector2(GameData.player.spriteRenderer.flipX ? -1 : 1, 0);
        spriteRenderer.flipX = GameData.player.spriteRenderer.flipX;
        transform.position = new Vector2(GameData.player.transform.position.x, GameData.player.transform.position.y) + (direction * 2);
    }

    protected override void FixedUpdate()
    {
        return;
    }

    void Start()
    {
        if (ability.isEvolved)
        {
            StartCoroutine(EvoSlash());
        }
        else
        {
            StartCoroutine(Slash());
        }
    }

    private IEnumerator Slash()
    {
        float elapsedTime = 0f;
        Color transparent = new Color(1, 1, 1, 0);
        while (spriteRenderer.color.a > 0)
        {
            elapsedTime += Time.deltaTime;
            
            spriteRenderer.color = Color.Lerp(Color.white, transparent, elapsedTime / ability.activeTime);
            yield return null;
        }
        Destroy(gameObject);
    }

    private IEnumerator EvoSlash()
    {
        float elapsedTime = 0f;
        Quaternion dir = Quaternion.FromToRotation(Vector2.right, direction);
        m_PSInstance = Instantiate(m_evoParticles, transform.position, dir);
        while (spriteRenderer.color.a > 0)
        {
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), elapsedTime / ability.activeTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
