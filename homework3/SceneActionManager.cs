using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneActionManager : SSActionManager,ISSActionCallback {

	public void MoveBoat(BoatController boat){
		CCMoveToActions action = CCMoveToActions.GetSSAction(boat.getDestination(),20);
//		Debug.Log (boat.getDestination ());
		this.RunAction (boat.getGameobj (), action, this);
	}

	public void MoveCharacter(CharacterController _characterCtrl,Vector3 des) {

		Vector3 pos = _characterCtrl.getPos();

//		Debug.Log (pos);
		Vector3 mid = pos;
		if (des.y > pos.y)	
			mid.y = des.y;
		else 
			mid.x = des.x;
//		Debug.Log(mid);
//		Debug.Log(des);
		SSAction action1 = CCMoveToActions.GetSSAction(mid, 40);
		SSAction action2 = CCMoveToActions.GetSSAction (des, 40);
		SSAction action = CCSequenceAction.GetSSAction (1, 0, new List<SSAction>{ action1, action2 });
		this.RunAction (_characterCtrl.getGameobj (), action, this);
//		this.RunAction(_characterCtrl.getGameobj(),action1,this);
//		this.RunAction (_characterCtrl.getGameobj (), action2, this);
	}
		

	public new void SSActionEvent(SSAction source){}
}