using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private List<IPlayerRespawnListener> _listeners;

    public void Awake()
    {
        _listeners = new List<IPlayerRespawnListener>();
    }

    public void PlayerHitCheckPoint() // invoked by level manager 
    {
        StartCoroutine(PlayerHitCheckpointCo(LevelManager.Instance.CurrentTimeBonus)); /* "StartCoroutine" is a unity or vs thing (it recognised it as I typed it in) 
                                                                                          A coroutine is exicuted over multiple frames. generally speaking it puts 
                                                                                          whatever it is calling on a different thread. In this case - 
                                                                                          levelManager.Update is calling this func, this func sdoesnt hang up the 
                                                                                          update method, also when the update method gets to the end it would kill 
                                                                                          this process, obviously this would happen too quickly which is why we have 
                                                                                          a need to use a coroutine */

    }

    private IEnumerator PlayerHitCheckpointCo(int bonus) // private so has to be called from within
    {
        FloatingText.Show("Checkpoint!!!", "CheckpointText", new CenteredTextPositioner(.5f)); /*so '"CheckpointText"' is a GUISkin/Custom styles (in unity). Add a 
                                                                                                 new one by changing the size and then changing the name to 
                                                                                                 "CheckpointText" as it is in the method. then you can change the 
                                                                                                 attributes of the custom style in unity.NB when you increment the number
                                                                                                 and press enter or click out of the box it will make a copy of the 
                                                                                                 previous style which you then need to change */
        yield return new WaitForSeconds(.5f);
        FloatingText.Show(string.Format("+{0} Time Bonus!", bonus), "CheckpointText", new CenteredTextPositioner(.5f));
    }

    public void PlayerLeftCheckpoint()
    {

    }

    public void SpawnPlayer(Player player)
    {
        player.RespawnAt(transform);
        foreach (var listener in _listeners)
        {
            listener.OnPlayerRespawnInThischeckpoint(this, player);
        }
    }

    public void AssignObjectToCheckpoint(IPlayerRespawnListener listener)
    {
        //Debug.Log("hello from AssignObjectToCheckpoint() in Checkpoint");
        _listeners.Add(listener);
    }
}

