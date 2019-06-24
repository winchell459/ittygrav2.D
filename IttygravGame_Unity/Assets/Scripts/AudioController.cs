using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public float MusicVolume = 0.50f;
    public float FXVolume = 0.5f;

    public AudioClip[] SceneMusic;
    public AudioClip[] PlayerFX;
    public float[] PlayerFXVolumeScale;

    private bool muteFX = false;
    private int currentFX = 0;
    
	// Use this for initialization
	void Start () {
        GameObject AS = GameObject.FindGameObjectWithTag("AudioSource");
        if (AS && AS != gameObject) Destroy(gameObject);
        else GameObject.DontDestroyOnLoad(gameObject);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        GetComponent<AudioSource>().volume = MusicVolume;
    }
    public void MuteVolume()
    {
        GetComponent<AudioSource>().mute = !GetComponent<AudioSource>().mute;
        
    }

    public void SetFXVolume(float volume){
        FXVolume = volume;
    }

    public void MuteFX(){
        muteFX = !muteFX;
    }

    public void PlayPlayerFX(int index){
        if(index < PlayerFX.Length && index < PlayerFXVolumeScale.Length){
            AudioSource playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<AudioSource>();
            playerAudio.mute = muteFX;
            if(index != 2 || currentFX != 2 || (currentFX == 2 && !playerAudio.isPlaying))
            {
                playerAudio.volume = PlayerFXVolumeScale[index] * FXVolume;
                playerAudio.clip = PlayerFX[index];
                playerAudio.Play();
            }
            

            currentFX = index;
        }
    }
}
