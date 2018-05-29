using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript2 : MonoBehaviour {

     public AudioClip MusicClip1;

      public AudioSource MusicSource1;
    // Use this for initialization
    void Start () {
        MusicSource1.clip = MusicClip1;
    }

    // Update is called once per frame
    void Update () {
        if (PlayerController.isHole == true) { MusicSource1.Play(); }
        if (PlayerController.isFalling == true) { MusicSource1.Play(); }
        if (PlayerController.isFire == true) { MusicSource1.Play(); }
        if (PlayerController.isCollision == true) { MusicSource1.Play(); }
         }
    }

