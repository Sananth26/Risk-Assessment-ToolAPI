using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//https://www.c-sharpcorner.com/blogs/database-connections-in-asp-net-core-20
namespace URSAPI.Models
{
    public partial class dbURSContext : DbContext
    {
        private string connectionString;
        public dbURSContext()
        {
        }

        public IConfiguration Configuration { get; }
        public dbURSContext(DbContextOptions<dbURSContext> options)
            : base(options)
        {
           // var builder = new ConfigurationBuilder();
            //builder.AddJsonFile("appsettings.json", optional: false);
          }

        public virtual DbSet<AuditTrail> AuditTrail { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CompanyInfo> CompanyInfo { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<ErrorLog> ErrorLog { get; set; }
        public virtual DbSet<Holidayplanner> Holidayplanner { get; set; }
        public virtual DbSet<LevelMaster> LevelMaster { get; set; }
        public virtual DbSet<LookupCategory> LookupCategory { get; set; }
        public virtual DbSet<LookupItem> LookupItem { get; set; }
        public virtual DbSet<LookupSubitem> LookupSubitem { get; set; }
        public virtual DbSet<Maildetails> Maildetails { get; set; }
        public virtual DbSet<MainCategory> MainCategory { get; set; }
        public virtual DbSet<Modules> Modules { get; set; }
        public virtual DbSet<Organisation> Organisation { get; set; }
        public virtual DbSet<RemediationComment> RemediationComment { get; set; }
        public virtual DbSet<RemediationLike> RemediationLike { get; set; }
        public virtual DbSet<RemediationReply> RemediationReply { get; set; }
        public virtual DbSet<RemediationUser> RemediationUser { get; set; }
        public virtual DbSet<RequestForm> RequestForm { get; set; }
        public virtual DbSet<RequestLevel> RequestLevel { get; set; }
        public virtual DbSet<RequestSecurityPolicy> RequestSecurityPolicy { get; set; }
        public virtual DbSet<RolePermission> RolePermission { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SubCategory> SubCategory { get; set; }
        public virtual DbSet<SubModules> SubModules { get; set; }
        public virtual DbSet<UarRequestDetails> UarRequestDetails { get; set; }
        public virtual DbSet<UarRequestmaster> UarRequestmaster { get; set; }
        public virtual DbSet<UrsOrg> UrsOrg { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Weekdays> Weekdays { get; set; }
        public virtual DbSet<WorkFlowLevel> WorkFlowLevel { get; set; }
        public virtual DbSet<WorkflowUsers> WorkflowUsers { get; set; }
        public virtual DbSet<Workflowcategory> Workflowcategory { get; set; }
        public virtual DbSet<Workflowdetails> Workflowdetails { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        // Unable to generate entity type for table 'dbo.user_roles'. Please see the warning messages.

        public static string GetConnectionString()
        {
            return Startup.ConnectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.

          
           //    optionsBuilder.UseSqlServer(GetConnectionString());
                
            // optionsBuilder.UseSqlServer("Server=.;Database=dbURS;Uid=sa;Pwd=india@123;Trusted_Connection=false;");
              optionsBuilder.UseSqlServer(@"Server=DESKTOP-3Q8POA5\VEEAMSQL2016; Database=dbURS;Uid=firewall;Pwd=india@123;Trusted_Connection=false;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<AuditTrail>(entity =>
            {
                entity.ToTable("audit_trail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Attachments)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Browser)
                    .HasColumnName("browser")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Event)
                    .HasColumnName("event")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IpAddress)
                    .HasColumnName("ip_address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Module)
                    .HasColumnName("module")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Orgid).HasColumnName("orgid");

                entity.Property(e => e.RequestId)
                    .HasColumnName("requestId")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Systemremarks)
                    .HasColumnName("systemremarks")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.ToTable("User_Roles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");
            });  

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.HeaderNumber).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsActive)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedBy).HasColumnName("last_updated_by");

                entity.Property(e => e.LastUpdatedTime)
                    .HasColumnName("last_updated_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CompanyInfo>(entity =>
            {
                entity.ToTable("company_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("delete_flag")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.FormData)
                    .IsRequired()
                    .HasColumnName("form_data");

                entity.Property(e => e.LastUpdatedBy).HasColumnName("last_updated_by");

                entity.Property(e => e.LastUpdatedTime)
                    .HasColumnName("last_updated_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("department");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("createdDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("deleteFlag")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasColumnName("departmentName")
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("isActive")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modifiedDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NoOfLevel)
                    .HasColumnName("noOfLevel")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.SlaFlag)
                    .IsRequired()
                    .HasColumnName("slaFlag")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.SlaJson).HasColumnName("slaJson");
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.ErrorMessage).IsRequired();

                entity.Property(e => e.Methodname)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Page)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Holidayplanner>(entity =>
            {
                entity.ToTable("holidayplanner");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdby).HasColumnName("createdby");

                entity.Property(e => e.Createddate)
                    .HasColumnName("createddate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Enddate)
                    .IsRequired()
                    .HasColumnName("enddate")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");

                entity.Property(e => e.Modifieddate)
                    .HasColumnName("modifieddate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Startdate)
                    .IsRequired()
                    .HasColumnName("startdate")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LevelMaster>(entity =>
            {
                entity.HasIndex(e => e.WorkFlowLevelName)
                    .HasName("UQ__LevelMas__B4730D41DF47F720")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActiveFlag)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy).HasColumnName("Created_by");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("Created_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasColumnName("Updated_by");

                entity.Property(e => e.UpdatedTime)
                    .HasColumnName("Updated_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.WorkFlowLevelName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LookupCategory>(entity =>
            {
                entity.ToTable("lookup_category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.Property(e => e.UpdatedTime)
                    .HasColumnName("updated_time")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<LookupItem>(entity =>
            {
                entity.ToTable("lookup_item");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActiveFlag)
                    .IsRequired()
                    .HasColumnName("active_flag")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.DisplayOrder).HasColumnName("display_order");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("key")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.Property(e => e.UpdatedTime)
                    .HasColumnName("updated_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LookupSubitem>(entity =>
            {
                entity.ToTable("lookup_subitem");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActiveFlag)
                    .IsRequired()
                    .HasColumnName("active_flag")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.DisplayOrder).HasColumnName("display_order");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("key")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.Property(e => e.UpdatedTime)
                    .HasColumnName("updated_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Maildetails>(entity =>
            {
                entity.ToTable("maildetails");

                entity.Property(e => e.Group)
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.Mailaddress)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MainCategory>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedBy).HasColumnName("last_updated_by");

                entity.Property(e => e.LastUpdatedTime)
                    .HasColumnName("last_updated_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MainCategoryName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Modules>(entity =>
            {
                entity.ToTable("modules");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Children)
                    .IsRequired()
                    .HasColumnName("children")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.DisplayOrder).HasColumnName("display_order");

                entity.Property(e => e.DynamicPresence)
                    .HasColumnName("dynamic_presence")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.DynamicTemplateName)
                    .HasColumnName("dynamic_template_name")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Icon)
                    .IsRequired()
                    .HasColumnName("icon")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Organisation>(entity =>
            {
                entity.ToTable("organisation");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("delete_flag")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Filepath)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.OrgFormData)
                    .IsRequired()
                    .HasColumnName("org_form_data");

                entity.Property(e => e.OrgName)
                    .IsRequired()
                    .HasColumnName("org_name")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.SopCount).HasColumnName("sop_count");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<RemediationComment>(entity =>
            {
                entity.HasKey(e => e.CommentId);

                entity.Property(e => e.Comments)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<RequestForm>(entity =>
            {
                entity.HasKey(e => e.Requestid);

                entity.ToTable("requestForm");

                entity.Property(e => e.Requestid).HasColumnName("requestid");

                entity.Property(e => e.BusinessImpact)
                    .HasColumnName("businessImpact")
                    .IsUnicode(false);

                entity.Property(e => e.BusinessJustification)
                    .HasColumnName("business_Justification")
                    .IsUnicode(false);

                entity.Property(e => e.BusinessOwners)
                    .HasColumnName("businessOwners")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BusinessOwnersId).HasColumnName("businessOwners_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Createdtime)
                    .HasColumnName("createdtime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.EditorId).HasDefaultValueSql("((0))");

                entity.Property(e => e.Editortimeon).HasColumnType("datetime");

                entity.Property(e => e.FirewallRegion).HasColumnName("firewall_Region");

                entity.Property(e => e.IsEditing).HasDefaultValueSql("((0))");

                entity.Property(e => e.ItOwners)
                    .HasColumnName("itOwners")
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ItownersId).HasColumnName("ITowners_id");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.ManagedServices)
                    .HasColumnName("managed_Services")
                    .IsUnicode(false);

                entity.Property(e => e.NameOfProject).IsUnicode(false);

                entity.Property(e => e.NormalExpedited).HasColumnName("normalExpedited");

                entity.Property(e => e.PeerReviewId)
                    .HasColumnName("peerReviewId")
                    .IsUnicode(false);

                entity.Property(e => e.Remarks)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RequestSno)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RiskandRankDetails)
                    .HasColumnName("riskandRankDetails")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Status)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");

                entity.Property(e => e.UserId).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<RequestLevel>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LevelId).HasColumnName("levelId");

                entity.Property(e => e.RequestId).HasColumnName("requestId");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("updateDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("userId");
            });

            modelBuilder.Entity<RequestSecurityPolicy>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Application)
                    .IsRequired()
                    .HasColumnName("application")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DestinationAddress)
                    .IsRequired()
                    .HasColumnName("destinationAddress")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DestinationVpcaccount).HasColumnName("destinationVPCAccount");

