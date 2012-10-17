using UnityEngine;
using System.Collections;

/**
 * Provides functions to control playback of audio: sounds, music etc. 
 * @author	Robert Fell
 */
public interface IAudioManager
{
	/**
	 * If true all audio playback is silenced.
	 */
	bool IsMute ();
	/**
	 * Begin playback of any specified sound.  Optional parameters allow further control.
	 * @param	id	The unique id of the audio media asset.  Can be the className of a loaded asset library.
	 * @param	?audioChannelType	Sounds can be assigned specific channels to allow transformation of groups of related sounds.
	 * @param	?loops	How many times the specified sound should repeat.  Set to -1 for continual loop.
	 * @param	?startTime	Time displacement (ms) from the start of the sound file.
	 * @param	?volume	Adjusts this sound's amplitude relative to the audioChannel.  0...1: 0 is silent, 1 is full.
	 * @param	?pan	Adjusts this sound's stereo effect relative to the audioChannel.  -1...1: -1 is left channel only, 0 is central, 1 is right channel only.
	 * @param	?isIgnoredIfPlaying	If true and this sound is already playing in the specified channel the start request will be skipped.  If false there is a potential for the same sound to play over the top of itself.
	 * @param	?onCompleteCallback	Callback method to execute on sound complete.
	 */
	void Start (string id,
				EAudioChannel audioChannelType,
				int loops = 1,
				int startTime = 0,
				float volume = 1,
				float pan = 0,
				bool isIgnoredIfPlaying = false,
				GameObject onCompleteCallBackObj = null,
				string onCompleteCallBackFuncName = "" );
	/**
	 * End playback of any specified sound.  To stop all sounds on all channels, leave all parameters blank.
	 * @param	?id	The unique id of the audio media asset intended to be stopped.  If null will stop all sounds on the specific audioChannel.
	 * @param	?audioChannelType	If specified will only stop sounds assigned to this channel.
	 */
	void Stop ( string id, EAudioChannel audioChannelType );
	/**
	 * Adjusts the playback of any specified sound.  To adjust all sounds, ommit id and audioChannelType.
	 * @param	?id	The unique id of the audio media asset intended to be transformed.  If null will transform all sounds on the specific audioChannel.
	 * @param	?audioChannelType	If specified will only transform sounds assigned to this channel.
	 * @param	?volume	Adjusts this sound's amplitude relative to the audioChannel.  0...1: 0 is silent, 1 is full.
	 * @param	?pan	Adjusts this sound's stereo effect relative to the audioChannel.  -1...1: -1 is left channel only, 0 is central, 1 is right channel only.
	 * @param	?asRelative	If true will adjust sounds relative to their original transformation.  If false will set them as absolute values.
	 */
	void adjust ( string id, EAudioChannel audioChannelType, float volume = 1, float pan = 0, bool asRelative = false );
	/**
	 * Discover if a specified sound is playing.
	 * @param	?id	The unique id of the audio media asset under investigation.  If null will search entire audioChannel for activity.
	 * @param	?audioChannelType	If specified will only investigate the specified channel.  If ommitted will investigate all channels.
	 * @return	Returns true if a match is found, otherwise false.
	 */
	bool isPlaying ( string id, EAudioChannel audioChannelType );
}