﻿using UnityEngine;
using System.Collections;

public class UserTargetScript : MonoBehaviour
{

    float randomTime;
    bool routineStarted = false;

    //Used to play animation at upper object(parent)
    public Animation animation = new Animation();
    public bool canReturn = true;

    //Used to check if the target has been hit
    public bool isHit = false;
    [Header("Customizable Options")]
    //Minimum time before the target goes back up
    public float minTime;
    //Maximum time before the target goes back up
    public float maxTime;

    [Header("Audio")]
    public AudioClip upSound;
    public AudioClip downSound;

    public AudioSource audioSource;

    private void Start()
    {
        animation = this.gameObject.GetComponentInParent<Animation>();
    }

    void Update()
    {

        //Generate random time based on min and max time values
        randomTime = Random.Range(minTime, maxTime);

        //If the target is hit
        if (isHit == true)
        {
            if (routineStarted == false)
            {
                //Animate the target "down"
                animation.Play("target_down");

                //Set the downSound as current sound, and play it
                audioSource.GetComponent<AudioSource>().clip = downSound;
                audioSource.Play();

                if (canReturn)
                {
                    //Start the timer
                    StartCoroutine(DelayTimer());
                }
                routineStarted = true;
            }
        }
    }

    //Time before the target pops back up
    IEnumerator DelayTimer()
    {
        //Wait for random amount of time
        yield return new WaitForSeconds(randomTime);
        //Animate the target "up" 
        animation.Play("target_up");

        //Set the upSound as current sound, and play it
        audioSource.GetComponent<AudioSource>().clip = upSound;
        audioSource.Play();

        //Target is no longer hit
        isHit = false;
        routineStarted = false;
    }
}