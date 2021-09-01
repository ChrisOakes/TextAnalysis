using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextFrequencyAnalysis.Interfaces;
using TextFrequencyAnalysis.Models;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Text;

namespace TextFrequencyAnalysis.Services
{
    public class TextAnalysis : ITextAnalysis
    {
        private ILogger<TextAnalysis> _logger;

        public TextAnalysis(ILogger<TextAnalysis> logger)
        {
            _logger = logger;
        }
        public async Task<string> ConvertToText(string fileContents)
        {
            try
            {
                //First lets get a byte array that we can then conver to an ASCII string
                Byte[] byteArray = GenerateByteArray(fileContents);
                fileContents = "";
                fileContents = Encoding.ASCII.GetString(byteArray);

                if (fileContents.Length == 0)
                {
                    throw new Exception("The file contents contain no information");
                }

                return fileContents;

            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }

        public Byte[] GenerateByteArray(string binary)
        {
            try
            {
                //Create a Byte list that we will populate based off of the binary data
                var list = new List<Byte>();

                binary = binary.Replace(" ", "");

                //We know that binary is comes int lengths of 8, so we can analyse the file for numbers 
                for (int i = 0; i < binary.Length; i += 8)
                {
                    string a = binary.Substring(i, 8);
                    list.Add(Convert.ToByte(a, 2));
                }

                return list.ToArray();
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }

        public async Task<bool> DetermineBinary(string fileContents)
        {
            try
            {
                int letterCount = Regex.Matches(fileContents, @"[a-zA-Z]").Count;

                if (letterCount == 0)
                {
                    //This means that it contains no letters and is thus it could be binary
                    //Lets first check though that it only contains 0s and 1s

                    int numberCount = Regex.Matches(fileContents, @"[0-1]").Count;

                    if (numberCount == 0)
                    {
                        //the file cannot be processed
                        throw new Exception("The file cannot be processed");
                    }

                    return true;
                } else
                {
                    //This means that it is text already
                    return false;
                }
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }

        public async Task<bool> DetermineFileExist(string fileLocation)
        {
            try
            {
                bool exists = false;

                if (File.Exists(fileLocation))
                {
                    exists = true;
                }

                return exists;
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }

        public async Task<List<t_analysis>> GetWordHierarchy(string fileLocation)
        {
            try
            {
                List<t_analysis> li_t_analysis = new List<t_analysis>();
                //First lets look if the provided file location yields an actual file
                bool exists = await DetermineFileExist(fileLocation);

                if (!exists)
                {
                    throw new Exception("The file does not exists");
                }

                //Now we can get the file contents that we need
                string fileContents = await ReadFile(fileLocation);


                if (fileContents.Length == 0)
                {
                    //There is nothing in the file so we do not need to progress
                    throw new Exception("The file is empty");
                }

                //Now lets determine if the file has text in it or if it is binary
                bool isBinary = await DetermineBinary(fileContents);

                if (isBinary)
                {
                    //We now need to convert this into text
                    fileContents = await ConvertToText(fileContents);
                }

                //Now it should no longer be Binary so lets check
                isBinary = await DetermineBinary(fileContents);

                if (isBinary)
                {
                    throw new Exception("The file still does not contain letters/words for us to analyse");
                }

                string[] wordArray = GetWordArray(fileContents);

                if (wordArray.Count() == 0)
                {
                    throw new Exception("The word array is empty");
                }

                var word_groupings = wordArray.ToList().ConvertAll(x => x.ToLower()).GroupBy(x => x).ToList();

                foreach(var item in word_groupings.OrderByDescending(x => x.Count()))
                {
                    t_analysis t_analysis = new t_analysis();
                    t_analysis.Frequency = item.Count();
                    t_analysis.Word = item.Key;

                    li_t_analysis.Add(t_analysis);

                    if (li_t_analysis.Count == 20)
                    {
                        //We now have our complete list
                        
                        return li_t_analysis;
                    }
                }

                return null;

            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }

        public async Task<string> ReadFile(string fileLocation)
        {
            try
            {
                string fileContents = "";

                //If we get here, we know that the file exists
                //so we can simply read it
                fileContents = await File.ReadAllTextAsync(fileLocation);

                return fileContents;
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }

        public string[] GetWordArray(string fileContents)
        {
            try
            {
                //Lets get rid of any punctuation and replace it with spaces
                var sb = new StringBuilder();

                foreach (char c in fileContents)
                {
                    if (!char.IsPunctuation(c))
                    {
                        sb.Append(c);
                    } else
                    {
                        sb.Append(" ");
                    }
                }

                fileContents = sb.ToString();

                fileContents = fileContents.Replace("\n", " ");

                //first lets convert everything between spaces into an array
                string[] textArray = fileContents.Split(" ");

                //now that we have a list of all of our words, lets make sure that we only contain words in our array
                string[] wordArray = textArray.Where(x => Regex.IsMatch(x, @"[a-zA-Z \n* ^$]")).ToArray();

                return wordArray;
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }

        public async Task DeleteFile(string fileLocation)
        {
            try
            {
                if (await DetermineFileExist(fileLocation))
                {
                    File.Delete(fileLocation);
                }
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }
    }
}
