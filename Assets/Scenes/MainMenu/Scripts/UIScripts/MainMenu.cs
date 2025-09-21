using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject divergenceMeter;
    private divergenceMeter_anim dmAnimScript;
    private DivergenceMeter_Idle dmIdleScript;
    private Canvas mainMenuCanvas;
    void Awake()
    {
        dmAnimScript = divergenceMeter.GetComponent<divergenceMeter_anim>();
        dmIdleScript = divergenceMeter.GetComponent<DivergenceMeter_Idle>();
        mainMenuCanvas = GetComponent<Canvas>();
    }

    void FixedUpdate()
    {
        if (dmAnimScript.isEnded)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }

    public void PlayGame()
    {
        mainMenuCanvas.enabled = false;
        dmIdleScript.OnDisable();
        dmIdleScript.enabled = false;
        dmAnimScript.enabled = true;
    }
    public static void Quit()
    {
        // Save parameters here
        Application.Quit();
    }
}
