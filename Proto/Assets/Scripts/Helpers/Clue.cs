public class Clue
{
    public enum ClueStrengthType
    {
        WEAKHAT,
        WEAKFACIAL,
        WEAKACCESSORIES,
        MEDHAT,
        MEDFACIAL,
        MEDACCESSORIES,
        STRONGHAT,
        STRONGFACIAL,
        STRONGACCESSORIES
    }

    int _ClueGiverID;
    ClueStrengthType _ClueStrength;
    string _Clue;

    public int ClueGiverID
    {
        get { return _ClueGiverID; }
        set { _ClueGiverID = value; }
    }

    public ClueStrengthType ClueStrength
    {
        get { return _ClueStrength; }
        set { _ClueStrength = value; }
    }

    public string ClueString
    {
        get { return _Clue; }
        set { _Clue = value; }
    }
}
