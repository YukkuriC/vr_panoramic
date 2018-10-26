using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class VideoNodeInput{
	public Vector3 position;
	public VideoClip video;
	public string name;
}

[System.Serializable]
public class VideoPrefabInput{
	public VideoPlayer defaultPlayer;
	public RenderTexture defaultScreen;
	public GameObject defaultIcon;
}

public class VideoSwapper : MonoBehaviour {
	public VideoPrefabInput prefabs;
	public Material output;
	public CanvasGroup shader;
	public VideoNodeInput[] videos;

	VideoNode[] nodes;
	VideoNode activeNode;
	GameObject[] activeIcons;
	bool isSwapping=false;

	const int split=10;
	const float lag=0.01f;
	const float iconDist=150;
	const int raycastSkip=3;

	#region smooth swapper
	IEnumerator FadeIn(VideoNode to){
		output.SetTexture("_MainTex",to.screen);
		UpdateIcons();
		for(int i=split;i>=0;i--){
			to.player.SetDirectAudioVolume(0,1-(float)i/split);
			shader.alpha=(float)i/split;
			yield return new WaitForSeconds(lag);
		}
		isSwapping=false;
	}

	public IEnumerator Switch(VideoNode to){
		if(isSwapping)yield break;
		isSwapping=true;
		to.player.SetDirectAudioVolume(0,0);
		to.player.frame=activeNode.player.frame;
		to.player.Play();
		for(int i=0;i<=split;i++){
			activeNode.player.SetDirectAudioVolume(0,1-(float)i/split);
			shader.alpha=(float)i/split;
			yield return new WaitForSeconds(lag);
		}
		activeNode.player.Pause();
		activeNode=to;
		yield return FadeIn(to);
	}
	#endregion

	#region icon switch
	void UpdateIcons(){
		int ico_id=0;
		foreach(var node in nodes)if(node!=activeNode){
			GameObject new_icon=activeIcons[ico_id++];
			new_icon.transform.position=
				(node.position-activeNode.position).normalized*iconDist;
			new_icon.transform.LookAt(Camera.main.transform);

			//connections
			var ico=new_icon.GetComponent<VideoIconControl>();
			ico.title.text=node.name;
			ico.main=this;
			ico.node=node;
		}
	}
	#endregion

	void Awake(){
		//video
		nodes=new VideoNode[videos.Length];
		for(int i=0;i<nodes.Length;i++)
			nodes[i]=new VideoNode(
				videos[i],
				Instantiate(prefabs.defaultScreen),
				Instantiate(prefabs.defaultPlayer)
				);

		//icons
		activeIcons=new GameObject[videos.Length-1];
		for(int i=0;i<videos.Length-1;i++){
			activeIcons[i]=Instantiate(prefabs.defaultIcon);
		}

		//first video
		activeNode=nodes[0];
		activeNode.player.Play();
		isSwapping=true;
		StartCoroutine(FadeIn(activeNode));
	}

	public VideoIconControl rayPointer;
	int raycastCounter=0;

	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			if(activeNode.player.isPlaying)activeNode.player.Pause();
			else activeNode.player.Play();
		}

		raycastCounter++;
		if(raycastCounter>=raycastSkip){
			raycastCounter-=raycastSkip;
			
			Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width * 0.5f,Screen.height * 0.5f));
			RaycastHit hit;
			rayPointer=null;
			if (Physics.Raycast(ray, out hit, iconDist+10)){
				rayPointer=hit.collider.GetComponent<VideoIconControl>();
				Debug.Log(hit.collider);
			}
			// PointerEventData pointerData = new PointerEventData (EventSystem.current)
			// {
			// 	pointerId = -1,
			// };
			// pointerData.position = new Vector2(Screen.width * 0.5f,Screen.height * 0.5f);
			// // Input.mousePosition;

			// List<RaycastResult> results = new List<RaycastResult>();
			// EventSystem.current.RaycastAll(pointerData, results);
			// rayPointer=null;
			// if(results.Count>0)
			// 	rayPointer=
			// 		results[0].gameObject.transform.parent.gameObject.GetComponent<VideoIconControl>();
		}
	}
}
