using UnityEngine;


public class SimpleProjectile : Projectile, ITakeDamage // inherits from projectile so therefore inherits monobehavior and therefore can be attached to a game object
{
    public int Damage;
    public GameObject DestroyedEffect;
    public int PointsToGiveToPlayer;
    public float TimeToLive;
    public AudioClip DestroyProjectileSound;

    public void Update() // move the projectile along the path the owner set and kills it when its time to live runs out 
    {
        if((TimeToLive -= Time.deltaTime) <= 0)
        {
            Debug.Log("in Update() about to call func to DestroyProjectile()");
            DestroyProjectile(); // there is a unity Destory() but the reason he made is own is because he wants the method to also invoke other things such as a projectile destroyed animation. the animations or whatever could be put after the unity destroy method here but it will be repeated a lot so making a method will remove repeated code
            return;
        }

        /* this line added the velocity of the player to the microphone which obviously wasnt wanted
         * transform.Translate((Direction + new Vector2(InitialVelocity.x, 0)) * Speed * Time.deltaTime, Space.World);
         */
        transform.Translate(Direction * ((Mathf.Abs(InitialVelocity.x) + Speed) * Time.deltaTime), Space.World);
    }


    public void TakeDamage(int damage, GameObject instigator)
    {
        if(PointsToGiveToPlayer != 0)
        {
            var projectile = instigator.GetComponent<Projectile>();
            if (projectile != null && projectile.Owner.GetComponent<Player>() != null)
            {
                GameManager.Instance.AddPoints(PointsToGiveToPlayer);
                FloatingText.Show(string.Format("+{0}!", PointsToGiveToPlayer), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50)); // NB "PointStarText" we made for hitting a pointstar he couldnt be bother to make another animation but obviously its possible
            }
        }

        DestroyProjectile();
    }

    protected override void OnCollideOther(Collider2D other)
    {
        // base.OnCollideOther(other); if you use the suggestion VS give this will be auto generated so that by default the parent version is used. i left it here as a reminder of how to do this
        DestroyProjectile();
    }

    protected override void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage)
    {
        takeDamage.TakeDamage(Damage, gameObject);
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        if(DestroyedEffect != null)
        {
            Instantiate(DestroyedEffect, transform.position, transform.rotation);
        }
        if (DestroyProjectileSound != null)
            AudioSource.PlayClipAtPoint(DestroyProjectileSound, transform.position);

        Destroy(gameObject);
    }

}

