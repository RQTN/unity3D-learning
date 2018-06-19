using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {

    public GameObject water;
    public BankController rBank;
    public BankController lBank;
    public BoatController boat;
    public static IState state = new IState(0, 0, 3, 3, false, null);
    public static IState endState = new IState(3, 3, 0, 0, true, null);
    private CharacterController[] characters = new CharacterController[6];
    UserGUI userGUI;

    // the first scripts
    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        director.currentSceneController.loadResources();
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
    }

    // ISceneController
    public void loadResources()
    {
        // water = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Water"), new Vector3(0, -0.8f, 0), Quaternion.identity);
        // water.name = "water";

        rBank = new BankController(1);

        lBank = new BankController(-1);

        boat = new BoatController();

        for(int i = 0; i < 3; ++i)
        {
            CharacterController priest = new CharacterController(0, i);
            characters[i] = priest;
            rBank.getOnBank(priest);
        }

        for (int i = 0; i < 3; ++i)
        {
            CharacterController devil = new CharacterController(1, i);
            characters[i + 3] = devil;
            rBank.getOnBank(devil);
        }


    }

    // IUserAction
    public void moveBoat()
    {
        if (boat.isEmpty()) return;
        boat.move();
        userGUI.GameStatus = checkGameOver();
        Debug.Log(userGUI.GameStatus);
    }


    public void clickCharacter(CharacterController character)
    {
        // if the character we click is aleardy on boat
        if(character._isOnBoat())
        {
            BankController bank;
            if(boat.boatStatus == 0)    // if the boat is near the right bank
            {
                bank = rBank;           
            } else                      // if the boat is near the left bank
            {
                bank = lBank;
            }

            // then we get off boat
            boat.getOffBoat(character.getName());      // remove the character from boat
            character.move(bank.getEmptyPos());        // move the character to the bank
            character.getOnBank(bank);                 // after get on bank, set the character's bank to OnBank
            bank.getOnBank(character);                 // add the character to the bank
        }
        else    
        {
            BankController bank = character.getBankController();    

            // return -1 means boat is full
            if (boat.getEmptySeatIndex() == -1)
            {
                return;
            }

            // the character stand place and the boat should be on the same side
            if((bank.bankFlag == 1 && boat.boatStatus == 1) || (bank.bankFlag == -1 && boat.boatStatus == 0))
            {
                return;
            }

            bank.getOffBank(character.getName());       // remove the character from bank
            character.move(boat.getEmptySeatPos());     // move the character to the boat
            character.getOnBoat(boat);                  // after get on boat, set the character's bank to null
            boat.getOnBoat(character);                  // add the character to the boat

        }
        userGUI.GameStatus = checkGameOver();
        Debug.Log(userGUI.GameStatus);

    }

    private int checkGameOver()
    {
        int rBankPriest = 0;
        int rBankDevil = 0;
        int lBankPriest = 0;
        int lBankDevil = 0;

        int[] rBankState = rBank.getBankState();
        rBankPriest += rBankState[0];
        rBankDevil += rBankState[1];

        int[] lBankState = lBank.getBankState();
        lBankPriest += lBankState[0];
        lBankDevil += lBankState[1];

        
        if(lBankDevil + lBankPriest == 6)
        {
            return 2;               // return 2 -> Win
        }

        int[] boatCount = boat.getCharacterNum();
        if(boat.boatStatus == 1)
        {
            lBankPriest += boatCount[0];
            lBankDevil += boatCount[1];
        } else
        {
            rBankPriest += boatCount[0];
            rBankDevil += boatCount[1];
        }

        Debug.Log("lBankPriest: " + lBankPriest + ", lBankDevil: " + lBankDevil
            + ", rBankPriest: " + rBankPriest + ", rBankDevil: " + rBankDevil);

        if(rBankPriest < rBankDevil && rBankPriest > 0)        // lose
        {
            return 1;
        }

        if(lBankPriest < lBankDevil && lBankPriest > 0)         // lose
        {
            return 1;
        }

        return 0;                   // not finish
    }

    public void restart()
    {
        boat.reset();
        rBank.reset();
        lBank.reset();
        for(int i = 0; i < characters.Length; ++i)
        {
            characters[i].reset();
        }
    }

}



