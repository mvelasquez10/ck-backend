namespace CK.Rest.Users.Form
{
    public class UserResultForm
    {
        #region Public Properties

        public uint Id { get; set; }

        public bool IsAdmin { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Token { get; set; }

        #endregion Public Properties
    }
}