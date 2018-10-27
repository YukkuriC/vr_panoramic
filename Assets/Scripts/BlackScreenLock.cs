using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreenLock : MonoBehaviour {
	Transform target;
	void Start(){
		target=Camera.main.transform;
	}
	void Update () {
		transform.position=target.position+target.forward;
		transform.LookAt(target.position);
	}
}
