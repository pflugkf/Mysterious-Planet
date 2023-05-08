using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public PlayerManager player;
    private AudioSource music;

    void Awake()
    {
        //Check to see if the singleton exists already
        if (instance == null) {
            //Create singleton by assigning it to this game object
            instance = this;

            //Prevents this game object from getting destroyed when we change screen
            DontDestroyOnLoad(gameObject);
        } else {
            //Singleton exists already, so destroy this game object
            Destroy(gameObject);
        }

        music = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //restart the level by pressing R
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void StopMusic()
    {
        if (music.isPlaying)
        {
            music.Stop();
        }
    }

    public void Win()
    {
        StopMusic();
        //print("You have finished the prototype level");
        SceneManager.LoadScene("WinScreen");
        Destroy(gameObject);
    }

    public IEnumerator Lose()
    {
        StopMusic();
        //print("You died");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("LoseScreen");
        Destroy(gameObject);
    }
}
