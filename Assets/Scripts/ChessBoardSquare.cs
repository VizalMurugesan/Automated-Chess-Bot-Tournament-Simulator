
using UnityEngine;
using static Unity.VisualScripting.StickyNote;

public class ChessBoardSquare
{
    public int x;
    public int y;

    public Vector3 WorldPos;     // Position in world space (for placing pieces)
    public Vector3Int Cell;      // Position in grid/cell coordinates

    public bool IsOccupied;      // Whether a piece is on this square
    public ChessPiece OccupyingPiece; // Reference to the piece currently on this square (if any)

    public ColorEnum SquareColor;  // Light or Dark square
    public bool IsHighlighted = false; // For move preview, selection, etc.

    public ChessBoardSquare(int x, int y, Vector3 worldPos, Vector3Int cell, ColorEnum squareColor)
    {
        this.x = x;
        this.y = y;
        this.WorldPos = worldPos;
        this.Cell = cell;
        this.SquareColor = squareColor;
        this.IsOccupied = false;
        this.OccupyingPiece = null;
    }

    // Assign a piece to this square
    public void PlacePiece(ChessPiece piece)
    {
        OccupyingPiece = piece;
        IsOccupied = piece != null;
    }

    // Remove the piece from this square
    public void RemovePiece()
    {
        OccupyingPiece = null;
        IsOccupied = false;
    }

    // For equality (so you can compare squares easily)
    public bool Equals(ChessBoardSquare other)
    {
        return Cell == other.Cell;
    }

    // Optional: highlight management
    public void SetHighlight(bool state)
    {
        IsHighlighted = state;
    }
}
