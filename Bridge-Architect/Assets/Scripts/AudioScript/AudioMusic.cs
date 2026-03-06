using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMusic : MonoBehaviour
{
    
    public AudioSource music;
    public AudioSource water;

    void Start()
    {
        music.Play();
        water.Play();
    }

    void Update()
    {
        
    }
}
