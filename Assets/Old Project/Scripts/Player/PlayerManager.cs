using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    public static PlayerManager Instance { get { return _instance; } }
    public Player player;

    private void Awake() {
  
        if( _instance != null )
            Destroy(_instance.gameObject);
        else
            _instance = this;


    }
}
