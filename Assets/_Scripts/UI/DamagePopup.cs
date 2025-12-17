using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;
    private TMP_Text _text;
    private TextMeshPro _textMesh;
    private const float _LIFETIME = 1f;

    private static int sortingOrder;

    public static DamagePopup Create(Vector3 position, float damage, Vector2 direction, bool isCritical = false, Transform parent = null, Color textColor = default)
    {
        DamagePopup damagePopup = Instantiate(isCritical ? AssetManager.Instance.criticalDamagePopupPrefab : AssetManager.Instance.damagePopupPrefab, position, Quaternion.identity, parent);
        damagePopup.Setup(damage, direction, textColor);
        return damagePopup;
    }

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _textMesh = GetComponent<TextMeshPro>();
    }

    public void Setup(float damage, Vector2 direction, Color textColor = default)
    {
        _text.color = textColor == default ? _text.color : textColor;
        _text.SetText(Utilities.FormatFloat(damage));
        sortingOrder++;
        _textMesh.sortingOrder = sortingOrder;
        StartCoroutine(Animation(direction));
    }

    private IEnumerator Animation(Vector2 direction)
    {
        float elapsedTime = 0f;
        float moveSpeed = 4f;
        float timeSpeed = 1.25f;
        float rotationDir = Random.Range(-1f, 1f);
        while(elapsedTime < _LIFETIME)
        {
            if(elapsedTime > _LIFETIME / 2f)
            {
                _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, Mathf.Lerp(1f, 0f, elapsedTime / 2f / _LIFETIME));
                transform.RotateAround(transform.position, Vector3.forward, rotationDir * moveSpeed * 200 * Time.deltaTime);
            }

            elapsedTime += Time.deltaTime * timeSpeed;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, _curve.Evaluate(elapsedTime / _LIFETIME));
            transform.position += (Vector3)(moveSpeed * Time.deltaTime * direction);
            _text.ForceMeshUpdate();
            yield return null;
        }
        Destroy(gameObject);
        sortingOrder--;
    }
}
