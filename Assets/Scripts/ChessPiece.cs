using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PieceType
{
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King
}

public enum ColorEnum
{
    Light,
    Dark
}

public class ChessPiece : MonoBehaviour
{
    public PieceType pieceType;
    public ColorEnum pieceColor;

    public ChessBoardSquare DefaultSquare {  get; private set; }
    public ChessBoardSquare CurrentSquare { get; private set; }
   
    public ChessBoard2D chessBoard;

    public Sprite _Sprite {  get; private set; }

    private void Awake()
    {
        Initialize(pieceType, pieceColor);
        if (chessBoard == null)
            chessBoard = FindFirstObjectByType<ChessTileManager>()?.ChessBoard;
        if (chessBoard != null)
            DefaultSquare = chessBoard.GetClosestSquare(transform.position);
    }

    public void Initialize(PieceType type, ColorEnum color)
    {
        pieceType = type;
        pieceColor = color;
    }

    /// <summary>
    /// Returns all potential squares the piece can move to
    /// (for now ignores occupancy and chess rules)
    /// </summary>
   

    // <summary>
    /// Snap the piece to its default square
    /// </summary>
    public void SnapToSquare()
    {
        if (DefaultSquare != null)
        {
            transform.position = DefaultSquare.WorldPos;
        }
    }

    public void SetCurrentSquare(ChessBoardSquare square)
    {
        CurrentSquare = square;
        //Debug.Log($"{gameObject.name}'s current square has been set to {CurrentSquare.Cell}");
    }
    public void SetDefaultSquare(ChessBoardSquare square)
    {
        DefaultSquare = square;
        //Debug.Log($"{gameObject.name}'s Default square has been set to {DefaultSquare.Cell}");
    }
}
