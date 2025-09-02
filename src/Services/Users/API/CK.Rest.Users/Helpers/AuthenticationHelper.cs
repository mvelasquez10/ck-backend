using System;
using System.Collections.Immutable;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using CK.Entities;
using CK.Repository;
using CK.Rest.Common.Shared;
using CK.Rest.Users.Shared.Forms;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CK.Rest.Users.Helpers
{
    internal class AuthenticationHelper : IAuthenticationHelper
    {
        #region Private Fields

        private readonly IConfiguration _config;

        private readonly ILogger _logger;

        private readonly EntityRepository<User, uint> _userRepository;

        #endregion Private Fields

        #region Public Constructors

        public AuthenticationHelper(
            EntityRepository<User, uint> userRepository,
            IConfiguration config,
            ILogger<AuthenticationHelper> logger)
        {
            _userRepository = userRepository;
            _config = config;
            _logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public UserResultForm Authenticate(string email, string password)
        {
            _logger.LogInformation("Authenticating user {Email}", email);

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                _logger.LogWarning("Email or password is null or empty");
                return null;
            }

            var result = _userRepository.ListEntities(ImmutableList.Create(new[]
            {
                new Filter<User>(nameof(User.Email), email),
                new Filter<User>(nameof(User.IsActive), true),
            }));

            if (!result.IsValid)
            {
                _logger.LogError("Failed to list users: {Error}", result.Exception);
                return null;
            }

            var user = result.Value.FirstOrDefault();

            if (user == null)
            {
                _logger.LogWarning("User {Email} not found", email);
                return null;
            }

            if (!user.Pass.SequenceEqual(password.ToSha256()))
            {
                _logger.LogWarning("Invalid password for user {Email}", email);
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Common:Secret"]);
            var keyId = _config["Common:KeyId"];

            _ = int.TryParse(_config["SessionExpireInDays"], out var expireDays);

            DateTime? expire = null;
            if (expireDays > 0)
                expire = DateTime.UtcNow.AddDays(expireDays);

            var securityKey = new SymmetricSecurityKey(key)
            {
                KeyId = keyId,
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"),
                }),
                Expires = expire,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
            };

            _logger.LogInformation("Token Descriptor: {@TokenDescriptor}", tokenDescriptor);

            var tokenHanlder = tokenHandler.CreateToken(tokenDescriptor);
            return new UserResultForm
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                IsAdmin = user.IsAdmin,
                Token = tokenHandler.WriteToken(tokenHanlder),
            };
        }

        #endregion Public Methods
    }
}
