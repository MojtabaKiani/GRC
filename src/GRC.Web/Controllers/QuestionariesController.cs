using AutoMapper;
using GRC.Core.Entities;
using GRC.Web.Extensions;
using GRC.Web.Features.QuestionaryHandlers;
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
using System.Threading.Tasks;

namespace GRC.Web.Controllers
{
    [Authorize]
    public class QuestionariesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<Questionary> _logger;
        private readonly IMapper _mapper;

        public QuestionariesController(IMediator mediator, ILogger<Questionary> logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: Questionaries
        public async Task<ActionResult<List<Questionary>>> Index()
        {
            try
            {
                return View(await _mediator.Send(new GetAllHandler.Request(User.Identity.Name, User.IsAdministrator())));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on get List of questionaries");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // GET: Questionaries/Details/5
        public async Task<ActionResult<QuestionaryViewModel>> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            try
            {
                var questionary = await _mediator.Send(new GetByIdFullIncludeHandler.Request(id.Value));
                if (questionary == null)
                {
                    _logger.LogWarning("Requested questionary {id} could not be found or isn't related to user {UserName}.", id, User.Identity.Name);
                    return NotFound();
                }

                bool UserHasAccess = questionary.OwnerUid == User.Identity.Name && User.IsAdministrator();
                if (!UserHasAccess)
                {
                    _logger.LogWarning("Requested questioary {id} doesn't belong to user {UserName}.", id, User.Identity.Name);
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
                var domains = await _mediator.Send(new GRC.Web.Features.DomainHandlers.GetDomainWithQuestionCountHandler.Request(questionary.StandardId));
                questionary.CalculateResult(domains);
                var questionaryViewModel = _mapper.Map<QuestionaryViewModel>(questionary);
                return View(questionaryViewModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting questionary {Id} details by user {UserName}", id.Value, User.Identity.Name);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // GET: Questionaries/Create
        public async Task<ActionResult<QuestionaryViewModel>> Create()
        {
            try
            {
                var questionaryViewModel = await AddStandardListToModel();
                return View(questionaryViewModel);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting standard list for");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Questionaries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<QuestionaryViewModel>> Create([FromForm] QuestionaryViewModel questionaryViewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var questionary = new Questionary(User.Identity.Name);
                    _mapper.Map<QuestionaryViewModel, Questionary>(questionaryViewModel, questionary);
                    await _mediator.Send(new CreateHandler.Request(questionary));
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on creating questionary");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            await AddStandardListToModel(questionaryViewModel);
            return View(questionaryViewModel);
        }

        // GET: Questionaries/Edit/5
        public async Task<ActionResult<QuestionaryViewModel>> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();

                var questionary = await _mediator.Send(new GetByIDHandler.Request(id.Value, User.Identity.Name));
                if (questionary == null)
                {
                    _logger.LogWarning("Requested questionary {id} could not be found or isn't related to user {UserName}.", id, User.Identity.Name);
                    return NotFound();
                }
                var questionaryViewModel = await AddStandardListToModel(_mapper.Map<QuestionaryViewModel>(questionary));
                return View(questionaryViewModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting questionary {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Questionaries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<QuestionaryViewModel>> Edit(int id, [FromForm] QuestionaryViewModel questionary)
        {
            if (questionary==null)
            {
                _logger.LogWarning("Editing object Can not be null");
                return BadRequest();
            }

            if (id != questionary.Id)
            {
                _logger.LogWarning("Editing object isn't match with questionary {id}.", id);
                return BadRequest();
            }

            try
            {
                var savedQuestionary = await _mediator.Send(new GetByIDHandler.Request(id, User.Identity.Name));
                if (savedQuestionary == null)
                {
                    _logger.LogWarning("Requested questionary {id} could not be found or isn't related to user {UserName}.", id, User.Identity.Name);
                    return NotFound();
                }

                if (User.Identity.Name != savedQuestionary.OwnerUid)
                {
                    _logger.LogWarning("Editing questioary {id} doesn't belong to user {UserName}.", id, User.Identity.Name);
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }

                if (ModelState.IsValid)
                {

                    _mapper.Map<QuestionaryViewModel, Questionary>(questionary, savedQuestionary);
                    var result = await _mediator.Send(new UpdateHandler.Request(savedQuestionary));
                    if (result > 0)
                        return RedirectToAction(nameof(Index));
                    else
                    {
                        _logger.LogWarning("Could not update questionary {Id}", id);
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Updating questionary {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var questionaryViewModel = await AddStandardListToModel(questionary);
            return View(questionaryViewModel);
        }

        // GET: Questionaries/Delete/5
        public async Task<ActionResult<Questionary>> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            try
            {
                var questionary = await _mediator.Send(new GetByIDHandler.Request(id.Value, User.Identity.Name));
                if (questionary == null)
                {
                    _logger.LogWarning("Requested questionary {id} could not be found or isn't related to user {UserName}.", id, User.Identity.Name);
                    return NotFound();
                }
                return View(questionary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Getting questionary {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Questionaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionary = await _mediator.Send(new GetByIDHandler.Request(id, User.Identity.Name));
            if (questionary == null)
            {
                _logger.LogWarning("Requested questionary {id} could not be found or isn't related to user {UserName}.", id, User.Identity.Name);
                return NotFound();
            }

            try
            {
                var cnt = await _mediator.Send(new DeleteHandler.Request(questionary));
                if (cnt > 0)
                    return RedirectToAction((nameof(Index)));
                else
                {
                    _logger.LogError("Could not Delete questionary {Id}", id);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on Deleting questionary {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        private async Task<QuestionaryViewModel> AddStandardListToModel(QuestionaryViewModel questionary=null)
        {
            var standardList = await _mediator.Send(new GRC.Web.Features.StandardHandlers.GetAllHandler.Request());
            if (questionary == null)
                questionary = new QuestionaryViewModel(); 
            if (standardList !=null && standardList?.Count>0)
                questionary.Standards = standardList.Select(q => new SelectListItem { Value = q.Id.ToString(), Text = q.FullName });
           
            return questionary;
        }
    }
}
