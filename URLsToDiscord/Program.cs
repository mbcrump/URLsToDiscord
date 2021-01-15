using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class UrlsToDiscord
{
    public static void Main()
	{

        string path = @"C:\Users\micrum\source\repos\URLsToDiscord\URLsToDiscord\mbcrump-twitch01102021.json";
        List<string> myList = new List<string>();
        int counter = 0;
        string line;

        // Read the file and display it line by line.  
        StreamReader file = new StreamReader(path);
        while ((line = file.ReadLine()) != null)
        {
            var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            
            foreach (Match m in linkParser.Matches(line))
                if (!m.Value.StartsWith("https://static-cdn") && !m.Value.StartsWith("https://www.youtube.com/user/mbcrump") && !m.Value.StartsWith("https://youtube.com/mbcrump?sub_confirmation") && !m.Value.StartsWith("https://discord.gg/qrGrx8m"))
                {
                    myList.Add(m.Value);
                    //results += m.Value;
                }
                
            counter++;
        }

        file.Close();
        var noDupes = myList.Distinct().ToList();
        noDupes.ForEach(Console.WriteLine);
    }
}