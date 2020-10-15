using Xunit;
using Moq;
using MediatR;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using GRC.Core.Entities;
using GRC.Web.Features.StandardCategoryHandlers;
using Microsoft.Extensions.Logging;
using GRC.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FunctionalTests.GRCWeb.Controllers
{
    public class StandardCategoryControllerTests
    {

        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILogger<StandardCategory>> _logger;

        public StandardCategoryControllerTests()
        {
            _logger = new Mock<ILogger<StandardCategory>>();
            _mediator = new Mock<IMediator>();
        }


        [Fact]
        [Trait("Action","GetList")]
        public async void GetAll_Shoud_Returns_All_Objects()
        {
            //Given
            var scs = new List<StandardCategory>(){
                 new StandardCategory(){Id = 1,  Title = "IT"},
                 new StandardCategory(){Id = 2, Title = "Cyber Security"},
                 new StandardCategory(){Id = 3, Title = "Programming"}
            };
            _mediator.Setup(x => x.Send(It.IsAny<GetAllHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(scs);

            //When
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Index();
            Assert.Equal(3, ((List<StandardCategory>)((ViewResult)result.Result).Model).Count());
        }

        [Fact]
        [Trait("Action", "GetList")]
        public async void GetAll_Shoud_Returns_InternalServerError_When_Rais_Error()
        {

            //When  
            _mediator.Setup(x => x.Send(It.IsAny<GetAllHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Index();
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }



        [Fact]
        [Trait("Action","GetDetails")]
        public async void GetById_Should_Returns_Object_When_Id_Exists()
        {
            //Given
            var sc = new StandardCategory
            {
                Id = 1,
                Title = "IT"
            };
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sc);

            //When
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Details(1);
            Assert.Equal(1, ((StandardCategory)((ViewResult)result.Result).Model).Id);
        }

        [Fact]
        [Trait("Action","GetDetails")]
        public async void GetById_Should_Returns_NotFound_When_Id_Not_Exists()
        {
            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((StandardCategory)null);

            //When
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Details(1);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action","GetDetails")]
        public async void GetById_Should_Returns_BadRequest_When_Id_Is_Null()
        {
            //Given
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((StandardCategory)null);

            //When
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Details(null);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "GetDetails")]
        public async void GetById_Shoud_Returns_InternalServerError_When_Rais_Error()
        {

            //When  
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Details(2);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }


        [Fact]
        [Trait("Action","Add")]
        public async void Add_Shoud_Insert_One_When_Post_Valid_Contact()
        {
            //Given
            var sc = new StandardCategory()
            {
                Id = 2,
                Title="IT"
            };

            //When  
            _mediator.Setup(x => x.Send(It.IsAny<CreateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sc);
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Create(sc);
            Assert.IsType<RedirectToActionResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Add")]
        public async void Add_Shoud_Regturns_Error_When_Raise_Error()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<CreateHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Create(new StandardCategory());
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }



        [Fact]
        [Trait("Action", "Update")]
        public async void Edit_Object_Shoud_Return_OK_When_Post_Valid_Object()
        {
            //given
            var sc = new StandardCategory
            {
                Id = 2,
                Title="IT"
            };

            //when  
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //then
            var result = await controller.Edit(2, sc);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Posted_Object_Not_Exsits()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(0,new StandardCategory { Id = 0 });
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Posted_Mismatch_Object()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(1, new StandardCategory { Id = 2 });
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_OK_When_Get_Valid_Object()
        {
            var sc = new StandardCategory
            {
                Id = 2,
                Title = "IT"
            };

            //when  
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sc);
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //then
            var result = await controller.Edit(2);
            Assert.Equal(2, ((StandardCategory)((ViewResult)result.Result).Model).Id);
        }



        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Get_Object_Not_Exsits()
        {
            //When  
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(0);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Object_Shoud_Returns_Error_When_Requested_Object_Is_Null()
        {
            //When  
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(null);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Shoud_Returns_InternalServerError_When_Update_Rais_Error()
        {
            //Given
            var sc = new StandardCategory
            {
                Id = 2,
                Title = "IT"
            };

            //When  
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sc);
            _mediator.Setup(x => x.Send(It.IsAny<UpdateHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(2,sc);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }

        [Fact]
        [Trait("Action", "Update")]
        public async void Update_Shoud_Returns_InternalServerError_When_Get_Update_Page_Rais_Error()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Edit(2);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }



        [Fact]
        [Trait("Action","Delete")]
        public async void Delete_Object_Shoud_Returns_OK_When_Post_Valid_Object()
        {
            var sc = new StandardCategory
            {
                Id = 2,
                Title = "IT"
            };

            //when  
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sc);
            _mediator.Setup(x => x.Send(It.IsAny<DeleteHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //then
            var result = await controller.DeleteConfirmed(2);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        [Trait("Action","Delete")]
        public async void Delete_Object_Shoud_Returns_Error_When_Posted_Object_Not_Exsits()
        {
            //When  
            _mediator.Setup(x => x.Send(It.IsAny<DeleteHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.DeleteConfirmed(0);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_OK_When_Get_Valid_Object()
        {
            var sc = new StandardCategory
            {
                Id = 2,
                Title = "IT"
            };

            //when  
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sc);
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //then
            var result = await controller.Delete(2);
            Assert.Equal(2, ((StandardCategory)((ViewResult)result.Result).Model).Id);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_Error_When_Get_Object_Not_Exsits()
        {
            //When  
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Delete(0);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Object_Shoud_Returns_Error_When_Requested_Object_Is_Null()
        {
            //When  
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Delete(null);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Shoud_Returns_InternalServerError_When_Delete_Rais_Error()
        {
            //Given
            var sc = new StandardCategory
            {
                Id = 2,
                Title = "IT"
            };

            //When  
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).ReturnsAsync(sc);
            _mediator.Setup(x => x.Send(It.IsAny<DeleteHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.DeleteConfirmed(2);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }

        [Fact]
        [Trait("Action", "Delete")]
        public async void Delete_Shoud_Returns_InternalServerError_When_Get_Delete_Page_Rais_Error()
        {

            //When  
            _mediator.Setup(x => x.Send(It.IsAny<GetByIDHandler.Request>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());
            var controller = new StandardCategoriesController(_mediator.Object, _logger.Object);

            //Then
            var result = await controller.Delete(2);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result.Result).StatusCode);
        }

    }
}
