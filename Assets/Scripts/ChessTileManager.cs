//(RangerD, 2025)
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChessTileManager : MonoBehaviour
{
    [Header("Board Settings")]
    public int BoardWidth = 8;
    public int BoardHeight = 8;

    [Header("Tilemaps")]
    public Tilemap WhiteTilemap;
    public Tilemap BlackTilemap;

    [Header("Tiles")]
    public TileBase WhiteTile;
    public TileBase BlackTile;

    public ChessBoard2D ChessBoard { get; private set; }

    [Header("Optional Origin")]
    public Transform BoardOrigin;

    [Header("Chess Pieces")]
    public GameObject[] WhitePieces;
    public GameObject[] BlackPieces;

    private void Awake()
    {
        InitializeBoard();

    }

    public void InitializeBoard()
    {
        if (WhiteTilemap == null || BlackTilemap == null)
        {
            Debug.LogError("Both WhiteTilemap and BlackTilemap must be assigned!");
            return;
        }

        ChessBoard = new ChessBoard2D(BoardWidth, BoardHeight);

        for (int x = 0; x < BoardWidth; x++)
        {
            for (int y = 0; y < BoardHeight; y++)
            {
                // Determine square color
                ColorEnum squareColor = ((x + y) % 2 == 0) ? ColorEnum.Light : ColorEnum.Dark;
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                // Place tile in the correct tilemap
                if (squareColor == ColorEnum.Light)
                {
                    WhiteTilemap.SetTile(cellPos, WhiteTile);
                }
                else
                {
                    BlackTilemap.SetTile(cellPos, BlackTile);
                }

                // Get the world position from one of the tilemaps
                Vector3 worldPos = (squareColor == ColorEnum.Light)
                    ? WhiteTilemap.GetCellCenterWorld(cellPos)
                    : BlackTilemap.GetCellCenterWorld(cellPos);

                // Create a new chess square object
                ChessBoardSquare square = new ChessBoardSquare(x, y, worldPos, cellPos, squareColor);
                ChessBoard.SetSquare(x, y, square);
            }
        }

        //Debug.Log($"Chessboard initialized with {BoardWidth * BoardHeight} squares using two tilemaps.");
    }

    public void AdjustPieces()
    {
        int adjustedCount = 0;

        void SnapPiece(GameObject piece)
        {
            if (piece == null) return;

            Vector3Int whiteCell = WhiteTilemap.WorldToCell(piece.transform.position);
            Vector3Int blackCell = BlackTilemap.WorldToCell(piece.transform.position);

            Vector3 centerPos;

            if (WhiteTilemap.HasTile(whiteCell))
                centerPos = WhiteTilemap.GetCellCenterWorld(whiteCell);
            else if (BlackTilemap.HasTile(blackCell))
                centerPos = BlackTilemap.GetCellCenterWorld(blackCell);
            else
                return;

            piece.transform.position = new Vector3(centerPos.x, centerPos.y, piece.transform.position.z);
            adjustedCount++;
        }

        foreach (var p in WhitePieces)
            SnapPiece(p);

        foreach (var p in BlackPieces)
            SnapPiece(p);

        //Debug.Log($" Adjusted {adjustedCount} chess pieces to the center of their grid squares.");
    }

    // Get square from world position
    public ChessBoardSquare GetSquareFromWorld(Vector3 worldPos)
    {
        Vector3Int whiteCell = WhiteTilemap.WorldToCell(worldPos);
        Vector3Int blackCell = BlackTilemap.WorldToCell(worldPos);

        ChessBoardSquare square = ChessBoard.GetSquare(whiteCell.x, whiteCell.y);
        if (square == null)
            square = ChessBoard.GetSquare(blackCell.x, blackCell.y);
        
        return square;
    }
    
    /// <summary>
    /// Assigns DefaultSquare and CurrentSquare to all pieces based on their positions
    /// </summary>
    public void AssignSquaresToPieces()
    {
        int assignedCount = 0;

        void AssignSquares(GameObject piece)
        {
            if (piece == null) return;

            ChessPiece chessPiece = piece.GetComponent<ChessPiece>();
            if (chessPiece == null) { Debug.Log($"chess piece script is null"); return; }
            if(ChessBoard == null) { Debug.Log($"chess borard 2D script is null"); }

            ChessBoardSquare closestSquare = ChessBoard.GetClosestSquare(piece.transform.position);
            //Debug.Log($"closest square: {closestSquare.Cell}");
            chessPiece.SetDefaultSquare(closestSquare);
            chessPiece.SetCurrentSquare(closestSquare);

            assignedCount++;
        }

        foreach (var p in WhitePieces)
            AssignSquares(p);

        foreach (var p in BlackPieces)
            AssignSquares(p);

        //Debug.Log($" Assigned DefaultSquare and CurrentSquare to {assignedCount} pieces.");
    }




    // Highlight system (applies tint color)
    public void HighlightSquare(ChessBoardSquare square, Color highlightColor)
    {
        if (square == null) return;

        if (square.SquareColor == ColorEnum.Light)
        {
            WhiteTilemap.SetTileFlags(square.Cell, TileFlags.None);
            WhiteTilemap.SetColor(square.Cell, highlightColor);
        }
        else
        {
            BlackTilemap.SetTileFlags(square.Cell, TileFlags.None);
            BlackTilemap.SetColor(square.Cell, highlightColor);
        }
    }

    public void ClearHighlights()
    {
        foreach (ChessBoardSquare square in ChessBoard.squares)
        {
            if (square.SquareColor == ColorEnum.Light)
            {
                WhiteTilemap.SetColor(square.Cell, Color.white);
            }
            else
            {
                BlackTilemap.SetColor(square.Cell, Color.white);
            }
        }
    }
}
