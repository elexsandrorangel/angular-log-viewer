using LogViewer.Business.Interfaces;
using LogViewer.Models.SearchModels;
using LogViewer.Models.ViewModels;
using LogViewer.Repository.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController
        : BaseController<IAccessLogBusiness, AccessLog, AccessLogViewModel, AccessLogSearchModel>
    {
        public LogsController(IAccessLogBusiness business) : base(business)
        {
        }


        /// <summary>
        /// Import log entries from one or more log files
        /// </summary>
        /// <param name="files"></param>
        /// <remarks>Async action</remarks>
        /// <returns></returns>
        [HttpPost]
        [Route("import")]
        public async Task<IActionResult> UploadSingle(IFormFile[] files) 
        {
            var result = new List<ResultViewModel>();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await file.OpenReadStream().CopyToAsync(ms);

                        string fileContent = Encoding.UTF8.GetString(ms.ToArray());

                        result.Add(await Business.ParseLogEntryAsync(file.FileName, fileContent));
                    }
                }
            }
            return Ok(result);
        }
    }
}
