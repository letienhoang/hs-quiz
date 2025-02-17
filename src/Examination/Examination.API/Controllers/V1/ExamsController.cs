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
        public ExamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetExamListAsync()
        {
            var query = new GetHomeExamListQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}