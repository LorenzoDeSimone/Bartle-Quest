using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public enum TORCH_STATUS { UNLIT, RED, BLUE};

    [SerializeField] private bool isActive = false;
    [SerializeField] public TORCH_STATUS status;

    public Color redLight, blueLight;
    public Gradient redGradient, blueGradient;
    [SerializeField] private Light myLight;
    [SerializeField] private ParticleSystem myParticle;

	// Use this for initialization
	void Start ()
    {
        SetStatus(status);
        Collider collider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void SetStatus(TORCH_STATUS status)
    {
        this.status = status;

        if (status.Equals(TORCH_STATUS.UNLIT))
        {
            myLight.gameObject.SetActive(false);
            myParticle.gameObject.SetActive(false);
        }
        else if (status.Equals(TORCH_STATUS.RED))
        {
            myLight.gameObject.SetActive(true);
            myLight.color = redLight;

            myParticle.gameObject.SetActive(true);
            var col = myParticle.colorOverLifetime;
            col.enabled = true;
            col.color = redGradient;
            //myParticle.colorOverLifetime.color = redGradient;
        }
        else if (status.Equals(TORCH_STATUS.BLUE))
        {
            myLight.gameObject.SetActive(true);
            myLight.color = blueLight;

            myParticle.gameObject.SetActive(true);
            var col = myParticle.colorOverLifetime;
            col.enabled = true;
            col.color = blueGradient;
        }
    }


    void OnTriggerEnter(Collider collision)
    {
        if (!isActive)
            return;

        Torch otherTorch = collision.GetComponent<Torch>();

        if (otherTorch)
        {
            if (otherTorch.status.Equals(TORCH_STATUS.UNLIT))
            {
                if (status.Equals(TORCH_STATUS.RED))
                    otherTorch.SetStatus(TORCH_STATUS.RED);
            }
            else if (otherTorch.status.Equals(TORCH_STATUS.RED))
            {
                if (status.Equals(TORCH_STATUS.BLUE))
                    otherTorch.SetStatus(TORCH_STATUS.UNLIT);
                if (status.Equals(TORCH_STATUS.UNLIT))
                    SetStatus(TORCH_STATUS.RED);
            }
            else if (otherTorch.status.Equals(TORCH_STATUS.BLUE))
            {
                if (status.Equals(TORCH_STATUS.RED))
                    SetStatus(TORCH_STATUS.UNLIT);
            }
        }
    }
}
