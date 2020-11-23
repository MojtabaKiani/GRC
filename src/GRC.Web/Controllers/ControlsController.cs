using GRC.Core.Entities;
using GRC.Web.Features.ControlHandlers;
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
    [Route("Domains/{domainId:int}/Controls/{action=Index}/{id:int?}")]
    [Authorize(Policy = "Admin")]
    public class ControlsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<Control> _logger;

        public ControlsController(IMediator mediator, ILogger<Control> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: Domains/1/Controls
        public async Task<ActionResult<List<Control>>> Index([FromRoute] int domainId)
        {
            try
            {
                var domain = await _mediator.Send(new GRC.Web.Features.DomainHandlers.GetByIdHandler.Request(domainId));
                if (domain==null)
                {
                    _logger.LogWarning("Requested Domain {Id} could not be found.", domainId);
                    return NotFound();
                }
                ViewBag.DomainName = domain.FullText;
                ViewBag.StandardId = domain.StandardId;
                return View(await _mediator.Send(new GetAllHandler.Request(domainId)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on get List of Controls");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // GET: Domains/1/Controls/Details/5
        public async Task<ActionResult<Control>> Details(int domainId, int? id)
        {
            if (id == null)
                return BadRequest();

            try
            {
                var Control = await _mediator.Send(new GetByIDHandler.Request(id.Value));
                if (Control == null)
                {
                    _logger.LogWarning("Requested Control {id} could not be found.", id);
                    return NotFound();
                }
                if (Control.DomainId != domainId)
                {
                    _logger.LogWarning("Requested Control {id} is not related to Domain {DomainId}", id, domainId);
                    return BadRequest();
                }

                return View(Control);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting Control {Id}", id.Value);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // GET: Domains/1/Controls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Domains/1/Controls/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Control>> Create(int domainId, [FromForm] Control Control)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var domain = await _mediator.Send(new GRC.Web.Features.DomainHandlers.GetByIdHandler.Request(domainId));
                    domain.AddControl(Control);
                    await _mediator.Send(new GRC.Web.Features.DomainHandlers.UpdateHandler.Request(domain));
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Requested Control code already Exists in Domain {DomainId}", domainId);
                return (BadRequest(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Adding Control to domain {DomainId}", domainId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return View(Control);
        }

        // GET: Domains/1/Controls/Edit/5
        public async Task<ActionResult<Control>> Edit(int domainId, int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();

                var Control = await _mediator.Send(new GetByIDHandler.Request(id.Value));
                if (Control == null)
                {
                    _logger.LogWarning("Requested Control {id} could not be found.", id);
                    return NotFound();
                }
                if (Control.DomainId != domainId)
                {
                    _logger.LogWarning("Requested Control {id} is not related to Domain {DomainId}", id, domainId);
                    return BadRequest();
                }
                return View(Control);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting Control {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Domains/1/Controls/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Control>> Edit(int domainId, int id, [FromForm] Control Control)
        {
            if (id != Control.Id)
            {
                _logger.LogWarning("Editing object isn't match with Control {id}.", id);
                return BadRequest();
            }

            if (domainId != Control.DomainId)
            {
                _logger.LogWarning("Editing object isn't match with Domain {id}.", domainId);
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cnt = await _mediator.Send(new UpdateHandler.Request(Control));
                    if (cnt > 0)
                        return RedirectToAction(nameof(Index));
                    else
                    {
                        _logger.LogWarning("Could not update Control {Id}", id);
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    var sc = await _mediator.Send(new GetByIDHandler.Request(id));
                    if (sc == null)
                    {
                        _logger.LogWarning("Requested Control {id} could not be found.", id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Error occured on Updating Control {Id}", id);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
            }
            return View(Control);
        }

        // GET: Domains/1/Controls/Delete/5
        public async Task<ActionResult<Control>> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            try
            {
                var Control = await _mediator.Send(new GetByIDHandler.Request(id.Value));
                if (Control == null)
                {
                    _logger.LogWarning("Requested Control {id} could not be found.", id);
                    return NotFound();
                }
                return View(Control);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting Control {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Domains/1/Controls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Control = await _mediator.Send(new GetByIDHandler.Request(id));
            if (Control == null)
            {
                _logger.LogWarning("Requested Control {id} could not be found.", id);
                return NotFound();
            }

            try
            {
                var cnt = await _mediator.Send(new DeleteHandler.Request(Control));
                if (cnt > 0)
                    return RedirectToAction((nameof(Index)));
                else
                {
                    _logger.LogError("Could not Delete Control {Id}", id);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Deleting Control {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
