using UnityEngine.Audio;
using UnityEngine;

public class Sound {

	public string name;

	public AudioClip clip;

	public float volume = .75f;

	public float volumeVariance = .1f;

	public float pitch = 1f;

	public float pitchVariance = .1f;

	public bool loop = false;

	public AudioMixerGroup mixerGroup;

	public AudioSource source;

	public Sound(string _name, AudioClip _clip, float _volume, bool _loop)
    {
		name = _name;
		clip = _clip;
		volume = _volume;
		loop = _loop;
    }

}
