using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour, ITakeDamage {

    private bool _isFacingRight = true;
    private CharacterController2D _controller;
    private float _normalizedHorizontalSpeed;

    public float MaxSpeed = 8;
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;
    public int MaxHealth = 100;
    public GameObject OuchEffect;
    public /* GameObject */ Projectile Projectile;
    public float FireRate;
    public Transform ProjectileFireLocation;
    public GameObject FireProjectileEffect;
    public AudioClip PlayerHitSound;
    public AudioClip PlayerShootSound;
    public AudioClip PlayerHealthSound;

    public int Health { get; private set; }
    public bool IsDead { get; private set; }

    private float _canFireIn;
   

    public void Awake() // note: Awake() runs before Start() which is why Awake is used here because we want it to run before levelManagers Start() 
    {
        _controller = GetComponent<CharacterController2D>();
        _isFacingRight = transform.localScale.x > 0;
        Health = MaxHealth; // so at game/level start health is set to max
        
    }

    public void Update()
    {
        _canFireIn -= Time.deltaTime;

        if(!IsDead) // so if is dead then no more inputs are used
            HandleInput();

        var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;

        if (IsDead)
            _controller.SetHorizontalForce(0);
        else
            _controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));
    }

    public void FinishLevel()
    {
        Debug.Log("FINISH LEVEL TRIGGERED");
        enabled = false; // stops the player from moving -- think its --> this.enabled...
        _controller.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        //collider2D.enabled = false;
    }

    public void Kill()
    {
        _controller.HandleCollisions = false;
        GetComponent<Collider2D>().enabled = false; // the code he uses is depricated the code used here was suggested in comments section. The code he used is as follows: collider2D.enabled = false;
        IsDead = true;
        Health = 0; // required so that an insta death makes the health bar go to 0

        _controller.SetForce(new Vector2(0, 10));
    }

    public void RespawnAt(Transform spawnpoint)
    {
        if (!_isFacingRight)
            Flip();

        IsDead = false;
        GetComponent<Collider2D>().enabled = true; // the code he uses is depricated the code used here was suggested in comments section. The code he used is as follows: collider2D.enabled = true;
        _controller.HandleCollisions = true;
        Health = MaxHealth;

        transform.position = spawnpoint.position;
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        AudioSource.PlayClipAtPoint(PlayerHitSound, transform.position);
        FloatingText.Show(string.Format("-{0}", damage), "PlayerTakeDamageText", new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));

        Instantiate(OuchEffect, transform.position, transform.rotation);
        Health -= damage;

        if(Health <= 0)
        {
            LevelManager.Instance.KillPlayer();
        }
    }

    public void GiveHealth(int health, GameObject instigator)
    {
        AudioSource.PlayClipAtPoint(PlayerHitSound, transform.position);
        FloatingText.Show(string.Format("+{0}", health), "PlayerGotHealthText", new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60));
        Health = Mathf.Min(Health + health, MaxHealth); // so this adds health to Health and stops Health from exceeding MaxHealth
    }

    private void HandleInput()
    {
        float movement = Input.GetAxis("leftJS");
        bool moving = false;
        if (movement != 0) { moving = true; }

        if (Input.GetAxis("leftJS") > 0) // moving right i think
        {
            _normalizedHorizontalSpeed = 1;
            if (!_isFacingRight)
                Flip();
        }
        else if (Input.GetAxis("leftJS") < 0)
        {
            _normalizedHorizontalSpeed = -1;
            if (_isFacingRight)
                Flip();
        }
        else
        {
            _normalizedHorizontalSpeed = 0;
        }
        
        if(_controller.CanJump && Input.GetButton("A"))
        {
            _controller.Jump();
        }

        if (Input.GetButton("B"))
        {
            FireProjectile();
        }

        //if(Input.GetMouseButtonDown(0))
        //{
        //    FireProjectile();
        //}

    } // END HandleInput()

    private void FireProjectile()
    {
        if(_canFireIn > 0 )
        {
            return;
        }

        if (FireProjectileEffect != null)
        {
           var effect = (GameObject)Instantiate(FireProjectileEffect, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
            effect.transform.parent = transform;
        }

        var direction = _isFacingRight ? Vector2.right : -Vector2.right;

        var projectile = (Projectile)Instantiate(Projectile, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
        projectile.Initialize(gameObject, direction, _controller.Velocity);

        /* this is one way of doing - he did thi first and then decided on a better way which he implements in 
         * Projectile.cs/ initialise()
         * projectile.transform.localScale = new Vector3(_isFacingRight ? 1 : -1, 1, 1);
         */

        _canFireIn = FireRate;

        AudioSource.PlayClipAtPoint(PlayerShootSound, transform.position);
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        _isFacingRight = transform.localScale.x < 0;
    }
}
