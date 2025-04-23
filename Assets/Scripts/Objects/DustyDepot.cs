using UnityEngine;

public class DustyDepot : MonoBehaviour,IDamageable
{

    void IDamageable.TakeDamage(float damage)
    {
        Debug.Log($"VRO I ({gameObject.name}) JUST TOOK {damage} DAMAGE TWIN");
    }

}
