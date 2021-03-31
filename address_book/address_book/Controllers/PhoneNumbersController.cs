using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using address_book.Models;
using Microsoft.AspNetCore.SignalR;

namespace address_book.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneNumbersController : ControllerBase
    {
        private readonly address_bookContext _context;
        private readonly IHubContext<SignalServer> signalContext;

        public PhoneNumbersController(address_bookContext context, IHubContext<SignalServer> signalContext)
        {
            _context = context;
            this.signalContext = signalContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhoneNumber>>> GetPhoneNumbers()
        {
            return await _context.PhoneNumbers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PhoneNumber>> GetPhoneNumber(int id)
        {
            var phoneNumber = await _context.PhoneNumbers.FindAsync(id);

            if (phoneNumber == null)
            {
                return NotFound();
            }

            return phoneNumber;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoneNumber(int id, PhoneNumber phoneNumber)
        {
            if (id != phoneNumber.Id)
            {
                return BadRequest();
            }

            _context.Entry(phoneNumber).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneNumberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await signalContext.Clients.All.SendAsync("refreshContacts");

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PhoneNumber>> PostPhoneNumber(PhoneNumber phoneNumber)
        {
            _context.PhoneNumbers.Add(phoneNumber);
            await _context.SaveChangesAsync();

            await signalContext.Clients.All.SendAsync("refreshContacts");

            return CreatedAtAction("GetPhoneNumber", new { id = phoneNumber.Id }, phoneNumber);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PhoneNumber>> DeletePhoneNumber(int id)
        {
            var phoneNumber = await _context.PhoneNumbers.FindAsync(id);
            if (phoneNumber == null)
            {
                return NotFound();
            }

            _context.PhoneNumbers.Remove(phoneNumber);
            await _context.SaveChangesAsync();

            await signalContext.Clients.All.SendAsync("refreshContacts");

            return phoneNumber;
        }

        public async Task PostMultiplePhoneNumber(string[] numbers, int id)
        {
            foreach (var number in numbers)
            {
                PhoneNumber phoneNumber = new PhoneNumber();
                phoneNumber.ContactsId = id;
                phoneNumber.Number = number;
                _context.PhoneNumbers.Add(phoneNumber);
            }

            await _context.SaveChangesAsync();
        }

        private bool PhoneNumberExists(int id)
        {
            return _context.PhoneNumbers.Any(e => e.Id == id);
        }
    }
}
