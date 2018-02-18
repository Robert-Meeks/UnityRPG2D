using UnityEngine;


public class GiveDamageToPlayer : MonoBehaviour
{
    public int DamageToGive = 10;

    private Vector2
        _lastPosition,
        _velocity;

    public void LateUpdate() // so this is recogniced as a system method like Update. THe difference is that its called after the Update()
    {
        _velocity = (_lastPosition - (Vector2)transform.position) / Time.deltaTime;
        _lastPosition = transform.position;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player == null)
            return;


        player.TakeDamage(DamageToGive, gameObject);
        var controller = player.GetComponent<CharacterController2D>();
        var totalVelocity = controller.Velocity + _velocity;

        controller.SetForce(new Vector2(
            -1 * Mathf.Sign(totalVelocity.x) * Mathf.Clamp(Mathf.Abs(totalVelocity.x) * 6, 10, 40), // the knock back is constrained to min==10 and max == 40  NB Abs is absolute val
            -1 * Mathf.Sign(totalVelocity.y) * Mathf.Clamp(Mathf.Abs(totalVelocity.y) * 2, 5 , 30))); // the knock up/down is constrained to min==5 and max == 30 
    }

}

