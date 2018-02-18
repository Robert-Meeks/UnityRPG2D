using UnityEngine;

public class GameManager
{
    private static GameManager _instance; // singleton (only ever want one of these)
    public static GameManager Instance { get { return _instance ?? (_instance = new GameManager()); } } /* READS: return _instance if null (otherwise return current instance) set _instance to new GameManager and then return this new instance */

    public int Points { get; private set; }

    private GameManager() /* include a private constuctor because now the only way to make an instance is to call the static property above which is what he wants */
    {

    }

    public void Reset()
    {
        Points = 0;
    }

    public void ResetPoints(int points)
    {
        Points = points;
    }

    public void AddPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }
}

