using System.Text.RegularExpressions;
using UnityEngine;

public static class LevelLayoutParser
{
    public static bool GetLevelLayout(out bool[][] layout, out int fullTileAmount)
    {
        fullTileAmount = 0;
        string[] lines = ParseLayoutDocument();

        if (lines == null)
        {
            layout = null;
            return false;
        }

        layout = new bool[lines.Length + 2][];
        // All lines should be the same length, as we work with x*y squares.
        int lineLength = lines[0].Length + 2;

        // We go through all the lines in the document + 2 (first and last rows are empty)
        for (int i = 0; i < layout.Length; i++)
        {
            layout[i] = new bool[lineLength];

            // If first or last line
            if (i == 0 || i == layout.Length - 1)
            {
                // fill the matrix with empty tiles
                for (int j = 0; j < layout[i].Length; j++)
                    layout[i][j] = false;
            }
            else
            {
                // we go through all characters in the current line, + 2 (first and last columns are empty)
                for (int j = 0; j < layout[i].Length; j++)
                {
                    // If first or last character (do not exist in the document), we have an empty tile
                    if (j == 0 || j == layout[i].Length - 1)
                    {
                        layout[i][j] = false;
                    }
                    else
                    {
                        // Depending on the character, the current tile is either empty or not
                        switch (lines[i - 1][j - 1])
                        {
                            case '0':
                                layout[i][j] = false;
                                break;
                            case 'X':
                                layout[i][j] = true;
                                fullTileAmount++;
                                break;
                            default:
                                Debug.LogErrorFormat("The character '{0}' isn't recognized when handling layout file. Please change it to 0 or X.", lines[i - 1][j - 1]);
                                return false;
                        }
                    }
                }
            }
        }

        CheckForOddTileAmount(ref layout, ref fullTileAmount);
        return true;
    }

    private static void CheckForOddTileAmount(ref bool[][] layout, ref int fullTileAmount)
    {
        // Checking if we have an odd number of tiles
        if (fullTileAmount % 2 == 1)
        {
            // We remove one from the final amount
            fullTileAmount--;

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