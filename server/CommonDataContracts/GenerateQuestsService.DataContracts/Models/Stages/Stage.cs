using GenerateQuestsService.DataContracts.Enums;

namespace GenerateQuestsService.DataContracts.Models.Stages
{
    public class Stage
    {
        /// <summary>
        /// internal set - получить в модели id можно,присвоить нельзя
        /// </summary>

        public int Order { get; set; }

        public string Title { get; set; }

        public virtual StageType Type { get; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(obj is not Stage) return false;

            var compareStage = (obj as Stage);
            return compareStage.Title == Title 
                && compareStage.Type == Type 
                && compareStage.Order == Order;
        }
    }
}
