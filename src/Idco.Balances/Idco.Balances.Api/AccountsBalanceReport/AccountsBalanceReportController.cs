namespace Idco.Balances.Api.AccountBalanceReport
{
    using AutoMapper;
    using Idco.Balances.Domain.Requests;
    using Idco.Balances.Domain.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Accounts Balance Report Controller
    /// </summary>
    [Route("[controller]")]
    [Produces("application/json")]
    public class AccountsBalanceReportController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AccountsBalanceReportController> _logger;

        public AccountsBalanceReportController(
            IMapper mapper,
            ILogger<AccountsBalanceReportController> logger)
            => (_mapper, _logger) = (mapper, logger);


        /// <summary>
        /// Produces an Accounts Balance Report from provided Accounts information
        /// </summary>
        /// <response code="200">The reqest has been processed successfully.</response>
        /// /// <response code="200">The reqest was invalid.</response>
        /// <response code="500">There was a server problem.</response>
        [HttpPost("Process")]
        [ProducesResponseType(typeof(EodBalanceListReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBalanceReport(
            [FromBody] AccountsEodBalanceRequestDto accountsBalanceRequestDto,
            [FromServices] IAccountsBalanceReportService accountsBalanceReportService)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var domainRequest = _mapper.Map<AccountsBalanceRequest>(accountsBalanceRequestDto);
                var domainResult = await accountsBalanceReportService.GetEodBalanceReport(domainRequest);

                var reportDto = _mapper.Map<EodBalanceListReportDto>(domainResult);
                return new OkObjectResult(reportDto);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


        }
    }
}
