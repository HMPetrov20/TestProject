using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PractiseApp.DAL;
using PractiseApp.DAL.Models;

namespace PractiseApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly EntityContext _context;

    public HomeController(ILogger<HomeController> logger, EntityContext context)
    {
        _logger = logger;
        _context = context;
    }

    // GET: TestModel
    public async Task<IActionResult> Index(string searchTerm, string sortBy, string sortOrder, int page = 1, int pageSize = 5)
    {
        // Start with base query
        var query = _context.TestModels.Where(x => x.IsDeleted == false);

        // Apply search filter if provided
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(x => 
                x.FirstName.Contains(searchTerm) ||
                x.LastName.Contains(searchTerm) ||
                x.Email.Contains(searchTerm) ||
                x.AddingInfo.Contains(searchTerm));
        }

        // Apply sorting
        query = sortBy?.ToLower() switch
        {
            "firstname" => sortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(x => x.FirstName)
                : query.OrderBy(x => x.FirstName),
            "lastname" => sortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(x => x.LastName)
                : query.OrderBy(x => x.LastName),
            "email" => sortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(x => x.Email)
                : query.OrderBy(x => x.Email),
            "age" => sortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(x => x.Age)
                : query.OrderBy(x => x.Age),
            "testnumber" => sortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(x => x.TestNumber)
                : query.OrderBy(x => x.TestNumber),
            "testdouble" => sortOrder?.ToLower() == "desc" 
                ? query.OrderByDescending(x => x.TestDouble)
                : query.OrderBy(x => x.TestDouble),
            _ => query.OrderBy(x => x.FirstName) // Default sorting
        };

        // Get total count for pagination
        var totalRecords = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        // Apply pagination
        var records = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Create view model with pagination info
        var viewModel = new IndexViewModel
        {
            Records = records,
            CurrentPage = page,
            TotalPages = totalPages,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            SearchTerm = searchTerm,
            SortBy = sortBy,
            SortOrder = sortOrder,
            HasPreviousPage = page > 1,
            HasNextPage = page < totalPages
        };

        return View(viewModel);
    }

    // GET: TestModel/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TestModel/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Age,TestNumber,TestDouble,AddingInfo")] TestModel testModel)
    {
        if (ModelState.IsValid)
        {
            testModel.Id = Guid.NewGuid();
            _context.Add(testModel);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = $"Record for {testModel.FirstName} {testModel.LastName} has been created successfully.";
            return RedirectToAction(nameof(Index));
        }
        return View(testModel);
    }

    // GET: TestModel/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var testModel = await _context.TestModels.FindAsync(id);
        if (testModel == null)
        {
            return NotFound();
        }
        return View(testModel);
    }

    // POST: TestModel/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,LastName,Email,Age,TestNumber,TestDouble,AddingInfo")] TestModel testModel)
    {
        if (id != testModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(testModel);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"Record for {testModel.FirstName} {testModel.LastName} has been updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestModelExists(testModel.Id))
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
        return View(testModel);
    }

    // POST: TestModel/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, string returnUrl)
    {
        var testModel = await _context.TestModels.FindAsync(id);
        if (testModel != null)
        {
            var name = $"{testModel.FirstName} {testModel.LastName}";
            testModel.IsDeleted = true;
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = $"Record for {name} has been deleted successfully.";
        }

        // Return to the same page with filters if returnUrl is provided
        if (!string.IsNullOrEmpty(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }
    
    private bool TestModelExists(Guid id)
    {
        return _context.TestModels.Any(e => e.Id == id);
    }
}

// View Model for Index page
public class IndexViewModel
{
    public IEnumerable<TestModel> Records { get; set; } = new List<TestModel>();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}