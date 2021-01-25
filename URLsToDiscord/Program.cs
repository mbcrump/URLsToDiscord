using System;
using System.Collections.Specialized;
using URLsToDiscord;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace UrlsToDiscord
{
    class Program
    {
       
        static void Main(string[] args)
        {
            string path = @"/home/mbcrump/src/twitchjsonparser/twitchjsonparser/bin/Debug/netcoreapp3.1/mbcrump-twitch" + DateTime.Now.ToString("MMddyyyy") + ".json";
            if (!File.Exists(path))
            {
                Environment.Exit(0);
                //Console.WriteLine("The file exists.");
            }

            List<string> myList = new List<string>();
            int counter = 0;
            string line;
            string url = "put your webhook here";


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

            for (int i = 0; i < noDupes.Count; i++)
            {
                sendWebHook(url, noDupes[i], "twitch-to-discord-bot");
            }
        }

        public static void sendWebHook(string URL, string msg, string username)
        {
            Http.Post(URL, new NameValueCollection()
            {
                { "username", username },
                { "content", msg }
            });
        }

    }
}