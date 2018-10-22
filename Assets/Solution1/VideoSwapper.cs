using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoSwapper : MonoBehaviour {

	public VideoPlayer[] vps;
	public RenderTexture[] vts;
	public Material output;
	public CanvasGroup shader;

	int ptr=0;
	bool isSwapping=false;

	const int split=10;
	const float lag=0.01f;

	IEnumerator FadeIn(int to){
		for(int i=split;i>=0;i--){
			vps[to].SetDirectAudioVolume(0,1-(float)i/split);
			shader.alpha=(float)i/split;
			yield return new WaitForSeconds(lag);
		}
	}

	IEnumerator Switch(int from,int to){
		Debug.Log(from);
		isSwapping=true;
		vps[to].SetDirectAudioVolume(0,0);
		vps[to].frame=vps[ptr].frame;
		vps[to].Play();
		Debug.Log(to);
		for(int i=0;i<=split;i++){
			vps[from].SetDirectAudioVolume(0,1-(float)i/split);
			shader.alpha=(float)i/split;
			yield return new WaitForSeconds(lag);
		}
		vps[ptr].Pause();
		output.SetTexture("_MainTex",vts[to]);
		ptr=to;
		yield return FadeIn(to);
		isSwapping=false;
	}

	void Start(){
		for(int i=0;i<vps.Length;i++){
			vps[i].Prepare();
			vps[i].targetTexture=vts[i];
		}
		ptr=0;
		isSwapping=false;
		vps[0].Play();
		output.SetTexture("_MainTex",vts[0]);
		StartCoroutine(FadeIn(0));
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			if(vps[ptr].isPlaying)vps[ptr].Pause();
			else vps[ptr].Play();
		}
		if(!isSwapping){
			if(Input.GetKeyDown(KeyCode.LeftArrow))
				StartCoroutine(Switch(ptr,(ptr+vps.Length-1)%vps.Length));
			if(Input.GetKeyDown(KeyCode.RightArrow))
				StartCoroutine(Switch(ptr,(ptr+1)%vps.Length));
		}
	}
}
