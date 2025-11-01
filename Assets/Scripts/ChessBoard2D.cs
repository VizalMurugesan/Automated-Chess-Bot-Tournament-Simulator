//(RangerD, 2025)
using System;
using System.Collections.Generic;
using UnityEngine;

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

    // Get adjacent squares (optional diagonal access)
    public ChessBoardSquare[] GetNeighbours(ChessBoardSquare square)
    {
        return new ChessBoardSquare[]
        {
            GetSquare(square.x + 1, square.y),
            GetSquare(square.x - 1, square.y),
            GetSquare(square.x, square.y + 1),
            GetSquare(square.x, square.y - 1),

            // Diagonals if needed for certain piece moves
            GetSquare(square.x + 1, square.y + 1),
            GetSquare(square.x + 1, square.y - 1),
            GetSquare(square.x - 1, square.y + 1),
            GetSquare(square.x - 1, square.y - 1)
        };
    }

    // Optional helper: find a square from world position
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

    // Optional helper: clear highlights
    public void ClearHighlights()
    {
        foreach (ChessBoardSquare s in squares)
        {
            s.SetHighlight(false);
        }
    }

    public  List<ChessBoardSquare> GetAllMovableSquares( ChessPiece piece)
    {
        List<ChessBoardSquare> movable = new List<ChessBoardSquare>();

        foreach (ChessBoardSquare square in squares)
        {
            if (square == null) continue;

            // Skip the square the piece is currently on
            if (square == piece.CurrentSquare) continue;

            movable.Add(square);
        }

        return movable;
    }

    #region
    //IndividualPieceMovementSquares

    //Rook
    public List<ChessBoardSquare> GetRookMovableSquaresRook(ChessPiece piece)
    {
        List<ChessBoardSquare> movable = new List<ChessBoardSquare>();
        int startX = piece.CurrentSquare.x;
        int startY = piece.CurrentSquare.y;

        // Horizontal and vertical moves
        for (int x = 0; x < Manager.Instance.tileManager.BoardWidth; x++)
        {
            if (x != startX)
            {
                ChessBoardSquare squareInConsideration = squares[x, startY];
                if (squareInConsideration.IsOccupied)
                {
                    movable.Add(squareInConsideration);
                    break;
                }
                
            }
                
        }
        for (int y = 0; y < Manager.Instance.tileManager.BoardHeight; y++)
        {
            if (y != startY)
            {
                ChessBoardSquare squareInConsideration = squares[y, startX];
                if (squareInConsideration.IsOccupied)
                {
                    movable.Add(squareInConsideration);
                    break;
                }
            }
        }
            

        return movable;
    }

    #endregion
}
