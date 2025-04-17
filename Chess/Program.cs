namespace Chess;

class Program
{
    static void Main(string[] args)
    {
        ChessBoard chessBoard;
        Player white, black, comWhite, comBlack;
        Player currentPlayer, opponent;
        bool whiteTurn = true, turnComplete, check, legalMoveExists, drawCondition;
        white = new Player(true);
        black = new Player(false);
        comWhite = new ComputerPlayer(true, new string[] {
            "C2C4", "B1C3", "C3D5", "G2G3", "F1G2", "G1F3", "E1G1", "C4D5", "F3E1", "D2D3", "C1E3", "D3E4",
            "D1D4", "E1D3", "E2D3", "D4E4", "E4F4", "E3F4", "A1D1", "D1D2", "F4D2", "D2B4", "F1E1", "E1E2",
            "E2D2", "A2B3", "G1F1", "F1E2", "G2H3", "G3G4", "G4G5", "H3E6", "D3D4", "E2D1", "D4C5"
            });
        comBlack = new ComputerPlayer(false, new string[] {
            "E7E5", "F8B4", "A7A5", "G8F6", "D7D6", "E8G8", "F6D5", "E5E4", "C8F5", "D8E7", "B8A6", "F5E4",
            "F8E8", "E4D3", "B4C5", "E7F6", "F6F4", "E8E2", "E2B2", "B2D2", "A6B4", "A5B4", "F7F5", "A8A3",
            "B4B3", "A3B3", "B7B5", "B5B4", "G7G6", "F5F4", "B3A3", "G8G7", "F4F3", "B4B3", "A3A1"
            });
        chessBoard = new ChessBoard(white, black);
        
        InitializeBoard(chessBoard);
        
        Console.WriteLine(chessBoard);
        
        do
        {
            currentPlayer = whiteTurn? chessBoard.GetWhitePlayer() : chessBoard.GetBlackPlayer();
            opponent = whiteTurn? chessBoard.GetBlackPlayer() : chessBoard.GetWhitePlayer();
            currentPlayer.SetDrawRequest(false);
            turnComplete = PlayerTurn(currentPlayer, chessBoard, opponent.IsDrawRequest());
            Console.WriteLine(chessBoard);
            check = opponent.IsInCheck(chessBoard);
            legalMoveExists = opponent.HasLegalMoves(chessBoard);
            if (check && legalMoveExists)
                Console.WriteLine("Check!");
            whiteTurn = !whiteTurn;
            drawCondition = chessBoard.FiftyMoveRule() || 
            chessBoard.IsDeadPosition() || chessBoard.IsThreefoldRepetition(whiteTurn);
        } while (turnComplete && legalMoveExists && !drawCondition);
        if (currentPlayer.IsDrawRequest() && opponent.IsDrawRequest())
        {
            Console.WriteLine("The game ends in a draw by agreement!");
        }
        else
        {
            if (!legalMoveExists && check)
            {
                
                Console.WriteLine("Checkmate!");
                Console.WriteLine("{0} wins!", currentPlayer);
            }
            else
            {
                if (drawCondition || (!check && !legalMoveExists))
                {
                    if (!check && !legalMoveExists)
                        Console.WriteLine("Stalemate!");
                    if (chessBoard.FiftyMoveRule())
                        Console.WriteLine("Fifty move rule!");
                    if (chessBoard.IsDeadPosition())
                        Console.WriteLine("Board is in dead position!");
                    if (chessBoard.IsThreefoldRepetition(whiteTurn))
                        Console.WriteLine("Threefold repetition!");
                    Console.WriteLine("The game ends in a draw!");
                }
            }
        }
        
        
        
        
        /*PlayerTurn(white, chessBoard);
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
        AddPieceToGame(king, 0, 0, chessBoard);
        AddPieceToGame(knight, 3, 3, chessBoard);
        AddPieceToGame(new Rook(white), 2, 3, chessBoard);
        AddPieceToGame(new Rook(black), 1, 1, chessBoard);
        AddPieceToGame(new Rook(white), 4, 4, chessBoard);
        AddPieceToGame(new Rook(black), 2, 2, chessBoard);
        Console.WriteLine(chessBoard);
        Console.WriteLine(white.IsInCheck(chessBoard));
        Console.WriteLine(chessBoard.Update(new PlayerMove(0, 0, 1, 1)));
        
        Pawn bp = new Pawn(black);
        Pawn wp = new Pawn(white);
        Queen queen = new Queen(black);
        Knight knight = new Knight(white);
        King whiteKing = new King(white);
        King blackKing = new King(black);
        //AddPieceToGame(whiteKing, 0, 4, chessBoard);
        //AddPieceToGame(blackKing, 7, 4, chessBoard);
        //AddPieceToGame(knight, 0, 0, chessBoard);
        //AddPieceToGame(new Rook(white), 0, 7, chessBoard);
        //AddPieceToGame(new Rook(black), 1, 2, chessBoard);
        //AddPieceToGame(new Rook(black), 7, 7, chessBoard);
        //AddPieceToGame(wp, 6, 0, chessBoard);
        InitializeBoard(chessBoard, black, white);
        Console.WriteLine(chessBoard);
        bool whiteTurn = true;
        ComputerPlayer current = whiteTurn? (ComputerPlayer)white : (ComputerPlayer)black;
        PlayerMove? move;
        do
        {
            move = current.GetNextMove();
            if (move == null) break;
            chessBoard.MakeMove(move, whiteTurn);
            Console.WriteLine(chessBoard);
            
            whiteTurn = !whiteTurn;
            Console.WriteLine(chessBoard.IsThreefoldRepetition(whiteTurn));
            current = whiteTurn? (ComputerPlayer)white : (ComputerPlayer)black;
        } while (true);
        //Console.WriteLine(chessBoard.ToStateEncoding(true));
        //Console.WriteLine(black.IsCastlingPossible(chessBoard));
        //AddPieceToGame(new Bishop(black), 2, 7, chessBoard);
        //AddPieceToGame(king, 0, 5, chessBoard);
        //chessBoard.PlacePiece(new Pawn(black), 2, 2);
        //chessBoard.PlacePiece(new Pawn(white), 6, 2);
        
        //Console.WriteLine(chessBoard);
        //Console.WriteLine(chessBoard.IsDeadPosition());
        //CheckForPromotion(move, chessBoard);
        //Console.WriteLine(chessBoard);
        //Console.WriteLine(white.IsInCheck(chessBoard));
        //Console.WriteLine(white.HasLegalMoves(chessBoard));
        /*
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
    
    static bool PlayerTurn(Player player, ChessBoard chessBoard, bool activeDrawRequest)
    {
        bool legalMove = false;
        PlayerMove? move;
        while (!legalMove)
        {
            move = player is ComputerPlayer? ((ComputerPlayer)player).GetNextMove() : GetUserInput(player, activeDrawRequest);
            if (move == null)
                return false;
            if (move.GetStartRow() == move.GetEndRow() && move.GetStartColumn() == move.GetEndColumn())
            {
                Console.WriteLine("Start position is identical to end position!");
                if (player is ComputerPlayer) return false;
                continue;
            }
            ChessPiece? piece = chessBoard.GetPiece(move.GetStartRow(), move.GetStartColumn());
            if (piece == null)
            {
                Console.WriteLine("No piece to move!");
                if (player is ComputerPlayer) return false;
                continue;
            }
            if (!piece.GetPlayer().Equals(player))
            {
                Console.WriteLine("{0} cannot move {1}'s piece!", player, piece.GetPlayer());
                if (player is ComputerPlayer) return false;
                continue;
            }
            // Is end position occupied by piece of the same color
            ChessPiece? endPiece = chessBoard.GetPiece(move.GetEndRow(), move.GetEndColumn());
            if (endPiece != null && endPiece.GetPlayer().Equals(player))
            {
                Console.WriteLine("{0} cannot move to a position occupied by one of {0}'s pieces!", player);
                if (player is ComputerPlayer) return false;
                continue;
            }
            // is move legal
            if (!piece.IsLegalMove(move, chessBoard))
            {
                Console.WriteLine("Move is illegal for {0}", piece);
                if (player is ComputerPlayer) return false;
                continue;
            }
            if (chessBoard.IsSelfCheck(move, player))
            {
                
                Console.WriteLine("Move places {0} in check!", player);
                if (player is ComputerPlayer) return false;
                continue;
            }
            chessBoard.MakeMove(move, player.IsWhite());
            CheckForPromotion(move, chessBoard);
            legalMove = true;
        }
        return true;
    }
    static PlayerMove? GetUserInput(Player player, bool activeDrawRequest)
    {
        string? input;
        string userInput;
        PlayerMove? move = null;
        bool invalid = false;
        
        while (move == null)
        {
            if (invalid)
                Console.WriteLine("Invalid move format!");
            Console.WriteLine("{0} please enter a move:", player);
            input = Console.ReadLine();
            userInput = input != null? input.Trim() : "";
            if (userInput == "DRAW" && !player.IsDrawRequest())
            {
                player.SetDrawRequest(true);
                if (activeDrawRequest)
                {
                    Console.WriteLine("{0} agrees to a draw!", player);
                    return null;
                }
                Console.WriteLine("{0} requests a draw!", player);
                Console.WriteLine("{0} please enter a move:", player);
                input = Console.ReadLine();
                userInput = input != null? input.Trim() : "";
            }
                
            move = PlayerMove.FromString(userInput);
            invalid = move == null;
        }
        return move;
    }
    static void InitializeBoard(ChessBoard chessBoard)
    {
        InitializeBoard(chessBoard, chessBoard.GetBlackPlayer(), chessBoard.GetWhitePlayer());
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
    static void CheckForPromotion(PlayerMove move, ChessBoard chessBoard)
    {
        ChessPiece? movedPiece = chessBoard.GetPiece(move.GetEndRow(), move.GetEndColumn());
        if (movedPiece is Pawn)
        {
            int lastRank = movedPiece.GetPlayer().IsWhite()? 7 : 0;
            if (movedPiece.GetCurrentRow() == lastRank)
            {
                Pawn pawnToPromote = (Pawn)movedPiece;
                string? input;
                bool success = true;
                Console.WriteLine(chessBoard);
                do
                {
                    if (!success)
                        Console.WriteLine("Invalid input!");
                    Console.WriteLine("Pawn has reached last rank. Please promote the pawn.");
                    Console.WriteLine("Please enter the desired promotion (R for rook, B for bishop, N for knight, Q for queen):");
                    input = Console.ReadLine();
                    if (input != null && input.Trim().Length >= 1)
                        success = pawnToPromote.Promote(input.Trim()[0], chessBoard);
                    else
                        success = false;
                } while (!success);
                Console.WriteLine("Pawn has been promoted!");
                
            }
        }
    }
}

class ChessPiece
{
    protected int currentRow;
    protected int currentColumn;
    Player player;
    bool captured;
    protected int indexForPlayer;
    public ChessPiece(Player player, int currentRow, int currentColumn) : this(player)
    {
        SetCurrentRow(currentRow);
        SetCurrentColumn(currentColumn);
    }
    public ChessPiece(Player player)
    {
        this.player = player;
        SetCaptured(false);
        SetIndexForPlayer(0);
    }
    public bool SetIndexForPlayer(int indexForPlayer)
    {
        if (indexForPlayer < 0 || indexForPlayer >= 16) return false;
        this.indexForPlayer = indexForPlayer;
        return true;
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
        if (captured) return false;
        if (move.IsStationary()) return false;
        ChessPiece? endPosition = chessBoard.GetPiece(move.GetEndRow(), move.GetEndColumn());
        if (endPosition != null && endPosition.GetPlayer().Equals(player)) return false;
        return true;
    }
    public virtual void UpdateAfterMove(PlayerMove move) {}
    public virtual void Revert(PlayerMove move) {}
    public virtual bool HasLegalMoves(ChessBoard chessBoard)
    {
        return false;
    }
    public virtual string ToStateEncoding()
    {
        return ToString();
    }
    
    protected bool HasLegalMovesAlongPath(int startRow, int startCol, int rowStep, int colStep, ChessBoard chessBoard)
    {
        for (int row = startRow, col = startCol; row < 8 && row >= 0 && col < 8 && col >= 0; row += rowStep, col += colStep)
        {
            ChessPiece? piece = chessBoard.GetPiece(row, col);
            if (piece != null && piece.GetPlayer().Equals(player))
                break;
            PlayerMove move = new PlayerMove(currentRow, currentColumn, row, col);
            if (!chessBoard.IsSelfCheck(move, player))
                return true;
            if (piece != null)
                break;
        }
        return false;
    }
}

class Pawn : ChessPiece
{
    //bool enPassantPossible;
    int moveCount;
    bool enPassantFlag;
    int enPassantTurn;
    
    public Pawn(Player player) : base(player) 
    {
        //enPassantPossible = false;
        enPassantFlag = false;
        moveCount = 0;
        enPassantTurn = 0;
    }
    public bool Promote(char promoteTo, ChessBoard chessBoard)
    {
        ChessPiece? promotion;
        switch (promoteTo)
        {
            case 'Q': case 'q':
                promotion = new Queen(GetPlayer());
                break;
            case 'R': case 'r':
                promotion = new Rook(GetPlayer(), moveCount);
                break;
            case 'B': case 'b':
                promotion = new Bishop(GetPlayer());
                break;
            case 'N': case 'n':
                promotion = new Knight(GetPlayer());
                break;
            default:
                return false;
        }
        bool success = chessBoard.PlacePiece(promotion, currentRow, currentColumn);
        if (!success) return false;
        success = GetPlayer().ChangePiece(indexForPlayer, promotion);
        if (!success)
        {
            chessBoard.PlacePiece(this, currentRow, currentColumn);
            return false;
        }
        return true;
    }
    public bool IsEnPassantPossible()
    {
        return enPassantFlag && GetPlayer().GetTurnCount() == enPassantTurn;
    }
    public override string ToString()
    {
        return base.ToString() + "P";
    }
    public bool IsCapturingMove(PlayerMove move, ChessBoard chessBoard)
    {
        if (!base.IsLegalMove(move, chessBoard)) return false;
        int rowDiff = move.GetEndRow() - move.GetStartRow();
        // check if move direction is backwards
        if (GetPlayer().IsWhite() != (rowDiff > 0))
            return false;
        if (!move.IsDiagonal()) return false;
        // check path distance is 1
        if (move.GetColumnDistance() != 1)
            return false;
        return true;
    }
    public override bool IsLegalMove(PlayerMove move, ChessBoard chessBoard)
    {
        if (!base.IsLegalMove(move, chessBoard)) return false;
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
            int maxDistance = moveCount == 0? 2 : 1;
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
                return ((Pawn)adjacentPiece).IsEnPassantPossible();
            }
        }
        return false;
    }
    public override void UpdateAfterMove(PlayerMove move)
    {
        if (move.GetRowDistance() == 2)
        {
            //enPassantPossible = true;
            enPassantFlag = true;
            enPassantTurn = GetPlayer().GetTurnCount() + 1;
        }
        /*else
            enPassantPossible = false;*/
        
        moveCount++;
    }
    public override void Revert(PlayerMove move)
    {
        moveCount--;
        //enPassantPossible = moveCount == 1 && enPassantFlag;
        if (move.GetRowDistance() == 2)
            enPassantFlag = false;
    }
    public override bool HasLegalMoves(ChessBoard chessBoard)
    {
        int forwardDirection = GetPlayer().IsWhite()? 1 : -1;
        bool valid;
        PlayerMove move = new PlayerMove(currentRow, currentColumn, currentRow, currentColumn);
        valid = move.SetEndRow(currentRow + forwardDirection);
        if (valid && IsLegalMove(move, chessBoard) && !chessBoard.IsSelfCheck(move, GetPlayer())) return true;
        valid = move.SetEndRow(currentRow + 2 * forwardDirection);
        if (valid && IsLegalMove(move, chessBoard) && !chessBoard.IsSelfCheck(move, GetPlayer())) return true;
        move.SetEndRow(currentRow + forwardDirection);
        valid = move.SetEndColumn(currentColumn - 1);
        if (valid && IsLegalMove(move, chessBoard) && !chessBoard.IsSelfCheck(move, GetPlayer())) return true;
        valid = move.SetEndColumn(currentColumn + 1);
        if (valid && IsLegalMove(move, chessBoard) && !chessBoard.IsSelfCheck(move, GetPlayer())) return true;
        return false;
    }

