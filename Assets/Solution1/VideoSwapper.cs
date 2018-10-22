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

	const int split=10;
	const float lag=0.01f;

	IEnumerator FadeIn(){
		for(int i=split;i>=0;i--){
			shader.alpha=(float)i/split;
			yield  return new  WaitForSeconds(lag);
		}
	}

	IEnumerator FadeOut(int to){
		for(int i=0;i<=split;i++){
			shader.alpha=(float)i/split;
			yield  return new  WaitForSeconds(lag);
		}
		output.SetTexture("_MainTex",vts[to]);
		yield  return FadeIn();
	}
	IEnumerator Switch(int from,int to){
		vps[to].volumn=0;
		vps[to].frame=vps[ptr].frame;
		vps[to].Play();
		for(int i=0;i<=split;i++){
			vps[from].volumn=shader.alpha=(float)i/split;
			yield  return new  WaitForSeconds(lag);
		}
		vps[ptr].Pause();
		output.SetTexture("_MainTex",vts[to]);
		for(int i=split;i>=0;i--){
			vps[to].volumn=shader.alpha=(float)i/split;
			yield  return new  WaitForSeconds(lag);
		}
	}

	void Start(){
		for(int i=0;i<vps.Length;i++){
			vps[i].Prepare();
			vps[i].targetTexture=vts[i];
		}
		ptr=0;
		vps[0].Play();
		StartCoroutine(FadeIn());
	}

	void SwitchVideo (int to) {
		StartCoroutine(FadeOut(to));
		// Debug.Log(to);
		if(vps[ptr].isPlaying){
			vps[to].frame=vps[ptr].frame;
			vps[ptr].Pause();
			vps[to].Play();
		}else{

		}
		// output.SetTexture("_MainTex",vts[to]);
		ptr=to;
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			if(vps[ptr].isPlaying)vps[ptr].Pause();
			else vps[ptr].Play();
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow))
			// SwitchVideo((ptr+2)%3);
			Switch(ptr,(ptr+vps.Length-1)%vps.Length);
		if(Input.GetKeyDown(KeyCode.RightArrow))
			// SwitchVideo((ptr+1)%3);
			Switch(ptr,(ptr+1)%vps.Length);
	}
}
