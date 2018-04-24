/** 
 * 这个文件是用来场景控制的，负责各个场景的切换， 
 * 虽然目前只有一个场景 
 */  
    
using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
  
public interface ISceneControl  {  
    void LoadResources();  
}  
  
public interface IActionManager {  
    void StartThrow(Queue<GameObject> diskQueue);  
    int getDiskNumber();  
    void setDiskNumber(int num);  
}  

public enum SSActionEventType:int { Started, Competeted }  
  
public interface ISSActionCallback {  
    void SSActionEvent(SSAction source,  
        SSActionEventType events = SSActionEventType.Competeted,  
        int intParam = 0,  
        string strParam = null,  
        Object objectParam = null);  
      
}  

public enum GameState { ROUND_START, ROUND_FINISH, RUNNING, PAUSE, START}  
public enum ActionMode { PHYSIC, KINEMATIC, NOTSET }  
  
public interface IUserAction {  
    void GameOver();  
    GameState getGameState();  
    void setGameState(GameState gs);  
    int GetScore();  
    void hit(Vector3 pos);  
    ActionMode getMode();  
    void setMode(ActionMode m);  
}  


public class Director : System.Object {  
  
    /** 
     * currentSceneControl标志目前正在使用的场景 
     */  
  
    public ISceneControl currentSceneControl { get; set; }  
  
    /** 
     * Director这个类是采用单例模式 
     */   
  
    private static Director director;  
  
    private Director()  
    {  
  
    }  
  
    public static Director getInstance()  
    {  
        if (director == null)  
        {  
            director = new Director();  
        }  
        return director;  
    }  
}  