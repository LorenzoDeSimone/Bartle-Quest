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

    void OnTriggerStay(Collider collider)
    {

        Target t = collider.gameObject.GetComponent<Target>();
        if (t != null && !t.IsEnemy && Vector3.Distance(transform.position, t.transform.position) <= Vector3.Distance(transform.position , GetNearestTarget().position))
        {
            interactionPopup.SetActive(true);
            buttonSlot.sprite = t.Buttonimage;
            textSlot.text = t.Text;
        }
    }

    new void OnTriggerExit(Collider collider)
    {
        base.OnTriggerExit(collider);
        interactionPopup.SetActive(false);
    }
}
