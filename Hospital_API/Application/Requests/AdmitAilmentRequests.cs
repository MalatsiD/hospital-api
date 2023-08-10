using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class CheckAilmentInAdmitAilmentExistRequest : IRequest<ResponseModelView>
    {
        public int AilmentId { get; set; }
    }
}
