namespace Chess;

class Program
{
    static void Main(string[] args)
    {
        ChessBoard chessBoard = new ChessBoard();
        for (int col = 0; col < 8; col++)
        {
            chessBoard.PlacePiece(new Pawn(true), 1, col);
            chessBoard.PlacePiece(new Pawn(false), 6, col);
        }
        chessBoard.PlacePiece(new Rook(true), 0, 0);
        chessBoard.PlacePiece(new Rook(false), 7, 0);
        chessBoard.PlacePiece(new Rook(true), 0, 7);
        chessBoard.PlacePiece(new Rook(false), 7, 7);
        chessBoard.PlacePiece(new Knight(true), 0, 1);
        chessBoard.PlacePiece(new Knight(false), 7, 1);
        chessBoard.PlacePiece(new Knight(true), 0, 6);
        chessBoard.PlacePiece(new Knight(false), 7, 6);
        chessBoard.PlacePiece(new Bishop(true), 0, 2);
        chessBoard.PlacePiece(new Bishop(false), 7, 2);
        chessBoard.PlacePiece(new Bishop(true), 0, 5);
        chessBoard.PlacePiece(new Bishop(false), 7, 5);
        chessBoard.PlacePiece(new Queen(true), 0, 3);
        chessBoard.PlacePiece(new Queen(false), 7, 3);
        chessBoard.PlacePiece(new King(true), 0, 4);
        chessBoard.PlacePiece(new King(false), 7, 4);
        Console.WriteLine(chessBoard);
    }
}

class ChessPiece
{
    bool white;
    int currentRow;
    int currentColumn;
    public ChessPiece(bool white, int currentRow, int currentColumn) : this(white)
    {
        SetCurrentRow(currentRow);
        SetCurrentColumn(currentColumn);
    }
    public ChessPiece(bool white)
    {
        this.white = white;
    }
    public bool SetCurrentRow(int currentRow)
    {
        if (currentRow < 0 || currentRow >= 8)
            return false;
        this.currentRow = currentRow;
        return true;
    }
    public bool SetCurrentColumn(int currentColumn)
    {
        if (currentColumn < 0 || currentColumn >= 8)
            return false;
        this.currentColumn = currentColumn;
        return true;
    }
    public int GetCurrentRow()
    {
        return currentRow;
    }
    public int GetCurrentColumn()
    {
        return currentColumn;
    }
    public override string ToString()
    {
        return white? "W" : "B";
    }
}

class Pawn : ChessPiece
{

    public Pawn(bool white, int currentRow, int currentColumn) 
    : base(white, currentRow, currentColumn)
    {}
    public Pawn(bool white) : base(white) {}

    public override string ToString()
    {
        return base.ToString() + "P";
    }
}
class Rook : ChessPiece
{

    public Rook(bool white, int currentRow, int currentColumn) 
    : base(white, currentRow, currentColumn)
    {}
    public Rook(bool white) : base(white) {}

    public override string ToString()
    {
        return base.ToString() + "R";
    }
}
class Bishop : ChessPiece
{

    public Bishop(bool white, int currentRow, int currentColumn) 
    : base(white, currentRow, currentColumn)
    {}
    public Bishop(bool white) : base(white) {}

    public override string ToString()
    {
        return base.ToString() + "B";
    }
}
class Knight : ChessPiece
{

    public Knight(bool white, int currentRow, int currentColumn) 
    : base(white, currentRow, currentColumn)
    {}
    public Knight(bool white) : base(white) {}

    public override string ToString()
    {
        return base.ToString() + "N";
    }
}
class Queen : ChessPiece
{

    public Queen(bool white, int currentRow, int currentColumn) 
    : base(white, currentRow, currentColumn)
    {}
    public Queen(bool white) : base(white) {}

    public override string ToString()
    {
        return base.ToString() + "Q";
    }
}
class King : ChessPiece
{

    public King(bool white, int currentRow, int currentColumn) 
    : base(white, currentRow, currentColumn)
    {}
    public King(bool white) : base(white) {}

    public override string ToString()
    {
        return base.ToString() + "K";
    }
}
class ChessBoard
{
    ChessPiece?[,] board;
    public ChessBoard()
    {
        board = new ChessPiece[8, 8];
    }
    public bool PlacePiece(ChessPiece piece, int row, int column)
    {
        
        int oldRow = piece.GetCurrentRow();
        bool result = piece.SetCurrentRow(row);
        if (!result)
            return false;
        result = piece.SetCurrentColumn(column);
        if (!result)
        {
            piece.SetCurrentRow(oldRow);
            return false;
        }
        board[row, column] = piece;
        return true;
    }
    public override string ToString()
    {
        //string toColumnChar = "ABCDEFGH";
        string result = "   A  B  C  D  E  F  G  H\n";
        for (int row = board.GetLength(0) - 1; row >= 0; row--)
        {
            result += string.Format("{0}  ", row + 1);
            for (int col = 0; col < board.GetLength(1); col++)
            {
                string square = board[row, col] == null? "   " : board[row, col] + " ";
                result += square;
            }
            result += '\n';
        }
        return result;
    }
}