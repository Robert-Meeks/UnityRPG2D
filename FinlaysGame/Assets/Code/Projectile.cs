using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    /* example of abstract mono behavior - can have a player with a public field of type projectile and will be able 
     * to fill it in with any prefab that has a component on it that inherits from this class that will gaurantee 
     * certain functionality  
     */

    public float Speed;
    public LayerMask CollisionMask;

    public GameObject Owner { get; private set; }
    public Vector2 Direction { get; private set; }
    public Vector2 InitialVelocity { get; private set; }

    public void Initialize(GameObject owner, Vector2 direction, Vector2 initialVelocity)
    {
        transform.right = direction; // the microphones were pointing in the wrong direction when fired right to left this fixes it 
        Owner = owner;
        Direction = direction;
        InitialVelocity = initialVelocity;
        OnInitialized();
    }

    // NB protected means only this class and children can call them AND virtual means you CAN override them if you 
    // want the child to do that
    protected virtual void OnInitialized()
    {

    }

    // this is a unity method
    public virtual void OnTriggerEnter2D(Collider2D other) // there are 4 collision scenarios 1) something that doesnt match the collision mask 2) the owner of the projectile 3) some thing capable or receiving damage 4) none of the above
    {
        Debug.Log("OnTriggerEnter2D");
        /*layers refers to the layers in unity 
         the logic in the IF below:
         take the layers assume there are only 8 layers (there are actually 32 (0-31)
         This would mean that each layer can be stored as an 8 bit BYTE. The binary representation of each layer
         is therefore as follows:
         layer #   Binary      Decimal
         layer 0 = 0000 0001 = 1
         layer 1 = 0000 0010 = 2
         layer 2 = 0000 0100 = 4
         layer 3 = 0000 1000 = 8
         layer 4 = 0001 0000 = 16
         layer 5 = 0010 0000 = 32
         layer 6 = 0100 0000 = 64
         layer 7 = 1000 0000 = 128

        so if we wanted to write a layer mask that only effects layers 2 and 5 you would get 0010 0100
        for layers 0 3 4 and 6 it would be 0101 1001

        so lets say our layer mask = 0110 0110   (layers 2, 4, 5, 6)
        so in the if we areasking: is layer 5 in the mask?
        to do this we first need to change 5 into 32 (decimal value of the binary number assigned to layer 5 above)
        this is done by (1 << 5) or in english take "1" and shift it left 5 times so we have 0000 0000 and if we do
        1 << 5 on this start with the far right 0 which is position 0 and move left 5 times then turn that 0 into a
        1 which leaves 0010 0000
        0000 0001 << 5 = 0010 0000 = 32

        now we need to know if 32 is in the mask, to do this we use the bit wise operator like so:
          0110 0110   (our mask)
          &
          0010 0000   (layer 5)
        = 0010 0000
         
         */
        if ((CollisionMask.value & (1  << other.gameObject.layer)) == 0) // scenario 1 (something that doesnt match the collision mask)
        {// so we enter if the other.gameObject.layer is not in the mask because if its not in the mask a bit wise & operation with the mask and a layer only == 0 if it is not in the mask
            Debug.Log("collison not on mask");
            OnNotCollideWith(other);
            return;
        }

        var isOwner = other.gameObject == Owner;// scenario 2
        if(isOwner)
        {
            Debug.Log("is owner");
            OnCollideOwner();
            return;
        }

        // normally would use the generic form of GetComponent ie GetComponent<> but this will not work with an Interface 
        // so we have to use the non generic form of GetComponent. this version works by passing in the type of the 
        // interface by getting the type using typeof and then casting the return value (which is of type GetComponent
        // to the interface 
        var takeDamage = (ITakeDamage)other.GetComponent(typeof(ITakeDamage)); // scenario 3
        if(takeDamage != null)
        {
            Debug.Log("take damage");
            OnCollideTakeDamage(other, takeDamage);
            return;
        }

        Debug.Log("OnCollideOther()");
        OnCollideOther(other); // scenario 4

    }

    protected virtual void OnNotCollideWith(Collider2D other)
    {

    }

    protected virtual void OnCollideOwner()
    {

    }

    protected virtual void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage)
    {

    }

    protected virtual void OnCollideOther(Collider2D other)
    {

    }
}
