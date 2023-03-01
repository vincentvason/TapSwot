using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    private void Awake()
    {
        //cardList.Add(new Card(0, "better reflects the world we live in", "people live their lives in a triple access system, not (only) a transport system", "how people fulfil their access needs and desires is influenced by the supply of access opportunity  across transport, land-use and telecommunications systems. a planning approach that recognises this is better able to understand and shape overall access and the place of mobility within this.", "s", 0));

        //cardList.Add(new Card(1, "hard to model", "representing a more complex system is too resource and time hungry", "representing supply and demand for physical mobility, spatial proximity and digital connectivity would need further model development, and data may not exist to support this. to then model multiple ‘what-if’ futures would need more resources and time that could slow the planning process.", "w", 0));

        //cardList.Add(new Card(2, "covid-19 exposure", "greater familiarity with triple access and uncertainty opens people’s minds", "the covid-19 pandemic was a global shock that demonstrated how things can change in unexpected, uncertain ways. many people shifted significantly from a reliance on physical mobility to a greater reliance on digital connectivity. professionals and the public alike are more likely to ‘get it’ now.", "o", 0));


    }
}
