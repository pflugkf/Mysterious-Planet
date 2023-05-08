using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoxManager : MonoBehaviour
{
    [SerializeField] private GameObject lid;
    [SerializeField] private ParticleSystem healthEffect;
    private bool boxOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!boxOpened)
        {
            //Open chest
            lid.SetActive(false);

            //Play health particle effect
            healthEffect.Play();

            //Add health to player
            GameManager.instance.player.ChangeHealth(25);
            boxOpened = true;
        }
        
    }
}
