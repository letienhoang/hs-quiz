using Examination.Application.Queries.V1.GetHomeExamList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{sersion:apiVersion}/exams")]
    [ApiVersion("1.0")]
    public class ExamsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ExamsController> _logger;
        public ExamsController(IMediator mediator, ILogger<ExamsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetExamListAsync()
        {
            _logger.LogInformation("GetExamListAsync called");
            var query = new GetHomeExamListQuery();
            var result = await _mediator.Send(query);
            _logger.LogInformation("GetExamListAsync returned");
            return Ok(result);
        }
    }
}