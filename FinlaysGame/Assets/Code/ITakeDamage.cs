using UnityEngine;


// so any mono behaviour that implements this interface will be picked up by our projectile and the proper method 
// will be invoked (the OnCollideTakeDamage())
public interface ITakeDamage
{
    void TakeDamage(int damage, GameObject instigator);
}

