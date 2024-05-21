using FunlabProgramChallenge.Core;
using FunlabProgramChallenge.Helpers;
using FunlabProgramChallenge.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunlabProgramChallenge.Managers
{
    public class MemberManager : IMemberManager
    {
        private readonly IConfiguration _iConfiguration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _iHostingEnvironment;

        public MemberManager(IConfiguration iConfiguration, Microsoft.AspNetCore.Hosting.IHostingEnvironment iHostingEnvironment)
        {
            _iConfiguration = iConfiguration;
            _iHostingEnvironment = iHostingEnvironment;
        }

        public async Task<List<MemberViewModel>> GetMembersAsync()
        {
            try
            {
                JsonDataStoreHelper jsonDataStoreHelper = new JsonDataStoreHelper(_iHostingEnvironment, "employee.json");
                var employeeJson = await jsonDataStoreHelper.ReadJsonData();
                var employeeViewModelList = JsonConvert.DeserializeObject<List<MemberViewModel>>(employeeJson);
                return employeeViewModelList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MemberViewModel> GetMemberByIdAsync(int employeeId)
        {
            try
            {
                JsonDataStoreHelper jsonDataStoreHelper = new JsonDataStoreHelper(_iHostingEnvironment, "employee.json");
                var employeeJson = await jsonDataStoreHelper.ReadJsonData();
                var employeeViewModelList = JsonConvert.DeserializeObject<List<MemberViewModel>>(employeeJson);
                var employeeViewModel = employeeViewModelList.FirstOrDefault(x => x.MemberId == employeeId);
                return employeeViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Result> InsertAsync(MemberViewModel viewModel)
        {
            try
            {
                if (viewModel.MemberId == 0)
                {
                    if (viewModel != null)
                    {
                        JsonDataStoreHelper jsonDataStoreHelper = new JsonDataStoreHelper(_iHostingEnvironment, "employee.json");
                        var viewModelList = await GetMembersAsync();
                        viewModelList.Add(viewModel);
                        string viewModelListJson = JsonConvert.SerializeObject(viewModelList);
                        var isWrite = await jsonDataStoreHelper.WriteJsonData(viewModelListJson);
                        return Result.Ok(MessageHelper.Save);
                    }
                    else
                    {
                        return Result.Fail(MessageHelper.SaveFail);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Result.Fail(MessageHelper.SaveFail);
        }

        public async Task<Result> UpdateAsync(MemberViewModel viewModel)
        {
            try
            {
                if (viewModel.MemberId > 0)
                {
                    if (viewModel != null)
                    {
                        JsonDataStoreHelper jsonDataStoreHelper = new JsonDataStoreHelper(_iHostingEnvironment, "employee.json");
                        var viewModelList = await GetMembersAsync();
                        var viemModelRemove = viewModelList.FirstOrDefault(x => x.MemberId == viewModel.MemberId);
                        viewModelList.Remove(viemModelRemove);
                        viewModelList.Add(viewModel);
                        string viewModelListJson = JsonConvert.SerializeObject(viewModelList);
                        var isWrite = await jsonDataStoreHelper.WriteJsonData(viewModelListJson);
                        return Result.Ok(MessageHelper.Update);
                    }
                    else
                    {
                        return Result.Fail(MessageHelper.UpdateFail);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Result.Fail(MessageHelper.UpdateFail);
        }

        public async Task<Result> DeleteAsync(MemberViewModel viewModel)
        {
            try
            {
                if (viewModel.MemberId > 0)
                {
                    if (viewModel != null)
                    {
                        JsonDataStoreHelper jsonDataStoreHelper = new JsonDataStoreHelper(_iHostingEnvironment, "employee.json");
                        var viewModelList = await GetMembersAsync();
                        var viemModelRemove = viewModelList.FirstOrDefault(x => x.MemberId == viewModel.MemberId);
                        viewModelList.Remove(viemModelRemove);
                        string viewModelListJson = JsonConvert.SerializeObject(viewModelList);
                        var isWrite = await jsonDataStoreHelper.WriteJsonData(viewModelListJson);
                        return Result.Ok(MessageHelper.Delete);
                    }
                    else
                    {
                        return Result.Fail(MessageHelper.DeleteFail);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Result.Fail(MessageHelper.DeleteFail);
        }
    }

    public interface IMemberManager
    {
        Task<List<MemberViewModel>> GetMembersAsync();
        Task<MemberViewModel> GetMemberByIdAsync(int employeeId);
        Task<Result> InsertAsync(MemberViewModel viewModel);
        Task<Result> UpdateAsync(MemberViewModel viewModel);
        Task<Result> DeleteAsync(MemberViewModel viewModel);
    }
}
