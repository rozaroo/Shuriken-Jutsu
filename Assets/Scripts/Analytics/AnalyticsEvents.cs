

public class ShurikenSelectedEvent : Unity.Services.Analytics.Event 
{
    public ShurikenSelectedEvent() : base("shuriken_selected") 
    {

    }
    public int Index { set { SetParameter("index", value); } }
    public string usuario_identified { set { SetParameter("usuario_identified", value); } }
    public int Count { set { SetParameter("Count", value); } }
}


public class gameFinishedEvent : Unity.Services.Analytics.Event
{
    public gameFinishedEvent() : base("gameFinished")
    {

    }
    public float PlayTime { set { SetParameter("PlayTime", value); } }
    public string usuario_identified { set { SetParameter("usuario_identified", value); } }
}

