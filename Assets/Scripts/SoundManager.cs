using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {


    public static SoundManager S_INSTANCE;
    public static AudioSource[] audioSources;


	// Use this for initialization
	void Start () {
        S_INSTANCE = this;
        audioSources = GetComponentsInChildren<AudioSource>();

	}
	
    public static void PlayAudio(AudioClip clip)
    {
        if (clip == null)
            return;

        AudioSource source = null;
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].clip == null)
            {
                source = audioSources[i];
                break;
            }
        }

        if(source != null)
        {
            source.clip = clip;
            source.Play();
            S_INSTANCE.StartCoroutine(SoundCallback(source)); ;
        }

    }

    public static IEnumerator SoundCallback(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        source.clip = null;
    }



	// Update is called once per frame
	void Update () {
		
	}
}
