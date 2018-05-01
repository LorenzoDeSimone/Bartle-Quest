using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableWithCounter : Hittable
{
    [SerializeField] private KillCounter killCounter;
    [SerializeField] private Transform[] barriers;
    [SerializeField] private Transform[] enemiesToDestroy;
    [SerializeField] private Transform[] enemiesToActivate;
    private bool alarmCalled = false;


    public override void UpdateHealth(int deltaHealth)
    {
        base.UpdateHealth(deltaHealth);
        if (!GetComponent<EnemyStatus>().AIManager.enabled)
            Alarm();
    }

    public void Alarm()
    {
        if (!alarmCalled)
        {
            alarmCalled = true;
            BartleStatistics.Instance().IncrementKiller();
            killCounter.StartAttack(barriers, enemiesToDestroy, enemiesToActivate);
        }
    }

    private void OnDestroy()
    {
        killCounter.NotifyKill();
    }
}