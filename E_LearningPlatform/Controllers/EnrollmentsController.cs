using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // For user management
using E_LearningPlatform.Models.Entities;
using E_LearningPlatorm.Data;

public class EnrollmentsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public EnrollmentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Enrollments
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Enrollments.Include(e => e.Course).Include(e => e.User);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Enrollments/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enrollment = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (enrollment == null)
        {
            return NotFound();
        }

        return View(enrollment);
    }

    // GET: Enrollments/Create
    public IActionResult Create()
    {
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName");
        return View();
    }

    // POST: Enrollments/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CourseId,EnrollmentDate")] Enrollment enrollment)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return StatusCode(400, "User is not authenticated.");
        }

        if (ModelState.IsValid)
        {
            if (int.TryParse(user.Id, out int userId))
            {
                enrollment.UserId = userId;
            }
            else
            {
                return StatusCode(400, "Invalid user ID format.");
            }

            _context.Add(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", enrollment.CourseId);
        return View(enrollment);
    }

    // GET: Enrollments/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null)
        {
            return NotFound();
        }
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", enrollment.CourseId);
        return View(enrollment);
    }

    // POST: Enrollments/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,EnrollmentDate")] Enrollment enrollment)
    {
        if (id != enrollment.Id.GetValueOrDefault())
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(enrollment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(enrollment.Id.GetValueOrDefault()))
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
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", enrollment.CourseId);
        return View(enrollment);
    }

    // GET: Enrollments/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enrollment = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (enrollment == null)
        {
            return NotFound();
        }

        return View(enrollment);
    }

    // POST: Enrollments/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Enrollments == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Enrollments' is null.");
        }
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment != null)
        {
            _context.Enrollments.Remove(enrollment);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EnrollmentExists(int id)
    {
        return _context.Enrollments.Any(e => e.Id == id);
    }
}
