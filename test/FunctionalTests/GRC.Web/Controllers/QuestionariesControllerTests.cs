using Xunit;
using Moq;
using MediatR;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using GRC.Core.Entities;
using GRC.Web.Features.QuestionaryHandlers;
using Microsoft.Extensions.Logging;
using GRC.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using GRC.Web.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

namespace FunctionalTests.GRCWeb.Controllers
{
    public class QuestionariesControllerTests
    {


        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<Questionary>> _logger;
        private ControllerContext _controllerContext;

        public QuestionariesControllerTests()
        {
            _logger = new Mock<ILogger<Questionary>>();
            _mediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();
            var identity = new GenericIdentity("Owner", "test");
            var contextUser = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext() { User = contextUser };
            _controllerContext = new ControllerContext() { HttpContext = httpContext };
        }


        [Fact]
        [Trait("Action", "GetList")]
        public async void GetAll_Shoud_Returns_All_Objects()
        {
            //Given
            var sut = new List<Questionary>(){
                 new Questionary("Owner"){Id = 1, StandardId=1, ComplianceLevel=5},
                 new Questionary("Owner"){Id = 2, StandardId=1, ComplianceLevel=5},
            };
            _mediator.Setup(x => x.Send(It.IsAny<GetAllHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);

            //When
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Index();
            Assert.Equal(2, ((List<Questionary>)((ViewResult)result.Result).Model).Count());
        }

        [Fact]
        [Trait("Action", "GetList")]
        public async void GetAll_Shoud_Returns_InternalServerError_When_Rais_Error()
        {
            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GetAllHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());

            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Index();
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }



        [Fact]
        [Trait("Action", "GetDetails")]
        public async void GetById_Should_Returns_Object_When_Id_Exists()
        {
            //Given
            var sut = new QuestionaryViewModel() { Id = 1, StandardId = 1, ComplianceLevel = 5, Name="Questionary" };
            var questionary = new Questionary("Owner") { Id = 1, StandardId = 1, ComplianceLevel = 5, Name="Questionary" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIdFullIncludeHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(questionary);
            _mapper.Setup(m => m.Map<QuestionaryViewModel>(It.IsAny<Questionary>())).Returns(sut);
           
            //When
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Details(1);
            Assert.Equal(1, ((QuestionaryViewModel)((ViewResult)result.Result).Model).Id);
            Assert.Equal(5, ((QuestionaryViewModel)((ViewResult)result.Result).Model).ComplianceLevel);

        }

        [Fact]
        [Trait("Action", "GetDetails")]
        public async void GetById_Should_Returns_BadRequest_When_Id_Is_Null()
        {
            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GetByIdFullIncludeHandler.Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Questionary)null);

            //When
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object);

