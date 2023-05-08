using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    [SerializeField] GameObject door;
    private bool isDoorOpen = false;
    private Vector3 closedPosition;
    private float duration = 3f;
    private float openHeight = -19.5f;

    private AudioSource gateOpenSound;

    // Start is called before the first frame update
    void Start()
    {
        gateOpenSound = GetComponent<AudioSource>();
        gateOpenSound.Stop();
        closedPosition = door.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Lowers door if player is at the gate with both keys
            if (GameManager.instance.player.HasBothKeys())
            {
                StopAllCoroutines();
                if (!isDoorOpen)
                {
                    Vector3 openPosition = closedPosition + Vector3.up * openHeight;
                    StartCoroutine(MoveDoor(openPosition));
                }
                isDoorOpen = true;
            }
            else
            {
                //print("player needs both keys");
            } 
        }
    }

    IEnumerator MoveDoor(Vector3 targetPosition)
    {
        float timeElapsed = 0;
        gateOpenSound.Play();
        
        while (timeElapsed < duration)
        {
            door.transform.position = Vector3.Lerp(closedPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        gateOpenSound.Stop();
    }
}
