using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] GameObject door;
    private bool isDoorOpen = false;
    private Vector3 closedPosition;
    private float duration = 1;
    private float openHeight = -4f;

    private AudioSource doorOpenSound;

    // Start is called before the first frame update
    void Start()
    {
        doorOpenSound = GetComponent<AudioSource>();
        doorOpenSound.Stop();
        closedPosition = door.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Lower the door if the player stands on the corresponding pressure plate
            StopAllCoroutines();

            if (!isDoorOpen)
            {
                Vector3 openPosition = closedPosition + Vector3.up * openHeight;
                StartCoroutine(MoveDoor(openPosition));
            }

            isDoorOpen = true;
        }
    }

    IEnumerator MoveDoor(Vector3 targetPosition)
    {
        float timeElapsed = 0;
        doorOpenSound.Play();
        
        while (timeElapsed <= duration)
        {
            door.transform.position = Vector3.Lerp(closedPosition, targetPosition, timeElapsed / duration);
            
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        doorOpenSound.Stop();
    }
}
