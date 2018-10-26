using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class VideoNode{
	public string name;
	public Vector3 position;
	public VideoClip video;
	public RenderTexture screen;
	public VideoPlayer player;
	public GameObject icon;

	#region interface
	#endregion

	public VideoNode(VideoNodeInput param, RenderTexture def_screen, VideoPlayer def_plr){
		#region Params
		name=param.name;
		position=param.position;
		video=param.video;
		#endregion

		#region Player init
		screen=def_screen;
		player=def_plr;
		player.clip=video;
		player.isLooping=true;
		player.skipOnDrop=true;
		// player.waitForFirstFrame=false;

		player.renderMode=VideoRenderMode.RenderTexture;
		player.targetTexture=screen;

		player.Prepare();
		player.Pause();
		#endregion

	}
}