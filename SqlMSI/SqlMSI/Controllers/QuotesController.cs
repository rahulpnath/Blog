using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlMSI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuotesController : ControllerBase
    {
        public QuotesController(QuoteContext context)
        {
            Context = context;
        }

        public QuoteContext Context { get; }

        public async Task<IActionResult> Get()
        {
            var quotes = await Context.Quote.ToListAsync();
            return Ok(quotes);
        }
    }

    public class Quote
    {
        public Guid Id { get; set; }
        public int QuoteNumber { get; set; }
    }
}