    public override string ToStateEncoding()
    {
        string result = ToString();
        if (moveCount == 0)
            result += "2";
        if (IsEnPassantPossible())
            result += "ep";
        return result;
    }
}
class Rook : ChessPiece
{

    int moveCount;
    public Rook(Player player) : this(player, 0)
    {

    }
    public Rook(Player player, int moveCount) : base(player)
    {
        this.moveCount = moveCount;
    }

    public override string ToString()
    {
        return base.ToString() + "R";
    }
    public override bool IsLegalMove(PlayerMove move, ChessBoard chessBoard)
    {
        if (!base.IsLegalMove(move, chessBoard)) return false;
        return move.IsNonBlockedHorizontal(chessBoard) || move.IsNonBlockedVertical(chessBoard);
    }
    public override bool HasLegalMoves(ChessBoard chessBoard)
    {
        if (HasLegalMovesAlongPath(currentRow + 1, currentColumn, 1, 0, chessBoard)) return true;
        if (HasLegalMovesAlongPath(currentRow - 1, currentColumn, -1, 0, chessBoard)) return true;
        if (HasLegalMovesAlongPath(currentRow, currentColumn + 1, 0, 1, chessBoard)) return true;
        if (HasLegalMovesAlongPath(currentRow, currentColumn - 1, 0, -1, chessBoard)) return true;
        return false;
    }
    public override void UpdateAfterMove(PlayerMove move)
    {
        moveCount++;
    }
    public override void Revert(PlayerMove move)
    {
        moveCount--;
    }
    public bool IsCastlingPossible()
    {
        return moveCount == 0;
    }
    public override string ToStateEncoding()
    {
        string result = ToString();
        if (IsCastlingPossible())
            result += "c";
        return result;
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
        if (!base.IsLegalMove(move, chessBoard)) return false;
        return move.IsNonBlockedDiagonal(chessBoard);
    }

