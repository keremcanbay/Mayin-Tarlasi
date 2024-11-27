using System;

class Program
{
    static void Main(string[] args)
    {
        do
        {
            PlayGame();
            Console.WriteLine("\nYeniden başlamak için 'R' tuşuna basın. Çıkmak için başka bir tuşa basın...");
        } while (Console.ReadKey().Key == ConsoleKey.R);
    }

    static void PlayGame()
    {
        int rows = 20; // Harita boyutu: 20x20
        int cols = 20;
        int mineCount = 123; // Mayın sayısı
        char[,] board = new char[rows, cols];
        bool[,] mines = new bool[rows, cols];
        Random rnd = new Random();

        // Mayınları rastgele yerleştir
        for (int i = 0; i < mineCount; i++)
        {
            int r, c;
            do
            {
                r = rnd.Next(rows);
                c = rnd.Next(cols);
            } while (mines[r, c]);
            mines[r, c] = true;
        }

        // Tahtayı boş olarak başlat
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                board[r, c] = '-';
            }
        }

        bool gameOver = false;
        int safeCells = rows * cols - mineCount;

        while (!gameOver)
        {
            Console.Clear();
            PrintBoard(board);
            Console.Write("Satır (0-19): ");
            int row = int.Parse(Console.ReadLine());
            Console.Write("Sütun (0-19): ");
            int col = int.Parse(Console.ReadLine());

            if (row < 0 || row >= rows || col < 0 || col >= cols)
            {
                Console.WriteLine("Geçersiz seçim! Tekrar deneyin.");
                continue;
            }

            if (mines[row, col])
            {
                Console.Clear();
                Console.WriteLine("Mayına bastınız! Oyun bitti.");
                gameOver = true;
            }
            else
            {
                board[row, col] = CountAdjacentMines(mines, row, col).ToString()[0];
                safeCells--;
                if (safeCells == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Tebrikler! Tüm güvenli hücreleri buldunuz!");
                    gameOver = true;
                }
            }
        }

        Console.WriteLine("\nOyun sonu. Tahtadaki mayınlar:");
        RevealMines(board, mines);
        PrintBoard(board);
    }

    static void PrintBoard(char[,] board)
    {
        for (int r = 0; r < board.GetLength(0); r++)
        {
            for (int c = 0; c < board.GetLength(1); c++)
            {
                Console.Write(board[r, c] + " ");
            }
            Console.WriteLine();
        }
    }

    static int CountAdjacentMines(bool[,] mines, int row, int col)
    {
        int count = 0;
        int[] directions = { -1, 0, 1 };

        foreach (int dr in directions)
        {
            foreach (int dc in directions)
            {
                if (dr == 0 && dc == 0) continue;

                int newRow = row + dr;
                int newCol = col + dc;

                if (newRow >= 0 && newRow < mines.GetLength(0) && newCol >= 0 && newCol < mines.GetLength(1))
                {
                    if (mines[newRow, newCol]) count++;
                }
            }
        }

        return count;
    }

    static void RevealMines(char[,] board, bool[,] mines)
    {
        for (int r = 0; r < mines.GetLength(0); r++)
        {
            for (int c = 0; c < mines.GetLength(1); c++)
            {
                if (mines[r, c])
                {
                    board[r, c] = '*';
                }
            }
        }
    }
}
