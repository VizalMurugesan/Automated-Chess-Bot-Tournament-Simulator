// (RangerD, 2025)
using System;
using System.Collections.Generic;
using UnityEngine;

public enum ActionEnum
{
    MoveTo,
    CaptureTo
}

public class ChessBoard2D
{
    public int width;
    public int height;
    ChessBoardSquare[,] squares;

    public ChessBoard2D(int width, int height)
    {
        this.width = width;
        this.height = height;
        squares = new ChessBoardSquare[width, height];
    }

    public void SetSquare(int x, int y, ChessBoardSquare square)
    {
        squares[x, y] = square;
    }

    public ChessBoardSquare GetSquare(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return squares[x, y];
        return null;
    }

    public ChessBoardSquare[] GetNeighbours(ChessBoardSquare square)
    {
        return new ChessBoardSquare[]
        {
            GetSquare(square.x + 1, square.y),
            GetSquare(square.x - 1, square.y),
            GetSquare(square.x, square.y + 1),
            GetSquare(square.x, square.y - 1),

            // Optional diagonals
            GetSquare(square.x + 1, square.y + 1),
            GetSquare(square.x + 1, square.y - 1),
            GetSquare(square.x - 1, square.y + 1),
            GetSquare(square.x - 1, square.y - 1)
        };
    }

    public ChessBoardSquare GetClosestSquare(Vector3 worldPos)
    {
        ChessBoardSquare closest = null;
        float minDist = float.MaxValue;

        foreach (ChessBoardSquare s in squares)
        {
            float dist = Vector3.Distance(worldPos, s.WorldPos);
            if (dist < minDist)
            {
                minDist = dist;
                closest = s;
            }
        }

        return closest;
    }

    public ChessBoardSquare[,] GetAllSquares()
    {
        return squares;
    }

    public void ClearHighlights()
    {
        foreach (ChessBoardSquare s in squares)
        {
            s.SetHighlight(false);
        }
    }

    public List<ChessBoardSquare> GetAllMovableSquares(ChessPiece piece)
    {
        List<ChessBoardSquare> movable = new List<ChessBoardSquare>();

        foreach (ChessBoardSquare square in squares)
        {
            if (square == null) continue;
            if (square == piece.CurrentSquare) continue;
            movable.Add(square);
        }

        return movable;
    }

    // ============================
    // Rook movement generation
    // ============================
    public Dictionary<ChessBoardSquare, ActionEnum> GetRookMovableSquares(GameObject obj)
    {
        Dictionary<ChessBoardSquare, ActionEnum> moves = new Dictionary<ChessBoardSquare, ActionEnum>();

        ChessPiece piece = obj?.GetComponent<ChessPiece>();
        if (piece == null)
        {
            Debug.LogError("GetRookMovableSquares called with null or invalid piece!");
            return moves;
        }

        if (piece.CurrentSquare == null)
        {
            Debug.LogError($"Piece '{piece.name}' has no CurrentSquare assigned!");
            return moves;
        }

        int startX = piece.CurrentSquare.x;
        int startY = piece.CurrentSquare.y;
        Debug.Log($"Calculating rook moves for '{piece.name}' at {startX}, {startY}");

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0),   // right
            new Vector2Int(-1, 0),  // left
            new Vector2Int(0, 1),   // up
            new Vector2Int(0, -1)   // down
        };

        foreach (var dir in directions)
        {
            int x = startX;
            int y = startY;

            while (true)
            {
                x += dir.x;
                y += dir.y;

                ChessBoardSquare nextSquare = GetSquare(x, y);
                if (nextSquare == null)
                {
                    Debug.Log($"{dir} is outtabounds");
                    break; // Out of bounds
                }

                if (nextSquare.IsOccupied)
                {
                    ChessPiece occupant = nextSquare.OccupyingPiece;
                    Debug.Log($"{dir} occupied by {occupant.name}");
                    if (occupant != null && occupant.pieceColor != piece.pieceColor)
                    {
                        moves[nextSquare] = ActionEnum.CaptureTo;
                    }
                    // Stop regardless (rook can't jump)
                    break;
                }

                moves[nextSquare] = ActionEnum.MoveTo;
            }
        }

        return moves;
    }
}
