using System.Security.Claims;
using Application.Abstractions.Services.Memberships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Areas.Account.Models;

namespace Presentation.WebApp.Areas.Account.Controllers;

[Area("Account")]
[Authorize]
public class MembershipController(IMembershipService membershipService, IUserMembershipService userMembershipService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Redirect("sign-out");

        var viewModel = new MembershipsViewModel()
        {
            AvailableMembershipsViewModel = new AvailableMembershipsViewModel()
        };

        var membershipsResult = await membershipService.GetAllAsync();
        if (!membershipsResult.Succeeded || membershipsResult.Memberships?.Count == 0)
        {
            viewModel.AvailableMembershipsViewModel.ErrorMessage =
                membershipsResult.ErrorMessage ??
                "Couldn't fetch memberships, try again later.";

            return View(viewModel);
        }

        var userCurrentMembershipResult = await userMembershipService.GetByUserIdAsync(userId);
        var userMembershipId = userCurrentMembershipResult.UserMembership?.MembershipId;

        foreach (var membership in membershipsResult.Memberships!)
        {
            viewModel.AvailableMembershipsViewModel.Memberships.Add(new()
            {
                Name = membership.Name,
                Price = membership.Price,
                IsCurrent = membership.Id == userMembershipId
            });
        }

        return View(viewModel);
    }

}