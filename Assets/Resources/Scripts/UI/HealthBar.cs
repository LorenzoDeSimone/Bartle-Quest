using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform characterToFollow;
    public Hittable characterHittable;
    public Image healthBar;
    public Text hpText;

    public float height = 2f;
    private static Vector3 Scale = new Vector3(1f, 1f, 1f);


    public static GameObject CreateHealthBar(Transform objectToFollow, Hittable hittable)
    {
        GameObject Canvas = GameObject.Find("Canvas");

        UnityEngine.Object healthBarPrefab = Resources.Load("Prefabs/UI/HealthBar");
        GameObject healthBarGO = (GameObject) Instantiate(healthBarPrefab);

        HealthBar myHealthBar;
        myHealthBar = healthBarGO.GetComponent<HealthBar>();
        myHealthBar.characterHittable = hittable;
        myHealthBar.characterToFollow = objectToFollow;

        myHealthBar.transform.SetParent(Canvas.transform);
        myHealthBar.transform.localPosition = Vector3.zero;
        myHealthBar.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        myHealthBar.transform.localScale = Scale;

        myHealthBar.hpText.gameObject.SetActive(PlayerChoices.Instance().CanSeeEnemyHP);

        return healthBarGO;
    }

    void Start()
    {
        if (!healthBar)
            Debug.LogError("The health bar is missing bar image or background image.");
    }

    void LateUpdate()
    {
        if (characterToFollow)
        {
            transform.position = characterToFollow.transform.position + Vector3.up * height;
            healthBar.fillAmount = (float)characterHittable.CurrentHealth / (float)characterHittable.MaxHealth;
            if (PlayerChoices.Instance().CanSeeEnemyHP)
                hpText.text = characterHittable.CurrentHealth + "/" + characterHittable.MaxHealth;
        }
        else
            Destroy(gameObject);
    }
}
