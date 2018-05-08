using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] private Torch[] torches;
    [SerializeField] private Vector3 finalPosition;
    [SerializeField] private float timeToReachFinalPosition = 5f;
    [SerializeField] private bool playSuccessSound = false;
    bool puzzleSolved = false;
    private float elapsedTime = 0f;

    void Start()
    {
        finalPosition = transform.position + Vector3.forward * 3;
    }

	// Update is called once per frame
	void Update ()
    {
        if (!puzzleSolved && AllTorchesLit())
        {
            puzzleSolved = true;

            if (playSuccessSound)
                AudioManager.Instance().PlaySuccess();

            StartCoroutine(MoveAway(transform.position));
        }
	}

    private bool AllTorchesLit()
    {
        foreach(Torch torch in torches)
        {
            if (!torch.status.Equals(Torch.TORCH_STATUS.RED))
                return false;
        }
        return true;
    }


    private IEnumerator MoveAway(Vector3 startPosition)
    {
        while (elapsedTime < timeToReachFinalPosition)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, finalPosition, elapsedTime / timeToReachFinalPosition);
            yield return null;
        }
        transform.position = finalPosition;
        enabled = false;
    }
}
