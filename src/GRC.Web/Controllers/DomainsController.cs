using GRC.Core.Entities;
using GRC.Web.Features.DomainHandlers;
using GRC.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GRC.Web.Controllers
{
    [Route("Standards/{standardId:int}/domains/{action=Index}/{id:int?}")]
    [Authorize]
    public class DomainsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<Domain> _logger;

        public DomainsController(IMediator mediator, ILogger<Domain> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: Standards/1/Domains
        public async Task<ActionResult<List<Domain>>> Index([FromRoute] int standardId)
        {
            try
            {
                return View(await _mediator.Send(new GetAllHandler.Request(standardId)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on get List of Domains");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // GET: Standards/1/Domains/Details/5
        public async Task<ActionResult<Domain>> Details(int standardId, int? id)
        {
            if (id == null)
                return BadRequest();

            try
            {
                var Domain = await _mediator.Send(new GetByIDHandler.Request(id.Value));
                if (Domain == null)
                {
                    _logger.LogWarning("Requested Domain {id} could not be found.", id);
                    return NotFound();
                }
                if (Domain.StandardId != standardId)
                {
                    _logger.LogWarning("Requested Domain {id} is not related to Standard {StandardId}", id, standardId);
                    return BadRequest();
                }

                return View(Domain);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting Domain {Id}", id.Value);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // GET: Standards/1/Domains/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Standards/1/Domains/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Domain>> Create(int standardId, [FromForm] Domain Domain)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var standard = await _mediator.Send(new GRC.Web.Features.StandardHandlers.GetByIDHandler.Request(standardId));
                    standard.AddDomains(Domain);
                    await _mediator.Send(new GRC.Web.Features.StandardHandlers.UpdateHandler.Request(standard));
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Requested Domain code already Exists in Standard {StandardId}", standardId);
                return (BadRequest(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Adding Domain to standard {StandardId}", standardId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return View(Domain);
        }

        // GET: Standards/1/Domains/Edit/5
        public async Task<ActionResult<Domain>> Edit(int standardId, int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();

                var Domain = await _mediator.Send(new GetByIDHandler.Request(id.Value));
                if (Domain == null)
                {
                    _logger.LogWarning("Requested Domain {id} could not be found.", id);
                    return NotFound();
                }
                if (Domain.StandardId != standardId)
                {
                    _logger.LogWarning("Requested Domain {id} is not related to Standard {StandardId}", id, standardId);
                    return BadRequest();
                }
                return View(Domain);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting Domain {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Standards/1/Domains/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Domain>> Edit(int standardId, int id, [FromForm] Domain Domain)
        {
            if (id != Domain.Id)
            {
                _logger.LogWarning("Editing object isn't match with Domain {id}.", id);
                return BadRequest();
            }

            if (standardId != Domain.StandardId)
            {
                _logger.LogWarning("Editing object isn't match with Standard {id}.", standardId);
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cnt = await _mediator.Send(new UpdateHandler.Request(Domain));
                    if (cnt > 0)
                        return RedirectToAction(nameof(Index));
                    else
                    {
                        _logger.LogWarning("Could not update Domain {Id}", id);
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    var sc = await _mediator.Send(new GetByIDHandler.Request(id));
                    if (sc == null)
                    {
                        _logger.LogWarning("Requested Domain {id} could not be found.", id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Error occured on Updating Domain {Id}", id);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
            }
            return View(Domain);
        }

        // GET: Standards/1/Domains/Delete/5
        public async Task<ActionResult<Domain>> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            try
            {
                var Domain = await _mediator.Send(new GetByIDHandler.Request(id.Value));
                if (Domain == null)
                {
                    _logger.LogWarning("Requested Domain {id} could not be found.", id);
                    return NotFound();
                }
                return View(Domain);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting Domain {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Standards/1/Domains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Domain = await _mediator.Send(new GetByIDHandler.Request(id));
            if (Domain == null)
            {
                _logger.LogWarning("Requested Domain {id} could not be found.", id);
                return NotFound();
            }

            try
            {
                var cnt = await _mediator.Send(new DeleteHandler.Request(Domain));
                if (cnt > 0)
                    return RedirectToAction((nameof(Index)));
                else
                {
                    _logger.LogError("Could not Delete Domain {Id}", id);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Deleting Domain {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
