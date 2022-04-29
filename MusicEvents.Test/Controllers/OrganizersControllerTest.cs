using MusicEvents.Controllers;
using MusicEvents.Data.Models;
using MusicEvents.Models.Organizers;
using MyTested.AspNetCore.Mvc;
using System.Linq;
using Xunit;

namespace MusicEvents.Test.Controllers
{
    public class OrganizersControllerTest
    {
        [Fact]
        public void GetBecomeShouldBeForAuthorizedUsersAndReturnView()
           => MyController<OrganizersController>
               .Instance()
               .Calling(c => c.Create())
               .ShouldHave()
               .ActionAttributes(attributes => attributes
                   .RestrictingForAuthorizedRequests())
               .AndAlso()
               .ShouldReturn()
               .View();

        [Theory]
        [InlineData("Dealer", "+359888888888")]
        public void PostBecomeShouldBeForAuthorizedUsersAndReturnRedirectWithValidModel(
            string dealerName,
            string phoneNumber)
            => MyController<OrganizersController>
                .Instance(controller => controller
                    .WithUser())
                .Calling(c => c.Create(new CreateOrganizerFormModel
                {
                    Name = dealerName,
                    PhoneNumber = phoneNumber
                }))
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                    .RestrictingForHttpMethod(HttpMethod.Post)
                    .RestrictingForAuthorizedRequests())
                .ValidModelState();
    }
}
