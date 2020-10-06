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
using CK.Rest.Users.Form;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CK.Rest.Users.Helpers
{
    internal class AuthenticationHelper : IAuthenticationHelper
    {
        #region Private Fields

        private readonly IConfiguration _config;

        private readonly EntityRepository<User, uint> _userRepository;

        #endregion Private Fields

        #region Public Constructors

        public AuthenticationHelper(EntityRepository<User, uint> userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        #endregion Public Constructors

        #region Public Methods

        public UserResultForm Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var result = _userRepository.ListEntities(ImmutableList.Create(new[]
            {
                new Filter<User>(nameof(User.Email), email),
                new Filter<User>(nameof(User.IsActive), true),
            }));

            if (!result.IsValid)
                return null;

            var user = result.Value.FirstOrDefault();

            if (user == null || !user.Pass.SequenceEqual(password.ToSha256()))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Common:Secret"]);

            _ = int.TryParse(_config["SessionExpireInDays"], out var expireDays);

            DateTime? expire = null;
            if (expireDays > 0)
                expire = DateTime.UtcNow.AddDays(expireDays);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"),
                }),
                Expires = expire,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
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