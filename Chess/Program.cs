namespace Chess;

class Program
{
    static void Main(string[] args)
    {
        new ChessGame().Play();
    }
    
    
}

class ChessPiece
{
    protected Position currentPosition;
    Player player;
    bool captured;
    protected int indexForPlayer;
    public ChessPiece(Player player, Position position) : this(player)
    {
        SetCurrentPosition(position);
    }
    public ChessPiece() 
    {
        player = new Player(false);
        currentPosition = new Position();
    }
    public ChessPiece(Player player)
    {
        this.player = player;
        currentPosition = new Position();
        SetCaptured(false);
        SetIndexForPlayer(-1);
    }
    public void SetIndexForPlayer(int indexForPlayer)
    {
        this.indexForPlayer = indexForPlayer;
    }
    public bool SetCurrentRow(int currentRow)
    {
        return currentPosition.SetRow(currentRow);
    }
    public bool SetCurrentColumn(int currentColumn)
    {
        return currentPosition.SetColumn(currentColumn);
    }
    public void SetCurrentPosition(Position currentPosition)
    {
        this.currentPosition = currentPosition;
    }
    public int GetCurrentRow()
    {
        return currentPosition.GetRow();
    }
    public int GetCurrentColumn()
    {
        return currentPosition.GetColumn();
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
        if (captured) return "__";
        return player.IsWhite()? "W" : "B";
    }
    public virtual bool IsSameColor(ChessPiece other)
    {
        if (other is EmptyPiece) return false;
        return player.Equals(other.player);
    }
    public virtual bool IsLegalMove(PlayerMove move, ChessGame game)
    {
        if (captured) return false;
        if (move.IsStationary()) return false;
        ChessPiece endPosition = game.GetPiece(move.GetEndRow(), move.GetEndColumn());
        if (!(endPosition is EmptyPiece) && IsSameColor(endPosition)) return false;
        return true;
    }
    public virtual void UpdateAfterMove(PlayerMove move) {}
    public virtual void Revert(PlayerMove move) {}
    public virtual bool HasLegalMoves(ChessGame game)
    {
        bool validStep;
        PlayerMove move;
        foreach (Step step in GetPossibleSteps())
        {
            validStep = currentPosition.IsValidStep(step);
            move = new PlayerMove(currentPosition.Copy(), currentPosition.AddStep(step));
            if (validStep && IsLegalMove(move, game) && 
                !game.IsSelfCheck(move, GetPlayer()))
                return true;
        }
        return false;
    }
    public virtual Step[] GetPossibleSteps()
    {
        return new Step[] {};
    }
    public virtual string ToStateEncoding()
    {
        if (captured) return "_";
        return ToString();
    }
    
}
class EmptyPiece : ChessPiece
{
    public EmptyPiece() : base() {}
    public override bool IsLegalMove(PlayerMove move, ChessGame game)
    {
        return false;
    }
    public override bool HasLegalMoves(ChessGame game)
    {
        return false;
    }
    public override string ToString()
    {
        return "__";
    }
    public override string ToStateEncoding()
    {
        return "_";
    }
    public override bool IsSameColor(ChessPiece other)
    {
        return false;
    }
}
class Pawn : ChessPiece
{
    int moveCount;
    bool movedTwoRows;
    int enPassantTurn;
    
