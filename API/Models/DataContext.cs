using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace API.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<CountryCode> CountryCodes { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerInvoice> CustomerInvoices { get; set; }
        public virtual DbSet<FeeRate> FeeRates { get; set; }
        public virtual DbSet<FeeTransaction> FeeTransactions { get; set; }
        public virtual DbSet<Industry> Industries { get; set; }
        public virtual DbSet<Machine> Machines { get; set; }
        public virtual DbSet<MachineCapacityEntry> MachineCapacityEntries { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<ManufacturingLocation> ManufacturingLocations { get; set; }
        public virtual DbSet<ManufacturingProcess> ManufacturingProcesses { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderLine> OrderLines { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<SpareCapacity> SpareCapacities { get; set; }
        public virtual DbSet<OrderWithDetails> OrderWithDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-M5RP1E5;Database=ProductionCapacityMarketplace;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address");

                entity.HasIndex(e => e.CountryCode, "UQ_address_country_code")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("address_2");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("country_code");

                entity.Property(e => e.County)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("county");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Fax)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("fax");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Postcode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("postcode");

                entity.Property(e => e.Telephone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("telephone");

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithOne(p => p.Address)
                    .HasForeignKey<Address>(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_address_country_code");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Address)
                    .HasPrincipalKey<Customer>(p => p.BillingAddressId)
                    .HasForeignKey<Address>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_address_customer");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("contact");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");

                entity.Property(e => e.Salutation)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("salutation");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<CountryCode>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("country_code");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.HasIndex(e => e.AddressId, "UQ_customer_address")
                    .IsUnique();

                entity.HasIndex(e => e.BillingAddressId, "UQ_customer_billing_address")
                    .IsUnique();

                //entity.HasIndex(e => e.IndustryId, "UQ_customer_industry")
                //    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.BillingAddressId).HasColumnName("billing_address_id");

                entity.Property(e => e.IndustryId).HasColumnName("industry_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.VatRegistrationNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("vat_registration_no");

                entity.HasOne(d => d.AddressNavigation)
                    .WithOne(p => p.Customer)
                    .HasForeignKey<Customer>(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_customer_address");

                entity.HasOne(d => d.Industry)
                    .WithOne(p => p.Customer)
                    .HasForeignKey<Customer>(d => d.IndustryId)
                    .HasConstraintName("FK_customer_industry");
            });

            modelBuilder.Entity<CustomerInvoice>(entity =>
            {
                entity.HasKey(e => e.InvoiceNo);

                entity.ToTable("customer_invoice");

                entity.HasIndex(e => e.OrderNo, "UQ_customer_invoice_order")
                    .IsUnique();

                entity.Property(e => e.InvoiceNo)
                    .HasColumnName("invoice_no")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");

                entity.Property(e => e.NetAmount)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("net_amount");

                entity.Property(e => e.OrderNo).HasColumnName("order_no");

                entity.Property(e => e.Paid).HasColumnName("paid");

                entity.Property(e => e.PaymentRef)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("payment_ref");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("total_amount");

                entity.Property(e => e.VatAmount)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("vat_amount");

                entity.Property(e => e.VatRegistrationNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("vat_registration_no");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CustomerInvoices)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_customer_invoice_customer");

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.CustomerInvoices)
                    .HasForeignKey(d => d.ManufacturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_customer_invoice_manufacturer");

                entity.HasOne(d => d.OrderNoNavigation)
                    .WithOne(p => p.CustomerInvoice)
                    .HasForeignKey<CustomerInvoice>(d => d.OrderNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_customer_invoice_order");
            });

            modelBuilder.Entity<FeeRate>(entity =>
            {
                entity.ToTable("fee_rate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Percentage)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("percentage");
            });

            modelBuilder.Entity<FeeTransaction>(entity =>
            {
                entity.ToTable("fee_transaction");

                entity.HasIndex(e => e.InvoiceId, "UQ_fee_transaction_customer_invoice")
                    .IsUnique();

                entity.HasIndex(e => e.ManufacturerId, "UQ_fee_transaction_manufacturer")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.FeeAmount)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("fee_amount");

                entity.Property(e => e.FeePercentage)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("fee_percentage");

                entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");

                entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");

                entity.Property(e => e.NetAmount)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("net_amount");

                entity.Property(e => e.Paid).HasColumnName("paid");

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("total_amount");

                entity.Property(e => e.VatAmount)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("vat_amount");

                entity.HasOne(d => d.Invoice)
                    .WithOne(p => p.FeeTransaction)
                    .HasForeignKey<FeeTransaction>(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_fee_transaction_customer_invoice");

                entity.HasOne(d => d.Manufacturer)
                    .WithOne(p => p.FeeTransaction)
                    .HasForeignKey<FeeTransaction>(d => d.ManufacturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_fee_transaction_manufacturer");
            });

            modelBuilder.Entity<Industry>(entity =>
            {
                entity.ToTable("industry");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Machine>(entity =>
            {
                entity.ToTable("machine");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Capacity)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("capacity");

                entity.Property(e => e.CostPerUnit)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("cost_per_unit");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.ManufacturingProcessId).HasColumnName("manufacturing_process_id");

                entity.Property(e => e.ModelNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("model_no");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.SetupTime)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("setup_time");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Machines)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_machine_manufacturing_location");

                entity.HasOne(d => d.ManufacturingProcess)
                    .WithMany(p => p.Machines)
                    .HasForeignKey(d => d.ManufacturingProcessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_machine_manufacturing_process");
            });

            modelBuilder.Entity<MachineCapacityEntry>(entity =>
            {
                entity.HasKey(e => e.EntryNo);

                entity.ToTable("machine_capacity_entry");

                entity.Property(e => e.EntryNo).HasColumnName("entry_no");

                entity.Property(e => e.Capacity)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("capacity");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.MachineId).HasColumnName("machine_id");

                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.HasOne(d => d.Machine)
                    .WithMany(p => p.MachineCapacityEntries)
                    .HasForeignKey(d => d.MachineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_machine_capacity_entry_machine");
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("manufacturer");

                entity.HasIndex(e => e.AddressId, "FK_manufacturer_addressid")
                    .IsUnique();

                entity.HasIndex(e => e.AddressId, "UQ_manufacturer_address")
                    .IsUnique();

                entity.HasIndex(e => e.BillingAddressId, "UQ_manufacturer_billing_address")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.BillingAddressId).HasColumnName("billing_address_id");

                entity.Property(e => e.FeeRateId).HasColumnName("fee_rate_id");

                entity.Property(e => e.IndustryId).HasColumnName("industry_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.VatRegistrationNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("vat_registration_no");

                entity.HasOne(d => d.Address)
                    .WithOne(p => p.ManufacturerAddress)
                    .HasForeignKey<Manufacturer>(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_manufacturer_address");

                entity.HasOne(d => d.BillingAddress)
                    .WithOne(p => p.ManufacturerBillingAddress)
                    .HasForeignKey<Manufacturer>(d => d.BillingAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_manufacturer_billing_address");

                entity.HasOne(d => d.FeeRate)
                    .WithOne(p => p.Manufacturer)
                    .HasForeignKey<Manufacturer>(d => d.FeeRateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_manufacturer_fee_rate");

                entity.HasOne(d => d.Industry)
                    .WithMany(p => p.Manufacturers)
                    .HasForeignKey(d => d.IndustryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_manufacturer_industry");
            });

            modelBuilder.Entity<ManufacturingLocation>(entity =>
            {
                entity.ToTable("manufacturing_location");

                entity.HasIndex(e => e.AddressId, "UQ_manufacturing_location_address")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.HasOne(d => d.Address)
                    .WithOne(p => p.ManufacturingLocation)
                    .HasForeignKey<ManufacturingLocation>(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_manufacturing_location_address");

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.ManufacturingLocations)
                    .HasForeignKey(d => d.ManufacturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_manufacturing_location_manufacturer");
            });

            modelBuilder.Entity<ManufacturingProcess>(entity =>
            {
                entity.ToTable("manufacturing_process");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IndustryId).HasColumnName("industry_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.HasOne(d => d.Industry)
                    .WithMany(p => p.ManufacturingProcesses)
                    .HasForeignKey(d => d.IndustryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_manufacturing_process_industry");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("notification");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateSent)
                    .HasColumnType("datetime")
                    .HasColumnName("date_sent");

                entity.Property(e => e.FromCustomerId).HasColumnName("from_customer_id");

                entity.Property(e => e.MessageContent)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("message_content");

                entity.Property(e => e.RelatesToNotificationId).HasColumnName("relates_to_notification_id");

                entity.Property(e => e.Sequence).HasColumnName("sequence");

                entity.Property(e => e.ToManufacturerId).HasColumnName("to_manufacturer_id");

                entity.HasOne(d => d.FromCustomer)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.FromCustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_notification_customer");

                entity.HasOne(d => d.ToManufacturer)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.ToManufacturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_notification_manufacturer");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderNo);

                entity.ToTable("order");

                entity.Property(e => e.OrderNo).HasColumnName("order_no");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("customer_name");

                entity.Property(e => e.Fulfilled).HasColumnName("fulfilled");

                entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");

                entity.Property(e => e.ManufacturerName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("manufacturer_name");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("order_date");

                entity.Property(e => e.OrderedByName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ordered_by_name");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_customer");

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ManufacturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_manufacturer");
            });

            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.HasKey(e => new { e.OrderNo, e.LineNo });

                entity.ToTable("order_line");

                entity.Property(e => e.OrderNo).HasColumnName("order_no");

                entity.Property(e => e.LineNo).HasColumnName("line_no");

                entity.Property(e => e.Capacity)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("capacity");

                entity.Property(e => e.CapacityEntryNo).HasColumnName("capacity_entry_no");

                entity.Property(e => e.CostPerUnit)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("cost_per_unit");

                entity.Property(e => e.LineAmount)
                    .HasColumnType("decimal(38, 20)")
                    .HasColumnName("line_amount");

                entity.Property(e => e.LocationId).HasColumnName("location_id");

                entity.Property(e => e.MachineId).HasColumnName("machine_id");

                entity.HasOne(d => d.CapacityEntryNoNavigation)
                    .WithOne(p => p.OrderLine)
                    .HasForeignKey<OrderLine>(d => d.CapacityEntryNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_line_capacity_entry");

                entity.HasOne(d => d.Location)
                    .WithOne(p => p.OrderLine)
                    .HasForeignKey<OrderLine>(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_line_location");

                entity.HasOne(d => d.Machine)
                    .WithOne(p => p.OrderLine)
                    .HasForeignKey<OrderLine>(d => d.MachineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_line_machine");

                entity.HasOne(d => d.OrderNoNavigation)
                    .WithMany(p => p.OrderLines)
                    .HasForeignKey(d => d.OrderNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_order_line_order");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .HasColumnName("password_hash");

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .HasColumnName("password_salt");

                entity.Property(e => e.Salutation)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("salutation");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<SpareCapacity>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.IndustryId).HasColumnName("industry_id");
                entity.Property(e => e.IndustryName).HasColumnName("industry_name");
                entity.Property(e => e.ProcessId).HasColumnName("process_id");
                entity.Property(e => e.ProcessName).HasColumnName("process_name");
                entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
                entity.Property(e => e.ManufacturerName).HasColumnName("manufacturer_name");
                entity.Property(e => e.LocationId).HasColumnName("location_id");
                entity.Property(e => e.LocationName).HasColumnName("location_name");
                entity.Property(e => e.City).HasColumnName("city");
                entity.Property(e => e.MachineId).HasColumnName("machine_id");
                entity.Property(e => e.MachineName).HasColumnName("machine_name");
                entity.Property(e => e.ModelNo).HasColumnName("model_no");
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.StartTime).HasColumnName("start_time");
                entity.Property(e => e.Capacity).HasColumnName("capacity");
                entity.Property(e => e.CostPerUnit).HasColumnName("cost_per_unit");
                entity.Property(e => e.CapacityCost).HasColumnName("capacity_cost");
                entity.Property(e => e.CapacityEntryNo).HasColumnName("capacity_entry_no");

                entity.ToView("spare_capacity");
            });

            modelBuilder.Entity<OrderWithDetails>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.OrderNumber).HasColumnName("order_no");
                entity.Property(e => e.CustomerId).HasColumnName("customer_id");
                entity.Property(e => e.CustomerName).HasColumnName("customer_name");
                entity.Property(e => e.OrderDate).HasColumnName("order_date");
                entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
                entity.Property(e => e.ManufacturerName).HasColumnName("manufacturer_name");
                entity.Property(e => e.OrderedByName).HasColumnName("ordered_by_name");
                entity.Property(e => e.Fulfilled).HasColumnName("fulfilled");
                entity.Property(e => e.LineNumber).HasColumnName("line_no");
                entity.Property(e => e.LocationId).HasColumnName("location_id");
                entity.Property(e => e.MachineId).HasColumnName("machine_id");
                entity.Property(e => e.Capacity).HasColumnName("capacity");
                entity.Property(e => e.CostPerUnit).HasColumnName("cost_per_unit");
                entity.Property(e => e.LineAmount).HasColumnName("line_amount");
                entity.Property(e => e.CapacityEntryNo).HasColumnName("capacity_entry_no");
                entity.Property(e => e.LocationName).HasColumnName("machine_location_name");
                entity.Property(e => e.MachineName).HasColumnName("machine_name");
                entity.Property(e => e.ModelNo).HasColumnName("model_no");
                entity.Property(e => e.StartTime).HasColumnName("start_time");

                entity.ToView("order_with_details");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
