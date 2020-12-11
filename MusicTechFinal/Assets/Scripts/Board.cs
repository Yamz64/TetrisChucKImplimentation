using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    IEnumerator game_tick()
    {
        //shift the block position down and draw the current tetrimino if there is nothing below the tetrimino, if there is update consecutive blocked ticks
        if (!Collision(2) && block_position.y != board_data.GetLength(0)-1)
        {
            blocked_tick = max_blocked_tick;
            block_position = new Vector2Int(block_position.x, block_position.y + 1);
        }
        else
        {
            blocked_tick--;
        }

        if(blocked_tick == 0 || (quick_drop && Collision(2)) || (quick_drop && block_position.y == board_data.GetLength(0)))
        {
            UpdateData();
            block_position = new Vector2Int(0, 0);
            blocked_tick = max_blocked_tick;
            current_tetrimino = next_tetrimino;
            next_tetrimino = Random.Range(1, 8);
            DrawNext();
            CheckLevel();
            swapped = false;
        }

        DetectMatches();
        if (!quick_drop)
        {
            yield return new WaitForSeconds(game_tick_delay / level);
        }
        else
        {
            yield return new WaitForSeconds(game_tick_delay / block_drop_decrease);
        }
        StartCoroutine(game_tick());
    }

    Vector2Int Array2Tile(Vector2Int array)
    {
        return new Vector2Int(array.x, board_data.GetLength(0) - 1 - array.y);
    }

    Vector2Int Tile2Array(Vector2Int tile)
    {
        return new Vector2Int(tile.x, (board_data.GetLength(0) - 1) - tile.y);
    }

    bool Collision(int mode)
    {
        //left mode
        if(mode == 0)
        {
            //loop through all tile locations to check the areas beneath them
            List<Vector2Int> left = new List<Vector2Int>();
            for (int i = 0; i < tetrimino_positions.Count; i++)
            {
                Vector2Int pos = tetrimino_positions[i];
                left.Add(new Vector2Int(pos.x - 1, pos.y));
            }

            //see if beneath tiles are in the board data
            for (int i = 0; i < left.Count; i++)
            {
                Vector2Int array = Tile2Array(left[i]);
                if (array.x >= 0)
                {
                    if (board_data[array.y, array.x] != 0)
                    {
                        Debug.Log("Colliding Left");
                        return true;
                    }
                }
            }
        }
        //right mode
        else if(mode == 1)
        {
            //loop through all tile locations to check the areas beneath them
            List<Vector2Int> right = new List<Vector2Int>();
            for (int i = 0; i < tetrimino_positions.Count; i++)
            {
                Vector2Int pos = tetrimino_positions[i];
                right.Add(new Vector2Int(pos.x + 1, pos.y));
            }

            //see if beneath tiles are in the board data
            for (int i = 0; i < right.Count; i++)
            {
                Vector2Int array = Tile2Array(right[i]);
                if (array.x <= board_data.GetLength(1) - 1)
                {
                    if (board_data[array.y, array.x] != 0)
                    {
                        Debug.Log("Colliding Right");
                        return true;
                    }
                }
            }
        }
        //down mode
        else if(mode == 2)
        {
            //loop through all tile locations to check the areas beneath them
            List<Vector2Int> beneath = new List<Vector2Int>();
            for (int i = 0; i < tetrimino_positions.Count; i++)
            {
                Vector2Int pos = tetrimino_positions[i];
                beneath.Add(new Vector2Int(pos.x, pos.y - 1));
            }

            //see if beneath tiles are in the board data
            for (int i = 0; i < beneath.Count; i++)
            {
                Vector2Int array = Tile2Array(beneath[i]);
                if (array.y <= board_data.GetLength(0) - 1)
                {
                    if (board_data[array.y, array.x] != 0)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void CheckLevel()
    {
        int i = score % 1000;
        int i2 = score - i + 1;
        level = i2 / 1000 + 1;
        level_text.text = level.ToString();
    }

    void Hold()
    {
        if (!swapped)
        {
            if (held_tetrimino == -1)
            {
                block_position = new Vector2Int(block_position.x, 0);
                held_tetrimino = current_tetrimino;
                current_tetrimino = next_tetrimino;
                next_tetrimino = Random.Range(1, 8);

                //clear previous held
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        board.SetTile(new Vector3Int(-6 + x, 18 - y, 0), null);
                    }
                }

                //draw held
                for (int y = 0; y < tetriminos[held_tetrimino - 1].GetData().GetLength(0); y++)
                {
                    for (int x = 0; x < tetriminos[held_tetrimino - 1].GetData().GetLength(1); x++)
                    {
                        if (tetriminos[held_tetrimino - 1].GetData()[y, x] != 0)
                            board.SetTile(new Vector3Int(-6 + x, 18 - y, 0), tetrimino_tiles[held_tetrimino]);
                    }
                }
            }
            else
            {
                block_position = new Vector2Int(block_position.x, 0);
                int temp = held_tetrimino;
                held_tetrimino = current_tetrimino;
                current_tetrimino = temp;

                //clear previous held
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        board.SetTile(new Vector3Int(-6 + x, 18 - y, 0), null);
                    }
                }

                //draw held
                for (int y = 0; y < tetriminos[held_tetrimino - 1].GetData().GetLength(0); y++)
                {
                    for (int x = 0; x < tetriminos[held_tetrimino - 1].GetData().GetLength(1); x++)
                    {
                        if (tetriminos[held_tetrimino - 1].GetData()[y, x] != 0)
                            board.SetTile(new Vector3Int(-6 + x, 18 - y, 0), tetrimino_tiles[held_tetrimino]);
                    }
                }
            }
        }
        swapped = true;
    }

    void DetectMatches()
    {
        //first check to see if there are any rows of blocks
        bool found_row = true;
        int multiplier = 0;
        List<long> instruments = new List<long>();
        List<long> midi_scores = new List<long>();
        while (found_row == true)
        {
            found_row = false;
            int row_y_pos = 0;

            for (int y = 0; y < board_data.GetLength(0); y++)
            {
                for (int x = 0; x < board_data.GetLength(1); x++)
                {
                    if (board_data[y, x] != 0)
                    {
                        //found a row of blocks!
                        if (x == board_data.GetLength(1) - 1)
                        {
                            found_row = true;
                            row_y_pos = y;
                        }
                        //found a possible row of blocks
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //if a row of blocks was found clear the row and shift everything above that row down
            if (found_row)
            {
                multiplier++;
                int instrument = 19 - row_y_pos;
                int midi_score = 0;
                for (int x = 0; x < board_data.GetLength(1); x++)
                {
                    midi_score += board_data[row_y_pos, x];
                    board_data[row_y_pos, x] = 0;
                    Vector2Int tile = Array2Tile(new Vector2Int(x, row_y_pos));
                    board.SetTile((Vector3Int)tile, null);
                }
                midi_scores.Add(midi_score);
                instruments.Add(instrument);
                for (int y = row_y_pos - 1; y > -1; y--)
                {
                    for (int x = 0; x < board_data.GetLength(1); x++)
                    {
                        board_data[y + 1, x] = board_data[y, x];
                        Vector2Int above = Array2Tile(new Vector2Int(x, y + 1));
                        Vector2Int current = Array2Tile(new Vector2Int(x, y));
                        board.SetTile((Vector3Int)above, board.GetTile((Vector3Int)current));
                        board_data[y, x] = 0;
                        board.SetTile((Vector3Int)current, null);
                    }
                }
            }
        }
        score += 100 * multiplier;
        score_text.text = score.ToString();

        //handle sound info
        long[] instrument_data = new long[instruments.Count];
        long[] midi_data = new long[midi_scores.Count];

        for(int i=0; i<instrument_data.GetLength(0); i++)
        {
            instrument_data[i] = instruments[i];
            midi_data[i] = midi_scores[i];
        }
        if(instrument_data.GetLength(0) > 0 && midi_data.GetLength(0) > 0) chuck.PlayAudio(instrument_data, midi_data);
    }

    void UpdateData()
    {
        for(int i=0; i<tetrimino_positions.Count; i++)
        {
            Vector2Int array = Tile2Array(tetrimino_positions[i]);
            board_data[array.y, array.x] = current_tetrimino;
        }
        tetrimino_positions.Clear();
    }

    void ClearTetrimino()
    {
        for(int i=0; i<tetrimino_positions.Count; i++)
        {
            board.SetTile((Vector3Int)tetrimino_positions[i], null);
        }
        tetrimino_positions.Clear();
    }

    void DrawTetrimino()
    {
        ClearTetrimino();
        Tetrimino temp_tetrimino = new Tetrimino(tetriminos[current_tetrimino-1]);
        //rotate tetrimino
        int direction_count = 0;
        while (direction_count != direction)
        {
            if (direction_count < direction)
            {
                temp_tetrimino.Rotate(true);
                direction_count++;
            }
            else if (direction_count > direction)
            {
                temp_tetrimino.Rotate(false);
                direction_count--;
            }
        }
        //determine the actual bounds of the tetrimino
        int top_bound = 0;
        int bottom_bound = 0;
        int left_bound = 0;
        int right_bound = 0;

        //loop to find the top bound
        for(int y=0; y<temp_tetrimino.GetData().GetLength(0); y++)
        {
            bool valid = true;
            for(int x=0; x<temp_tetrimino.GetData().GetLength(1); x++)
            {
                if (temp_tetrimino.GetData()[y, x] == 0)
                {
                    if (x == temp_tetrimino.GetData().GetLength(1) - 1)
                    {
                        break;
                    }
                    continue;
                }
                else
                {
                    valid = false;
                    top_bound = y;
                    break;
                }
            }
            if (!valid) break;
        }

        //loop to find the bottom bound
        for(int y=temp_tetrimino.GetData().GetLength(1)-1; y > -1; y--)
        {
            bool valid = true;
            for (int x = 0; x < temp_tetrimino.GetData().GetLength(1); x++)
            {
                if (temp_tetrimino.GetData()[y, x] == 0)
                {
                    if (x == temp_tetrimino.GetData().GetLength(1) - 1)
                    {
                        break;
                    }
                    continue;
                }
                else
                {
                    valid = false;
                    bottom_bound = y;
                    break;
                }
            }
            if (!valid) break;
        }

        //loop to find the left bound
        for (int x = 0; x < temp_tetrimino.GetData().GetLength(1); x++)
        {
            bool valid = true;
            for (int y = 0; y < temp_tetrimino.GetData().GetLength(0); y++)
            {
                if (temp_tetrimino.GetData()[y, x] == 0)
                {
                    if (y == temp_tetrimino.GetData().GetLength(0) - 1)
                    {
                        break;
                    }
                    continue;
                }
                else
                {
                    valid = false;
                    left_bound = x;
                    break;
                }
            }
            if (!valid) break;
        }

        //loop to find the right bound
        for (int x = temp_tetrimino.GetData().GetLength(1) - 1; x > -1; x--)
        {
            bool valid = true;
            for (int y = 0; y < temp_tetrimino.GetData().GetLength(0); y++)
            {
                if (temp_tetrimino.GetData()[y,x] == 0)
                {
                    if (y == temp_tetrimino.GetData().GetLength(0) - 1)
                    {
                        break;
                    }
                    continue;
                }
                else
                {
                    valid = false;
                    right_bound = x;
                    break;
                }
            }
            if (!valid) break;
        }

        //set the true x of the block position
        block_position = new Vector2Int(Mathf.Clamp(block_position.x, 0, board_data.GetLength(1) - 1 - (right_bound - left_bound)), Mathf.Clamp(block_position.y, 0+bottom_bound, board_data.GetLength(0) - 1));

        //print the block at the correct position
        for(int y=top_bound; y<bottom_bound + 1; y++)
        {
            for(int x=left_bound; x<right_bound + 1; x++)
            {
                if (temp_tetrimino.GetData()[y, x] == 0) continue;
                Vector2Int tile_pos = Array2Tile(new Vector2Int(x - left_bound + block_position.x, y - bottom_bound + block_position.y));
                board.SetTile(new Vector3Int(tile_pos.x, tile_pos.y, 0), tetrimino_tiles[temp_tetrimino.GetData()[y, x]]);
                tetrimino_positions.Add(tile_pos);
            }
        }
    }

    void DrawNext()
    {
        //clear previous next
        for(int y=0; y < 4; y++)
        {
            for(int x=0; x<4; x++)
            {
                board.SetTile(new Vector3Int(14 + x, 17 - y, 0), null);
            }
        }

        //draw next
        for (int y = 0; y < tetriminos[next_tetrimino-1].GetData().GetLength(0); y++)
        {
            for (int x = 0; x < tetriminos[next_tetrimino-1].GetData().GetLength(1); x++)
            {
                if(tetriminos[next_tetrimino-1].GetData()[y,x] != 0)
                board.SetTile(new Vector3Int(14 + x, 17-y, 0), tetrimino_tiles[next_tetrimino]);
            }
        }
    }

    public bool swapped;
    public int score;
    public int level;
    public int blocked_tick;
    public int current_tetrimino;
    public int next_tetrimino;
    public int held_tetrimino;
    public int direction;
    public float game_tick_delay;
    public float block_drop_decrease;
    public Vector2Int block_position;
    public Text score_text;
    public Text level_text;
    public ChuckAudioHandler chuck;

    [SerializeField]
    public List<Tile> tetrimino_tiles;

    private bool quick_drop;
    private int max_blocked_tick;
    private int[,] board_data;
    private List<Tetrimino> tetriminos;
    private List<Vector2Int> tetrimino_positions;
    private Tilemap board;
    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        level_text.text = level.ToString();
        max_blocked_tick = blocked_tick;
        current_tetrimino = Random.Range(1, 8);
        next_tetrimino = Random.Range(1, 8);
        tetrimino_positions = new List<Vector2Int>();
        //initialize the board data
        board_data = new int[20, 10];

        //populate a list of tetriminos for use in the game
        tetriminos = new List<Tetrimino>();
        Tetrimino i_piece = new Tetrimino(new int[,]
        {
            {0,0,1,0},
            {0,0,1,0},
            {0,0,1,0},
            {0,0,1,0}
        });

        Tetrimino j_piece = new Tetrimino(new int[,]
        {
            {0,2,0},
            {0,2,0},
            {2,2,0}
        });

        Tetrimino l_piece = new Tetrimino(new int[,]
        {
            {0,3,0},
            {0,3,0},
            {0,3,3}
        });

        Tetrimino o_piece = new Tetrimino(new int[,]
        {
            {4,4},
            {4,4}
        });

        Tetrimino s_piece = new Tetrimino(new int[,]
        {
            {0,0,0},
            {0,5,5},
            {5,5,0}
        });

        Tetrimino t_piece = new Tetrimino(new int[,] 
        {
            {0,0,0},
            {0,6,0},
            {6,6,6}
        });

        Tetrimino z_piece = new Tetrimino(new int[,]
        {
            {0,0,0},
            {7,7,0},
            {0,7,7}
        });

        tetriminos.Add(i_piece);
        tetriminos.Add(j_piece);
        tetriminos.Add(l_piece);
        tetriminos.Add(o_piece);
        tetriminos.Add(s_piece);
        tetriminos.Add(t_piece);
        tetriminos.Add(z_piece);

        board = transform.GetChild(0).GetComponent<Tilemap>();
        DrawNext();
        StartCoroutine(game_tick());
    }

    private void Update()
    {
        DrawTetrimino();
        if (Input.GetKeyDown(KeyCode.RightArrow) && !Collision(1)) block_position = new Vector2Int(block_position.x + 1, block_position.y);
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !Collision(0)) block_position = new Vector2Int(block_position.x - 1, block_position.y);
        if (Input.GetKeyDown(KeyCode.Z) && !Collision(0))
        {
            direction--;
            while (Collision(2))
            {
                block_position = new Vector2Int(block_position.x, block_position.y - 1);
                DrawTetrimino();
            }
        }
        if (Input.GetKeyDown(KeyCode.X) && !Collision(1))
        {
            direction++;
            while (Collision(2))
            {
                block_position = new Vector2Int(block_position.x, block_position.y - 1);
                DrawTetrimino();
            }
        }
        if (Input.GetKeyDown(KeyCode.C)) Hold();
        quick_drop = Input.GetKey(KeyCode.DownArrow);
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StopAllCoroutines();
            StartCoroutine(game_tick());
        }
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
}
