using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    [SerializeField] private string keyName;

    private void OnTriggerEnter(Collider other)
    {
        //Player picks up key
        GameManager.instance.player.PickUpKey(keyName);
        Destroy(gameObject);
    }
}
