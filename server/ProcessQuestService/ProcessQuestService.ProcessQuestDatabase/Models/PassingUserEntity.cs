using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessQuestService.ProcessQuestDatabase.Models
{
    public class PassingUserEntity : BaseEntity
    {
        public int UserId { get; set; }

        public int Stage { get; set; } = 0;

        public bool IsPassed { get; set; } = false;

        public DateTime EndDate { get; set; }

        [ForeignKey("room_id")]
        public RoomEntity Room { get; set; }
    }
}
