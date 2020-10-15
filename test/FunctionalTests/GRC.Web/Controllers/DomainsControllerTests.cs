using Xunit;
using Moq;
using MediatR;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using GRC.Core.Entities;
using GRC.Web.Features.DomainHandlers;
using Microsoft.Extensions.Logging;
using GRC.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using GRC.Web.Models;

namespace FunctionalTests.GRCWeb.Controllers
{
    public class DomainsControllerTests
    {


        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<Domain>> _logger;

        public DomainsControllerTests()
        {
            _logger = new Mock<ILogger<Domain>>();
            _mediator = new Mock<IMediator>();
        }


        [Fact]
        [Trait("Action", "GetList")]
        public async void GetAll_Shoud_Returns_All_Objects()
        {
            //Given
            var sut = new List<Domain>(){
                 new Domain(){Id = 1, Code="1", StandardId=1,  Title = "Policies"},
                 new Domain(){Id = 1, Code="2", StandardId=1,  Title = "Policies2"},
            };
            _mediator.Setup(x => x.Send(It.IsAny<GetAllHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);

            //When
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Index(1);
            Assert.Equal(2, ((List<Domain>)((ViewResult)result.Result).Model).Count());
        }

        [Fact]
        [Trait("Action", "GetList")]
        public async void GetAll_Shoud_Returns_InternalServerError_When_Rais_Error()
        {

            //When  
            _mediator.Setup(x => x.Send(It.IsAny<GetAllHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Index(1);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }



        [Fact]
        [Trait("Action", "GetDetails")]
        public async void GetById_Should_Returns_Object_When_Id_Exists()
        {
            //Given
            var sut = new Domain() { Id = 1, Code = "1", StandardId = 1, Title = "Policies" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);

            //When
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Details(1, 1);
            Assert.Equal(1, ((Domain)((ViewResult)result.Result).Model).Id);
        }

        [Fact]
        [Trait("Action", "GetDetails")]
        public async void GetById_Should_Returns_BadRequest_On_Object_Mismatch()
        {
            //Given
            var sut = new Domain() { Id = 1, Code = "1", StandardId = 1, Title = "Policies" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);

            //When
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Details(2, 1);
            Assert.IsType<BadRequestResult>(result.Result);

        }

        [Fact]
        [Trait("Action", "GetDetails")]
        public async void GetById_Should_Returns_NotFound_When_Id_Not_Exists()
        {
            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain)null);

            //When
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Details(1, 1);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "GetDetails")]
        public async void GetById_Should_Returns_BadRequest_When_Id_Is_Null()
        {
            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain)null);

