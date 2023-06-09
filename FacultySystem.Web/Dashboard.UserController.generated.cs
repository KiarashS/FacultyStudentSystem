// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace ContentManagementSystem.Web.Areas.Dashboard.Controllers
{
    public partial class UserController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected UserController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult List()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.List);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> Create()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Update()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Update);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Delete()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Delete);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult CheckEmail()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CheckEmail);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult CheckPageId()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CheckPageId);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public UserController Actions { get { return MVC.Dashboard.User; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Dashboard";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "User";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "User";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string List = "List";
            public readonly string Create = "Create";
            public readonly string Update = "Update";
            public readonly string Delete = "Delete";
            public readonly string CheckEmail = "CheckEmail";
            public readonly string CheckPageId = "CheckPageId";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string List = "List";
            public const string Create = "Create";
            public const string Update = "Update";
            public const string Delete = "Delete";
            public const string CheckEmail = "CheckEmail";
            public const string CheckPageId = "CheckPageId";
        }


        static readonly ActionParamsClass_List s_params_List = new ActionParamsClass_List();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_List ListParams { get { return s_params_List; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_List
        {
            public readonly string filterFirstname = "filterFirstname";
            public readonly string filterLastname = "filterLastname";
            public readonly string filterEmail = "filterEmail";
            public readonly string filterPageId = "filterPageId";
            public readonly string filterRole = "filterRole";
            public readonly string filterIsBanned = "filterIsBanned";
            public readonly string filterIsSoftDelete = "filterIsSoftDelete";
            public readonly string jtStartIndex = "jtStartIndex";
            public readonly string jtPageSize = "jtPageSize";
        }
        static readonly ActionParamsClass_Create s_params_Create = new ActionParamsClass_Create();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Create CreateParams { get { return s_params_Create; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Create
        {
            public readonly string user = "user";
            public readonly string userRoles = "userRoles";
        }
        static readonly ActionParamsClass_Update s_params_Update = new ActionParamsClass_Update();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Update UpdateParams { get { return s_params_Update; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Update
        {
            public readonly string user = "user";
            public readonly string userRoles = "userRoles";
        }
        static readonly ActionParamsClass_Delete s_params_Delete = new ActionParamsClass_Delete();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Delete DeleteParams { get { return s_params_Delete; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Delete
        {
            public readonly string userId = "userId";
        }
        static readonly ActionParamsClass_CheckEmail s_params_CheckEmail = new ActionParamsClass_CheckEmail();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CheckEmail CheckEmailParams { get { return s_params_CheckEmail; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CheckEmail
        {
            public readonly string userId = "userId";
            public readonly string email = "email";
            public readonly string oldEmail = "oldEmail";
        }
        static readonly ActionParamsClass_CheckPageId s_params_CheckPageId = new ActionParamsClass_CheckPageId();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CheckPageId CheckPageIdParams { get { return s_params_CheckPageId; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CheckPageId
        {
            public readonly string userId = "userId";
            public readonly string pageId = "pageId";
            public readonly string oldPageId = "oldPageId";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
            }
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_UserController : ContentManagementSystem.Web.Areas.Dashboard.Controllers.UserController
    {
        public T4MVC_UserController() : base(Dummy.Instance) { }

        [NonAction]
        partial void ListOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string filterFirstname, string filterLastname, string filterEmail, string filterPageId, string filterRole, bool filterIsBanned, bool filterIsSoftDelete, int jtStartIndex, int jtPageSize);

        [NonAction]
        public override System.Web.Mvc.ActionResult List(string filterFirstname, string filterLastname, string filterEmail, string filterPageId, string filterRole, bool filterIsBanned, bool filterIsSoftDelete, int jtStartIndex, int jtPageSize)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.List);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "filterFirstname", filterFirstname);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "filterLastname", filterLastname);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "filterEmail", filterEmail);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "filterPageId", filterPageId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "filterRole", filterRole);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "filterIsBanned", filterIsBanned);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "filterIsSoftDelete", filterIsSoftDelete);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "jtStartIndex", jtStartIndex);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "jtPageSize", jtPageSize);
            ListOverride(callInfo, filterFirstname, filterLastname, filterEmail, filterPageId, filterRole, filterIsBanned, filterIsSoftDelete, jtStartIndex, jtPageSize);
            return callInfo;
        }

        [NonAction]
        partial void CreateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, ContentManagementSystem.Models.ViewModels.UserListViewModel user, string[] userRoles);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> Create(ContentManagementSystem.Models.ViewModels.UserListViewModel user, string[] userRoles)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Create);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "user", user);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "userRoles", userRoles);
            CreateOverride(callInfo, user, userRoles);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

        [NonAction]
        partial void UpdateOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, ContentManagementSystem.Models.ViewModels.UserListViewModel user, string[] userRoles);

        [NonAction]
        public override System.Web.Mvc.ActionResult Update(ContentManagementSystem.Models.ViewModels.UserListViewModel user, string[] userRoles)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Update);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "user", user);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "userRoles", userRoles);
            UpdateOverride(callInfo, user, userRoles);
            return callInfo;
        }

        [NonAction]
        partial void DeleteOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int userId);

        [NonAction]
        public override System.Web.Mvc.ActionResult Delete(int userId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Delete);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "userId", userId);
            DeleteOverride(callInfo, userId);
            return callInfo;
        }

        [NonAction]
        partial void CheckEmailOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int userId, string email, string oldEmail);

        [NonAction]
        public override System.Web.Mvc.ActionResult CheckEmail(int userId, string email, string oldEmail)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CheckEmail);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "userId", userId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "email", email);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "oldEmail", oldEmail);
            CheckEmailOverride(callInfo, userId, email, oldEmail);
            return callInfo;
        }

        [NonAction]
        partial void CheckPageIdOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int userId, string pageId, string oldPageId);

        [NonAction]
        public override System.Web.Mvc.ActionResult CheckPageId(int userId, string pageId, string oldPageId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CheckPageId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "userId", userId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "pageId", pageId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "oldPageId", oldPageId);
            CheckPageIdOverride(callInfo, userId, pageId, oldPageId);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
