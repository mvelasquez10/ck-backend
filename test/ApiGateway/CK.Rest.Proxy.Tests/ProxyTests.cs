using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CK.Rest.Posts.Shared.Forms;
using CK.Rest.Users.Shared.Forms;
using Xunit;

namespace CK.Rest.Proxy.Tests
{
    public class ProxyTests : IClassFixture<AuthenticatedTestFixture>
    {
        private readonly AuthenticatedTestFixture _fixture;

        public ProxyTests(AuthenticatedTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task User_Lifecycle_ReturnsOk()
        {
            // 1. POST - Create a new user
            var newUserForm = new UserFormPost
            {
                Name = "Test",
                Surname = "User",
                Email = "test.lifecycle@user.com",
                Password = "password"
            };
            var postContent = new StringContent(JsonSerializer.Serialize(newUserForm), Encoding.UTF8, "application/json");
            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"{TestConfiguration.GetHost("User")}/user")
            {
                Content = postContent
            };
            postRequest.Headers.Add("Authorization", $"Bearer {_fixture.AdminToken}");
            var postResponse = await _fixture.Client.SendAsync(postRequest);
            postResponse.EnsureSuccessStatusCode();
            var postResponseContent = await postResponse.Content.ReadAsStringAsync();
            var newUser = JsonSerializer.Deserialize<UserResultForm>(postResponseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(newUser);

            // 2. GET - Retrieve the new user to verify creation
            var getRequest = new HttpRequestMessage(HttpMethod.Get, $"{TestConfiguration.GetHost("User")}/user/{newUser.Id}");
            getRequest.Headers.Add("Authorization", $"Bearer {_fixture.AdminToken}");
            var getResponse = await _fixture.Client.SendAsync(getRequest);
            getResponse.EnsureSuccessStatusCode();

            // 3. PUT - Update the user
            var updateUserForm = new UserFormPut
            {
                Name = "Updated",
                Surname = "User",
                Email = "test.lifecycle.updated@user.com",
                Password = "newpassword"
            };
            var putContent = new StringContent(JsonSerializer.Serialize(updateUserForm), Encoding.UTF8, "application/json");
            var putRequest = new HttpRequestMessage(HttpMethod.Put, $"{TestConfiguration.GetHost("User")}/user/{newUser.Id}")
            {
                Content = putContent
            };
            putRequest.Headers.Add("Authorization", $"Bearer {_fixture.AdminToken}");
            var putResponse = await _fixture.Client.SendAsync(putRequest);
            putResponse.EnsureSuccessStatusCode();

            // 4. DELETE - Delete the user
            var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"{TestConfiguration.GetHost("User")}/user/{newUser.Id}");
            deleteRequest.Headers.Add("Authorization", $"Bearer {_fixture.AdminToken}");
            var deleteResponse = await _fixture.Client.SendAsync(deleteRequest);
            deleteResponse.EnsureSuccessStatusCode();

            // 5. GET (final) - Verify deletion
            var finalGetRequest = new HttpRequestMessage(HttpMethod.Get, $"{TestConfiguration.GetHost("User")}/user/{newUser.Id}");
            finalGetRequest.Headers.Add("Authorization", $"Bearer {_fixture.AdminToken}");
            var finalGetResponse = await _fixture.Client.SendAsync(finalGetRequest);
            Assert.Equal(HttpStatusCode.NotFound, finalGetResponse.StatusCode);
        }

        [Fact]
        public async Task Post_Lifecycle_ReturnsOk()
        {
            // 1. POST - Create a new post
            var newPostForm = new PostFormPost
            {
                Title = "Test Lifecycle Post",
                Description = "A post for testing.",
                Language = 1,
                Snippet = "Console.WriteLine(\"Hello, World!\");"
            };
            var postContent = new StringContent(JsonSerializer.Serialize(newPostForm), Encoding.UTF8, "application/json");
            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"{TestConfiguration.GetHost("Post")}/post")
            {
                Content = postContent
            };
            postRequest.Headers.Add("Authorization", $"Bearer {_fixture.AdminToken}");
            var postResponse = await _fixture.Client.SendAsync(postRequest);
            postResponse.EnsureSuccessStatusCode();
            var postResponseContent = await postResponse.Content.ReadAsStringAsync();
            var newPost = JsonSerializer.Deserialize<JsonElement>(postResponseContent);
            var newPostId = newPost.GetProperty("id").GetUInt32();

