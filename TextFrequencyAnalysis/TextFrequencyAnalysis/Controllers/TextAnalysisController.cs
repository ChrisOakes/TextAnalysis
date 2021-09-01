using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextFrequencyAnalysis.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace TextFrequencyAnalysis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextAnalysisController : ControllerBase
    {
        private ILogger<TextAnalysisController> _logger;
        private ITextAnalysis _textAnalysis;
        private IWebHostEnvironment _env;

        public TextAnalysisController(ILogger<TextAnalysisController> logger,
            ITextAnalysis textAnalysis,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _textAnalysis = textAnalysis;
            _env = env;
        }


        [HttpGet]
        [Route("DetermineFileExists/{fileLocation}")]
        public async Task<IActionResult> DetermineFileExists(string fileLocation)
        {
            try
            {
                bool outcome = false;
                fileLocation = Path.Combine(_env.WebRootPath, "textPlaceholder", fileLocation);

                outcome = await _textAnalysis.DetermineFileExist(fileLocation);

                return Ok(outcome);
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }
        [HttpGet]
        [Route("ReadFile/{fileLocation}")]
        public async Task<IActionResult> ReadFile(string fileLocation)
        {
            try
            {
                bool outcome = false;
                fileLocation = Path.Combine(_env.WebRootPath, "textPlaceholder", fileLocation);

                string contents = await _textAnalysis.ReadFile(fileLocation);

                if (contents.Length > 0)
                {
                    outcome = true;
                }

                return Ok(outcome);
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                return Ok(false);
            }
        }
        [HttpGet]
        [Route("DetermineBinary/{fileLocation}")]
        public async Task<IActionResult> DetermineBinary(string fileLocation)
        {
            try
            {
                bool outcome = false;
                fileLocation = Path.Combine(_env.WebRootPath, "textPlaceholder", fileLocation);

                string contents = await _textAnalysis.ReadFile(fileLocation);

                outcome = await _textAnalysis.DetermineBinary(contents);

                return Ok(outcome);
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }
        [HttpGet]
        [Route("GenerateByteArray/{fileLocation}")]
        public async Task<IActionResult> GenerateByteArray(string fileLocation)
        {
            try
            {
                fileLocation = Path.Combine(_env.WebRootPath, "textPlaceholder", fileLocation);

                string contents = await _textAnalysis.ReadFile(fileLocation);

                var outcome = _textAnalysis.GenerateByteArray(contents);

                return Ok(outcome);
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }
        [HttpGet]
        [Route("ConvertToText/{fileLocation}")]
        public async Task<IActionResult> ConvertToText(string fileLocation)
        {
            try
            {
                fileLocation = Path.Combine(_env.WebRootPath, "textPlaceholder", fileLocation);

                string contents = await _textAnalysis.ReadFile(fileLocation);

                var outcome = _textAnalysis.ConvertToText(contents);

                return Ok(outcome);
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }

        [HttpGet]
        [Route("GetWordArray/{fileLocation}")]
        public async Task<IActionResult> GetWordArray(string fileLocation)
        {
            try
            {
                fileLocation = Path.Combine(_env.WebRootPath, "textPlaceholder", fileLocation);

                string contents = await _textAnalysis.ReadFile(fileLocation);

                var outcome = _textAnalysis.GetWordArray(contents);

                return Ok(outcome);
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }

        [HttpGet]
        [Route("GetWordHierarchy/{fileLocation}")]
        public async Task<IActionResult> GetWordHierarchy(string fileLocation)
        {
            try
            {
                fileLocation = Path.Combine(_env.WebRootPath, "textPlaceholder", fileLocation);

                var outcome = await _textAnalysis.GetWordHierarchy(fileLocation);

                return Ok(outcome);
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }
        [HttpDelete]
        [Route("DeleteFile/{fileLocation}")]
        public async Task<IActionResult> DeleteFile(string fileLocation)
        {
            try
            {
                fileLocation = Path.Combine(_env.WebRootPath, "textPlaceholder", "DeleteTest", fileLocation);

                await _textAnalysis.DeleteFile(fileLocation);
                 
                return Ok();
            }
            catch (Exception er)
            {
                _logger.LogError(er, "Error encountered");
                throw;
            }
        }
    }
}
