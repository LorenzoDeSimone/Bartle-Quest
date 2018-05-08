using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLibrary : MonoBehaviour
{
    [SerializeField] private float timeToReachFinalPosition = 4f;
    [SerializeField] private GameObject columnProps;

    private IEnumerator MoveAway()
    {        
        float elapsedTime = 0f;
        Vector3 finalPosition = transform.position + Vector3.right * 3;
        Vector3 startPosition = transform.position;
        GetComponent<Talker>().enabled = false;
        GetComponent<Target>().enabled = false;

        if (columnProps)
            columnProps.SetActive(false);

        while (elapsedTime < timeToReachFinalPosition)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, finalPosition, elapsedTime / timeToReachFinalPosition);
            yield return null;
        }
        transform.position = finalPosition;
        enabled = false;
    }

    public void Move()
    {
        AudioManager.Instance().PlaySuccess();
        StartCoroutine(MoveAway());
    }

    // Use this for initialization
    void Start () {
        if (PlayerChoices.Instance().CanSeeHiddenWalls)
            Destroy(GetComponent<Target>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
