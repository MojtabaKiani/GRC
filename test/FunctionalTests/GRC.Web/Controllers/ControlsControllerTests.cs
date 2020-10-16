using Xunit;
using Moq;
using MediatR;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using GRC.Core.Entities;
using GRC.Web.Features.ControlHandlers;
using Microsoft.Extensions.Logging;
using GRC.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FunctionalTests.GRCWeb.Controllers
{
    public class ControlsControllerTests
    {


        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<Control>> _logger;

        public ControlsControllerTests()
        {
            _logger = new Mock<ILogger<Control>>();
            _mediator = new Mock<IMediator>();
        }


        [Fact]
        [Trait("Action", "GetList")]
        public async void GetAll_Shoud_Returns_All_Objects()
        {
            //Given
            var sut = new List<Control>(){
                 new Control(){Id = 1, Code="1", DomainId=1,  Text = "Control1"},
                 new Control(){Id = 1, Code="2", DomainId=1,  Text = "Control2"},
            };
            var domain = new Domain { Id = 1, Title = "Policies" };
            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.DomainHandlers.GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(domain);
            _mediator.Setup(x => x.Send(It.IsAny<GetAllHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);

            //When
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Index(1);
            Assert.Equal(2, ((List<Control>)((ViewResult)result.Result).Model).Count());
        }

        [Fact]
        [Trait("Action", "GetList")]
        public async void GetAll_Shoud_Returns_Error_When_Domain_not_Exists()
        {
            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.DomainHandlers.GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync((Domain)null);

            //When
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Index(1);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "GetList")]
        public async void GetAll_Shoud_Returns_InternalServerError_When_Rais_Error()
        {
            //Given
            var domain = new Domain { Id = 1, Title = "Policies" };
            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.DomainHandlers.GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(domain);
            _mediator.Setup(x => x.Send(It.IsAny<GetAllHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());

            //When  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Index(1);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }



        [Fact]
        [Trait("Action", "GetDetails")]
        public async void GetById_Should_Returns_Object_When_Id_Exists()
        {
            //Given
            var sut = new Control() { Id = 1, Code = "1", DomainId = 1, Text = "Control1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);

            //When
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Details(1, 1);
            Assert.Equal(1, ((Control)((ViewResult)result.Result).Model).Id);
        }

        [Fact]
        [Trait("Action", "GetDetails")]
        public async void GetById_Should_Returns_BadRequest_On_Object_Mismatch()
        {
            //Given
            var sut = new Control() { Id = 1, Code = "1", DomainId = 1, Text = "Control1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);

            //When
            var controller = new ControlsController(_mediator.Object, _logger.Object);

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
            .ReturnsAsync((Control)null);

            //When
            var controller = new ControlsController(_mediator.Object, _logger.Object);

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
            .ReturnsAsync((Control)null);

            //When
            var controller = new ControlsController(_mediator.Object, _logger.Object);

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
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Details(1, 2);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }


        [Fact]
        [Trait("Action", "Add")]
        public async void Add_Shoud_Insert_One_When_Post_Valid_Object()
        {
            //Given
            var sut = new Control() { Id = 1, Code = "1", DomainId = 1, Text = "Control1" };
            var domain = new Domain { Id = 1, Title = "Control1" };

            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.DomainHandlers.GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(domain);
            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.DomainHandlers.UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);


            //When  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Create(1, sut);
            Assert.IsType<RedirectToActionResult>(result.Result);
        }


        [Fact]
        [Trait("Action", "Add")]
        public async void Add_Duplicate_Control_Code_Shoud_Return_Error()
        {
            //Given
            var sut = new Control() { Id = 1, Code = "1", DomainId = 1, Text = "Control1" };
            var sut1 = new Control() { Id = 2, Code = "1", DomainId = 1, Text = "Control2" };
            var domain = new Domain { Id = 1, Title = "Control1" };
            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.DomainHandlers.GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(domain);
            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.DomainHandlers.UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);


            //When  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

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
            var sut = new Control() { Id = 1, Code = "1", DomainId = 1, Text = "Control1" };

            //When  
            var controller = new ControlsController(_mediator.Object, _logger.Object);
            controller.ViewData.ModelState.AddModelError("Test", "Test");

            //Then
            var result = await controller.Create(1, sut);
            Assert.Equal(1, ((Control)((ViewResult)result.Result).Model).Id);
        }

        [Fact]
        [Trait("Action", "Add")]
        public async void Add_Shoud_Regturns_Error_When_Raise_Error()
        {

            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.DomainHandlers.UpdateHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());

            //When  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Create(1, new Control());
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }



        [Fact]
        [Trait("Action", "Update")]
        public async void Edit_Object_Shoud_Return_OK_When_Post_Valid_Object()
        {
            //given
            var sut = new Control() { Id = 1, Code = "1", DomainId = 1, Text = "Control1" };
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);

            //when  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //then
            var result = await controller.Edit(1, 1, sut);
            Assert.IsType<RedirectToActionResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Edit_Should_Returns_BadRequest_On_Object_Mismatch()
        {
            //Given
            var sut = new Control() { Id = 1, Code = "1", DomainId = 1, Text = "Control1" };

            //When
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(2, 1, sut);
            Assert.IsType<BadRequestResult>(result.Result);
        }


        [Fact]
        [Trait("Action", "Update")]
        public async void Edit_Shoud_Return_Object_Model_When_Post_Invalid_Object()
        {
            //Given
            var sut = new Control() { Id = 1, Code = "1", DomainId = 1, Text = "Control1" };
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);


            //When  
            var controller = new ControlsController(_mediator.Object, _logger.Object);
            controller.ViewData.ModelState.AddModelError("Test", "Test");

            //Then
            var result = await controller.Edit(1, 1, sut);
            Assert.Equal(1, ((Control)((ViewResult)result.Result).Model).Id);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Posted_Object_Not_Exsits()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(1, 0, new Control { Id = 0, DomainId = 1 });
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Posted_Mismatch_Object()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(1, 1, new Control { Id = 2, DomainId = 1 });
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_OK_When_Get_Valid_Object()
        {
            //Given
            var sut = new Control() { Id = 1, Code = "1", DomainId = 1, Text = "Control1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);


            //when  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //then
            var result = await controller.Edit(1, 1);
            Assert.Equal(1, ((Control)((ViewResult)result.Result).Model).Id);

        }



        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Get_Object_Not_Exsits()
        {

            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync((Control)null);

            //When  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(1, 0);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Requested_Object_Is_Null()
        {
            //When  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(1, null);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Shoud_Returns_InternalServerError_When_Update_Rais_Error()
        {
            //Given
            var sut = new Control() { Id = 1, Code = "1", DomainId = 1, Text = "Control1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());


            //When  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

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
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(1, 2);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }



        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_OK_When_Post_Valid_Object()
        {
            //Given
            var sut = new Control() { Id = 1, Code = "1", DomainId = 1, Text = "Control1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);
            _mediator.Setup(x => x.Send(It.IsAny<DeleteHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);


            //when  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

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
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.DeleteConfirmed(0);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_OK_When_Get_Valid_Object()
        {
            //Given
            var sut = new Control() { Id = 1, Code = "1", DomainId = 1, Text = "Control1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);

            //when  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //then
            var result = await controller.Delete(1);
            Assert.Equal(1, ((Control)((ViewResult)result.Result).Model).Id);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_Error_When_Get_Object_Not_Exsits()
        {
            //When  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Delete(0);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_Error_When_Requested_Object_Is_Null()
        {
            //When  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Delete(null);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Shoud_Returns_InternalServerError_When_Delete_Rais_Error()
        {
            //Given
            var sut = new Control() { Id = 1, Code = "1", DomainId = 1, Text = "Control1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);
            _mediator.Setup(x => x.Send(It.IsAny<DeleteHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());


            //When  
            var controller = new ControlsController(_mediator.Object, _logger.Object);

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
            var controller = new ControlsController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Delete(2);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }

    }
}
