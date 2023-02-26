using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CommonInfrastructure.Http.Attributes
{
    public class AddUserInRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //получаем пользователя
            var user = context.HttpContext.User;

            //получаем тип модели тела запроса
            var typeModel = context.ActionArguments.FirstOrDefault().Value;

            //если он авторизирован
            //и модель унаследована от CommonHttpRequest 
            if (user != null && user.Identity.IsAuthenticated &&
                    typeModel is CommonHttpRequest)
            {
                //typeModel.GetType().IsSubclassOf(typeof(CommonHttpRequest))
                //получаем модель
                var model = typeModel as CommonHttpRequest;
                //и в Claim есть имя и идентификатор
                if (user.HasClaim(el => el.Type.Equals(ClaimTypes.Name)))
                {
                    model.RequestUserName = user.FindFirst(ClaimTypes.Name.ToString()).Value;
                }
                if (user.HasClaim(el => el.Type.Equals(ClaimTypes.NameIdentifier)))
                {
                    model.RequestUserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier.ToString()).Value);
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
