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

    public ChessBoard2D chessBoard;

    [Header("Optional Origin")]
    public Transform BoardOrigin;

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

        chessBoard = new ChessBoard2D(BoardWidth, BoardHeight);

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
                chessBoard.SetSquare(x, y, square);
            }
        }

        Debug.Log($"Chessboard initialized with {BoardWidth * BoardHeight} squares using two tilemaps.");
    }

    // Get square from world position
    public ChessBoardSquare GetSquareFromWorld(Vector3 worldPos)
    {
        Vector3Int whiteCell = WhiteTilemap.WorldToCell(worldPos);
        Vector3Int blackCell = BlackTilemap.WorldToCell(worldPos);

        ChessBoardSquare square = chessBoard.GetSquare(whiteCell.x, whiteCell.y);
        if (square == null)
            square = chessBoard.GetSquare(blackCell.x, blackCell.y);

        return square;
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
        foreach (ChessBoardSquare square in chessBoard.squares)
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