    public override bool HasLegalMoves(ChessBoard chessBoard)
    {
        if (HasLegalMovesAlongPath(currentRow + 1, currentColumn + 1, 1, 1, chessBoard)) return true;
        if (HasLegalMovesAlongPath(currentRow - 1, currentColumn + 1, -1, 1, chessBoard)) return true;
        if (HasLegalMovesAlongPath(currentRow + 1, currentColumn - 1, 1, -1, chessBoard)) return true;
        if (HasLegalMovesAlongPath(currentRow - 1, currentColumn - 1, -1, -1, chessBoard)) return true;
        return false;
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
        if (!base.IsLegalMove(move, chessBoard)) return false;
        return (move.GetRowDistance() == 2 && move.GetColumnDistance() == 1) ||
            (move.GetColumnDistance() == 2 && move.GetRowDistance() == 1);
    }
    public override bool HasLegalMoves(ChessBoard chessBoard)
    {
        int[] rowSteps = {1, 1, 2, 2, -1, -1, -2, -2};
        int[] colSteps = {2, -2, 1, -1, 2, -2, 1, -1};
        bool validRow, validCol;
        PlayerMove move = new PlayerMove(currentRow, currentColumn, currentRow, currentColumn);
        for (int i = 0; i < 8; i++)
        {
            validRow = move.SetEndRow(currentRow + rowSteps[i]);
            validCol = move.SetEndColumn(currentColumn + colSteps[i]);
            if (validRow && validCol && base.IsLegalMove(move, chessBoard) && 
                !chessBoard.IsSelfCheck(move, GetPlayer()))
                return true;

        }
        return false;
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
        if (!base.IsLegalMove(move, chessBoard)) return false;
        return move.IsNonBlockedHorizontal(chessBoard) || move.IsNonBlockedVertical(chessBoard) ||
            move.IsNonBlockedDiagonal(chessBoard);
    }
    public override bool HasLegalMoves(ChessBoard chessBoard)
    {
        if (HasLegalMovesAlongPath(currentRow + 1, currentColumn, 1, 0, chessBoard)) return true;
        if (HasLegalMovesAlongPath(currentRow - 1, currentColumn, -1, 0, chessBoard)) return true;
        if (HasLegalMovesAlongPath(currentRow, currentColumn + 1, 0, 1, chessBoard)) return true;
        if (HasLegalMovesAlongPath(currentRow, currentColumn - 1, 0, -1, chessBoard)) return true;
        if (HasLegalMovesAlongPath(currentRow + 1, currentColumn + 1, 1, 1, chessBoard)) return true;
        if (HasLegalMovesAlongPath(currentRow - 1, currentColumn + 1, -1, 1, chessBoard)) return true;
        if (HasLegalMovesAlongPath(currentRow + 1, currentColumn - 1, 1, -1, chessBoard)) return true;
        if (HasLegalMovesAlongPath(currentRow - 1, currentColumn - 1, -1, -1, chessBoard)) return true;
        return false;
    }
}
class King : ChessPiece
{

