using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordOfTheDeadsStatus : EnemyStatus
{
    [SerializeField] public Transform[] teleportPoints;
    [SerializeField] public ParticleSystem[] floatingSkullLights;
    [SerializeField] private MeshRenderer head;
    [SerializeField] private MeshRenderer weapon;
    [SerializeField] private SkinnedMeshRenderer body;
    [SerializeField] private Material stone;
    [SerializeField] public int skeletonToSummon = 2;
    [SerializeField] public int deltaHitToTeleport = 5;
    [SerializeField] public HashSet<Transform> currentSkeletons;

    protected new void Start()
    {
        base.Start();
        currentSkeletons = new HashSet<Transform>();
    }

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
                foreach (ParticleSystem t in floatingSkullLights)
                {
                    if (t)
                    {
                        t.GetComponentInParent<AnimationScript>().enabled = false;
                        Destroy(t);
                    }
                }
            }
        }
    }
}
