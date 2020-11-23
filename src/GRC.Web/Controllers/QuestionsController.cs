using AutoMapper;
using GRC.Core.Entities;
using GRC.Web.Features.QuestionHandlers;
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
    [Route("Controls/{controlId:int}/questions/{action=Index}/{id:int?}")]
    [Authorize(Policy = "Admin")]
    public class QuestionsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<Question> _logger;

        public QuestionsController(IMediator mediator, ILogger<Question> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: Controls/1/Questions
        public async Task<ActionResult<List<Question>>> Index([FromRoute] int controlId)
        {
            try
            {
                var control = await _mediator.Send(new GRC.Web.Features.ControlHandlers.GetByIDHandler.Request(controlId));
                if (control==null)
                {
                    _logger.LogWarning("Requested Control {Id} could not be found.", controlId);
                    return NotFound();
                }
                ViewBag.ControlName = control.FullText;
                ViewBag.DomainId = control.DomainId;
                return View(await _mediator.Send(new GetAllHandler.Request(controlId)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on get List of Questions");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // GET: Controls/1/Questions/Details/5
        public async Task<ActionResult<Question>> Details(int controlId, int? id)
        {
            if (id == null)
                return BadRequest();

            try
            {
                var Question = await _mediator.Send(new GetByIdHandler.Request(controlId, id.Value));
                if (Question == null)
                {
                    _logger.LogWarning("Requested Question {id} could not be found.", id);
                    return NotFound();
                }
                if (Question.ControlId != controlId)
                {
                    _logger.LogWarning("Requested Question {id} is not related to Control {ControlId}", id, controlId);
                    return BadRequest();
                }

                return View(Question);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting Question {Id}", id.Value);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // GET: Controls/1/Questions/Create
        public ActionResult<Question> Create()
        {
            var question = new Question(answerCount: 4);
            return View(question);
        }

        // POST: Controls/1/Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Question>> Create(int controlId, [FromForm] Question question)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var control = await _mediator.Send(new GRC.Web.Features.ControlHandlers.GetByIDHandler.Request(controlId));
                    control.AddQuestion(question);
                    var count = await _mediator.Send(new GRC.Web.Features.ControlHandlers.UpdateHandler.Request(control));
                    if (count>0)
                        return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Adding Question to control {ControlId}", controlId);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return View(question );
        }

        // GET: Controls/1/Questions/Edit/5
        public async Task<ActionResult<Question>> Edit(int controlId, int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();

                var question = await _mediator.Send(new GetByIdHandler.Request(controlId, id.Value));
                if (question == null)
                {
                    _logger.LogWarning("Requested Question {id} could not be found or  is not related to Control {ControlId}", id, controlId);
                    return NotFound();
                }
                return View(question);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting Question {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Controls/1/Questions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Question>> Edit([FromRoute] int controlId, [FromRoute] int id, [FromForm] Question question)
        {
            if (question==null)
            {
                _logger.LogWarning("Editing object Question {id} Can't be null", id);
                return BadRequest();
            }

            if (id != question.Id || controlId != question.ControlId)
            {
                _logger.LogWarning("Editing object isn't match with Question {id}.", id);
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var cnt = await _mediator.Send(new UpdateHandler.Request(question));
                    if (cnt > 0)
                        return RedirectToAction(nameof(Index));
                    else
                    {
                        _logger.LogWarning("Could not update Question {Id}", id);
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    var sc = await _mediator.Send(new GetByIdHandler.Request(controlId, id));
                    if (sc == null)
                    {
                        _logger.LogWarning("Requested Question {id} could not be found.", id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Error occured on Updating Question {Id}", id);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
            }
            return View(question);
        }

        // GET: Controls/1/Questions/Delete/5
        public async Task<ActionResult<Question>> Delete(int controlId,int? id)
        {
            if (id == null)
                return BadRequest();
            try
            {
                var Question = await _mediator.Send(new GetByIdHandler.Request(controlId,id.Value));
                if (Question == null)
                {
                    _logger.LogWarning("Requested Question {id} could not be found.", id);
                    return NotFound();
                }
                return View(Question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting Question {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Controls/1/Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int controlId, int id)
        {
            var Question = await _mediator.Send(new GetByIdHandler.Request(controlId, id));
            if (Question == null)
            {
                _logger.LogWarning("Requested Question {id} could not be found.", id);
                return NotFound();
            }

            try
            {
                var cnt = await _mediator.Send(new DeleteHandler.Request(Question));
                if (cnt > 0)
                    return RedirectToAction((nameof(Index)));
                else
                {
                    _logger.LogError("Could not Delete Question {Id}", id);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Deleting Question {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
