using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Class helper to parse the txt files containing the layout for the current level
/// </summary>
public static class LevelLayoutParser
{
    /// <summary>
    /// Fetch the layout for the current level.
    /// </summary>
    /// <param name="layout">The layout we for the current level</param>
    /// <returns>True if we could parse the txt file correctly</returns>
    public static bool GetLevelLayout()
    {
        string[] lines = ParseLayoutDocument();
        bool[][] layout;

        if (lines == null)
            return false;

        try
        {
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
                                    GameManager.CurrentLevel.BaseTileAmount++;
                                    break;
                                default:
                                    Debug.LogErrorFormat("The character '{0}' isn't recognized when handling layout file. Please change it to 0 or X.", lines[y - 1][x - 1]);
                                    return false;
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception e) 
        {
            Debug.LogErrorFormat("An error has occured when parsing the layout with name {0}. " +
                "Be sure that it has the same amount of character on every lines. Error is as follow: \n{2}", GameManager.CurrentLevel.LayoutFileName, e.ToString());
            return false;
        }

        CheckForOddTileAmount(ref layout);
        GameManager.CurrentLevel.LevelLayout = layout;
        return true;
    }

    private static void CheckForOddTileAmount(ref bool[][] layout)
    {
        // Checking if we have an odd number of tiles
        if (GameManager.CurrentLevel.BaseTileAmount % 2 == 1)
        {
            // We remove one from the final amount
            GameManager.CurrentLevel.BaseTileAmount--;

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

    /// <summary>
    /// Parse the layout document
    /// </summary>
    /// <returns>Return an array containing all the lines</returns>
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