    int moveCount;
    public King(Player player) : base(player) 
    {
        moveCount = 0;
    }

    public override string ToString()
    {
        return base.ToString() + "K";
    }
    public override bool IsLegalMove(PlayerMove move, ChessBoard chessBoard)
    {
        if (!base.IsLegalMove(move, chessBoard)) return false;
        if (move.GetColumnDistance() == 2 && move.GetRowDistance() == 0)
            return GetPlayer().IsCastlingPossible(chessBoard, move);
        return move.GetRowDistance() <= 1 && move.GetColumnDistance() <= 1;
    }

    public override bool HasLegalMoves(ChessBoard chessBoard)
    {
        int[] rowSteps = {-1, -1, -1, 0, 0, 1, 1, 1};
        int[] colSteps = {-1, 0, 1, -1, 1, -1, 0, 1};
        bool validRow, validCol;
        PlayerMove move = new PlayerMove(currentRow, currentColumn, currentRow, currentColumn);
        //Player opponent = GetPlayer().IsWhite()? chessBoard.GetBlackPlayer() : chessBoard.GetWhitePlayer();
        for (int i = 0; i < 8; i++)
        {
            validRow = move.SetEndRow(currentRow + rowSteps[i]);
            validCol = move.SetEndColumn(currentColumn + colSteps[i]);
            if (validRow && validCol && base.IsLegalMove(move, chessBoard) && 
                !chessBoard.IsSelfCheck(move, GetPlayer()))
                return true;

        }
        if (GetPlayer().IsCastlingPossible(chessBoard)) return true;
        return false;
    }
    public override void UpdateAfterMove(PlayerMove move)
    {
        moveCount++;
    }
    public override void Revert(PlayerMove move)
    {
        moveCount--;
    }
    public bool IsCastlingPossible()
    {
        return moveCount == 0;
    }

