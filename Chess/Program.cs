namespace Chess;

class Program
{
    static void Main(string[] args)
    {
        ChessBoard chessBoard = new ChessBoard();
        Player white, black;
        white = new Player(true);
        black = new Player(false);
        /*InitializeBoard(chessBoard, black, white);
        Console.WriteLine(chessBoard);
        PlayerTurn(white, chessBoard);
        Console.WriteLine(chessBoard);
        PlayerTurn(black, chessBoard);
        Console.WriteLine(chessBoard);
        ChessPiece rook = new Rook(white);
        ChessPiece bishop = new Bishop(white);
        ChessPiece queen1 = new Queen(white);
        ChessPiece queen2 = new Queen(white);
        chessBoard.PlacePiece(queen1, 3, 3);
        chessBoard.PlacePiece(queen2, 2, 2);
        ChessPiece king = new King(white);
        ChessPiece knight = new Knight(black);
        chessBoard.PlacePiece(knight, 3, 3);
        chessBoard.PlacePiece(new Rook(white), 2, 3);
        chessBoard.PlacePiece(new Rook(black), 3, 4);
        chessBoard.PlacePiece(new Rook(white), 4, 4);
        chessBoard.PlacePiece(new Rook(black), 2, 2);
        Pawn bp = new Pawn(black);
        Pawn wp = new Pawn(white);
        chessBoard.PlacePiece(wp, 2, 3);
        chessBoard.PlacePiece(bp, 6, 4);
        //chessBoard.PlacePiece(new Pawn(black), 2, 2);
        //chessBoard.PlacePiece(new Pawn(white), 6, 2);
        Console.WriteLine(chessBoard);
        chessBoard.Update(new PlayerMove(2, 3, 4, 3));
        Console.WriteLine(chessBoard);
        //Console.WriteLine(wp.IsEnPassantPossible()); // true
        chessBoard.Update(new PlayerMove(6, 4, 4, 4));
        Console.WriteLine(chessBoard);
        Console.WriteLine(wp.IsLegalMove(new PlayerMove(4, 3, 5, 3), chessBoard)); // true
        Console.WriteLine(wp.IsLegalMove(new PlayerMove(4, 3, 6, 3), chessBoard)); // false
        Console.WriteLine(wp.IsLegalMove(new PlayerMove(4, 3, 5, 4), chessBoard)); // true
        //Console.WriteLine(wp.IsLegalMove(new PlayerMove(1, 3, 0, 3), chessBoard)); // false
        Console.WriteLine(bp.IsLegalMove(new PlayerMove(4, 4, 3, 4), chessBoard)); // true
        Console.WriteLine(bp.IsLegalMove(new PlayerMove(4, 4, 2, 4), chessBoard)); // false
        Console.WriteLine(bp.IsLegalMove(new PlayerMove(4, 4, 3, 3), chessBoard)); // true
        chessBoard.Update(new PlayerMove(4, 3, 5, 4));
        Console.WriteLine(chessBoard);
        //Console.WriteLine(bp.IsLegalMove(new PlayerMove(6, 3, 7, 3), chessBoard)); // false
        
        Console.WriteLine(knight.IsLegalMove(new PlayerMove(3, 3, 4, 3), chessBoard)); // false
        Console.WriteLine(knight.IsLegalMove(new PlayerMove(3, 3, 3, 5), chessBoard)); // false
        Console.WriteLine(knight.IsLegalMove(new PlayerMove(3, 3, 4, 5), chessBoard)); // true
        Console.WriteLine(knight.IsLegalMove(new PlayerMove(3, 3, 5, 4), chessBoard)); // true
        Console.WriteLine(knight.IsLegalMove(new PlayerMove(3, 3, 1, 2), chessBoard)); // true
        Console.WriteLine(knight.IsLegalMove(new PlayerMove(3, 3, 1, 4), chessBoard)); // true
        Console.WriteLine(knight.IsLegalMove(new PlayerMove(3, 3, 1, 5), chessBoard)); // false
        
        
        Console.WriteLine(queen1.IsLegalMove(new PlayerMove(3, 3, 6, 3), chessBoard));
        Console.WriteLine(queen1.IsLegalMove(new PlayerMove(3, 3, 3, 4), chessBoard));
        Console.WriteLine(queen1.IsLegalMove(new PlayerMove(3, 3, 1, 3), chessBoard));
        Console.WriteLine(queen1.IsLegalMove(new PlayerMove(3, 3, 3, 6), chessBoard));
        Console.WriteLine(queen1.IsLegalMove(new PlayerMove(3, 3, 4, 4), chessBoard));
        Console.WriteLine(queen2.IsLegalMove(new PlayerMove(2, 2, 1, 1), chessBoard));
        Console.WriteLine(queen2.IsLegalMove(new PlayerMove(2, 2, 4, 4), chessBoard));
        Console.WriteLine(queen2.IsLegalMove(new PlayerMove(2, 2, 1, 3), chessBoard));
        Console.WriteLine(queen2.IsLegalMove(new PlayerMove(2, 2, 1, 0), chessBoard));
        */
    }
    static void PlayerTurn(Player player, ChessBoard chessBoard)
    {
        bool legalMove = false;
        PlayerMove move;
        while (!legalMove)
        {
            move = GetUserInput(player);
            if (move.GetStartRow() == move.GetEndRow() && move.GetStartColumn() == move.GetEndColumn())
            {
                Console.WriteLine("Start position is identical to end position!");
                continue;
            }
            ChessPiece? piece = chessBoard.GetPiece(move.GetStartRow(), move.GetStartColumn());
            if (piece == null)
            {
                Console.WriteLine("No piece to move!");
                continue;
            }
            if (!piece.GetPlayer().Equals(player))
            {
                Console.WriteLine("{0} cannot move {1}'s piece!", player, piece.GetPlayer());
                continue;
            }
            // Is end position occupied by piece of the same color
            ChessPiece? endPiece = chessBoard.GetPiece(move.GetEndRow(), move.GetEndColumn());
            if (endPiece != null && endPiece.GetPlayer().Equals(player))
            {
                Console.WriteLine("{0} cannot move to a position occupied by one of {0}'s pieces!", player);
                continue;
            }
            // is move legal
            legalMove = chessBoard.MovePiece(move);
        }
    }
    static PlayerMove GetUserInput(Player player)
    {
        string? input;
        PlayerMove? move;
        Console.WriteLine("{0} please enter a move:", player);
        input = Console.ReadLine();
        move = PlayerMove.FromString(input != null? input.Trim() : null);
        while (move == null)
        {
            Console.WriteLine("Invalid move!");
            Console.WriteLine("{0} please enter a move:", player);
            input = Console.ReadLine() ?? "";
            move = PlayerMove.FromString(input != null? input.Trim() : null);
        }
        return move;
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
    bool captured;
    public ChessPiece(Player player, int currentRow, int currentColumn) : this(player)
    {
        SetCurrentRow(currentRow);
        SetCurrentColumn(currentColumn);
    }
    public ChessPiece(Player player)
    {
        this.player = player;
        SetCaptured(false);
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
    public bool IsCaptured()
    {
        return captured;
    }
    public void SetCaptured(bool captured)
    {
        this.captured = captured;
    }
    public override string ToString()
    {
        return player.IsWhite()? "W" : "B";
    }
    public virtual bool IsLegalMove(PlayerMove move, ChessBoard chessBoard)
    {
        return false;
    }
    public virtual void UpdateAfterMove(PlayerMove move) {}
}

class Pawn : ChessPiece
{
    bool firstMove;
    bool enPassantPossible;
    
    public Pawn(Player player) : base(player) 
    {
        firstMove = true;
        enPassantPossible = false;
    }
    public bool IsEnPassantPossible()
    {
        return enPassantPossible;
    }
    public override string ToString()
    {
        return base.ToString() + "P";
    }
    public override bool IsLegalMove(PlayerMove move, ChessBoard chessBoard)
    {
        int rowDiff = move.GetEndRow() - move.GetStartRow();
        // check if move direction is backwards
        if (GetPlayer().IsWhite() != (rowDiff > 0))
            return false;
        // move direction is forward
        ChessPiece? endPosition = chessBoard.GetPiece(move.GetEndRow(), move.GetEndColumn());
        if (move.IsNonBlockedVertical(chessBoard))
        {
            if (endPosition != null)
                return false;
            // end position is empty
            int maxDistance = firstMove? 2 : 1;
            return move.GetRowDistance() <= maxDistance;
        }
        if (move.IsDiagonal())
        {
            // check path distance is 1
            if (move.GetColumnDistance() != 1)
                return false;
            
            if (endPosition != null)
            {
                // check capture is possible
                return !endPosition.GetPlayer().Equals(GetPlayer());
            }
            // check if en passant is possible
            ChessPiece? adjacentPiece = chessBoard.GetPiece(GetCurrentRow(), move.GetEndColumn());
            if (adjacentPiece is Pawn)
            {
                return ((Pawn)adjacentPiece).enPassantPossible;
            }
        }
        return false;
    }
    public override void UpdateAfterMove(PlayerMove move)
    {
        firstMove = false;
        enPassantPossible = move.GetRowDistance() == 2;
    }
}
class Rook : ChessPiece
{

    
    public Rook(Player player) : base(player) {}

    public override string ToString()
    {
        return base.ToString() + "R";
    }
    public override bool IsLegalMove(PlayerMove move, ChessBoard chessBoard)
    {
        return move.IsNonBlockedHorizontal(chessBoard) || move.IsNonBlockedVertical(chessBoard);
    }
}
class Bishop : ChessPiece
{

    
    public Bishop(Player player) : base(player) {}

    public override string ToString()
    {
        return base.ToString() + "B";
    }
    public override bool IsLegalMove(PlayerMove move, ChessBoard chessBoard)
    {
        return move.IsNonBlockedDiagonal(chessBoard);
    }
}
class Knight : ChessPiece
{

    
    public Knight(Player player) : base(player) {}

    public override string ToString()
    {
        return base.ToString() + "N";
    }
    public override bool IsLegalMove(PlayerMove move, ChessBoard chessBoard)
    {
        return (move.GetRowDistance() == 2 && move.GetColumnDistance() == 1) ||
            (move.GetColumnDistance() == 2 && move.GetRowDistance() == 1);
    }
}
class Queen : ChessPiece
{

    
    public Queen(Player player) : base(player) {}

    public override string ToString()
    {
        return base.ToString() + "Q";
    }
    public override bool IsLegalMove(PlayerMove move, ChessBoard chessBoard)
    {
        return move.IsNonBlockedHorizontal(chessBoard) || move.IsNonBlockedVertical(chessBoard) ||
            move.IsNonBlockedDiagonal(chessBoard);
    }
}
class King : ChessPiece
{

    
    public King(Player player) : base(player) {}

    public override string ToString()
    {
        return base.ToString() + "K";
    }
    public override bool IsLegalMove(PlayerMove move, ChessBoard chessBoard)
    {
        return move.GetRowDistance() <= 1 && move.GetColumnDistance() <= 1;
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
        
        board[startRow, startColumn] = null;
        bool canPlace = PlacePiece(piece, endRow, endColumn);
        if (!canPlace)
        {
            board[startRow, startColumn] = piece;
            return false;
        }
        
        return true;
    }
    public bool MovePiece(PlayerMove move)
    {
        return MovePiece(move.GetStartRow(), move.GetStartColumn(), move.GetEndRow(), move.GetEndColumn());
    }
    public void Update(PlayerMove move)
    {
        ChessPiece? endPosition = GetPiece(move.GetEndRow(), move.GetEndColumn());
        
        bool canMove = MovePiece(move);
        
        if (canMove)
        {
            if (endPosition != null)
                endPosition.SetCaptured(true);
            ChessPiece? movedPiece = GetPiece(move.GetEndRow(), move.GetEndColumn());
            if (movedPiece != null)
                movedPiece.UpdateAfterMove(move);
            // check if move is en passant
            
            if (movedPiece is Pawn && move.IsDiagonal() && endPosition == null)
            {
                ChessPiece? adjacent = GetPiece(move.GetStartRow(), move.GetEndColumn());
                if (adjacent is Pawn && ((Pawn)adjacent).IsEnPassantPossible())
                    adjacent.SetCaptured(true);
            }
        }
    }
    public override string ToString()
    {
        
        string result = "   A  B  C  D  E  F  G  H\n";
        for (int row = board.GetLength(0) - 1; row >= 0; row--)
        {
            result += string.Format("{0}  ", row + 1);
            for (int col = 0; col < board.GetLength(1); col++)
            {
                string square = "  ";
                ChessPiece? piece = board[row, col];
                if (piece != null && !piece.IsCaptured())
                {
                    square = piece.ToString();
                }
                square += " ";
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
        return string.Format("{0} Player", white? "White" : "Black");
    }
    public void AddPiece(ChessPiece piece)
    {
        if (pieceCount < 16)
        {
            pieces[pieceCount] = piece;
            pieceCount++;
        }
    }
    public override bool Equals(object? obj)
    {
        if (!(obj is Player)) return false;
        Player other = (Player)obj;
        return white == other.white;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
class PlayerMove
{
    int startRow;
    int startColumn;
    int endRow;
    int endColumn;
    public PlayerMove(int startRow, int startColumn, int endRow, int endColumn)
    {
        this.startRow = startRow;
        this.startColumn = startColumn;
        this.endRow = endRow;
        this.endColumn = endColumn;
    }
    public int GetStartRow()
    {
        return startRow;
    }
    public int GetStartColumn()
    {
        return startColumn;
    }
    public int GetEndRow()
    {
        return endRow;
    }
    public int GetEndColumn()
    {
        return endColumn;
    }
    public override string ToString()
    {
        string result = "";
        string columnChars = "ABCDEFGH";
        result += columnChars[startColumn];
        result += (startRow + 1);
        result += columnChars[endColumn];
        result += (endRow + 1);
        return result;
    }
    public int GetRowDistance()
    {
        int diff = endRow - startRow;
        if (diff < 0)
            return -diff;
        return diff;
    }
    public int GetColumnDistance()
    {
        int diff = endColumn - startColumn;
        if (diff < 0)
            return -diff;
        return diff;
    }
    public bool IsDiagonal()
    {
        return GetRowDistance() == GetColumnDistance();
    }
    public bool IsNonBlockedDiagonal(ChessBoard chessBoard)
    {
        if (!IsDiagonal())
            return false;
        int minRow, minCol, maxRow, maxCol;
        if (startRow < endRow)
        {
            minRow = startRow;
            maxRow = endRow;
        }
        else 
        {
            minRow = endRow;
            maxRow = startRow;
        }
        if (startColumn < endColumn)
        {
            minCol = startColumn;
            maxCol = endColumn;
        }
        else
        {
            minCol = endColumn;
            maxCol = startColumn;
        }
        for (int row = minRow+1, col = minCol+1; row < maxRow && col < maxCol; row++, col++)
            if (chessBoard.GetPiece(row, col) != null)
                return false;
        return true;
    }
    public bool IsNonBlockedHorizontal(ChessBoard chessBoard)
    {
        if (startRow != endRow)
            return false;
        int start, end;
        if (startColumn < endColumn)
        {
            start = startColumn;
            end = endColumn;
        }
        else
        {
            end = startColumn;
            start = endColumn;
        }
        for (int col = start+1; col < end; col++)
                if (chessBoard.GetPiece(endRow, col) != null)
                    return false; // blocking piece
        return true;
    }
    public bool IsNonBlockedVertical(ChessBoard chessBoard)
    {
        if (startColumn != endColumn)
            return false;
        int start, end;
        if (startRow < endRow)
        {
            start = startRow;
            end = endRow;
        }
        else
        {
            end = startRow;
            start = endRow;
        }
        for (int row = start+1; row < end; row++)
                if (chessBoard.GetPiece(row, endColumn) != null)
                    return false; // blocking piece
        return true;
    }
    public static PlayerMove? FromString(string? moveString)
    {
        if (moveString == null)
            return null;
        if (moveString.Length != 4)
            return null;
        int startColumn = CharToColumn(moveString[0]);
        if (startColumn == -1)
            return null;
        int startRow = CharToRow(moveString[1]);
        if (startRow == -1)
            return null;
        int endColumn = CharToColumn(moveString[2]);
        if (endColumn == -1)
            return null;
        int endRow = CharToRow(moveString[3]);
        if (endRow == -1)
            return null;
        return new PlayerMove(startRow, startColumn, endRow, endColumn);
    }
    static int CharToColumn(char letter)
    {
        int num = -1;
        switch (letter)
        {
            case 'A': case 'a':
                num = 0;
                break;
            case 'B': case 'b':
                num = 1;
                break;
            case 'C': case 'c':
                num = 2;
                break;
            case 'D': case 'd':
                num = 3;
                break;
            case 'E': case 'e':
                num = 4;
                break;
            case 'F': case 'f':
                num = 5;
                break;
            case 'G': case 'g':
                num = 6;
                break;
            case 'H': case 'h':
                num = 7;
                break;
            default:
                break;
        }
        return num;
    }
    static int CharToRow(char number)
    {
        int num = -1;
        
        switch (number)
        {
            case '1':
                num = 0;
                break;
            case '2':
                num = 1;
                break;
            case '3':
                num = 2;
                break;
            case '4':
                num = 3;
                break;
            case '5':
                num = 4;
                break;
            case '6':
                num = 5;
                break;
            case '7':
                num = 6;
                break;
            case '8':
                num = 7;
                break;
            default:
                break;
        }
        return num;
    }
}