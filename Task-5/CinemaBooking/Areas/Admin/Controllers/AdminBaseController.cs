using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBooking.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class AdminBaseController : Controller { }
