using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using address_book.Models;
using X.PagedList;
using Microsoft.AspNetCore.SignalR;

namespace address_book.Controllers
{
    [Route("api/contacts")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly address_bookContext _context;
        private readonly IHubContext<SignalServer> signalContext;

        public ContactsController(address_bookContext context, IHubContext<SignalServer> signalContext)
        {
            _context = context;
            this.signalContext = signalContext;
        }

        [HttpGet]
        public async Task<IPagedList> GetContacts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
        {
            return await _context.Contactswithnumbers.ToPagedListAsync(pageNumber, pageSize); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contactswithnumber>> GetContact(int id)
        {
            var contact = await _context.Contactswithnumbers.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }

            if (await _context.Contacts.AnyAsync(c => c.Id != id && c.Name == contact.Name && c.Address == contact.Address))
            {
                return StatusCode(403);
            }

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
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
        public async Task<ActionResult<Contact>> PostContact(Contactswithnumber contactwithnumbers)
        {
            Contact contact = new Contact();
            contact.Name = contactwithnumbers.Name;
            contact.Address = contactwithnumbers.Address;
            contact.DateOfBirth = contactwithnumbers.DateOfBirth;

            if (await _context.Contacts.AnyAsync(c => c.Name == contact.Name && c.Address == contact.Address))
            {
                return StatusCode(403);
            }

            _context.Contacts.Add(contact);
            string[] numbers = contactwithnumbers.Numbers.Split(';');

            await _context.SaveChangesAsync();

            PhoneNumbersController numbersController = new PhoneNumbersController(_context, signalContext);
            await numbersController.PostMultiplePhoneNumber(numbers, contact.Id);

            await signalContext.Clients.All.SendAsync("refreshContacts");

            return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Contact>> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            List<PhoneNumber> numbers = _context.PhoneNumbers.Where(p => p.ContactsId == id).ToList();

            numbers.ForEach(p => _context.PhoneNumbers.Remove(p));
            await _context.SaveChangesAsync();
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            await signalContext.Clients.All.SendAsync("refreshContacts");

            return contact;
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
