using UnityEngine;

public class PathedProjectileSpawner : MonoBehaviour
{
    public Transform Destination;
    public PathedProjectile Projectile; // is a game object also a class and prefab type (gameobject can be cast to this)
    public float Speed;
    public float FireRate;

    public GameObject SpawnEffect;
    public AudioClip CannonFireSound;

    public Animator Animator;

    private float _nextShotInSeconds;

    public void Start()
    {
        _nextShotInSeconds = FireRate;
    }

    public void Update()
    {
        if ((_nextShotInSeconds -= Time.deltaTime) > 0)
        {
            return;
        }

        _nextShotInSeconds = FireRate;
        var projectile = (PathedProjectile) Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initalize(Destination, Speed);

        if(SpawnEffect != null)
        {
            Instantiate(SpawnEffect, transform.position, transform.rotation);
        }

        if(CannonFireSound != null)
            AudioSource.PlayClipAtPoint(CannonFireSound, transform.position);

        if (Animator != null)
            Animator.SetTrigger("Fire");

    }

    public void OnDrawGizmos()
    {
        if(Destination == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Destination.position);
    }


}
