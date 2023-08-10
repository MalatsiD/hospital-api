using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class CheckGenderInPersonExistRequest : IRequest<ResponseModelView>
    {
        public int GenderId { get; set; }
    }

    public class CheckTitleInPersonExistRequest : IRequest<ResponseModelView>
    {
        public int TitleId { get; set; }
    }
}
