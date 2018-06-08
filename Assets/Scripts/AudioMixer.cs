using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioMixer : MonoBehaviour 
{
	private const float MIN = -1.0f;
	private const float MAX = 1.0f;

	[SerializeField]private AudioClip _firstAudio = null;
	[SerializeField]private AudioClip _secondAudio = null;

	// Use this for initialization
	void Start () 
	{
		MixAudio ();
	}
	
	private void MixAudio()
	{
		float[] firstAudioInFloatFormat = new float[_firstAudio.samples * _firstAudio.channels];
		_firstAudio.GetData(firstAudioInFloatFormat, 0);

		float[] secondAudioInFloatFormat = new float[_secondAudio.samples*_secondAudio.channels];
		_secondAudio.GetData(secondAudioInFloatFormat, 0);

		float[] mixedFloatArray = MixAndClampFloatBuffers(firstAudioInFloatFormat, secondAudioInFloatFormat);

		AudioClip mixedClip = AudioClip.Create("Combine", mixedFloatArray.Length, _firstAudio.channels, _secondAudio.frequency,false);

		mixedClip.SetData(mixedFloatArray, 0);

		SavWav.Save ("MixedClip", mixedClip);
	}

	private float ClampToValidRange(float value)
	{
		return (value < MIN) ? MIN : (value > MAX) ? MAX : value;
	}

	private float[] MixAndClampFloatBuffers(float[] bufferA, float[] bufferB)
	{
		int maxLength = Math.Min(bufferA.Length, bufferB.Length);
		float[] mixedFloatArray = new float[maxLength];

		for (int i = 0; i < maxLength; i++)
		{
			mixedFloatArray[i] = ClampToValidRange((bufferA[i] + bufferB[i])/2);
		}
		return mixedFloatArray;
	}
}
