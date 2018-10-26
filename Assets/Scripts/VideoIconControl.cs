using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoIconControl : MonoBehaviour {
	public Text title;
	public Transform icon;
	public VideoSwapper main;
	public VideoNode node;

	CanvasGroup titleShader;
	float iconRadius;

	void Awake(){
		title=GetComponentInChildren<Text>();
		titleShader=title.GetComponent<CanvasGroup>();
		titleShader.alpha=0;
		icon=GetComponentInChildren<MeshFilter>().transform;
		iconRadius=5;
	}

	// void Start(){
	// 	transform.LookAt(Camera.main.transform);
	// }

	void Update () {
		// var tmp=transform.rotation;
		// transform.LookAt(Camera.main.transform);
		// transform.rotation=Quaternion.Lerp(tmp,transform.rotation,0.1f);

		if(this==main.rayPointer){
			titleShader.alpha+=0.05f;
			if(Input.GetKeyDown(KeyCode.KeypadEnter))
				main.StartCoroutine(main.Switch(node));
			iconRadius+=(7.5f-iconRadius)*0.05f;
			icon.localScale+=(Vector3.one*15-icon.localScale)*0.05f;
		}
		else{
			iconRadius=5;
			titleShader.alpha-=0.02f;
		}
		icon.localScale=Vector3.one*(iconRadius*2);
	}
}
