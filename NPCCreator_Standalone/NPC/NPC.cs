public class NPC
{
    public Hat Hat { get; }
    public Top Top { get; }
    public Pants Pants { get; }
    public Hair Hair { get; }
    public FacialHair FacialHair { get; }
    public Smoker Smoker { get; }

    public NPC(Hat hat, Top top, Pants pant, Hair hair, FacialHair facialHair, Smoker smoker)
    {
        Hat = hat;
        Top = top;
        Pants = pant;
        Hair = hair;
        FacialHair = facialHair;
        Smoker = smoker;
    }
}