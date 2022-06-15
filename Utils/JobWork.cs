using PgJobs.Data;
using PgJobs.Models;
using Serilog;

namespace PgJobs.Utils
{
    public class JobWork : IHostedService, IAsyncDisposable
    {
        private readonly Task _completedTask = Task.CompletedTask;
        // private readonly ILogger<Work> _logger;        
        private Timer _timer;
        protected JobDbContext _context;

        public virtual string Name => "Db Work";

        public JobWork(JobDbContext context) => _context = context;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("{0} is running.", Name);
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return _completedTask;
        }

        private void DoWork(object state)
        {
            var jobs = _context.Jobs.Where(j => j.NextDate < DateTime.Now).ToArray();
            Parallel.ForEach(jobs, job =>
            {
                job.ThisDate = DateTime.Now;
                Log.Information("Start job - {0}", job.Id);
                SaveChanges();
                JobLog log = new JobLog();
                log.Job = job;
                log.Start = DateTime.Now;
                try
                {
                    Npgsql.NpgsqlConnection con = new Npgsql.NpgsqlConnection(JobDbContext.DbConn);
                    con.Open();
                    var com = con.CreateCommand();
                    com.CommandText = job.What;
                    com.ExecuteNonQuery();
                    try
                    {
                        job.LastDate = job.NextDate;
                        com.CommandText = "SELECT " + job.Interval;
                        job.NextDate = Convert.ToDateTime(com.ExecuteScalar());
                    }
                    catch (Exception)
                    {
                        job.NextDate = null;
                    }
                    finally
                    {
                        job.ThisDate = null;
                    }

                }
                catch (Exception ex)
                {
                    log.Error = ex.ToString();
                    job.Failures++;
                    Log.Error("Failures job - {0},{1}", job.Id, ex);
                }
                log.End = DateTime.Now;
                _context.Logs.Add(log);
                SaveChanges();
                Log.Information("End job - {0}", job.Id);
            });
        }

        private void SaveChanges()
        {
            lock (_context)
                _context.SaveChanges();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("{0} is stopping.", Name);

            _timer?.Change(Timeout.Infinite, 0);

            return _completedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_timer is IAsyncDisposable timer)
            {
                await timer.DisposeAsync();
            }

            _timer = null;
        }
    }
}
