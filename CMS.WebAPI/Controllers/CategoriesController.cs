using CMS.Domain.Entities;
using CMS.Infrastructure.Data; // DbContext
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
	private readonly AppDbContext _context;

	public CategoriesController(AppDbContext context)
	{
		_context = context;
	}
}
