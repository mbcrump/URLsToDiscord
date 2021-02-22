using System;
using System.Collections.Specialized;
using URLsToDiscord;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace UrlsToDiscord
{
    class Program
    {

        static async System.Threading.Tasks.Task Main(string[] args)
        {

            //test 
            //string path =
            //    @"C:\\Users\\micrum\\source\\repos\\URLsToDiscord\\URLsToDiscord\\mbcrump-twitch01172021.json";

            //prod
            string path = "/home/mbcrump/src/twitchjsonparser/twitchjsonparser/bin/Debug/netcoreapp3.1/mbcrump-twitch" + DateTime.Now.ToString("MMddyyyy") + ".json";
            if (!File.Exists(path))
            {
                Console.WriteLine("Exiting");
                Environment.Exit(0);
            }

            List<string> myList = new List<string>();
            int counter = 0;
            string line;
            string url = "your discord webhook";


            // Read the file and display it line by line.  
            StreamReader file = new StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                foreach (Match m in linkParser.Matches(line))
                    if (!m.Value.StartsWith("https://static-cdn") && !m.Value.StartsWith("https://www.youtube.com/user/mbcrump") && !m.Value.StartsWith("https://twitter.com/mbcrump") && !m.Value.StartsWith("https://youtube.com/mbcrump?sub_confirmation") && !m.Value.StartsWith("https://discord.gg/qrGrx8m"))
                    {
                        myList.Add(m.Value);
                        //Console.WriteLine(m.Value); //results += m.Value;
                    }

                counter++;
            }

            file.Close();

            string fileName = "latest.html";
            string currentContent = String.Empty;
            if (File.Exists(fileName))
            {
                currentContent = File.ReadAllText(fileName);
            }
            
            var noDupes = myList.Distinct().ToList();
            string output = String.Empty;
            // var result = noDupes.Aggregate((a, b) => a + ", " + b);
            output = "<h1>" + DateTime.Now.ToString("MMddyyyy") + "</h1>" + Environment.NewLine + Environment.NewLine;

            for (int i = 0; i < noDupes.Count; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(5000);
                sendWebHook(url, noDupes[i], "twitch-to-discord-bot");
                output = output + "<a href=\"" + noDupes[i] + "\">" + noDupes[i] + "</a>" + Environment.NewLine + "</br>";

            }

            output = output + Environment.NewLine;
            File.WriteAllText(fileName, output + currentContent);

            // upload to storage account
            var blobServiceClient = new BlobServiceClient("");
            var containerClient = blobServiceClient.GetBlobContainerClient("streamlinks");
            var blob = containerClient.GetBlobClient(fileName);
            await using var fileStream = System.IO.File.OpenRead(fileName);

            var blobHttpHeader = new BlobHttpHeaders();
            blobHttpHeader.ContentType = "text/html";

            var uploadedBlob = await blob.UploadAsync(fileStream, blobHttpHeader);

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