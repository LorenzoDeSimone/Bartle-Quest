using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordOfTheDeadsStatus : EnemyStatus
{
    [SerializeField] public Transform[] teleportPoints;
    [SerializeField] public Transform[] floatingSkulls;
    [SerializeField] private MeshRenderer head;
    [SerializeField] private MeshRenderer weapon;
    [SerializeField] private SkinnedMeshRenderer body;
    [SerializeField] private Material stone;

    public bool PetrifiedStatus
    {
        get
        {
            return myAnimator.GetBool("isPetrified");
        }
        set
        {
            if(value)
            {
                myAnimator.SetBool("isPetrified", true);
                weapon.material = head.material = body.material = stone;
                Destroy(GetComponent<Hittable>());
                Destroy(healthBarGO);
                myAnimator.speed = 0f;
                foreach (Transform t in floatingSkulls)
                {
                    t.GetComponent<AnimationScript>().enabled = false;
                    Destroy(t.GetComponentInChildren<ParticleSystem>());
                    Destroy(t.GetComponentInChildren<Light>());
                }
            }
        }
    }
}
