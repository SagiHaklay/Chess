namespace Chess;

class Program
{
    static void Main(string[] args)
    {
        ChessBoard chessBoard = new ChessBoard();
        Player white, black;
        white = new Player(true);
        black = new Player(false);
        InitializeBoard(chessBoard, black, white);
        Console.WriteLine(chessBoard);
        /*chessBoard.MovePiece(1, 0, 3, 0);
        Console.WriteLine(chessBoard);
        chessBoard.MovePiece(7, 1, 5, 2);
        Console.WriteLine(chessBoard);*/
    }

    static void InitializeBoard(ChessBoard chessBoard, Player black, Player white)
    {
        for (int col = 0; col < 8; col++)
        {
            AddPieceToGame(new Pawn(white), 1, col, chessBoard);
            AddPieceToGame(new Pawn(black), 6, col, chessBoard);
        }
        AddPieceToGame(new Rook(white), 0, 0, chessBoard);
        AddPieceToGame(new Rook(black), 7, 0, chessBoard);
        AddPieceToGame(new Rook(white), 0, 7, chessBoard);
        AddPieceToGame(new Rook(black), 7, 7, chessBoard);
        AddPieceToGame(new Knight(white), 0, 1, chessBoard);
        AddPieceToGame(new Knight(black), 7, 1, chessBoard);
        AddPieceToGame(new Knight(white), 0, 6, chessBoard);
        AddPieceToGame(new Knight(black), 7, 6, chessBoard);
        AddPieceToGame(new Bishop(white), 0, 2, chessBoard);
        AddPieceToGame(new Bishop(black), 7, 2, chessBoard);
        AddPieceToGame(new Bishop(white), 0, 5, chessBoard);
        AddPieceToGame(new Bishop(black), 7, 5, chessBoard);
        AddPieceToGame(new Queen(white), 0, 3, chessBoard);
        AddPieceToGame(new Queen(black), 7, 3, chessBoard);
        AddPieceToGame(new King(white), 0, 4, chessBoard);
        AddPieceToGame(new King(black), 7, 4, chessBoard);
    }
    static void AddPieceToGame(ChessPiece piece, int row, int column, ChessBoard chessBoard)
    {
        chessBoard.PlacePiece(piece, row, column);
        piece.GetPlayer().AddPiece(piece);
    }
}

class ChessPiece
{
    int currentRow;
    int currentColumn;
    Player player;
    public ChessPiece(Player player, int currentRow, int currentColumn) : this(player)
    {
        SetCurrentRow(currentRow);
        SetCurrentColumn(currentColumn);
    }
    public ChessPiece(Player player)
    {
        this.player = player;
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
    public Player GetPlayer()
    {
        return player;
    }
    public override string ToString()
    {
        return player.IsWhite()? "W" : "B";
    }
}

class Pawn : ChessPiece
{

    
    public Pawn(Player player) : base(player) {}

    public override string ToString()
    {
        return base.ToString() + "P";
    }
}
class Rook : ChessPiece
{

    
    public Rook(Player player) : base(player) {}

    public override string ToString()
    {
        return base.ToString() + "R";
    }
}
class Bishop : ChessPiece
{

    
    public Bishop(Player player) : base(player) {}

    public override string ToString()
    {
        return base.ToString() + "B";
    }
}
class Knight : ChessPiece
{

    
    public Knight(Player player) : base(player) {}

    public override string ToString()
    {
        return base.ToString() + "N";
    }
}
class Queen : ChessPiece
{

    
    public Queen(Player player) : base(player) {}

    public override string ToString()
    {
        return base.ToString() + "Q";
    }
}
class King : ChessPiece
{

    
    public King(Player player) : base(player) {}

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
    public ChessPiece? GetPiece(int row, int column)
    {
        if (row < 0 || row >= board.GetLength(0) || column < 0 || column >= board.GetLength(1))
            return null;
        return board[row, column];
    }
    public bool MovePiece(int startRow, int startColumn, int endRow, int endColumn)
    {
        ChessPiece? piece = GetPiece(startRow, startColumn);
        if (piece == null)
            return false;
        bool canPlace = PlacePiece(piece, endRow, endColumn);
        if (!canPlace)
            return false;
        board[startRow, startColumn] = null;
        return true;
    }
    public override string ToString()
    {
        
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
class Player
{
    bool white;
    ChessPiece[] pieces;
    int pieceCount;
    public Player(bool white)
    {
        this.white = white;
        pieces = new ChessPiece[16];
        pieceCount = 0;
    }
    public bool IsWhite()
    {
        return white;
    }
    public override string ToString()
    {
        return string.Format("Player {0}", white? "white" : "black");
    }
    public void AddPiece(ChessPiece piece)
    {
        if (pieceCount < 16)
        {
            pieces[pieceCount] = piece;
            pieceCount++;
        }
    }
}