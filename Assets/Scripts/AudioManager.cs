using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    /* Audio Clips */
    public AudioClip deathClip;
    public AudioClip rockFormationClip;
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
    /* FIXME: Up Noraml (Used Jab)*/
    //public List<AudioClip> upNormalGruntClips;
    //public List<AudioClip> upNormalClips;
    /* FIXME: Down Normal  (used mixed audio instead two audios) */
    public List<AudioClip> downNoramlGruntClips;
    public List<AudioClip> downNoramlClips;
    /* FIXME: Back Air (Using only BackAirClips)*/
    public List<AudioClip> backAirGruntClips; // Not using this
    public List<AudioClip> backAirClips;
    /* FIXME: Forward Air (Using forwardNoramal)*/
    //public List<AudioClip> forwardAirGruntClips;
    //public List<AudioClip> forwardAirClips;
    /*FIXME: Up Air using jab*/
    //public List<AudioClip> upAirGruntClips;
    //public List<AudioClip> upAirClips;
    /* FIXME: Down Air (Using Down Normal)*/
    //public List<AudioClip> downAirGruntClips;
    //public List<AudioClip> downAirClips;
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
    private float frameCounter = 0f;
    private bool once = false;
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
                PlayDashingSound(Time.deltaTime / 3);
                break;
            case PlayerState.Respawning:
                //if (!audioSource.isPlaying) PlayRespawnRockFormation(0f);
                break;
            default:
                audioSource.loop = false;
                once = false;
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

        if (clips1.Count > 0)
            audioSource.clip = clips1[Random.Range(0, clips1.Count)];
        if (clips2.Count > 0)
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
        if (frameCounter >= frameNumUntillNextClip)
        {
            frameCounter = 0f; // reset frame counter for jumping

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
            frameCounter += Time.deltaTime;
        }

    }


    public void PlayJumpingSound()
    {
        PlayTwoAudios(jumpGruntClips, jumpClips, 0.25f, 0.25f);

    }

    public void PlayRespawnRockFormation(float frameNumUntillNextClip)
    {
        if (frameCounter >= frameNumUntillNextClip)
        {
            frameCounter = 0f; // reset frame counter for jumping

            audioSource.clip = rockFormationClip;
            audioSource.volume = 0.3f;
            if (!once) audioSource.Play(); once = true;

            // Reset the volume back
            //audioSource.volume = 1f;
        }
        else
        {
            frameCounter += Time.deltaTime;
        }

    }


    public void PlayHurtSound()
    {
        PlayTwoAudios(gettingHurtClips, gettingHurtGruntClipst, 0.30f, 0.25f);
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
                        0.4f / echoFactor, 0.25f / echoFactor);
        else
            PlayTwoAudios(forwardNoramlClips, forwardNormalGruntClips, 
                            0.4f, 0.25f); 
    }


    public void PlayUpNormal()
    {
        PlayJabSound();
    }


    public void PlayDownNormal()
    {
        if (playerManager.GetWhichPlayer() == PlayerIdentity.Echo)
            PlayTwoAudios(downNoramlClips, downNoramlGruntClips,
                        0.8f / echoFactor, 0.25f / echoFactor);
        else
            PlayTwoAudios(downNoramlClips, downNoramlGruntClips,
                            0.8f, 1f);
    }


    public void PlayBackAir()
    {
        //if (playerManager.GetWhichPlayer() == PlayerIdentity.Echo)
        //    PlayTwoAudios(backAirClips, backAirGruntClips,
        //                0.8f / echoFactor, 0f / echoFactor);
        //else
        //PlayTwoAudios(backAirClips, backAirGruntClips,
        //1f, 0f);
        PlayForwardNormalSound();
    }


    public void PlayForwardAir()
    {
        PlayForwardNormalSound();

    }


    public void PlayUpAir()
    {
        PlayJabSound();
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
