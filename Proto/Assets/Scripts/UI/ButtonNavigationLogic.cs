using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonNavigationLogic : MonoBehaviour,  IMoveHandler {
	[SerializeField] private Selectable _leftObject;
	[SerializeField] private Selectable _rightObject;
	[SerializeField] private Selectable _upObject;
	[SerializeField] private Selectable _downObject;

		//When the focus moves to another selectable object, Invoke this Method.
	public void OnMove (AxisEventData eventData) 
	{
		//Assigns the move direction and the raw input vector representing the direction from the event data.
		MoveDirection moveDir = eventData.moveDir;

		if (_downObject && moveDir == MoveDirection.Down)
			_downObject.Select ();
		else if (_upObject && moveDir == MoveDirection.Up)
			_upObject.Select ();
		else if (_rightObject && moveDir == MoveDirection.Right)
			_rightObject.Select ();
		else if (_leftObject && moveDir == MoveDirection.Left)
			_leftObject.Select ();
	}
}
