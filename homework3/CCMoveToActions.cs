using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CCMoveToActions : SSAction {

	public Vector3 target;
	public float speed;

	public static CCMoveToActions GetSSAction(Vector3 target,float speed){
		CCMoveToActions action = ScriptableObject.CreateInstance<CCMoveToActions> ();
		action.target = target;
		action.speed = speed;
		return action;
	}

	public override void Start (){}


	public override void Update(){
		this.transform.position = Vector3.MoveTowards (this.transform.position, target, speed * Time.deltaTime);
		if (this.transform.position == target) {
			this.destory = true;
			this.callback.SSActionEvent (this);
		}
	}


}