            //Then
            var result = await controller.Details(null);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "GetDetails")]
        public async void GetById_Should_Returns_NotFound_If_User_Isnot_The_Object_Owner()
        {
            //Given
            var sut = new Questionary("MainOwner") { Id = 1, StandardId = 1, ComplianceLevel = 5 };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIdFullIncludeHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);

            //When
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Details(1);
            Assert.Equal(StatusCodes.Status401Unauthorized, ((StatusCodeResult)result.Result).StatusCode);

        }

        [Fact]
        [Trait("Action", "GetDetails")]
        public async void GetById_Should_Returns_NotFound_When_Id_Not_Exists()
        {
            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GetByIdFullIncludeHandler.Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Questionary)null);

            //When
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Details(1);
            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        [Trait("Action", "GetDetails")]
        public async void GetById_Shoud_Returns_InternalServerError_When_Raise_Error()
        {
            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GetByIdFullIncludeHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());

            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Details(1);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }


        [Fact]
        [Trait("Action", "Add")]
        public async void Add_Shoud_Insert_One_When_Post_Valid_Object()
        {
            //Given
            var sut = new QuestionaryViewModel { Id = 1, StandardId = 1, ComplianceLevel = 5 };
            var questionary = new Questionary("Owner") { Id = 1, StandardId = 1, ComplianceLevel = 5 };
            _mediator.Setup(x => x.Send(It.IsAny<CreateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(questionary);

            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Create(sut);
            Assert.IsType<RedirectToActionResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Add")]
        public async void Add_StandardList_To_QuestionaryViewModel_Should_Work_Properly()
        {
            //Given
            var sut = new QuestionaryViewModel { Id = 1, StandardId = 1, ComplianceLevel = 5 };
            var standards = new List<Standard>()
            {
                new Standard{Id=1, StandardCategoryId=1},
                new Standard{Id=2, StandardCategoryId=1},
                new Standard{Id=3, StandardCategoryId=1}
            };
            var questionary = new Questionary("Owner") { Id = 1, StandardId = 1, ComplianceLevel = 5 };
            _mediator.Setup(x => x.Send(It.IsAny<CreateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(questionary);
            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.StandardHandlers.GetAllHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(standards);

            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object);

            //Then
            var result = await controller.Create();
            Assert.Equal(3, ((QuestionaryViewModel)((ViewResult)result.Result).Model).Standards.Count());
        }

        [Fact]
        [Trait("Action", "Add")]
        public async void Add_Shoud_Return_Object_Model_When_Post_Invalid_Object()
        {
            //Given
            var sut = new QuestionaryViewModel { Id = 1, StandardId = 1, ComplianceLevel = 5 };
            var questionary = new Questionary("Owner") { Id = 1, StandardId = 1, ComplianceLevel = 5 };
            _mediator.Setup(x => x.Send(It.IsAny<CreateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(questionary);

            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };
            controller.ViewData.ModelState.AddModelError("Test", "Test");

            //Then
            var result = await controller.Create(sut);
            Assert.Equal(1, ((QuestionaryViewModel)((ViewResult)result.Result).Model).Id);
        }

        [Fact]
        [Trait("Action", "Add")]
        public async void Add_Shoud_Regturns_Error_When_Raise_Error()
        {

            //Given
            _mediator.Setup(x => x.Send(It.IsAny<CreateHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());

            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Create(new QuestionaryViewModel());
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }

        [Fact]
        [Trait("Action", "Add")]
        public async void Add_Shoud_Regturns_Error_When_GetStandardList_Raise_Error()
        {

            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GRC.Web.Features.StandardHandlers.GetAllHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());

            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object);

            //Then
            var result = await controller.Create(new QuestionaryViewModel());
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }



        [Fact]
        [Trait("Action", "Update")]
        public async void Edit_Object_Shoud_Return_OK_When_Post_Valid_Object()
        {
            //given
            var sut = new QuestionaryViewModel { Id = 1, StandardId = 1, ComplianceLevel = 5, Name="Questionary1" };
            var questionary = new Questionary("Owner") { Id = 1, StandardId = 1, ComplianceLevel = 5, Name="Questionary1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(questionary);
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mapper.Setup(m => m.Map<Questionary>(It.IsAny<QuestionaryViewModel>())).Returns(questionary);

            //when  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //then
            var result = await controller.Edit(1, sut);
            Assert.IsType<RedirectToActionResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Edit_Should_Returns_BadRequest_On_Object_Mismatch()
        {
            //Given
            var sut = new QuestionaryViewModel { Id = 1, StandardId = 1, ComplianceLevel = 5, Name = "Questionary1" };

            //When
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Edit(2, sut);
            Assert.IsType<BadRequestResult>(result.Result);
        }


        [Fact]
        [Trait("Action", "Update")]
        public async void Edit_Shoud_Return_Object_Model_When_Post_Invalid_Object()
        {
            //Given
            var sut = new QuestionaryViewModel { Id = 1, StandardId = 1, ComplianceLevel = 5, Name = "Questionary1" };
            var questionary = new Questionary("Owner") { Id = 1, StandardId = 1, ComplianceLevel = 5, Name="Questionary1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(questionary);


            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };
            controller.ViewData.ModelState.AddModelError("Test", "Test");

            //Then
            var result = await controller.Edit(1, sut);
            Assert.Equal(1, ((QuestionaryViewModel)((ViewResult)result.Result).Model).Id);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Posted_Object_Not_Exsits()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync((Questionary)null);
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Edit(1, new QuestionaryViewModel { Id = 1, StandardId = 1 });
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Posted_Object_Isnot_Owned_By_Sender()
        {
            //When  
            var questionary = new Questionary("MainOwner") { Id = 1, StandardId = 1, ComplianceLevel = 5, Name = "Questionary1" };
            var sut = new QuestionaryViewModel { Id = 1, StandardId = 1, ComplianceLevel = 5, Name = "Questionary1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(questionary);
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Edit(1, sut);
            Assert.Equal(StatusCodes.Status401Unauthorized, ((StatusCodeResult)result.Result).StatusCode);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Requested_Object_Is_Null()
        {
            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object);

            //Then
            var result = await controller.Edit(1, null);
            Assert.IsType<BadRequestResult>(result.Result);

        }


        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Get_Id_Is_Null()
        {
            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object);

            //Then
            var result = await controller.Edit(null);
            Assert.IsType<BadRequestResult>(result.Result);

        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_OK_When_Get_Valid_Object()
        {
            //Given
            var sut = new QuestionaryViewModel { Id = 1, StandardId = 1, ComplianceLevel = 5, Name = "Questionary1" };
            var questionary = new Questionary { Id = 1, StandardId = 1, ComplianceLevel = 5, Name = "Questionary1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(questionary);
            _mapper.Setup(q => q.Map<QuestionaryViewModel>(It.IsAny<Questionary>())).Returns(sut);

            //when  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //then
            var result = await controller.Edit(1);
            Assert.Equal(1, ((QuestionaryViewModel)((ViewResult)result.Result).Model).Id);

        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Get_Object_Not_Exsits()
        {

            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync((Questionary)null);

            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Edit(1);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Shoud_Returns_InternalServerError_When_Update_Rais_Error()
        {
            //Given
            var questionary = new Questionary("Owner") { Id = 1, StandardId = 1, ComplianceLevel = 5, Name = "Questionary1" };
            var sut = new QuestionaryViewModel { Id = 1, StandardId = 1, ComplianceLevel = 5, Name = "Questionary1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(questionary);
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());


            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Edit(1, sut);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Shoud_Returns_InternalServerError_When_Get_Update_Page_Rais_Error()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Edit(1);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_OK_When_Post_Valid_Object()
        {
            //Given
            var questionary = new Questionary("MainOwner") { Id = 1, StandardId = 1, ComplianceLevel = 5, Name = "Questionary1" };
            var sut = new QuestionaryViewModel { Id = 1, StandardId = 1, ComplianceLevel = 5, Name = "Questionary1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(questionary);
            _mediator.Setup(x => x.Send(It.IsAny<DeleteHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);


            //when  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //then
            var result = await controller.DeleteConfirmed(1);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_Error_When_Posted_Object_Not_Exsits()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync((Questionary)null);
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.DeleteConfirmed(0);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_OK_When_Get_Valid_Object()
        {
            //Given
            var sut = new Questionary("MainOwner") { Id = 1, StandardId = 1, ComplianceLevel = 5, Name = "Questionary1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sut);

            //when  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //then
            var result = await controller.Delete(1);
            Assert.Equal(1, ((Questionary)((ViewResult)result.Result).Model).Id);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_Error_When_Get_Object_Not_Exsits()
        {
            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Delete(0);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_Error_When_Requested_Object_Is_Null()
        {
            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Delete(null);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Shoud_Returns_InternalServerError_When_Delete_Rais_Error()
        {
            //Given
            var questionary = new Questionary("MainOwner") { Id = 1, StandardId = 1, ComplianceLevel = 5, Name = "Questionary1" };
            var sut = new QuestionaryViewModel { Id = 1, StandardId = 1, ComplianceLevel = 5, Name = "Questionary1" };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(questionary);
            _mediator.Setup(x => x.Send(It.IsAny<DeleteHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());


            //When  
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

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
            var controller = new QuestionariesController(_mediator.Object, _logger.Object, _mapper.Object) { ControllerContext = _controllerContext };

            //Then
            var result = await controller.Delete(2);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }

    }
}
