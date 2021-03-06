﻿using Infrastructure;
using OpenAuth.App;
using OpenAuth.App.SSO;
using OpenAuth.Mvc.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OpenAuth.App.Response;
using OpenAuth.Repository.Domain;

namespace OpenAuth.Mvc.Controllers
{
    public class ModuleManagerController : BaseController
    {
        private ModuleManagerApp _app;
        public ModuleManagerController(AuthUtil authUtil, ModuleManagerApp app) : base(authUtil)
        {
            _app = app;
        }

        // GET: /ModuleManager/
        [Authenticate]
        public ActionResult Index()
        {
            return View();
        }

        [Authenticate]
        public ActionResult Assign()
        {
            return View();
        }

        /// <summary>
        /// 加载特定用户的模块
        /// </summary>
        /// <param name="firstId">The user identifier.</param>
        /// <returns>System.String.</returns>
        public string LoadForUser(string firstId)
        {
            var modules = _app.LoadForUser(firstId);
            return JsonHelper.Instance.Serialize(modules);
        }
        /// <summary>
        /// 根据某用户ID获取可访问某模块的菜单项
        /// </summary>
        /// <returns></returns>
        public string LoadMenusForUser(string moduleId, string firstId)
        {
            var menus = _app.LoadMenusForUser(moduleId, firstId);
            return JsonHelper.Instance.Serialize(menus);
        }

        /// <summary>
        /// 加载角色模块
        /// </summary>
        /// <param name="firstId">The role identifier.</param>
        /// <returns>System.String.</returns>
        public string LoadForRole(string firstId)
        {
            var modules = _app.LoadForRole(firstId);
            return JsonHelper.Instance.Serialize(modules);
        }

        /// <summary>
        /// 根据某角色ID获取可访问某模块的菜单项
        /// </summary>
        /// <returns></returns>
        public string LoadMenusForRole(string moduleId, string firstId)
        {
            var menus = _app.LoadMenusForRole(moduleId, firstId);
            return JsonHelper.Instance.Serialize(menus);
        }

        /// <summary>
        /// 获取发起页面的菜单权限
        /// </summary>
        /// <returns>System.String.</returns>
        public string LoadAuthorizedMenus(string modulecode)
        {
            var user = _authUtil.GetCurrentUser();
            var module = user.Modules.First(u =>u.Code == modulecode);
            if (module != null)
            {
                return JsonHelper.Instance.Serialize(module.Elements);

            }
            return "";
        }


        #region 添加编辑模块

        //添加模块
        [HttpPost]
       
        public string Add(Module model)
        {
            try
            {
                _app.Add(model);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        //修改模块
        [HttpPost]
       
        public string Update(Module model)
        {
            try
            {
                _app.Update(model);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        [HttpPost]
        public string Delete(string[] ids)
        {
            try
            {
                _app.Delete(ids);
            }
            catch (Exception e)
            {
                Result.Code = 500;
                Result.Message = e.Message;
            }

            return JsonHelper.Instance.Serialize(Result);
        }

        #endregion 添加编辑模块

        /// <summary>
        /// 加载当前用户可访问模块的菜单
        /// </summary>
        /// <param name="moduleId">The module identifier.</param>
        /// <returns>System.String.</returns>
        public string LoadMenus(string moduleId)
        {
            var user = _authUtil.GetCurrentUser();

            var module = user.Modules.Single(u => u.Id == moduleId);
             
            var data = new TableData
            {
                data = module.Elements,
                count = module.Elements.Count(),
            };
            return JsonHelper.Instance.Serialize(data);
        }

        //添加菜单
        [HttpPost]
       
        public string AddMenu(ModuleElement model)
        {
            try
            {
                _app.AddMenu(model);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        //添加菜单
        [HttpPost]
       
        public string UpdateMenu(ModuleElement model)
        {
            try
            {
                _app.UpdateMenu(model);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }


        /// <summary>
        /// 删除菜单
        /// </summary>
        [HttpPost]
        public string DelMenu(params string[] ids)
        {
            try
            {
                _app.DelMenu(ids);
            }
            catch (Exception e)
            {
                Result.Code = 500;
                Result.Message = e.Message;
            }

            return JsonHelper.Instance.Serialize(Result);
        }

        
    }
}