using CommonInfrastructure.Http;
using GenerateQuestsService.DataContracts.Models.Stages;
using System.ComponentModel.DataAnnotations;

namespace GenerateQuestsService.DataContracts.DataContracts
{
    public class UpdateQuestContract : CommonHttpRequest
    {
        [Required(ErrorMessage = "Id обязательно")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно к заполнению"),
       MaxLength(100, ErrorMessage = "Максимальная длина названия 100 символов")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Описание обязательно к заполнению"),
        MaxLength(250, ErrorMessage = "Максимальная длина описания 250 символов")]
        public string Description { get; set; }

        public string Img { get; set; }

        public IList<Stage> Stages { get; set; }
    }
}
