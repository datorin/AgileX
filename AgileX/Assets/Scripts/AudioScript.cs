using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 


public class AudioScript : MonoBehaviour {

    public AudioClip MusicClip;

    public AudioSource MusicSource;

   // public AudioClip MusicClip1;

  //  public AudioSource MusicSource1;


    // Use this for initialization
    void Start () {
        MusicSource.clip = MusicClip;
       // MusicSource1.clip = MusicClip1;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.X))
        {
            if (PlayerController.amunition > 0)
            {
                MusicSource.Play();
            }
        }
       // else
        //{

          //  if (PlayerController.isHole == true) { MusicSource1.Play(); }
            //if (PlayerController.isFalling == true) { MusicSource1.Play(); }
            //if (PlayerController.isFire == true) { MusicSource1.Play(); }
            //if (PlayerController.isCollision == true) { MusicSource1.Play(); }
       // }
    }
}
