using UnityEngine;

public class EventDoor : Door
{
    [SerializeField]EventObject[] _EventObjects;

    new void Open()
    {
        base.Open();
        foreach(EventObject obj in _EventObjects)
        {
            obj.Interact();
        }
    }

    public override void MoveObject()
    {
        Open();
    }
}
