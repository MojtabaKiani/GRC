using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GRC.Core.Entities;
using GRC.Core.Interfaces;
using MediatR;
using GRC.Web.Features.StandardCategoryHandlers;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace GRC.Web.Controllers
{
    [Authorize]
    public class StandardCategoriesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StandardCategory> _logger;

        public StandardCategoriesController(IMediator mediator, ILogger<StandardCategory> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: StandardCategories
        public async Task<ActionResult<List<StandardCategory>>> Index()
        {
            try
            {
                return View(await _mediator.Send(new GetAllHandler.Request()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on get List of standard categories");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // GET: StandardCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StandardCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<StandardCategory>> Create([FromForm] StandardCategory standardCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(new CreateHandler.Request(standardCategory));
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on creating standard category");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return View(standardCategory);
        }

        // GET: StandardCategories/Edit/5
        public async Task<ActionResult<StandardCategory>> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();

                var standardCategory = await _mediator.Send(new GetByIDHandler.Request(id.Value));
                if (standardCategory == null)
                {
                    _logger.LogWarning("Requested standard category {id} could not be found.", id);
                    return NotFound();
                }
                return View(standardCategory);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting standard category {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: StandardCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Id")] StandardCategory standardCategory)
        {
            if (id != standardCategory.Id)
            {
                _logger.LogWarning("Editing object isn't match with standard category {id}.", id);
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cnt = await _mediator.Send(new UpdateHandler.Request(standardCategory));
                    if (cnt > 0)
                        return RedirectToAction(nameof(Index));
                    else
                    {
                        _logger.LogWarning("Could not update standard category {Id}", id);
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    var sc = await _mediator.Send(new GetByIDHandler.Request(id));
                    if (sc == null)
                    {
                        _logger.LogWarning("Requested standard category {id} could not be found.", id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Error occured on Updating standard category {Id}", id);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }

            }
            return View(standardCategory);
        }

        // GET: StandardCategories/Delete/5
        public async Task<ActionResult<StandardCategory>> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            try
            {
                var standardCategory = await _mediator.Send(new GetByIDHandler.Request(id.Value));
                if (standardCategory == null)
                {
                    _logger.LogWarning("Requested standard category {id} could not be found.", id);
                    return NotFound();
                }
                return View(standardCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting standard category {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: StandardCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var standardCategory = await _mediator.Send(new GetByIDHandler.Request(id));
            if (standardCategory == null)
            {
                _logger.LogWarning("Requested standard category {id} could not be found.", id);
                return NotFound();
            }

            try
            {
                var cnt = await _mediator.Send(new DeleteHandler.Request(standardCategory));
                if (cnt > 0)
                    return RedirectToAction((nameof(Index)));
                else
                {
                    _logger.LogError("Could not Delete standard category {Id}", id);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Deleting standard category {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
