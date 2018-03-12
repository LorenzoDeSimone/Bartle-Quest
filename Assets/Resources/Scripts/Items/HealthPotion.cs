using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int heal = 1;

    void OnTriggerEnter(Collider collider)
    {
        CharacterStatus character = collider.GetComponent<CharacterStatus>();
        
        if(character!=null && character.CanCollectItems)
        {
            Hittable hittable = character.GetComponent<Hittable>();
            if (hittable != null)
            {
                hittable.UpdateHealth(heal);
                Destroy(gameObject);
            }
        }
        
    }
}
