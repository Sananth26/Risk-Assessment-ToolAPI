using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace URSAPI.Models
{
    public partial class ursContext : DbContext
    {
        public ursContext()
        {
        }

        public ursContext(DbContextOptions<ursContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuditTrail> AuditTrail { get; set; }
        public virtual DbSet<CompanyInfo> CompanyInfo { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Holidayplanner> Holidayplanner { get; set; }
        public virtual DbSet<LookupCategory> LookupCategory { get; set; }
        public virtual DbSet<LookupItem> LookupItem { get; set; }
        public virtual DbSet<LookupSubitem> LookupSubitem { get; set; }
        public virtual DbSet<Maildetails> Maildetails { get; set; }
        public virtual DbSet<Modules> Modules { get; set; }
        public virtual DbSet<Organisation> Organisation { get; set; }
        public virtual DbSet<RolePermission> RolePermission { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SubModules> SubModules { get; set; }
        public virtual DbSet<UarRequestDetails> UarRequestDetails { get; set; }
        public virtual DbSet<UarRequestmaster> UarRequestmaster { get; set; }
        public virtual DbSet<UrsOrg> UrsOrg { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<Weekdays> Weekdays { get; set; }
        public virtual DbSet<Workflow> Workflow { get; set; }
        public virtual DbSet<Workflowcategory> Workflowcategory { get; set; }
        public virtual DbSet<Workflowdetails> Workflowdetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=127.0.0.1;port=3306;user=root;password=root;database=urs");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<AuditTrail>(entity =>
            {
                entity.ToTable("audit_trail", "urs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Browser)
                    .HasColumnName("browser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnName("created_time");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Event)
                    .IsRequired()
                    .HasColumnName("event")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IpAddress)
                    .IsRequired()
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

            modelBuilder.Entity<CompanyInfo>(entity =>
            {
                entity.ToTable("company_info", "urs");

                entity.HasIndex(e => e.CreatedBy)
                    .HasName("FK_company_info_user");

                entity.HasIndex(e => e.LastUpdatedBy)
                    .HasName("FK_company_info_user_2");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("delete_flag")
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("N");

                entity.Property(e => e.FormData)
                    .IsRequired()
                    .HasColumnName("form_data")
                    .HasColumnType("json");

                entity.Property(e => e.LastUpdatedBy).HasColumnName("last_updated_by");

                entity.Property(e => e.LastUpdatedTime)
                    .HasColumnName("last_updated_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("department", "urs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CreatedDate).HasColumnName("createdDate");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("deleteFlag")
                    .HasColumnType("char(50)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.DepartmentName)
                    .IsRequired()
                    .HasColumnName("departmentName")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("isActive")
                    .HasColumnType("char(50)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ModifiedDate).HasColumnName("modifiedDate");

                entity.Property(e => e.NoOfLevel)
                    .HasColumnName("noOfLevel")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SlaFlag)
                    .IsRequired()
                    .HasColumnName("slaFlag")
                    .HasColumnType("char(50)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SlaJson)
                    .HasColumnName("slaJson")
                    .HasColumnType("json");
            });

            modelBuilder.Entity<Holidayplanner>(entity =>
            {
                entity.ToTable("holidayplanner", "urs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Createdby).HasColumnName("createdby");

                entity.Property(e => e.Createddate).HasColumnName("createddate");

                entity.Property(e => e.Enddate)
                    .IsRequired()
                    .HasColumnName("enddate")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");

                entity.Property(e => e.Modifieddate).HasColumnName("modifieddate");

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

            modelBuilder.Entity<LookupCategory>(entity =>
            {
                entity.ToTable("lookup_category", "urs");

                entity.HasIndex(e => e.Name)
                    .HasName("name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint unsigned");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

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
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<LookupItem>(entity =>
            {
                entity.ToTable("lookup_item", "urs");

                entity.HasIndex(e => e.CategoryId)
                    .HasName("FK_lookup_item_lookup_category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActiveFlag)
                    .IsRequired()
                    .HasColumnName("active_flag")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("Y");

                entity.Property(e => e.Category)
                    .HasColumnName("category")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .HasColumnType("bigint unsigned");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("display_order")
                    .HasColumnType("smallint unsigned");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("key")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.Property(e => e.UpdatedTime)
                    .HasColumnName("updated_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                 
            });

            modelBuilder.Entity<LookupSubitem>(entity =>
            {
                entity.ToTable("lookup_subitem", "urs");

                entity.HasIndex(e => e.SubcategoryId)
                    .HasName("FK_lookup_subitem_lookup_item");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActiveFlag)
                    .IsRequired()
                    .HasColumnName("active_flag")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("Y");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DisplayOrder).HasColumnName("display_order");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("key")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubcategoryId).HasColumnName("subcategory_id");

                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.UpdatedTime)
                    .HasColumnName("updated_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                //entity.HasOne(d => d.Subcategory)
                //    .WithMany(p => p.LookupSubitem)
                //    .HasForeignKey(d => d.SubcategoryId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_lookup_subitem_lookup_item");
            });

            modelBuilder.Entity<Maildetails>(entity =>
            {
                entity.ToTable("maildetails", "urs");

                entity.Property(e => e.Id).HasDefaultValueSql("0");

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

            modelBuilder.Entity<Modules>(entity =>
            {
                entity.ToTable("modules", "urs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Children)
                    .IsRequired()
                    .HasColumnName("children")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("N");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.DisplayOrder).HasColumnName("display_order");

                entity.Property(e => e.DynamicPresence)
                    .HasColumnName("dynamic_presence")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("N");

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
                    .HasMaxLength(50)
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
                entity.ToTable("organisation", "urs");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint unsigned");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasColumnType("bigint unsigned");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("delete_flag")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("N");

                entity.Property(e => e.Filepath)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("N");

                entity.Property(e => e.OrgFormData)
                    .IsRequired()
                    .HasColumnName("org_form_data")
                    .HasColumnType("json");

                entity.Property(e => e.OrgName)
                    .IsRequired()
                    .HasColumnName("org_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SopCount)
                    .HasColumnName("sop_count")
                    .HasColumnType("bigint unsigned");

                entity.Property(e => e.UpdateBy)
                    .HasColumnName("update_by")
                    .HasColumnType("bigint unsigned");

                entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("role_permission", "urs");

                entity.HasIndex(e => e.CreatedBy)
                    .HasName("FK_role_permission_user");

                entity.HasIndex(e => e.ModuleId)
                    .HasName("FK_role_permission_eigen_modules");

                entity.HasIndex(e => e.RoleId)
                    .HasName("FK_permission_roles");

                entity.HasIndex(e => e.SubModuleId)
                    .HasName("FK_role_permission_eigen_sub_modules");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint unsigned");

                entity.Property(e => e.ActiveFlag)
                    .IsRequired()
                    .HasColumnName("active_flag")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("N");

                entity.Property(e => e.ButtonPermissionData)
                    .HasColumnName("button_permission_data")
                    .HasColumnType("json");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .HasDefaultValueSql("2");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModuleId).HasColumnName("module_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.SubModuleId).HasColumnName("sub_module_id");

                //entity.HasOne(d => d.CreatedByNavigation)
                //    .WithMany(p => p.RolePermission)
                //    .HasForeignKey(d => d.CreatedBy)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_role_permission_user");

                //entity.HasOne(d => d.Module)
                //    .WithMany(p => p.RolePermission)
                //    .HasForeignKey(d => d.ModuleId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_role_permission_wms_modules");

                //entity.HasOne(d => d.Role)
                //    .WithMany(p => p.RolePermission)
                //    .HasForeignKey(d => d.RoleId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_permission_roles");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("roles", "urs");

                entity.HasIndex(e => e.Name)
                    .HasName("UK_nb4h0p6txrmfc0xbrd1kglp9t")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SubModules>(entity =>
            {
                entity.ToTable("sub_modules", "urs");

                entity.HasIndex(e => e.CreatedBy)
                    .HasName("FK_wms_sub_modules_user");

                entity.HasIndex(e => e.TbdModuleId)
                    .HasName("FK_wms_sub_modules_wmsmodules");

                entity.HasIndex(e => e.UpdatedBy)
                    .HasName("FK_wms_sub_modules_user_2");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedTime).HasColumnName("created_time");

                entity.Property(e => e.DisplayOrder).HasColumnName("display_order");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
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

                entity.Property(e => e.UpdatedTime).HasColumnName("updated_time");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                //entity.HasOne(d => d.CreatedByNavigation)
                //    .WithMany(p => p.SubModulesCreatedByNavigation)
                //    .HasForeignKey(d => d.CreatedBy)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_wms_sub_modules_user");

                //entity.HasOne(d => d.TbdModule)
                //    .WithMany(p => p.SubModules)
                //    .HasForeignKey(d => d.TbdModuleId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_wms_sub_modules_wmsmodules");

                //entity.HasOne(d => d.UpdatedByNavigation)
                //    .WithMany(p => p.SubModulesUpdatedByNavigation)
                //    .HasForeignKey(d => d.UpdatedBy)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_wms_sub_modules_user_2");
            });

            modelBuilder.Entity<UarRequestDetails>(entity =>
            {
                entity.ToTable("uar_request_details", "urs");

                entity.HasIndex(e => e.RequestId)
                    .HasName("FK_uar_request_details_uar_requestmaster");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint unsigned");

                entity.Property(e => e.AccessCategoryId).HasColumnName("accessCategory_id");

                entity.Property(e => e.AccessTypeId).HasColumnName("accessType_id");

                entity.Property(e => e.ApprovalDetails)
                    .HasColumnName("approvalDetails")
                    .HasColumnType("json");

                entity.Property(e => e.Attachment)
                    .HasColumnName("attachment")
                    .HasColumnType("json");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.DateOfRevoke)
                    .HasColumnName("dateOfRevoke")
                    .HasColumnType("date");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("deleteFlag")
                    .HasColumnType("char(50)");

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.RequestDetailsId)
                    .HasColumnName("requestDetailsId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RequestId)
                    .HasColumnName("request_id")
                    .HasColumnType("bigint unsigned");

                entity.Property(e => e.SlaData)
                    .HasColumnName("slaData")
                    .HasColumnType("json");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubCategoryId).HasColumnName("subCategory_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.WorkflowStage)
                    .IsRequired()
                    .HasColumnName("workflowStage")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                //entity.HasOne(d => d.Request)
                //    .WithMany(p => p.UarRequestDetails)
                //    .HasForeignKey(d => d.RequestId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_uar_request_details_uar_requestmaster");
            });

            modelBuilder.Entity<UarRequestmaster>(entity =>
            {
                entity.ToTable("uar_requestmaster", "urs");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint unsigned");

                entity.Property(e => e.ApprovalAttachment)
                    .HasColumnName("approvalAttachment")
                    .HasColumnType("json");

                entity.Property(e => e.ApprovalDetails)
                    .HasColumnName("approvalDetails")
                    .HasColumnType("json");

                entity.Property(e => e.Attachment)
                    .HasColumnName("attachment")
                    .HasColumnType("json");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_Id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasColumnName("remarks")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RequestDate).HasColumnName("request_date");

                entity.Property(e => e.SlaData)
                    .HasColumnName("slaData")
                    .HasColumnType("json");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("0");

                entity.Property(e => e.WorkflowStage)
                    .IsRequired()
                    .HasColumnName("workflowStage")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UrsOrg>(entity =>
            {
                entity.ToTable("urs_org", "urs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FilePath)
                    .HasColumnName("file_path")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OrgId)
                    .HasColumnName("org_id")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ReqId).HasColumnName("req_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user", "urs");

                entity.HasIndex(e => e.CompanyId)
                    .HasName("FK_user_company_info");

                entity.HasIndex(e => e.Email)
                    .HasName("UKob8kqyqqgmefl0aco34akdtpe")
                    .IsUnique();

                entity.HasIndex(e => e.MobileNumber)
                    .HasName("mobile_number")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("delete_flag")
                    .HasColumnType("char(1)");

                entity.Property(e => e.Department)
                    .HasColumnName("department")
                    .HasDefaultValueSql("0");

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
                    .HasDefaultValueSql("N");

                entity.Property(e => e.EmpCode)
                    .HasColumnName("emp_code")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("N");

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ImmediateSupervisor).HasColumnName("immediateSupervisor");

                entity.Property(e => e.ItProcessAccess)
                    .IsRequired()
                    .HasColumnName("itProcessAccess")
                    .HasColumnType("char(50)")
                    .HasDefaultValueSql("Y");

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("location")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNumber).HasColumnName("mobile_number");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedTime)
                    .HasColumnName("updated_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                //entity.HasOne(d => d.Company)
                //    .WithMany(p => p.User)
                //    .HasForeignKey(d => d.CompanyId)
                //    .HasConstraintName("FK_user_company_info");
            });

            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.ToTable("user_roles", "urs");

                entity.HasIndex(e => e.RoleId)
                    .HasName("FKh8ciramu9cc9q3qcqiv4ue8a6");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                //entity.HasOne(d => d.Role)
                //    .WithMany(p => p.UserRoles)
                //    .HasForeignKey(d => d.RoleId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FKh8ciramu9cc9q3qcqiv4ue8a6");

                //entity.HasOne(d => d.User)
                //    .WithMany(p => p.UserRoles)
                //    .HasForeignKey(d => d.UserId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK55itppkw3i07do3h7qoclqd4k");
            });

            modelBuilder.Entity<Weekdays>(entity =>
            {
                entity.ToTable("weekdays", "urs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Days)
                    .IsRequired()
                    .HasColumnName("days")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Isactive)
                    .HasColumnName("isactive")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<Workflow>(entity =>
            {
                entity.ToTable("workflow", "urs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActiveFlag)
                    .IsRequired()
                    .HasColumnType("char(50)");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreatedDate).HasColumnName("createdDate");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("deleteFlag")
                    .HasColumnType("char(50)");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.LevelName)
                    .HasColumnName("levelName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");

                entity.Property(e => e.ModifiedDate).HasColumnName("modifiedDate");

                entity.Property(e => e.SelectedCategory)
                    .HasColumnName("selectedCategory")
                    .HasColumnType("json");

                entity.Property(e => e.SelectedRoles)
                    .HasColumnName("selectedRoles")
                    .HasColumnType("json");

                entity.Property(e => e.SelectedSubCategory)
                    .HasColumnName("selectedSubCategory")
                    .HasColumnType("json");

                entity.Property(e => e.SelectedUsers)
                    .HasColumnName("selectedUsers")
                    .HasColumnType("json");

                entity.Property(e => e.SlaActive)
                    .HasColumnName("slaActive")
                    .HasColumnType("char(50)");

                entity.Property(e => e.SlaDays).HasColumnName("slaDays");
            });

            modelBuilder.Entity<Workflowcategory>(entity =>
            {
                entity.ToTable("workflowcategory", "urs");

                entity.HasIndex(e => e.CategoryId)
                    .HasName("FK__lookup_item");

                entity.HasIndex(e => e.SubCategoryId)
                    .HasName("FK__lookup_subitem");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("categoryId")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SubCategoryId)
                    .HasColumnName("subCategoryId")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TotalLevel)
                    .HasColumnName("totalLevel")
                    .HasDefaultValueSql("0");

                //entity.HasOne(d => d.Category)
                //    .WithMany(p => p.Workflowcategory)
                //    .HasForeignKey(d => d.CategoryId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK__lookup_item");

                //entity.HasOne(d => d.SubCategory)
                //    .WithMany(p => p.Workflowcategory)
                //    .HasForeignKey(d => d.SubCategoryId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK__lookup_subitem");
            });

            modelBuilder.Entity<Workflowdetails>(entity =>
            {
                entity.ToTable("workflowdetails", "urs");

                entity.HasIndex(e => e.CombinationId)
                    .HasName("FK__workflowcategory");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActiveFlag)
                    .IsRequired()
                    .HasColumnName("activeFlag")
                    .HasColumnType("char(50)")
                    .HasDefaultValueSql("Y");

                entity.Property(e => e.CombinationId)
                    .HasColumnName("combinationId")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.CreatedDate).HasColumnName("createdDate");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("deleteFlag")
                    .HasColumnType("char(50)")
                    .HasDefaultValueSql("N");

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.LevelName)
                    .IsRequired()
                    .HasColumnName("levelName")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ModifiedDate).HasColumnName("modifiedDate");

                entity.Property(e => e.SelectedRoles)
                    .IsRequired()
                    .HasColumnName("selectedRoles")
                    .HasColumnType("json");

                entity.Property(e => e.SelectedUsers)
                    .IsRequired()
                    .HasColumnName("selectedUsers")
                    .HasColumnType("json");

                entity.Property(e => e.SlaActive)
                    .IsRequired()
                    .HasColumnName("slaActive")
                    .HasColumnType("char(50)")
                    .HasDefaultValueSql("Y");

                entity.Property(e => e.SlaDays)
                    .HasColumnName("slaDays")
                    .HasDefaultValueSql("0");

                //entity.HasOne(d => d.Combination)
                //    .WithMany(p => p.Workflowdetails)
                //    .HasForeignKey(d => d.CombinationId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK__workflowcategory");
            });
        }
    }
}
