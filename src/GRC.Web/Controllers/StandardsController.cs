using GRC.Core.Entities;
using GRC.Web.Features.StandardHandlers;
using GRC.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace GRC.Web.Controllers
{
    [Authorize]
    public class StandardsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<Standard> _logger;

        public StandardsController(IMediator mediator, ILogger<Standard> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: Standards
        public async Task<ActionResult<List<Standard>>> Index()
        {
            try
            {
                return View(await _mediator.Send(new GetAllHandler.Request()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on get List of standards");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // GET: Standards/Details/5
        public async Task<ActionResult<Standard>> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            try
            {
                var Standard = await _mediator.Send(new GetByIDHandler.Request(id.Value));
                if (Standard == null)
                {
                    _logger.LogWarning("Requested standard {id} could not be found.", id);
                    return NotFound();
                }

                return View(Standard);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting standard {Id}", id.Value);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // GET: Standards/Create
        public async Task<ActionResult<StandardViewModel>> Create()
        {
            try
            {
                var svm = await GetScList();
                return View(svm);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting standard cateories list");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Standards/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<StandardViewModel>> Create([FromForm] Standard Standard)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _mediator.Send(new CreateHandler.Request(Standard));
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on creating standard");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var svm = await GetScList(Standard);
            return View(svm);
        }

        // GET: Standards/Edit/5
        public async Task<ActionResult<StandardViewModel>> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();

                var Standard = await _mediator.Send(new GetByIDHandler.Request(id.Value));
                if (Standard == null)
                {
                    _logger.LogWarning("Requested standard {id} could not be found.", id);
                    return NotFound();
                }
                var svm = await GetScList(Standard);
                return View(svm);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting standard {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Standards/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<StandardViewModel>> Edit(int id, [FromForm] Standard Standard)
        {
            if (id != Standard.Id)
            {
                _logger.LogWarning("Editing object isn't match with standard {id}.", id);
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cnt = await _mediator.Send(new UpdateHandler.Request(Standard));
                    if (cnt > 0)
                        return RedirectToAction(nameof(Index));
                    else
                    {
                        _logger.LogWarning("Could not update standard {Id}", id);
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    var sc = await _mediator.Send(new GetByIDHandler.Request(id));
                    if (sc == null)
                    {
                        _logger.LogWarning("Requested standard {id} could not be found.", id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Error occured on Updating standard {Id}", id);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }

            }
            var svm = await GetScList(Standard);
            return View(svm);
        }

        // GET: Standards/Delete/5
        public async Task<ActionResult<Standard>> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            try
            {
                var Standard = await _mediator.Send(new GetByIDHandler.Request(id.Value));
                if (Standard == null)
                {
                    _logger.LogWarning("Requested standard {id} could not be found.", id);
                    return NotFound();
                }
                return View(Standard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting standard {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Standards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Standard = await _mediator.Send(new GetByIDHandler.Request(id));
            if (Standard == null)
            {
                _logger.LogWarning("Requested standard {id} could not be found.", id);
                return NotFound();
            }

            try
            {
                var cnt = await _mediator.Send(new DeleteHandler.Request(Standard));
                if (cnt > 0)
                    return RedirectToAction((nameof(Index)));
                else
                {
                    _logger.LogError("Could not Delete standard {Id}", id);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Deleting standard {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        private async Task<StandardViewModel> GetScList(Standard standard=null)
        {
            var scl = await _mediator.Send(new GRC.Web.Features.StandardCategoryHandlers.GetAllHandler.Request());
            var svm = new StandardViewModel
            {
                StandardCategories = new SelectList(scl, "Id", "Title"),
                Standard = standard
            };
            return svm;
        }
    }
}
