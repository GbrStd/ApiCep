using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AtvAPICep.Data;
using AtvAPICep.Models;
using Newtonsoft.Json;
using AtvAPICep.ViaCEP;
using System.Security.Policy;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtvAPICep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly AtvAPICepContext _context;

        public PessoasController(AtvAPICepContext context)
        {
            _context = context;
        }

        // GET: api/Pessoas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoa()
        {
            if (_context.Pessoa == null)
            {
                return NotFound();
            }
            return await _context.Pessoa.Include(p => p.Endereco).ToListAsync();
        }

        // GET: api/Pessoas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pessoa>> GetPessoa(int id)
        {
            if (_context.Pessoa == null)
            {
                return NotFound();
            }
            var pessoa = await _context.Pessoa.Include(p => p.Endereco)
                                              .Where(p => p.Id == id)
                                              .FirstOrDefaultAsync();

            if (pessoa == null)
            {
                return NotFound();
            }

            return pessoa;
        }

        // PUT: api/Pessoas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(int id, Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return BadRequest();
            }

            _context.Entry(pessoa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PessoaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pessoas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pessoa>> PostPessoa(Pessoa pessoa)
        {
            if (_context.Pessoa == null)
            {
                return Problem("Entity set 'AtvAPICepContext.Pessoa'  is null.");
            }

            if (pessoa.Endereco == null)
            {
                return BadRequest();
            }

            ViaCepEndereco? endereco = await ViaCep.GetEndereco(pessoa.Endereco.Cep);

            if (endereco == null)
            {
                return BadRequest();
            }

            pessoa.Endereco.Logradouro = endereco.Logradouro;
            pessoa.Endereco.Cidade = endereco.Localidade;
            pessoa.Endereco.Estado = endereco.Uf;

            _context.Pessoa.Add(pessoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPessoa", new { id = pessoa.Id }, pessoa);
        }

        // DELETE: api/Pessoas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            if (_context.Pessoa == null)
            {
                return NotFound();
            }
            var pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            _context.Pessoa.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PessoaExists(int id)
        {
            return (_context.Pessoa?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
