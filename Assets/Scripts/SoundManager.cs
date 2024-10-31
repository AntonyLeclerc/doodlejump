using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType{
    // Bgm,
    Platform,
    BreakingPlatform,
    Spring,
    Fall,
    Shooting
}

public enum AudioSourceType{
    // Bgm,
    Player,
    Game
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager Instance;

    public float volume = 1f;

    // public AudioSource bgm;
    public AudioSource playerSource;
    public AudioSource gameSource;

    [System.Serializable]
    public struct AudioData{
        public AudioClip clip;
        public AudioType type;
    }

    public AudioData[] audioData;

    void Awake(){
        Instance = this;
    }

    void Start(){
        // bgm.volume = volume;
        playerSource.volume = volume;
        gameSource.volume = volume;
    }

    public void PlaySound(AudioType type, AudioSourceType sourceType){
        AudioClip clip = getClip(type);

        switch (sourceType){
            //case AudioSourceType.Bgm:
            //    bgm.PlayOneShot(clip);
            //    break;
            case AudioSourceType.Player:
                if (type == AudioType.Shooting){
                    playerSource.clip = clip;
                    playerSource.Play();
                }
                else{
                    playerSource.PlayOneShot(clip);
                }
                break;
            case AudioSourceType.Game:
                gameSource.PlayOneShot(clip);
                break;
            default:
                Debug.LogError("AudioManager: AudioSourceType " + sourceType + " not found");
                break;
        }
    }

    AudioClip getClip(AudioType type){
        foreach (AudioData data in audioData){
            if (data.type == type){
                return data.clip;
            }
        }
        Debug.LogError("AudioManager: No clip found for type " + type);
        return null;
    }
}
