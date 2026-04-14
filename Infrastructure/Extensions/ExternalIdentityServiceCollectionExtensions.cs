using System.Security.Claims;
using AspNet.Security.OAuth.GitHub;
using Infrastructure.Identity.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ExternalIdentityServiceCollectionExtensions
{
    public static IServiceCollection AddExternalIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var authenticationBuilder = services.AddAuthentication();

        var gitHubOptions = configuration
            .GetSection(GitHubAuthOptions.SectionName)
            .Get<GitHubAuthOptions>();

        if (gitHubOptions != null &&
            !string.IsNullOrWhiteSpace(gitHubOptions.ClientId) &&
            !string.IsNullOrWhiteSpace(gitHubOptions.ClientSecret))
        {
            authenticationBuilder.AddGitHub(GitHubAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ClientSecret = gitHubOptions.ClientSecret;
                options.ClientId = gitHubOptions.ClientId;
                options.SignInScheme = IdentityConstants.ExternalScheme;
                options.CallbackPath = "/signin-github";

                options.Scope.Add("user:email");

                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");

                options.SaveTokens = true;
            });
        }

        return services;
    }
}
