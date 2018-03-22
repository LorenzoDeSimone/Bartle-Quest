using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrisonDetonator : MonoBehaviour
{
    [SerializeField] private ExplodingDoor door;
    [SerializeField] private GameObject debris;
    [SerializeField] private Transform finalPlayerTransform;
    [SerializeField] private Transform player;
    private float time = 0.5f;
    private float elapsedTime=0;

    public void Detonate()
    {
        if (door)
        {
            door.Explode();
            debris.SetActive(true);
            StartCoroutine(MovePlayer(player.transform.position));
        }
    }

    IEnumerator MovePlayer(Vector3 startPosition)
    {
        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            player.transform.position = Vector3.Lerp(startPosition, finalPlayerTransform.position, elapsedTime / time);
            yield return null;
        }
    }
}
