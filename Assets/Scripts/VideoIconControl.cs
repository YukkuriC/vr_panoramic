using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoIconControl : MonoBehaviour {
	public Text title;
	public Image icon;
	public VideoSwapper main;
	public VideoNode node;

	CanvasGroup titleShader;
	CanvasGroup iconShader;

	void Awake(){
		title=GetComponentInChildren<Text>();
		titleShader=title.GetComponent<CanvasGroup>();
		titleShader.alpha=0;
		icon=GetComponentInChildren<Image>();
		iconShader=icon.GetComponent<CanvasGroup>();
		iconShader.alpha=0.5f;
	}

	// void Start(){
	// 	transform.LookAt(Camera.main.transform);
	// }

	void Update () {
		// var tmp=transform.rotation;
		// transform.LookAt(Camera.main.transform);
		// transform.rotation=Quaternion.Lerp(tmp,transform.rotation,0.1f);

		if(main.rayPointer==this){
			titleShader.alpha+=0.05f;
			iconShader.alpha+=0.05f;
			if(Input.GetKeyDown(KeyCode.KeypadEnter))
				main.StartCoroutine(main.Switch(node));
			icon.transform.localScale+=(Vector3.one*1.5f-icon.transform.localScale)*0.05f;
		}
		else{
			icon.transform.localScale=Vector3.one;
			titleShader.alpha-=0.02f;
			if(iconShader.alpha>0.5f)iconShader.alpha-=0.02f;
		}
	}
}
