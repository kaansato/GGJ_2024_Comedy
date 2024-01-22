using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJFUK
{
    public class AudioManager : MonoBehaviour
    {
        AudioSource[] audioSources;

        public AudioClip gameAudio;

        public AudioClip applauseAudio;
        public AudioClip talkingAudio;
        public AudioClip laughAudio;
        public AudioClip tissAudio;
        public AudioClip dinkAudio;
        public AudioClip fallAudio;

        void Awake()
        {
            audioSources = new AudioSource[6];

            // 0 : BGM
            // 1-6 : SFX

            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i] = AddAudioSource();
            }
        }

        AudioSource AddAudioSource()
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            return audioSource;
        }

        void Start()
        {
            PlayMusic(gameAudio);
        }

        public void PlayAudio(AudioClip audioClip, float volume = 1.0f)
        {
            if (audioClip)
            {
                for (int i = 1; i < audioSources.Length; i++)
                {
                    if (!audioSources[i].isPlaying)
                    {
                        audioSources[i].Stop();
                        audioSources[i].volume = volume;
                        audioSources[i].PlayOneShot(audioClip);

                        break;
                    }
                }
            }
        }

        public void PlayMusic(AudioClip audioClip, float volume = 1.0f)
        {
            if (audioClip)
            {
                audioSources[0].Stop();
                audioSources[0].loop = true;
                audioSources[0].clip = audioClip;
                audioSources[0].volume = volume;
                audioSources[0].Play();
            }
        }

        public void StopAll()
        {
            for (int i = 1; i < audioSources.Length; i++)
            {
                if (audioSources[i].isPlaying)
                {
                    audioSources[i].Stop();
                }
            }
        }
    }
}
