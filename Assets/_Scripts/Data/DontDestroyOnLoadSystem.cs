using UnityEngine;

public class DontDestroyOnLoadSystem : MonoBehaviour
{
    public static DontDestroyOnLoadSystem instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