            //When
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Details(1, null);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "GetDetails")]
        public async void GetById_Shoud_Returns_InternalServerError_When_Rais_Error()
        {
            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());


            //When  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Details(1, 2);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }


        [Fact]
        [Trait("Action", "Add")]
        public async void Add_Shoud_Insert_One_When_Post_Valid_Object()
        {
            //Given
            var sut = new Domain() { Id = 1, Code = "1", StandardId = 1, Title = "Policies" };
            var standard = new Standard { Id = 1, Name = "Isms" };

            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.StandardHandlers.GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(standard);
            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.StandardHandlers.UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);


            //When  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Create(1, sut);
            Assert.IsType<RedirectToActionResult>(result.Result);
        }


        [Fact]
        [Trait("Action", "Add")]
        public async void Add_Duplicate_Domain_Code_Shoud_Return_Error()
        {
            //Given
            var sut = new Domain() { Id = 1, Code = "1", StandardId = 1, Title = "Policies" };
            var sut1 = new Domain() { Id = 2, Code = "1", StandardId = 1, Title = "Policies2" };
            var standard = new Standard { Id = 1, Name = "Isms" };
            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.StandardHandlers.GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(standard);
            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.StandardHandlers.UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);


            //When  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            await controller.Create(1, sut);
            var result = await controller.Create(1, sut1);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }


        [Fact]
        [Trait("Action", "Add")]
        public async void Add_Shoud_Return_Object_Model_When_Post_Invalid_Object()
        {
            //Given
            var sut = new Domain() { Id = 1, Code = "1", StandardId = 1, Title = "Policies" };

            //When  
            var controller = new DomainsController(_mediator.Object, _logger.Object);
            controller.ViewData.ModelState.AddModelError("Test", "Test");

            //Then
            var result = await controller.Create(1, sut);
            Assert.Equal(1, ((Domain)((ViewResult)result.Result).Model).Id);
        }

        [Fact]
        [Trait("Action", "Add")]
        public async void Add_Shoud_Regturns_Error_When_Raise_Error()
        {

            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.StandardHandlers.UpdateHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());

            //When  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Create(1, new Domain());
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }



        [Fact]
        [Trait("Action", "Update")]
        public async void Edit_Object_Shoud_Return_OK_When_Post_Valid_Object()
        {
            //given
            var sut = new Domain() { Id = 1, Code = "1", StandardId = 1, Title = "Policies" };
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

            //when  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //then
            var result = await controller.Edit(1, 1, sut);
            Assert.IsType<RedirectToActionResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Edit_Should_Returns_BadRequest_On_Object_Mismatch()
        {
            //Given
            var sut = new Domain() { Id = 1, Code = "1", StandardId = 1, Title = "Policies" };

            //When
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(2, 1, sut);
            Assert.IsType<BadRequestResult>(result.Result);
        }


        [Fact]
        [Trait("Action", "Update")]
        public async void Edit_Shoud_Return_Object_Model_When_Post_Invalid_Object()
        {
            //Given
            var sut = new Domain() { Id = 1, Code = "1", StandardId = 1, Title = "Policies" };
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);


            //When  
            var controller = new DomainsController(_mediator.Object, _logger.Object);
            controller.ViewData.ModelState.AddModelError("Test", "Test");

            //Then
            var result = await controller.Edit(1, 1, sut);
            Assert.Equal(1, ((Domain)((ViewResult)result.Result).Model).Id);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Posted_Object_Not_Exsits()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(1, 0, new Domain { Id = 0, StandardId = 1 });
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Posted_Mismatch_Object()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(1, 1, new Domain { Id = 2, StandardId = 1 });
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_OK_When_Get_Valid_Object()
        {
            //Given
            var sut = new Domain() { Id = 1, Code = "1", StandardId = 1, Title = "Policies" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);


            //when  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //then
            var result = await controller.Edit(1, 1);
            Assert.Equal(1, ((Domain)((ViewResult)result.Result).Model).Id);

        }



        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Get_Object_Not_Exsits()
        {

            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync((Domain)null);

            //When  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(1, 0);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Requested_Object_Is_Null()
        {
            //When  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(1, null);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Shoud_Returns_InternalServerError_When_Update_Rais_Error()
        {
            //Given
            var sut = new Domain() { Id = 1, Code = "1", StandardId = 1, Title = "Policies" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());


            //When  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(1, 1, sut);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Shoud_Returns_InternalServerError_When_Get_Update_Page_Rais_Error()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(1, 2);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }



        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_OK_When_Post_Valid_Object()
        {
            //Given
            var sut = new Domain() { Id = 1, Code = "1", StandardId = 1, Title = "Policies" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);
            _mediator.Setup(x => x.Send(It.IsAny<DeleteHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);


            //when  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //then
            var result = await controller.DeleteConfirmed(1);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_Error_When_Posted_Object_Not_Exsits()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<DeleteHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.DeleteConfirmed(0);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_OK_When_Get_Valid_Object()
        {
            //Given
            var sut = new Domain() { Id = 1, Code = "1", StandardId = 1, Title = "Policies" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);

            //when  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //then
            var result = await controller.Delete(1);
            Assert.Equal(1, ((Domain)((ViewResult)result.Result).Model).Id);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_Error_When_Get_Object_Not_Exsits()
        {
            //When  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Delete(0);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_Error_When_Requested_Object_Is_Null()
        {
            //When  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Delete(null);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Shoud_Returns_InternalServerError_When_Delete_Rais_Error()
        {
            //Given
            var sut = new Domain() { Id = 1, Code = "1", StandardId = 1, Title = "Policies" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);
            _mediator.Setup(x => x.Send(It.IsAny<DeleteHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());


            //When  
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.DeleteConfirmed(1);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Shoud_Returns_InternalServerError_When_Get_Delete_Page_Rais_Error()
        {

            //When  
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());
            var controller = new DomainsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Delete(2);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }

    }
}
