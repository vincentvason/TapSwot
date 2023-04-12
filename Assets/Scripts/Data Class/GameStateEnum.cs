using System.Collections;
using System.Collections.Generic;

public enum GameStateEnum
{
    SETUP, // The initial game state once the game transitions from Lobby Scene to Main Game scene which consists of 10-minute introduction video. 
         

    ROUND_ONE, // The card deck is shuffled and player specific game cards are available in the Card Manager. The game state where the card decks are dislayed after the introduction video finishes. Main card deck of remaining cards and player specific card deck is visualized on the UI. The player takes turn to PICK AND REPLACE or PICK AND DISCARD a card from the remaining card deck pile. The turn manager uses player confirmation for turn switching.
               // ROUND 1 ends when remaining card deck count == 0

    ROUND_TWO, // The players are now left with 5 cards each (Total 20 cards in the game) and the remainaing card deck is discarded.             

    ROUND_TWO_END, // Each player ranks their personal cards according to their individual preference/ Add remove  any custom cards and cards for each player are now moved to a common voting deck. Players can also skip their turn.
               // ROUND 2 ends when all players have finalized their 5 final cards and have ranked each card.

    ROUND_THREE, // The cards will be displayed in vertical columns based on the rankings. Voting will start from the least important card onwards(Bottom-Up approach). Voting will conitnue on each card until 15 cards are voted out with full majority voting system.
                
               // ROUND 3 ends when Elimination card count == 15 indicating we have 5 final remaiaing cards   


    ROUND_FOUR, // The player voting is now complete and the players are now left with 5 final cards which are the final outcome and the players individually decide if that is the current approach. Players then provide any additional feedback or discuss anything related to the game.

                // ROUND 4 marks the end of the game session when players choose to quit the game.

                // Player is now returned to Main Menu.
}
