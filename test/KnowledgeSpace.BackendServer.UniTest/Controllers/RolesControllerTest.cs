using KnowledgeSpace.BackendServer.Controllers;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace KnowledgeSpace.BackendServer.UniTest.Controllers
{
    public class RolesControllerTest
    {
        private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;

        public RolesControllerTest()
        {
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            _mockRoleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);
        }

        [Fact]
        public void RolesController_ShouldCreateInstance_NotNull()
        {
           
            var rolesController = new RolesController(_mockRoleManager.Object);

            Assert.NotNull(rolesController);
        }

        [Fact]
        public void PostRole_ValidInput_Success()
        {
            //var rolesController = new RolesController(mockRoleManager.Object);

            //Assert.NotNull(rolesController);
        }
    }
}