    public Pawn(Player player) : base(player) 
    {
        movedTwoRows = false;
        moveCount = 0;
        enPassantTurn = 0;
    }
    ChessPiece GetPromotion(char promoteTo)
    {
        switch (promoteTo)
        {
            case 'Q': case 'q':
                return new Queen(GetPlayer());
            case 'R': case 'r':
                return new Rook(GetPlayer(), moveCount);
            case 'B': case 'b':
                return new Bishop(GetPlayer());
            case 'N': case 'n':
                return new Knight(GetPlayer());
            default:
                return this;
        }
    }
    public bool Promote(char promoteTo, ChessGame game)
    {
        ChessPiece promotion = GetPromotion(promoteTo);
        if (promotion is Pawn) return false;
        game.PlacePiece(promotion, GetCurrentRow(), GetCurrentColumn());
        GetPlayer().ChangePiece(indexForPlayer, promotion);
        return true;
    }
    public bool IsEnPassantTarget()
    {
        return movedTwoRows && GetPlayer().GetTurnCount() == enPassantTurn;
    }
    public override string ToString()
    {
        if (IsCaptured()) return base.ToString();
        return base.ToString() + "P";
    }
    public bool IsCapturingMove(PlayerMove move, ChessGame game)
    {
        if (!base.IsLegalMove(move, game)) return false;
        if(!IsForward(move)) return false;
        if (!move.IsDiagonal()) return false;
        // check path distance is 1
        if (move.GetColumnDistance() != 1)
            return false;
        return true;
    }
    bool IsForward(PlayerMove move)
    {
        int rowDiff = move.GetEndRow() - move.GetStartRow();
        // check if move direction is backwards
        return GetPlayer().IsWhite() == (rowDiff > 0);
    }
    bool CanCaptureEnPassant(PlayerMove move, ChessGame game)
    {
        ChessPiece adjacentPiece = game.GetPiece(GetCurrentRow(), move.GetEndColumn());
        if (adjacentPiece is Pawn)
        {
            return ((Pawn)adjacentPiece).IsEnPassantTarget();
        }
        return false;
    }
    public override bool IsLegalMove(PlayerMove move, ChessGame game)
    {
        if (!base.IsLegalMove(move, game)) return false;
        if(!IsForward(move)) return false;
        ChessPiece endPosition = game.GetPiece(move.GetEndRow(), move.GetEndColumn());
        if (move.IsVertical() && move.IsNotBlocked(game))
        {
            if (!(endPosition is EmptyPiece))
                return false;
            // end position is empty
            int maxDistance = moveCount == 0? 2 : 1;
            return move.GetRowDistance() <= maxDistance;
        }
        if (move.IsDiagonal())
        {
            if (move.GetColumnDistance() != 1) // check path distance is 1
                return false;
            if (!(endPosition is EmptyPiece)) // check capture is possible
                return !IsSameColor(endPosition);
            return CanCaptureEnPassant(move, game); // check if en passant is possible
        }
        return false;
    }
    public override void UpdateAfterMove(PlayerMove move)
    {
        if (move.GetRowDistance() == 2)
        {
            movedTwoRows = true;
            enPassantTurn = GetPlayer().GetTurnCount() + 1;
        }
        moveCount++;
    }
    public override void Revert(PlayerMove move)
    {
        moveCount--;
        if (move.GetRowDistance() == 2)
            movedTwoRows = false;
    }
    public override Step[] GetPossibleSteps()
    {
        int forwardDirection = GetPlayer().IsWhite()? 1 : -1;
        return new Step[]
        {
            new Step(forwardDirection, 0), new Step(2 * forwardDirection, 0),
            new Step(forwardDirection, 1), new Step(forwardDirection, -1)
        };
    }
    public override string ToStateEncoding()
    {
        if (IsCaptured()) return base.ToStateEncoding();
        string result = ToString();
        if (moveCount == 0)
            result += "2";
        if (IsEnPassantTarget())
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
    public Rook(Player player, Position position) : this(player, 0)
    {
        SetCurrentPosition(position);
    }
    public Rook(Player player, int moveCount) : base(player)
    {
        this.moveCount = moveCount;
    }
    public override string ToString()
    {
        if (IsCaptured()) return base.ToString();
        return base.ToString() + "R";
    }
    public override bool IsLegalMove(PlayerMove move, ChessGame game)
    {
        if (!base.IsLegalMove(move, game)) return false;
        if (!move.IsHorizontal() && !move.IsVertical()) return false;
        return move.IsNotBlocked(game);
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
        if (IsCaptured()) return base.ToStateEncoding();
        string result = ToString();
        if (IsCastlingPossible())
            result += "c";
        return result;
    }
    public override Step[] GetPossibleSteps()
    {
        Step[] steps = new Step[28];
        int lastIndex = 0;
        for (int i = -7; i <= 7; i++)
        {
            if (i != 0)
            {
                steps[lastIndex] = new Step(i, 0);
                lastIndex++;
                steps[lastIndex] = new Step(0, i);
                lastIndex++;
            }
        }
        return steps;
    }
}
class Bishop : ChessPiece
{
    public Bishop(Player player) : base(player) {}
    public Bishop(Player player, Position position) : this(player)
    {
        SetCurrentPosition(position);
    }
    public override string ToString()
    {
        if (IsCaptured()) return base.ToString();
        return base.ToString() + "B";
    }
    public override bool IsLegalMove(PlayerMove move, ChessGame game)
    {
        if (!base.IsLegalMove(move, game)) return false;
        if (!move.IsDiagonal()) return false;
        return move.IsNotBlocked(game);
    }
    
