using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextFrequencyAnalysis.Models;

namespace TextFrequencyAnalysis.Interfaces
{
    public interface ITextAnalysis
    {
        //Test written
        public Task<bool> DetermineFileExist(string fileLocation);
        //Test written
        public Task<List<t_analysis>> GetWordHierarchy(string fileLocation);
        //Test written
        public Task<string> ConvertToText(string fileContents);
        //Test written
        public Task<bool> DetermineBinary(string fileContents);
        //Test written
        public Task<string> ReadFile(string fileLocation);
        //Test written
        public Byte[] GenerateByteArray(string binary);
        //Test written
        public string[] GetWordArray(string fileContents);
        //Test writt
        public Task DeleteFile(string fileLocation);

    }
}
