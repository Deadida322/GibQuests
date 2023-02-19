using System.ComponentModel.DataAnnotations.Schema;

namespace CommonDatabase.QuestDatabase.Models.Stages
{
    public class CoordinatesEntity : BaseEntity
    {
        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        [ForeignKey("map_stage_id")]
        public MapStageEntity MapStage { get; set; }
    }
}
