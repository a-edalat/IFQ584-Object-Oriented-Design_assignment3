namespace MultiGameApp; 

public enum GameType
{
    NUMERICAL_TIC_TAC_TOE,
    NOTAKTO,
    GOMOKU,
}

public enum GameMode
{
    HUMAN_VS_HUMAN,
    HUMAN_VS_COMPUTER,
}

public enum GameResult
{
    IN_PROGRESS,
    DRAW,
    PLAYER_ONE_WIN,
    PLAYER_TWO_WIN,
}

public enum CommandType
{
    MOVE,
    UNDO,
    REDO,
    SAVE,
    HELP,
}
