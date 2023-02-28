[System.Serializable]
public class Player
{
    public string playerName;
    public int playerID;

    public Player (string playerName, int playerID)
    {
        this.playerName = playerName;
        this.playerID = playerID;
    }
}
