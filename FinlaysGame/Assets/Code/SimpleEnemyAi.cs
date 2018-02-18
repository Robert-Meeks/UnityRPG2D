using System;
using UnityEngine;

public class SimpleEnemyAi : MonoBehaviour, ITakeDamage, IPlayerRespawnListener
{
    public float Speed;
    public float FireRate = 1;
    public Projectile Projectile;
    public GameObject DestroyedEffect;
    public int PointsToGivePlayer;
    public AudioClip EnemyShootSound;

    private CharacterController2D _controller; // responsible for having enemys interaction with platforms
    private Vector2 _direction;
    private Vector2 _startPosition;
    private float _canFireIn;


    public void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        _direction = new Vector2(1, 0);
        _startPosition = transform.position;
       // gameObject.SetActive(true);
    }

    public void Update()
    {
        _controller.SetHorizontalForce(_direction.x * Speed);

        if ((_direction.x < 0 && _controller.State.IsCollidingLeft) || (_direction.x > 0 && _controller.State.IsCollidingRight))
        {
            Debug.Log("the if****************************");
            _direction.x = _direction.x * -1;
           // _direction = -_direction;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }

        if ((_canFireIn -= Time.deltaTime) > 0)
        {
            return;
        }

        var raycast = Physics2D.Raycast(transform.position, _direction, 10, 1 << LayerMask.NameToLayer("Player"));

        if (!raycast)
            return;

        var projectile = (Projectile)Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initialize(gameObject, _direction, _controller.Velocity);
        _canFireIn = FireRate;

        if (EnemyShootSound != null)
            AudioSource.PlayClipAtPoint(EnemyShootSound, transform.position);

    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        if(PointsToGivePlayer != 0)
        {
            var projectile = instigator.GetComponent<Projectile>();
            if (projectile != null && projectile.Owner.GetComponent<Player>() != null)
            {
                GameManager.Instance.AddPoints(PointsToGivePlayer);
                FloatingText.Show(String.Format("+{0}!", PointsToGivePlayer), "PointStarText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
            }
        }
        Instantiate(DestroyedEffect, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    public void OnPlayerRespawnInThischeckpoint(Checkpoint checkpoint, Player player)
    {
        _direction = new Vector2(-1, 0);
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = _startPosition;
        gameObject.SetActive(true);
    }
}

