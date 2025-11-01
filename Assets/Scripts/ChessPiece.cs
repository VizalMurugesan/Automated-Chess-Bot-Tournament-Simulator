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

    // UI stuff
    public float _NativeWidth {  get; private set; }
    public float _NativeHeight {  get; private set; }

    public Sprite _Sprite {  get; private set; }

    private void Awake()
    {
        
        Vector3 size = GetComponent<Renderer>() != null
            ? GetComponent<Renderer>().bounds.size
            : Vector3.one;

        _Sprite = GetComponent<Image>() != null
            ? GetComponent<Image>().sprite : null;



        _NativeWidth = size.x;
        _NativeHeight = size.y;

        Initialize(pieceType, pieceColor);
    }

    public void Initialize(PieceType type, ColorEnum color)
    {
        pieceType = type;
        pieceColor = color;
    }
}