    public override string ToStateEncoding()
    {
        string result = ToString();
        if (IsCastlingPossible())
            result += "c";
        return result;
    }
}
class ChessBoard
{
    ChessPiece?[,] board;
    Player whitePlayer, blackPlayer;
    ChessPiece? endPosition, capturedPiece;
    int fiftyMoveCounter;
    int captureCount;
    string history;
    public ChessBoard(Player whitePlayer, Player blackPlayer)
    {
        board = new ChessPiece[8, 8];
        this.whitePlayer = whitePlayer;
        this.blackPlayer = blackPlayer;
        fiftyMoveCounter = 0;
        captureCount = 0;
        history = "";
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
    public Player GetWhitePlayer()
    {
        return whitePlayer;
    }
    public Player GetBlackPlayer()
    {
        return blackPlayer;
    }
    public bool MakeMove(PlayerMove move, bool white)
    {
        string stateEncoding = ToStateEncoding(white);
        bool result = Update(move);
        if (!result) return false;
        ChessPiece? movedPiece = GetPiece(move.GetEndRow(), move.GetEndColumn());
        if (capturedPiece != null)
            captureCount++;
        if (movedPiece is Pawn || capturedPiece != null)
            fiftyMoveCounter = 0;
        else
            fiftyMoveCounter++;
        Player currentPlayer = white? whitePlayer : blackPlayer;
        currentPlayer.UpdateTurnCount();
        if (history == "")
            history = stateEncoding;
        else
            history = stateEncoding + "|" + history;
        return true;
    }
    public bool FiftyMoveRule()
    {
        return fiftyMoveCounter >= 50;
    }
    public bool IsThreefoldRepetition(bool white)
    {
        string[] pastStates = history.Split('|');
        string currentState = ToStateEncoding(white);
        int repetition = 0;
        foreach (string state in pastStates)
        {
            if (currentState == state)
                repetition++;
            if (repetition >= 2)
                return true;
        }
        return false;
    }
    public bool Update(PlayerMove move)
    {
        endPosition = GetPiece(move.GetEndRow(), move.GetEndColumn());
        capturedPiece = endPosition;
        
        bool canMove = MovePiece(move);
        
        if (canMove)
        {
            
            ChessPiece? movedPiece = GetPiece(move.GetEndRow(), move.GetEndColumn());
            if (movedPiece != null)
                movedPiece.UpdateAfterMove(move);
            // check if move is en passant
            
            if (movedPiece is Pawn && move.IsDiagonal() && endPosition == null)
            {
                ChessPiece? adjacent = GetPiece(move.GetStartRow(), move.GetEndColumn());
                if (adjacent is Pawn && ((Pawn)adjacent).IsEnPassantPossible())
                    capturedPiece = adjacent;
            }
            // check if move is castling
            if (movedPiece is King && move.GetColumnDistance() == 2 && move.GetRowDistance() == 0)
            {
                bool kingside = move.GetEndColumn() > move.GetStartColumn();
                Rook? rook = kingside? movedPiece.GetPlayer().GetKingsideRook() : movedPiece.GetPlayer().GetQueensideRook();
                if (rook == null) return false;
                int rookEndColumn = kingside? move.GetEndColumn() - 1 : move.GetEndColumn() + 1;
                PlayerMove rookMove = new PlayerMove(rook.GetCurrentRow(), rook.GetCurrentColumn(), rook.GetCurrentRow(), rookEndColumn);
                MovePiece(rookMove);
                rook.UpdateAfterMove(rookMove);
            }
            if (capturedPiece != null)
                capturedPiece.SetCaptured(true);
            // did the move put the player in check?
            /*if (movedPiece != null && movedPiece.GetPlayer().IsInCheck(this))
            {
                // revert board to previous state
                //Revert(move);
                return false;
            }*/
            return true;
        }
        return false;
    }
    public bool IsSelfCheck(PlayerMove move, Player player)
    {
        Update(move);
        bool selfCheck = player.IsInCheck(this);
        Revert(move);
        return selfCheck;
    }
    public void Revert(PlayerMove move)
    {
        ChessPiece? movedPiece = GetPiece(move.GetEndRow(), move.GetEndColumn());
        if (movedPiece != null)
        {
            PlacePiece(movedPiece, move.GetStartRow(), move.GetStartColumn());
            movedPiece.Revert(move);
            if (movedPiece is King && move.GetColumnDistance() == 2 && move.GetRowDistance() == 0)
            {
                // revert castling
                bool kingside = move.GetEndColumn() > move.GetStartColumn();
                Rook? rook = kingside? movedPiece.GetPlayer().GetKingsideRook() : movedPiece.GetPlayer().GetQueensideRook();
                int rookColumn = kingside? 7 : 0;
                if (rook != null)
                {
                    PlacePiece(rook, rook.GetCurrentRow(), rookColumn);
                    rook.Revert(new PlayerMove(rook.GetCurrentRow(), rookColumn, rook.GetCurrentRow(), rook.GetCurrentColumn()));
                }
            }
        }
        if (endPosition != null)
            PlacePiece(endPosition, move.GetEndRow(), move.GetEndColumn());
        else
            board[move.GetEndRow(), move.GetEndColumn()] = null;
        if (capturedPiece != null)
            capturedPiece.SetCaptured(false);
    }
    public bool IsDeadPosition()
    {
        int onBoardCount = whitePlayer.GetPieceCount() + blackPlayer.GetPieceCount() - captureCount;
        if (whitePlayer.IsKingOnBoard() && blackPlayer.IsKingOnBoard())
        {
            if (onBoardCount == 2) return true;
            if (onBoardCount == 3)
                return whitePlayer.IsKnightOnBoard() || blackPlayer.IsKnightOnBoard();
        }
        return false;
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

    public string ToStateEncoding(bool white)
    {
        string result = white? "w" : "b";
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                ChessPiece? piece = board[row, col];
                if (piece != null && !piece.IsCaptured())
                {
                    result += piece.ToStateEncoding();
                }
                else
                {
                    result += "_";
                }
            }
        }
        return result;
    }
}
class Player
{
    bool white;
    ChessPiece[] pieces;
    int pieceCount;
    bool drawRequest;
    int kingIndex, kingsideRookIndex, queensideRookIndex;
    int turnCount;
    public Player(bool white)
    {
        this.white = white;
        pieces = new ChessPiece[16];
        pieceCount = 0;
        turnCount = 0;
        kingIndex = -1;
        kingsideRookIndex = -1;
        queensideRookIndex = -1;
        SetDrawRequest(false);
    }
    public int GetTurnCount()
    {
        return turnCount;
    }
    public void UpdateTurnCount()
    {
        turnCount++;
    }
    public bool IsKingOnBoard()
    {
        if (kingIndex == -1) return false;
        ChessPiece king = pieces[kingIndex];
        return !king.IsCaptured();
    }
    public bool IsKnightOnBoard()
    {
        for (int i = 0; i < pieceCount; i++)
            if (pieces[i] is Knight && !pieces[i].IsCaptured())
                return true;
        return false;
    }
    public int GetPieceCount()
    {
        return pieceCount;
    }
    public bool IsInCheck(ChessBoard chessBoard)
    {
        if (kingIndex == -1) return false;
        ChessPiece king = pieces[kingIndex];
        Player opponent = white? chessBoard.GetBlackPlayer() : chessBoard.GetWhitePlayer();
        /*for (int i = 0; i < opponent.pieceCount; i++)
        {
            ChessPiece opponentPiece = opponent.pieces[i];
            if (opponentPiece.IsCaptured()) continue;
            PlayerMove checkMove = new PlayerMove(opponentPiece.GetCurrentRow(), opponentPiece.GetCurrentColumn(), king.GetCurrentRow(), king.GetCurrentColumn());
            if (opponentPiece.IsLegalMove(checkMove, chessBoard))
                return true;
        }
        return false;*/
        return opponent.ThreatensPosition(king.GetCurrentRow(), king.GetCurrentColumn(), chessBoard);
    }
    public bool ThreatensPosition(int row, int column, ChessBoard chessBoard)
    {
        for (int i = 0; i < pieceCount; i++)
        {
            ChessPiece piece = pieces[i];
            if (piece.IsCaptured()) continue;
            PlayerMove captureMove = new PlayerMove(piece.GetCurrentRow(), piece.GetCurrentColumn(), row, column);
            if (piece is Pawn)
            {
                if (((Pawn)piece).IsCapturingMove(captureMove, chessBoard))
                    return true;
            }
            else
                if (piece.IsLegalMove(captureMove, chessBoard))
                    return true;
        }
        return false;
    }
    public bool HasLegalMoves(ChessBoard chessBoard)
    {
        for (int i = 0; i < pieceCount; i++)
        {
            if (!pieces[i].IsCaptured() && pieces[i].HasLegalMoves(chessBoard))
                return true;
        }
        return false;
    }
    public bool IsDrawRequest()
    {
        return drawRequest;
    }
    public void SetDrawRequest(bool drawRequest)
    {
        this.drawRequest = drawRequest;
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
            piece.SetIndexForPlayer(pieceCount);
            if (piece is King)
                kingIndex = pieceCount;
            if (piece is Rook)
            {
                if (piece.GetCurrentColumn() == 0)
                    queensideRookIndex = pieceCount;
                if (piece.GetCurrentColumn() == 7)
                    kingsideRookIndex = pieceCount;
            }
            pieceCount++;
        }
    }
    public bool ChangePiece(int index, ChessPiece piece)
    {
        if (index >= pieceCount || index < 0)
            return false;
        pieces[index] = piece;
        return true;
    }
    public Rook? GetKingsideRook()
    {
        if (kingsideRookIndex == -1) return null;
        return (Rook)pieces[kingsideRookIndex];
    }
    public Rook? GetQueensideRook()
    {
        if (queensideRookIndex == -1) return null;
        return (Rook)pieces[queensideRookIndex];
    }
    public bool IsCastlingPossible(ChessBoard chessBoard)
    {
        if (kingIndex == -1) return false;
        ChessPiece king = pieces[kingIndex];
        PlayerMove kingside = new PlayerMove(king.GetCurrentRow(), king.GetCurrentColumn(), king.GetCurrentRow(), king.GetCurrentColumn() + 2);
        PlayerMove queenside = new PlayerMove(king.GetCurrentRow(), king.GetCurrentColumn(), king.GetCurrentRow(), king.GetCurrentColumn() - 2);
        return IsCastlingPossible(chessBoard, kingside) || IsCastlingPossible(chessBoard, queenside);
    }
    public bool IsCastlingPossible(ChessBoard chessBoard, PlayerMove kingMove)
    {
        if (kingIndex == -1) return false;
        King king = (King)pieces[kingIndex];
        if (!king.IsCastlingPossible()) return false;
        Rook? rook;
        bool kingside = kingMove.GetEndColumn() > kingMove.GetStartColumn();
        rook = kingside? GetKingsideRook() : GetQueensideRook();
        if (rook == null) return false;
        if (!rook.IsCastlingPossible()) return false;
        PlayerMove pathBetween = new PlayerMove(king.GetCurrentRow(), king.GetCurrentColumn(), 
            rook.GetCurrentRow(), rook.GetCurrentColumn());
        if (!pathBetween.IsNonBlockedHorizontal(chessBoard)) return false;
        Player opponent = white? chessBoard.GetBlackPlayer() : chessBoard.GetWhitePlayer();
        int column = kingMove.GetStartColumn();
        while ((kingside && column <= kingMove.GetEndColumn()) || (!kingside && column >= kingMove.GetEndColumn()))
        {
            if (opponent.ThreatensPosition(king.GetCurrentRow(), column, chessBoard))
                return false;
            if (kingside)
                column++;
            else
                column--;
        }
        return true;
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
        this.SetStartRow(startRow);
        this.SetStartColumn(startColumn);
        this.SetEndRow(endRow);
        this.SetEndColumn(endColumn);
    }
    public bool IsStationary()
    {
        return startRow == endRow && startColumn == endColumn;
    }
    public bool SetStartRow(int startRow)
    {
        if (startRow < 0 || startRow >= 8)
            return false;
        this.startRow = startRow;
        return true;
    }
    public int GetStartRow()
    {
        return startRow;
    }
    public bool SetStartColumn(int startColumn)
    {
        if (startColumn < 0 || startColumn >= 8)
            return false;
        this.startColumn = startColumn;
        return true;
    }
    public int GetStartColumn()
    {
        return startColumn;
    }
    public bool SetEndRow(int endRow)
    {
        if (endRow < 0 || endRow >= 8)
            return false;
        this.endRow = endRow;
        return true;
    }
    public int GetEndRow()
    {
        return endRow;
    }
    public bool SetEndColumn(int endColumn)
    {
        if (endColumn < 0 || endColumn >= 8)
            return false;
        this.endColumn = endColumn;
        return true;
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
        int rowStep, colStep;
        rowStep = startRow < endRow? 1 : -1;
        colStep = startColumn < endColumn? 1 : -1;
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
        
        for (int row = startRow+rowStep, col = startColumn+colStep; row != endRow && col != endColumn; row+=rowStep, col+=colStep)
        {
            ChessPiece? piece = chessBoard.GetPiece(row, col);
            if (piece != null && !piece.IsCaptured())
                return false;
        }
            
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
        {
            ChessPiece? piece = chessBoard.GetPiece(endRow, col);
            if (piece != null && !piece.IsCaptured())
                return false;
        }
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
        {
            ChessPiece? piece = chessBoard.GetPiece(row, endColumn);
            if (piece != null && !piece.IsCaptured())
                return false;
        }
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

class ComputerPlayer : Player
{
    int moveIndex;
    PlayerMove?[] moves;
    public ComputerPlayer(bool white, PlayerMove[] moves) : base(white) 
    {
        this.moves = moves;
        moveIndex = 0;
    }
    public ComputerPlayer(bool white, string[] moveStrings) : base(white)
    {
        moves = new PlayerMove[moveStrings.Length];
        for (int i = 0; i < moveStrings.Length; i++)
            moves[i] = PlayerMove.FromString(moveStrings[i]);
        moveIndex = 0;
    }
    public PlayerMove? GetNextMove()
    {
        if (moveIndex >= moves.Length)
            return null;
        PlayerMove? nextMove = moves[moveIndex];
        moveIndex++;
        return nextMove;
    }
}