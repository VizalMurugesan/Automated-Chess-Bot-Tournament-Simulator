using UnityEngine;

public class Manager : MonoBehaviour
{
    
    public static Manager Instance { get; private set; }

    public ChessTileManager tileManager;

    public GameObject DebugPiece;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    private void Start()
    {
        tileManager.AssignSquaresToPieces();

        ChessPiece random1 = tileManager.BlackPieces[0].GetComponent<ChessPiece>() ;
        ChessPiece random2 = tileManager.BlackPieces[0].GetComponent<ChessPiece>();

        if(random1.gameObject==null || random2.gameObject == null)
        {
            Debug.Log("Random is null");
        }

        if(tileManager.gameObject == null)
        {
            Debug.Log("Tilemanager is null");
        }

        if (tileManager.ChessBoard == null)
        {
            Debug.Log("ChessBoard2D is null");
        }
        var moves = tileManager.ChessBoard.GetRookMovableSquares(DebugPiece.gameObject);

        foreach (var kvp in moves)
        {
            Debug.Log($"Square ({kvp.Key.x},{kvp.Key.y})  {kvp.Value}");
        }


    }
}
