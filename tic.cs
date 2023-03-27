using System;

namespace TicTacCheck
{
    class Program
    {
        static void Main (string[] args)
        {
            string userInput; //Receives de user's input.
            int userInputInt; //Receives the user's input (in case of it being a number) to be checked/flagged as a taken slot on the board.
            string checkedUserInput; //Receives de user's (one of the 2 players) after its been checked to be a number between 1 and 9.(also not picked before)
            int currentPlayer = 1; //Indicates which player is currently selecting.
            int currentPlayerIndicator = 2; //Current player indicator (is used with % to get 0 or 1, for player 1 and 2 respectively).
            string [,] boardAtPlay = MakeNewBoard(); //Initializing a board.
            int [] selectedSlots = new int [9]; //To keep track of the slots already selected.
                //Note that selectedSlots is being used as a "global array" of sorts.
            bool isGameOver = false;
     
            do 
            {
                Console.Clear();
                DisplayBoard(boardAtPlay);
                Console.WriteLine("\n'q;' to quit.\n'r;' to reset the board.\nPlayer {0} is selecting:", currentPlayer);

                userInput = Console.ReadLine();
                gameOverInput: //Tag for the end of a game.
                    if (userInput.Equals("r;")) //Input for resetting the game.
                    {
                        currentPlayer = 1;
                        currentPlayerIndicator = 0;
                        boardAtPlay = MakeNewBoard();
                        selectedSlots = new int [9];
                        continue;
                    }

                checkedUserInput = CheckInput(userInput, selectedSlots);
                if (checkedUserInput.Equals("Valid")) //Input to make a play.
                {
                    currentPlayerIndicator++;
                    currentPlayer = currentPlayerIndicator % 2 == 0 ? 1 : 2;
                    userInputInt = int.Parse(userInput);
                    selectedSlots[userInputInt-1] = 1;
                }
                
                boardAtPlay = ExecutePlay (userInput, boardAtPlay, currentPlayer); //Executes a valid play.
                isGameOver = CheckBoard (boardAtPlay); //Checks if there is a winner after a valid play.

                if (isGameOver == true)
                {
                    currentPlayer = currentPlayerIndicator % 2 == 0 ? 2 : 1;

                    while (true)
                    {
                        Console.Clear();
                        DisplayBoard(boardAtPlay);
                        Console.WriteLine("\nPlayer {0} is the winner!", currentPlayer);
                        Console.WriteLine("'q;' to quit.\n'r;' to reset the board.");
                        userInput = Console.ReadLine();

                        if (userInput.Equals("q;"))
                            break;
                        if (userInput.Equals("r;"))
                            goto gameOverInput; //Jumps to line 26.
                    }
                }

            } while (!userInput.Equals("q;"));
        }
        
        static string [,] ExecutePlay (string userInput, string [,] boardAtPlay, int currentPlayer)
        { //Receives a valid user input (between 1 and 9), the board currently at play and the current player (1 or 2).
            
            for (int i = 0; i < boardAtPlay.GetLength(0); i++)
            {
                for (int j = 0; j < boardAtPlay.GetLength(1); j++)
                {
                    if (boardAtPlay[i,j].Equals(userInput))
                        if (currentPlayer == 1) //currentPlayer = 1 actually means Player 2. (because of the order in lines 36,37).
                            boardAtPlay[i,j] = "O";
                        else
                            boardAtPlay[i,j] = "X";
                }
            }

            return (boardAtPlay); //Returns the board with X or O (depending on which player's turn it is) on the selected VALID slot.
                                    // This method is called only if the user input is valid.
        }

        static string CheckInput (string userInput, int [] selectedSlots)
        { //Checks if the user input is a number between 1 and 9.
            int userInputInt;
            int.TryParse(userInput, out userInputInt);
            if (userInputInt > 0 && userInputInt < 10 && selectedSlots[userInputInt-1] != 1)
                return "Valid";
            else 
                return "Invalid";
        }

        static void DisplayBoard (string [,] boardAtPlay)
        { //Displays the board with the slot numbers or symbols (X / O).
            for (int i = 0; i < boardAtPlay.GetLength(0); i++)
            {   
                if (i != 0)
                    Console.Write("\n");

                for (int j = 0; j < boardAtPlay.GetLength(1); j++)
                {
                    if (j == (boardAtPlay.GetLength(1)-1))
                        Console.Write(" "+boardAtPlay[i,j]);
                    else 
                        Console.Write(" "+boardAtPlay[i,j]+" |");
                }

                Console.Write("\n___|___|___");
            }
        }
        static string [,] MakeNewBoard ()
        { //Makes a new board with the numbers 1 through 9 in it's respective slots. 
            string [,] newBoard = 
            {
                {"1", "2", "3"},
                {"4", "5", "6"},
                {"7", "8", "9"}
            };

            return newBoard;
        }

        static bool CheckBoard (string [,] board)
        { //Checks if there is a winner.
            for (int i = 0; i < board.GetLength(0); i++)//Horizontal check
            {   
                int horizontalCount = 0;

                for (int j = 1; j < board.GetLength(1); j++)
                {
                    if (board[i,j-1].Equals(board[i,j]))
                        horizontalCount++;
                    if (horizontalCount == 2)
                        return true;
                }
            }
                   
            for (int j = 0; j < board.GetLength(1); j++)//Vertical check
            {
                int verticalCount = 0;

                for (int i = 1; i < board.GetLength(0); i++)
                {
                    if (board[i-1,j].Equals(board[i,j]))
                        verticalCount++;
                    if (verticalCount == 2)
                        return true;
                }
            }

            int diagonalCount = 0;
            for (int i = 1, j = 1; i < board.GetLength(0); i++, j++)//Diagonal check
            {
                if (board[i-1,j-1].Equals(board[i,j]))
                    diagonalCount++;
                if (diagonalCount == 2)
                    return true;
            }     

            int invertedDiagonalCount = 0;
            for (int i = 1, j = (board.GetLength(1))-2; i < board.GetLength(0); i++, j--)//Diagonal check (right to left)
            {
                if (board[i-1,j+1].Equals(board[i,j]))
                    invertedDiagonalCount++;
                if (invertedDiagonalCount == 2)
                    return true;
            }

            return false;
        }
    }
}