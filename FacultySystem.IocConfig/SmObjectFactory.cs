using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.ServiceLayer;
using ContentManagementSystem.ServiceLayer.Contracts;
using StructureMap;
using StructureMap.Web;
using Postal;

namespace ContentManagementSystem.IocConfig
{
    public static class SmObjectFactory
    {
        private static readonly Lazy<Container> _containerBuilder =
            new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return _containerBuilder.Value; }
        }

        private static Container defaultContainer()
        {
            return new Container(ioc =>
            {
                //ioc.For<Microsoft.AspNet.SignalR.IDependencyResolver>().Singleton().Add<StructureMapSignalRDependencyResolver>();

                //ioc.For<IIdentity>().Use(() => getIdentity());

                ioc.For<IUnitOfWork>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use<ApplicationDbContext>();
                // Remove these 2 lines if you want to use a connection string named connectionString1, defined in the web.config file.
                //.Ctor<string>("connectionString")
                //.Is("Data Source=(local);Initial Catalog=TestDbIdentity;Integrated Security = true");

                ioc.For<ApplicationDbContext>().HybridHttpOrThreadLocalScoped()
                   .Use(context => (ApplicationDbContext)context.GetInstance<IUnitOfWork>());
                ioc.For<DbContext>().HybridHttpOrThreadLocalScoped()
                   .Use(context => (ApplicationDbContext)context.GetInstance<IUnitOfWork>());

                //ioc.For<IUserStore<ApplicationUser, int>>()
                //    .HybridHttpOrThreadLocalScoped()
                //    .Use<CustomUserStore>();

                //ioc.For<IRoleStore<CustomRole, int>>()
                //    .HybridHttpOrThreadLocalScoped()
                //    .Use<RoleStore<CustomRole, int, CustomUserRole>>();

                //ioc.For<IAuthenticationManager>()
                //      .Use(() => HttpContext.Current.GetOwinContext().Authentication);

                //ioc.For<IApplicationSignInManager>()
                //      .HybridHttpOrThreadLocalScoped()
                //      .Use<ApplicationSignInManager>();

                //ioc.For<IApplicationRoleManager>()
                //      .HybridHttpOrThreadLocalScoped()
                //      .Use<ApplicationRoleManager>();

                // map same interface to different concrete classes
                //ioc.For<IIdentityMessageService>().Use<SmsService>();
                //ioc.For<IIdentityMessageService>().Use<EmailService>();

                //ioc.For<IApplicationUserManager>().HybridHttpOrThreadLocalScoped()
                //   .Use<ApplicationUserManager>()
                //   .Ctor<IIdentityMessageService>("smsService").Is<SmsService>()
                //   .Ctor<IIdentityMessageService>("emailService").Is<EmailService>()
                //   .Setter(userManager => userManager.SmsService).Is<SmsService>()
                //   .Setter(userManager => userManager.EmailService).Is<EmailService>();

                //ioc.For<ApplicationUserManager>().HybridHttpOrThreadLocalScoped()
                //   .Use(context => (ApplicationUserManager)context.GetInstance<IApplicationUserManager>());

                //ioc.For<ICustomRoleStore>()
                //      .HybridHttpOrThreadLocalScoped()
                //      .Use<CustomRoleStore>();

                //ioc.For<ICustomUserStore>()
                //      .HybridHttpOrThreadLocalScoped()
                //      .Use<CustomUserStore>();

                //config.For<IDataProtectionProvider>().Use(()=> app.GetDataProtectionProvider()); // In Startup class

                ioc.For<IUserService>().Use<EfUserService>();
                ioc.For<IEmailService>().Use<EmailService>();
                ioc.For<IEmailViewRenderer>().Use<EmailViewRenderer>();
                ioc.For<IEmailParser>().Use<EmailParser>();
                ioc.For<IProfessorService>().Use<EFProfessorService>();
                ioc.For<IAddressService>().Use<EFAddressService>();
                ioc.For<IRoleService>().Use<EFRoleService>();
                ioc.For<IEducationalDegreeService>().Use<EFEducationalDegreeService>();
                ioc.For<ICollegeService>().Use<EFCollegeService>();
                ioc.For<IEducationalGroupService>().Use<EFEducationalGroupService>();
                ioc.For<IAcademicRankService>().Use<EFAcademicRankService>();
                ioc.For<IDefaultFreeFieldService>().Use<EFDefaultFreeFieldService>();
                ioc.For<IFreeFieldService>().Use<EFFreeFieldService>();
                ioc.For<ISectionOrderService>().Use<EFSectionOrderService>();
                ioc.For<IAdministrationRecordService>().Use<EFAdministrationService>();
                ioc.For<IResearchRecordService>().Use<EFResearchService>();
                ioc.For<ITrainingRecordService>().Use<EFTrainingService>();
                ioc.For<IStudingRecordService>().Use<EFStudingService>();
                ioc.For<IThesisService>().Use<EFThesisService>();
                ioc.For<IHonorService>().Use<EFHonorService>();
                ioc.For<IWorkshopService>().Use<EFWorkshopService>();
                ioc.For<IPublicationService>().Use<EFPublicationService>();
                ioc.For<ILanguageService>().Use<EFLanguageService>();
                ioc.For<ILessonService>().Use<EFLessonService>();
                ioc.For<ILessonClassInfoService>().Use<EFLessonClassInfoService>();
                ioc.For<IPracticeClassInfoService>().Use<EFPracticeClassInfoService>();
                ioc.For<ILessonImportantDateService>().Use<EFLessonImportantDateService>();
                ioc.For<ILessonNewsService>().Use<EFLessonNewsService>();
                ioc.For<ILessonFileService>().Use<EFLessonFileService>();
                ioc.For<ILessonPracticeService>().Use<EFLessonPracticeService>();
                ioc.For<ILessonScoreService>().Use<EFLessonScoreService>();
                ioc.For<IActivityLogService>().Use<EFActivityLogService>();
                ioc.For<IMembershipService>().Use<EFMembershipService>();
                ioc.For<IGalleryService>().Use<EFGalleryService>();
                ioc.For<IGalleryItemService>().Use<EFGalleryItemService>();
                ioc.For<IAdminMessageService>().Use<EFAdminMessageService>();
                ioc.For<IDocumentCitationService>().Use<EFDocumentCitationService>();
                ioc.For<IWeeklyProgramService>().Use<EFWeeklyProgramService>();
                ioc.For<IExternalResearchService>().Use<EFExternalResearchService>();
                ioc.For<IExternalSeminarService>().Use<EFExternalSeminarService>();
                ioc.For<IInternalResearchService>().Use<EFInternalResearchService>();
                ioc.For<IInternalSeminarService>().Use<EFInternalSeminarService>();
                ioc.For<INewsService>().Use<EFNewsService>();

                //ioc.For<IProductService>().Use<EfProductService>();
            });
        }

        //private static IIdentity getIdentity()
        //{
        //    if (HttpContext.Current != null && HttpContext.Current.User != null)
        //    {
        //        return HttpContext.Current.User.Identity;
        //    }

        //    return ClaimsPrincipal.Current != null ? ClaimsPrincipal.Current.Identity : null;
        //}
    }
}