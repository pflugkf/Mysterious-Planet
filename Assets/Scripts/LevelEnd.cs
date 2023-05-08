using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] public GameObject endLevelSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        endLevelSpawnPoint = GameObject.Find("EndPointCheat");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector3 newPos = endLevelSpawnPoint.transform.position;
            GameManager.instance.player.EndCheatPressed(newPos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.Win();
        }
    }
}
