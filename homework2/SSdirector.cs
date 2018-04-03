using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// use to Load Game resources
public interface ISceneController
{
    void loadResources();
}

// what User can do
public interface IUserAction
{
    void moveBoat();
    void restart();
    void clickCharacter(CharacterController character);
}

// Director
public class SSDirector : System.Object
{
    private static SSDirector _instance;

    public ISceneController currentSceneController { get; set; }

    public static SSDirector getInstance()
    {
        if(_instance == null)
        {
            _instance = new SSDirector();
        }
        return _instance;
    }

}

// Moveable is a behaviour component use to make GameObject to move
// add this to those gameobject that need to move
public class Moveable: MonoBehaviour
{
    private float speed = 5;

    private int movingStatus;      // 0 -> not moving, 1 -> moving to middle pos(then move to dest), 2 -> moving to dest
    private Vector3 dest;
    private Vector3 middle;

    void Update()
    {
        if(movingStatus == 1)
        {
            Debug.Log("Moving Status: 1");
            this.transform.position = Vector3.MoveTowards(this.transform.position, middle, speed * Time.deltaTime);
            if(this.transform.position == middle)
            {
                Debug.Log("Turn 1 to 2");
                movingStatus = 2;   // now move at the middle, then move to the dest
            }
            else if (movingStatus == 2)
            {
                Debug.Log("Moving Status: 2");
                this.transform.position = Vector3.MoveTowards(this.transform.position, dest, speed * Time.deltaTime);
                if(this.transform.position == dest)
                {
                    Debug.Log("Moving over");
                    movingStatus = 0;
                }
            }
        }
    }

    // this function use to set the destination pos
    // before we add this component to gameobject, we first use this function to set
    public void setDestPos(Vector3 destPos)
    {
        
        dest = destPos;
        middle = destPos;

        // we need to judge if the moving need the middle state
        // if boat moving, it don't need
        // if priest or devil moving, it do need
        Debug.Log("destPos.y: " + destPos.y + " | " + "transform.position.y: " + transform.position.y);
        if (destPos.y == transform.position.y)   // it means boat moving
        {
            movingStatus = 2;
        } else if(destPos.y < transform.position.y)     // it means to get on boat(bank is higher than boat)
        {

            middle.y = transform.position.y;
        } else                                          //  it mean to get on bank
        {
            middle.x = transform.position.x;

        }
        movingStatus = 1;
    }

    public void reset()
    {
        movingStatus = 0;
    }
}


// Clickable is a component use to test click on a GameObject and arrange the click 
// to its corresponding UserAction.
// we use it to build a bridge between UserClick and UserAction
// add this to those gameobject that will be click
public class Clickable : MonoBehaviour
{
    IUserAction action;
    CharacterController character;

    void Start()
    {
        // first we need to get UserAction which make it more convient to link UserClick and UserAction
        action = SSDirector.getInstance().currentSceneController as IUserAction;
    }

    public void setCharacter(CharacterController _character)
    {
        character = _character;
    }

    // if click then arrange the click
    void OnMouseDown()
    {
        Debug.Log("Mouse Click!");
        if(gameObject.name == "boat")
        {
            Debug.Log("Boat Click!");
            action.moveBoat();
        } else
        {
            Debug.Log("Character Click!");
            action.clickCharacter(character);
        }
    }


}


public class CharacterController
{
    readonly GameObject character;
    readonly Moveable moveableObject;
    readonly Clickable clickableObject;
    private int characterType;      // 0: priest  1: devil

    private bool isOnBoat;          // Use to count the people on the boat

    BankController bank;

    public CharacterController(int cType, int ith)
    {
        if (cType == 0)
        {
            character = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Priest"), new Vector3(3.0f + 0.5f * ith, 0, 0), Quaternion.identity);
            character.name = "priest" + ith;

            characterType = 0;
        }
        else
        {
            character = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Devil"), new Vector3(4.5f + 0.5f * ith, 0, 0), Quaternion.identity);
            character.name = "devil" + ith;

            characterType = 1;
        }

        moveableObject = character.AddComponent(typeof(Moveable)) as Moveable;
        clickableObject = character.AddComponent(typeof(Clickable)) as Clickable;
        clickableObject.setCharacter(this);
        isOnBoat = false;
        bank = (SSDirector.getInstance().currentSceneController as FirstController).rBank;
    }

    public GameObject getGameObject()
    {
        return character;
    }

    public int getCharacterType()
    {
        return characterType;
    }

    public void move(Vector3 dest)
    {
        moveableObject.setDestPos(dest);
    }

    public string getName()
    {
        return character.name;
    }

    public void getOnBoat(BoatController boat)
    {
        bank = null;
        character.transform.parent = boat.getGameObject().transform;
        isOnBoat = true;
    }

    public void getOnBank(BankController bank_)
    {
        bank = bank_;
        character.transform.parent = null;
        isOnBoat = false;
    }

    public bool _isOnBoat()
    {
        return isOnBoat;
    }

    public BankController getBankController()
    {
        return bank;
    }

    public void setPos(Vector3 pos)
    {
        character.transform.position = pos;
    }

    public void reset()
    {
        moveableObject.reset();
        bank = (SSDirector.getInstance().currentSceneController as FirstController).rBank;
        getOnBank(bank);
        setPos(bank.getEmptyPos());
        bank.getOnBank(this);

    }

}