                entity.Property(e => e.PortService)
                    .IsRequired()
                    .HasColumnName("portService")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Protocol).HasColumnName("protocol");

                entity.Property(e => e.RequestFormId).HasColumnName("requestFormId");

                entity.Property(e => e.SourceIpaddress)
                    .IsRequired()
                    .HasColumnName("sourceIPAddress")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SourceVpcid).HasColumnName("sourceVPCId");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("role_permission");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActiveFlag)
                    .IsRequired()
                    .HasColumnName("active_flag")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ButtonPermissionData).HasColumnName("button_permission_data");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModuleId).HasColumnName("module_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.SubModuleId).HasColumnName("sub_module_id");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedBy).HasColumnName("last_updated_by");

                entity.Property(e => e.LastUpdatedTime)
                    .HasColumnName("last_updated_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.QuestionNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Risk)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubModules>(entity =>
            {
                entity.ToTable("sub_modules");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.DisplayOrder).HasColumnName("display_order");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TbdModuleId).HasColumnName("tbd_module_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.Property(e => e.UpdatedTime)
                    .HasColumnName("updated_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UarRequestDetails>(entity =>
            {
                entity.ToTable("uar_request_details");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccessCategoryId).HasColumnName("accessCategory_id");

                entity.Property(e => e.AccessTypeId).HasColumnName("accessType_id");

                entity.Property(e => e.ApprovalDetails).HasColumnName("approvalDetails");

                entity.Property(e => e.Attachment).HasColumnName("attachment");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.DateOfRevoke)
                    .HasColumnName("dateOfRevoke")
                    .HasColumnType("date");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("deleteFlag")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.RequestDetailsId)
                    .HasColumnName("requestDetailsId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.SlaData).HasColumnName("slaData");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SubCategoryId).HasColumnName("subCategory_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.WorkflowStage)
                    .IsRequired()
                    .HasColumnName("workflowStage")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UarRequestmaster>(entity =>
            {
                entity.ToTable("uar_requestmaster");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApprovalAttachment).HasColumnName("approvalAttachment");

                entity.Property(e => e.ApprovalDetails).HasColumnName("approvalDetails");

                entity.Property(e => e.Attachment).HasColumnName("attachment");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_Id");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasColumnName("remarks")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RequestDate)
                    .HasColumnName("request_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.SlaData).HasColumnName("slaData");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.WorkflowStage)
                    .IsRequired()
                    .HasColumnName("workflowStage")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UrsOrg>(entity =>
            {
                entity.ToTable("urs_org");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FilePath)
                    .HasColumnName("file_path")
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.OrgId)
                    .HasColumnName("org_id")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.ReqId).HasColumnName("req_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USER");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.CorporateId)
                    .IsRequired()
                    .HasColumnName("corporateId")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("delete_flag")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Department)
                    .HasColumnName("department")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Doj)
                    .HasColumnName("doj")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAlertFlag)
                    .HasColumnName("email_alert_flag")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.EmpCode)
                    .HasColumnName("emp_code")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ImmediateSupervisor).HasColumnName("immediateSupervisor");

                entity.Property(e => e.IsActive)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ItProcessAccess)
                    .IsRequired()
                    .HasColumnName("itProcessAccess")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("location")
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ManagerAccess)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNumber)
                    .HasColumnName("mobile_number")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedTime)
                    .HasColumnName("updated_time")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Weekdays>(entity =>
            {
                entity.ToTable("weekdays");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Days)
                    .HasColumnName("days")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Isactive).HasColumnName("isactive");
            });

            modelBuilder.Entity<Workflowcategory>(entity =>
            {
                entity.ToTable("workflowcategory");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("categoryId")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.SubCategoryId)
                    .HasColumnName("subCategoryId")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.TotalLevel)
                    .HasColumnName("totalLevel")
                    .HasDefaultValueSql("('0')");
            });

            modelBuilder.Entity<Workflowdetails>(entity =>
            {
                entity.ToTable("workflowdetails");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActiveFlag)
                    .IsRequired()
                    .HasColumnName("activeFlag")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CombinationId)
                    .HasColumnName("combinationId")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("createdDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("deleteFlag")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.LevelName)
                    .IsRequired()
                    .HasColumnName("levelName")
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modifiedDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SelectedLevels).HasColumnName("selectedLevels");

                entity.Property(e => e.SelectedRoles)
                    .IsRequired()
                    .HasColumnName("selectedRoles");

                entity.Property(e => e.SelectedUsers)
                    .IsRequired()
                    .HasColumnName("selectedUsers");

                entity.Property(e => e.SlaActive)
                    .IsRequired()
                    .HasColumnName("slaActive")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.SlaDays)
                    .HasColumnName("slaDays")
                    .HasDefaultValueSql("('0')");
            });
        }
    }
}