    public override Step[] GetPossibleSteps()
    {
        Step[] steps = new Step[28];
        int lastIndex = 0;
        for (int i = -7; i <= 7; i++)
        {
            if (i != 0)
            {
                steps[lastIndex] = new Step(i, i);
                lastIndex++;
                steps[lastIndex] = new Step(i, -i);
                lastIndex++;
            }
        }
        return steps;
    }
}
class Knight : ChessPiece
{
    public Knight(Player player) : base(player) {}

    public override string ToString()
    {
        if (IsCaptured()) return base.ToString();
        return base.ToString() + "N";
    }
    public override bool IsLegalMove(PlayerMove move, ChessGame game)
    {
        if (!base.IsLegalMove(move, game)) return false;
        return (move.GetRowDistance() == 2 && move.GetColumnDistance() == 1) ||
            (move.GetColumnDistance() == 2 && move.GetRowDistance() == 1);
    }
    
    public override Step[] GetPossibleSteps()
    {
        return new Step[]
        {
            new Step(1, 2), new Step(1, -2), new Step(2, 1), new Step(2, -1), 
            new Step(-1, 2), new Step(-1, -2), new Step(-2, 1), new Step(-2, -1)
        };
    }
}
class Queen : ChessPiece
{
    Rook queenAsRook;
    Bishop queenAsBishop;
    public Queen(Player player) : base(player) 
    {
        queenAsBishop = new Bishop(player, currentPosition);
        queenAsRook = new Rook(player, currentPosition);
    }
    public override string ToString()
    {
        if (IsCaptured()) return base.ToString();
        return base.ToString() + "Q";
    }
    public override bool IsLegalMove(PlayerMove move, ChessGame game)
    {
        if (!base.IsLegalMove(move, game)) return false;
        return queenAsRook.IsLegalMove(move, game) || queenAsBishop.IsLegalMove(move, game);
    }
    public override bool HasLegalMoves(ChessGame game)
    {
        return queenAsRook.HasLegalMoves(game) || queenAsBishop.HasLegalMoves(game);
    }

