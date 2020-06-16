using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Transactions;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            List<string> wordList = new List<string>();
            string[] subStrings;
            string move = "F";

            var i = 0;

            while (i <= 100000)
            {
                Console.WriteLine("Starting");
                Console.WriteLine("-------------");
                Console.WriteLine("Moving: {0}", move);
                subStrings = postMove(move).Split('\r');
                Console.WriteLine("Current Pos: {0}", subStrings[0]);
                Console.WriteLine("Available Moves: {0}", subStrings[1]);
                Console.WriteLine("Letter Found: {0}", subStrings[2]);
                if (subStrings[2].Length > 0) wordList.Add(subStrings[2]);
                Console.WriteLine("URL Found: {0}", subStrings[3]);
                Console.Write("FoundLetters: ");
                foreach (string s in wordList)
                    Console.Write(s);
                Console.WriteLine();
                move = parseMove(subStrings[1]);
            }
            move = Console.ReadLine();

        }
        


        public static string postMove(string move)
        {

            // Create a request for the URL.
            WebRequest request = WebRequest.Create(
              "http://ec2-3-16-22-54.us-east-2.compute.amazonaws.com/VirtualControl/Rooms/HACKATHON23/cws/hackathon/maze/NBDGD/navigate");

            request.Method = "PUT";

            string postData = move;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            // Display the status.

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            // Get the response.
            WebResponse response = request.GetResponse();
            string responseFromServer;
            using (dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
            }
            Console.WriteLine(responseFromServer);

            // Close the response.
            response.Close();
            return responseFromServer;
        }


        public static string parseMove(string value)
        {
            if (value.Contains("R"))
                return "R";
            if (value.Contains("F"))
                return "F";
            if (value.Contains("L"))
                return "L";
            return "B";
        }
    }
}
