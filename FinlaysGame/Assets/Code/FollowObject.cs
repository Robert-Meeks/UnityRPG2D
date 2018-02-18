using UnityEngine;

/*
 NOTE - there is another way to do this:

    If the health bar is made child to player then it will follow the player around. Simples BUT because the player is set to flip when going from right to left 
    the bar will as well. To stop this from happening make a game object and child it to the player, then add the script that does this flip to the game object 
    and remove it from player. Now add all the child components that you want to flip to the game object and just add the health ber to the player. This will 
    mean that the health ber will inherit the code to follow the player but not the flip bit. 

    Also this is a good example I think of why creating script for different actions is an important feature of architecture design.
     */
     
public class FollowObject : MonoBehaviour
{
    public Vector2 Offset; // not this is changed in the unity ediditor and it is the offset from the player centre. so to move it higher for example above the player make the value bigger
    public Transform Following;

    public void Update()
    {
        transform.position = Following.transform.position + (Vector3)Offset;
    }

}

