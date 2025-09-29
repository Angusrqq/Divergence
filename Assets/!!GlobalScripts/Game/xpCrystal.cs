using UnityEngine;

public class xpCrystal : MonoBehaviour
{
    // public xpCrystal(int expContaining)
    // {
    //     if (expContaining > 0)
    //     {
    //         this.expContaining = expContaining;
    //     }
    //     else
    //     {
    //         Debug.LogError("xpCrystal expContaining must be greater than 0");
    //         Destroy(gameObject);
    //     }
    // }

    private readonly int expContaining;

    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.AddExp(gameObject, expContaining);
            Destroy(gameObject);
        }
    }
}
