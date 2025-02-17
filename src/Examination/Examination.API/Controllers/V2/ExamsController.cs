using Examination.Application.Queries.V2.GetHomeExamList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers.V2
{
    [ApiController]
    [Route("api/v{sersion:apiVersion}/exams")]
    [ApiVersion("2.0")]
    public class ExamsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ExamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetExamListAsync(string sample)
        {
            var query = new GetHomeExamListQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}