using UnityEngine;

public class GameHud : MonoBehaviour
{
    public GUISkin Skin;

    public void OnGUI()
    {
        GUI.skin = Skin;

        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        {
            
            GUILayout.BeginVertical(Skin.GetStyle("GameHud"));
            {/* the purpose of the {}'s is to mkae it clear where the block that starts and finishes with BeginArea() and EndArea() is, basically the {}'s links the 2 methods visually to make it easier to read */
                GUILayout.Label(string.Format("Points: {0}", GameManager.Instance.Points), Skin.GetStyle("PointText"));

                var time = LevelManager.Instance.RunningTime;
                GUILayout.Label(string.Format(
                    "{0:00}:{1:00} with {2} bonus",
                    time.Minutes + time.Hours * 60,
                    time.Seconds,
                    LevelManager.Instance.CurrentTimeBonus), Skin.GetStyle("TimeText"));
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea(); // required when you have a beginArea()
    }
}

