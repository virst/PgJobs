using System.ComponentModel.DataAnnotations.Schema;

namespace PgJobs.Models
{
    [Table("jobs_logs")]
    public class JobLog
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("job_id")]
        public int JobId { get; set; }

        [Column("job_id")]
        public Job Job { get; set; }

        [Column("date_beg")]
        public DateTime Start { get; set; }
        [Column("date_end")]
        public DateTime End { get; set; }
        [Column("err_txt")]
        public string Error { get; set; }
    }
}