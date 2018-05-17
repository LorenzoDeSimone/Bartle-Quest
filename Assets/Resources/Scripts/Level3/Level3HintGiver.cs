using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3HintGiver : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
        
	void Start () {
        StartCoroutine(HintCoroutine());
	}

    private IEnumerator HintCoroutine()
    {
        yield return new WaitForSeconds(300);
            dialogueManager.InitDialogue(GetComponent<Talker>());
    }
}
