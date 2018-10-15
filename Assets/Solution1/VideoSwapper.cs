using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoSwapper : MonoBehaviour {

	public VideoPlayer[] vps;
	public RenderTexture[] vts;
	public Material output;

	int ptr=0;

	void Start(){
		SwitchVideo(0);
		for(int i=0;i<vps.Length;i++){
			vps[i].Prepare();
			vps[i].targetTexture=vts[i];
		}
		// for(int i=0;i<vps.Length;i++)vps[i].targetTexture=vts[i];
	}

	void SwitchVideo (int to) {
		if(vps[ptr].isPlaying){
			vps[to].frame=vps[ptr].frame;
			vps[to].Play();
			vps[ptr].Pause();
		}else{

		}
		output.SetTexture("_MainTex",vts[to]);
		ptr=to;
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			if(vps[ptr].isPlaying)vps[ptr].Pause();
			else vps[ptr].Play();
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow))
			SwitchVideo((ptr+2)%3);
		if(Input.GetKeyDown(KeyCode.RightArrow))
			SwitchVideo((ptr+1)%3);
	}
}
