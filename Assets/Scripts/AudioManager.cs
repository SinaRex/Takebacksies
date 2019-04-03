﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    /* Audio Clips */
    public AudioClip deathClip;
    public List<AudioClip> dashingClips;
    /* Jumpting*/
    public List<AudioClip> jumpGruntClips;
    public List<AudioClip> jumpClips;
    /* FIXME: Jab might be change with something else */
    public List<AudioClip> jabGruntClips;
    public List<AudioClip> jabClips;
    /* Forward Normal*/
    public List<AudioClip> forwardNormalGruntClips;
    public List<AudioClip> forwardNoramlClips;
    /* Up Noraml*/
    public List<AudioClip> upNormalGruntClips;
    public List<AudioClip> upNormalClips;
    /* Down Normal*/
    public List<AudioClip> downNoramlGruntClips;
    public List<AudioClip> downNoramlClips;
    /* Back Air*/
    public List<AudioClip> backAirGruntClips;
    public List<AudioClip> backAirClips;
    /* Forward Air*/
    public List<AudioClip> forwardAirGruntClips;
    public List<AudioClip> forwardAirClips;
    /* Up Air*/
    public List<AudioClip> upAirGruntClips;
    public List<AudioClip> upAirClips;
    /* Down Air*/
    public List<AudioClip> downAirGruntClips;
    public List<AudioClip> downAirClips;
    /* Dash Attack*/
    public List<AudioClip> dashAttackGruntClips;
    public List<AudioClip> dashAttackclips;
    /* Getting Hurt*/
    public List<AudioClip> gettingHurtGruntClipst;
    public List<AudioClip> gettingHurtClips;

    /* This for playing multiple audio simultaniously*/
    public GameObject audioSourcePrefab;

    private AudioSource audioSource;
    private PlayerManager playerManager;
    private float jumpingFrameCounter = 0f;
    // Factor of how much echo must be quieter.
    private float echoFactor = 3; 


    // Start is called before the first frame update
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerManager = GetComponent<PlayerManager>();
    }


    // Update is called once per frame
    void Update()
    {
        CleanAudioPrefabs();

        switch(playerManager.GetState())
        {
            case PlayerState.Dead:
                PlayDeathSound();
                break;
            case PlayerState.Dashing:
                PlayDashingSound(Time.deltaTime);
                break;
            default:
                audioSource.loop = false;
                break;
        }
    }


    private void CleanAudioPrefabs()
    {
        foreach (var audio in GameObject.FindGameObjectsWithTag("AudioPrefab")) 
        {
            if (!audio.GetComponent<AudioSource>().isPlaying)
            {
                Destroy(audio);
            }
        }
    }


    private AudioSource GetNewAudioSource()
    {
        GameObject newAudio = Instantiate(audioSourcePrefab, 
                Vector3.zero, 
                Quaternion.identity);
        return newAudio.GetComponent<AudioSource>();
    }


    // Playing two Audios
    private void PlayTwoAudios(List<AudioClip> clips1, List<AudioClip> clips2, 
                                float volume1=1,  float volume2=1) 
    {
        // To play another audio simultaniously
        AudioSource newAud = GetNewAudioSource();

        audioSource.clip = clips1[Random.Range(0, clips1.Count)];
        newAud.clip = clips2[Random.Range(0, clips2.Count)];
        audioSource.volume = volume1;
        newAud.volume = volume2;

        newAud.Play();
        audioSource.Play();
    }


    // If you need to play more than 2 audios at the same use this
    private void PlayMultipleAudios(List<List<AudioClip>> listOfClips, List<List<float>> clipVolumes)
    {
        // Init the clips
        audioSource.clip = listOfClips[0][Random.Range(0, listOfClips[0].Count)];
        List<AudioSource> audios = new List<AudioSource>();
        for (int i = 0; i < listOfClips.Count - 1; i++)
        {
            audios.Add(GetNewAudioSource());
        }

        // Set the clips
        for (int i = 1; i < audios.Count; i++)
        {
            audios[i - 1].clip = listOfClips[i][Random.Range(0, listOfClips[i].Count)];
        }

        // Play the clips
        audioSource.Play();
        for (int i = 0; i < audios.Count; i++)
        { 
            audios[i].Play();
        }

    }


    /*** ============== AUDIO METHODS ============== **/
    public void PlayDeathSound()
    {
        audioSource.volume = 0.25f;
        audioSource.clip = deathClip;
        audioSource.Play();
    }


    public void PlayDashingSound(float frameNumUntillNextClip)
    {
        if (jumpingFrameCounter >= frameNumUntillNextClip)
        {
            jumpingFrameCounter = 0f; // reset frame counter for jumping

            if (playerManager.GetWhichPlayer() == PlayerIdentity.Echo)
                audioSource.volume = 0.25f/3; 
            else 
                audioSource.volume = 0.25f;

            audioSource.clip = dashingClips[Random.Range(0, dashingClips.Count)];
            audioSource.Play();

            // Reset the volume back
            //audioSource.volume = 1f;
        }
        else
        {
            jumpingFrameCounter += Time.deltaTime;
        }

    }


    public void PlayJumpingSound()
    {
        PlayTwoAudios(jumpGruntClips, jumpClips, 0.25f, 0.09f);

    }


    public void PlayJabSound()
    {
        if (playerManager.GetWhichPlayer() == PlayerIdentity.Echo)
            PlayTwoAudios(jabClips, jabGruntClips, 
                        1/echoFactor, 0.25f/echoFactor);
        else
            PlayTwoAudios(jabClips, jabGruntClips, 1, 0.25f);
    }


    public void PlayForwardNormalSound()
    {
        if (playerManager.GetWhichPlayer() == PlayerIdentity.Echo)
            PlayTwoAudios(forwardNoramlClips, forwardNormalGruntClips, 
                        0.8f / echoFactor, 0.25f / echoFactor);
        else
            PlayTwoAudios(forwardNoramlClips, forwardNormalGruntClips, 
                            0.8f, 0.25f); 
    }


    public void PlayUpNormal()
    {

    }


    public void PlayDownNormal()
    {

    }


    public void PlayBackAir()
    {

    }


    public void PlayForwardAir()
    {

    }


    public void PlayUpAir()
    {

    }


    public void PlayDownAirSound()
    {
        PlayDashAttackSound();
    }


    public void PlayDashAttackSound()
    {
        if (playerManager.GetWhichPlayer() == PlayerIdentity.Echo)
            PlayTwoAudios(dashAttackGruntClips, dashAttackclips, 0.25f/3, 0.25f/3);
        else 
            PlayTwoAudios(dashAttackGruntClips, dashAttackclips, 0.25f, 0.25f);
      
    }
    /*** ============== AUDIO METHODS ============== **/

}