            // 2. GET - Retrieve the new post
            var getRequest = new HttpRequestMessage(HttpMethod.Get, $"{TestConfiguration.GetHost("Post")}/post/{newPostId}");
            var getResponse = await _fixture.Client.SendAsync(getRequest);
            getResponse.EnsureSuccessStatusCode();

            // 3. PUT - Update the post
            var updatePostForm = new PostFormPut
            {
                Title = "Updated Test Lifecycle Post",
                Description = "An updated post for testing.",
                Language = 2,
                Snippet = "print(\"Hello, World!\")"
            };
            var putContent = new StringContent(JsonSerializer.Serialize(updatePostForm), Encoding.UTF8, "application/json");
            var putRequest = new HttpRequestMessage(HttpMethod.Put, $"{TestConfiguration.GetHost("Post")}/post/{newPostId}")
            {
                Content = putContent
            };
            putRequest.Headers.Add("Authorization", $"Bearer {_fixture.AdminToken}");
            var putResponse = await _fixture.Client.SendAsync(putRequest);
            putResponse.EnsureSuccessStatusCode();

            // 4. DELETE - Delete the post
            var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"{TestConfiguration.GetHost("Post")}/post/{newPostId}");
            deleteRequest.Headers.Add("Authorization", $"Bearer {_fixture.AdminToken}");
            var deleteResponse = await _fixture.Client.SendAsync(deleteRequest);
            deleteResponse.EnsureSuccessStatusCode();

            // 5. GET (final) - Verify deletion
            var finalGetRequest = new HttpRequestMessage(HttpMethod.Get, $"{TestConfiguration.GetHost("Post")}/post/{newPostId}");
            var finalGetResponse = await _fixture.Client.SendAsync(finalGetRequest);
            Assert.Equal(HttpStatusCode.NotFound, finalGetResponse.StatusCode);
        }

        [Fact]
        public async Task Language_Lifecycle_ReturnsOk()
        {
            // For languages, we'll just do GET and DELETE as there's no POST/PUT in the controller.
            // We'll test against a known language that should exist from seeding.
            const int languageId = 3; // javascript

            // 1. GET - Retrieve the language
            var getRequest = new HttpRequestMessage(HttpMethod.Get, $"{TestConfiguration.GetHost("Language")}/language/{languageId}");
            var getResponse = await _fixture.Client.SendAsync(getRequest);
            getResponse.EnsureSuccessStatusCode();

            // 2. DELETE - Delete the language
            var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"{TestConfiguration.GetHost("Language")}/language/{languageId}");
            deleteRequest.Headers.Add("Authorization", $"Bearer {_fixture.AdminToken}");
            var deleteResponse = await _fixture.Client.SendAsync(deleteRequest);
            deleteResponse.EnsureSuccessStatusCode();

            // 3. GET (final) - Verify deletion
            var finalGetRequest = new HttpRequestMessage(HttpMethod.Get, $"{TestConfiguration.GetHost("Language")}/language/{languageId}");
            var finalGetResponse = await _fixture.Client.SendAsync(finalGetRequest);
            Assert.Equal(HttpStatusCode.NotFound, finalGetResponse.StatusCode);
        }

        [Fact]
        public async Task Delete_User_AdminCannotDeleteSelf_ReturnsForbidden()
        {
            // This test remains valid as it checks a specific business rule.
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{TestConfiguration.GetHost("User")}/user/{_fixture.AdminId}");
            request.Headers.Add("Authorization", $"Bearer {_fixture.AdminToken}");

            var response = await _fixture.Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
