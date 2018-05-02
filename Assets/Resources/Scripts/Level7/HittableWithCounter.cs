using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableWithCounter : Hittable
{
    [SerializeField] private KillCounter killCounter;
    [SerializeField] private Transform[] barriers;
    [SerializeField] private Transform[] enemiesToDisable;
    [SerializeField] private Transform[] enemiesToActivate;
    [SerializeField] private bool isHuman = false;

    private bool alarmCalled = false;


    public override void UpdateHealth(int deltaHealth)
    {
        base.UpdateHealth(deltaHealth);
        if (!GetComponent<EnemyStatus>().AIManager.enabled)
            Alarm();
    }

    public void Alarm()
    {
        if (!alarmCalled && !PlayerChoices.Instance().Lv7SkeletonControlled)
        {
            alarmCalled = true;
            PlayerChoices.Instance().Lv7AlarmTriggered = true;
            killCounter.StartAttack(barriers, enemiesToDisable, enemiesToActivate);
        }
    }

    public void AttackGuard()
    {
        PlayerChoices.Instance().Lv7SkeletonControlled = true;
        foreach (Transform t in enemiesToActivate)
        {
            if (!t.GetComponent<HittableWithCounter>().isHuman)
            {
                t.gameObject.SetActive(true);
                t.GetComponent<EnemyStatus>().AIManager.enabled = true;
                t.GetComponent<EnemyStatus>().target = enemiesToDisable[0];
            }
        }
    }

    private void OnDestroy()
    {
        killCounter.NotifyKill();
    }
}