
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    public string LevelName;

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("FINISH LEVEL CS USED");
        if (other.GetComponent<Player>() == null)
        {
            return;
        }

        LevelManager.Instance.GoToNextLevel(LevelName);
    }
}

