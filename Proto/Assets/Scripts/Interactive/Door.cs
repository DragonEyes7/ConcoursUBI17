public class Door : Interactive
{
    private DoorRecorder _doorRecorder;

    private new void Start()
    {
        base.Start();
        _doorRecorder = GetComponent<DoorRecorder>();
        m_IsActivated = true;
    }

    public override void Interact()
    {
        if (!_doorRecorder) return;
        if (_doorRecorder.DoorStatus()) return;
        _doorRecorder.DoorInteraction(true);
    }


}
