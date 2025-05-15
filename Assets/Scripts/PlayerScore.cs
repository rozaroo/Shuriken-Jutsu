[System.Serializable]
public class PlayerScore
{
    public string playerName;
    public string score;
    public PlayerScore(string name, string score) 
    {
        this.playerName = name;
        this.score = score;
    }
}

