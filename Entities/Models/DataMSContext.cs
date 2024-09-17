using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

public partial class DataMSContext : DbContext
{
    public DataMSContext()
    {
    }

    public DataMSContext(DbContextOptions<DataMSContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountAccessApi> AccountAccessApis { get; set; }

    public virtual DbSet<AccountAccessApiPermission> AccountAccessApiPermissions { get; set; }

    public virtual DbSet<AccountClient> AccountClients { get; set; }

    public virtual DbSet<Action> Actions { get; set; }

    public virtual DbSet<AddressClient> AddressClients { get; set; }

    public virtual DbSet<AffiliateGroupProduct> AffiliateGroupProducts { get; set; }

    public virtual DbSet<AllCode> AllCodes { get; set; }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }

    public virtual DbSet<ArticleRelated> ArticleRelateds { get; set; }

    public virtual DbSet<ArticleTag> ArticleTags { get; set; }

    public virtual DbSet<AttachFile> AttachFiles { get; set; }

    public virtual DbSet<BankOnePay> BankOnePays { get; set; }

    public virtual DbSet<BankingAccount> BankingAccounts { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Campaign> Campaigns { get; set; }

    public virtual DbSet<CampaignAd> CampaignAds { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientLinkAff> ClientLinkAffs { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<ContractHistory> ContractHistories { get; set; }

    public virtual DbSet<ContractPay> ContractPays { get; set; }

    public virtual DbSet<ContractPayDetail> ContractPayDetails { get; set; }

    public virtual DbSet<DebtStatistic> DebtStatistics { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DepositHistory> DepositHistories { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<GroupProduct> GroupProducts { get; set; }

    public virtual DbSet<ImageSize> ImageSizes { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    public virtual DbSet<InvoiceRequest> InvoiceRequests { get; set; }

    public virtual DbSet<InvoiceRequestDetail> InvoiceRequestDetails { get; set; }

    public virtual DbSet<InvoiceRequestHistory> InvoiceRequestHistories { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<LocationProduct> LocationProducts { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuPermission> MenuPermissions { get; set; }

    public virtual DbSet<Mfauser> Mfausers { get; set; }

    public virtual DbSet<National> Nationals { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentAccount> PaymentAccounts { get; set; }

    public virtual DbSet<PaymentRequest> PaymentRequests { get; set; }

    public virtual DbSet<PaymentRequestDetail> PaymentRequestDetails { get; set; }

    public virtual DbSet<PaymentVoucher> PaymentVouchers { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<PolicyDetail> PolicyDetails { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<ProvinceHotel> ProvinceHotels { get; set; }

    public virtual DbSet<ReceivePromotion> ReceivePromotions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<RunningScheduleService> RunningScheduleServices { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierContact> SupplierContacts { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TelegramDetail> TelegramDetails { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAgent> UserAgents { get; set; }

    public virtual DbSet<UserDepart> UserDeparts { get; set; }

    public virtual DbSet<UserPosition> UserPositions { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<VoucherCampaign> VoucherCampaigns { get; set; }

    public virtual DbSet<VoucherLogActivity> VoucherLogActivities { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=103.163.216.41;Initial Catalog=Hulotoy;Persist Security Info=True;User ID=us;Password=us@585668;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountAccessApi>(entity =>
        {
            entity.ToTable("AccountAccessApi");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AccountAccessApiPermission>(entity =>
        {
            entity.ToTable("AccountAccessApiPermission");

            entity.HasOne(d => d.AccountAccessApi).WithMany(p => p.AccountAccessApiPermissions)
                .HasForeignKey(d => d.AccountAccessApiId)
                .HasConstraintName("FK_AccountAccessApiPermission_AccountAccessApi");

            entity.HasOne(d => d.ProjectTypeNavigation).WithMany(p => p.AccountAccessApiPermissions)
                .HasForeignKey(d => d.ProjectType)
                .HasConstraintName("FK_AccountAccessApiPermission_AllCode");
        });

        modelBuilder.Entity<AccountClient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Account");

            entity.ToTable("AccountClient");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ForgotPasswordToken)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GoogleToken)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordBackup)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Action>(entity =>
        {
            entity.ToTable("Action");

            entity.Property(e => e.ActionName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ControllerName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Menu).WithMany(p => p.Actions)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FK_Action_Menu");

            entity.HasOne(d => d.Permission).WithMany(p => p.Actions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK_Action_Permission");
        });

        modelBuilder.Entity<AddressClient>(entity =>
        {
            entity.ToTable("AddressClient");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DistrictId).HasMaxLength(5);
            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Đây là số điện thoại nhận hàng");
            entity.Property(e => e.ProvinceId).HasMaxLength(5);
            entity.Property(e => e.ReceiverName).HasMaxLength(255);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.WardId).HasMaxLength(5);
        });

        modelBuilder.Entity<AffiliateGroupProduct>(entity =>
        {
            entity.ToTable("AffiliateGroupProduct");
        });

        modelBuilder.Entity<AllCode>(entity =>
        {
            entity.ToTable("AllCode");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable("Article");

            entity.Property(e => e.Body)
                .IsRequired()
                .HasColumnType("ntext");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DownTime).HasColumnType("datetime");
            entity.Property(e => e.Image11).HasMaxLength(350);
            entity.Property(e => e.Image169)
                .IsRequired()
                .HasMaxLength(350);
            entity.Property(e => e.Image43).HasMaxLength(350);
            entity.Property(e => e.Lead)
                .IsRequired()
                .HasMaxLength(400);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.PublishDate).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.UpTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<ArticleCategory>(entity =>
        {
            entity.ToTable("ArticleCategory");

            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
        });

        modelBuilder.Entity<ArticleRelated>(entity =>
        {
            entity.ToTable("ArticleRelated");

            entity.Property(e => e.UpdateLast).HasColumnType("datetime");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleRelateds)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_ArticleRelated_Article");
        });

        modelBuilder.Entity<ArticleTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ArticleTags");

            entity.ToTable("ArticleTag");

            entity.Property(e => e.UpdateLast).HasColumnType("datetime");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleTags)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_ArticleTags_Article");

            entity.HasOne(d => d.Tag).WithMany(p => p.ArticleTags)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("FK_ArticleTags_Tags");
        });

        modelBuilder.Entity<AttachFile>(entity =>
        {
            entity.ToTable("AttachFile");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Ext)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Path).HasMaxLength(400);
        });

        modelBuilder.Entity<BankOnePay>(entity =>
        {
            entity.ToTable("BankOnePay");

            entity.Property(e => e.BankName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bank_name");
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.FullnameEn)
                .HasMaxLength(200)
                .HasColumnName("fullname_en");
            entity.Property(e => e.FullnameVi)
                .HasMaxLength(200)
                .HasColumnName("fullname_vi");
            entity.Property(e => e.Logo)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("logo");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<BankingAccount>(entity =>
        {
            entity.ToTable("BankingAccount");

            entity.Property(e => e.AccountName).HasMaxLength(200);
            entity.Property(e => e.AccountNumber)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.BankId)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Branch).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("Brand");

            entity.Property(e => e.BrandCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BrandName).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tblPrice");

            entity.ToTable("Campaign");

            entity.Property(e => e.CampaignCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
        });

        modelBuilder.Entity<CampaignAd>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CampaignName).HasMaxLength(300);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(400);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tblAccount");

            entity.ToTable("Client");

            entity.Property(e => e.Avartar)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.BusinessAddress).HasMaxLength(200);
            entity.Property(e => e.ClientCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ClientName).HasMaxLength(256);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ExportBillAddress).HasMaxLength(200);
            entity.Property(e => e.IsReceiverInfoEmail).HasColumnName("isReceiverInfoEmail");
            entity.Property(e => e.JoinDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(400);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReferralId)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.TaxNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<ClientLinkAff>(entity =>
        {
            entity.ToTable("ClientLinkAff");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ClientId).ValueGeneratedOnAdd();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.LinkAff)
                .IsRequired()
                .IsUnicode(false);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(1000);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ContractHistory>(entity =>
        {
            entity.ToTable("ContractHistory");

            entity.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.ActionDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ContractPay>(entity =>
        {
            entity.HasKey(e => e.PayId);

            entity.ToTable("ContractPay");

            entity.Property(e => e.AttatchmentFile)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BillNo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ExportDate)
                .HasComment("Ngày xuất hóa đơn")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Note).HasMaxLength(300);
            entity.Property(e => e.PayType).HasComment("1: Tiền mặt , 2: Chuyển khoản");
            entity.Property(e => e.Type).HasComment("1:Thu tiền đơn hàng , 2: Thu tiền nạp quỹ");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ContractPayDetail>(entity =>
        {
            entity.ToTable("ContractPayDetail");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ServiceCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DebtStatistic>(entity =>
        {
            entity.ToTable("DebtStatistic");

            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Currency).HasMaxLength(50);
            entity.Property(e => e.DeclineReason).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.OrderIds)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.VerifyDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepartmentName).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.FullParent)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.IsReport).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DepositHistory>(entity =>
        {
            entity.ToTable("DepositHistory");

            entity.Property(e => e.BankAccount).HasMaxLength(150);
            entity.Property(e => e.BankName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate)
                .HasComment("Thời gian giao dịch")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageScreen)
                .HasMaxLength(200)
                .HasComment("Ảnh ủy nhiệm chi");
            entity.Property(e => e.NoteReject).HasMaxLength(300);
            entity.Property(e => e.PaymentType).HasComment("HÌnh thức thanh toán");
            entity.Property(e => e.Price).HasComment("Số tiền nạp");
            entity.Property(e => e.Status).HasComment("Trạng thái");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasComment("Tiêu đề nạp");
            entity.Property(e => e.TransNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Mã giao dịch");
            entity.Property(e => e.TransType).HasComment("Loại giao dịch");
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasComment("User nạp trans. Lấy user login");
            entity.Property(e => e.VerifyDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__District__85FDA4C6D88CB461");

            entity.ToTable("District");

            entity.Property(e => e.DistrictId)
                .IsRequired()
                .HasMaxLength(5);
            entity.Property(e => e.Location).HasMaxLength(30);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.NameNonUnicode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ProvinceId)
                .IsRequired()
                .HasMaxLength(5);
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(30);
        });

        modelBuilder.Entity<GroupProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_groupProduct");

            entity.ToTable("GroupProduct");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImagePath)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(400);
            entity.Property(e => e.Path)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ImageSize>(entity =>
        {
            entity.ToTable("ImageSize");

            entity.Property(e => e.PositionName)
                .IsRequired()
                .HasMaxLength(250);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoice");

            entity.Property(e => e.AttactFile)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceFromId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceSignId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.VerifyDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.ToTable("InvoiceDetail");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<InvoiceRequest>(entity =>
        {
            entity.ToTable("InvoiceRequest");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.AttachFile)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeclineReason).HasMaxLength(500);
            entity.Property(e => e.InvoiceRequestNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.TaxNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.VerifyDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<InvoiceRequestDetail>(entity =>
        {
            entity.ToTable("InvoiceRequestDetail");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ProductName).HasMaxLength(500);
            entity.Property(e => e.Unit).HasMaxLength(200);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Vat).HasColumnName("VAT");
        });

        modelBuilder.Entity<InvoiceRequestHistory>(entity =>
        {
            entity.ToTable("InvoiceRequestHistory");

            entity.Property(e => e.Actioin).HasMaxLength(4000);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.ToTable("Job");

            entity.Property(e => e.Type).HasComment("1: sync client ; 2 : sync order");
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Store");

            entity.ToTable("Label");

            entity.Property(e => e.CreateTime).HasColumnType("datetime");
            entity.Property(e => e.DescExpire).HasMaxLength(300);
            entity.Property(e => e.Domain)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Icon).HasMaxLength(500);
            entity.Property(e => e.PrefixOrderCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StoreName).HasMaxLength(50);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<LocationProduct>(entity =>
        {
            entity.ToTable("LocationProduct");

            entity.Property(e => e.CreateOn).HasColumnType("datetime");
            entity.Property(e => e.ProductCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("Menu");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.FullParent)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.Link).HasMaxLength(200);
            entity.Property(e => e.MenuCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<Mfauser>(entity =>
        {
            entity.ToTable("MFAUser");

            entity.Property(e => e.BackupCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SecretKey)
                .IsRequired()
                .HasMaxLength(32)
                .IsFixedLength();
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            entity.Property(e => e.UserCreatedYear)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<National>(entity =>
        {
            entity.ToTable("National");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.NameVn)
                .HasMaxLength(200)
                .HasColumnName("NameVN");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.ToTable("Note");

            entity.Property(e => e.Comment).HasMaxLength(400);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasComment("Chính là label so với wiframe");
            entity.Property(e => e.OrderNo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.ReceiverName).HasMaxLength(150);
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
            entity.Property(e => e.UserGroupIds).HasMaxLength(250);
            entity.Property(e => e.UtmMedium).HasMaxLength(250);
            entity.Property(e => e.UtmSource).HasMaxLength(50);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ProductId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductLink)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payment");

            entity.Property(e => e.BankName).HasMaxLength(50);
            entity.Property(e => e.BotPaymentScreenShot).HasMaxLength(250);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DepositPaymentType).HasComment("loại thanh toán cho đối tượng nào. Đơn hàng hay nạp quỹ");
            entity.Property(e => e.ImageScreenShot).HasMaxLength(250);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.TransferContent).HasMaxLength(250);
        });

        modelBuilder.Entity<PaymentAccount>(entity =>
        {
            entity.ToTable("PaymentAccount");

            entity.Property(e => e.AccountName)
                .HasMaxLength(50)
                .HasComment("Tên chủ tài khoản");
            entity.Property(e => e.AccountNumb)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Số tài khoản");
            entity.Property(e => e.BankName)
                .HasMaxLength(50)
                .HasComment("Tên ngân hàng");
            entity.Property(e => e.Branch)
                .HasMaxLength(50)
                .HasComment("Chi nhánh");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(0);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<PaymentRequest>(entity =>
        {
            entity.ToTable("PaymentRequest");

            entity.Property(e => e.AbandonmentReason).HasMaxLength(500);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BankAccount)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankName).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeclineReason).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(3000);
            entity.Property(e => e.IsPaymentBefore).HasDefaultValue(false);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.PaymentCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.Type).HasComment("1: Thanh toán dịch vụ , 2: Thanh toán khác");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.VerifyDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<PaymentRequestDetail>(entity =>
        {
            entity.ToTable("PaymentRequestDetail");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ServiceCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<PaymentVoucher>(entity =>
        {
            entity.ToTable("PaymentVoucher");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AttachFiles)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BankAccount)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankName).HasMaxLength(200);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.PaymentCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RequestId)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("Id Phiếu yêu cầu chi");
            entity.Property(e => e.Type).HasComment("1: Thanh toán dịch vụ , 2: Thanh toán khác");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permission");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.ToTable("Policy");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EffectiveDate).HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.IsPrivate).HasDefaultValue(false);
            entity.Property(e => e.PolicyCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PolicyName).HasMaxLength(500);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<PolicyDetail>(entity =>
        {
            entity.ToTable("PolicyDetail");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.HotelDebtAmout).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HotelDepositAmout).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductFlyTicketDebtAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductFlyTicketDepositAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TourDebtAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TourDepositAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TouringCarDebtAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TouringCarDepositAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.VinWonderDebtAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VinWonderDepositAmount).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("Position");

            entity.Property(e => e.PositionName)
                .IsRequired()
                .HasMaxLength(250);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.CreateOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Information).HasColumnType("ntext");
            entity.Property(e => e.LinkSource)
                .HasMaxLength(4000)
                .IsUnicode(false);
            entity.Property(e => e.Manufacturer).HasMaxLength(400);
            entity.Property(e => e.Path)
                .HasMaxLength(4000)
                .IsUnicode(false);
            entity.Property(e => e.ProductCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Rating)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SellerId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SellerName)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(300);
            entity.Property(e => e.UnitWeight)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
            entity.Property(e => e.Variations).HasMaxLength(400);
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Province__FD0A6F838767F971");

            entity.ToTable("Province");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.NameNonUnicode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ProvinceId)
                .IsRequired()
                .HasMaxLength(5);
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(30);
        });

        modelBuilder.Entity<ProvinceHotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProvinceHotel");

            entity.ToTable("ProvinceHotel");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ReceivePromotion>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("RolePermission");

            entity.HasOne(d => d.Menu).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Menu");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Permission");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Role");
        });

        modelBuilder.Entity<RunningScheduleService>(entity =>
        {
            entity.ToTable("RunningScheduleService");

            entity.Property(e => e.LogDate).HasColumnType("datetime");

            entity.HasOne(d => d.Price).WithMany(p => p.RunningScheduleServices)
                .HasForeignKey(d => d.PriceId)
                .HasConstraintName("FK_RunningScheduleService_Price");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Supplier");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(500);
            entity.Property(e => e.IsDisplayWebsite).HasDefaultValue(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ResidenceType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ServiceType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName).HasMaxLength(50);
            entity.Property(e => e.SupplierCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TaxCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<SupplierContact>(entity =>
        {
            entity.ToTable("SupplierContact");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Position).HasMaxLength(500);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Tags");

            entity.ToTable("Tag");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.TagName).HasMaxLength(100);
        });

        modelBuilder.Entity<TelegramDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Telegram");

            entity.ToTable("TelegramDetail");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.GroupChatId)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GroupLog)
                .IsRequired()
                .HasMaxLength(80);
            entity.Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.Property(e => e.BankReference)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.ContractNo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(400);
            entity.Property(e => e.TransactionNo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.VerifyDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User_1");

            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Avata).HasMaxLength(500);
            entity.Property(e => e.BirthDay).HasColumnType("datetime");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(500);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(2500);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ResetPassword)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserAgent>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ClientId });

            entity.ToTable("UserAgent");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.MainFollow).HasComment("Quyền danh cho  0: Đối tác | 1: nhân viên của đối tác | 2: Saler phụ trách chính | 3: saler phụ trách cùng");
            entity.Property(e => e.UpdateLast).HasColumnType("datetime");
            entity.Property(e => e.VerifyDate).HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.UserAgents)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAgent_Client");

            entity.HasOne(d => d.User).WithMany(p => p.UserAgents)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAgent_User");
        });

        modelBuilder.Entity<UserDepart>(entity =>
        {
            entity.ToTable("UserDepart");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserPosition>(entity =>
        {
            entity.ToTable("UserPosition");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_User");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.ToTable("Voucher");

            entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
            entity.Property(e => e.Cdate)
                .HasColumnType("datetime")
                .HasColumnName("cdate");
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EDate)
                .HasColumnType("datetime")
                .HasColumnName("eDate");
            entity.Property(e => e.GroupUserPriority)
                .IsUnicode(false)
                .HasComment("Trường này để lưu nhóm những user được áp dụng trên voucher này")
                .HasColumnName("group_user_priority");
            entity.Property(e => e.IsLimitVoucher).HasColumnName("is_limit_voucher");
            entity.Property(e => e.IsMaxPriceProduct).HasColumnName("is_max_price_product");
            entity.Property(e => e.IsPublic)
                .HasComment("Nêu set true thì hiểu voucher này được public cho các user thanh toán đơn hàng")
                .HasColumnName("is_public");
            entity.Property(e => e.LimitTotalDiscount).HasColumnName("limit_total_discount");
            entity.Property(e => e.LimitUse).HasColumnName("limitUse");
            entity.Property(e => e.MinTotalAmount).HasColumnName("min_total_amount");
            entity.Property(e => e.PriceSales)
                .HasColumnType("money")
                .HasColumnName("price_sales");
            entity.Property(e => e.RuleType)
                .HasComment("Trường này dùng để phân biệt voucher triển khai này chạy theo rule nào. Ví dụ: rule giảm giá với 1 số tiền vnđ trên toàn bộ đơn hàng. Giảm giá 20% phí first pound đầu tiên của nhãn hàng amazon. 1: triển khai rule giảm giá cho toàn bộ đơn hàng. 2 là rule áp dụng cho 20% phí first pound đầu tiên.")
                .HasColumnName("rule_type");
            entity.Property(e => e.StoreApply)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("store_apply");
            entity.Property(e => e.Udate)
                .HasColumnType("datetime")
                .HasColumnName("udate");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<VoucherCampaign>(entity =>
        {
            entity.ToTable("VoucherCampaign");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CampaignVoucher)
                .HasMaxLength(400)
                .HasColumnName("campaign_voucher");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
        });

        modelBuilder.Entity<VoucherLogActivity>(entity =>
        {
            entity.ToTable("voucherLogActivity");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PriceSaleVnd).HasColumnName("price_sale_vnd");
            entity.Property(e => e.Status)
                .HasComment("Trang thai giao dịch voucher. 1: khoa. 0: dang ap dung")
                .HasColumnName("status");
            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.UpdateTime)
                .HasColumnType("datetime")
                .HasColumnName("update_time");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VoucherId).HasColumnName("voucher_id");

            entity.HasOne(d => d.Voucher).WithMany(p => p.VoucherLogActivities)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("FK_voucherLogActivity_Voucher");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ward__C6BD9BCA1EF01D69");

            entity.ToTable("Ward");

            entity.Property(e => e.DistrictId)
                .IsRequired()
                .HasMaxLength(5);
            entity.Property(e => e.Location).HasMaxLength(30);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.NameNonUnicode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(30);
            entity.Property(e => e.WardId)
                .IsRequired()
                .HasMaxLength(5);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
