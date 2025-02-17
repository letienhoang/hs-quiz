using Examination.Application.Queries.GetHomeExamList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Examination.API.Controllers
{
    [ApiController]
    [Route("api/exams")]
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