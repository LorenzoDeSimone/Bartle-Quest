using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : TargetManager
{
    [SerializeField] GameObject interactionPopup;//Hierarchy: PopupGameObject -> Text -> Image

    private Image buttonSlot;
    private Text textSlot;

    // Use this for initialization
    new void Start()
    {
        base.Start();

        if (interactionPopup == null)
            Debug.LogError("Missing interaction popup reference.");
        else
        {
            textSlot = interactionPopup.GetComponentInChildren<Text>();
            buttonSlot = textSlot.GetComponentInChildren<Image>();
        }
    }

    void Update()
    {
        if (DialogueManager.IsDialogueOn)
            return;

        Transform nearestTarget= GetNearestTarget();
        if (nearestTarget == null)
            interactionPopup.SetActive(false);
        else
        {
            Interactable i = nearestTarget.GetComponent<Interactable>();
            if (i != null && (!i.CanInteract() || !i.enabled))
                return;

            Target t = nearestTarget.gameObject.GetComponent<Target>();
            if (t != null && !t.IsEnemy)
            {
                interactionPopup.SetActive(true);
                buttonSlot.sprite = t.Buttonimage;
                textSlot.text = t.Text;
            }
        }
    }

    /*
   new void OnTriggerEnter(Collider collider)
   {
       base.OnTriggerEnter(collider);
   }

   
   new void OnTriggerExit(Collider collider)
   {
       base.OnTriggerExit(collider);
       interactionPopup.SetActive(false);
   }*/
}
