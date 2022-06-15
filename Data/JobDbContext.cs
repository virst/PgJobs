using Microsoft.EntityFrameworkCore;
using PgJobs.Models;
using Serilog;

namespace PgJobs.Data
{
    public class JobDbContext : DbContext
    {
        private static string _dbConn;
#if DEBUG
        static JobDbContext()
        {
            _dbConn = "Server=172.30.2.51;Port=5432;Database=xgate;User Id=etran_loader;Password=alia0w5j;Command Timeout=0";
        }
#endif

        public static string DbConn
        {
            get => _dbConn ?? Environment.GetEnvironmentVariable("DB_CONN");
            set => _dbConn = value;
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobLog> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options
                .UseNpgsql(DbConn);
            if (FirstRun)
                Log.Information("DbConn={0}", DbConn);            
        }

        public JobDbContext()
        {
            if (FirstRun)
                Database.Migrate();
            FirstRun = false;
        }

        public static bool FirstRun { get; private set; } = true;
    }
}
