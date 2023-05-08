using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static GUIManager instance;

    [SerializeField] private Image healthBarImage;

    private void Awake()
    {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }


    }
    public void UpdateHealthBar(float healthPercentage)
    {
        healthBarImage.fillAmount = healthPercentage;
    }
}
