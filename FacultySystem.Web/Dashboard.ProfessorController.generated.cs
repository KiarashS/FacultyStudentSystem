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
    public partial class ProfessorController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected ProfessorController(Dummy d) { }

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


        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ProfessorController Actions { get { return MVC.Dashboard.Professor; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Dashboard";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Professor";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Professor";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Resume = "Resume";
            public readonly string Membership = "Membership";
            public readonly string Gallery = "Gallery";
            public readonly string AdminMessage = "AdminMessage";
            public readonly string WeeklyProgram = "WeeklyProgram";
            public readonly string Settings = "Settings";
            public readonly string ExternalArticles = "ExternalArticles";
            public readonly string ExternalSeminars = "ExternalSeminars";
            public readonly string InternalArticles = "InternalArticles";
            public readonly string InternalSeminars = "InternalSeminars";
            public readonly string UpdateExternalArticles = "UpdateExternalArticles";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Resume = "Resume";
            public const string Membership = "Membership";
            public const string Gallery = "Gallery";
            public const string AdminMessage = "AdminMessage";
            public const string WeeklyProgram = "WeeklyProgram";
            public const string Settings = "Settings";
            public const string ExternalArticles = "ExternalArticles";
            public const string ExternalSeminars = "ExternalSeminars";
            public const string InternalArticles = "InternalArticles";
            public const string InternalSeminars = "InternalSeminars";
            public const string UpdateExternalArticles = "UpdateExternalArticles";
        }


        static readonly ActionParamsClass_Resume s_params_Resume = new ActionParamsClass_Resume();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Resume ResumeParams { get { return s_params_Resume; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Resume
        {
            public readonly string resume = "resume";
            public readonly string deleteResume = "deleteResume";
        }
        static readonly ActionParamsClass_WeeklyProgram s_params_WeeklyProgram = new ActionParamsClass_WeeklyProgram();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_WeeklyProgram WeeklyProgramParams { get { return s_params_WeeklyProgram; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_WeeklyProgram
        {
            public readonly string details = "details";
        }
        static readonly ActionParamsClass_Settings s_params_Settings = new ActionParamsClass_Settings();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Settings SettingsParams { get { return s_params_Settings; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Settings
        {
            public readonly string settings = "settings";
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
                public readonly string AdminMessage = "AdminMessage";
                public readonly string ExternalArticles = "ExternalArticles";
                public readonly string ExternalSeminars = "ExternalSeminars";
                public readonly string Gallery = "Gallery";
                public readonly string InternalArticles = "InternalArticles";
                public readonly string InternalSeminars = "InternalSeminars";
                public readonly string Membership = "Membership";
                public readonly string Resume = "Resume";
                public readonly string Settings = "Settings";
                public readonly string WeeklyProgram = "WeeklyProgram";
            }
            public readonly string AdminMessage = "~/Areas/Dashboard/Views/Professor/AdminMessage.cshtml";
            public readonly string ExternalArticles = "~/Areas/Dashboard/Views/Professor/ExternalArticles.cshtml";
            public readonly string ExternalSeminars = "~/Areas/Dashboard/Views/Professor/ExternalSeminars.cshtml";
            public readonly string Gallery = "~/Areas/Dashboard/Views/Professor/Gallery.cshtml";
            public readonly string InternalArticles = "~/Areas/Dashboard/Views/Professor/InternalArticles.cshtml";
            public readonly string InternalSeminars = "~/Areas/Dashboard/Views/Professor/InternalSeminars.cshtml";
            public readonly string Membership = "~/Areas/Dashboard/Views/Professor/Membership.cshtml";
            public readonly string Resume = "~/Areas/Dashboard/Views/Professor/Resume.cshtml";
            public readonly string Settings = "~/Areas/Dashboard/Views/Professor/Settings.cshtml";
            public readonly string WeeklyProgram = "~/Areas/Dashboard/Views/Professor/WeeklyProgram.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_ProfessorController : ContentManagementSystem.Web.Areas.Dashboard.Controllers.ProfessorController
    {
        public T4MVC_ProfessorController() : base(Dummy.Instance) { }

        [NonAction]
        partial void ResumeOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Resume()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Resume);
            ResumeOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ResumeOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Web.HttpPostedFileBase resume, bool deleteResume);

        [NonAction]
        public override System.Web.Mvc.ActionResult Resume(System.Web.HttpPostedFileBase resume, bool deleteResume)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Resume);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "resume", resume);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "deleteResume", deleteResume);
            ResumeOverride(callInfo, resume, deleteResume);
            return callInfo;
        }

        [NonAction]
        partial void MembershipOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Membership()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Membership);
            MembershipOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void GalleryOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Gallery()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Gallery);
            GalleryOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void AdminMessageOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult AdminMessage()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AdminMessage);
            AdminMessageOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void WeeklyProgramOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult WeeklyProgram()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.WeeklyProgram);
            WeeklyProgramOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void WeeklyProgramOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, ContentManagementSystem.Models.ViewModels.WeeklyProgramDetailsViewModel details);

        [NonAction]
        public override System.Web.Mvc.ActionResult WeeklyProgram(ContentManagementSystem.Models.ViewModels.WeeklyProgramDetailsViewModel details)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.WeeklyProgram);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "details", details);
            WeeklyProgramOverride(callInfo, details);
            return callInfo;
        }

        [NonAction]
        partial void SettingsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Settings()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Settings);
            SettingsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ExternalArticlesOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult ExternalArticles()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ExternalArticles);
            ExternalArticlesOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ExternalSeminarsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult ExternalSeminars()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ExternalSeminars);
            ExternalSeminarsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void InternalArticlesOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult InternalArticles()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.InternalArticles);
            InternalArticlesOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void InternalSeminarsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult InternalSeminars()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.InternalSeminars);
            InternalSeminarsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void SettingsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, ContentManagementSystem.Models.ViewModels.UserSettingsViewModel settings);

        [NonAction]
        public override System.Web.Mvc.ActionResult Settings(ContentManagementSystem.Models.ViewModels.UserSettingsViewModel settings)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Settings);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "settings", settings);
            SettingsOverride(callInfo, settings);
            return callInfo;
        }

        [NonAction]
        partial void UpdateExternalArticlesOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> UpdateExternalArticles()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UpdateExternalArticles);
            UpdateExternalArticlesOverride(callInfo);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114