using System.ComponentModel.DataAnnotations.Schema;

namespace PgJobs.Models
{
    [Table("jobs")]
    public class Job
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("last_date")]
        public DateTime? LastDate { get; set; }

        [Column("this_date")]
        public DateTime? ThisDate { get; set; }

        [Column("next_date")]
        public DateTime? NextDate { get; set; }

        [NotMapped]
        public bool Active
        {
            get
            {
                return NextDate == null;
            }
            set
            {
                if (!value)
                    NextDate = null;
                else
                    NextDate = DateTime.Now;
            }
        }

        [Column("interval")]
        public string Interval { get; set; }

        [Column("failures")]
        public int Failures { get; set; }

        [Column("what")]
        public string What { get; set; }

        public ICollection<JobLog> Logs { get; set; }
    }
}
