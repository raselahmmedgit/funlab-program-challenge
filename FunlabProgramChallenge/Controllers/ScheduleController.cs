using FunlabProgramChallenge.Core;
using FunlabProgramChallenge.Extensions;
using FunlabProgramChallenge.Helpers;
using FunlabProgramChallenge.Managers;
using FunlabProgramChallenge.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FunlabProgramChallenge.Controllers
{
    public class ScheduleController : BaseController
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IMemberManager _iMemberManager;
        private readonly IScheduleManager _iScheduleManager;

        public ScheduleController(IMemberManager iMemberManager, IScheduleManager iScheduleManager, ILogger<ScheduleController> logger)
        {
            _iMemberManager = iMemberManager;
            _iScheduleManager = iScheduleManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "Index"));
                return ErrorView(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddAjax(ScheduleViewModel viewModel)
        {
            try
            {
                if (viewModel != null)
                {
                    DateTime? startDateFormated = viewModel.StartDate.ToFormatedDateFromStartWithMonth();
                    DateTime startDateTime = DateTime.Parse(startDateFormated.ToFormatedDateString() + " " + viewModel.StartTime, CultureInfo.InvariantCulture);
                    DateTime.SpecifyKind(startDateTime, DateTimeKind.Local);
                    DateTime startDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(startDateTime);
                    viewModel.StartDateTime = startDateTimeUtc;
                    viewModel.StartDate = startDateTimeUtc.ToFormatedDateString();
                    viewModel.StartTime = startDateTimeUtc.ToFormatedShortTimeWithAmPmString();

                    DateTime? endDateFormated = viewModel.EndDate.ToFormatedDateFromStartWithMonth();
                    DateTime endDateTime = DateTime.Parse(endDateFormated.ToFormatedDateString() + " " + viewModel.EndTime, CultureInfo.InvariantCulture);
                    DateTime.SpecifyKind(endDateTime, DateTimeKind.Local);
                    DateTime endDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(endDateTime);
                    viewModel.EndDateTime = endDateTimeUtc;
                    viewModel.EndDate = endDateTimeUtc.ToFormatedDateString();
                    viewModel.EndTime = endDateTimeUtc.ToFormatedShortTimeWithAmPmString();

                    var scheduleViewModelList = await _iScheduleManager.GetSchedulesAsync();
                    var scheduleViewModel = scheduleViewModelList.FirstOrDefault(s => s.StartDateTime >= startDateTimeUtc && s.EndDateTime <= endDateTimeUtc);

                    if (scheduleViewModel == null)
                    {
                        //return PartialView("_AddOrEditModal", viewModel);
                        return PartialView("~/Views/Home/_AddOrEditModal.cshtml", viewModel);
                    }
                    else
                    {
                        //return PartialView("_AlreadyExistsModal", viewModel);
                        return PartialView("~/Views/Home/_AlreadyExistsModal.cshtml");
                    }

                }
                else
                {
                    return ErrorPartialView(ExceptionHelper.ExceptionErrorMessageForNullObject());
                }
            }
            catch (Exception ex)
            {
                return ErrorPartialView(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAjax(int scheduleId)
        {
            try
            {
                if (scheduleId > 0)
                {
                    var viewModel = await _iScheduleManager.GetScheduleByIdAsync(scheduleId);

                    DateTime startDateTimeUtc = viewModel.StartDateTime;
                    viewModel.StartDateTime = startDateTimeUtc;
                    viewModel.StartDate = startDateTimeUtc.ToFormatedDateString();
                    viewModel.StartTime = startDateTimeUtc.ToFormatedShortTimeWithAmPmString();

                    DateTime endDateTimeUtc = viewModel.EndDateTime;
                    viewModel.EndDateTime = endDateTimeUtc;
                    viewModel.EndDate = endDateTimeUtc.ToFormatedDateString();
                    viewModel.EndTime = endDateTimeUtc.ToFormatedShortTimeWithAmPmString();

                    if (viewModel != null)
                    {
                        //return PartialView("_AddOrEditModal", viewModel);
                        return PartialView("~/Views/Home/_AddOrEditModal.cshtml", viewModel);
                    }
                    else
                    {
                        return ErrorPartialView(ExceptionHelper.ExceptionErrorMessageForNullObject());
                    }
                }
                else
                {
                    return ErrorPartialView(ExceptionHelper.ExceptionErrorMessageForNullObject());
                }
            }
            catch (Exception ex)
            {
                return ErrorPartialView(ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAjax(ScheduleViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DateTime? startDate = viewModel.StartDate.ToFormatedDateFromStartWithMonth();
                    DateTime startDateTime = DateTime.Parse(startDate.ToDbFormatedDateString() + " " + viewModel.StartTime, CultureInfo.InvariantCulture);
                    DateTime.SpecifyKind(startDateTime, DateTimeKind.Local);
                    DateTime startDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(startDateTime);
                    DateTime dtStartDateTime = DateTime.Parse(startDateTimeUtc.ToDbFormatedDateTimeString());
                    viewModel.StartDateTime = dtStartDateTime;

                    DateTime? endDate = viewModel.EndDate.ToFormatedDateFromStartWithMonth();
                    DateTime endDateTime = DateTime.Parse(endDate.ToDbFormatedDateString() + " " + viewModel.EndTime, CultureInfo.InvariantCulture);
                    DateTime.SpecifyKind(endDateTime, DateTimeKind.Local);
                    DateTime endDateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(endDateTime);
                    DateTime dtEndDateTime = DateTime.Parse(endDateTimeUtc.ToDbFormatedDateTimeString());
                    viewModel.EndDateTime = dtEndDateTime;

                    var scheduleViewModelList = await _iScheduleManager.GetSchedulesAsync();
                    var scheduleViewModel = scheduleViewModelList.FirstOrDefault(s => s.StartDateTime >= startDateTimeUtc && s.EndDateTime <= endDateTimeUtc);

                    if (scheduleViewModel == null)
                    {
                        //add
                        if (viewModel.ScheduleId == 0)
                        {
                            _result = await _iScheduleManager.InsertAsync(viewModel);
                        }
                        else if (viewModel.ScheduleId > 0) //edit
                        {
                            _result = await _iScheduleManager.UpdateAsync(viewModel);
                        }
                    }
                    else
                    {
                        _result = Result.Fail(MessageHelper.ScheduleAlreadyExists);
                    }
                    
                }
                else
                {
                    _result = Result.Fail(MessageHelper.SaveFail);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "SaveAjax[POST]"));
                _result = Result.Fail(MessageHelper.UnhandledError);
            }

            return JsonResult(_result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            try
            {
                if (id > 0)
                {
                    var viewModel = await _iScheduleManager.GetScheduleByIdAsync(id);
                    _result = await _iScheduleManager.DeleteAsync(viewModel);
                }
                else
                {
                    _result = Result.Fail(MessageHelper.DeleteFail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "DeleteAjax[POST]"));
                _result = Result.Fail(MessageHelper.UnhandledError);
            }

            return JsonResult(_result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchedulesAjax()
        {
            try
            {
                var scheduleViewModelList = await _iScheduleManager.GetSchedulesAsync();

                _result = Result.Ok(MessageHelper.Success);
                var jsonData = new { success = _result.Success, data = scheduleViewModelList };

                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "GetSchedulesAjax"));
                _result = Result.Fail(MessageHelper.DataNotFound);
            }
            var json = new { success = _result.Success, errortype = _result.ErrorType, error = _result.Error };
            return new JsonResult(json);
        }

        [HttpGet]
        public async Task<IActionResult> GetExistScheduleAjax(string startDate, string startTime, string endDate, string endTime)
        {
            try
            {
                var scheduleViewModelList = await _iScheduleManager.GetSchedulesAsync();

                DateTime? startDateFormated = startDate.ToFormatedDateFromStartWithMonth();
                DateTime startDateTime = DateTime.Parse(startDateFormated.ToFormatedDateString() + " " + startTime, CultureInfo.InvariantCulture);

                DateTime? endDateFormated = endDate.ToFormatedDateFromStartWithMonth();
                DateTime endDateTime = DateTime.Parse(endDateFormated.ToFormatedDateString() + " " + endTime, CultureInfo.InvariantCulture);

                var scheduleViewModel = scheduleViewModelList.FirstOrDefault(s => s.StartDateTime >= startDateTime && s.EndDateTime <= endDateTime);

                if (scheduleViewModel != null) {
                    _result = Result.Ok(MessageHelper.Success);
                }
                else {
                    _result = Result.Fail(MessageHelper.DataNotFound);
                }
                var jsonData = new { success = _result.Success, data = scheduleViewModel };

                return new JsonResult(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "GetExistScheduleAjax"));
                _result = Result.Fail(MessageHelper.DataNotFound);
            }
            var json = new { success = _result.Success, errortype = _result.ErrorType, error = _result.Error };
            return new JsonResult(json);
        }

        [HttpGet]
        public async Task<IActionResult> GetMembersAjax(string id)
        {
            try
            {
                var employeeViewModelList = await _iMemberManager.GetMembersAsync();

                if (string.IsNullOrEmpty(id))
                {
                    var selectList = SelectListItemExtension.PopulateDropdownList(employeeViewModelList, "MemberId", "FullName");
                    return new JsonResult(selectList);
                }
                else
                {
                    var selectList = SelectListItemExtension.PopulateDropdownList(employeeViewModelList, "MemberId", "FullName", isEdit: true, selectedValue: id);
                    return new JsonResult(selectList);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(LoggerMessageHelper.FormateMessageForException(ex, "GetSchedulesAjax"));
                _result = Result.Fail(MessageHelper.DataNotFound);
            }
            var json = new { success = _result.Success, errortype = _result.ErrorType, error = _result.Error };
            return new JsonResult(json);
        }

    }

}
