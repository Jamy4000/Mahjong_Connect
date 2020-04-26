using System.Text.RegularExpressions;
using UnityEngine;

public static class LevelLayoutParser
{
    public static bool GetLevelLayout(out bool[][] layout)
    {
        GameManager.Instance.TileAmount = 0;
        string[] lines = ParseLayoutDocument();

        if (lines == null)
        {
            layout = null;
            return false;
        }

        // Basically how much char do we have per line, +2 for empty char
        int lineLength = lines[0].Length + 2;
        layout = new bool[lineLength][];

        // We go through all the char in a line + 2 to create columns (first and last columns are empty)
        for (int x = 0; x < lineLength; x++)
        {
            layout[x] = new bool[lines.Length + 2];

            // If first or last line
            if (x == 0 || x == lineLength - 1)
            {
                // fill the matrix with empty tiles
                for (int y = 0; y < layout[x].Length; y++)
                    layout[x][y] = false;
            }
            else
            {
                // we go through all characters in the current line, + 2 (first and last columns are empty)
                for (int y = 0; y < layout[x].Length; y++)
                {
                    // If first or last character (do not exist in the document), we have an empty tile
                    if (y == 0 || y == layout[x].Length - 1)
                    {
                        layout[x][y] = false;
                    }
                    else
                    {
                        // Depending on the character, the current tile is either empty or not
                        switch (lines[y - 1][x - 1])
                        {
                            case '0':
                                layout[x][y] = false;
                                break;
                            case 'X':
                                layout[x][y] = true;
                                GameManager.Instance.TileAmount++;
                                break;
                            default:
                                Debug.LogErrorFormat("The character '{0}' isn't recognized when handling layout file. Please change it to 0 or X.", lines[y - 1][x - 1]);
                                return false;
                        }
                    }
                }
            }
        }

        CheckForOddTileAmount(ref layout);
        return true;
    }

    private static void CheckForOddTileAmount(ref bool[][] layout)
    {
        // Checking if we have an odd number of tiles
        if (GameManager.Instance.TileAmount % 2 == 1)
        {
            // We remove one from the final amount
            GameManager.Instance.TileAmount--;

            // We go through the layout matrix and set the first available value to false. Starting from 1 as first row and columns are empty
            for (int i = 1; i < layout.Length - 1; i++)
            {
                for (int j = 1; j < layout[i].Length - 1; j++)
                {
                    if (layout[i][j])
                    {
                        layout[i][j] = false;
                        return;
                    }

                }
            }
        }
    }

    private static string[] ParseLayoutDocument()
    {
        // Fetch the layout file
        string pathToLayout = System.IO.Path.Combine("Layouts", GameManager.CurrentLevel.LayoutFileName);
        TextAsset txt = (TextAsset)Resources.Load(pathToLayout);

        // Split the file line by line using regular expressions
        var lines = Regex.Split(txt.text, "\r\n|\r|\n");

        return lines;
    }
}