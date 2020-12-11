using System.Collections;
using System.Collections.Generic;

public class Tetrimino
{
    private int[,] data;

    //basic constructor handles creation of tetrimino
    public Tetrimino(int[,] d)
    {
        data = d;
    }

    //copy constructor
    public Tetrimino(Tetrimino t)
    {
        data = t.data;
    }

    public int[,] GetData() { return data; }

    //function that handles rotation of a tetrimino block, false is clockwise, true is counterclockwise
    public void Rotate(bool direction)
    {
        int[,] temp = new int[data.GetLength(0), data.GetLength(1)];
        if (!direction)
        {
            //first flip along the diagonal
            for (int y = 0; y < temp.GetLength(0); y++)
            {
                for (int x = 0; x < temp.GetLength(1); x++)
                {
                    int value_1 = data[y, x];
                    int value_2 = data[x, y];
                    temp[y, x] = value_2;
                    temp[x, y] = value_1;
                }
            }
        }
        else
        {
            for (int y = 0; y < temp.GetLength(0); y++)
            {
                for (int x = 0; x < temp.GetLength(1); x++)
                {
                    int value_1 = data[temp.GetLength(0) - 1 - y, temp.GetLength(1) - 1 - x];
                    int value_2 = data[temp.GetLength(1) - 1 - x, temp.GetLength(0) - 1 - y];
                    temp[y, x] = value_2;
                    temp[x, y] = value_1;
                }
            }
        }
        //then flip horizontally
        for (int y = 0; y < temp.GetLength(0); y++)
        {
            for (int x = 0; x < temp.GetLength(1) / 2; x++)
            {
                int value_1 = temp[y, x];
                int value_2 = temp[y, temp.GetLength(1) - 1 - x];
                temp[y, x] = value_2;
                temp[y, temp.GetLength(1) - 1 - x] = value_1;
            }
        }
        data = temp;
    }
}
