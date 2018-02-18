
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    
    public string FirstLevel;

    public void Update()
    {
        // when user clicks left button level loads
        if (!Input.GetMouseButtonDown(0))
            return;

        GameManager.Instance.Reset();

        // Application.LoadLevel(FirstLevel); // obsolete
        UnityEngine.SceneManagement.SceneManager.LoadScene(FirstLevel);
    }
}

