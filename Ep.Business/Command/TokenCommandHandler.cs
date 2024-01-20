using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Base.Encryption;
using Base.Response;
using Base.Token;
using Business.Cqrs;
using Data.DbContext;
using Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Schema;

namespace Business.Command;

public class TokenCommandHandler : //Mediator Interfaces
    IRequestHandler<CreateTokenCommand, ApiResponse<TokenResponse>>
{
    private readonly EpDbContext _dbContext;
    private readonly JwtConfig _jwtConfig;

    public TokenCommandHandler(EpDbContext dbContext, IOptionsMonitor<JwtConfig> jwtConfig) //DI for dbContext and jwtConfig
    {
        _dbContext = dbContext; //DI 
        _jwtConfig = jwtConfig.CurrentValue; //DI
    }

    public async Task<ApiResponse<TokenResponse>> Handle(CreateTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<ApplicationUser>().Where(x => x.UserName == request.Model.UserName)
            .FirstOrDefaultAsync(cancellationToken);
        if (user == null)
        {
            return new ApiResponse<TokenResponse>("Invalid user information");
        }

        var hash = Md5Extension.GetHash(request.Model.Password.Trim());
        if (hash != user.Password)
        {
            user.LastActivityDate = DateTime.UtcNow;
            user.PasswordRetryCount++;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse<TokenResponse>("Invalid user information");
        }

        if (user.Status != 1)
        {
            return new ApiResponse<TokenResponse>("Invalid user status");
        }

        if (user.PasswordRetryCount > 3)
        {
            return new ApiResponse<TokenResponse>("Invalid user status");
        }

        user.LastActivityDate = DateTime.UtcNow;
        user.PasswordRetryCount = 0;
        await _dbContext.SaveChangesAsync(cancellationToken);

        var token = Token(user);

        return new ApiResponse<TokenResponse>(new TokenResponse()
        {
            Email = user.Email,
            Token = token,
            ExpireDate = DateTime.Now.AddMinutes(_jwtConfig.AccessTokenExpiration)
        });
    }

    private string Token(ApplicationUser user)
    {
        var claims = GetClaims(user);
        var secret = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

        var jwtToken = new JwtSecurityToken(
            _jwtConfig.Issuer,
            _jwtConfig.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtConfig.AccessTokenExpiration),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature)
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return accessToken;
    }

    private IEnumerable<Claim> GetClaims(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim("UserName", user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };
        return claims;
    }
}