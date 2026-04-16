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
        var userMembership = userCurrentMembershipResult.UserMembership;

        foreach (var membership in membershipsResult.Memberships!)
        {
            var membershipModel = new Membership()
            {
                Id = membership.Id,
                Name = membership.Name,
                Price = membership.Price,
                IsCurrent = membership.Id == userMembership?.MembershipId,
                IsActive = userMembership?.IsActive ?? false
            };

            viewModel.AvailableMembershipsViewModel.Memberships.Add(membershipModel);
            if (membershipModel.IsCurrent)
                viewModel.CurrentMembership = membershipModel;
        }

        return View(viewModel);
    }

    public async Task<IActionResult> SelectPlan(Guid membershipId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Redirect("sign-out");

        var resultCurrent = await userMembershipService.GetByUserIdAsync(userId);
        if (resultCurrent.UserMembership is not null)
        {
            var result = await userMembershipService.ChangeMembership(userId, membershipId);
            Console.WriteLine(result.ErrorMessage);
        }
        else
        {
            var result = await userMembershipService.SignUpMembership(userId, membershipId);
            Console.WriteLine(result.ErrorMessage);
        }

        return RedirectToAction(nameof(Index));
    }
}