    public override void UpdateAfterMove(PlayerMove move)
    {
        queenAsRook.SetCurrentPosition(currentPosition);
        queenAsBishop.SetCurrentPosition(currentPosition);
    }
    public override void Revert(PlayerMove move)
    {
        queenAsRook.SetCurrentPosition(currentPosition);
        queenAsBishop.SetCurrentPosition(currentPosition);
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
        if (IsCaptured()) return base.ToString();
        return base.ToString() + "K";
    }
    public override bool IsLegalMove(PlayerMove move, ChessGame game)
    {
        if (!base.IsLegalMove(move, game)) return false;
        if (move.GetColumnDistance() == 2 && move.GetRowDistance() == 0)
            return GetPlayer().IsCastlingPossible(game, move);
        return move.GetRowDistance() <= 1 && move.GetColumnDistance() <= 1;
    }
    public override Step[] GetPossibleSteps()
    {
        return new Step[]
        {
            new Step(-1, -1), new Step(-1, 0), new Step(-1, 1), new Step(0, -1), 
            new Step(0, 1), new Step(1, -1), new Step(1, 0), new Step(1, 1)
        };
    }
    public override bool HasLegalMoves(ChessGame game)
    {
        if (base.HasLegalMoves(game)) return true;
        if (GetPlayer().IsCastlingPossible(game)) return true;
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
        if (IsCaptured()) return base.ToStateEncoding();
        string result = ToString();
        if (IsCastlingPossible())
            result += "c";
        return result;
    }
}
class ChessGame
{
    ChessPiece[,] board;
    Player whitePlayer, blackPlayer;
    bool whiteTurn, check, legalMoveExists;
    int nonCaptureOrPawnMoveCount;
    string gameStateHistory;
    ChessPiece endPosition, capturedPiece;
    int onBoardCount;
    public ChessGame()
    {
        whitePlayer = new Player(true);
        blackPlayer = new Player(false);
        board = new ChessPiece[8, 8];
        whiteTurn = true;
        nonCaptureOrPawnMoveCount = 0;
        gameStateHistory = "";
        endPosition = new EmptyPiece();
        capturedPiece = new EmptyPiece();
        onBoardCount = 0;
        check = false;
        legalMoveExists = true;
    }
    void PrintBoard()
    {
        Console.WriteLine(ToString());
    }
    public void Play()
    {
        InitializeBoard();
        PrintBoard();
        RunGameLoop();
    }
    void RunGameLoop()
    {
        Player currentPlayer, opponent;
        bool drawCondition = false;
        do
        {
            currentPlayer = whiteTurn? whitePlayer : blackPlayer;
            opponent = whiteTurn? blackPlayer : whitePlayer;
            ExecutePlayerTurn(currentPlayer, opponent.IsDrawRequest());
            whiteTurn = !whiteTurn;
            if (currentPlayer.IsDrawRequest())
            {
                if (!opponent.IsDrawRequest())
                    continue;
                break;
            }
            PrintBoard();
            check = opponent.IsInCheck(this);
            legalMoveExists = opponent.HasLegalMoves(this);
            if (check && legalMoveExists)
                Console.WriteLine("Check!");
            drawCondition = IsFiftyMoveRule() || IsDeadPosition() || IsThreefoldRepetition();
        } while (legalMoveExists && !drawCondition);
        EndGame(currentPlayer);
    }
    void EndGame(Player currentPlayer)
    {
        if (!legalMoveExists && check)
        {
            Console.WriteLine("Checkmate!");
            Console.WriteLine("{0} wins!", currentPlayer);
        }
        else
            EndGameInDraw();
    }
    void EndGameInDraw()
    {
        if (whitePlayer.IsDrawRequest() && blackPlayer.IsDrawRequest())
            Console.WriteLine("The game ends in a draw by agreement!");
        else if (IsFiftyMoveRule())
            Console.WriteLine("Fifty move rule!");
        else if (IsDeadPosition())
            Console.WriteLine("Board is in dead position!");
        else if (IsThreefoldRepetition())
            Console.WriteLine("Threefold repetition!");
        else
            Console.WriteLine("Stalemate!");
        Console.WriteLine("The game ends in a draw!");
    }
    void ExecutePlayerTurn(Player player, bool activeDrawRequest)
    {
        player.SetDrawRequest(false);
        PlayerMove move = GetUserInput(player, activeDrawRequest);
        MakeMove(move);
        if (IsPromotionRequired(move))
            PromotePawn(move);
    }
    bool IsMoveLegal(PlayerMove move, Player player)
    {
        if (move.IsStationary())
        {
            Console.WriteLine("Start position is identical to end position!");
            return false;
        }
        ChessPiece piece = GetPiece(move.GetStartRow(), move.GetStartColumn());
        if (piece is EmptyPiece)
        {
            Console.WriteLine("No piece to move!");
            return false;
        }
        if (!piece.GetPlayer().Equals(player))
        {
            Console.WriteLine("{0} cannot move {1}'s piece!", player, piece.GetPlayer());
            return false;
        }
        // is move legal
        if (!piece.IsLegalMove(move, this))
        {
            Console.WriteLine("Move is illegal for {0}", piece);
            return false;
        }
        if (IsSelfCheck(move, player))
        {
            Console.WriteLine("Move places {0} in check!", player);
            return false;
        }
        return true;
    }
    PlayerMove GetUserInput(Player player, bool activeDrawRequest)
    {
        PlayerMove move;
        bool invalid = false;
        do
        {
            if (invalid)
                Console.WriteLine("Invalid move format!");
            Console.WriteLine("{0} please enter a move:", player);
            string? input = Console.ReadLine();
            string userInput = input != null? input.Trim() : "";
            move = PlayerMove.FromString(userInput);
            if (move.IsDrawRequest() && !player.IsDrawRequest())
            {
                player.SetDrawRequest(true);
                if (activeDrawRequest)
                {
                    Console.WriteLine("{0} agrees to a draw!", player);
                    return move;
                }
                Console.WriteLine("{0} requests a draw!", player);
                return move;
            }
            
            invalid = !move.IsValid();
        }
        while (invalid || !IsMoveLegal(move, player));
        return move;
    }
    ChessPiece[] GetEmptyRow()
    {
        ChessPiece[] emptyRow = new ChessPiece[8];
        for (int col = 0; col < 8; col++)
            emptyRow[col] = new EmptyPiece();
        return emptyRow;
    }
    ChessPiece[] GetPawnRow(Player player)
    {
        ChessPiece[] pawnRow = new ChessPiece[8];
        for (int col = 0; col < 8; col++)
            pawnRow[col] = new Pawn(player);
        return pawnRow;
    }
    ChessPiece[] GetKingRow(Player player)
    {
        return new ChessPiece[]
        {
            new Rook(player), new Knight(player), new Bishop(player), new Queen(player),
            new King(player), new Bishop(player), new Knight(player), new Rook(player)
        };
    }
    void InitializeBoard()
    {
        ChessPiece[][] pieces = 
        {
            GetKingRow(whitePlayer),
            GetPawnRow(whitePlayer),
            GetEmptyRow(),
            GetEmptyRow(),
            GetEmptyRow(),
            GetEmptyRow(),
            GetPawnRow(blackPlayer),
            GetKingRow(blackPlayer)
        };
        for (int row = 0; row < 8; row++)
            for (int col = 0; col < 8; col++)
                AddPieceToGame(pieces[row][col], row, col);
    }
    void AddPieceToGame(ChessPiece piece, int row, int column)
    {
        PlacePiece(piece, row, column);
        piece.GetPlayer().AddPiece(piece);
        onBoardCount++;
    }
    bool IsPromotionRequired(PlayerMove move)
    {
        ChessPiece movedPiece = GetPiece(move.GetEndRow(), move.GetEndColumn());
        int lastRank = movedPiece.GetPlayer().IsWhite()? 7 : 0;
        return movedPiece is Pawn && movedPiece.GetCurrentRow() == lastRank;
    }
    void PromotePawn(PlayerMove move)
    {
        Pawn pawnToPromote = (Pawn)GetPiece(move.GetEndRow(), move.GetEndColumn());
        bool success = true;
        PrintBoard();
        do
        {
            if (!success)
                Console.WriteLine("Invalid input!");
            Console.WriteLine("Pawn has reached last rank. Please promote the pawn.");
            Console.WriteLine("Please enter the desired promotion (R for rook, B for bishop, N for knight, Q for queen):");
            string? input = Console.ReadLine();
            if (input != null && input.Trim().Length >= 1)
                success = pawnToPromote.Promote(input.Trim()[0], this);
            else
                success = false;
        } while (!success);
        Console.WriteLine("Pawn has been promoted!");
    }
    
