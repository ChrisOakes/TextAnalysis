using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TextFrequencyAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Xunit;
using TextFrequencyAnalysis.Models;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;   

namespace TextFrequencyAnalysis_Test
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        public UnitTest1(WebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateDefaultClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44370/api/");
        }

        [Fact]
        public void Test1()
        {

        }

        [Theory]
        [InlineData("mobydick-test.txt", true)]
        [InlineData("asda.txt", false)]
        public async Task DetermineFileExists(string fileLocation, bool expectedOutcome)
        {
            //arranges
            //act
            var response = await _httpClient.GetAsync($"TextAnalysis/DetermineFileExists/{fileLocation}");
            var content = response.Content.ReadAsStringAsync();
            var result = Convert.ToBoolean(content.Result);
            //assert

            Assert.Equal(expectedOutcome, result);
        }
        [Theory]
        [InlineData("mobydick-test.txt", false)]
        [InlineData("binarymoby-test.txt", true)]
        public async Task DetermineFileIsBinary(string fileLocation, bool expectedOutcome)
        {
            //arrange
            //act
            var response = await _httpClient.GetAsync($"TextAnalysis/DetermineBinary/{fileLocation}");
            var content = response.Content.ReadAsStringAsync();
            var result = Convert.ToBoolean(content.Result);
            //assert

            Assert.Equal(expectedOutcome, result);
        }

        [Theory]
        [InlineData("mobydick-test.txt", true)]
        [InlineData("asda.txt", false)]
        public async Task ReadfileReturnsOK(string fileLocation, bool expectedOutcome)
        {
            //arrange
            //act
            var response = await _httpClient.GetAsync($"TextAnalysis/ReadFile/{fileLocation}");
            var content = response.Content.ReadAsStringAsync();
            var result = Convert.ToBoolean(content.Result);
            //assert

            Assert.Equal(expectedOutcome, result);
        }
        [Fact]
        public async Task GenerateByteArray()
        {
            //arrange
            //act
            var response = await _httpClient.GetAsync($"TextAnalysis/GenerateByteArray/binarymoby-test.txt");
            var content = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Byte[]>(content.Result);
            //assert

            Assert.IsType<Byte[]>(result);
            Assert.True(result.Count() > 0);
        }
        [Fact]
        public async Task ConvertBinaryToText()
        {
            //arrange
            //act
            var response = await _httpClient.GetAsync($"TextAnalysis/ConvertToText/binarymoby-test.txt");
            var content = response.Content.ReadAsStringAsync();
            var result = content.Result;
            //assert
            Assert.True(Regex.Matches(result, @"[a-zA-Z]").Count > 0);
        }
        [Fact]
        public async Task GenerateWordArray()
        {
            //arrange
            //act
            var response = await _httpClient.GetAsync($"TextAnalysis/GetWordArray/mobydick-test.txt");
            var content = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<string[]>(content.Result);
            //assert

            Assert.IsType<string[]>(result);
            Assert.True(result.Count() > 0);
        }

        [Theory]
        [InlineData("mobydick-test.txt")]
        [InlineData("binarymoby-test.txt")]
        public async Task GetWordHierarchy(string fileLocation)
        {
            //arrange
            //act
            var response = await _httpClient.GetAsync($"TextAnalysis/GetWordHierarchy/{fileLocation}");
            var content = response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<t_analysis>>(content.Result);
            //assert

            Assert.IsType<List<t_analysis>>(result);
            Assert.True(result.Count > 0);
            Assert.Equal(20, result.Count);
        }

        [Fact]
        public async Task DeleteFile()
        {
            //arrange
            //act
            var response = await _httpClient.DeleteAsync($"TextAnalysis/DeleteFile/mobydick-test.txt");
            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
