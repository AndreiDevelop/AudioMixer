using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.IO;

public class AudioMixer : MonoBehaviour 
{
	private const float MIN_FLOAT = -1.0f;
	private const float MAX_FLOAT = 1.0f;

	[SerializeField]private AudioClip _firstAudio = null;
	[SerializeField]private AudioClip _secondAudio = null;

	[Space]
	[SerializeField]private string _pathToFirstAudio = null;
	[SerializeField]private string _pathToSecondAudio = null;

	// Use this for initialization
	void Start () 
	{
		MixAudioFloatExample ();
	}

	#if UNITY_EDITOR
	private void OnValidate()
	{
		_pathToFirstAudio = AssetDatabase.GetAssetPath (_firstAudio);
		_pathToSecondAudio = AssetDatabase.GetAssetPath (_secondAudio);
	}
	#endif

	#region File in float format

	private void MixAudioFloatExample()
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

	private float ClampToValidRangeFloat(float value)
	{
		return (value < MIN_FLOAT) ? MIN_FLOAT : (value > MAX_FLOAT) ? MAX_FLOAT : value;
	}

	private float[] MixAndClampFloatBuffers(float[] bufferA, float[] bufferB)
	{
		int length = Math.Min(bufferA.Length, bufferB.Length);

		float[] mixedFloatArray = new float[length];

		for (int i = 0; i < length; i++)
		{
			mixedFloatArray[i] = ClampToValidRangeFloat((bufferA[i] + bufferB[i])/2);
		}
		return mixedFloatArray;
	}

	#endregion
}
