using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioClipFromPath : MonoBehaviour 
{
	private const string FILE_URL = "file:///";

	[SerializeField] private string _path = string.Empty;
	[SerializeField] private AudioSource _source = null;

	[SerializeField] private string _url = string.Empty;

	#if UNITY_EDITOR
	private void OnValidate()
	{
		_url = FILE_URL + _path;
	}
	#endif

    void Start()
	{
        _source = GetComponent<AudioSource>();
        
		StartCoroutine (GetAudioSourseWithDelay ());
    }

	private IEnumerator GetAudioSourseWithDelay()
	{
		Debug.Log ("1");
		using (var www = new WWW(_url))
		{
			yield return www;
			Debug.Log ("2");
			_source.clip = www.GetAudioClip();
		}

		yield return new WaitUntil (()=>_source.clip != null && _source.clip.isReadyToPlay);
		Debug.Log ("3");
		_source.Play();
	}
}
