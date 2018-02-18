using UnityEngine;
using System.Collections;

public class JumpPlatform : MonoBehaviour {

    public float JumpMagnitude = 5; // the jump foce that is applied to the character
    public AudioClip PlayerHitJumpPad;

    public void ControllerEnter2D(CharacterController2D controller)
    {
        controller.SetVerticalForce(JumpMagnitude);

        if(PlayerHitJumpPad != null)
            AudioSource.PlayClipAtPoint(PlayerHitJumpPad, transform.position);
    }
}
