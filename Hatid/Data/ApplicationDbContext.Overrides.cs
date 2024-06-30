using Hatid.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using System.Security.Claims;

namespace Hatid.Data
{
    public partial class ApplicationDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void SetGlobalQuery<T>(ModelBuilder builder) where T : BaseEntity
        {
            builder.Entity<T>().HasKey(e => e.Id);
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);

            //classes that has no BaseEntity inheritance
            builder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
        }

        static readonly MethodInfo SetGolbalQueryMethod = typeof(ApplicationDbContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        private static List<Type> _entityTypeCache;
        private static List<Type> GetEntityTypes()
        {
            if (_entityTypeCache != null)
            {
                return _entityTypeCache.ToList();
            }
            _entityTypeCache = (from a in GetReferencingAssemblies()
                                from t in a.DefinedTypes
                                where t.BaseType == typeof(BaseEntity)
                                select t.AsType()).ToList();

            return _entityTypeCache;
        }
        private static List<Assembly> GetReferencingAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;

            foreach (var library in dependencies)
            {
                try
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
                catch (FileNotFoundException)
                { }
            }
            return assemblies;
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries.ToList())
            {
                if (entry.Entity is BaseEntity trackable)
                {
                    int currentUserId = -1;
                    if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                    {
                        var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                        int.TryParse(userId, out currentUserId);
                    }
                   
                    var currentDateTime = DateTime.UtcNow;
                    

                    switch (entry.State)
                    {
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;                           
                            trackable.ModifiedDate = currentDateTime;
                            trackable.ModifiedById = currentUserId;
                            trackable.IsDeleted = true;
                            break;
                        case EntityState.Modified:
                            trackable.ModifiedDate = currentDateTime;
                            trackable.ModifiedById = currentUserId;
                            break;
                        case EntityState.Added:
                            trackable.CreatedDate = currentDateTime;
                            trackable.CreatedById = currentUserId;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.Property(p => p.Id).HasConversion<int>();
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.Property(p => p.Id).HasConversion<int>();
            });            

            foreach (var type in GetEntityTypes())
            {
                var method = SetGolbalQueryMethod.MakeGenericMethod(type);
                method.Invoke(this, new object[] { modelBuilder });
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