public class BankController
{
    readonly GameObject bank;
    readonly Vector3 lPos = new Vector3(-4.3f, -0.6f, 0);
    readonly Vector3 rPos = new Vector3(4.3f, -0.6f, 0);
    readonly Vector3[] positions;
    public int bankFlag;        // left bank: -1,  right bank: 1


    CharacterController[] characters;

    public BankController(int bankFlag_)
    {

        characters = new CharacterController[6];

        if (bankFlag_ == 1)
        {
            bank = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Bank"), rPos, Quaternion.identity);
            bank.name = "rBank";
            bankFlag = 1;
        }
        else
        {
            bank = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Bank"), lPos, Quaternion.identity);
            bank.name = "lBank";
            bankFlag = -1;
        }

        positions = new Vector3[] {bankFlag * new Vector3(3.0f, 0, 0), bankFlag * new Vector3(3.5f, 0, 0), bankFlag * new Vector3(4.0f, 0, 0),
            bankFlag * new Vector3(4.5f, 0, 0), bankFlag * new Vector3(5.0f, 0, 0), bankFlag * new Vector3(5.5f, 0, 0)};

    }

    public int getEmptyIndex()
    {
        for (int i = 0; i < characters.Length; ++i)
        {
            if (characters[i] == null)
            {
                return i;
            }
        }
        return -1;      // I think is almost imposible to return -1
    }


    public Vector3 getEmptyPos()
    {
        int emptyIndex = getEmptyIndex();
        return positions[emptyIndex];
    }


    public void getOnBank(CharacterController character)
    {
        int index = getEmptyIndex();
        characters[index] = character;
    }

    public CharacterController getOffBank(string passengerName)
    {
        for (int i = 0; i < characters.Length; ++i)
        {
            if (characters[i] != null && characters[i].getName() == passengerName)
            {
                CharacterController character = characters[i];
                characters[i] = null;
                return character;
            }
        }

        return null;
    }

    public int[] getBankState()
    {
        int[] ret = { 0, 0 };
        for (int i = 0; i < characters.Length; ++i)
        {
            if (characters[i] == null)
            {
                continue;
            }
            if (characters[i].getCharacterType() == 0)
            {
                ret[0]++;
            }
            else
            {
                ret[1]++;
            }
        }

        return ret;
    }

    public void reset()
    {
        characters = new CharacterController[6];
    }

}

public class BoatController
{
    readonly GameObject boat;
    readonly Moveable moveableObject;
    readonly Vector3 rPos = new Vector3(1.5f, -0.4f, 0);
    readonly Vector3 lPos = new Vector3(-1.5f, -0.4f, 0);
    readonly Vector3[] rSeatsPos;
    readonly Vector3[] lSeatsPos;
    private CharacterController[] passengers = new CharacterController[2];

    public int boatStatus = 0;     // 0: near the rBank  1: near the lBank
    public int PeopleCount = 0;    // count the people on the boat
    
    public BoatController()
    {
        rSeatsPos = new Vector3[2] { new Vector3(1, 0, 0), new Vector3(2, 0, 0) };
        lSeatsPos = new Vector3[2] { new Vector3(-2, 0, 0), new Vector3(-1, 0, 0) };

        boat = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Boat"), new Vector3(1.5f, -0.4f, 0), Quaternion.identity);
        boat.name = "boat";

        boat.AddComponent(typeof(Clickable));
        moveableObject = boat.AddComponent(typeof(Moveable)) as Moveable;
    }

    public GameObject getGameObject()
    {
        return boat;
    }

    public void move()
    {
        if(boatStatus == 0)
        {
            Debug.Log("Move from r to l");
            moveableObject.setDestPos(lPos);
            boatStatus = 1;
        } else
        {
            Debug.Log("Move from l to r");
            moveableObject.setDestPos(rPos);
            boatStatus = 0;
        }
    }

    public bool isEmpty()
    {
        return PeopleCount == 0;
    }

    public bool isFull()
    {
        return PeopleCount == 2;
    }

    public int getEmptySeatIndex()
    {
        for(int i = 0; i < passengers.Length; ++i)
        {
            if(passengers[i] == null)
            {
                return i;
            }
        }
        return -1;      // return -1 means no empty seat
    }

    public Vector3 getEmptySeatPos()
    {
        int emptySeatIndex = getEmptySeatIndex();
        if(emptySeatIndex != -1)
        {
            if(boatStatus == 0)
            {
                return rSeatsPos[emptySeatIndex];
            }
            else
            {
                return lSeatsPos[emptySeatIndex];
            }
        }

        return Vector3.zero;
    }

    public void getOnBoat(CharacterController character)
    {
        int index = getEmptySeatIndex();
        if (index != -1)
        {
            passengers[index] = character;
            ++PeopleCount;
        }
    }

    public CharacterController getOffBoat(string passengerName)
    {
        for(int i = 0; i < passengers.Length; ++i)
        {
            if(passengers[i] != null && passengers[i].getName() == passengerName)
            {
                CharacterController character = passengers[i];
                passengers[i] = null;
                --PeopleCount;
                return character;
            }
        }

        return null;
    }

    public int[] getCharacterNum()
    {
        int[] count = { 0, 0 };
        for(int i = 0; i < passengers.Length; ++i)
        {
            if(passengers[i] == null)
            {
                continue;
            }
            if(passengers[i].getCharacterType() == 0)
            {
                count[0]++;
            } else
            {
                count[1]++;
            }
        }
        return count;
    }

    public void reset()
    {
        moveableObject.reset();
        if(boatStatus == 1)
        {
            move();
        }
        passengers = new CharacterController[2];
    }
}
