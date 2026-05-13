using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VC_IMS.Models;

namespace VC_IMS.Controllers
{
    // Standard CRUD controller for VC_identity
    public class identityController : Controller
    {
        private readonly VC_IMSDb_moreContext _context;

        public identityController(VC_IMSDb_moreContext context)
        {
            _context = context;
        }

        // GET: identity
        public async Task<IActionResult> Index()
        {
            return View(await _context.VC_identities.ToListAsync());
        }

        // GET: identity/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_identity = await _context.VC_identities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (VC_identity == null)
            {
                return NotFound();
            }

            return View(VC_identity);
        }

        // GET: identity/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: identity/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,desc,logo,media_01,media_02,media_03,header,signature")] VC_identity VC_identity)
        {

            var logo = Request.Form.Files["logo"] as IFormFile;
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            if (logo == null || logo.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uniqueFile_logo_name = Guid.NewGuid().ToString() + "_" + logo.FileName;
            VC_identity.logo = uniqueFile_logo_name;
            var filePath = Path.Combine(uploadPath, uniqueFile_logo_name);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await logo.CopyToAsync(stream);
            }

            if (ModelState.IsValid)
            {
                _context.Add(VC_identity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(VC_identity);
        }

        // GET: identity/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_identity = await _context.VC_identities.FindAsync(id);
            if (VC_identity == null)
            {
                return NotFound();
            }
            return View(VC_identity);
        }

        // POST: identity/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,desc,logo,media_01,media_02,media_03,header,signature")] VC_identity VC_identity)
        {
            if (id != VC_identity.Id)
            {
                return NotFound();
            }

            var logo = Request.Form.Files["logo"] as IFormFile;
            var media_01 = Request.Form.Files["media_01"] as IFormFile;
            var media_02 = Request.Form.Files["media_02"] as IFormFile;
            var media_03 = Request.Form.Files["media_03"] as IFormFile;


            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            if (logo == null || logo.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            if (media_01 == null || media_01.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            if (media_02 == null || media_02.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            if (media_03 == null || media_03.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }


            var uniqueFile_logo_name = Guid.NewGuid().ToString() + "_" + logo.FileName;
            VC_identity.logo = uniqueFile_logo_name;
            var filePath = Path.Combine(uploadPath, uniqueFile_logo_name);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await logo.CopyToAsync(stream);
            }
            var uniqueFile_media01_name = Guid.NewGuid().ToString() + "_" + media_01.FileName;
            VC_identity.media_01 = uniqueFile_media01_name;
            var filePath1 = Path.Combine(uploadPath, uniqueFile_media01_name);
            using (var stream = new FileStream(filePath1, FileMode.Create))
            {
                await media_01.CopyToAsync(stream);
            }
            var uniqueFile_media02_name = Guid.NewGuid().ToString() + "_" + media_02.FileName;
            VC_identity.media_02 = uniqueFile_media02_name;
            var filePath2 = Path.Combine(uploadPath, uniqueFile_media02_name);
            using (var stream = new FileStream(filePath2, FileMode.Create))
            {
                await media_02.CopyToAsync(stream);
            }
            var uniqueFile_media03_name = Guid.NewGuid().ToString() + "_" + media_03.FileName;
            VC_identity.media_03 = uniqueFile_media03_name;
            var filePath3 = Path.Combine(uploadPath, uniqueFile_media03_name);
            using (var stream = new FileStream(filePath3, FileMode.Create))
            {
                await media_03.CopyToAsync(stream);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(VC_identity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VC_identityExists(VC_identity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(VC_identity);
        }

        // GET: identity/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var VC_identity = await _context.VC_identities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (VC_identity == null)
            {
                return NotFound();
            }

            return View(VC_identity);
        }

        // POST: identity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var VC_identity = await _context.VC_identities.FindAsync(id);
            if (VC_identity != null)
            {
                _context.VC_identities.Remove(VC_identity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VC_identityExists(int id)
        {
            return _context.VC_identities.Any(e => e.Id == id);
        }
    }
}