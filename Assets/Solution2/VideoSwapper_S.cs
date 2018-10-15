using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoSwapper_S : MonoBehaviour {

	public VideoPlayer vp;
	public uint nSplit;

	public ulong[] fB;
	public ulong[] fE;

	public ulong fStart{
		get{
			return fB[ptr];
		}
	}
	public ulong fEnd{
		get{
			return fE[ptr];
		}
	}

	long ptr=0;

	void Start(){
		vp.Prepare();
		fB=new ulong[nSplit];
		fE=new ulong[nSplit];
		ulong step=(ulong)(vp.frameCount/nSplit);
		for(uint i=0;i<nSplit;i++){
			fB[i]=step*i;
			fE[i]=step*(i+1);
		}
		fE[nSplit-1]=vp.frameCount;
	}

	void SwitchVideo (long to) {
		Debug.Log(vp.frame);
		vp.Pause();
		vp.frame=vp.frame+(long)(fB[to]-fStart);
		vp.frame=2000;
		vp.Play();
		Debug.Log(vp.frame);
		ptr=to;
	}
	
	void Update () {
		#region video border
		ulong f=(ulong)vp.frame;
		if(vp.isPlaying && (f>=fEnd || f<fStart)){
			vp.frame=(long)fStart;
		}
		#endregion
		#region key control
		if(Input.GetKeyDown(KeyCode.Space)){
			if(vp.isPlaying)vp.Pause();
			else vp.Play();
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow))
			SwitchVideo((ptr+nSplit-1)%nSplit);
		if(Input.GetKeyDown(KeyCode.RightArrow))
			SwitchVideo((ptr+1)%nSplit);
		#endregion
	}
}
