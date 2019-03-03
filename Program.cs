/*
    Author: Jason Ege
    
    Date: March 2, 2019

    Company: BitTitan, Inc.

    Description: Remove white spaces in the public folder provisioning scripts.
    
    Purpose: When setting up a large public folder migration (>20GB), scripts need to be provided to the customer so that they can
    complete their migrations without running into certain problems at the destination. However, the code that takes the CSV and
    converts it into a Powershell script does not remove white spaces at the end of the folder names. These white spaces cause problems
    for the migration. The purpose of this console application is to programmatically remove the white spaces from the public folder
    provisioning scripts before they are sent to the customers.

*/

using System;
using System.IO;
using System.Text;

namespace PF_White_Space_Removal
{
    class Program
    {
        // This function asks for a file, checks if the path exists, and then returns a file stream if so.
        static FileStream GetFileToRead()
        {
            // Start of loop to prompt the user repeatedly.
            while (true)
            {
                // Prompt the user for a file path
                Console.WriteLine("Enter the full path of the script to repair:");
                string sourceFilePath = Console.ReadLine();

                // Check the be sure that the files exist.
                if (File.Exists(sourceFilePath))
                {
                    //Return as a read-only file stream.
                    return new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read);
                }
                // If the file does not exist, say something.
                else if (File.Exists(sourceFilePath) == false)
                {
                    Console.WriteLine("File does not exist or could not be found!");
                }
            }
        }

        static void Main(string[] args)
        {
            //Set the title of the console window
            Console.Title = "Public Folder White Space Removal Utility";

            // Write our header to the window.
            Console.WriteLine(" ===========================================");
            Console.WriteLine("|                                           |");
            Console.WriteLine("| PUBLIC FOLDER WHITE SPACE REMOVAL UTILITY |");
            Console.WriteLine("|                                           |");
            Console.WriteLine(" ===========================================");
            Console.WriteLine("");

            // Acquire our input file
            FileStream sourceFile = GetFileToRead();

            // Acquire our output file
            string destinationFilePath = sourceFile.Name + "_Fixed.ps1";
            FileStream destinationFile = new FileStream(destinationFilePath, FileMode.Create, FileAccess.ReadWrite);
            string line;

            // Prepare stream reader for reading in the input file.
            StreamReader reader = new StreamReader(sourceFile);
            // Prepare stream writer for writing to the output file.
            StreamWriter writer = new StreamWriter(destinationFile, Encoding.UTF8);

            // Let the user know we are starting the process
            Console.WriteLine("The white space removal process has started - Do not close this window");

            // Beginning of main loop
            while (true)
            {
                // Read a line from the script file.
                line = reader.ReadLine();

                // If the line has no value (meaning the end of the file has been reached), break out of the loop.
                if (line == null)
                {
                    break;
                }

                // If nothing needs to be changed just write the line and move on.
                if (line.Contains(" \\") == false && line.Contains(" \" ") == false)
                {
                    writer.WriteLine(line);
                }

                // If the line contains a floating double quote.
                if (line.Contains(" \" "))
                {
                    // Remove the white spaces surrounding the double quote.
                    string newLine = line.Replace(" \" ", "\"");

                    // We still have to maintain a space between the parameter name and the double quote, so now we need to repair those:

                    // Pad the "Name" parameter
                    if (newLine.Contains("-Name\""))
                    {
                        newLine = newLine.Replace("-Name\"", "-Name \"");
                    }

                    // Pad the "Path" parameter
                    if (newLine.Contains("-Path\""))
                    {
                        newLine = newLine.Replace("-Path\"", "-Path \"");
                    }

                    // Pad the "Identity" parameter
                    if (newLine.Contains("-Identity\""))
                    {
                        newLine = newLine.Replace("-Identity\"", "-Identity \"");
                    }

                    //Write the new line to the file.
                    writer.WriteLine(newLine);
                }

                // If the line contains a white space before a backslash...
                if (line.Contains(" \\"))
                {
                    // Remove the white space
                    string newLine = line.Replace(" \\", "\\");
                    writer.WriteLine(newLine);
                }
            }

            //Close the reader and writers.
            writer.Close();
            reader.Close();

            // Let the user know we're done.
            Console.WriteLine("The operation has completed.");
            // Prompt for input before exiting.
            Console.ReadLine();
        }
    }
}
