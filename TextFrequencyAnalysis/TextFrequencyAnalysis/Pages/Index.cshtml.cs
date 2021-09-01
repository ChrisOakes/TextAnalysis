using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TextFrequencyAnalysis.Models;
using TextFrequencyAnalysis.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace TextFrequencyAnalysis.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private ITextAnalysis _textAnalysis;
        private IWebHostEnvironment _env;


        public IndexModel(ILogger<IndexModel> logger,
            ITextAnalysis textAnalysis,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _textAnalysis = textAnalysis;
            _env = env;
        }

        public string Message { get; set; } = "";

        public List<t_analysis> li_t_analysis { get; set; }
        [BindProperty]
        public string FileLocation { get; set; } = "";

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {

                return Page();
            }
            catch (Exception er)
            {

                _logger.LogError(er, "Error encountered");
                Message = $"There was an error loading the page. Error: {er.Message}";

                return Page();
            }
        }

        public async Task<IActionResult> OnPostAnalyseFile(object sender, EventArgs e)
        {
            try
            {
                string fileLocation = "";
                string folderName = "textPlaceholder";
                li_t_analysis = new List<t_analysis>();
                var fileToUpload = Request.Form.Files.FirstOrDefault();

                fileLocation = Path.Combine(_env.WebRootPath, folderName, fileToUpload.FileName);

                using (var stream = new FileStream(fileLocation, FileMode.Create))
                {
                    fileToUpload.CopyTo(stream);
                }

                li_t_analysis = await _textAnalysis.GetWordHierarchy(fileLocation);

                await _textAnalysis.DeleteFile(fileLocation);


                return new JsonResult(li_t_analysis);
            }
            catch (Exception er)
            {

                _logger.LogError(er, "Error encountered");
                Message = "Error encountered: " + er.Message;

                return new JsonResult(Message);
            }
        }
    }
}