    public bool IsFiftyMoveRule()
    {
        return nonCaptureOrPawnMoveCount >= 50;
    }
    public bool IsThreefoldRepetition()
    {
        string[] pastStates = gameStateHistory.Split('|');
        string currentState = ToStateEncoding();;
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
    public void MakeMove(PlayerMove move)
    {
        string stateEncoding = ToStateEncoding();
        UpdateBoard(move);
        ChessPiece? movedPiece = GetPiece(move.GetEndRow(), move.GetEndColumn());
        if (CapturedPieceExists())
            onBoardCount--;
        if (movedPiece is Pawn || CapturedPieceExists())
            nonCaptureOrPawnMoveCount = 0;
        else
            nonCaptureOrPawnMoveCount++;
        Player currentPlayer = whiteTurn? whitePlayer : blackPlayer;
        currentPlayer.UpdateTurnCount();
        if (gameStateHistory == "")
            gameStateHistory = stateEncoding;
        else
            gameStateHistory = stateEncoding + "|" + gameStateHistory;
    }
    public bool IsDeadPosition()
    {
        if (whitePlayer.IsKingOnBoard() && blackPlayer.IsKingOnBoard())
        {
            if (onBoardCount == 2) return true;
            if (onBoardCount == 3)
                return whitePlayer.IsKnightOnBoard() || blackPlayer.IsKnightOnBoard();
        }
        return false;
    }
    public bool IsSelfCheck(PlayerMove move, Player player)
    {
        UpdateBoard(move);
        bool selfCheck = player.IsInCheck(this);
        RevertBoard(move);
        return selfCheck;
    }
    public bool CapturedPieceExists()
    {
        return !(capturedPiece is EmptyPiece);
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
    public ChessPiece GetPiece(int row, int column)
    {
        if (row < 0 || row >= board.GetLength(0) || column < 0 || column >= board.GetLength(1))
            return new EmptyPiece();
        return board[row, column];
    }
    public bool MovePiece(int startRow, int startColumn, int endRow, int endColumn)
    {
        ChessPiece piece = GetPiece(startRow, startColumn);
        if (piece is EmptyPiece)
            return false;
        board[startRow, startColumn] = new EmptyPiece();
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
    public void UpdateBoard(PlayerMove move)
    {
        endPosition = GetPiece(move.GetEndRow(), move.GetEndColumn());
        capturedPiece = endPosition;
        MovePiece(move);
        ChessPiece movedPiece = GetPiece(move.GetEndRow(), move.GetEndColumn());
        movedPiece.UpdateAfterMove(move);
        // check if move is en passant
        if (movedPiece is Pawn && move.IsDiagonal() && endPosition is EmptyPiece)
            capturedPiece = GetPiece(move.GetStartRow(), move.GetEndColumn());
        // check if move is castling
        if (movedPiece is King && move.GetColumnDistance() == 2 && move.GetRowDistance() == 0)
        {
            bool kingside = move.GetEndColumn() > move.GetStartColumn();
            ChessPiece rook = movedPiece.GetPlayer().GetRook(kingside);
            int rookEndColumn = kingside? move.GetEndColumn() - 1 : move.GetEndColumn() + 1;
            PlayerMove rookMove = new PlayerMove(rook.GetCurrentRow(), rook.GetCurrentColumn(), rook.GetCurrentRow(), rookEndColumn);
            MovePiece(rookMove);
            rook.UpdateAfterMove(rookMove);
        }
        capturedPiece.SetCaptured(true);
    }
    public void RevertBoard(PlayerMove move)
    {
        ChessPiece movedPiece = GetPiece(move.GetEndRow(), move.GetEndColumn());
        PlacePiece(movedPiece, move.GetStartRow(), move.GetStartColumn());
        movedPiece.Revert(move);
        if (movedPiece is King && move.GetColumnDistance() == 2 && move.GetRowDistance() == 0)
        {
            // revert castling
            bool kingside = move.GetEndColumn() > move.GetStartColumn();
            ChessPiece rook = movedPiece.GetPlayer().GetRook(kingside);
            int rookColumn = kingside? 7 : 0;
            if (rook is Rook)
            {
                PlacePiece(rook, rook.GetCurrentRow(), rookColumn);
                rook.Revert(new PlayerMove(rook.GetCurrentRow(), rookColumn, rook.GetCurrentRow(), rook.GetCurrentColumn()));
            }
        }
        PlacePiece(endPosition, move.GetEndRow(), move.GetEndColumn());
        capturedPiece.SetCaptured(false);
    }
    public override string ToString()
    {
        string result = "   A  B  C  D  E  F  G  H\n";
        for (int row = board.GetLength(0) - 1; row >= 0; row--)
        {
            result += string.Format("{0}  ", row + 1);
            for (int col = 0; col < board.GetLength(1); col++)
            {
                result += board[row, col];
                result += " ";
            }
            result += '\n';
        }
        return result;
    }
    public string ToStateEncoding()
    {
        string result = whiteTurn? "w" : "b";
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                result += board[row, col];
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
    
    public bool IsInCheck(ChessGame game)
    {
        if (kingIndex == -1) return false;
        ChessPiece king = pieces[kingIndex];
        Player opponent = white? game.GetBlackPlayer() : game.GetWhitePlayer();
        return opponent.ThreatensPosition(king.GetCurrentRow(), king.GetCurrentColumn(), game);
    }
    public bool ThreatensPosition(int row, int column, ChessGame game)
    {
        for (int i = 0; i < pieceCount; i++)
        {
            ChessPiece piece = pieces[i];
            if (piece.IsCaptured()) continue;
            PlayerMove captureMove = new PlayerMove(piece.GetCurrentRow(), piece.GetCurrentColumn(), row, column);
            if (piece is Pawn)
            {
                if (((Pawn)piece).IsCapturingMove(captureMove, game))
                    return true;
            }
            else
                if (piece.IsLegalMove(captureMove, game))
                    return true;
        }
        return false;
    }
    public bool HasLegalMoves(ChessGame game)
    {
        for (int i = 0; i < pieceCount; i++)
        {
            if (!pieces[i].IsCaptured() && pieces[i].HasLegalMoves(game))
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
    public ChessPiece GetKingsideRook()
    {
        if (kingsideRookIndex == -1) return new EmptyPiece();
        return pieces[kingsideRookIndex];
    }
    public ChessPiece GetQueensideRook()
    {
        if (queensideRookIndex == -1) return new EmptyPiece();
        return pieces[queensideRookIndex];
    }
    public ChessPiece GetRook(bool kingside)
    {
        return kingside? GetKingsideRook() : GetQueensideRook();
    }
    public bool IsCastlingPossible(ChessGame game)
    {
        if (kingIndex == -1) return false;
        ChessPiece king = pieces[kingIndex];
        PlayerMove kingside = new PlayerMove(king.GetCurrentRow(), king.GetCurrentColumn(), king.GetCurrentRow(), king.GetCurrentColumn() + 2);
        PlayerMove queenside = new PlayerMove(king.GetCurrentRow(), king.GetCurrentColumn(), king.GetCurrentRow(), king.GetCurrentColumn() - 2);
        return IsCastlingPossible(game, kingside) || IsCastlingPossible(game, queenside);
    }
    public bool IsCastlingPossible(ChessGame game, PlayerMove kingMove)
    {
        if (kingIndex == -1) return false;
        King king = (King)pieces[kingIndex];
        if (!king.IsCastlingPossible()) return false;
        bool kingside = kingMove.GetEndColumn() > kingMove.GetStartColumn();
        ChessPiece rook = GetRook(kingside);
        if (rook is EmptyPiece) return false;
        if (rook.IsCaptured() || !((Rook)rook).IsCastlingPossible()) return false;
        PlayerMove pathBetween = new PlayerMove(king.GetCurrentRow(), king.GetCurrentColumn(), 
            rook.GetCurrentRow(), rook.GetCurrentColumn());
        if (!pathBetween.IsNotBlocked(game)) return false;
        Player opponent = white? game.GetBlackPlayer() : game.GetWhitePlayer();
        int column = kingMove.GetStartColumn();
        while ((kingside && column <= kingMove.GetEndColumn()) || (!kingside && column >= kingMove.GetEndColumn()))
        {
            if (opponent.ThreatensPosition(king.GetCurrentRow(), column, game))
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
    Position start, end;
    bool valid, drawRequest;
    public PlayerMove(int startRow, int startColumn, int endRow, int endColumn)
        : this(new Position(startRow, startColumn), new Position(endRow, endColumn))
    {
        
    }
    public PlayerMove(Position start, Position end)
    {
        this.start = start;
        this.end = end;
        valid = true;
        drawRequest = false;
    }
    PlayerMove(bool drawRequest) 
    {
        start = new Position();
        end = new Position();
        this.drawRequest = drawRequest;
        valid = false;
    }
    public bool IsValid()
    {
        return valid;
    }
    public bool IsDrawRequest()
    {
        return drawRequest;
    }
    public bool IsStationary()
    {
        return start.Equals(end);
    }
    
    public bool SetStartRow(int startRow)
    {
        return start.SetRow(startRow);
    }
    public int GetStartRow()
    {
        return start.GetRow();
    }
    public bool SetStartColumn(int startColumn)
    {
        return start.SetColumn(startColumn);
    }
    public int GetStartColumn()
    {
        return start.GetColumn();
    }
    public bool SetEndRow(int endRow)
    {
        return end.SetRow(endRow);
    }
    public int GetEndRow()
    {
        return end.GetRow();
    }
    public bool SetEndColumn(int endColumn)
    {
        return end.SetColumn(endColumn);
    }
    public int GetEndColumn()
    {
        return end.GetColumn();
    }
    public override string ToString()
    {
        return start.ToString() + end.ToString();
    }
    public int GetRowDistance()
    {
        int diff = end.GetRow() - start.GetRow();
        if (diff < 0)
            return -diff;
        return diff;
    }
    public int GetColumnDistance()
    {
        int diff = end.GetColumn() - start.GetColumn();
        if (diff < 0)
            return -diff;
        return diff;
    }
    public bool IsDiagonal()
    {
        return GetRowDistance() == GetColumnDistance();
    }
    public bool IsHorizontal()
    {
        return GetRowDistance() == 0 && GetColumnDistance() != 0;
    }
    public bool IsVertical()
    {
        return GetRowDistance() != 0 && GetColumnDistance() == 0;
    }
    public bool IsNotBlocked(ChessGame game)
    {
        int rowStep = start.GetRow() < end.GetRow()? 1 : -1;
        int colStep = start.GetColumn() < end.GetColumn()? 1 : -1;
        if (IsHorizontal())
            rowStep = 0;
        if (IsVertical())
            colStep = 0;
        Step step = new Step(rowStep, colStep);
        Position position = start.AddStep(step);
        while (!position.Equals(end))
        {
            ChessPiece piece = game.GetPiece(position.GetRow(), position.GetColumn());
            if (!(piece is EmptyPiece) && !piece.IsCaptured())
                return false;
            position = position.AddStep(step);
        }
        return true;
    }
    
    public static PlayerMove FromString(string moveString)
    {
        if (moveString.Length != 4)
            return new PlayerMove(false);
        if (moveString == "DRAW")
            return new PlayerMove(true);
        int startColumn = CharToColumn(moveString[0]);
        if (startColumn == -1)
            return new PlayerMove(false);
        int startRow = CharToRow(moveString[1]);
        if (startRow == -1)
            return new PlayerMove(false);
        int endColumn = CharToColumn(moveString[2]);
        if (endColumn == -1)
            return new PlayerMove(false);
        int endRow = CharToRow(moveString[3]);
        if (endRow == -1)
            return new PlayerMove(false);
        return new PlayerMove(startRow, startColumn, endRow, endColumn);
    }
    static int CharToColumn(char letter)
    {
        switch (letter)
        {
            case 'A': case 'a':
                return 0;
            case 'B': case 'b':
                return 1;
            case 'C': case 'c':
                return 2;
            case 'D': case 'd':
                return 3;
            case 'E': case 'e':
                return 4;
            case 'F': case 'f':
                return 5;
            case 'G': case 'g':
                return 6;
            case 'H': case 'h':
                return 7;
            default:
                break;
        }
        return -1;
    }
    static int CharToRow(char digit)
    {
        switch (digit)
        {
            case '1':
                return 0;
            case '2':
                return 1;
            case '3':
                return 2;
            case '4':
                return 3;
            case '5':
                return 4;
            case '6':
                return 5;
            case '7':
                return 6;
            case '8':
                return 7;
            default:
                break;
        }
        return -1;
    }
}
class Position
{
    int row, column;
    public Position() {}
    public Position(int row, int column)
    {
        SetRow(row);
        SetColumn(column);
    }
    public bool SetRow(int row)
    {
        if (row < 0 || row >= 8) return false;
        this.row = row;
        return true;
    }
    public bool SetColumn(int column)
    {
        if (column < 0 || column >= 8) return false;
        this.column = column;
        return true;
    }
    public int GetRow()
    {
        return row;
    }
    public int GetColumn()
    {
        return column;
    }
    public override string ToString()
    {
        string result = "";
        string columnChars = "ABCDEFGH";
        result += columnChars[column];
        result += (row + 1);
        return result;
    }
    public override bool Equals(object? obj)
    {
        if (!(obj is Position)) return false;
        Position other = (Position)obj;
        return row == other.row && column == other.column;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public bool IsValidStep(Step step)
    {
        int newRow = row + step.GetRowStep();
        int newCol = column + step.GetColumnStep();
        return newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8;
    }
    public Position AddStep(Step step)
    {
        int newRow = row + step.GetRowStep();
        int newCol = column + step.GetColumnStep();
        return new Position(newRow, newCol);
    }
    public Position Copy()
    {
        return new Position(row, column);
    }
}
class Step
{
    int rowStep, columnStep;
    public Step(int rowStep, int columnStep)
    {
        this.rowStep = rowStep;
        this.columnStep = columnStep;
    }
    public int GetRowStep()
    {
        return rowStep;
    }
    public int GetColumnStep()
    {
        return columnStep;
    }
}