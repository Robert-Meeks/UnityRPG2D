using UnityEngine;
using System.Collections;
using System;

public class PointStar : MonoBehaviour, IPlayerRespawnListener {

    public GameObject Effect;
    public int PointsToAdd = 10;
    public AudioClip PlayerHitStar;

    public Animator Animator;
    public SpriteRenderer Renderer;

    private bool _isCollected;

    public void OnTriggerEnter2D(Collider2D collidedWith) // he used other for the name of the arg
    {
        if (_isCollected)
            return;

        Debug.Log("in PointStar.cs OnTriggerEnter2d()");
        if (collidedWith.GetComponent<Player>() == null) // code he used is depricated 
            return; // means collider was not a player and therefore want to exit method

        if(PlayerHitStar != null)
        {
            AudioSource.PlayClipAtPoint(PlayerHitStar, transform.position);
        }

        GameManager.Instance.AddPoints(PointsToAdd);

        Instantiate(Effect, transform.position, transform.rotation); // doesnt require "Object." --> I took it out because VS found it ambiguous, the arguments are; the object to create, position, rotation

      
        string str = PointsToAdd.ToString();
        Debug.Log(String.Format("in PointStar.cs calling floatingText.Show(PointsToAdd = {0}, \"...\" , )", str));
        FloatingText.Show(String.Format("+{0}!", PointsToAdd), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));

        _isCollected = true;
        Animator.SetTrigger("Collect");
    }

    // this method is invoked by the animation when it finishes (tutorial 23 41:30
    public void FinishAnimationEvent()
    {
        Debug.Log("FinishedAnimation()");
        //gameObject.SetActive(false); // destroys the game object - dont want to destroy because if player dies and starts from check point you might want to bring object back 
        Renderer.enabled = false;
        Animator.SetTrigger("Reset");
    }

    void IPlayerRespawnListener.OnPlayerRespawnInThischeckpoint(Checkpoint checkpoint, Player player)
    {
        _isCollected = false;
        //gameObject.SetActive(true);
        Renderer.enabled = true;
    }
}
