namespace ContentManagementSystem.DataLayer.Context
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WeeklyPrograms",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        StartTime = c.String(nullable: false),
                        EndTime = c.String(nullable: false),
                        Description = c.String(),
                        DayOfProgram = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.Professors",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        PageId = c.String(nullable: false, maxLength: 450, unicode: false),
                        CommonAuthorPaperName = c.String(),
                        SecondaryEmails = c.String(),
                        Sex = c.Byte(nullable: false),
                        MaritalStatus = c.Byte(nullable: false),
                        AcademicRankId = c.Int(nullable: false),
                        EducationalDegreeId = c.Int(nullable: false),
                        EducationalGroupId = c.Int(nullable: false),
                        CollegeId = c.Int(nullable: false),
                        Mobile = c.String(),
                        Location = c.String(),
                        ResearchFields = c.String(),
                        Interests = c.String(),
                        PersonalWebPage = c.String(),
                        PersianResumeFileName = c.String(),
                        EnglishResumeFileName = c.String(),
                        ScopusId = c.String(),
                        OrcidId = c.String(),
                        ResearchGateId = c.String(),
                        GoogleScholarId = c.String(),
                        ResearcherId = c.String(),
                        PubMedId = c.String(),
                        MedLibId = c.String(),
                        Bio = c.String(),
                        BirthPlace = c.String(),
                        IsBanned = c.Boolean(nullable: false),
                        LastUpdateTime = c.DateTime(),
                        IsApproved = c.Boolean(nullable: false),
                        IsSoftDelete = c.Boolean(nullable: false),
                        IsActiveBio = c.Boolean(nullable: false),
                        BannedDate = c.DateTime(),
                        BannedReason = c.String(),
                        Avatar = c.String(),
                        BirthDate = c.DateTime(),
                        ScopusHIndex = c.Int(),
                        GoogleHIndex = c.Int(),
                        ScopusCitations = c.Int(),
                        ScopusDocuments = c.Int(),
                        ScopusTotalDocumentsCited = c.Int(),
                        GoogleCitations = c.Int(),
                        OtherNamesFormat = c.String(),
                        FreePage = c.String(),
                        IsActiveFreePage = c.Boolean(nullable: false),
                        ShowScopusDocumentsCitationChart = c.Boolean(nullable: false),
                        ShowGoogleDocumentsCitationChart = c.Boolean(nullable: false),
                        ShowHIndexSection = c.Boolean(nullable: false),
                        FreePageLastUpdateTime = c.DateTime(),
                        WeeklyProgramStartDate = c.String(),
                        WeeklyProgramEndDate = c.String(),
                        WeeklyProgramDescription = c.String(),
                        IsActiveWeeklyProgram = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AcademicRanks", t => t.AcademicRankId, cascadeDelete: true)
                .ForeignKey("dbo.Colleges", t => t.CollegeId, cascadeDelete: true)
                .ForeignKey("dbo.EducationalDegrees", t => t.EducationalDegreeId, cascadeDelete: true)
                .ForeignKey("dbo.EducationalGroups", t => t.EducationalGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.AcademicRankId)
                .Index(t => t.EducationalDegreeId)
                .Index(t => t.EducationalGroupId)
                .Index(t => t.CollegeId);
            
            CreateTable(
                "dbo.AcademicRanks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        PostalAddress = c.String(),
                        PostalCode = c.String(),
                        Tell = c.String(),
                        Fax = c.String(),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.AdministrationRecords",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Post = c.String(),
                        Place = c.String(),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        Description = c.String(),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.AdminMessages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        ReplyContent = c.String(),
                        State = c.Byte(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.Colleges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CourseAndWorkshops",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Title = c.String(),
                        Host = c.String(),
                        Place = c.String(),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        Description = c.String(),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.DocumentCitations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Citation = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Document = c.Int(),
                        ProfessorId = c.Int(nullable: false),
                        Source = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.EducationalDegrees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EducationalGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExternalResearchRecords",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Doi = c.String(),
                        Title = c.String(),
                        Authors = c.String(),
                        Journal = c.String(),
                        Volume = c.String(),
                        Issue = c.String(),
                        Pages = c.String(),
                        Year = c.Int(),
                        TimesCited = c.Int(),
                        Link = c.String(),
                        Filename = c.String(),
                        Abstract = c.String(),
                        Description = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.ExternalSeminarRecords",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Doi = c.String(),
                        Title = c.String(),
                        Authors = c.String(),
                        Conference = c.String(),
                        Date = c.String(),
                        Link = c.String(),
                        Filename = c.String(),
                        Description = c.String(),
                        Abstract = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.FreeFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 300),
                        Value = c.String(maxLength: 300),
                        Order = c.Int(),
                        ProfessorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.Galleries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.GalleryItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        MediaFilename = c.String(),
                        MediaType = c.Byte(nullable: false),
                        ProfessorId = c.Int(nullable: false),
                        GalleryId = c.Long(nullable: false),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Galleries", t => t.GalleryId, cascadeDelete: true)
                .ForeignKey("dbo.Professors", t => t.ProfessorId)
                .Index(t => t.ProfessorId)
                .Index(t => t.GalleryId);
            
            CreateTable(
                "dbo.Honors",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Title = c.String(),
                        Time = c.Int(),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.LessonImportantDates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Date = c.String(),
                        Time = c.String(),
                        DateDay = c.Byte(nullable: false),
                        ProfessorId = c.Int(nullable: false),
                        LessonId = c.Long(nullable: false),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.LessonId, cascadeDelete: true)
                .ForeignKey("dbo.Professors", t => t.ProfessorId)
                .Index(t => t.ProfessorId)
                .Index(t => t.LessonId);
            
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        LessonCode = c.String(),
                        GroupNumber = c.Int(),
                        LessonName = c.String(),
                        Description = c.String(),
                        ScoringDescription = c.String(),
                        ProjectDescription = c.String(),
                        LessonGrade = c.Byte(nullable: false),
                        Field = c.String(),
                        Trend = c.String(),
                        AcademicYear = c.Int(),
                        Semester = c.String(),
                        LessonType = c.Byte(nullable: false),
                        LessonState = c.Byte(nullable: false),
                        UnitState = c.Byte(nullable: false),
                        UnitNumber = c.Single(),
                        CreateDate = c.DateTime(nullable: false),
                        Link = c.String(),
                        Order = c.Int(),
                        Reference = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.LessonClassInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Place = c.String(),
                        Description = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        StartHour = c.String(),
                        EndHour = c.String(),
                        ClassDay = c.Byte(nullable: false),
                        ProfessorId = c.Int(nullable: false),
                        LessonId = c.Long(nullable: false),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.LessonId, cascadeDelete: true)
                .ForeignKey("dbo.Professors", t => t.ProfessorId)
                .Index(t => t.ProfessorId)
                .Index(t => t.LessonId);
            
            CreateTable(
                "dbo.LessonFiles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        CoverFilename = c.String(),
                        Filename = c.String(),
                        FileLink = c.String(),
                        FileType = c.Byte(nullable: false),
                        ProfessorId = c.Int(nullable: false),
                        LessonId = c.Long(nullable: false),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.LessonId, cascadeDelete: true)
                .ForeignKey("dbo.Professors", t => t.ProfessorId)
                .Index(t => t.ProfessorId)
                .Index(t => t.LessonId);
            
            CreateTable(
                "dbo.LessonNews",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        NewsColor = c.Byte(nullable: false),
                        ProfessorId = c.Int(nullable: false),
                        LessonId = c.Long(nullable: false),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.LessonId, cascadeDelete: true)
                .ForeignKey("dbo.Professors", t => t.ProfessorId)
                .Index(t => t.ProfessorId)
                .Index(t => t.LessonId);
            
            CreateTable(
                "dbo.LessonPractices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Filename = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        DeliverDate = c.String(),
                        FileLink = c.String(),
                        ProfessorId = c.Int(nullable: false),
                        LessonId = c.Long(nullable: false),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.LessonId, cascadeDelete: true)
                .ForeignKey("dbo.Professors", t => t.ProfessorId)
                .Index(t => t.ProfessorId)
                .Index(t => t.LessonId);
            
            CreateTable(
                "dbo.LessonScores",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Filename = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        FileLink = c.String(),
                        ProfessorId = c.Int(nullable: false),
                        LessonId = c.Long(nullable: false),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.LessonId, cascadeDelete: true)
                .ForeignKey("dbo.Professors", t => t.ProfessorId)
                .Index(t => t.ProfessorId)
                .Index(t => t.LessonId);
            
            CreateTable(
                "dbo.PracticeClassInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Place = c.String(),
                        Description = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        StartHour = c.String(),
                        EndHour = c.String(),
                        TeacherName = c.String(),
                        PracticeClassDay = c.Byte(nullable: false),
                        ProfessorId = c.Int(nullable: false),
                        LessonId = c.Long(nullable: false),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.LessonId, cascadeDelete: true)
                .ForeignKey("dbo.Professors", t => t.ProfessorId)
                .Index(t => t.ProfessorId)
                .Index(t => t.LessonId);
            
            CreateTable(
                "dbo.InternalResearchRecords",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Title = c.String(),
                        Authors = c.String(),
                        Journal = c.String(),
                        Year = c.Int(),
                        Link = c.String(),
                        Filename = c.String(),
                        Abstract = c.String(),
                        Description = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.InternalSeminarRecords",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Title = c.String(),
                        Authors = c.String(),
                        Conference = c.String(),
                        Date = c.String(),
                        Link = c.String(),
                        Filename = c.String(),
                        Description = c.String(),
                        Abstract = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Name = c.Byte(nullable: false),
                        Level = c.Byte(nullable: false),
                        Description = c.String(),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.ProfessorMemberships",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        CommitteeTitle = c.String(),
                        Post = c.String(),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        Description = c.String(),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.Publications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Title = c.String(),
                        Publisher = c.String(),
                        Time = c.Int(),
                        Description = c.String(),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.ResearchRecords",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Title = c.String(),
                        Place = c.String(),
                        StartTime = c.String(),
                        EndTime = c.String(),
                        Description = c.String(),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.SectionOrders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        SectionName = c.Byte(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => new { t.ProfessorId, t.SectionName }, unique: true, name: "IX_ProfessorIdAndSectionName");
            
            CreateTable(
                "dbo.StudingRecords",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Grade = c.String(),
                        Field = c.String(),
                        Trend = c.String(),
                        University = c.String(),
                        StartTime = c.Int(),
                        EndTime = c.Int(),
                        ThesisTitle = c.String(),
                        ThesisSupervisors = c.String(),
                        ThesisAdvisors = c.String(),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.Theses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Title = c.String(),
                        ThesisState = c.Byte(nullable: false),
                        ThesisPost = c.Byte(nullable: false),
                        Executers = c.String(),
                        ThesisGrade = c.Byte(nullable: false),
                        ThesisType = c.Byte(nullable: false),
                        Time = c.Int(),
                        Description = c.String(),
                        Link = c.String(),
                        Order = c.Int(),
                        University = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.TrainingRecords",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProfessorId = c.Int(nullable: false),
                        Title = c.String(),
                        Place = c.String(),
                        Time = c.Int(),
                        Link = c.String(),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Professors", t => t.ProfessorId, cascadeDelete: true)
                .Index(t => t.ProfessorId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 450, unicode: false),
                        LastIp = c.String(),
                        Note = c.String(),
                        LastLoginDate = c.DateTime(),
                        RegisterDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DefaultFreeFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 300),
                        Value = c.String(maxLength: 300),
                        Order = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ActivityLogs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SourceAddress = c.String(),
                        ActionBy = c.String(),
                        ActionType = c.String(),
                        Message = c.String(),
                        ActionLevel = c.Byte(nullable: false),
                        ActionDate = c.DateTime(nullable: false),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRolesJunction",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WeeklyPrograms", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.Professors", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRolesJunction", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserRolesJunction", "UserId", "dbo.Users");
            DropForeignKey("dbo.TrainingRecords", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.Theses", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.StudingRecords", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.SectionOrders", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.ResearchRecords", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.Publications", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.PracticeClassInfoes", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.ProfessorMemberships", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.LessonScores", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.LessonPractices", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.LessonNews", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.LessonFiles", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.LessonClassInfoes", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.Languages", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.InternalSeminarRecords", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.InternalResearchRecords", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.LessonImportantDates", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.LessonImportantDates", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.Lessons", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.PracticeClassInfoes", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.LessonScores", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.LessonPractices", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.LessonNews", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.LessonFiles", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.LessonClassInfoes", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.Honors", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.GalleryItems", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.Galleries", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.GalleryItems", "GalleryId", "dbo.Galleries");
            DropForeignKey("dbo.FreeFields", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.ExternalSeminarRecords", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.ExternalResearchRecords", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.Professors", "EducationalGroupId", "dbo.EducationalGroups");
            DropForeignKey("dbo.Professors", "EducationalDegreeId", "dbo.EducationalDegrees");
            DropForeignKey("dbo.DocumentCitations", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.CourseAndWorkshops", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.Professors", "CollegeId", "dbo.Colleges");
            DropForeignKey("dbo.AdminMessages", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.AdministrationRecords", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.Addresses", "ProfessorId", "dbo.Professors");
            DropForeignKey("dbo.Professors", "AcademicRankId", "dbo.AcademicRanks");
            DropIndex("dbo.UserRolesJunction", new[] { "RoleId" });
            DropIndex("dbo.UserRolesJunction", new[] { "UserId" });
            DropIndex("dbo.TrainingRecords", new[] { "ProfessorId" });
            DropIndex("dbo.Theses", new[] { "ProfessorId" });
            DropIndex("dbo.StudingRecords", new[] { "ProfessorId" });
            DropIndex("dbo.SectionOrders", "IX_ProfessorIdAndSectionName");
            DropIndex("dbo.ResearchRecords", new[] { "ProfessorId" });
            DropIndex("dbo.Publications", new[] { "ProfessorId" });
            DropIndex("dbo.ProfessorMemberships", new[] { "ProfessorId" });
            DropIndex("dbo.Languages", new[] { "ProfessorId" });
            DropIndex("dbo.InternalSeminarRecords", new[] { "ProfessorId" });
            DropIndex("dbo.InternalResearchRecords", new[] { "ProfessorId" });
            DropIndex("dbo.PracticeClassInfoes", new[] { "LessonId" });
            DropIndex("dbo.PracticeClassInfoes", new[] { "ProfessorId" });
            DropIndex("dbo.LessonScores", new[] { "LessonId" });
            DropIndex("dbo.LessonScores", new[] { "ProfessorId" });
            DropIndex("dbo.LessonPractices", new[] { "LessonId" });
            DropIndex("dbo.LessonPractices", new[] { "ProfessorId" });
            DropIndex("dbo.LessonNews", new[] { "LessonId" });
            DropIndex("dbo.LessonNews", new[] { "ProfessorId" });
            DropIndex("dbo.LessonFiles", new[] { "LessonId" });
            DropIndex("dbo.LessonFiles", new[] { "ProfessorId" });
            DropIndex("dbo.LessonClassInfoes", new[] { "LessonId" });
            DropIndex("dbo.LessonClassInfoes", new[] { "ProfessorId" });
            DropIndex("dbo.Lessons", new[] { "ProfessorId" });
            DropIndex("dbo.LessonImportantDates", new[] { "LessonId" });
            DropIndex("dbo.LessonImportantDates", new[] { "ProfessorId" });
            DropIndex("dbo.Honors", new[] { "ProfessorId" });
            DropIndex("dbo.GalleryItems", new[] { "GalleryId" });
            DropIndex("dbo.GalleryItems", new[] { "ProfessorId" });
            DropIndex("dbo.Galleries", new[] { "ProfessorId" });
            DropIndex("dbo.FreeFields", new[] { "ProfessorId" });
            DropIndex("dbo.ExternalSeminarRecords", new[] { "ProfessorId" });
            DropIndex("dbo.ExternalResearchRecords", new[] { "ProfessorId" });
            DropIndex("dbo.DocumentCitations", new[] { "ProfessorId" });
            DropIndex("dbo.CourseAndWorkshops", new[] { "ProfessorId" });
            DropIndex("dbo.AdminMessages", new[] { "ProfessorId" });
            DropIndex("dbo.AdministrationRecords", new[] { "ProfessorId" });
            DropIndex("dbo.Addresses", new[] { "ProfessorId" });
            DropIndex("dbo.Professors", new[] { "CollegeId" });
            DropIndex("dbo.Professors", new[] { "EducationalGroupId" });
            DropIndex("dbo.Professors", new[] { "EducationalDegreeId" });
            DropIndex("dbo.Professors", new[] { "AcademicRankId" });
            DropIndex("dbo.Professors", new[] { "UserId" });
            DropIndex("dbo.WeeklyPrograms", new[] { "ProfessorId" });
            DropTable("dbo.UserRolesJunction");
            DropTable("dbo.ActivityLogs");
            DropTable("dbo.DefaultFreeFields");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.TrainingRecords");
            DropTable("dbo.Theses");
            DropTable("dbo.StudingRecords");
            DropTable("dbo.SectionOrders");
            DropTable("dbo.ResearchRecords");
            DropTable("dbo.Publications");
            DropTable("dbo.ProfessorMemberships");
            DropTable("dbo.Languages");
            DropTable("dbo.InternalSeminarRecords");
            DropTable("dbo.InternalResearchRecords");
            DropTable("dbo.PracticeClassInfoes");
            DropTable("dbo.LessonScores");
            DropTable("dbo.LessonPractices");
            DropTable("dbo.LessonNews");
            DropTable("dbo.LessonFiles");
            DropTable("dbo.LessonClassInfoes");
            DropTable("dbo.Lessons");
            DropTable("dbo.LessonImportantDates");
            DropTable("dbo.Honors");
            DropTable("dbo.GalleryItems");
            DropTable("dbo.Galleries");
            DropTable("dbo.FreeFields");
            DropTable("dbo.ExternalSeminarRecords");
            DropTable("dbo.ExternalResearchRecords");
            DropTable("dbo.EducationalGroups");
            DropTable("dbo.EducationalDegrees");
            DropTable("dbo.DocumentCitations");
            DropTable("dbo.CourseAndWorkshops");
            DropTable("dbo.Colleges");
            DropTable("dbo.AdminMessages");
            DropTable("dbo.AdministrationRecords");
            DropTable("dbo.Addresses");
            DropTable("dbo.AcademicRanks");
            DropTable("dbo.Professors");
            DropTable("dbo.WeeklyPrograms");
        }
    }
}
