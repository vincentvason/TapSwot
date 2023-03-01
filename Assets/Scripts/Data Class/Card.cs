// Card class for card content
[System.Serializable]
public class Card 
{
    public int cardId;
    public string cardTitle;
    public string cardDescription;
    public string cardBrief;
    public string cardCategory;
    public int cardRank;

    public Card()
    {

    }
    
    public Card(int id, string title, string description,string brief, string category, int rank)
    {
        cardId =id;
        cardTitle = title;
        cardDescription = description;
        cardBrief = brief;
        cardCategory = category;
        cardRank = rank;

    }
}
